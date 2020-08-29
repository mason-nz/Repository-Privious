using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类AutoCall_ProjectInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-09-14 09:57:58 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AutoCall_ProjectInfo : DataBase
    {
        #region Instance
        public static readonly AutoCall_ProjectInfo Instance = new AutoCall_ProjectInfo();
        #endregion

        #region const
        private const string P_AUTOCALL_PROJECTINFO_SELECT = "p_AutoCall_ProjectInfo_Select";
        private const string P_AUTOCALL_PROJECTINFO_INSERT = "p_AutoCall_ProjectInfo_Insert";
        private const string P_AUTOCALL_PROJECTINFO_UPDATE = "p_AutoCall_ProjectInfo_Update";
        private const string P_AUTOCALL_PROJECTINFO_DELETE = "p_AutoCall_ProjectInfo_Delete";
        #endregion

        #region Contructor
        protected AutoCall_ProjectInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetAutoCall_ProjectInfo(QueryAutoCall_ProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataTable GetAutoCall_ProjectInfo(string proName, string strBGID, string strSCID, string strStatus, string ACStatus, int UserId, int currentPage, int pageSize, out int totalCount)
        {

            StringBuilder sbWhere = new StringBuilder();
            if (string.IsNullOrEmpty(strBGID) || strBGID == "-1")
            {
                sbWhere.Append(string.Format(" where (a.CreateUserID={0} or b.BGID in (SELECT BGID FROM dbo.UserGroupDataRigth u WHERE UserID={0}))", UserId));
            }
            else
            {
                sbWhere.Append(string.Format(" where b.BGID ={0} ", StringHelper.SqlFilter(strBGID)));
            }

            if (!string.IsNullOrEmpty(strSCID))
            {
                sbWhere.Append(string.Format(" and b.SCID ={0} ", StringHelper.SqlFilter(strSCID)));
            }

            if (!string.IsNullOrEmpty(strStatus))
            {
                sbWhere.Append(string.Format(" and b.Status IN({0}) ", Dal.Util.SqlFilterByInCondition(strStatus)));
            }
            if (!string.IsNullOrEmpty(ACStatus))
            {
                sbWhere.Append(string.Format(" and a.ACStatus IN({0}) ", Dal.Util.SqlFilterByInCondition(ACStatus)));
            }

            if (!string.IsNullOrEmpty(proName))
            {
                sbWhere.Append(string.Format(" and b.Name LIKE '%{0}%' ", StringHelper.SqlFilter(proName)));
            }

            string strOrder =
                " CASE  a.ACStatus WHEN 1 THEN 0 WHEN 2 THEN 1 WHEN 0 THEN 2 WHEN 3 THEN 3 ELSE 4 END,a.CreateTime DESC ";

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sbWhere.ToString();
            parameters[1].Value = strOrder;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_GetAutoCallProject_info", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        public DataSet GetOutCall400Number()
        {
            string strSql = " SELECT cdid,remark FROM dbo.CallDisplay WHERE status=0 AND HotlineID>0 ORDER BY OrderNum ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.AutoCall_ProjectInfo GetAutoCall_ProjectInfo(long ProjectID)
        {
            QueryAutoCall_ProjectInfo query = new QueryAutoCall_ProjectInfo();
            query.ProjectID = ProjectID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAutoCall_ProjectInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleAutoCall_ProjectInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.AutoCall_ProjectInfo LoadSingleAutoCall_ProjectInfo(DataRow row)
        {
            Entities.AutoCall_ProjectInfo model = new Entities.AutoCall_ProjectInfo();

            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            if (row["SkillID"].ToString() != "")
            {
                model.SkillID = int.Parse(row["SkillID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["ACStatus"].ToString() != "")
            {
                model.ACStatus = int.Parse(row["ACStatus"].ToString());
            }
            model.CallNum = row["CallNum"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            if (row["TotalTaskNum"].ToString() != "")
            {
                model.TotalTaskNum = int.Parse(row["TotalTaskNum"].ToString());
            }
            if (row["AppendDataTime"].ToString() != "")
            {
                model.AppendDataTime = DateTime.Parse(row["AppendDataTime"].ToString());
            }
            //model.Timestamp=row["Timestamp"].ToString();
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.AutoCall_ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@SkillID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@CallNum", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@TotalTaskNum", SqlDbType.Int,4),
					new SqlParameter("@AppendDataTime", SqlDbType.DateTime)};
            //new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SkillID;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.ACStatus;
            parameters[4].Value = model.CallNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;
            parameters[9].Value = model.TotalTaskNum;
            parameters[10].Value = model.AppendDataTime;
            //parameters[11].Value = model.Timestamp;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.AutoCall_ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@SkillID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@CallNum", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@TotalTaskNum", SqlDbType.Int,4),
					new SqlParameter("@AppendDataTime", SqlDbType.DateTime)};
            //new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SkillID;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.ACStatus;
            parameters[4].Value = model.CallNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;
            parameters[9].Value = model.TotalTaskNum;
            parameters[10].Value = model.AppendDataTime;
            //parameters[11].Value = model.Timestamp;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_INSERT, parameters);
        }


        public void InsertAutoProBatch(string strProjectIDs, string skillGroupID, string CallID, int UserId, out string errorMsg)
        {
            errorMsg = string.Empty;
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectIDS", strProjectIDs),
					new SqlParameter("@SillGroupID", skillGroupID),
					new SqlParameter("@CallID", CallID),
					new SqlParameter("@userId", UserId),
					new SqlParameter("@errorMsg", SqlDbType.VarChar,4000)};

            parameters[4].Value = errorMsg;
            parameters[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_AutoCallProject_Insert", parameters);
        }



        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.AutoCall_ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@SkillID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@CallNum", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@TotalTaskNum", SqlDbType.Int,4),
					new SqlParameter("@AppendDataTime", SqlDbType.DateTime)};
            //new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SkillID;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.ACStatus;
            parameters[4].Value = model.CallNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;
            parameters[9].Value = model.TotalTaskNum;
            parameters[10].Value = model.AppendDataTime;
            //parameters[11].Value = model.Timestamp;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.AutoCall_ProjectInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@SkillID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@CallNum", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@TotalTaskNum", SqlDbType.Int,4),
					new SqlParameter("@AppendDataTime", SqlDbType.DateTime)};
            //new SqlParameter("@Timestamp", SqlDbType.Timestamp,8)};
            parameters[0].Value = model.ProjectID;
            parameters[1].Value = model.SkillID;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.ACStatus;
            parameters[4].Value = model.CallNum;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;
            parameters[9].Value = model.TotalTaskNum;
            parameters[10].Value = model.AppendDataTime;
            //parameters[11].Value = model.Timestamp;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_UPDATE, parameters);
        }


        public int Update(string strProjectID, string strSkillID, string strCallNum)
        {
            string strSql = string.Format("UPDATE dbo.AutoCall_ProjectInfo SET SkillID={0},CDID={1},ModifyTime=GETDATE() WHERE ProjectID={2}", StringHelper.SqlFilter(strSkillID), StringHelper.SqlFilter(strCallNum), StringHelper.SqlFilter(strProjectID));
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        public int UpdateAutoProjectStatus(string strProjectID, ProjectACStatus status)
        {
            string strSql = string.Format(" UPDATE dbo.AutoCall_ProjectInfo SET ACStatus={0},ModifyTime=GETDATE() WHERE ProjectID={1}",
                (int)status, StringHelper.SqlFilter(strProjectID));
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        public int InportAutoCallTask(string strProjectID, int userid)
        {
            return Dal.AutoCall_TaskInfo.Instance.AutoCallTaskInfoUpdate(CommonFunction.ObjectToLong(strProjectID), userid);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long ProjectID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt)};
            parameters[0].Value = ProjectID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long ProjectID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.BigInt)};
            parameters[0].Value = ProjectID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_PROJECTINFO_DELETE, parameters);
        }
        #endregion

    }
}

