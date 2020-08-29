using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类AllocationAgent 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-03-05 10:05:58 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AllocationAgent
    {
        #region Instance
        public static readonly AllocationAgent Instance = new AllocationAgent();
        #endregion

        #region Contructor
        protected AllocationAgent()
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
        public DataTable GetAllocationAgent(QueryAllocationAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AllocationAgent.Instance.GetAllocationAgent(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetAllocationList(QueryAllocationAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AllocationAgent.Instance.GetAllocationList(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 根据会话ID获取实体信息
        /// </summary>
        /// <param name="AllocID"></param>
        /// <returns></returns>
        public string GetAllocationAgent(int AllocID)
        {            
            Entities.AllocationAgent model = Dal.AllocationAgent.Instance.GetAllocationAgent(AllocID);
            string tmsg = "";
            if (model != null)
            {
                string endtime = "";
                if (model.UserEndTime != Constant.DATE_INVALID_VALUE)
                {
                    endtime = model.UserEndTime.ToString();
                }
                else if (model.AgentEndTime != Constant.DATE_INVALID_VALUE)
                {
                    endtime = model.AgentEndTime.ToString();
                }

                string talktime = "";
                if (!string.IsNullOrEmpty(endtime) && model.StartTime != Constant.DATE_INVALID_VALUE)
                {
                    DateTime _endtime = System.DateTime.Now;
                    DateTime.TryParse(endtime, out _endtime);

                    TimeSpan ts = _endtime - Convert.ToDateTime(model.StartTime);

                    Int32 min = (Int32)ts.TotalMinutes;
                    int sec = (int)ts.TotalSeconds % 60;

                    if (min < 0)
                        min = 0;

                    if (sec < 0)
                        sec = 0;
                    talktime = min + "分" + sec + "秒";
                }
                
                tmsg += "{'QueueStartTime':'" + model.QueueStartTime + "','StartTime':'" + model.StartTime + "','EndTime':'" + endtime + "','TalkTime':'" + talktime + 
                        "','AgentID':'" + model.UserName + "','Location':'" + model.Location + "','LocalIP':'" + model.LocalIP + "','UserReferURL':'" + model.UserReferURL + "'}";
            }

            return tmsg;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.AllocationAgent.Instance.GetAllocationAgent(new QueryAllocationAgent(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.AllocationAgent GetAllocationAgent(long AllocID)
        {

            return Dal.AllocationAgent.Instance.GetAllocationAgent(AllocID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByAllocID(long AllocID)
        {
            QueryAllocationAgent query = new QueryAllocationAgent();
            query.AllocID = AllocID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAllocationAgent(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Insert(Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long AllocID)
        {

            return Dal.AllocationAgent.Instance.Delete(AllocID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long AllocID)
        {

            return Dal.AllocationAgent.Instance.Delete(sqltran, AllocID);
        }

        #endregion

        public void UpdateEndTime(string userid)
        {
            Dal.AllocationAgent.Instance.UpdateEndTime(userid);
        }
        /// <summary>
        /// 根据坐席id取坐席现在正在聊天的网友人数
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public int SelectCurrentAgentUserCount(string agentid)
        {
            return Dal.AllocationAgent.Instance.SelectCurrentAgentUserCount(agentid);
        }

    }
}

