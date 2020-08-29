using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class PublishAuditInfo : DataBase
    {
        public const string P_PublishAuditInfo_SELECT = "p_PublishAuditInfo_Select";

        #region Instance

        public static readonly PublishAuditInfo Instance = new PublishAuditInfo();

        #endregion Instance

        #region 修改刊例状态

        /// <summary>
        /// 修改刊例状态
        /// </summary>
        /// <param name="pubid">刊例ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public int UpdateStatusByPubID_PublishBasic(int pubid, int status)
        {
            string sqlstr = "";
            if (status == 15003)//审核通过
            {
                sqlstr = string.Format("UPDATE dbo.Publish_BasicInfo SET Status={0},PublishStatus=15005 WHERE PubID={1};UPDATE dbo.Publish_DetailInfo SET PublishStatus=15005 WHERE PubID={1};", status, pubid);
            }
            else
            {
                sqlstr = string.Format("UPDATE dbo.Publish_BasicInfo SET Status={0} WHERE PubID={1}", status, pubid);
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }

        #endregion 修改刊例状态

        #region 根据刊例ID获取状态

        /// <summary>
        /// 根据刊例ID获取状态
        /// </summary>
        /// <param name="pubid"></param>
        /// <returns></returns>
        public int GetStatus_PublishID(int pubid)
        {
            int retval = 0;
            string sqlstr = string.Format("SELECT Status FROM dbo.Publish_BasicInfo WHERE PubID={0}", pubid);
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr).Tables[0];

            if (dt != null && Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                retval = Convert.ToInt32(dt.Rows[0][0]);
            }
            return retval;
        }

        #endregion 根据刊例ID获取状态

        #region Select

        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.PublishAuditInfo GetModel(string recid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,MediaType,PublishID,OptType,PubStatus,RejectMsg,CreateTime,CreateUserID");
            strSql.Append(" FROM PublishAuditInfo ");
            strSql.Append(" WHERE RecID=@RecID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4)
            };
            parameters[0].Value = recid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.PublishAuditInfo DataRowToModel(DataRow row)
        {
            Entities.PublishAuditInfo model = new Entities.PublishAuditInfo();
            if (row != null)
            {
                if (row["RecId"] != null && row["RecId"].ToString() != "")
                {
                    model.RecID = int.Parse(row["RecId"].ToString());
                }
                if (row["MediaType"] != null && row["MediaType"].ToString() != "")
                {
                    model.MediaType = int.Parse(row["MediaType"].ToString());
                }

                if (row["PublishID"] != null && row["PublishID"].ToString() != "")
                {
                    model.PublishID = int.Parse(row["PublishID"].ToString());
                }
                if (row["OptType"] != null && row["OptType"].ToString() != "")
                {
                    model.OptType = int.Parse(row["OptType"].ToString());
                }
                if (row["PubStatus"] != null && row["PubStatus"].ToString() != "")
                {
                    model.PubStatus = int.Parse(row["PubStatus"].ToString());
                }
                if (row["RejectMsg"] != null && row["RejectMsg"].ToString() != "")
                {
                    model.RejectMsg = row["RejectMsg"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
                }
            }
            return model;
        }

        #region 根据订单号查询得到一个对象实体

        public Entities.PublishAuditInfo GetPublishAuditInfo(int recid)
        {
            QueryPublishAuditInfo query = new QueryPublishAuditInfo();
            query.PublishID = recid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetPublishAuditInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSinglePublishAuditInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        private Entities.PublishAuditInfo LoadSinglePublishAuditInfo(DataRow row)
        {
            Entities.PublishAuditInfo model = new Entities.PublishAuditInfo();
            model.RecID = int.Parse(row["RecID"].ToString());

            if (row["MediaType"] != null && row["MediaType"].ToString() != "")
            {
                model.MediaType = int.Parse(row["MediaType"].ToString());
            }

            if (row["PublishID"] != null && row["PublishID"].ToString() != "")
            {
                model.PublishID = int.Parse(row["PublishID"].ToString());
            }
            if (row["OptType"] != null && row["OptType"].ToString() != "")
            {
                model.OptType = int.Parse(row["OptType"].ToString());
            }
            if (row["PubStatus"] != null && row["PubStatus"].ToString() != "")
            {
                model.PubStatus = int.Parse(row["PubStatus"].ToString());
            }
            if (row["RejectMsg"] != null && row["RejectMsg"].ToString() != "")
            {
                model.RejectMsg = row["RejectMsg"].ToString();
            }
            if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }

            return model;
        }

        #endregion 根据订单号查询得到一个对象实体

        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetPublishAuditInfo(QueryPublishAuditInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.PublishID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.PublishID = " + query.PublishID;
            }
            if (query.MediaType != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.MediaType = " + query.MediaType;
            }
            if (query.OptType != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.OptType = " + query.OptType;
            }

            DateTime beginTime;
            if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            {
                where += " and a.CreateTime>='" + beginTime + "'";
            }
            DateTime endTime;
            if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            {
                where += " and a.CreateTime<'" + endTime.AddDays(1) + "'";
            }

            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@order", SqlDbType.NVarChar, 200),
                    new SqlParameter("@pagesize", SqlDbType.Int, 4),
                    new SqlParameter("@indexpage", SqlDbType.Int, 4),
                    new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
                    };
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PublishAuditInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion Select

        #region Insert

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Entities.PublishAuditInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@PublishID",SqlDbType.Int,4),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@PubStatus", SqlDbType.Int,4),
                    new SqlParameter("@RejectMsg",SqlDbType.Text),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.MediaType;
            parameters[1].Value = model.PublishID;
            parameters[2].Value = model.OptType;
            parameters[3].Value = model.PubStatus;
            parameters[4].Value = model.RejectMsg;
            parameters[5].Value = model.CreateUserID;

            int retval = 0;
            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PublishAuditInfo_Insert", parameters);
            return retval;
        }

        #endregion Insert

        #region Update

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.PublishAuditInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@PublishID",SqlDbType.Int,4),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@PubStatus", SqlDbType.Int,4),
                    new SqlParameter("@RejectMsg",SqlDbType.VarChar,200),
                                        };
            parameters[0].Value = model.MediaType;
            parameters[1].Value = model.PublishID;
            parameters[2].Value = model.OptType;
            parameters[3].Value = model.PubStatus;
            parameters[4].Value = model.RejectMsg;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PublishAuditInfo_Update", parameters);
            return (int)parameters[0].Value;
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int PublishID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PublishID", SqlDbType.Int,4)};
            parameters[0].Value = PublishID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PublishAuditInfo_Delete", parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int PublishID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PublishID", SqlDbType.VarChar,50)};
            parameters[0].Value = PublishID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "p_PublishAuditInfo_Delete", parameters);
        }

        #endregion Delete
    }
}