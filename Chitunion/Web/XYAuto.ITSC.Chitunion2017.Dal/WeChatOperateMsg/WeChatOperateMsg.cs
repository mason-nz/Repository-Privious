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
    public class WeChatOperateMsg : DataBase
    {
        public const string P_WeChatOperateMsg_SELECT = "p_WeChatOperateMsg_Select";

        #region Instance
        public static readonly WeChatOperateMsg Instance = new WeChatOperateMsg();
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Insert(Entities.WeChatOperateMsg model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID",SqlDbType.Int,4),
                    new SqlParameter("@MediaName",SqlDbType.VarChar,200),
                    new SqlParameter("@PublishID",SqlDbType.Int,4),
                    new SqlParameter("@PublishName",SqlDbType.VarChar,200),
                    new SqlParameter("@OptType", SqlDbType.Int,4),
                    new SqlParameter("@SubmitUserName",SqlDbType.VarChar,200),
                    new SqlParameter("@SubmitUserID",SqlDbType.Int,4),
                    new SqlParameter("@MsgType",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.MediaType;
            parameters[1].Value = model.MediaID;
            parameters[2].Value = model.MediaName;
            parameters[3].Value = model.PublishID;
            parameters[4].Value = model.PublishName;
            parameters[5].Value = model.OptType;
            parameters[6].Value = model.SubmitUserName;
            parameters[7].Value = model.SubmitUserID;
            parameters[8].Value = model.MsgType;
            parameters[9].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_WeChatOperateMsg_InsertV1_1", parameters);
        }
        #endregion

        #region Select
        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.WeChatOperateMsg GetModel(string recid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,OrderID,SubOrderID,OptType,OrderStatus,RejectMsg,CreateTime,CreateUserID");
            strSql.Append(" FROM WeChatOperateMsg ");
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
        public Entities.WeChatOperateMsg DataRowToModel(DataRow row)
        {
            Entities.WeChatOperateMsg model = new Entities.WeChatOperateMsg();
            if (row != null)
            {
                if (row["RecId"] != null && row["RecId"].ToString() != "")
                {
                    model.RecID = int.Parse(row["RecId"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["MediaType"] != null && row["MediaType"].ToString() != "")
                {
                    model.MediaType = int.Parse(row["MediaType"].ToString());
                }
                if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                {
                    model.MediaID = int.Parse(row["MediaID"].ToString());
                }

                if (row["MediaName"] != null && row["MediaName"].ToString() != "")
                {
                    model.MediaName = row["MediaName"].ToString();
                }
                if (row["PublishID"] != null && row["PublishID"].ToString() != "")
                {
                    model.PublishID = int.Parse(row["PublishID"].ToString());
                }
                if (row["PublishName"] != null && row["PublishName"].ToString() != "")
                {
                    model.PublishName = row["PublishName"].ToString();
                }
                if (row["OptType"] != null && row["OptType"].ToString() != "")
                {
                    model.OptType = int.Parse(row["OptType"].ToString());
                }
                if (row["SubmitUserName"] != null && row["SubmitUserName"].ToString() != "")
                {
                    model.SubmitUserName = row["SubmitUserName"].ToString();
                }
                if (row["SubmitUserID"] != null && row["SubmitUserID"].ToString() != "")
                {
                    model.SubmitUserID = int.Parse(row["SubmitUserID"].ToString());
                }
                if (row["MsgType"] != null && row["MsgType"].ToString() != "")
                {
                    model.MsgType = int.Parse(row["MsgType"].ToString());
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
        public DataTable GetWeChatOperateMsg(int userid, int currentPage, int pageSize, out int totalCount)
        {           
            SqlParameter[] parameters = {
					new SqlParameter("@userid", SqlDbType.Int, 4),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = userid;
            parameters[1].Value = pageSize;
            parameters[2].Value = currentPage;
            parameters[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_WeChatOperateMsg_SelectV1_1", parameters);
            totalCount = (int)(parameters[3].Value);
            return ds.Tables[0];
        }

        #endregion

        #region 更新或查询消息数量V1.1
        public string p_WeChatOperateMsg_UpdateReadV1_1(int OptType, int userid, out int totalCount)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OptType", SqlDbType.Int, 4),
                    new SqlParameter("@userid", SqlDbType.Int, 4),
                    new SqlParameter("@Msg", SqlDbType.VarChar, 200),
                    new SqlParameter("@TotalCount", SqlDbType.Int, 4)
                    };
            parameters[0].Value = OptType;
            parameters[1].Value = userid;
            parameters[2].Direction = ParameterDirection.Output;
            parameters[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_WeChatOperateMsg_UpdateReadV1_1", parameters);
            totalCount = (int)(parameters[3].Value);
            return (string)(parameters[2].Value);
        }
        #endregion
        #region 查询媒体是否已有通用模板
        public bool HasPulblicTemplate(int mediaID)
        {
            string sqlstr = @"SELECT  COUNT(1)
                            FROM dbo.App_AdTemplate
                            WHERE   AuditStatus = 48002
                                    AND BaseMediaID = (SELECT  BaseMediaID
                                                       FROM    dbo.Media_PCAPP
                                                       WHERE   MediaID = @MediaID
                                                      )";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@MediaID",mediaID)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr,parameters) > 0 ? true : false;
        }
        #endregion
    }
}
