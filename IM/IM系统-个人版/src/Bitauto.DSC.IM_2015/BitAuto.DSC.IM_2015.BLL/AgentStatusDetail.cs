using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class AgentStatusDetail
    {
        private AgentStatusDetail() { }
        private static AgentStatusDetail instance = null;
        public static AgentStatusDetail Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AgentStatusDetail();
                }
                return instance;
            }
        }

        public int UpdateAgentLastStatus(int nRecID)
        {
            int nResult = 0;
            try
            {
                nResult = Dal.AgentStatusDentail.Instance.UpdateAgentLastStatus(nRecID);
            }
            catch (Exception ex)
            {
                nResult = -1;
                BLL.Loger.Log4Net.Info(string.Format("在Bll函数UpdateAgentLastStatus中发生异常{0}", ex.Message));
            }
            return nResult;

        }

        public int AddAgentStatusData(int UserId, int Status, int RecID)
        {
            return Dal.AgentStatusDentail.Instance.AddAgentStatusData(UserId, Status, RecID);
        }
        /// <summary>
        /// 分组获取对应的业务线id
        /// </summary>
        /// <param name="bgids"></param>
        /// <returns></returns>
        public DataTable GetSourceTypeByBGIDS(string bgids)
        {
            return Dal.AgentStatusDentail.Instance.GetSourceTypeByBGIDS(bgids);
        }
    }
}
