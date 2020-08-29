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
    /// 数据访问类QS_RulesRange。
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
    public class QS_RulesRange : DataBase
    {
        public static readonly QS_RulesRange Instance = new QS_RulesRange();

        private const string P_QS_RULESRANGE_SELECT = "p_QS_RulesRange_Select";
        private const string p_QS_RulesRange_Manage = "p_QS_RulesRange_Manage";

        protected QS_RulesRange()
        { }
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetQS_RulesRange(QueryQS_RulesRange query, int userid, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "AND bg.BGID IN (SELECT BGID FROM UserGroupDataRigth WHERE USERID =" + userid + ") ";

            if (string.IsNullOrEmpty(order))
            {
                order = "bg.CreateTime DESC";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_RULESRANGE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        private Entities.QS_RulesRange LoadSingleQS_RulesRange(DataRow row)
        {
            Entities.QS_RulesRange model = new Entities.QS_RulesRange();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["QS_RTID"].ToString() != "")
            {
                model.QS_RTID = int.Parse(row["QS_RTID"].ToString());
            }
            if (row["QS_IM_RTID"].ToString() != "")
            {
                model.QS_IM_RTID = int.Parse(row["QS_IM_RTID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
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
            return model;
        }

        #region 应用范围管理
        /// <summary>
        /// 
        /// </summary>
        public int RangeManage(int BGID, int QS_RTID, int QS_IM_RTID, int UserID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int),
					new SqlParameter("@QS_RTID", SqlDbType.Int),
                    new SqlParameter("@QS_IM_RTID", SqlDbType.Int),
					new SqlParameter("@UserID", SqlDbType.Int)};
            parameters[0].Value = BGID;
            parameters[1].Value = QS_RTID;
            parameters[2].Value = QS_IM_RTID;
            parameters[3].Value = UserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QS_RulesRange_Manage", parameters);
        }

        public Entities.QS_RulesRange getModelByBGID(int BGID)
        {
            Entities.QS_RulesRange model = null;
            string sqlStr = "select * from QS_RulesRange where BGID=" + BGID;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model = LoadSingleQS_RulesRange(ds.Tables[0].Rows[0]);
                }
            }
            return model;
        }
        #endregion
    }
}

