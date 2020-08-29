using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class FeedbackData : DataBase
    {
        public static readonly FeedbackData Instance = new FeedbackData();

        /// <summary>
        /// 2017-02-25张立彬
        /// 添加或修改反馈数据
        /// </summary>
        /// <param name="feedbackData"></param>
        /// <returns></returns>
        public int InserFeedbackData(Entities.FeedbackData feedbackData)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@ADDetailID",SqlDbType.Int),
                    new SqlParameter("@ReadCount",SqlDbType.Int),
                    new SqlParameter("@TransmitCount",SqlDbType.Int),
                    new SqlParameter("@ClickCount",SqlDbType.Int),
                    new SqlParameter("@CommentCount",SqlDbType.Int),
                    new SqlParameter("@LinkCount",SqlDbType.Int),
                    new SqlParameter("@PVCount", SqlDbType.Int),
                    new SqlParameter("@UVCount", SqlDbType.Int),
                    new SqlParameter("@OrderCount",SqlDbType.Int),
                    new SqlParameter("@Value",SqlDbType.Decimal),
                    new SqlParameter("@ClickRate",SqlDbType.Decimal),
                    new SqlParameter("@FilePath",SqlDbType.VarChar,200),
                    new SqlParameter("@FeedbackBeginDate", SqlDbType.Date),
                    new SqlParameter("@FeedbackEndDate", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID",SqlDbType.Int),
                    new SqlParameter("@DeliveredCount",SqlDbType.Int),
                    new SqlParameter("@NowDate",SqlDbType.DateTime),
                    new SqlParameter("@Result",SqlDbType.Int)
                                        };
            parameters[0].Value = feedbackData.MediaType;
            parameters[1].Value = feedbackData.SubOrderID;
            parameters[2].Value = feedbackData.ADDetailID;
            parameters[3].Value = feedbackData.ReadCount;
            parameters[4].Value = feedbackData.TransmitCount;
            parameters[5].Value = feedbackData.ClickCount;
            parameters[6].Value = feedbackData.CommentCount;
            parameters[7].Value = feedbackData.LinkCount;
            parameters[8].Value = feedbackData.PVCount;
            parameters[9].Value = feedbackData.UVCount;
            parameters[10].Value = feedbackData.OrderCount;
            parameters[11].Value = feedbackData.Value;
            parameters[12].Value = feedbackData.ClickRate;
            parameters[13].Value = feedbackData.FilePath;
            parameters[14].Value = Convert.ToDateTime(feedbackData.FeedbackBeginDate).Date;
            parameters[15].Value = Convert.ToDateTime(feedbackData.FeedbackEndDate);
            parameters[16].Value = feedbackData.CreateUserID;
            parameters[17].Value = feedbackData.DeliveredCount;
            parameters[18].Value = DateTime.Now;
            parameters[19].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_InserFeedbackData", parameters);
            return Convert.ToInt32(parameters[19].Value);

        }

        /// <summary>
        /// 2017-02-27张立彬
        /// 根据子单编号和媒体类型查询反馈数据
        /// </summary>
        /// <param name="subOrderID">子单编号</param>
        /// <param name="mediaType">媒体类型</param>
        /// <returns></returns>
        public DataTable SelectFeedbackData(string subOrderID, int mediaType, string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@subOrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@mediaType", SqlDbType.Int),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,300),
                    };
            parameters[0].Value = subOrderID;
            parameters[1].Value = mediaType;
            parameters[2].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectFeedbackData", parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 2017-03-10 张立彬
        /// 删除反馈数据
        /// </summary>
        /// <param name="FeedbackID">反馈ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns></returns>
        public int DeleteFeedbackData(int MediaType, int FeedbackID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@FeedbackID", SqlDbType.Int),
                     new SqlParameter("@Result", SqlDbType.Int),
                    };
            parameters[0].Value = MediaType;
            parameters[1].Value = FeedbackID;
            parameters[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_DeleteFeedBackDataByID", parameters);
            return Convert.ToInt32(parameters[2].Value);
        }
        /// <summary>
        /// 2017-06-15 张立彬
        /// 根据子订单号查询大于dtStart时间的订单数量
        /// </summary>
        /// <param name="subOrderID">子订单号</param>
        /// <param name="dtStart">时间</param>
        /// <returns></returns>
        public int SelectGreaterThanSomeDay(string subOrderID, DateTime dtStart)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@subOrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@dtStart", SqlDbType.DateTime)
                    };
            parameters[0].Value = subOrderID;
            parameters[1].Value = dtStart;
            string strSql = "Select count(1) from ADScheduleInfo where SubOrderID=@subOrderID and BeginData<=@dtStart";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);

        }
        /// <summary>
        /// zlb 2017-07-25
        /// 批量增加反馈文件
        /// </summary>
        /// <param name="FilePathList">反馈文件集合</param>
        /// <param name="mt">媒体类型</param>
        /// <param name="FeedBackID">反馈ID</param>
        /// <returns></returns>
        public int InsertFeedBackFile(List<string> FilePathList, MediaType mt, int FeedBackID)
        {
            string strSql = "";
            for (int i = 0; i < FilePathList.Count; i++)
            {

                strSql += string.Format("insert into OrderFeedbackData_File VALUES ({0},{1},'{2}');", (int)mt, FeedBackID, FilePathList[i]);
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// <summary>
        /// zlb 2017-07-25
        /// 查询反馈文件
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="FeedBackIDList">反馈ID集合字符串</param>
        /// <returns></returns>
        public DataTable SelectFeedBackFile(int MediaType, string FeedBackIDList)
        {
            string strSql = string.Format("SELECT FeedBackID,UploadFileURL FROM OrderFeedbackData_File WHERE MediaType={0} AND FeedBackID IN ({1}) ", MediaType, FeedBackIDList);

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-07-27
        /// 删除反馈数据的图片信息
        /// </summary>
        /// <param name="FeedBackID"></param>
        /// <returns></returns>
        public int DeleteFeedBackFileByID(int FeedBackID)
        {
            string strSql = "DELETE FROM OrderFeedbackData_File WHERE FeedBackID=" + FeedBackID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
    }
}
