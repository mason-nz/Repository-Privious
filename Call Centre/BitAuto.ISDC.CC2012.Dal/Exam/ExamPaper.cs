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
    /// 数据访问类ExamPaper。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamPaper : DataBase
    {
        #region Instance
        public static readonly ExamPaper Instance = new ExamPaper();
        #endregion

        #region const
        private const string P_EXAMPAPER_SELECT = "p_ExamPaper_Select";
        private const string P_EXAMPAPER_INSERT = "p_ExamPaper_Insert";
        private const string P_EXAMPAPER_UPDATE = "p_ExamPaper_Update";
        private const string P_EXAMPAPER_DELETE = "p_ExamPaper_Delete";
        #endregion

        #region Contructor
        protected ExamPaper()
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
        public DataTable GetExamPaper(QueryExamPaper query, string order, int currentPage, int pageSize, int loginUserID, out int totalCount)
        {
            string where = loginUserID > 0
                ? Dal.UserGroupDataRigth.Instance.GetSqlRightstr("ExamPaper", "BGID", "CreaetUserID", loginUserID)
                : string.Empty;

            if (query.EPID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EPID=" + query.EPID.ToString();
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND Name like '%" + StringHelper.SqlFilter(query.Name.ToString()) + "%'";
            }
            if (query.ECID != Constant.INT_INVALID_VALUE && query.ECID != -1)
            {
                where += " AND ECID=" + query.ECID.ToString();
            }
            if (query.ECIDStr != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ECID in(" + Dal.Util.SqlFilterByInCondition(query.ECIDStr) + ") ";
            }
            if (query.CreateBeginTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND CreateTime >='" + StringHelper.SqlFilter(query.CreateBeginTime.ToString()) + "'";
            }
            if (query.CreateEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND CreateTime <='" + StringHelper.SqlFilter(Convert.ToDateTime(query.CreateEndTime).AddDays(1).ToString()) + "'";
            }
            if (query.Status != Constant.STRING_INVALID_VALUE)
            {
                where += " AND dbo.GetStatusByEPID(EPID,Status) in(" + Dal.Util.SqlFilterByInCondition(query.Status.ToString()) + ")";
            }
            if (query.CreaetUserID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND CreaetUserID in(" + Dal.Util.SqlFilterByInCondition(query.CreaetUserID.ToString()) + ")";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND BGID=" + query.BGID.ToString();
            }

            //添加数据权限

            //string wherePlus = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("KnowledgeLib", "BGID", "CreateUserID", BLL.Util.GetLoginUserID()) + "   AND KnowledgeLib.Status!=5 ";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPAPER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照查询条件查询（在线考试列表页使用 lxw）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetExamPaperByExamList(QueryExamPaper query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where1 = string.Empty;

            where1 += " AND ei.ExamStartTime<='" + DateTime.Now + "'";

            if (query.ExamPersonID != Constant.INT_INVALID_VALUE)
            {
                where1 += " AND ePerson.ExamPerSonID=" + query.ExamPersonID;
            }
            if (query.ExamCategory != Constant.STRING_INVALID_VALUE)
            {
                where1 += " AND ep.ECID IN (" + Dal.Util.SqlFilterByInCondition(query.ExamCategory) + ")";
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where1 += " AND ep.Name like '%" + StringHelper.SqlFilter(query.Name.ToString()) + "%'";
            }
            if (query.TestOverEndTime != Constant.STRING_INVALID_VALUE)
            {
                where1 += " AND ei.ExamEndTime<'" + DateTime.Now + "'";
            }
            //if (query.NoTestEndTime != Constant.STRING_INVALID_VALUE)
            //{
            //    where1 += " AND ei.ExamEndTime>='" + DateTime.Now + "'";
            //}


            string where2 = string.Empty;

            where2 += " AND muei.MakeUpExamStartTime<='" + DateTime.Now + "'";

            if (query.ExamPersonID != Constant.INT_INVALID_VALUE)
            {
                where2 += " AND ePerson.ExamPerSonID=" + query.ExamPersonID;
            }
            if (query.ExamCategory != Constant.STRING_INVALID_VALUE)
            {
                where2 += " AND ep.ECID IN (" + Dal.Util.SqlFilterByInCondition(query.ExamCategory) + ")";
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where2 += " AND ep.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.TestOverEndTime != Constant.STRING_INVALID_VALUE)
            {
                where2 += " AND muei.MakeUpExamEndTime<'" + DateTime.Now + "'";
            }
            //if (query.NoTestEndTime != Constant.STRING_INVALID_VALUE)
            //{
            //    where2 += " AND muei.MakeUpExamEndTime>='" + DateTime.Now + "'";
            //}

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where1", SqlDbType.NVarChar, 40000),
					new SqlParameter("@where2", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where1;
            parameters[1].Value = where2;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_EXAMPAPER_SELECTByExamList", parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamPaper GetExamPaper(long EPID)
        {
            QueryExamPaper query = new QueryExamPaper();
            query.EPID = EPID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamPaper(query, string.Empty, 1, 1, 0, out count);
            if (count > 0)
            {
                return LoadSingleExamPaper(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamPaper LoadSingleExamPaper(DataRow row)
        {
            Entities.ExamPaper model = new Entities.ExamPaper();

            if (row["EPID"].ToString() != "")
            {
                model.EPID = long.Parse(row["EPID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["ECID"].ToString() != "")
            {
                model.ECID = int.Parse(row["ECID"].ToString());
            }
            model.ExamDesc = row["ExamDesc"].ToString();
            if (row["TotalScore"].ToString() != "")
            {
                model.TotalScore = int.Parse(row["TotalScore"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
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
            if (row["bgid"].ToString() != "")
            {
                model.BGID = int.Parse(row["bgid"].ToString());
            }

            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ExamPaper model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@ExamDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@TotalScore", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Bgid", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.ExamDesc;
            parameters[4].Value = model.TotalScore;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;
            parameters[8].Value = model.LastModifyTime;
            parameters[9].Value = model.LastModifyUserID;
            parameters[10].Value = model.BGID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPAPER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamPaper model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.Int,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@ExamDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@TotalScore", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Bgid", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.ExamDesc;
            parameters[4].Value = model.TotalScore;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;
            parameters[8].Value = model.LastModifyTime;
            parameters[9].Value = model.LastModifyUserID;
            parameters[10].Value = model.BGID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMPAPER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamPaper model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@ExamDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@TotalScore", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Bgid", SqlDbType.Int,4)};
            parameters[0].Value = model.EPID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.ExamDesc;
            parameters[4].Value = model.TotalScore;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;
            parameters[8].Value = model.LastModifyTime;
            parameters[9].Value = model.LastModifyUserID;
            parameters[10].Value = model.BGID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPAPER_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamPaper model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.Int,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@ExamDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@TotalScore", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Bgid", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.EPID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ECID;
            parameters[3].Value = model.ExamDesc;
            parameters[4].Value = model.TotalScore;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;
            parameters[8].Value = model.LastModifyTime;
            parameters[9].Value = model.LastModifyUserID;
            parameters[10].Value = model.BGID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMPAPER_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long EPID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt)};
            parameters[0].Value = EPID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPAPER_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EPID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt)};
            parameters[0].Value = EPID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMPAPER_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 获取所有创建人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers(int userId)
        {

            //            string sqlStr = @"	
            //                SELECT 
            //		            a.UserID,a.TrueName 
            //	            FROM SysRightsManager.dbo.UserInfo a
            //		            JOIN dbo.EmployeeAgent b ON a.UserID = b.UserID
            //	            WHERE EXISTS(SELECT 1 from dbo.ExamPaper c WHERE c.CreaetUserID =a.UserID )
            //		            AND b.RegionID =(SELECT RegionID FROM dbo.EmployeeAgent WHERE UserID=" + userId.ToString() + ")";
            string sqlStr = "SELECT UserID,TrueName FROM SysRightsManager.dbo.UserInfo WHERE "
                            + " EXISTS ("
                            + " SELECT 1 FROM (SELECT CreaetUserID FROM dbo.ExamPaper GROUP BY CreaetUserID) e WHERE UserID=e.CreaetUserID)";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds;
        }
    }
}

