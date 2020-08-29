using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class WOrderProcess
    {
        public static WOrderProcess Instance = new WOrderProcess();

        /// ���ݹ���ID��ȡ�����¼
        /// <summary>
        /// ���ݹ���ID��ȡ�����¼
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public List<WOrderProcessInfo> GetWOrderProcessByOrderID(string orderID)
        {
            return Dal.WOrderProcess.Instance.GetWOrderProcessByOrderID(orderID);
        }
        /// �Ƿ��Ѿ��ط�
        /// <summary>
        /// �Ƿ��Ѿ��ط�
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool IsHasReturnForProcess(string orderid)
        {
            return Dal.WOrderProcess.Instance.IsHasReturnForProcess(orderid);
        }
        /// ����Ȩ����֤
        /// <summary>
        /// ����Ȩ����֤
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="message"></param>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool ValidateWOrderProcessRight(string orderid, ref string message, ref WOrderOperTypeEnum oper, out WOrderInfoInfo info, WOrderProcessRightJsonData right)
        {
            message = "";
            oper = WOrderOperTypeEnum.None;
            info = null;
            if (string.IsNullOrEmpty(orderid))
            {
                message = "�����������ID����ȷ��";
                return false;
            }
            info = BLL.WOrderInfo.Instance.GetWOrderInfoInfo(orderid);
            if (info == null)
            {
                message = "�鲻����Ӧ�Ĺ������ݣ�";
                return false;
            }
            oper = ValidateWOrderProcessRight(orderid, info.WorkOrderStatus_Value, info.LastReceiverID_Value, info.CreateUserID_Value, ref message, right);
            return oper != WOrderOperTypeEnum.None;
        }
        ///  ����Ȩ����֤
        /// <summary>
        ///  ����Ȩ����֤
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="orderstatus"></param>
        /// <param name="lastrecid"></param>
        /// <param name="ordercreateuserid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public WOrderOperTypeEnum ValidateWOrderProcessRight(string orderid, int orderstatus, int lastrecid, int ordercreateuserid, ref string message, WOrderProcessRightJsonData right = null)
        {
            message = "";
            WOrderOperTypeEnum oper = WOrderOperTypeEnum.None;
            //��¼id
            int loginuserid = BLL.Util.GetLoginUserID();
            //����Ȩ����֤��ʽ��Ĭ�ϣ����õ�ǰ��¼�˵�id��֤��
            if (right == null || Enum.IsDefined(typeof(WOrderProcessRightTypeEnum), right.RightType_Out) == false)
            {
                right = new WOrderProcessRightJsonData()
                {
                    RightType = (int)WOrderProcessRightTypeEnum.R01_��ԱID,
                    RightData = "",
                    LoginUserID = loginuserid,
                    OrderID = orderid
                };
            }
            else
            {
                //У��right��Ч��
                if (right.OrderID != orderid || right.LoginUserID != loginuserid)
                {
                    message = "�����е�Ȩ��������Ч�Բ���ȷ��";
                    return oper;
                }
            }
            /// ���Ȩ��
            bool PendingRight = BLL.Util.CheckRight(loginuserid, "SYS024BUT102102B");
            /// ����Ȩ��
            bool ProcessRight = BLL.Util.CheckRight(loginuserid, "SYS024BUT102102C");
            /// �ط�Ȩ��
            bool ReturnRight = BLL.Util.CheckRight(loginuserid, "SYS024BUT102102D");

            if (string.IsNullOrEmpty(orderid))
            {
                message = "�����������ID����ȷ��";
                return oper;
            }
            WorkOrderStatus status = (WorkOrderStatus)Enum.Parse(typeof(WorkOrderStatus), orderstatus.ToString());
            switch (status)
            {
                case WorkOrderStatus.Pending:
                    //���
                    {
                        if (PendingRight)
                        {
                            oper = WOrderOperTypeEnum.L03_���;
                            return oper;
                        }
                        else
                        {
                            message = "��û�д˹��������Ȩ�ޣ�";
                            return oper;
                        }
                    }
                case WorkOrderStatus.Untreated:
                case WorkOrderStatus.Processing:
                    //����Ȩ��
                    {
                        //��Ȩ��+�Ǵ�����+�Ƿ���һ�������¼�ƶ��Ľ�����
                        if (ProcessRight ||
                            loginuserid == ordercreateuserid ||
                            BLL.WOrderToAndCC.Instance.IsToPersonForNumber(orderid, lastrecid, right))
                        {
                            oper = WOrderOperTypeEnum.L04_����;
                            return oper;
                        }
                        else
                        {
                            message = "��û�д˹����Ĵ���Ȩ�ޣ�";
                            return oper;
                        }
                    }
                case WorkOrderStatus.Processed:
                    //�ط�Ȩ��
                    {
                        if (ReturnRight)
                        {
                            if (BLL.WOrderProcess.Instance.IsHasReturnForProcess(orderid) == false)
                            {
                                oper = WOrderOperTypeEnum.L05_�ط�;
                                return oper;
                            }
                            else
                            {
                                message = "�˹����Ѿ��طù��ˣ�";
                                return oper;
                            }
                        }
                        else
                        {
                            message = "��û�д˹����Ļط�Ȩ�ޣ�";
                            return oper;
                        }
                    }
                case WorkOrderStatus.Completed:
                    message = "�޷���������ɵĹ�����";
                    return oper;
                case WorkOrderStatus.Closed:
                    message = "�޷������ѹرյĹ�����";
                    return oper;
                default:
                    message = "����״̬����ȷ��";
                    return oper;
            }
        }

        /// ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        public void WOrderProcessMain(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderOperTypeEnum oper, WOrderInfoInfo worderinfo)
        {
            //���湤�������¼
            SaveWOrderProcess(jsondata, sysinfo, oper, worderinfo.OrderID_Value);
            //���湤������
            SaveCommonAttachment(jsondata, sysinfo);
            //���湤������
            SaveWOrderData(jsondata, sysinfo, worderinfo.OrderID_Value);
            //������ճ�����
            SaveToAndCC(jsondata, sysinfo, worderinfo.OrderID_Value);
            //���湤������״̬+����id
            SaveWOrderInfo(jsondata, sysinfo, worderinfo);
            //�����ʼ�-�첽
            SendEMail(jsondata, sysinfo, worderinfo);
        }
        /// ���湤�������¼
        /// <summary>
        /// ���湤�������¼
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        public void SaveWOrderProcess(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderOperTypeEnum oper, string orderid)
        {
            WOrderProcessInfo process = new WOrderProcessInfo();
            process.ProcessType = (int)oper;
            process.OrderID = orderid;
            process.WorkOrderStatus = (int)jsondata.WorkOrderStatus_Out;
            process.IsReturnVisit = jsondata.IsReturnVisit_Out;
            process.ProcessContent = jsondata.ProcessContent_Out;
            process.Status = 0;
            process.CreateUserID = sysinfo.UserID;
            process.CreateUserNum = sysinfo.UserCode;
            process.CreateUserName = sysinfo.TrueName;
            process.CreateUserDeptName = sysinfo.MainDepartName;
            process.CreateTime = DateTime.Now;
            CommonBll.Instance.InsertComAdoInfo(process);
            jsondata.ProcessID = process.RecID_Value;
        }
        /// ���渽��
        /// <summary>
        /// ���渽��
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        public void SaveCommonAttachment(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo)
        {
            if (jsondata.imgData != null)
            {
                foreach (AttachmentJsonData item in jsondata.imgData)
                {
                    CommonAttachmentInfo attach = new CommonAttachmentInfo();
                    attach.BTypeID = (int)BLL.Util.ProjectTypePath.WorkOrder;
                    attach.RelatedID = jsondata.ProcessID.ToString();
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
        /// ���滰��
        /// <summary>
        /// ���滰��
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        public void SaveWOrderData(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, string orderid)
        {
            if (jsondata.CallID_Out != null && jsondata.CallID_Out.Count > 0)
            {
                foreach (long callid in jsondata.CallID_Out)
                {
                    //�����������ݱ� WOrderData
                    BLL.WOrderData.Instance.InsertWOrderDataForCC(orderid, jsondata.ProcessID, callid, sysinfo.UserID);
                }
            }
        }
        /// ������ճ�����
        /// <summary>
        /// ������ճ�����
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        /// <param name="orderid"></param>
        public void SaveToAndCC(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, string orderid)
        {
            List<string> hasnum = new List<string>();
            if (jsondata.Recevicer != null && jsondata.Recevicer.Count > 0)
            {
                foreach (ToAndCcPerson person in jsondata.Recevicer)
                {
                    if (!string.IsNullOrEmpty(person.UserNum_Out) && hasnum.Contains(person.UserNum_Out) == false)
                    {
                        BLL.WOrderToAndCC.Instance.SaveWOrderToAndCC(orderid, jsondata.ProcessID, WOrderPersonTypeEnum.P01_������,
                            person.UserID_Out, person.UserNum_Out, person.UserName_Out, sysinfo.UserID);
                        hasnum.Add(person.UserNum_Out);
                    }
                }
            }

            hasnum = new List<string>();
            if (jsondata.ExtendRecev != null && jsondata.ExtendRecev.Count > 0)
            {
                foreach (ToAndCcPerson person in jsondata.ExtendRecev)
                {
                    if (!string.IsNullOrEmpty(person.UserNum_Out) && hasnum.Contains(person.UserNum_Out) == false)
                    {
                        BLL.WOrderToAndCC.Instance.SaveWOrderToAndCC(orderid, jsondata.ProcessID, WOrderPersonTypeEnum.P02_������,
                            person.UserID_Out, person.UserNum_Out, person.UserName_Out, sysinfo.UserID);
                        hasnum.Add(person.UserNum_Out);
                    }
                }
            }
        }
        /// ���湤������
        /// <summary>
        /// ���湤������
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        /// <param name="worderinfo"></param>
        public void SaveWOrderInfo(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderInfoInfo worderinfo)
        {
            worderinfo.WorkOrderStatus = (int)jsondata.WorkOrderStatus_Out;
            worderinfo.LastReceiverID = jsondata.ProcessID;
            worderinfo.LastUpdateTime = DateTime.Now;
            worderinfo.LastUpdateUserID = sysinfo.UserID;
            CommonBll.Instance.UpdateComAdoInfo(worderinfo);
        }

        /// �����ʼ��������˺ͳ�����
        /// <summary>
        /// �����ʼ��������˺ͳ�����
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        /// <param name="worderinfo"></param>
        public void SendEMailAsync(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderInfoInfo worderinfo)
        {
            Action<WOrderProcessJsonData, SysRightUserInfo, WOrderInfoInfo> action = new Action<WOrderProcessJsonData, SysRightUserInfo, WOrderInfoInfo>(SendEMail);
            action.BeginInvoke(jsondata, sysinfo, worderinfo, null, null);
        }
        /// �����ʼ��������˺ͳ�����
        /// <summary>
        /// �����ʼ��������˺ͳ�����
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        /// <param name="worderinfo"></param>        
        public void SendEMail(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderInfoInfo worderinfo)
        {
            try
            {
                //״̬У��
                WorkOrderStatus status = (WorkOrderStatus)worderinfo.WorkOrderStatus_Value;
                if (status == WorkOrderStatus.Pending || status == WorkOrderStatus.Processing || status == WorkOrderStatus.Untreated)
                {
                    //��ѯ�ʼ���ַ
                    string[] ccUserID = jsondata.Recevicer.Select(x => x.UserID_Out.ToString()).ToArray();
                    string[] toUserID = jsondata.ExtendRecev.Select(x => x.UserID_Out.ToString()).ToArray();
                    List<SysRightUserInfo> cc_sysinfos = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(string.Join(",", ccUserID));
                    List<SysRightUserInfo> to_sysinfos = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(string.Join(",", toUserID));
                    //����
                    string[] cc_email = cc_sysinfos.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    string[] to_email = to_sysinfos.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    //��ѯ�����û���Ϣ
                    CustTypeEnum ctype = CustTypeEnum.T01_����;
                    DataTable dt = BLL.WOrderInfo.Instance.GetCBInfoByPhone(worderinfo.CBID_Value, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ctype = (CustTypeEnum)CommonFunction.ObjectToInteger(dt.Rows[0]["CustCategoryID"]);
                    }
                    //��ȡ��������
                    WOrderCategoryEnum wtype = (WOrderCategoryEnum)worderinfo.CategoryID_Value;
                    //��������
                    //cc_email = new string[] { "weisz@yiche.com", "liuying7@yiche.com", "qiangfei@yiche.com", "masj@yiche.com" };
                    //to_email = new string[] { "weisz@yiche.com", "liuying7@yiche.com", "qiangfei@yiche.com", "masj@yiche.com" };
                    //�ʼ�����1
                    string body1 = "����һ��<strong style='color:red;'>" + BLL.Util.GetEnumOptText(typeof(CustTypeEnum), (int)ctype) +
                        BLL.Util.GetEnumOptText(typeof(WOrderCategoryEnum), (int)wtype) + "</strong>������";
                    //�ʼ�����2
                    string body2 = worderinfo.Content_Value;
                    //ccϵͳ��ַ
                    string weburl = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress");
                    //���ʼ�
                    if (cc_email.Length > 0)
                    {
                        WOrderInfo.LogToLog4("�����������ʼ���������" + string.Join(";", cc_email));
                        string title = "����һ�Ź���[" + worderinfo.OrderID_Value + "]������";
                        string a = "<a href='" + weburl + "/WOrderV2/WOrderProcess.aspx?OrderID=" + worderinfo.OrderID_Value + "'>��������</a>";
                        EmailHelper.Instance.SendMail(title, cc_email, new string[] { body1, body2, a }, "WOrderV2");
                    }
                    if (to_email.Length > 0)
                    {
                        WOrderInfo.LogToLog4("�����������ʼ���������" + string.Join(";", to_email));
                        string title = "����һ�Ź���[" + worderinfo.OrderID_Value + "]���鿴";
                        string a = "<a href='" + weburl + "/WOrderV2/WorkOrderView.aspx?OrderID=" + worderinfo.OrderID_Value + "'>�����鿴</a>";
                        EmailHelper.Instance.SendMail(title, to_email, new string[] { body1, body2, a }, "WOrderV2");
                    }
                }
            }
            catch (Exception ex)
            {
                WOrderInfo.ErrorToLog4("�����������ʼ��쳣", ex);
            }
        }
    }
}
