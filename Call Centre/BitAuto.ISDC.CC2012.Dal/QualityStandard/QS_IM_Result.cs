using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class QS_IM_Result : DataBase
    {
        public static QS_IM_Result Instance = new QS_IM_Result();

        public const string P_QS_IM_Result_SELECT = "P_QS_IM_Result_Select";

        public DataTable GetQS_IM_Result(QueryQS_IM_Result query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhere(query);
            if (string.IsNullOrEmpty(order))
            {
                order = "a.CreateTime desc";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_IM_Result_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 获取where条件
        /// <summary>
        /// 获取where条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetWhere(QueryQS_IM_Result query)
        {
            string where = "";
            //CSID
            if (!string.IsNullOrEmpty(query.CSID))
            {
                where += " AND a.CSID='" + StringHelper.SqlFilter(query.CSID) + "' ";
            }
            //对话起始时间
            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                where += " AND a.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + "' ";
            }
            //对话结束时间
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                where += " AND a.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + "' ";
            }
            //开始次数
            if (!string.IsNullOrEmpty(query.BeginCount))
            {
                where += " AND a.SumDailog>=" + StringHelper.SqlFilter(query.BeginCount);
            }
            //结束次数
            if (!string.IsNullOrEmpty(query.EndCount))
            {
                where += " AND a.SumDailog<=" + StringHelper.SqlFilter(query.EndCount);
            }
            //坐席id
            if (!string.IsNullOrEmpty(query.AgentUserID))
            {
                where += " AND a.UserID='" + StringHelper.SqlFilter(query.AgentUserID) + "' ";
            }
            //评分日期-开始
            if (!string.IsNullOrEmpty(query.ScoreBeginTime))
            {
                where += " AND b.CreateTime>='" + StringHelper.SqlFilter(query.ScoreBeginTime) + " 00:00:00' ";
            }
            //评分日期-结束
            if (!string.IsNullOrEmpty(query.ScoreEndTime))
            {
                where += " AND b.CreateTime<='" + StringHelper.SqlFilter(query.ScoreEndTime) + " 23:59:59' ";
            }
            //评分表
            if (!string.IsNullOrEmpty(query.ScoreTable) && query.ScoreTable != "-1")
            {
                //待评分 取范围表字段；已评分 取结果表字段
                where += " AND ((c.QS_IM_RTID='" + StringHelper.SqlFilter(query.ScoreTable) + "' AND ISNULL(b.STATUS, 20001)=20001) OR b.QS_RTID='" + StringHelper.SqlFilter(query.ScoreTable) + "') ";
            }
            //坐席分组
            if (!string.IsNullOrEmpty(query.BGID) && query.BGID != "-1")
            {
                where += " AND a.BGID='" + StringHelper.SqlFilter(query.BGID) + "' ";
            }
            //业务线
            if (!string.IsNullOrEmpty(query.BusinessLine) && query.BusinessLine != "-1")
            {
                //暂时不做
                where += "";
            }
            //申诉时间
            where += GetQS_ApprovalHistoryWhere(query.AppealBeginTime, query.AppealEndTime);
            //评分人
            if (!string.IsNullOrEmpty(query.ScoreCreater) && query.ScoreCreater != "-1")
            {
                where += " AND b.CreateUserID='" + StringHelper.SqlFilter(query.ScoreCreater) + "' ";
            }   
            ////成绩
            //if (!string.IsNullOrEmpty(query.Qualified))
            //{
            //    where += " AND b.IsQualified IN (" + Dal.Util.SqlFilterByInCondition(query.Qualified) + ") ";
            //}
            //评分状态
            if (!string.IsNullOrEmpty(query.QSResultStatus) && query.QSResultStatus != "-1")
            {
                where += " AND ISNULL(b.STATUS, 20001)='" + StringHelper.SqlFilter(query.QSResultStatus) + "' ";

                //申诉结果 (已申诉)
                if (!string.IsNullOrEmpty(query.QSStateResult) && query.QSResultStatus == "20006")
                {
                    where += " AND b.StateResult IN (" + Dal.Util.SqlFilterByInCondition(query.QSStateResult) + ") ";
                }
            }

            //是否评价 0 已评价 1未评价
            if (!string.IsNullOrEmpty(query.QSResultScore) && query.QSResultScore != "-1")
            {
                if (query.QSResultScore == "0")
                {
                    string subwhere = "";
                    //服务评价
                    if (!string.IsNullOrEmpty(query.PerQSResultScore) && query.PerQSResultScore != "-1")
                    {
                        subwhere += " a.PerSatisfaction='" + StringHelper.SqlFilter(query.PerQSResultScore) + "' AND ";
                    }
                    else
                    {
                        //有评价
                        subwhere += " a.PerSatisfaction IS NOT NULL AND ";
                    }
                    //产品评价
                    if (!string.IsNullOrEmpty(query.ProQSResultScore) && query.ProQSResultScore != "-1")
                    {
                        subwhere += " a.ProSatisfaction='" + StringHelper.SqlFilter(query.ProQSResultScore) + "' AND ";
                    }
                    else
                    {
                        //有评价
                        subwhere += " a.ProSatisfaction IS NOT NULL AND ";
                    }
                    if (subwhere.Length > 4)
                    {
                        subwhere = subwhere.Substring(0, subwhere.Length - 4);
                    }

                    where += " AND (" + subwhere + ") ";
                }
                else if (query.QSResultScore == "1")
                {
                    //没有评价
                    where += " AND a.PerSatisfaction IS NULL AND a.ProSatisfaction IS NULL ";
                }
            }
            #region 数据权限判断
            if (query.LoginUerID > 0)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "UserID", query.LoginUerID);
                where += whereDataRight;
            }
            #endregion
            return where;
        }
        /// 申诉条件控制
        /// <summary>
        /// 申诉条件控制
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private string GetQS_ApprovalHistoryWhere(string st, string et)
        {
            if (!string.IsNullOrEmpty(st) || !string.IsNullOrEmpty(et))
            {
                string sql = " and exists ( select 1 from QS_ApprovalHistory where ApprovalType='30007' and QS_ApprovalHistory.QS_RID=b.QS_RID ";
                if (!string.IsNullOrEmpty(st))
                {
                    sql += " and CreateTime >='" + StringHelper.SqlFilter(st) + " 00:00:00' ";
                }
                if (!string.IsNullOrEmpty(et))
                {
                    sql += " and CreateTime <='" + StringHelper.SqlFilter(et) + " 23:59:59' ";
                }
                sql += ")";
                return sql;
            }
            else return "";
        }

        public bool HasScored(string csid)
        {
            string strSql = "select count(1) from qs_im_result where csid='" + StringHelper.SqlFilter(csid) + "'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj != null)
            {
                int num = 0;
                int.TryParse(obj.ToString(), out num);
                return num > 0 ? true : false;
            }
            else
            {
                return false;
            }
        }

        /// 生成主键
        /// <summary>
        /// 生成主键
        /// </summary>
        /// <returns></returns>
        public int CreateQS_RID()
        {
            string sql = @"SELECT dbo.fn_CreateQS_RID()";
            object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return CommonFunction.ObjectToInteger(o);
        }
        /// <summary>
        /// 根据RTID和CSID获取RTID
        /// </summary>
        /// <param name="qsRtid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetRidByCsidAndRtid(int qsRtid,Int64 csid)
        {
            string sql = @"SELECT QS_RID FROM dbo.QS_IM_Result WHERE QS_RTID=" + qsRtid + " AND CSID=" + csid;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return CommonFunction.ObjectToInteger(obj, 0);
        }
    }
}
