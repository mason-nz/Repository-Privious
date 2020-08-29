using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Diagnostics;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class AgentStateDetail
    {
        #region Instance
        public static readonly AgentStateDetail Instance = new AgentStateDetail();
        #endregion

        #region Contructor
        protected AgentStateDetail()
        { }
        #endregion

        #region GetReport
        public DataTable GetStateDetail(QueryAgentStateDetail query, int currentPage, int pageSize, int loginUser, out int totalCount)
        {
            int isToday = 0;
            string strWhere = " where a.State not in (1,2) ";
            if (!string.IsNullOrEmpty(query.AgentID))
            {
                strWhere += " and a.AgentID ='" + StringHelper.SqlFilter(query.AgentID) + "' ";
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                strWhere += " and b.AgentNum='" + StringHelper.SqlFilter(query.AgentNum) + "' ";
            }
            if (!string.IsNullOrEmpty(query.StartTime))
            {
                DateTime dt;
                if (DateTime.TryParse(query.StartTime, out dt))
                {
                    strWhere += " and a.StartTime between '" + dt.ToShortDateString() + "' and '" + dt.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    if (DateTime.Now.ToShortDateString() != dt.ToShortDateString())
                    {
                        isToday = 1;
                    }
                }
            }
            if (!string.IsNullOrEmpty(query.State) && query.State != "-1")
            {
                strWhere += " and a.state =" + StringHelper.SqlFilter(query.State);
            }
            if (!string.IsNullOrEmpty(query.AgentAuxState) && query.AgentAuxState != "-1" && query.State == "4")
            {
                strWhere += " and a.AgentAuxState=" + StringHelper.SqlFilter(query.AgentAuxState);
            }
            strWhere += " and( a.BGID in(select BGID from UserGroupDataRigth where UserID=" + loginUser.ToString() + " ) or a.AgentID=" + loginUser.ToString() + ") ";
            BLL.Loger.Log4Net.Info(string.Format("【查询坐席状态明细——查询条件】：{0}", strWhere));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataTable dtAgentStateDetail = BitAuto.ISDC.CC2012.Dal.AgentStateDetail.Instance.GetAgentsDetails(strWhere, isToday, currentPage, pageSize, loginUser, out totalCount);
            stopwatch.Stop();
            BLL.Loger.Log4Net.Info(string.Format("【查询坐席状态明细——总耗时】：{0}毫秒", stopwatch.Elapsed.TotalMilliseconds));
            return dtAgentStateDetail;
        }
        #endregion
    }
}
