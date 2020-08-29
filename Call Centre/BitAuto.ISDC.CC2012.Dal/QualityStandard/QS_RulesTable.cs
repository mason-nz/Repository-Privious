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
    /// 数据访问类QS_RulesTable。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:37 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_RulesTable : DataBase
    {
        #region Instance
        public static readonly QS_RulesTable Instance = new QS_RulesTable();
        #endregion

        #region const
        private const string P_QS_RULESTABLE_SELECT = "p_QS_RulesTable_Select";
        private const string P_QS_RULESTABLE_INSERT = "p_QS_RulesTable_Insert";
        private const string P_QS_RULESTABLE_UPDATE = "p_QS_RulesTable_Update";
        private const string P_QS_RULESTABLE_DELETE = "p_QS_RulesTable_Delete";
        #endregion

        #region Contructor
        protected QS_RulesTable()
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
        public DataTable GetQS_RulesTable(QueryQS_RulesTable query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 条件
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += "and QS_RulesTable.QS_RTID=" + query.QS_RTID;
            }
            if (query.RuleTableStatus != Constant.STRING_INVALID_VALUE && query.RuleTableStatus != string.Empty)
            {
                where += "and QS_RulesTable.Status in (" + Dal.Util.SqlFilterByInCondition(query.RuleTableStatus) + ")";
            }
            if (query.RuleTableInUseStatus != Constant.STRING_INVALID_VALUE && query.RuleTableInUseStatus != string.Empty)
            {
                string[] array = query.RuleTableInUseStatus.Split(',');
                if (array.Length > 1)
                {
                    //未使用、使用的都显示，不加限制条件
                }
                else
                {
                    if (array[0] == "10001")//未使用
                    {
                        where += " and isInUse=0 ";
                    }
                    else
                    {
                        where += " and isInUse>0 ";
                    }
                }
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " and QS_RulesTable.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE && query.BeginTime != Constant.STRING_EMPTY_VALUE)
            {
                where += " and QS_RulesTable.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:0:0'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE && query.EndTime != Constant.STRING_EMPTY_VALUE)
            {
                where += " and QS_RulesTable.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE && query.CreateUserID != -1)
            {
                where += " and QS_RulesTable.CreateUserID=" + query.CreateUserID;
            }

            where += " and QS_RulesTable.Status<>10004";


            #endregion

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RULESTABLE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_RulesTable GetQS_RulesTable(int QS_RTID)
        {
            QueryQS_RulesTable query = new QueryQS_RulesTable();
            query.QS_RTID = QS_RTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_RulesTable(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleQS_RulesTable(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.QS_RulesTable LoadSingleQS_RulesTable(DataRow row)
        {
            Entities.QS_RulesTable model = new Entities.QS_RulesTable();

            if (row["QS_RTID"].ToString() != "")
            {
                model.QS_RTID = int.Parse(row["QS_RTID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["ScoreType"].ToString() != "")
            {
                model.ScoreType = int.Parse(row["ScoreType"].ToString());
            }
            model.Description = row["Description"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["isInUse"].ToString() != "")
            {
                model.StatusInUse = int.Parse(row["isInUse"].ToString());
            }
            if (row["DeadItemNum"].ToString() != "")
            {
                model.DeadItemNum = int.Parse(row["DeadItemNum"].ToString());
            }
            if (row["NoDeadItemNum"].ToString() != "")
            {
                model.NoDeadItemNum = int.Parse(row["NoDeadItemNum"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            model.LastModifyUserID = row["LastModifyUserID"].ToString();
            if (row["HaveQAppraisal"].ToString() != "")
            {
                model.HaveQAppraisal = int.Parse(row["HaveQAppraisal"].ToString());
            }
            model.RegionID = row["RegionID"].ToString();
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.QS_RulesTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@DeadItemNum", SqlDbType.Int,4),
					new SqlParameter("@NoDeadItemNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.VarChar,50),
					new SqlParameter("@HaveQAppraisal", SqlDbType.Int,4),
					new SqlParameter("@RegionID", SqlDbType.VarChar,10)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ScoreType;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.DeadItemNum;
            parameters[6].Value = model.NoDeadItemNum;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.LastModifyTime;
            parameters[10].Value = model.LastModifyUserID;
            parameters[11].Value = model.HaveQAppraisal;
            parameters[12].Value = model.RegionID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RULESTABLE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_RulesTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@DeadItemNum", SqlDbType.Int,4),
					new SqlParameter("@NoDeadItemNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.VarChar,50),
					new SqlParameter("@HaveQAppraisal", SqlDbType.Int,4),
					new SqlParameter("@RegionID", SqlDbType.VarChar,10)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ScoreType;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.DeadItemNum;
            parameters[6].Value = model.NoDeadItemNum;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.LastModifyTime;
            parameters[10].Value = model.LastModifyUserID;
            parameters[11].Value = model.HaveQAppraisal;
            parameters[12].Value = model.RegionID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_RULESTABLE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.QS_RulesTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@DeadItemNum", SqlDbType.Int,4),
					new SqlParameter("@NoDeadItemNum", SqlDbType.Int,4),
				 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.VarChar,50),
					new SqlParameter("@HaveQAppraisal", SqlDbType.Int,4),
					new SqlParameter("@RegionID", SqlDbType.VarChar,10)};
            parameters[0].Value = model.QS_RTID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ScoreType;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.DeadItemNum;
            parameters[6].Value = model.NoDeadItemNum;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.HaveQAppraisal;
            parameters[10].Value = model.RegionID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RULESTABLE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_RulesTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@DeadItemNum", SqlDbType.Int,4),
					new SqlParameter("@NoDeadItemNum", SqlDbType.Int,4),					 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.VarChar,50),
					new SqlParameter("@HaveQAppraisal", SqlDbType.Int,4),
					new SqlParameter("@RegionID", SqlDbType.VarChar,10)};
            parameters[0].Value = model.QS_RTID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.ScoreType;
            parameters[3].Value = model.Description;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.DeadItemNum;
            parameters[6].Value = model.NoDeadItemNum;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.HaveQAppraisal;
            parameters[10].Value = model.RegionID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_RULESTABLE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int QS_RTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4)};
            parameters[0].Value = QS_RTID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RULESTABLE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_RTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4)};
            parameters[0].Value = QS_RTID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_RULESTABLE_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 根据评分表ID取评分表明细，返回Dataset包括五个DataTable 分别是评分分类，质检项目，质检标准，扣分项，致命项
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesTableDetailByQS_RTID(int QS_RTID)
        {
            DataSet ds = null;
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = QS_RTID;
            string sqlstr = "select * from dbo.QS_RulesTable where qs_RTID=@QS_RTID;select * from dbo.QS_Category where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_Item where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_Standard where qs_RTID=@QS_RTID and status=0 ORDER BY QS_CID,QS_IID,SkillLevel DESC;select * from dbo.QS_Marking where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_DeadOrAppraisal where qs_RTID=@QS_RTID and status=0";
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return ds;

        }

        /// <summary>
        /// 根据评分表ID,质检成绩ID取评分表明细，返回Dataset包括五个DataTable 分别是评分分类，质检项目，质检标准，扣分项，致命项
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesTableDetailByQS_RTID(int QS_RTID, int QS_RID)
        {
            DataSet ds = null;
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
                    new SqlParameter("@QS_RID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = QS_RTID;
            parameters[1].Value = QS_RID;
            string sqlstr = @"SELECT  *
                    FROM    dbo.QS_RulesTable
                    WHERE   qs_RTID = @QS_RTID;
                    SELECT  *
                    FROM    dbo.QS_Category
                    WHERE   qs_RTID = @QS_RTID
                            AND status = 0;
                    SELECT  *
                    FROM    dbo.QS_Item
                    WHERE   qs_RTID = @QS_RTID
                            AND status = 0;
                    SELECT  *
                    FROM    dbo.QS_Standard
                    WHERE   qs_RTID = @QS_RTID
                            AND status = 0
                    ORDER BY QS_CID,QS_IID,SkillLevel DESC;
                    SELECT  *
                    FROM    dbo.QS_Marking
                    WHERE   qs_RTID = @QS_RTID
                            AND status = 0;
                    SELECT  *
                    FROM    dbo.QS_DeadOrAppraisal
                    WHERE   qs_RTID = @QS_RTID
                            AND status = 0;
                    SELECT  *
                    FROM    dbo.QS_ResultDetail
                    WHERE   QS_RTID = @QS_RTID
                            AND QS_RID = @QS_RID
                            AND status = 0;
                    SELECT  *
                    FROM    dbo.QS_Result
                    WHERE   qs_RID = @QS_RID;
                    SELECT  *
                    FROM    dbo.QS_IM_Result
                    WHERE   qs_RID = @QS_RID";
                
                // "select * from dbo.QS_RulesTable where qs_RTID=@QS_RTID;select * from dbo.QS_Category where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_Item where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_Standard where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_Marking where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_DeadOrAppraisal where qs_RTID=@QS_RTID and status=0;select * from dbo.QS_ResultDetail where QS_RTID=@QS_RTID and QS_RID=@QS_RID and status=0;select * from dbo.QS_Result where qs_RID=@QS_RID";
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return ds;

        }

        /// <summary>
        /// 根据RTID获取评分表类型
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        public string GetScoreTypeByRTID(int rtid)
        {
            string strSql = " SELECT ScoreType FROM   dbo.QS_RulesTable WHERE QS_RTID=" + rtid;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? "" : obj.ToString();
        }
        
        /// <summary>
        /// 判断指定评分表的名称是否已经存在（指定评分表类型）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scoreType"></param>
        /// <returns></returns>
        public bool IsRuleTableNameExist(string name, int scoreType)
        {
            string strSql = " SELECT COUNT(1) FROM dbo.QS_RulesTable WHERE name='" + SqlFilter(name) + "' AND ScoreType=" + scoreType;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            int nameCount = CommonFunction.ObjectToInteger(obj,0);
            if (nameCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


          
    }
}

