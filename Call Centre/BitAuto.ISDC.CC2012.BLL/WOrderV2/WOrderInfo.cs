using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// �������
    /// <summary>
    /// �������
    /// </summary>
    public class WOrderInfo
    {
        public static WOrderInfo Instance = new WOrderInfo();
        Random random = new Random();

        /// ����3�����ں����������
        /// <summary>
        /// ����3�����ں����������
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int CalcCallInCountByPhone(string phone)
        {
            return Dal.WOrderInfo.Instance.CalcCallInCountByPhone(phone);
        }
        /// ��ѯ�����û���Ϣ
        /// <summary>
        /// ��ѯ�����û���Ϣ
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public DataTable GetCBInfoByPhone(string custid, string phone)
        {
            return Dal.WOrderInfo.Instance.GetCBInfoByPhone(custid, phone);
        }

        /// ��־��¼
        /// <summary>
        /// ��־��¼
        /// </summary>
        /// <param name="info"></param>
        public static void LogToLog4(string info)
        {
            Loger.Log4Net.Info("[****��������V2****] " + info);
        }
        public static void LogForWebForCall(string msg, int loginuserid, string loginusername)
        {
            BLL.Util.LogForWebForModule("�������", "����", msg, loginuserid, loginusername);
        }
        /// ��־��¼
        /// <summary>
        /// ��־��¼
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ex"></param>
        public static void ErrorToLog4(string name, Exception ex)
        {
            Loger.Log4Net.Error("[****��������V2****] " + name, ex);
        }

        /// ���ɹ���id
        /// <summary>
        /// ���ɹ���id
        /// </summary>
        /// <returns></returns>
        private string GetNewWOrderID()
        {
            string str = "WO" + DateTime.Now.ToString("yyMMddHHmmss") + random.Next(100, 999);
            return str;
        }

        /// ���������Ϣ
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveCustBasicInfo(WOrderJsonData jsondata, int loginuserid)
        {
            //���������������
            Entities.CustBasicInfo model = new Entities.CustBasicInfo();
            model.CustName = jsondata.CustBaseInfo.CBName_Out;
            model.Sex = jsondata.CustBaseInfo.CBSex_Out;
            model.CustCategoryID = (int)jsondata.CustBaseInfo.CustTypeID_Out;
            model.ProvinceID = jsondata.CustBaseInfo.ProvinceID_Out;
            model.CityID = jsondata.CustBaseInfo.CityID_Out;
            model.CountyID = jsondata.CustBaseInfo.CountyID_Out;
            //�����ֶ�
            model.Address = null;
            model.DataSource = null;
            model.CallTime = null;
            model.Status = 0;

            //��ѯ���ݿ�
            string cbid = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(jsondata.CustBaseInfo.Phone_Out);
            if (!string.IsNullOrEmpty(cbid))
            {
                //����
                model.CustID = cbid;
                model.ModifyTime = DateTime.Now;
                model.ModifyUserID = loginuserid;
                BLL.CustBasicInfo.Instance.Update(model);
            }
            else
            {
                //����
                model.CreateTime = DateTime.Now;
                model.CreateUserID = loginuserid;
                model.ModifyTime = null;
                model.ModifyUserID = null;
                cbid = BLL.CustBasicInfo.Instance.Insert(model);

                //����绰��Ϣ
                Entities.CustTel model_Tel = new Entities.CustTel();
                model_Tel.CustID = cbid;
                model_Tel.Tel = jsondata.CustBaseInfo.Phone_Out;
                model_Tel.CreateTime = DateTime.Now;
                model_Tel.CreateUserID = loginuserid;
                BLL.CustTel.Instance.Insert(model_Tel);
            }
            //�洢
            jsondata.CBID = cbid;
            //ɾ����������Ϣ
            BLL.DealerInfo.Instance.Delete(cbid);
            //�洢��������Ϣ
            if (jsondata.CustBaseInfo.CustTypeID_Out == CustTypeEnum.T02_������)
            {
                //���뾭������Ϣ
                Entities.DealerInfo model_Dealer = new Entities.DealerInfo();
                model_Dealer.CustID = cbid;
                model_Dealer.MemberCode = jsondata.CustBaseInfo.MemberCode_Out;
                model_Dealer.Name = jsondata.CustBaseInfo.MemberName_Out;
                model_Dealer.Status = 0;
                model_Dealer.CreateTime = DateTime.Now;
                model_Dealer.CreateUserID = loginuserid;
                BLL.DealerInfo.Instance.Insert(model_Dealer);
            }
        }
        /// ���湤����Ϣ
        /// <summary>
        /// ���湤����Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveWOrderInfo(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            //�������� WOrderInfo
            SaveWOrderMainInfo(jsondata, sysinfo);
            //����WOrderProcess��
            SaveWOrderProcessInfo(jsondata, sysinfo);
            //������ CommonAttachment
            SaveCommonAttachment(jsondata, sysinfo);
        }
        /// ���湤��������Ϣ
        /// <summary>
        /// ���湤��������Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        private void SaveWOrderMainInfo(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            WOrderInfoInfo main = new WOrderInfoInfo();
            main.OrderID = GetNewWOrderID();
            main.CallSource = (int)jsondata.Common.CallSource_Out;
            main.ModuleSource = (int)jsondata.Common.ModuleSource_Out;
            main.DataSource = (int)jsondata.WOrderInfo.DataSource_Out;
            main.WorkOrderStatus = (int)jsondata.OrderStatus;
            main.CategoryID = (int)jsondata.WOrderInfo.Category_Out;
            main.ComplaintLevel = jsondata.WOrderInfo.ComplaintLevel_Out;
            main.BusinessType = jsondata.WOrderInfo.BusinessType_Out;
            main.BusinessTag = jsondata.WOrderInfo.BusinessTag_Out;
            main.IsSyncCRM = jsondata.WOrderInfo.IsSysCRM_Out;
            main.VisitType = jsondata.WOrderInfo.VisitType_Out;
            main.CBID = jsondata.CBID;
            main.Phone = jsondata.CustBaseInfo.Phone_Out;
            main.CRMCustID = jsondata.CRMCustID_Out;
            main.Content = jsondata.WOrderInfo.Content_Out;
            main.ContactName = jsondata.WOrderInfo.ContactName_Out;
            main.ContactTel = jsondata.WOrderInfo.ContactTel_Out;
            main.LastReceiverID = null;
            main.BGID = sysinfo.BGID;
            main.Status = 0;
            main.CreateUserID = sysinfo.UserID;
            main.CreateTime = DateTime.Now;
            main.LastUpdateUserID = sysinfo.UserID;
            main.LastUpdateTime = DateTime.Now;
            CommonBll.Instance.InsertComAdoInfo(main);
            //�洢����id
            jsondata.WOrderID = main.OrderID;
        }
        /// ���湤�������¼
        /// <summary>
        /// ���湤�������¼
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        private void SaveWOrderProcessInfo(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            WOrderProcessInfo process = new WOrderProcessInfo();
            process.ProcessType = (int)jsondata.OperType;
            process.OrderID = jsondata.WOrderID;
            process.WorkOrderStatus = (int)jsondata.OrderStatus;
            process.IsReturnVisit = -1;
            process.ProcessContent = sysinfo.TrueName + jsondata.Oper + "�˹��� [" + jsondata.WOrderID + "]";
            process.Status = 0;
            process.CreateUserID = sysinfo.UserID;
            process.CreateUserNum = sysinfo.UserCode;
            process.CreateUserName = sysinfo.TrueName;
            process.CreateUserDeptName = sysinfo.MainDepartName;
            process.CreateTime = DateTime.Now;
            CommonBll.Instance.InsertComAdoInfo(process);
        }
        /// ���渽����Ϣ
        /// <summary>
        /// ���渽����Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        private void SaveCommonAttachment(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            if (jsondata.WOrderInfo.Attachment != null)
            {
                foreach (AttachmentJsonData item in jsondata.WOrderInfo.Attachment)
                {
                    CommonAttachmentInfo attach = new CommonAttachmentInfo();
                    attach.BTypeID = (int)BLL.Util.ProjectTypePath.WorkOrder;
                    attach.RelatedID = jsondata.WOrderID;
                    attach.FileName = item.FileRealName_Out;
                    attach.FileType = item.FileType_Out;
                    attach.FileSize = item.FileSize_Out;
                    attach.FilePath = item.FileAllPath_Out;
                    attach.Status = 0;
                    attach.CreateUserID = sysinfo.UserID;
                    attach.CreateTime = DateTime.Now;
                    CommonBll.Instance.InsertComAdoInfo(attach);
                }
            }
        }

        /// �������Ϣ
        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveBindInfo(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            try
            {
                LogForWebForCall("[�������Ϣ]Start", loginuserid, loginusername);
                //��ȥ��� CallRecordInfo������ҵ��� CallRecord_ORIG_Business�������������ݱ� WOrderData�����ű� SMSSendHistory
                //������Ϣ
                if (jsondata.CustBaseInfo.CallIds_Out != null && jsondata.CustBaseInfo.CallIds_Out.Count > 0)
                {
                    foreach (string callid in jsondata.CustBaseInfo.CallIds_Out)
                    {
                        long callid_val = CommonFunction.ObjectToLong(callid);
                        //�����������ݱ� WOrderData
                        BLL.WOrderData.Instance.InsertWOrderDataForCC(jsondata.WOrderID, -1, callid_val, loginuserid);
                        LogForWebForCall("[�������Ϣ]�����������ݱ�����ID=" + jsondata.WOrderID + "������ID=" + callid_val, loginuserid, loginusername);
                    }
                    string callids = string.Join(",", jsondata.CustBaseInfo.CallIds_Out.ToArray());
                    //��ȥ��� CallRecordInfo (CustID,CustName,TaskTypeID,TaskID)
                    BLL.CallAfterTaskProcess.Instance.UpdateCallRecordInfoAfterTask(callids, jsondata.CBID, jsondata.CustBaseInfo.CBName_Out, ProjectSource.S3_����, jsondata.WOrderID);
                    //����ҵ��� CallRecord_ORIG_Business (BusinessID)
                    BLL.CallAfterTaskProcess.Instance.UpdateCallRecord_ORIG_BusinessAfterTask(callids, jsondata.WOrderID);

                    LogForWebForCall("[�������Ϣ]��ȥ���ͻ���ҵ�������ID=" + jsondata.WOrderID + "������ID=" + callids + "��CBID=" + jsondata.CBID, loginuserid, loginusername);
                }
                //�Ի���Ϣ
                if ((jsondata.Common.CallSource_Out == CallSourceEnum.C03_IM�Ի� || jsondata.Common.CallSource_Out == CallSourceEnum.C04_IM����)
                    && jsondata.Common.RelatedCSID > 0)
                {
                    //�����������ݱ� WOrderData
                    BLL.WOrderData.Instance.InserWOrderDataForIM(jsondata.WOrderID, -1, jsondata.Common.RelatedCSID, loginuserid);
                    LogForWebForCall("[�������Ϣ]�����������ݱ�����ID=" + jsondata.WOrderID + "���ỰID=" + jsondata.Common.RelatedCSID, loginuserid, loginusername);
                }
                //δ������
                if (jsondata.Common.ModuleSource_Out == ModuleSourceEnum.M04_δ������ && jsondata.Common.RelatedMissID > 0)
                {
                    //���湤��id��cbid��δ������� CustomerVoiceMsg
                    BLL.CustomerVoiceMsg.Instance.UpdateCustomerVoiceMsgInfoAfterNewOrder(jsondata.Common.RelatedMissID, jsondata.WOrderID, jsondata.CBID);
                    LogForWebForCall("[�������Ϣ]���湤��id��cbid��δ�������RecID=" + jsondata.Common.RelatedMissID, loginuserid, loginusername);
                }
                //������Ϣ
                if (jsondata.CustBaseInfo.SmsIds_Out != null && jsondata.CustBaseInfo.SmsIds_Out.Count > 0)
                {
                    string smsids = string.Join(",", jsondata.CustBaseInfo.SmsIds_Out.ToArray());
                    //���ű� SMSSendHistory (CustID,CRMCustID,TaskType,TaskID)
                    BLL.CallAfterTaskProcess.Instance.UpdateSMSSendHistoryAfterTask(smsids, jsondata.CBID, jsondata.CRMCustID_Out, ProjectSource.S3_����, jsondata.WOrderID);
                    LogForWebForCall("[�������Ϣ]���ű� SMSSendHistory��smsids=" + smsids, loginuserid, loginusername);
                }
            }
            catch (Exception ex)
            {
                LogForWebForCall("[�������Ϣ]�������Ϣ " + ex.Message + ex.StackTrace, loginuserid, loginusername);
            }
            LogForWebForCall("[�������Ϣ]END\r\n", loginuserid, loginusername);
        }
        /// �������Ϣ
        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveBindInfoASync(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            Action<WOrderJsonData, int, string> func = new Action<WOrderJsonData, int, string>(SaveBindInfo);
            func.BeginInvoke(jsondata, loginuserid, loginusername, null, null);
        }

        ///  ������־����Ϣ
        /// <summary>
        ///  ������־����Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveLogsInfo(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            try
            {
                LogForWebForCall("[������־����Ϣ]Start", loginuserid, loginusername);
                //������ʱ� CustPhoneVisitBusiness
                CustPhoneVisitBusinessInfo visitinfo = new CustPhoneVisitBusinessInfo();
                visitinfo.PhoneNum = jsondata.CustBaseInfo.Phone_Out;
                visitinfo.TaskID = jsondata.WOrderID;
                visitinfo.BusinessType = (int)VisitBusinessTypeEnum.S1_����; //�������ͣ�-1 ������ 0 ������CCϵͳ ��1������  3���ͻ���ʵ  4����������  5��YJK 6��CJK  7:���Ź� 
                visitinfo.TaskSource = (int)jsondata.Common.CallSource_Out; //������Դ 0��δ֪ 1������ 2������ 3,4��IM 
                visitinfo.CallID = jsondata.CustBaseInfo.CallIds_Out != null && jsondata.CustBaseInfo.CallIds_Out.Count > 0 ?
                    CommonFunction.ObjectToLong(jsondata.CustBaseInfo.CallIds_Out[jsondata.CustBaseInfo.CallIds_Out.Count - 1]) : -1;//���һ�λ���ID
                BLL.CustPhoneVisitBusiness.Instance.InsertOrUpdateCustPhoneVisitBusiness(visitinfo, loginuserid);
                LogForWebForCall("[������־����Ϣ]���ʼ�¼���绰=" + jsondata.CustBaseInfo.Phone_Out + "������ID=" + jsondata.WOrderID, loginuserid, loginusername);

                //�������� CallResult_ORIG_Task
                if (jsondata.WOrderInfo.IsHuifang && jsondata.WOrderInfo.IsJxs)
                {
                    BLL.CallResult_ORIG_Task.Instance.InseretOrUpdateOneData(jsondata.WOrderID, -1, ProjectSource.S3_����,
                        jsondata.WOrderInfo.IsJieTong_Out, jsondata.WOrderInfo.NoJtReason_Out, -1, -1);
                    LogForWebForCall("[������־����Ϣ]�������� CallResult_ORIG_Task������ID=" + jsondata.WOrderID
                        + "�����=" + jsondata.WOrderInfo.IsJieTong_Out + "-" + jsondata.WOrderInfo.NoJtReason_Out, loginuserid, loginusername);
                }
            }
            catch (Exception ex)
            {
                LogForWebForCall("[������־����Ϣ]������־����Ϣ " + ex.Message + ex.StackTrace, loginuserid, loginusername);
            }
            LogForWebForCall("[������־����Ϣ]END\r\n", loginuserid, loginusername);
        }
        ///  ������־����Ϣ
        /// <summary>
        ///  ������־����Ϣ
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveLogsInfoASync(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            Action<WOrderJsonData, int, string> func = new Action<WOrderJsonData, int, string>(SaveLogsInfo);
            func.BeginInvoke(jsondata, loginuserid, loginusername, null, null);
        }

        /// ���¹���״̬
        /// <summary>
        /// ���¹���״̬
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="status"></param>
        /// <param name="receiverid"></param>
        /// <param name="loginuserid"></param>
        /// <returns></returns>
        public bool UpdateWOrderInfoStatus(string orderid, int status, int receiverid, int loginuserid)
        {
            WOrderInfoInfo info = GetWOrderInfoInfo(orderid);
            if (info == null)
                return false;
            info.WorkOrderStatus = status;
            info.LastReceiverID = receiverid;
            info.LastUpdateUserID = loginuserid;
            info.LastUpdateTime = DateTime.Now;
            return CommonBll.Instance.UpdateComAdoInfo(info);
        }
        /// ��ѯ����
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public WOrderInfoInfo GetWOrderInfoInfo(string orderid)
        {
            return Dal.WOrderInfo.Instance.GetWOrderInfoInfo(orderid);
        }

        /// ���ղ�ѯ������ѯ(��������Ȩ��)
        /// <summary>
        /// ���ղ�ѯ������ѯ(��������Ȩ��)
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetWorkOrderInfoForList(QueryWOrderV2DataInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WOrderInfo.Instance.GetWorkOrderInfoForList(query, order, currentPage, pageSize, out totalCount);
        }
        /// ������¼�������ݲ�ѯ
        /// <summary>
        /// ������¼�������ݲ�ѯ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetExportWorkOrderList(QueryWOrderV2DataInfo query, string order)
        {
            return Dal.WOrderInfo.Instance.GetExportWorkOrderList(query, order);
        }
        /// ��ѯ�¾ɹ���-�ͻ��ط�
        /// <summary>
        /// ��ѯ�¾ɹ���-�ͻ��ط�
        /// </summary>
        /// <param name="crmcustid"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoForV1V2Info(string crmcustid, int loginuserid, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WOrderInfo.Instance.GetWorkOrderInfoForV1V2Info(crmcustid, loginuserid, currentPage, pageSize, out  totalCount);
        }

        /// ��ѯCRM���ʷ���
        /// <summary>
        /// ��ѯCRM���ʷ���
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetCrmVistType()
        {
            return Dal.WOrderInfo.Instance.GetCrmVistType();
        }
    }
}
