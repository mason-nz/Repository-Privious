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
    /// 数据访问类ExamInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:16 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamInfo : DataBase
    {
        #region Instance
        public static readonly ExamInfo Instance = new ExamInfo();
        #endregion

        #region const
        private const string P_EXAMINFO_SELECT = "p_ExamInfo_Select";
        private const string P_EXAMINFO_INSERT = "p_ExamInfo_Insert";
        private const string P_EXAMINFO_UPDATE = "p_ExamInfo_Update";
        private const string P_EXAMINFO_DELETE = "p_ExamInfo_Delete";
        private const string P_GETSCORELISTBYEIID = "p_GetScoreListByEiid";
        #endregion

        #region Contructor
        protected ExamInfo()
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
        public DataTable GetExamInfo(QueryExamInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.EIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ExamInfo.EIID=" + query.EIID;
            }
            if (query.IsMakeUp != Constant.INT_INVALID_VALUE)
            {
                where += " AND ExamInfo.IsMakeUp=" + query.IsMakeUp;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMINFO_SELECT, parameters);
            totalCount = Convert.ToInt32((parameters[4].Value));
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetExamInfo2(string query, string order, int currentPage, int pageSize, out int totalCount, int userid)
        {
            string where = query;
            where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("ExamInfo", "BGID", "CreaetUserID", userid);
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMINFO_SELECT, parameters);
            totalCount = Convert.ToInt32((parameters[4].Value));
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamInfo GetExamInfo(long EIID)
        {
            QueryExamInfo query = new QueryExamInfo();
            query.EIID = EIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamInfo LoadSingleExamInfo(DataRow row)
        {
            Entities.ExamInfo model = new Entities.ExamInfo();

            if (row["EIID"].ToString() != "")
            {
                model.EIID = long.Parse(row["EIID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["ECID"].ToString() != "")
            {
                model.ECID = int.Parse(row["ECID"].ToString());
            }
            model.Description = row["Description"].ToString();
            model.BusinessGroup = row["BusinessGroup"].ToString();
            if (row["EPID"].ToString() != "")
            {
                model.EPID = int.Parse(row["EPID"].ToString());
            }
            if (row["ExamStartTime"].ToString() != "")
            {
                model.ExamStartTime = DateTime.Parse(row["ExamStartTime"].ToString());
            }
            if (row["ExamEndTime"].ToString() != "")
            {
                model.ExamEndTime = DateTime.Parse(row["ExamEndTime"].ToString());
            }
            if (row["JoinNum"].ToString() != "")
            {
                model.JoinNum = int.Parse(row["JoinNum"].ToString());
            }
            if (row["IsMakeUp"].ToString() != "")
            {
                model.IsMakeUp = int.Parse(row["IsMakeUp"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreaetUserID"].ToString() != "")
            {
                model.CreaetUserID = int.Parse(row["CreaetUserID"].ToString());
            }
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["BGID"] != null && row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ExamInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.BusinessGroup;
            parameters[5].Value = model.EPID;
            parameters[6].Value = model.ExamStartTime;
            parameters[7].Value = model.ExamEndTime;
            parameters[8].Value = model.JoinNum;
            parameters[9].Value = model.IsMakeUp;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreaetUserID;
            parameters[12].Value = model.LastModifyTime;
            parameters[13].Value = model.LastModifyUserID;
            parameters[14].Value = model.Status;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.BusinessGroup;
            parameters[5].Value = model.EPID;
            parameters[6].Value = model.ExamStartTime;
            parameters[7].Value = model.ExamEndTime;
            parameters[8].Value = model.JoinNum;
            parameters[9].Value = model.IsMakeUp;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreaetUserID;
            parameters[12].Value = model.LastModifyTime;
            parameters[13].Value = model.LastModifyUserID;
            parameters[14].Value = model.Status;
            parameters[15].Value = model.BGID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMINFO_INSERT, parameters);
            return Convert.ToInt32(parameters[0].Value);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Value = model.EIID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.BusinessGroup;
            parameters[5].Value = model.EPID;
            parameters[6].Value = model.ExamStartTime;
            parameters[7].Value = model.ExamEndTime;
            parameters[8].Value = model.JoinNum;
            parameters[9].Value = model.IsMakeUp;
            parameters[10].Value = model.LastModifyTime;
            parameters[11].Value = model.LastModifyUserID;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.BGID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@BusinessGroup", SqlDbType.NVarChar,50),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@JoinNum", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Value = model.EIID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.BusinessGroup;
            parameters[5].Value = model.EPID;
            parameters[6].Value = model.ExamStartTime;
            parameters[7].Value = model.ExamEndTime;
            parameters[8].Value = model.JoinNum;
            parameters[9].Value = model.IsMakeUp;
            parameters[10].Value = model.LastModifyTime;
            parameters[11].Value = model.LastModifyUserID;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.BGID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long EIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt)};
            parameters[0].Value = EIID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt)};
            parameters[0].Value = EIID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMINFO_DELETE, parameters);
        }
        #endregion

        #region 获取所有创建人
        /// <summary>
        /// 获取所有创建人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers()
        {
            string sqlStr = "SELECT UserID,TrueName FROM SysRightsManager.dbo.UserInfo WHERE "
                            + " EXISTS ("
                            + " SELECT 1 FROM (SELECT CreaetUserID FROM dbo.ExamInfo GROUP BY CreaetUserID) e WHERE UserID=e.CreaetUserID)";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds;
        }
        /// <summary>
        /// 获取所有创建人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers(int userId)
        {

            string sqlStr = @"	
                SELECT 
		            a.UserID,a.TrueName 
	            FROM SysRightsManager.dbo.UserInfo a
		            JOIN dbo.EmployeeAgent b ON a.UserID = b.UserID
	            WHERE EXISTS(SELECT 1 from dbo.ExamInfo c WHERE c.CreaetUserID =a.UserID )
		            AND b.RegionID =(SELECT RegionID FROM dbo.EmployeeAgent WHERE UserID=" + userId.ToString() + ")";           
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds;
        }
        #endregion

        /// <summary>
        /// 根据项目ID获取成绩
        /// </summary>
        /// <param name="eiid"></param>
        /// <returns></returns>
        public DataTable GetScoreListByEIID(string eiid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@eiid", SqlDbType.NVarChar, 40000)
				
					};

            parameters[0].Value = eiid;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GETSCORELISTBYEIID, parameters);
            return ds.Tables[0];
        }

        public int GetExamPaperUsedCount(long epid)
        {
            string strSql = "SELECT COUNT (1) FROM dbo.ExamInfo WHERE EPID =" + epid ;
            return CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql));
           
        }
    }
}

