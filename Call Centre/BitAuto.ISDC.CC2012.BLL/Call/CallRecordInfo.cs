using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���CallRecordInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecordInfo
    {
        public static readonly CallRecordInfo Instance = new CallRecordInfo();

        protected CallRecordInfo()
        { }

        #region �ɰ����͸������ϣ������°�CallRecordInfoInfo��ʵ��
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.CallRecordInfo model)
        {
            return Dal.CallRecordInfo.Instance.Insert(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CallRecordInfo model)
        {
            return Dal.CallRecordInfo.Instance.Update(model);
        }
        #endregion

        #region ���²�ѯ
        /// ��ѯ��ȥ������
        /// <summary>
        /// ��ѯ��ȥ������
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="userid">��ǰ�û�ID</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns></returns>
        public DataTable GetCallRecordInfo(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfo(query, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// ��������id��ѯ������Ϣ
        /// <summary>
        /// ��������id��ѯ������Ϣ
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable GetCallRecordByTaskID(string TaskID, string tableEndName)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordByTaskID(TaskID, tableEndName);
        }
        /// ��ȡʵ����
        /// <summary>
        /// ��ȡʵ����
        /// </summary>
        /// <param name="callID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByCallID(long callID, string tableEndName)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoByCallID(callID, tableEndName);
        }
        #endregion

        #region ��������
        /// �ط��б�-���ܷ���
        /// <summary>
        /// �ط��б�-���ܷ���
        /// </summary>
        /// <param name="queryCC_CallRecords"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCC_CallRecordsByRV(QueryCallRecordInfo queryCC_CallRecords, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CallRecordInfo.Instance.GetCC_CallRecordsByRV(queryCC_CallRecords, currentPage, pageSize, out totalCount);
        }
        /// ���ܷ���-RVID��ֵ
        /// <summary>
        /// ���ܷ���-RVID��ֵ
        /// </summary>
        /// <param name="RVID"></param>
        /// <returns></returns>
        public DataTable GetCallRecordByRVID(string RVID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordByRVID(RVID);
        }
        /// ��������ID��ѯʵ����-ֻ�ܲ�ѯ���ڱ�-���ܷ���
        /// <summary>
        /// ��������ID��ѯʵ����-ֻ�ܲ�ѯ���ڱ�-���ܷ���
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByTaskID(string taskID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoByTaskID(taskID);
        }
        /// ���ݿͻ�ID��ѯ-ֻ�ܲ�ѯ���ڱ�-ɾ���ͻ�����ʱ��-���ܷ���
        /// <summary>
        /// ���ݿͻ�ID��ѯ-ֻ�ܲ�ѯ���ڱ�-ɾ���ͻ�����ʱ��-���ܷ���
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public bool HavCarRecordInfoByCustID(string custID)
        {
            return Dal.CallRecordInfo.Instance.HavCarRecordInfoByCustID(custID);
        }
        /// ��Ա��CRM�����ɹ��󣬻�д����CC_CallRecords��
        /// <summary>
        /// ��Ա��CRM�����ɹ��󣬻�д����CC_CallRecords��
        /// </summary>
        /// <param name="cc_DMSMemberID">CCϵͳ�л�ԱID</param>
        /// <param name="memberID">CRMϵͳ�л�ԱID</param>
        public void UpdateOriginalDMSMemberIDByID(int cc_DMSMemberID, string memberID)
        {
            if (Dal.CallRecordInfo.Instance.UpdateOriginalDMSMemberIDByID(cc_DMSMemberID, memberID) > 0)
            {
                string content = "��д��ԱID���ڱ�CC_CallRecords�У���CCMemberID�ֶ�Ϊ:" + cc_DMSMemberID + "��ֵ����ΪNULL��DMSMemberID��Ϊ:" + memberID;
                //������־
                BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("BindRecordLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
            }
        }
        /// �ͻ���CRM�����ɹ��󣬻�д����CC_CallRecords��
        /// <summary>
        /// �ͻ���CRM�����ɹ��󣬻�д����CC_CallRecords��
        /// </summary>
        /// <param name="cc_DMSMemberID">CCϵͳ��Excel����ͻ���ϢID</param>
        /// <param name="memberID">CRMϵͳ�пͻ�ID</param>
        public void UpdateCRMCustIDByID(string cccustid, string custid)
        {
            if (Dal.CallRecordInfo.Instance.UpdateCRMCustIDByID(cccustid, custid) > 0)
            {
                string content = "��д�ͻ�ID���ڱ�CC_CallRecords�У���CCCustID�ֶ�Ϊ:" + cccustid + "��ֵ����ΪNULL��CRMCustID��Ϊ:" + custid;
                //������־
                BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("BindRecordLogModuleID"),
                    (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
            }
        }
        #endregion

        #region ��������
        /// ������������ȡ��ѯ��
        /// <summary>
        /// ������������ȡ��ѯ��
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ANI"></param>
        /// <param name="Agent">��ϯ����</param>
        /// <param name="TaskID"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="AgentNum"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="TaskCategory"></param>
        /// <param name="SpanTime1"></param>
        /// <param name="SpanTime2"></param>
        /// <param name="AgentGroup"></param>
        /// <returns></returns>
        public QueryCallRecordInfo GetQueryModel(string Name, string ANI, string Agent, string TaskID, string CallID,
                     string BeginTime, string EndTime, string AgentNum, string PhoneNum, string TaskCategory,
                     string SpanTime1, string SpanTime2, string AgentGroup, string CallStatus, int loginID,
                     string ownGroup, string oneSelf, string Category, string ivrScore, string icomingSource, string selBusinessType, string SelSolve)
        {

            DateTime tmpDt = new DateTime();
            int tmpInt = 0;
            Int64 tmpInt64 = 0;

            QueryCallRecordInfo query = new QueryCallRecordInfo();

            if (icomingSource != "" && int.TryParse(icomingSource, out tmpInt))
            {
                query.IncomingSource = tmpInt;
            }
            if (ivrScore != "" && ivrScore != "-1" && int.TryParse(ivrScore, out tmpInt))
            {
                query.IVRScore = tmpInt;
            }
            if (Name != "")
            {
                query.CustName = Name;
            }
            if (ANI != "")
            {
                query.ANI = ANI;
            }
            if (Agent != "")
            {
                query.Agent = Agent;
            }
            if (TaskID != "")
            {
                query.TaskID = TaskID;
            }
            if (CallID != "" && Int64.TryParse(CallID, out tmpInt64))
            {
                query.CallID = tmpInt64;
            }
            if (BeginTime != "" && DateTime.TryParse(BeginTime, out tmpDt))
            {
                query.BeginTime = tmpDt;
            }
            if (EndTime != "" && DateTime.TryParse(EndTime, out tmpDt))
            {
                query.EndTime = tmpDt;
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                query.AgentNum = AgentNum;
            }

            if (PhoneNum != "")
            {
                query.PhoneNum = PhoneNum;
            }
            if (TaskCategory != "" && int.TryParse(TaskCategory, out tmpInt))
            {
                query.TaskTypeID = tmpInt;
            }

            if (SpanTime1 != "" && int.TryParse(SpanTime1, out tmpInt))
            {
                query.SpanTime1 = tmpInt;
            }
            if (SpanTime2 != "" && int.TryParse(SpanTime2, out tmpInt))
            {
                query.SpanTime2 = tmpInt;
            }
            if (AgentGroup != "" && int.TryParse(AgentGroup, out tmpInt) && AgentGroup != "-1")
            {
                query.BGID = tmpInt;
            }
            if (Category != "" && int.TryParse(Category, out tmpInt) && Category != "-1")
            {
                query.SCID = tmpInt;
            }
            if (CallStatus != "" && int.TryParse(CallStatus, out tmpInt))
            {
                query.CallStatus = tmpInt;
            }
            if (selBusinessType != "" && selBusinessType != "-1")
            {
                query.selBusinessType = selBusinessType;
            }
            if (SelSolve != "" && SelSolve != "-1")
            {
                query.SelSolve = CommonFunction.ObjectToInteger(SelSolve);
            }

            #region ����Ȩ��
            if (loginID != Constant.INT_INVALID_VALUE)
            {
                query.LoginID = loginID;
            }
            if (ownGroup != string.Empty)
            {
                query.OwnGroup = ownGroup;
            }
            if (oneSelf != string.Empty)
            {
                query.OneSelf = oneSelf;
            }
            #endregion

            return query;
        }
        public string GetViewUrl(string TaskTypeID, string TaskID, string carType, string source)
        {
            string url = "";

            if (TaskTypeID == "6")
            {
                //��������
                url = "/CRMStopCust/Edit.aspx?TaskID=" + TaskID;
            }

            if (TaskTypeID == "5")
            {
                //��������
                url = "/OtherTask/OtherTaskDealView.aspx?OtherTaskID=" + TaskID;
            }

            if (TaskTypeID == "4")
            {
                //�ͻ��ط�
                url = "/ReturnVisit/ReturnVisitRecordView.aspx?RVID=" + TaskID;
            }

            if (TaskTypeID == "3")
            {
                //����ҵ��
                url = "/TaskManager/TaskDetail.aspx?TaskID=" + TaskID;
            }
            else if (TaskTypeID == "2")
            {
                //��������
                url = "/TaskManager/NoDealerOrder/NoDealerOrderView.aspx?TaskID=" + TaskID;
            }
            else if (TaskTypeID == "1")
            {
                //������ϴ

                if (source == "2")
                {
                    //CRM
                    if (carType == "2")
                    {
                        url = "/CustCheck/CrmCustCheck/SecondCarView.aspx?TID=" + TaskID + "&Action=view";
                    }
                    else
                    {
                        url = "/CustCheck/CrmCustCheck/View.aspx?TID=" + TaskID + "&Action=view";
                    }
                }
                else
                {
                    //Excel
                    if (carType == "2")
                    {
                        url = "/CustCheck/NewCustCheck/SecondCarView.aspx?TID=" + TaskID + "&Action=view";
                    }
                    else
                    {
                        url = "/CustCheck/NewCustCheck/View.aspx?TID=" + TaskID + "&Action=view";
                    }
                }
            }


            return url;
        }

        /// ����ϵͳ-�ӿ�
        /// <summary>
        /// ����ϵͳ-�ӿ�
        /// </summary>
        /// <param name="model"></param>
        /// <param name="model_ORIG"></param>
        /// <param name="recID"></param>
        /// <returns></returns>
        public bool InsertCallRecordInfoToHuiMaiChe(CallRecordInfoInfo model, Entities.CallRecord_ORIG model_ORIG, out long recID)
        {
            recID = 0;
            string logDesc = string.Empty;
            if (model_ORIG.EstablishedTime == null)
            {
                logDesc = "��ͨ�绰δ��ͨ";
                BLL.Loger.Log4Net.Info("������ϵͳ�ӿڻ�����á�������ȥ������ɹ�����ʧ�ܡ�" + logDesc);
                return false;
            }

            CallRecordInfoInfo model_RecordInfo = BLL.CallRecordInfo.Instance.GetCallRecordInfoInfo(model.CallID.Value);
            if (model_RecordInfo != null)
            {
                recID = model_RecordInfo.RecID_Value;
                logDesc = " ��CallID��" + model.CallID + "��¼��CallRecordInfo�����Ѵ��ڣ������ٴβ���CallRecordInfo������������" + model_RecordInfo.RecID + "��";
                BLL.Loger.Log4Net.Info("������ϵͳ�ӿڻ�����á�������ȥ������ɹ���" + logDesc);
                return true;
            }
            try
            {
                model.SessionID = model_ORIG.SessionID;
                model.ExtensionNum = model_ORIG.ExtensionNum;
                model.PhoneNum = BLL.Util.HaoMaProcess(model_ORIG.ANI);
                model.ANI = model_ORIG.PhoneNum;
                model.CallStatus = model_ORIG.CallStatus;
                model.BeginTime = model_ORIG.EstablishedTime;
                model.EndTime = model_ORIG.CustomerReleaseTime == null ? model_ORIG.AgentReleaseTime : model_ORIG.CustomerReleaseTime;
                model.TallTime = model_ORIG.TallTime;
                model.AudioURL = model_ORIG.AudioURL;
                model.SkillGroup = model_ORIG.SkillGroup;
                //recordInfo.CallID = long.Parse(info.CallID);// ���渳ֵ
                //recordInfo.SCID = int.Parse(info.SCID);// ���渳ֵ
                //recordInfo.TaskID = info.BusinessID;// ���渳ֵ
                //recordInfo.TaskTypeID = (int)ProjectSource.None;// ���渳ֵ
                //recordInfo.BGID = int.Parse(info.BGID);// ���渳ֵ
                //recordInfo.CustID = custId;// ���渳ֵ
                //recordInfo.CustName = info.CustName;// ���渳ֵ
                //recordInfo.Contact = info.CustName;// ���渳ֵ
                model.CreateTime = DateTime.Now;
                model.CreateUserID = model_ORIG.CreateUserID;
                CommonBll.Instance.InsertComAdoInfo(model);
                recID = model.RecID_Value;
                BLL.Loger.Log4Net.Info("������ϵͳ�ӿڻ�����á�������ȥ������ɹ�������������" + recID);
                return true;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("������ϵͳ�ӿڻ�����á�������ȥ������ɹ���", ex);
                return false;
            }
        }

        /// �õ�һ������ʵ��--ֻ��ѯ���ڱ�
        /// <summary>
        /// �õ�һ������ʵ��--ֻ��ѯ���ڱ�
        /// </summary>
        public Entities.CallRecordInfo GetCallRecordInfo(long RecID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfo(RecID);
        }
        /// ����callid��ȡʵ����-ֻ��ѯ���ڱ�
        /// <summary>
        /// ����callid��ȡʵ����-ֻ��ѯ���ڱ�
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByCallID(long CallID)
        {
            //��ѯ���ڱ�
            return GetCallRecordInfoByCallID(CallID, "");
        }
        /// ����SessionID��ȡʵ����-ֻ��ѯ���ڱ�
        /// <summary>
        /// ����SessionID��ȡʵ����-ֻ��ѯ���ڱ�
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoBySessionID(string SessionID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoBySessionID(SessionID);
        }
        /// ��ȡδ�ɹ�ԭ��
        /// <summary>
        /// ��ȡδ�ɹ�ԭ��
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public string GetNotSuccessReason(long ProjectId)
        {
            return Dal.CallRecordInfo.Instance.GetNotSuccessReason(ProjectId);
        }
        /// ����CallID��ȡ¼��AudioURL
        /// <summary>
        /// ����CallID��ȡ¼��AudioURL
        /// </summary>
        /// <param name="callID"></param>
        /// <returns></returns>
        public string GetAudioURLByCallID(string callID)
        {
            return Dal.CallRecordInfo.Instance.GetAudioURLByCallID(callID);
        }
        #endregion

        #region �°�CallRecordInfoInfo
        /// ��ѯʵ������
        /// <summary>
        /// ��ѯʵ������
        /// </summary>
        /// <param name="callid"></param>
        /// <returns></returns>
        public CallRecordInfoInfo GetCallRecordInfoInfo(long callid)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoInfo(callid);
        }
        #endregion
    }
}

