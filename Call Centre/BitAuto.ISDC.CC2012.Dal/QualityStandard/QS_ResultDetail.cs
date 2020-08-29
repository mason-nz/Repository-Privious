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
    /// 数据访问类QS_ResultDetail。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_ResultDetail : DataBase
    {
        #region Instance
        public static readonly QS_ResultDetail Instance = new QS_ResultDetail();
        #endregion

        #region const
        private const string P_QS_RESULTDETAIL_SELECT = "p_QS_ResultDetail_Select";
        private const string P_QS_RESULTDETAIL_INSERT = "p_QS_ResultDetail_Insert";
        private const string P_QS_RESULTDETAIL_UPDATE = "p_QS_ResultDetail_Update";
        private const string P_QS_RESULTDETAIL_DELETE = "p_QS_ResultDetail_Delete";
        #endregion

        #region Contructor
        protected QS_ResultDetail()
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
        public DataTable GetQS_ResultDetail(QueryQS_ResultDetail query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.QS_RID != Constant.INT_INVALID_VALUE)
            {
                where += " and QS_RID=" + query.QS_RID;
            }
            if (query.QS_RDID != Constant.INT_INVALID_VALUE)
            {
                where += " and QS_RDID=" + query.QS_RDID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULTDETAIL_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_ResultDetail GetQS_ResultDetail(int QS_RDID)
        {
            QueryQS_ResultDetail query = new QueryQS_ResultDetail();
            query.QS_RDID = QS_RDID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_ResultDetail(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleQS_ResultDetail(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.QS_ResultDetail LoadSingleQS_ResultDetail(DataRow row)
        {
            Entities.QS_ResultDetail model = new Entities.QS_ResultDetail();

            if (row["QS_RDID"].ToString() != "")
            {
                model.QS_RDID = int.Parse(row["QS_RDID"].ToString());
            }
            if (row["ScoreType"].ToString() != "")
            {
                model.ScoreType = int.Parse(row["ScoreType"].ToString());
            }
            if (row["QS_RTID"].ToString() != "")
            {
                model.QS_RTID = int.Parse(row["QS_RTID"].ToString());
            }
            if (row["QS_RID"].ToString() != "")
            {
                model.QS_RID = int.Parse(row["QS_RID"].ToString());
            }
            if (row["QS_CID"].ToString() != "")
            {
                model.QS_CID = int.Parse(row["QS_CID"].ToString());
            }
            if (row["QS_IID"].ToString() != "")
            {
                model.QS_IID = int.Parse(row["QS_IID"].ToString());
            }
            if (row["QS_SID"].ToString() != "")
            {
                model.QS_SID = int.Parse(row["QS_SID"].ToString());
            }
            if (row["QS_MID"].ToString() != "")
            {
                model.QS_MID = int.Parse(row["QS_MID"].ToString());
            }
            if (row["QS_MID_End"].ToString() != "")
            {
                model.QS_MID_End = int.Parse(row["QS_MID_End"].ToString());
            }
            if (row["Type"].ToString() != "")
            {
                model.Type = int.Parse(row["Type"].ToString());
            }
            if (row["ScoreDeadID"].ToString() != "")
            {
                model.ScoreDeadID = int.Parse(row["ScoreDeadID"].ToString());
            }
            if (row["ScoreDeadID_End"].ToString() != "")
            {
                model.ScoreDeadID_End = int.Parse(row["ScoreDeadID_End"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
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
            model.Remark = row["Remark"].ToString();
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.QS_ResultDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RDID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID_End", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID_End", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.ScoreType;
            parameters[2].Value = model.QS_RTID;
            parameters[3].Value = model.QS_RID;
            parameters[4].Value = model.QS_CID;
            parameters[5].Value = model.QS_IID;
            parameters[6].Value = model.QS_SID;
            parameters[7].Value = model.QS_MID;
            parameters[8].Value = model.QS_MID_End;
            parameters[9].Value = model.Type;
            parameters[10].Value = model.ScoreDeadID;
            parameters[11].Value = model.ScoreDeadID_End;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CreateTime;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.ModifyTime;
            parameters[16].Value = model.ModifyUserID;
            parameters[17].Value = model.Remark;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULTDETAIL_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_ResultDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RDID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID_End", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID_End", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.ScoreType;
            parameters[2].Value = model.QS_RTID;
            parameters[3].Value = model.QS_RID;
            parameters[4].Value = model.QS_CID;
            parameters[5].Value = model.QS_IID;
            parameters[6].Value = model.QS_SID;
            parameters[7].Value = model.QS_MID;
            parameters[8].Value = model.QS_MID_End;
            parameters[9].Value = model.Type;
            parameters[10].Value = model.ScoreDeadID;
            parameters[11].Value = model.ScoreDeadID_End;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CreateTime;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.ModifyTime;
            parameters[16].Value = model.ModifyUserID;
            parameters[17].Value = model.Remark;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_RESULTDETAIL_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.QS_ResultDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RDID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID_End", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID_End", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500)};
            parameters[0].Value = model.QS_RDID;
            parameters[1].Value = model.ScoreType;
            parameters[2].Value = model.QS_RTID;
            parameters[3].Value = model.QS_RID;
            parameters[4].Value = model.QS_CID;
            parameters[5].Value = model.QS_IID;
            parameters[6].Value = model.QS_SID;
            parameters[7].Value = model.QS_MID;
            parameters[8].Value = model.QS_MID_End;
            parameters[9].Value = model.Type;
            parameters[10].Value = model.ScoreDeadID;
            parameters[11].Value = model.ScoreDeadID_End;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CreateTime;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.ModifyTime;
            parameters[16].Value = model.ModifyUserID;
            parameters[17].Value = model.Remark;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULTDETAIL_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_ResultDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RDID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@QS_RID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID", SqlDbType.Int,4),
					new SqlParameter("@QS_MID_End", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID", SqlDbType.Int,4),
					new SqlParameter("@ScoreDeadID_End", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.VarChar,500)};
            parameters[0].Value = model.QS_RDID;
            parameters[1].Value = model.ScoreType;
            parameters[2].Value = model.QS_RTID;
            parameters[3].Value = model.QS_RID;
            parameters[4].Value = model.QS_CID;
            parameters[5].Value = model.QS_IID;
            parameters[6].Value = model.QS_SID;
            parameters[7].Value = model.QS_MID;
            parameters[8].Value = model.QS_MID_End;
            parameters[9].Value = model.Type;
            parameters[10].Value = model.ScoreDeadID;
            parameters[11].Value = model.ScoreDeadID_End;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CreateTime;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.ModifyTime;
            parameters[16].Value = model.ModifyUserID;
            parameters[17].Value = model.Remark;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_RESULTDETAIL_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int QS_RDID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RDID", SqlDbType.Int,4)};
            parameters[0].Value = QS_RDID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RESULTDETAIL_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_RDID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_RDID", SqlDbType.Int,4)};
            parameters[0].Value = QS_RDID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_RESULTDETAIL_DELETE, parameters);
        }

        /// <summary>
        /// 根据质检成绩主键，删除明细，逻辑删除
        /// </summary>
        /// <returns></returns>
        public int DeleteByQS_RID(int QS_RID)
        {
            string sqlstr = "update QS_ResultDetail set status=-1 where QS_RID=" + QS_RID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
        }

        #endregion

        /// <summary>
        /// 根据成绩表id，取得分情况或致命项非致命项数
        /// </summary>
        /// <param name="scoretype"></param>
        /// <param name="QS_RID"></param>
        /// <param name="QS_RTID"></param>
        /// <param name="ccOrIM">此字段供五级质检使用</param>
        /// <returns></returns>
        public DataTable GetQS_ResultForCalculate(string scoretype, int QS_RID, int QS_RTID,string ccOrIM)
        {
            DataTable dt = null;
            string sqlstr = string.Empty;
            //评分型
            if (scoretype == "1")
            {
                sqlstr = "select case when (d.score+sum(c.score))>=0 then (d.score+sum(c.score)) else 0 end as enscore from qs_ResultDetail a left join qs_Result b on a.qs_RID=b.qs_RID left join qs_Marking c on a.qs_mid_end=c.qs_mid left join qs_standard d on a.qs_sid=d.qs_sid where b.qs_RID=" + QS_RID + " and a.status=0 and qs_MID_End!=-2   group by a.qs_sid,d.score union all select d.score  enscore from qs_standard d where d.qs_RTID=" + QS_RTID + " and d.qs_sid not in (select qs_sid from qs_ResultDetail where qs_RID=" + QS_RID + " and status=0 and qs_MID_End!=-2) and d.status=0";
            }
            //五级质检_IM对话质检（qs_IM_Result）
            else if (scoretype == "3")
                {
                    if (ccOrIM.ToLower() == "cc" || ccOrIM.ToLower() == "im")
                    {
                        sqlstr = @"SELECT d.score AS enscore
                            FROM    qs_ResultDetail a
                                    LEFT JOIN qs" + (ccOrIM.ToLower() == "im" ? "_IM" : "") + @"_Result b ON a.qs_RID = b.qs_RID
                                    LEFT JOIN qs_Marking c ON a.qs_mid_end = c.qs_mid
                                    LEFT JOIN qs_standard d ON a.qs_sid = d.qs_sid
                            WHERE   b.qs_RID = " + QS_RID + @" 
                                    AND a.status = 0
                                    AND qs_MID_End != -2";
                    } 
                    else
                    {
                        return null;
                    }
                }
            //合格型
            else
            {
                if (ccOrIM.ToLower() == "cc" || ccOrIM.ToLower() == "im")
                {
                    sqlstr =  @"SELECT  SUM(CASE WHEN d.IsIsDead = 1 THEN 1
                                                 ELSE 0
                                            END) AS deadcount ,
                                        SUM(CASE WHEN d.IsIsDead = 0 THEN 1
                                                 ELSE 0
                                            END) AS nodeadcount
                                FROM    qs_ResultDetail a
                                        LEFT JOIN QS" + (ccOrIM.ToLower() == "im" ? "_IM" : "") + @"_Result b ON a.qs_RID = b.qs_RID
                                        LEFT JOIN qs_Marking c ON a.qs_mid_end = c.qs_mid
                                        LEFT JOIN qs_standard d ON a.qs_sid = d.qs_sid
                                WHERE   b.qs_RID = " + QS_RID + @"
                                        AND a.status = 0
                                        AND a.qs_mid_end != -2
                                GROUP BY d.qs_sid";
                } 
                else
                {
                    sqlstr = "SELECT    '' AS deadcount ,'' AS nodeadcount";
                }

                //sqlstr = "select sum(case when d.IsIsDead=1 then 1 else 0 end) as deadcount,sum(case when d.IsIsDead=0 then 1 else 0 end) as nodeadcount from qs_ResultDetail a left join qs_Result b on a.qs_RID=b.qs_RID left join qs_Marking c on a.qs_mid_end=c.qs_mid left join qs_standard d on a.qs_sid=d.qs_sid where b.qs_RID=" + QS_RID + " and a.status=0 and a.qs_mid_end!=-2   group by d.qs_sid";
            }
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            return dt;
        }
        public DataTable getDetailsByExport_IM(string where)
        {
            string sql = @"SELECT  a.QS_RID ,
                                            a.QS_IID ,
                                            a.CreateUserID ,
                                            a.QS_SID ,
                                            a.QS_MID ,
                                            d.score ,
                                            CASE WHEN ( d.score + SUM(c.score) ) >= 0 THEN SUM(c.score)
                                                 ELSE MIN(c.score)
                                            END AS enscore ,
                                            SUM(CASE WHEN a.ScoreDeadID_End > 0 THEN 1
                                                     ELSE 0
                                                END) AS deadNum
                                    FROM    dbo.QS_ResultDetail AS a
                                            LEFT JOIN qs_Marking c ON a.qs_mid_end = c.qs_mid
                                                                      AND c.status = 0
                                            LEFT JOIN qs_standard d ON a.qs_sid = d.qs_sid
                                                                       AND d.status = 0
                                            LEFT JOIN dbo.QS_Item e ON a.qs_IID = e.qs_IID
                                                                       AND e.status = 0
                                    WHERE   a.Status = 0 " + where + @"                                            
                                    GROUP BY a.QS_RID ,
                                            a.QS_IID ,
                                            a.QS_SID ,
                                            a.QS_MID ,
                                            a.CreateUserID ,
                                            d.score ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        #region 分月查询
        /// 成绩表详情导出查询
        /// <summary>
        /// 成绩表详情导出查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable getDetailsByExport(string where, string tableEndName)
        {
            DataSet ds = null;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20)
					};
            parameters[0].Value = where;
            parameters[1].Value = tableEndName;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QS_ResultDetail_selectByExport", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            { return null; }
        }
        #endregion
    }
}

