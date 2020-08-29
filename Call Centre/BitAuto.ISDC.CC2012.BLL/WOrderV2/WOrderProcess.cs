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

        /// 根据工单ID获取处理记录
        /// <summary>
        /// 根据工单ID获取处理记录
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public List<WOrderProcessInfo> GetWOrderProcessByOrderID(string orderID)
        {
            return Dal.WOrderProcess.Instance.GetWOrderProcessByOrderID(orderID);
        }
        /// 是否已经回访
        /// <summary>
        /// 是否已经回访
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool IsHasReturnForProcess(string orderid)
        {
            return Dal.WOrderProcess.Instance.IsHasReturnForProcess(orderid);
        }
        /// 处理权限验证
        /// <summary>
        /// 处理权限验证
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
                message = "传入参数工单ID不正确！";
                return false;
            }
            info = BLL.WOrderInfo.Instance.GetWOrderInfoInfo(orderid);
            if (info == null)
            {
                message = "查不到对应的工单数据！";
                return false;
            }
            oper = ValidateWOrderProcessRight(orderid, info.WorkOrderStatus_Value, info.LastReceiverID_Value, info.CreateUserID_Value, ref message, right);
            return oper != WOrderOperTypeEnum.None;
        }
        ///  处理权限验证
        /// <summary>
        ///  处理权限验证
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
            //登录id
            int loginuserid = BLL.Util.GetLoginUserID();
            //处理权限验证方式（默认：采用当前登录人的id验证）
            if (right == null || Enum.IsDefined(typeof(WOrderProcessRightTypeEnum), right.RightType_Out) == false)
            {
                right = new WOrderProcessRightJsonData()
                {
                    RightType = (int)WOrderProcessRightTypeEnum.R01_人员ID,
                    RightData = "",
                    LoginUserID = loginuserid,
                    OrderID = orderid
                };
            }
            else
            {
                //校验right有效性
                if (right.OrderID != orderid || right.LoginUserID != loginuserid)
                {
                    message = "参数中的权限数据有效性不正确！";
                    return oper;
                }
            }
            /// 审核权限
            bool PendingRight = BLL.Util.CheckRight(loginuserid, "SYS024BUT102102B");
            /// 处理权限
            bool ProcessRight = BLL.Util.CheckRight(loginuserid, "SYS024BUT102102C");
            /// 回访权限
            bool ReturnRight = BLL.Util.CheckRight(loginuserid, "SYS024BUT102102D");

            if (string.IsNullOrEmpty(orderid))
            {
                message = "传入参数工单ID不正确！";
                return oper;
            }
            WorkOrderStatus status = (WorkOrderStatus)Enum.Parse(typeof(WorkOrderStatus), orderstatus.ToString());
            switch (status)
            {
                case WorkOrderStatus.Pending:
                    //审核
                    {
                        if (PendingRight)
                        {
                            oper = WOrderOperTypeEnum.L03_审核;
                            return oper;
                        }
                        else
                        {
                            message = "您没有此工单的审核权限！";
                            return oper;
                        }
                    }
                case WorkOrderStatus.Untreated:
                case WorkOrderStatus.Processing:
                    //处理权限
                    {
                        //有权限+是创建人+是否上一步处理记录制定的接收人
                        if (ProcessRight ||
                            loginuserid == ordercreateuserid ||
                            BLL.WOrderToAndCC.Instance.IsToPersonForNumber(orderid, lastrecid, right))
                        {
                            oper = WOrderOperTypeEnum.L04_处理;
                            return oper;
                        }
                        else
                        {
                            message = "您没有此工单的处理权限！";
                            return oper;
                        }
                    }
                case WorkOrderStatus.Processed:
                    //回访权限
                    {
                        if (ReturnRight)
                        {
                            if (BLL.WOrderProcess.Instance.IsHasReturnForProcess(orderid) == false)
                            {
                                oper = WOrderOperTypeEnum.L05_回访;
                                return oper;
                            }
                            else
                            {
                                message = "此工单已经回访过了！";
                                return oper;
                            }
                        }
                        else
                        {
                            message = "您没有此工单的回访权限！";
                            return oper;
                        }
                    }
                case WorkOrderStatus.Completed:
                    message = "无法处理已完成的工单！";
                    return oper;
                case WorkOrderStatus.Closed:
                    message = "无法处理已关闭的工单！";
                    return oper;
                default:
                    message = "工单状态不正确！";
                    return oper;
            }
        }

        /// 工单处理
        /// <summary>
        /// 工单处理
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        public void WOrderProcessMain(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderOperTypeEnum oper, WOrderInfoInfo worderinfo)
        {
            //保存工单处理记录
            SaveWOrderProcess(jsondata, sysinfo, oper, worderinfo.OrderID_Value);
            //保存工单附件
            SaveCommonAttachment(jsondata, sysinfo);
            //保存工单话务
            SaveWOrderData(jsondata, sysinfo, worderinfo.OrderID_Value);
            //保存接收抄送人
            SaveToAndCC(jsondata, sysinfo, worderinfo.OrderID_Value);
            //保存工单主表状态+处理id
            SaveWOrderInfo(jsondata, sysinfo, worderinfo);
            //发送邮件-异步
            SendEMail(jsondata, sysinfo, worderinfo);
        }
        /// 保存工单处理记录
        /// <summary>
        /// 保存工单处理记录
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
        /// 保存附件
        /// <summary>
        /// 保存附件
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
        /// 保存话务
        /// <summary>
        /// 保存话务
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        public void SaveWOrderData(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, string orderid)
        {
            if (jsondata.CallID_Out != null && jsondata.CallID_Out.Count > 0)
            {
                foreach (long callid in jsondata.CallID_Out)
                {
                    //工单关联数据表 WOrderData
                    BLL.WOrderData.Instance.InsertWOrderDataForCC(orderid, jsondata.ProcessID, callid, sysinfo.UserID);
                }
            }
        }
        /// 保存接收抄送人
        /// <summary>
        /// 保存接收抄送人
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
                        BLL.WOrderToAndCC.Instance.SaveWOrderToAndCC(orderid, jsondata.ProcessID, WOrderPersonTypeEnum.P01_接收人,
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
                        BLL.WOrderToAndCC.Instance.SaveWOrderToAndCC(orderid, jsondata.ProcessID, WOrderPersonTypeEnum.P02_抄送人,
                            person.UserID_Out, person.UserNum_Out, person.UserName_Out, sysinfo.UserID);
                        hasnum.Add(person.UserNum_Out);
                    }
                }
            }
        }
        /// 保存工单主表
        /// <summary>
        /// 保存工单主表
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

        /// 发送邮件给接收人和抄送人
        /// <summary>
        /// 发送邮件给接收人和抄送人
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        /// <param name="worderinfo"></param>
        public void SendEMailAsync(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderInfoInfo worderinfo)
        {
            Action<WOrderProcessJsonData, SysRightUserInfo, WOrderInfoInfo> action = new Action<WOrderProcessJsonData, SysRightUserInfo, WOrderInfoInfo>(SendEMail);
            action.BeginInvoke(jsondata, sysinfo, worderinfo, null, null);
        }
        /// 发送邮件给接收人和抄送人
        /// <summary>
        /// 发送邮件给接收人和抄送人
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="sysinfo"></param>
        /// <param name="worderinfo"></param>        
        public void SendEMail(WOrderProcessJsonData jsondata, SysRightUserInfo sysinfo, WOrderInfoInfo worderinfo)
        {
            try
            {
                //状态校验
                WorkOrderStatus status = (WorkOrderStatus)worderinfo.WorkOrderStatus_Value;
                if (status == WorkOrderStatus.Pending || status == WorkOrderStatus.Processing || status == WorkOrderStatus.Untreated)
                {
                    //查询邮件地址
                    string[] ccUserID = jsondata.Recevicer.Select(x => x.UserID_Out.ToString()).ToArray();
                    string[] toUserID = jsondata.ExtendRecev.Select(x => x.UserID_Out.ToString()).ToArray();
                    List<SysRightUserInfo> cc_sysinfos = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(string.Join(",", ccUserID));
                    List<SysRightUserInfo> to_sysinfos = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(string.Join(",", toUserID));
                    //邮箱
                    string[] cc_email = cc_sysinfos.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    string[] to_email = to_sysinfos.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    //查询个人用户信息
                    CustTypeEnum ctype = CustTypeEnum.T01_个人;
                    DataTable dt = BLL.WOrderInfo.Instance.GetCBInfoByPhone(worderinfo.CBID_Value, "");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ctype = (CustTypeEnum)CommonFunction.ObjectToInteger(dt.Rows[0]["CustCategoryID"]);
                    }
                    //获取工单类型
                    WOrderCategoryEnum wtype = (WOrderCategoryEnum)worderinfo.CategoryID_Value;
                    //测试数据
                    //cc_email = new string[] { "weisz@yiche.com", "liuying7@yiche.com", "qiangfei@yiche.com", "masj@yiche.com" };
                    //to_email = new string[] { "weisz@yiche.com", "liuying7@yiche.com", "qiangfei@yiche.com", "masj@yiche.com" };
                    //邮件正文1
                    string body1 = "你有一张<strong style='color:red;'>" + BLL.Util.GetEnumOptText(typeof(CustTypeEnum), (int)ctype) +
                        BLL.Util.GetEnumOptText(typeof(WOrderCategoryEnum), (int)wtype) + "</strong>工单！";
                    //邮件正文2
                    string body2 = worderinfo.Content_Value;
                    //cc系统地址
                    string weburl = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress");
                    //发邮件
                    if (cc_email.Length > 0)
                    {
                        WOrderInfo.LogToLog4("工单处理发送邮件：接收人" + string.Join(";", cc_email));
                        string title = "您有一张工单[" + worderinfo.OrderID_Value + "]待处理";
                        string a = "<a href='" + weburl + "/WOrderV2/WOrderProcess.aspx?OrderID=" + worderinfo.OrderID_Value + "'>工单处理</a>";
                        EmailHelper.Instance.SendMail(title, cc_email, new string[] { body1, body2, a }, "WOrderV2");
                    }
                    if (to_email.Length > 0)
                    {
                        WOrderInfo.LogToLog4("工单处理发送邮件：抄送人" + string.Join(";", to_email));
                        string title = "您有一张工单[" + worderinfo.OrderID_Value + "]待查看";
                        string a = "<a href='" + weburl + "/WOrderV2/WorkOrderView.aspx?OrderID=" + worderinfo.OrderID_Value + "'>工单查看</a>";
                        EmailHelper.Instance.SendMail(title, to_email, new string[] { body1, body2, a }, "WOrderV2");
                    }
                }
            }
            catch (Exception ex)
            {
                WOrderInfo.ErrorToLog4("工单处理发送邮件异常", ex);
            }
        }
    }
}
