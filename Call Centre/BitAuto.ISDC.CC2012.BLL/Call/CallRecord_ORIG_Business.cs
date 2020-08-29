using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Web.Caching;
using System.Web;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���CallRecord_ORIG_Business ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-05-27 10:46:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG_Business
    {
        public static readonly CallRecord_ORIG_Business Instance = new CallRecord_ORIG_Business();
        private Random R = new Random();

        protected CallRecord_ORIG_Business()
        { }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG_Business model)
        {
            BLL.Loger.Log4Net.Info("[BLL]InsertCallRecord_ORIG_Business ...���뿪ʼ...CallID:" + model.CallID);
            return Dal.CallRecord_ORIG_Business.Instance.Insert(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Business model)
        {
            return Dal.CallRecord_ORIG_Business.Instance.Update(model);
        }
        /// ��ȡҵ��url
        /// <summary>
        /// ��ȡҵ��url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="Source"></param>
        /// <param name="CarType"></param>
        /// <returns></returns>
        public string GetTaskUrl(string TaskID, string BGID, string SCID)
        {
            try
            {
                if (string.IsNullOrEmpty(TaskID))
                {
                    return "";
                }
                //֧���¾ɹ������������ݿ⣩
                switch (Dal.CallRecord_ORIG_Business.Instance.GetProjectSource(TaskID))
                {
                    case ProjectSource.S2_�ͻ���ʵ:
                        return "/CRMStopCust/View.aspx?TaskID={0}";
                    case ProjectSource.S3_����:
                        string newurl = "/WOrderV2/WorkOrderView.aspx?OrderID={0}"; //����id����17λ
                        string oldurl = "/WorkOrder/WorkOrderView.aspx?OrderID={0}"; //����id����16λ������
                        if (TaskID.Length == 17)
                        {
                            return newurl;
                        }
                        else return oldurl;
                    case ProjectSource.S4_��������:
                        return "/OtherTask/OtherTaskDealView.aspx?OtherTaskID={0}";
                    case ProjectSource.S5_�׼���:
                        return "/LeadsTask/LeadTaskView.aspx?TaskID={0}";
                    case ProjectSource.S6_���Ҽ���:
                        return "/LeadsTask/CSLeadTaskView.aspx?TaskID={0}";
                    case ProjectSource.S7_���Ź�:
                        return "/YTGActivityTask/YTGActivityTaskView.aspx?TaskID={0}";
                }

                string url = "";
                DataTable businessDt = DictionaryDataCache.Instance.GetDataTableByKey(DataCacheKey.BusinessUrL);
                if (BGID != "" && SCID != "" && businessDt != null && businessDt.Rows.Count > 0)
                {
                    string where = "1=1";
                    where += " And BGID='" + CommonFunction.ObjectToInteger(BGID, -1) + "'";
                    where += " And SCID='" + CommonFunction.ObjectToInteger(SCID, -1) + "'";
                    DataRow[] rowList = businessDt.Select(where);
                    if (rowList.Length > 0)
                    {
                        url = rowList[0]["BusinessDetailURL"].ToString();
                    }
                    else
                    {
                        url = "";
                    }
                }
                else
                {
                    url = "";
                }
                return url;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("��ȡurlʱ�����쳣��BLL.CallRecord_ORIG_Business.GetTaskUrl()........" + ex.Message.ToString(), ex);
                return "";
            }
        }

        /// ����һ��URL
        /// <summary>
        /// ����һ��URL
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="webBaseUrl"></param>
        /// <returns></returns>
        public int AddBusinessUrl(int BGID, int SCID, string webBaseUrl)
        {
            return Dal.CallRecord_ORIG_Business.Instance.AddBusinessUrl(BGID, SCID, webBaseUrl);
        }
        /// ɾ��ҵ��url
        /// <summary>
        /// ɾ��ҵ��url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public int DeleteBusinessUrl(int BGID, int SCID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.DeleteBusinessUrl(BGID, SCID);
        }
        /// ����ҵ���飬����ȡurl
        /// <summary>
        /// ����ҵ���飬����ȡurl
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public DataTable GetBusinessUrl(int BGID, int SCID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetBusinessUrl(BGID, SCID);
        }
        /// ����ҵ��鿴ҳ�棬����ҵ����������ȵĻ���
        /// <summary>
        /// ����ҵ��鿴ҳ�棬����ҵ����������ȵĻ���
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="GetViewUrl"></param>
        /// <param name="CreateUserID"></param>
        /// <returns></returns>
        public string GetViewUrl(string BusinessID, string GetViewUrl)
        {
            if (string.IsNullOrEmpty(BusinessID))
            {
                return "";
            }
            GetViewUrl += "&r={1}";
            GetViewUrl = string.Format(GetViewUrl, BusinessID, R.Next(100000));
            return GetViewUrl;
        }
        /// ����CallID������ҵ�����ݣ�businessIDΪҵ��ID��BGIDΪ����ID��SCIDΪ����ID
        /// <summary>
        /// ����CallID������ҵ�����ݣ�businessIDΪҵ��ID��BGIDΪ����ID��SCIDΪ����ID
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <param name="callID"></param>
        /// <param name="businessID"></param>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="createuserid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int UpdateBusinessDataByCallID(Int64 callID, string businessID, int BGID, int SCID, int createuserid, ref string msg)
        {
            msg = "";
            try
            {
                //����������ݣ������ֱ�
                if (BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callID))
                {
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]��ϵͳ���Ѿ�����ҵ�����ݣ����£�callID=" + callID);
                    Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Update(model);
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]������ϣ�callID=" + callID);
                    return recID;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]׼���������ݣ�callID=" + callID);
                    Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Insert(model);
                    model.RecID = recID;
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]������ϣ�callID=" + callID);
                    return recID;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]����callID=" + callID + ",errorMsg:" + msg);
                return -1;
            }
        }
        /// �Ƿ���ڸü�¼-���ڱ�
        /// <summary>
        /// �Ƿ���ڸü�¼-���ڱ�
        /// </summary>
        public bool IsExistsByCallID(Int64 callid)
        {
            return Dal.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callid);
        }
        /// �������ͽ���
        /// <summary>
        /// �������ͽ���
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public ProjectSource GetProjectSource(string businessID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetProjectSource(businessID);
        }

        #region ���²�ѯ
        /// ��ѯ����ҵ������
        /// <summary>
        /// ��ѯ����ҵ������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIG_Business(QueryCallRecord_ORIG_Business query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetCallRecord_ORIG_Business(query, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// ��ѯ����ҵ��ʵ����
        /// <summary>
        /// ��ѯ����ҵ��ʵ����
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG_Business GetByCallID(Int64 CallID, string tableEndName)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetByCallID(CallID, tableEndName);
        }
        #endregion
    }
}

