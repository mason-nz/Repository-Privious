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
    /// 数据访问类EPVisitLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:02 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserVisitLog : DataBase
    {
        public static readonly UserVisitLog Instance = new UserVisitLog();
        private const string P_EPVISITLOG_SELECT = "p_UserVisitLog_Select";
        protected UserVisitLog()
        { }

        /// 按照查询条件查询
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetUserVisitLog(QueryUserVisitLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.VisitID != Constant.INT_INVALID_VALUE)
            {
                where += " and visitid=" + query.VisitID;
            }
            DataSet ds;


            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EPVISITLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #region GetModel

        /// <summary>
        /// 根据访问id串，返回访问记录实体list
        /// </summary>
        /// <param name="visitids"></param>
        /// <returns></returns>
        public List<Entities.UserVisitLog> GetUserVisitLogListByVisitIDS(string visitids)
        {
            List<Entities.UserVisitLog> visitlist = new List<Entities.UserVisitLog>();
            string sqlstr = "select * from dbo.UserVisitLog where visitid in (" + visitids + ")";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Entities.UserVisitLog model = LoadSingleUserVisitLog(dt.Rows[i]);
                    if (model != null)
                    {
                        visitlist.Add(model);
                    }
                }
            }
            return visitlist;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserVisitLog GetUserVisitLog(int VisitID)
        {
            QueryUserVisitLog query = new QueryUserVisitLog();
            query.VisitID = VisitID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserVisitLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleUserVisitLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.UserVisitLog LoadSingleUserVisitLog(DataRow row)
        {
            Entities.UserVisitLog model = new Entities.UserVisitLog();

            if (row["VisitID"].ToString() != "")
            {
                model.VisitID = int.Parse(row["VisitID"].ToString());
            }
            if (row["SourceType"].ToString() != "")
            {
                model.SourceType = row["SourceType"].ToString();
            }
            if (row["LoginID"].ToString() != "")
            {
                model.LoginID = row["LoginID"].ToString();
            }
            if (row["CustID"].ToString() != "")
            {
                model.CustID = row["CustID"].ToString();
            }
            model.UserName = row["UserName"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreatTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["UpdateTime"].ToString() != "")
            {
                model.UpdateTime = DateTime.Parse(row["UpdateTime"].ToString());
            }
            if (row["queuefailtime"].ToString() != "")
            {
                model.QueuefailTime = DateTime.Parse(row["queuefailtime"].ToString());
            }
            if (row["Phone"].ToString() != "")
            {
                model.Phone = row["Phone"].ToString();
            }
            if (row["Sex"].ToString() != "")
            {
                bool.TryParse(row["Sex"].ToString(), out model.Sex);
            }
            if (row["ProvinceID"].ToString() != "")
            {
                model.ProvinceID = int.Parse(row["ProvinceID"].ToString());
            }
            if (row["CityID"].ToString() != "")
            {
                model.CityID = int.Parse(row["CityID"].ToString());
            }
            if (row["UserReferTitle"].ToString() != "")
            {
                model.UserReferTitle = row["UserReferTitle"].ToString();
            }
            if (row["UserReferURL"].ToString() != "")
            {
                model.UserReferURL = row["UserReferURL"].ToString();
            }
            if (row["Remark"].ToString() != "")
            {
                model.UserReferURL = row["Remark"].ToString();
            }

            return model;
        }
        #endregion
        public int InsertUserVisitLog(Entities.UserVisitLog model)
        {
            int visitid = 0;
            SqlParameter[] parameters = {
					new SqlParameter("@visitid", SqlDbType.Int, 4),
					new SqlParameter("@SourceType",SqlDbType.Int, 4),
					new SqlParameter("@LoginID", SqlDbType.VarChar,100),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,28),
                    new SqlParameter("@Sex", SqlDbType.Bit),
                    new SqlParameter("@provinceid", SqlDbType.Int),
                    new SqlParameter("@cityid", SqlDbType.Int),
                    new SqlParameter("@userrefertitle", SqlDbType.VarChar,256),
                    new SqlParameter("@userreferurl", SqlDbType.VarChar,100),
                    new SqlParameter("@remark", SqlDbType.VarChar,300),
                    new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@queuefailtime",SqlDbType.DateTime)
					};
            parameters[1].Value = model.SourceType;
            parameters[2].Value = model.LoginID;
            parameters[3].Value = model.CustID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.UserReferTitle;
            parameters[10].Value = model.UserReferURL;
            parameters[11].Value = model.Remark;
            parameters[12].Value = model.CreatTime;
            parameters[13].Value = model.UpdateTime;
            parameters[14].Value = model.QueuefailTime;
            parameters[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserVisitLog_Insert", parameters);
            visitid = (int)(parameters[0].Value);
            return visitid;
        }

        public void UpdateUserVisitLog(Entities.UserVisitLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@visitid", SqlDbType.Int, 4),
					new SqlParameter("@SourceType",SqlDbType.Int, 4),
					new SqlParameter("@LoginID", SqlDbType.VarChar,100),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,28),
                    new SqlParameter("@Sex", SqlDbType.Bit),
                    new SqlParameter("@provinceid", SqlDbType.Int),
                    new SqlParameter("@cityid", SqlDbType.Int),
                    new SqlParameter("@userrefertitle", SqlDbType.VarChar,256),
                    new SqlParameter("@userreferurl", SqlDbType.VarChar,100),
                    new SqlParameter("@remark", SqlDbType.VarChar,300),
                    new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@queuefailtime", SqlDbType.DateTime),
					};
            parameters[1].Value = model.SourceType;
            parameters[2].Value = model.LoginID;
            parameters[3].Value = model.CustID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.UserReferTitle;
            parameters[10].Value = model.UserReferURL;
            parameters[11].Value = model.Remark;
            parameters[12].Value = model.CreatTime;
            parameters[13].Value = model.UpdateTime;
            parameters[14].Value = model.QueuefailTime;
            parameters[0].Value = model.VisitID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserVisitLog_Update", parameters);
        }

        /// <summary>
        /// 根据访问id，会话id取
        /// </summary>
        /// <param name="visitid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public DataTable GetVisitAndCs(int visitid, int csid)
        {
            DataTable dt = null;
            string sqlstr = "select a.*,b.Csid,b.AgentStartTime,b.CreateTime,IsTurnIn,IsTurnOut,OrderID,b.CreateTime as 'cscreatetime',c.PerSatisfaction,c.ProSatisfaction from dbo.UserVisitLog a left join dbo.Conversations b on a.VisitID=b.VisitID left join UserSatisfaction c on b.csid=c.csid where a.visitid=" + visitid + " and b.CsID=" + csid;
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据访问id，更新排队放弃时间
        /// </summary>
        /// <param name="visitid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public void UpdateQueueFailTime(int visitid)
        {
            string sqlstr = "update dbo.UserVisitLog set queuefailtime='" + DateTime.Now + "',updatetime='" + DateTime.Now + "' where visitid=" + visitid;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);   
        }

    }
}

