using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class AgentTimeState
    {
        public static readonly AgentTimeState Instance = new AgentTimeState();
        protected AgentTimeState()
        {
        }

        public DataTable GetAllAreaFromDB()
        {
            return Dal.AgentTimeState.Instance.GetAllStateFromDB();
        }

        /// <summary>
        /// 查询坐席实时状态
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetAllStateFromDB(QueryAgentState query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AgentTimeState.Instance.GetAllStateFromDB(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetAllStateStatisticsFromDB(QueryAgentState query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AgentTimeState.Instance.GetAllStateStatisticsFromDB(query, order, currentPage, pageSize, out totalCount);
        }
        #region 插入坐席临时状态记录

        public bool InsertAgentState2DB(int state, int agentAuxState, int callType, int agTime, DateTime startTime, string agentid, string truename, string groupname, string extensionnum)
        {
            return Dal.AgentTimeState.Instance.InsertAgentState2DB(state, agentAuxState, callType, agTime, startTime, agentid, truename, groupname, extensionnum);
        }

        #endregion

        #region 更新坐席临时状态记录
        public bool UpdateAgentState2DB(int state, int agentAuxState, int callType, int agTime, DateTime startTime, string agentid)
        {
            return Dal.AgentTimeState.Instance.UpdateAgentState2DB(state, agentAuxState, callType, agTime, startTime, agentid);
        }
        #endregion

        #region 删除临时记录

        public bool DeleteAgentState2DB(string agentid)
        {
            return Dal.AgentTimeState.Instance.DeleteAgentState2DB(agentid);
        }

        #endregion

        #region 插入坐席状态明细记录
        public int InsertAgentStateDetail2DB(int state, int agentAuxState, int callType, DateTime startTime, DateTime endTime, string truename, string agentid, string extensionnum, int bgid)
        {
            return Dal.AgentTimeState.Instance.InsertAgentStateDetail2DB(state, agentAuxState, callType, startTime, endTime, truename, agentid, extensionnum, bgid);
        }
        #endregion

        #region 更新状态明细记录

        public bool UpdateStateDetail2DB(int oid, DateTime endTime)
        {
            return Dal.AgentTimeState.Instance.UpdateStateDetail2DB(oid, endTime);
        }

        /// <summary>
        /// 坐席登录时更新退出时间
        /// </summary>
        /// <param name="agentID"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string UpdateLoginOffTime(string agentID, DateTime endTime)
        {
            return Dal.AgentTimeState.Instance.UpdateLoginOffTime(agentID, endTime);
        }
        #endregion

        /// <summary>
        /// 获取负责分组名字字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userId)
        {
            return Dal.AgentTimeState.Instance.GetUserGroupNamesStr(userId);
        }

        /// <summary>
        /// 根据用户ID获取所属分组及外呼出局号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetBGNameAndOutNum(int userId)
        {
            return Dal.AgentTimeState.Instance.GetBGNameAndOutNum(userId);
        }


        /// <summary>
        /// 获取在线坐席列表（置闲、置忙）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentNumsOnLine(out int[] iarray, out string errormsg)
        {
            return Dal.AgentTimeState.Instance.GetAgentNumsOnLine(out iarray, out errormsg);
        }


        /// <summary>
        /// 获取在线坐席状态列表（置闲、置忙）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentStateOnLine(out DataTable dt, Vender vender, out string errormsg)
        {
            return Dal.AgentTimeState.Instance.GetAgentStateOnLine(out dt, vender, out errormsg);
        }


        /// <summary>
        /// 获取在线坐席状态列表（工作中）
        /// </summary>
        /// <param name="iarray"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public bool GetAgentStateWorking(out DataTable dt, Vender vender, out string errormsg)
        {
            return Dal.AgentTimeState.Instance.GetAgentStateWorking(out dt, vender, out errormsg);
        }

        /// <summary>
        /// 根据userid获取所属业务组id串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupIDsStr(int userId)
        {
            return Dal.AgentTimeState.Instance.GetUserGroupIDsStr(userId);
        }
        /// 通过条件查询置闲或置忙的坐席
        /// <summary>
        /// 通过条件查询置闲或置忙的坐席
        /// </summary>
        /// <param name="vender"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAgentStateOnLineByWhere(Vender vender, string where)
        {
            return Dal.AgentTimeState.Instance.GetAgentStateOnLineByWhere(vender, where);
        }
    }
}
