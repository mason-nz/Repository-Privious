using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类UserMessage。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:04 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserMessage : DataBase
    {
        #region Instance
        public static readonly UserMessage Instance = new UserMessage();
        #endregion

        #region const
        private const string P_USERMESSAGE_SELECT = "p_UserMessage_Select";
        private const string P_USERMESSAGE_INSERT = "p_UserMessage_Insert";
        private const string P_USERMESSAGE_UPDATE = "p_UserMessage_Update";
        private const string P_USERMESSAGE_DELETE = "p_UserMessage_Delete";
        #endregion

        #region Contructor
        protected UserMessage()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 根据客服id获取此客服管辖的城市群id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetCityGroupIDByUserID(string userid)
        {
            string sql = string.Format(@"SELECT ISNULL(STUFF((SELECT    ',''' + RTRIM(CityGroupID) +''''
                                FROM dbo.CityGroupAgent 
								WHERE UserID={0} OR UserID IN (
								SELECT c.UserID
								FROM  CC2012.dbo.UserGroupDataRigth a INNER JOIN CC2012.dbo.BusinessGroup b ON a.BGID = b.BGID
								LEFT JOIN  CC2012.dbo.EmployeeAgent AS c ON a.BGID=c.BGID
								WHERE b.Status=0 AND a.UserID={0}
								)
                                FOR
                                  XML PATH('')
                                ), 1, 1, ''), '')", StringHelper.SqlFilter(userid));
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 按照查询条件查询留言信息——供后台“在线留言”查询数据使用
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetUserMessage(QueryUserMessage query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string strwhere = " ";
            strwhere += " and uvl.SourceType in (select RecID from V_UserBussinessLine where BGID in(select  BGID from V_UserGroupDataRigth where userid=" + query.UserID + " union  select BGID from v_AgentInfo where userid=" + query.UserID + ") )";
            if (query.RecID != Constant.INT_INVALID_VALUE && query.LastModifyUserID != -1)
            {
                strwhere += " AND um.RecID=" + query.RecID;
            }
            if (query.UsertName != Constant.STRING_INVALID_VALUE)
            {
                strwhere += " AND uvl.UserName like '%" + StringHelper.SqlFilter(query.UsertName) + "%'";
            }
            if (query.LastModifyUserName != Constant.STRING_INVALID_VALUE)
            {
                strwhere += " AND UserInfo.TrueName like '%" + StringHelper.SqlFilter(query.LastModifyUserName) + "%'"; //通过存储过程决定 操作人的字段名
            }
            if (query.TypeID != Constant.INT_INVALID_VALUE && query.TypeID != -1)
            {
                strwhere += " AND  um.TypeID='" + query.TypeID + "'";
            }
            if (query.SourceType != Constant.INT_INVALID_VALUE && query.SourceType != -1)
            {
                strwhere += " AND  uvl.SourceType='" + query.SourceType + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE && query.Status != -1)
            {
                strwhere += " AND  um.Status='" + query.Status + "'";
            }
            if (query.QueryStarttime != Constant.STRING_INVALID_VALUE)
            {
                strwhere += " AND  um.CreateTime>='" + StringHelper.SqlFilter(query.QueryStarttime) + "'";
            }
            if (query.QueryEndTime != Constant.STRING_INVALID_VALUE)
            {
                strwhere += " AND  um.CreateTime<'" + StringHelper.SqlFilter(query.QueryEndTime) + "'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};


            parameters[0].Value = strwhere;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserMessage_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        ///// <summary>
        ///// 得到一个对象实体
        ///// </summary>
        //public Entities.UserMessage GetUserMessage(int RecID)
        //{
        //    QueryUserMessage query = new QueryUserMessage();
        //    query.RecID = RecID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetUserMessage(query, string.Empty, 1, 1, out count);
        //    if (count > 0)
        //    {
        //        return LoadSingleUserMessage(dt.Rows[0]);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        private Entities.UserMessage LoadSingleUserMessage(DataRow row)
        {
            Entities.UserMessage model = new Entities.UserMessage();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.VisitID = row["VisitID"].ToString();
            if (row["TypeID"].ToString() != "")
            {
                model.TypeID = int.Parse(row["TypeID"].ToString());
            }
            model.Content = row["Content"].ToString();
            model.UserName = row["UserName"].ToString();
            model.Email = row["Email"].ToString();
            model.Phone = row["Phone"].ToString();
            model.OrderID = row["OrderID"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            model.Remarks = row["Remarks"].ToString();
            if (row["RemarksTime"].ToString() != "")
            {
                model.RemarksTime = DateTime.Parse(row["RemarksTime"].ToString());
            }
            if (row["RemarkUserID"].ToString() != "")
            {
                model.RemarkUserID = int.Parse(row["RemarkUserID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.UserMessage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@TypeID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@UserName", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,20),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Remarks", SqlDbType.NVarChar,1000),
					new SqlParameter("@RemarksTime", SqlDbType.DateTime),
					new SqlParameter("@RemarkUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.VisitID;
            parameters[2].Value = model.TypeID;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.Phone;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.Remarks;
            parameters[10].Value = model.RemarksTime;
            parameters[11].Value = model.RemarkUserID;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.LastModifyTime;
            parameters[14].Value = model.LastModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserMessage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@TypeID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@UserName", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,20),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Remarks", SqlDbType.NVarChar,1000),
					new SqlParameter("@RemarksTime", SqlDbType.DateTime),
					new SqlParameter("@RemarkUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.VisitID;
            parameters[2].Value = model.TypeID;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.Phone;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.Remarks;
            parameters[10].Value = model.RemarksTime;
            parameters[11].Value = model.RemarkUserID;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.LastModifyTime;
            parameters[14].Value = model.LastModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERMESSAGE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.UserMessage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@TypeID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@UserName", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,20),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@Remarks", SqlDbType.NVarChar,1000),
					new SqlParameter("@RemarksTime", SqlDbType.DateTime),
					new SqlParameter("@RemarkUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.VisitID;
            parameters[2].Value = model.TypeID;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.Phone;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.Remarks;
            parameters[9].Value = model.RemarksTime;
            parameters[10].Value = model.RemarkUserID;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.LastModifyTime;
            parameters[13].Value = model.LastModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UserMessage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@TypeID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@UserName", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,20),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Remarks", SqlDbType.NVarChar,1000),
					new SqlParameter("@RemarksTime", SqlDbType.DateTime),
					new SqlParameter("@RemarkUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.VisitID;
            parameters[2].Value = model.TypeID;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.Phone;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.Remarks;
            parameters[10].Value = model.RemarksTime;
            parameters[11].Value = model.RemarkUserID;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.LastModifyTime;
            parameters[14].Value = model.LastModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERMESSAGE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERMESSAGE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERMESSAGE_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 用于工单层添加工单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertUserMessage(Entities.UserMessage model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@TypeID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@UserName", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,20),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)
             };

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.VisitID;
            parameters[2].Value = model.TypeID;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.Phone;
            parameters[7].Value = model.CreateTime;


            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserMessage_Insert", parameters);
            return (int)parameters[0].Value;
        }

        /// <summary>
        /// 用于更新留言信息的备注信息、工单、状态
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int UpdateUserMessageInfoByRecID(QueryUserMessage query)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update usermessage set ");

            if (query.Remarks != Constant.STRING_INVALID_VALUE)
            {
                strSql.Append(" Remarks='" + StringHelper.SqlFilter(query.Remarks) + "' ,");
            }
            if (query.RemarksTime != Constant.DATE_INVALID_VALUE)
            {
                strSql.Append(" RemarksTime='" + query.RemarksTime + "' ,");
            }
            if (query.RemarkUserID != Constant.INT_INVALID_VALUE)
            {
                strSql.Append(" RemarkUserID='" + query.RemarkUserID + "' ,");
            }

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                strSql.Append(" OrderID='" + StringHelper.SqlFilter(query.OrderID) + "' ,");
            }
            if (query.LastModifyTime != Constant.DATE_INVALID_VALUE)
            {
                strSql.Append(" LastModifyTime='" + query.LastModifyTime + "' ,");
            }
            if (query.LastModifyUserID != Constant.INT_INVALID_VALUE)
            {
                strSql.Append(" LastModifyUserID='" + query.LastModifyUserID + "' ,");
            }

            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                strSql.Append(" Status='" + query.Status + "' ,");
            }

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString().Substring(0, strSql.ToString().Length - 2) + " where RecID=" + query.RecID);
        }
        /// <summary>
        /// 根据登录人的id获取登录人的姓名
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string GetUserNameByUserID(int UserID)
        {
            String strSql = "SELECT TrueName FROM v_AgentInfo WHERE UserID=@UserID";
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = UserID;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);

            string objNum;
            if (obj != null)
            {
                objNum = obj.ToString();
            }
            else
            {
                objNum = "";
            }
            return objNum;
        }

        /// <summary>
        /// 得到一个留言信息对象实体
        /// </summary>
        public BitAuto.DSC.IM_2015.Entities.UserMessage GetModel(int RecID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RecID,VisitID,TypeID,Content,UserName,Email,Phone,OrderID,CreateTime,Remarks,RemarksTime,RemarkUserID,Status,LastModifyTime,LastModifyUserID from UserMessage ");
            strSql.Append(" where RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)
			};
            parameters[0].Value = RecID;

            // BitAuto.DSC.IM_2015.Entities.UserMessage model = new BitAuto.DSC.IM_2015.Entities.UserMessage();
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
        /// 将得到的单个留言信息对象数据行转化成实体——供GetModel(int RecID)方法使用
        /// </summary>
        public BitAuto.DSC.IM_2015.Entities.UserMessage DataRowToModel(DataRow row)
        {
            BitAuto.DSC.IM_2015.Entities.UserMessage model = new BitAuto.DSC.IM_2015.Entities.UserMessage();
            if (row != null)
            {
                if (row["RecID"] != null && row["RecID"].ToString() != "")
                {
                    model.RecID = int.Parse(row["RecID"].ToString());
                }
                if (row["VisitID"] != null)
                {
                    model.VisitID = row["VisitID"].ToString();
                }
                if (row["TypeID"] != null && row["TypeID"].ToString() != "")
                {
                    model.TypeID = int.Parse(row["TypeID"].ToString());
                }
                if (row["Content"] != null)
                {
                    model.Content = row["Content"].ToString();
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["Email"] != null)
                {
                    model.Email = row["Email"].ToString();
                }
                if (row["Phone"] != null)
                {
                    model.Phone = row["Phone"].ToString();
                }
                if (row["OrderID"] != null)
                {
                    model.OrderID = row["OrderID"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["Remarks"] != null)
                {
                    model.Remarks = row["Remarks"].ToString();
                }
                if (row["RemarksTime"] != null && row["RemarksTime"].ToString() != "")
                {
                    model.RemarksTime = DateTime.Parse(row["RemarksTime"].ToString());
                }
                if (row["RemarkUserID"] != null && row["RemarkUserID"].ToString() != "")
                {
                    model.RemarkUserID = int.Parse(row["RemarkUserID"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["LastModifyTime"] != null && row["LastModifyTime"].ToString() != "")
                {
                    model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
                }
                if (row["LastModifyUserID"] != null && row["LastModifyUserID"].ToString() != "")
                {
                    model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
                }
            }
            return model;
        }

    }
}

