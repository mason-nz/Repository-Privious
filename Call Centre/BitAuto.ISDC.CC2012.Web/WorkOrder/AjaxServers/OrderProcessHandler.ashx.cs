using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    /// <summary>
    /// OrderProcessHandler 的摘要说明
    /// </summary>
    public class OrderProcessHandler : IHttpHandler, IRequiresSessionState
    {

        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string OrderID
        {
            get
            {
                if (Request["OrderID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["OrderID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string WorkOrderStatus
        {
            get
            {
                if (Request["WorkOrderStatus"] != null)
                {
                    return HttpUtility.UrlDecode(Request["WorkOrderStatus"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string ValidateData
        {
            get
            {
                if (Request["ValidateData"] != null)
                {
                    return HttpUtility.UrlDecode(Request["ValidateData"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string OperData
        {
            get
            {
                if (Request["OperData"] != null)
                {
                    return Request["OperData"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string CallID
        {
            get
            {
                if (Request["CallID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CallID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string TagIDs
        {
            get
            {
                if (Request["TagIDs"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TagIDs"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string TagNames
        {
            get
            {
                if (Request["TagNames"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TagNames"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string RevertContent
        {
            get
            {
                if (Request["RevertContent"] != null)
                {
                    return HttpUtility.UrlDecode(Request["RevertContent"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string ReceiverName
        {
            get
            {
                if (Request["ReceiverName"] != null)
                {
                    return HttpUtility.UrlDecode(Request["ReceiverName"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string ReceiverID
        {
            get
            {
                if (Request["ReceiverID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["ReceiverID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string IsSales
        {
            get
            {
                if (Request["IsSales"] != null)
                {
                    return HttpUtility.UrlDecode(Request["IsSales"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string EmailErrorMsg
        {
            get
            {
                if (Request["EmailErrorMsg"] != null)
                {
                    return HttpUtility.UrlDecode(Request["EmailErrorMsg"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string Requester
        {
            get
            {
                if (Request["Requester"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Requester"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string RTWORID
        {
            get;
            set;
        }

        private DateTime operTime;
        private int operUserID;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            operTime = DateTime.Now;
            operUserID = BLL.Util.GetLoginUserID();

            switch (Action)
            {
                case "permission":
                    judgePermission(out msg);
                    msg = "{permission:'" + msg + "'}";
                    break;
                case "salesPermission":
                    judgeSalesPermission(out msg);
                    msg = "{permission:'" + msg + "'}";
                    break;
                case "salesProcessSubmit":
                    SalesProcessSubmit(out msg);
                    if (msg == string.Empty)
                    {
                        msg = "{permission:''}";
                    }
                    else if (msg == "true")
                    {
                        msg = "{result:'true',permission:'process'}";
                    }
                    else
                    {
                        msg = "{result:'false',msg:'',permission:'" + msg + "'}";
                    }
                    break;
                case "ccProcessSubmit":
                    CCProcessSubmit(out msg);
                    if (msg == string.Empty)
                    {
                        msg = "{permission:''}";
                    }
                    else if (msg == "true")
                    {
                        msg = "{result:'true',permission:'process'}";
                    }
                    break;
                case "SendErrorMail":
                    SendErrorMail(EmailErrorMsg, "工单" + OrderID + "业务记录保存错误", out msg);
                    break;
                default: break;
            }

            context.Response.Write(msg);
        }

        private void SendErrorMail(string mailBody, string subject, out string msg)
        {
            string userEmails = ConfigurationManager.AppSettings["ReceiveErrorEmail"];
            string[] userEmail = userEmails.Split(';');
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
                msg = "'result':'true'";
            }
            else
            {
                msg = "'result':'false'";
            }
        }

        private void SalesProcessSubmit(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(Requester) && Requester.Trim() == "intelligentplatform")
            {
                msg = "process";
            }
            else
            {
                judgeSalesPermission(out msg);
            }

            if (msg == "none" || msg == "view" || msg == "over")
            {
                return;
            }

            if (string.IsNullOrEmpty(OrderID))
            {
                msg = "工单号不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(RevertContent))
            {
                msg = "回复内容不能为空！";
                return;
            }
            Web.AjaxServers.ValidateDataFormat vd = new Web.AjaxServers.ValidateDataFormat();

            int contentLen = vd.GetLength(RevertContent);
            if (contentLen > 600)
            {
                msg = "回复内容超过300个字！";
                return;
            }

            #region 更新工单表

            Entities.WorkOrderInfo model_WorkOrderInfo = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);

            string oldReceiverName = model_WorkOrderInfo.ReceiverName;
            int _receiverID;
            if (int.TryParse(ReceiverID, out _receiverID))
            {
                //坐席操作，只有在选择接收人，才会覆盖当前操作人，否则不清空
                model_WorkOrderInfo.ReceiverID = _receiverID;
                model_WorkOrderInfo.ReceiverName = ReceiverName;
            }

            model_WorkOrderInfo.IsSales = IsSales == "true" ? true : false;

            if (RevertContent != string.Empty)
            {
                model_WorkOrderInfo.IsRevert = true;
            }
            if (!string.IsNullOrEmpty(ReceiverName) && !string.IsNullOrEmpty(ReceiverID) && ReceiverID != "-2")
            {
                model_WorkOrderInfo.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(int.Parse(ReceiverID));
            }

            model_WorkOrderInfo.ModifyTime = operTime;
            model_WorkOrderInfo.ModifyUserID = operUserID;

            model_WorkOrderInfo.WorkOrderStatus = (int)Entities.WorkOrderStatus.Processing;

            BLL.WorkOrderInfo.Instance.Update(model_WorkOrderInfo);

            #endregion

            #region 插入工单回复表(原来的)

            Entities.WorkOrderRevert model_Revert = new Entities.WorkOrderRevert();
            if (string.IsNullOrEmpty(RTWORID))  //没有接通电话时提交回复信息，则插入[WorkOrderRevert]表一条数据
            {
                model_Revert.OrderID = OrderID;
                model_Revert.RevertContent = RevertContent;

                if (!string.IsNullOrEmpty(oldReceiverName) || !string.IsNullOrEmpty(ReceiverName))
                {
                    model_Revert.ReceiverName = fieldDesc(oldReceiverName, model_WorkOrderInfo.ReceiverName);
                    model_Revert.ReceiverID = model_WorkOrderInfo.ReceiverID.ToString();
                }
                model_Revert.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(operUserID);
                model_Revert.WorkOrderStatus = "处理中";
                model_Revert.CreateTime = operTime;
                model_Revert.CreateUserID = operUserID;

                BLL.WorkOrderRevert.Instance.Insert(model_Revert);
            }
            else  //已经接通电话，并插入[WorkOrderRevert]表一条数据，则更新这条数据
            {
                model_Revert = BLL.WorkOrderRevert.Instance.GetWorkOrderRevert(long.Parse(RTWORID));
                model_Revert.RevertContent = RevertContent;
                if (!string.IsNullOrEmpty(oldReceiverName) || !string.IsNullOrEmpty(ReceiverName))
                {
                    model_Revert.ReceiverName = fieldDesc(oldReceiverName, model_WorkOrderInfo.ReceiverName);
                    model_Revert.ReceiverID = model_WorkOrderInfo.ReceiverID.ToString();
                }
                model_Revert.WorkOrderStatus = "处理中";

                BLL.WorkOrderRevert.Instance.Update(model_Revert);
            }

            #endregion

            #region 插入工单回复表、工单日志表

            //Entities.WorkOrderLog log = new Entities.WorkOrderLog();
            //Entities.WorkOrderReceiver receiver = new Entities.WorkOrderReceiver();

            //if (string.IsNullOrEmpty(RTWORID))  //没有接通电话时提交回复信息，则插入 日志表、回复表
            //{
            //    log.OrderID = OrderID;
            //    if (!string.IsNullOrEmpty(oldReceiverName) || !string.IsNullOrEmpty(ReceiverName))
            //    {
            //        log.LogDesc = fieldDesc(oldReceiverName, model_WorkOrderInfo.ReceiverName);
            //        /// model_Revert.WorkOrderStatus = "处理中";
            //    }
            //    log.CreateTime = operTime;
            //    log.CreateUserID = operUserID;

            //    receiver.RevertContent = RevertContent;
            //    receiver.ReceiverUserID = model_WorkOrderInfo.ReceiverID;
            //    receiver.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(operUserID);
            //    receiver.CreateTime = operTime;
            //    receiver.CreateUserID = operUserID;

            //    int ReceiverRecID = BLL.WorkOrderReceiver.Instance.Insert(receiver);
            //    log.ReceiverRecID = ReceiverRecID;
            //    BLL.WorkOrderLog.Instance.Insert(log);
            //}
            //else
            //{
            //    receiver = BLL.WorkOrderReceiver.Instance.GetWorkOrderReceiver(int.Parse(RTWORID));
            //    receiver.RevertContent = RevertContent;
            //    receiver.ReceiverUserID = model_WorkOrderInfo.ReceiverID;

            //    log = BLL.WorkOrderLog.Instance.GetWorkOrderLogByRtRecID(int.Parse(RTWORID));
            //    if (!string.IsNullOrEmpty(oldReceiverName) || !string.IsNullOrEmpty(ReceiverName))
            //    {
            //        log .LogDesc= fieldDesc(oldReceiverName, model_WorkOrderInfo.ReceiverName);
            //        //model_Revert.WorkOrderStatus = "处理中";
            //    }

            //    BLL.WorkOrderLog.Instance.Update(log);
            //    BLL.WorkOrderReceiver.Instance.Update(receiver);
            //}

            #endregion

            #region 发送邮件

            if (int.TryParse(ReceiverID, out _receiverID))
            {
                AjaxServers.SendEmailInfo sendInfo = new AjaxServers.SendEmailInfo();
                sendInfo.ReceiverID = _receiverID;
                sendInfo.ReceiverName = ReceiverName;
                sendInfo.LastOperTime = model_WorkOrderInfo.LastProcessDate;
                sendInfo.OrderID = model_WorkOrderInfo.OrderID;
                sendInfo.Title = model_WorkOrderInfo.Title;
                sendInfo.DepartName = model_WorkOrderInfo.ReceiverDepartName;
                sendInfo.Desc = model_WorkOrderInfo.Content;

                SendEmailToReceiver(sendInfo);
            }

            #endregion

            msg = "true";
        }

        private void CCProcessSubmit(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(Requester) && Requester.Trim() == "intelligentplatform")
            {
                msg = "process";
            }
            else
            {
                judgePermission(out msg);
            }

            if (msg == string.Empty)
            {
                return;
            }
            Web.AjaxServers.ValidateDataFormat vd = new Web.AjaxServers.ValidateDataFormat();

            int contentLen = vd.GetLength(RevertContent);
            if (contentLen > 600)
            {
                msg = "回复内容超过300个字！";
                return;
            }

            //提交

            #region 插入工单表


            Entities.WorkOrderInfo model_OldWorkOrderInfo = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);

            Entities.WorkOrderInfo model_NewWorkOrderInfo = bindWorkOrderInfo(BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID), out msg);

            if (msg != string.Empty)
            {
                return;
            }

            model_NewWorkOrderInfo.ModifyUserID = operUserID;
            model_NewWorkOrderInfo.ModifyTime = operTime;

            if (RevertContent != string.Empty)
            {
                model_NewWorkOrderInfo.IsRevert = true;
            }

            BLL.WorkOrderInfo.Instance.Update(model_NewWorkOrderInfo);

            #endregion

            #region 插入工单回复表

            EditWorkOrderRevert(model_NewWorkOrderInfo, model_OldWorkOrderInfo, out msg);

            #endregion

            #region 发送邮件


            if (model_NewWorkOrderInfo.ReceiverName != model_OldWorkOrderInfo.ReceiverName && model_NewWorkOrderInfo.ReceiverID != null)
            {
                AjaxServers.SendEmailInfo sendInfo = new AjaxServers.SendEmailInfo();
                sendInfo.ReceiverID = (int)model_NewWorkOrderInfo.ReceiverID;
                sendInfo.ReceiverName = model_NewWorkOrderInfo.ReceiverName;
                sendInfo.LastOperTime = model_NewWorkOrderInfo.LastProcessDate;
                sendInfo.OrderID = model_NewWorkOrderInfo.OrderID;
                sendInfo.Title = model_NewWorkOrderInfo.Title;
                sendInfo.DepartName = model_NewWorkOrderInfo.ReceiverDepartName;
                sendInfo.Desc = model_NewWorkOrderInfo.Content;

                SendEmailToReceiver(sendInfo);
            }

            #endregion

            msg = "true";
        }

        //编辑工单回复表信息
        private void EditWorkOrderRevert(Entities.WorkOrderInfo newModel, Entities.WorkOrderInfo oldModel, out string msg)
        {
            msg = string.Empty;

            if (newModel == null || oldModel == null)
            {
                return;
            }

            Entities.WorkOrderRevert model_Revert = new Entities.WorkOrderRevert();

            Int64 callID = 0;
            if (Int64.TryParse(CallID, out callID))
            {
                //如果有录音，则在之前的录音记录的基础上进行回复表的修改
                model_Revert = BLL.WorkOrderRevert.Instance.GetWorkOrderRevertByCallID(callID);
            }
            else
            {
                model_Revert.OrderID = oldModel.OrderID;
                model_Revert.CreateTime = operTime;
                model_Revert.CreateUserID = operUserID;
            }
            model_Revert.RevertContent = RevertContent;
            model_Revert.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(operUserID);

            model_Revert.IsReturnVisit = RevertContent == string.Empty ? "否" : "是";

            //比较工单分类
            if (newModel.CategoryID != oldModel.CategoryID)
            {
                string oldCategoryName = BLL.WorkOrderCategory.Instance.GetCategoryFullName(oldModel.CategoryID.ToString());

                string newCategoryName = BLL.WorkOrderCategory.Instance.GetCategoryFullName(newModel.CategoryID.ToString());

                model_Revert.CategoryName = fieldDesc(oldCategoryName, newCategoryName);
            }

            //比较工单状态
            if (newModel.WorkOrderStatus != oldModel.WorkOrderStatus)
            {
                string oldStatus = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), (int)oldModel.WorkOrderStatus);

                string newStatus = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), (int)newModel.WorkOrderStatus);

                model_Revert.WorkOrderStatus = fieldDesc(oldStatus, newStatus);
            }

            //比较优先级
            if (newModel.PriorityLevel != oldModel.PriorityLevel)
            {
                string oldLevel = oldModel.PriorityLevel == 1 ? "普通" : "紧急";

                string newLevel = newModel.PriorityLevel == 1 ? "普通" : "紧急";

                model_Revert.PriorityLevelName = fieldDesc(oldLevel, newLevel);
            }

            //比较接收人
            if (!string.IsNullOrEmpty(newModel.ReceiverName) || !string.IsNullOrEmpty(oldModel.ReceiverName))
            {
                model_Revert.ReceiverID = newModel.ReceiverID.ToString();
                model_Revert.ReceiverName = fieldDesc(oldModel.ReceiverName, newModel.ReceiverName);
            }

            //比较是否投诉
            if (newModel.IsComplaintType != oldModel.IsComplaintType)
            {
                string oldIsComplaint = oldModel.IsComplaintType == true ? "投诉" : "普通";

                string newIsComplaint = newModel.IsComplaintType == true ? "投诉" : "普通";

                model_Revert.IsComplaintType = fieldDesc(oldIsComplaint, newIsComplaint);
            }

            //比较标签
            string tagDesc = GetCompareTagDesc(model_Revert.OrderID, TagIDs);
            if (tagDesc != string.Empty)
            {
                model_Revert.TagName = tagDesc;

                //修改标签表
                BLL.WorkOrderTagMapping.Instance.DeleteByOrderID(model_Revert.OrderID);

                if (TagIDs != string.Empty)
                {
                    string[] array_TagIDs = TagIDs.Split(',');
                    for (int k = 0; k < array_TagIDs.Length; k++)
                    {
                        Entities.WorkOrderTagMapping model_Mapping = new Entities.WorkOrderTagMapping();
                        model_Mapping.OrderID = model_Revert.OrderID;
                        model_Mapping.TagID = int.Parse(array_TagIDs[k]);
                        model_Mapping.Status = 0;
                        model_Mapping.CreateTime = model_Mapping.ModifyTime = operTime;
                        model_Mapping.CreateUserID = model_Mapping.ModifyUserID = operUserID;
                        BLL.WorkOrderTagMapping.Instance.Insert(model_Mapping);
                    }
                }

            }

            if (callID != 0)
            {
                BLL.WorkOrderRevert.Instance.Update(model_Revert);
            }
            else
            {
                BLL.WorkOrderRevert.Instance.Insert(model_Revert);
            }
        }

        //获取标签是否修改的描述；如果没有修改，返回string.Empty，否则返回Desc描述信息
        private string GetCompareTagDesc(string orderID, string tagIDs)
        {
            string tagDesc = string.Empty;

            DataTable dt_OldTag = BLL.WorkOrderTag.Instance.GetWorkOrderTagByOrderID(orderID);
            ArrayList arryNewTag = new ArrayList();
            string[] ids = tagIDs.Split(',');
            for (int k = 0; k < ids.Length; k++)
            {
                arryNewTag.Add(ids[k]);
            }

            if (arryNewTag.Count == dt_OldTag.Rows.Count && dt_OldTag.Rows.Count == 0)
            {
                return tagDesc;
            }

            //先比较数量，数量不对，标签肯定有变化
            if (dt_OldTag.Rows.Count != arryNewTag.Count)
            {
                tagDesc = fieldDesc(BLL.WorkOrderTag.Instance.GetWorkOrderTagNames(orderID), TagNames);
                return tagDesc;
            }

            int removeListCount = 0;

            foreach (string newTag in arryNewTag)
            {
                if (newTag != string.Empty)
                {
                    DataRow[] drTag = dt_OldTag.Select(" TagID=" + newTag);
                    if (drTag.Length != 1)
                    {
                        tagDesc = fieldDesc(BLL.WorkOrderTag.Instance.GetWorkOrderTagNames(orderID), TagNames);
                        return tagDesc;
                    }

                    ++removeListCount;
                    dt_OldTag.Rows.Remove(drTag[0]);
                }
            }

            if (arryNewTag.Count == removeListCount && dt_OldTag.Rows.Count == 0)
            {
                return tagDesc;
            }
            else
            {
                tagDesc = fieldDesc(BLL.WorkOrderTag.Instance.GetWorkOrderTagNames(orderID), TagNames);
                return tagDesc;
            }

        }

        //回复表有变化的字段描述
        private string fieldDesc(string oldField, string newField)
        {
            string desc = string.Empty;
            desc = oldField == "" ? "变为 " + newField : "由 " + oldField + " 变为 " + newField;
            return desc;
        }

        //绑定回复时工单表信息
        private Entities.WorkOrderInfo bindWorkOrderInfo(Entities.WorkOrderInfo oldModel, out string msg)
        {
            msg = string.Empty;

            Entities.WorkOrderInfo model_WorkOrderInfo = oldModel;

            int? oldReceiverID = model_WorkOrderInfo.ReceiverID;

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.WorkOrderInfo> conver = new BLL.ConverToEntitie<Entities.WorkOrderInfo>(model_WorkOrderInfo);
            errMsg = conver.Conver(OperData);

            if (errMsg != "")
            {
                msg = "{'result':'false','msg':'给对应实体赋值时出错，操作失败！'}";
                return null;
            }

            if (model_WorkOrderInfo.ReceiverID == -2)
            {
                //在没有选择接收人 且 状态为已处理，不清空工单当前处理人 add lxw 13.9.10
                if (model_WorkOrderInfo.WorkOrderStatus == (int)Entities.WorkOrderStatus.Processed)
                {
                    model_WorkOrderInfo.ReceiverID = oldReceiverID;
                }
                else
                {
                    model_WorkOrderInfo.ReceiverName = "";
                    model_WorkOrderInfo.ReceiverDepartName = "";
                }
            }
            else
            {
                //存在接收人，赋值接收人的所在部门
                model_WorkOrderInfo.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID((int)model_WorkOrderInfo.ReceiverID);
            }

            return model_WorkOrderInfo;
        }

        //验证CC工单处理权限,如果没有权限msg返回string.Empty
        private void judgePermission(out string msg)
        {
            msg = string.Empty;

            if (!string.IsNullOrEmpty(OrderID))
            {
                OrderPermission op = new OrderPermission();

                Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                if (model == null)
                {
                    msg = string.Empty;
                }
                else
                {
                    string permission = string.Empty;
                    permission = op.JudgeOrderPermission(model, out msg);

                    msg = permission;
                }
            }
        }

        //验证销售工单处理权限,如果没有权限msg返回string.Empty
        private void judgeSalesPermission(out string msg)
        {
            msg = string.Empty;

            if (!string.IsNullOrEmpty(OrderID))
            {
                OrderPermission op = new OrderPermission();

                Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                string permission = string.Empty;
                permission = op.JudgeSalesPermission(model, out msg);

                msg = permission;
            }
        }

        //给 接收人 发送邮件
        private void SendEmailToReceiver(AjaxServers.SendEmailInfo info)
        {
            string url = string.Empty;

            if (info.ReceiverID == -2)
            {
                return;
            }

            //邮件接收人的邮箱 
            string userEmail = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetUserEmail(info.ReceiverID);
            string urlHead = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("TaskProcessUrl");
            if (info.DepartName.Contains("呼叫中心"))
            {
                url = "<a href='" + urlHead + "/WorkOrder/CCProcess.aspx?OrderID=" + info.OrderID + "'>" + info.OrderID + "</a>";
            }
            else
            {
                url = "<a href='" + urlHead + "/WorkOrder/SalesProcess.aspx?OrderID=" + info.OrderID + "'>" + info.OrderID + "</a>";
            }

            //邮件内容
            string content = string.Empty;
            content = "您有一个工单需要处理。<br/>";
            content += "工单标题：" + info.Title + "<br/>";
            content += "工单记录：" + info.Desc + "<br/>";
            content += "最晚处理日期：" + info.LastOperTime + "<br/>";
            content += "处理请点击：" + url;

            if (info.ReceiverName != string.Empty)
            {
                BLL.EmailHelper.Instance.SendMailByWorkOrder(info.ReceiverName, info.Title, info.Desc, info.LastOperTime, url, "您有一个工单需要处理", new string[] { userEmail });

                BLL.Util.InsertUserLog("【工单处理】发送邮件成功，收件人【" + info.ReceiverName + "】，邮箱【" + userEmail + "】，内容【" + content + "】");
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}