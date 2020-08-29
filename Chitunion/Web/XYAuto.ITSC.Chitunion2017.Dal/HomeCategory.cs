using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.Utils.Data;
using System.Data.SqlClient;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
namespace XYAuto.ITSC.Chitunion2017.Dal
{

    public class HomeCategory : DataBase
    {
        public static readonly HomeCategory Instance = new HomeCategory();
        /// <summary>
        /// 2017-03-15 张立彬
        /// 查询不同媒体的行业统计
        /// </summary>
        public DataSet SelectHomeCategoryStatistics(int TopCount)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TopCount", SqlDbType.Int)
                    };
            parameters[0].Value = TopCount;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectCategorySum", parameters);
            return ds;
        }
        /// <summary>
        /// 2017-03-16 张立彬
        /// 查询不同媒体信息
        /// </summary>
        /// <param name="MediaTypeID">媒体类型</param>
        /// <param name="CategoryID">行业分类ID</param>
        /// <param name="TopCount">查询行数</param>
        public DataTable SelectHomeMediaInfo(int MediaTypeID, int CategoryID, int TopCount)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaTypeID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@TopCount", SqlDbType.Int)
                    };
            parameters[0].Value = MediaTypeID;
            parameters[1].Value = CategoryID;
            parameters[2].Value = TopCount;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectMeidaByMediaTypeIDAndCategoryID", parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 2017-04-12 张立彬
        /// 删除首页的行业分类
        /// </summary>
        /// <returns></returns>
        public int ClearHomeCategory(int mediaType)
        {
            string strSql = "delete from Home_Category where MediaType=" + mediaType;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// 2017-04-12 张立彬
        /// 添加首页的行业分类
        /// </summary>
        /// <param name="listHomeCategory"></param>
        /// <returns></returns>
        public void InsertHomeCategory(List<HomeCategoryModle> listHomeCategory)
        {
            SqlBulkCopyInsertHomeCategory(listHomeCategory);
        }
        /// <summary>
        /// 使用SqlBulkCopy方式插入数据
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private void SqlBulkCopyInsertHomeCategory(List<HomeCategoryModle> listHomeCategory)
        {
            DataTable dataTable = GetTableSchemaHomeCategory();
            string passportKey;
            int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            for (int i = 0; i < listHomeCategory.Count; i++)
            {
                passportKey = Guid.NewGuid().ToString();
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = i + 1;
                dataRow[1] = listHomeCategory[i].CategoryID;
                dataRow[2] = listHomeCategory[i].CategoryName;
                dataRow[3] = listHomeCategory[i].MediaType;
                dataRow[4] = 0;
                dataRow[5] = userID;
                dataRow[6] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dataTable.Rows.Add(dataRow);
            }

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(CONNECTIONSTRINGS);
            sqlBulkCopy.DestinationTableName = "Home_Category";
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            SqlConnection sqlConnection = new SqlConnection(CONNECTIONSTRINGS);
            sqlConnection.Open();
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                sqlBulkCopy.WriteToServer(dataTable);
            }
            sqlBulkCopy.Close();
            sqlConnection.Close();
        }
        private DataTable GetTableSchemaHomeCategory()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("RecID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CategoryID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CategoryName") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("MediaType") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("PublishState") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CreateUserId") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CreateTime") });

            return dataTable;
        }
        /// <summary>
        /// 2017-04-12 张立彬
        /// 查询对应媒体下已选行业分类
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="IsPublish">是否只查询查发布的(1：只查询发布的,其他全部)</param>
        /// <returns></returns>
        public DataTable SelectSelectedCategory(int MediaType, int IsPublish)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@IsPublish", SqlDbType.Int),
                    };
            parameters[0].Value = MediaType;
            parameters[1].Value = IsPublish;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectHomeCategoryInfo", parameters);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 2017-04-13 张立彬
        /// 查询首页媒体预览
        /// </summary>
        /// <param name="MediaTypeID"></param>
        /// <param name="CategoryID"></param>
        /// <param name="TopCount"></param>
        /// <param name="PublishState"></param>
        /// <returns></returns>
        public DataTable SelectHomeMediaPreview(int MediaTypeID, int CategoryID, int TopCount, int PublishState)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaTypeID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@TopCount", SqlDbType.Int),
                    new SqlParameter("@PublishState", SqlDbType.Int)
                    };
            parameters[0].Value = MediaTypeID;
            parameters[1].Value = CategoryID;
            parameters[2].Value = TopCount;
            parameters[3].Value = PublishState;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectHomeMediaPreview", parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 查询产品分类
        /// </summary>
        /// <param name="PublishState"></param>
        /// <returns></returns>
        public DataTable SelectAllHomeCategory(int PublishState)
        {
            string sqlStr = "select MediaType,CategoryID,CategoryName, (select count(1) from Home_Media m where m.MediaType= C.MediaType and m.CategoryID=C.CategoryID)  CategoryCount from Home_Category C ";
            if (PublishState == 1)
            {
                sqlStr += " where C.PublishState = " + PublishState;
            }

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];
        }
        /// <summary>
        /// Auth:lixiong
        /// 发布
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int UpdatePublishState(int mediaType, HomePublishStateEnum state)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"	UPDATE DBO.Home_Category
	                            SET PublishState = @PublishState
	                            WHERE MediaType = MediaType");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",mediaType),
                        new SqlParameter("@PublishState",(int)state)
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }
        //public DataTable SelectPreviewCategory(int MediaTypeID)
        //{

        //    if (MediaTypeID == 14001)
        //    {

        //    }

        //    string TableName = "";
        //    switch (MediaTypeID)
        //    {
        //        case 14003:
        //            TableName = "Media_Weibo";
        //            break;
        //        case 14004:
        //            TableName = "Media_Video";
        //            break;
        //        case 14005:
        //            TableName = "Media_Broadcast";
        //            break;
        //        default:
        //            TableName = "Media_PCAPP";
        //            break;
        //    }
        //    string strSql = "select DictId,DictName from DictInfo  where DictId in (select distinct CategoryID from " + TableName + " where Status=0)";
        //    return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        //}



    }
}
