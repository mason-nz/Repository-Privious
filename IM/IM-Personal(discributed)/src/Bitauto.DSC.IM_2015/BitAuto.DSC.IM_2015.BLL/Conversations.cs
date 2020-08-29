using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���Conversations ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class Conversations
    {
        #region Instance
        public static readonly Conversations Instance = new Conversations();
        #endregion

        #region Contructor
        protected Conversations()
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
        public DataTable GetConversations(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConversations(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ���ղ�ѯ������ѯ�ͷ�ͳ��
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataSet GetConverStatistics(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConverStatistics(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable CheckConversationOrderCustInfo(string csid)
        {
            return Dal.Conversations.Instance.CheckConversationOrderCustInfo(csid);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.Conversations.Instance.GetConversations(new QueryConversations(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.Conversations GetConversations(int CSID)
        {

            return Dal.Conversations.Instance.GetConversations(CSID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByCSID(int CSID)
        {
            QueryConversations query = new QueryConversations();
            query.CSID = CSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetConversations(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Update(model);
        }

        public int CallBackUpdate(Entities.Conversations model)
        {
            return Dal.Conversations.Instance.CallBackUpdate(model);
        }
        public int UpdateConversationReplyTime(DateTime dt, int csid)
        {
            return Dal.Conversations.Instance.UpdateConversationReplyTime(dt, csid);
        }

        ///// <summary>
        ///// ����һ������
        ///// </summary>
        //public int Update(SqlTransaction sqltran, Entities.Conversations model)
        //{
        //    return Dal.Conversations.Instance.Update(sqltran, model);
        //}

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int CSID)
        {

            return Dal.Conversations.Instance.Delete(CSID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {

            return Dal.Conversations.Instance.Delete(sqltran, CSID);
        }

        #endregion


        #region  �Ϸ�  2014-10-31

        //<summary>
        //�жϻỰ�Ƿ����
        //</summary>
        //<param name="CSID">�ỰID</param>
        //<returns></returns>
        public bool Exists(int CSID)
        {
            return Dal.Conversations.Instance.Exists(CSID);
        }
        /// <summary>
        /// ����ָ��������ѯ�Ự����
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCSData(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int currentuserid = BLL.Util.GetLoginUserID();
            query.UserID = currentuserid;
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(currentuserid);
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                query.RightBGIDs = bgids.Substring(1);
            }
            string region = BLL.BaseData.Instance.GetAgentRegionByUserID(currentuserid.ToString());
            int intreatype;
            if (int.TryParse(region, out intreatype))
            {
                query.AreaType = intreatype;
            }
            return Dal.Conversations.Instance.GetCSData(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ����VisitID��ȡ�ͻ���Ϣ
        /// exec p_GetMemberInfoByVisitID ' and VisitID=''001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetMemberInfoByVisitID(string strWhere)
        {
            return Dal.Conversations.Instance.GetMemberInfoByVisitID(strWhere);
        }
        /// <summary>
        /// ����VisitID��ȡ�Ự�����Ϣ
        /// exec p_CSRelateInfoByVisitID ' and a.VisitID=''001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetCSRelateInfoByCSID(string strWhere)
        {
            return Dal.Conversations.Instance.GetCSRelateInfoByCSID(strWhere);
        }
        /// <summary>
        /// ����OrderID��ѯ���������Ϣ
        /// exec p_GetWorkOrderInfoByOrderID ' and a.OrderID = ''WO20130000000001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoByOrderID(string strWhere)
        {
            return Dal.Conversations.Instance.GetWorkOrderInfoByOrderID(strWhere);
        }

        /// <summary>
        /// ����ָ��������ѯ��ʷ��������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetConversationHistoryData(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int currentuserid = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(currentuserid);
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                bgids = bgids.Substring(1);
            }
            string region = BLL.BaseData.Instance.GetAgentRegionByUserID(currentuserid.ToString());
            int intreatype;
            if (int.TryParse(region, out intreatype))
            {
                query.AreaType = intreatype;
            }

            //����Ȩ�޿���(��ǰ��¼��+��ǰ��¼�˵Ĺ�Ͻ����ids)
            if (bgids != Constant.STRING_INVALID_VALUE)
            {
                query.RightBGIDs = " AND (b.UserID = '" + currentuserid + "' OR e.BGID  IN (" + StringHelper.SqlFilter(bgids) + "))";
            }
            else
            {
                query.RightBGIDs = " AND b.UserID = '" + currentuserid + "'";
            }



            return Dal.Conversations.Instance.GetConversationHistoryData(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetConversationingCSData(string strWhere)
        {
            return Dal.Conversations.Instance.GetConversationingCSData(strWhere);
        }
        #endregion

        public DataSet ExportCSDataForLiaoTianJiLu(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(BLL.Util.GetLoginUserID());
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                query.RightBGIDs = bgids.Substring(1);
            }

            return Dal.Conversations.Instance.ExportCSDataForLiaoTianJiLu(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetConversationHistoryDataForCC(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConversationHistoryDataForCC(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetConversationHistoryDataNew(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConversationHistoryDataNew(query, order, currentPage, pageSize, out totalCount);
        }

        public void UpdateConversationTag(string strCsid, string strTagId, string strTagName)
        {
            Dal.Conversations.Instance.UpdateConversationTag(strCsid, strTagId, strTagName);
        }
        public DataTable GetConversationTagData(int BGID)
        {
            return Dal.Conversations.Instance.GetConversationTagData(BGID);
        }

        public void GetChildNode(DataTable dt, string NodeID, ref string leafids)
        {

            DataRow[] rowsArr1 = dt.Select("PID=" + NodeID);
            if (rowsArr1 == null || rowsArr1.Length == 0)
            {
                leafids += "," + NodeID;
            }
            else
            {
                foreach (DataRow row in rowsArr1)
                {
                    DataRow[] rowsArr2 = dt.Select("PID=" + row["RecID"].ToString());
                    if (rowsArr2 == null || rowsArr2.Length == 0)
                    {
                        leafids += "," + row["RecID"].ToString();
                    }
                    else
                    {
                        GetChildNode(dt, row["RecID"].ToString(), ref leafids);
                    }
                }
            }
        }
        /// <summary>
        /// ȡ�ͷ�ͳ������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns>��ͳ�Ʒ��أ���daytime�����ڣ�����ͳ�Ʒ��أ�weekb:�ܿ�ʼ���ڣ�weeke���ܽ������ڣ�����ͳ�Ʒ��أ���monthb:�¿�ʼ���ڣ�monthe:�½������ڣ�����������userid��truename��������agentnum�����ţ�sumonlinetime��������ʱ����SumConversations���ܶԻ�����SumConversationTimeLong���ܶԻ�ʱ����SumFRTimeLong���ܵ��״λظ�ʱ����SumReception���ܵĽӴ�����SumAgentDailog����ϯ����Ϣ������SumNetFriendDailog�����ѷ���Ϣ����</returns>
        public DataTable S_Agent_Total_Select(QueryUserSatisfactionTotal query, string order, int currentPage, int pageSize, out int totalCount, int logUserID)
        {
            return Dal.Conversations.Instance.S_Agent_Total_Select(query, order, currentPage, pageSize, out totalCount, logUserID);
        }

        /// <summary>
        /// ȡ�ͷ�ͳ�ƻ�������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns>sumonlinetime��������ʱ����SumConversations���ܶԻ�����SumConversationTimeLong���ܶԻ�ʱ����SumFRTimeLong���ܵ��״λظ�ʱ����SumReception���ܵĽӴ�����SumAgentDailog����ϯ����Ϣ������SumNetFriendDailog�����ѷ���Ϣ����</returns>
        public DataTable S_Agent_Total_Select(QueryUserSatisfactionTotal query, int logUserID)
        {
            return Dal.Conversations.Instance.S_Agent_Total_Select(query, logUserID);
        }


        /// <summary>
        /// ҵ���߻�������
        /// </summary>
        /// <param name="sourcetype">ҵ���߱�ʶ����-1Ϊ����ҵ���ߣ����ѡ���׳����ܣ���ѡ��������100</param>
        /// <param name="selectType">1�գ�2�ܣ�3�£�4Сʱ</param>
        /// <param name="begintime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <returns>��ͳ�Ʒ��أ���daytime�����ڣ�����ͳ�Ʒ��أ�weekb:�ܿ�ʼ���ڣ�weeke���ܽ������ڣ�����ͳ�Ʒ��أ���monthb:�¿�ʼ���ڣ�monthe:�½������ڣ�������SourceType:ҵ���߱�ʶ��ͨ��bll.util�·���GetSourceTypeNameȡ���ƣ�,SumConversation���ܶԻ���,SumEffective:��Ч�Ի���,SumNoEffective:��Ч�Ի�����</returns>
        public DataTable S_BussinessLine_Total_Select(QueryBussinessLineTotal query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query, order, currentPage, pageSize, out totalCount);

        }

        /// <summary>
        /// ҵ���߻������ݺϼ�
        /// </summary>
        /// <param name="sourcetype">ҵ���߱�ʶ����-1Ϊ����ҵ���ߣ����ѡ���׳����ܣ���ѡ��������100�����ѡ���������ֻ������</param>
        /// <param name="begintime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <returns>SumConversation���ܶԻ���,SumEffective:��Ч�Ի���,SumNoEffective:��Ч�Ի���</returns>
        public DataTable S_BussinessLine_Total_Select(QueryBussinessLineTotal query)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query);
        }


        /// <summary>
        /// ҵ��������ͳ������
        /// </summary>
        /// <param name="sourcetype">ҵ���߱�ʶ����-1Ϊ����ҵ���ߣ����ѡ���׳����ܣ���ѡ��������100</param>
        /// <param name="selectType">1�գ�2�ܣ�3�£�4Сʱ</param>
        /// <param name="begintime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <returns>��ͳ�Ʒ��أ���daytime�����ڣ�����ͳ�Ʒ��أ���weekb:�ܿ�ʼ���ڣ�weeke���ܽ������ڣ�����ͳ�Ʒ��أ���monthb:�¿�ʼ���ڣ�monthe:�½������ڣ���Сʱͳ�Ʒ��أ���hourtime��Сʱ����ʽ0,1,2,������,hourtimename��Сʱ���ƣ���ʽ2015-09-09 17ʱ��18ʱ����������SourceType:ҵ���߱�ʶ��ͨ��bll.util�·���GetSourceTypeNameȡ���ƣ�,SumVisit:ҳ�������,SumConversation���ܶԻ���,SumQueueFail:���з�����,LoginVisit:��¼�ÿ�������NoLoginVisit�������ÿ�������</returns>
        public DataTable S_Flow_Total_Select(QueryBussinessLineTotal query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query, order, currentPage, pageSize, out totalCount);

        }

        /// <summary>
        /// ҵ��������ͳ�����ݺϼ�
        /// </summary>
        /// <param name="sourcetype">ҵ���߱�ʶ����-1Ϊ����ҵ���ߣ����ѡ���׳����ܣ���ѡ��������100�����ѡ���������ֻ������</param>
        /// <param name="begintime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <returns>SumVisit:ҳ�������,SumConversation���ܶԻ���,SumQueueFail:���з�����,LoginVisit:��¼�ÿ�������NoLoginVisit�������ÿ�����</returns>
        public DataTable S_Flow_Total_Select(QueryBussinessLineTotal query)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query);
        }

        /// <summary>
        /// ҵ����������������
        /// </summary>
        /// <param name="sourcetype">ҵ���߱�ʶ����-1Ϊ����ҵ���ߣ����ѡ���׳����ܣ���ѡ��������100</param>
        /// <param name="selectType">1�գ�2�ܣ�3�£�4Сʱ</param>
        /// <param name="begintime">��ʼʱ��</param>
        /// <param name="endtime">����ʱ��</param>
        /// <returns>��ͳ�Ʒ��أ���daytime�����ڣ�����ͳ�Ʒ��أ���weekb:�ܿ�ʼ���ڣ�weeke���ܽ������ڣ�����ͳ�Ʒ��أ���monthb:�¿�ʼ���ڣ�monthe:�½������ڣ���Сʱͳ�Ʒ��أ���hourtime��Сʱ����ʽ0,1,2,������,hourtimename��Сʱ���ƣ���ʽ2015-09-09 17ʱ��18ʱ����������SourceType:ҵ���߱�ʶ��ͨ��bll.util�·���GetSourceTypeNameȡ���ƣ�,SumVisit:ҳ�������,SumConversation���ܶԻ���,SumQueueFail:���з�����,LoginVisit:��¼�ÿ�������NoLoginVisit�������ÿ�����</returns>
        public DataTable S_FlowForMap_Select(QueryBussinessLineTotal query)
        {

            return Dal.Conversations.Instance.S_FlowForMap_Select(query);
        }

        /// <summary>
        /// ���ոſ�ҵ���߻������ݣ���ʼʱ�䣬����ʱ�䴫���죬selecttype��1��currentPage��1��pagesize��100000
        /// </summary>
        /// <returns>SourceType:ҵ���߱�ʶ��ͨ��bll.util�·���GetSourceTypeNameȡ���ƣ�,SumVisit:ҳ�������,SumConversation���ܶԻ���,SumQueueFail:���з�����,SumReception:�Ӵ�����LeaveMessage�����������Ի��еķÿ������Լ��Ŷ��зÿ������������ֶ�ͨ��ҳ��󶨺�̨������ֻ��ͨ��web�����</returns>
        public DataTable Today_Total_Select(QueryBussinessLineTotal query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.Today_Total_Select(query, order, currentPage, pageSize, out totalCount);
        }


        /// <summary>
        /// ���ոſ�ҵ���߻������ݺϼƣ���ʼʱ�䣬����ʱ�䴫���죬selecttype��1��currentPage��1��pagesize��100000
        /// </summary>
        /// <returns>SumVisit:ҳ�������,SumConversation���ܶԻ���,SumQueueFail:���з�����,SumReception:�Ӵ�����LeaveMessage�����������Ի��еķÿ������Լ��Ŷ��зÿ������������ֶ�ͨ��ҳ��󶨺�̨������ֻ��ͨ��web�����</returns>
        public DataTable Today_Total_Select(QueryBussinessLineTotal query)
        {
            return Dal.Conversations.Instance.Today_Total_Select(query);

        }
    }
}

