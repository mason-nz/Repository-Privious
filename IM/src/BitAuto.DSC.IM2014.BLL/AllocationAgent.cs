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
    /// ҵ���߼���AllocationAgent ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetAllocationAgent(QueryAllocationAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AllocationAgent.Instance.GetAllocationAgent(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetAllocationList(QueryAllocationAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AllocationAgent.Instance.GetAllocationList(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ���ݻỰID��ȡʵ����Ϣ
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
                    talktime = min + "��" + sec + "��";
                }
                
                tmsg += "{'QueueStartTime':'" + model.QueueStartTime + "','StartTime':'" + model.StartTime + "','EndTime':'" + endtime + "','TalkTime':'" + talktime + 
                        "','AgentID':'" + model.UserName + "','Location':'" + model.Location + "','LocalIP':'" + model.LocalIP + "','UserReferURL':'" + model.UserReferURL + "'}";
            }

            return tmsg;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.AllocationAgent.Instance.GetAllocationAgent(new QueryAllocationAgent(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.AllocationAgent GetAllocationAgent(long AllocID)
        {

            return Dal.AllocationAgent.Instance.GetAllocationAgent(AllocID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
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
        /// ����һ������
        /// </summary>
        public long Insert(Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.AllocationAgent model)
        {
            return Dal.AllocationAgent.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long AllocID)
        {

            return Dal.AllocationAgent.Instance.Delete(AllocID);
        }

        /// <summary>
        /// ɾ��һ������
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
        /// ������ϯidȡ��ϯ���������������������
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public int SelectCurrentAgentUserCount(string agentid)
        {
            return Dal.AllocationAgent.Instance.SelectCurrentAgentUserCount(agentid);
        }

    }
}

