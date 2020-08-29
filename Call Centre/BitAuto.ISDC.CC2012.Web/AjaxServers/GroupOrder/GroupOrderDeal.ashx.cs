using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder
{
    /// <summary>
    /// GroupOrderDeal 的摘要说明
    /// </summary>
    public class GroupOrderDeal : IHttpHandler, IRequiresSessionState
    {
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        private string RequestSex
        {
            get { return HttpContext.Current.Request["Sex"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Sex"].ToString()); }
        }
        private string RequestIsReturnVisit
        {
            get { return HttpContext.Current.Request["IsReturnVisit"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsReturnVisit"].ToString()); }
        }
        private string RequestFailReson
        {
            get { return HttpContext.Current.Request["FailReson"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["FailReson"].ToString()); }
        }
        private string RequestRemark
        {
            get { return HttpContext.Current.Request["Remark"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Remark"].ToString()); }
        }
        private string Action
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        //处理记录表主键
        private string HistoryLogID
        {
            get { return HttpContext.Current.Request["HistoryLogID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["HistoryLogID"].ToString()); }
        }
        //录音表主键ID
        private string RequestCallRecordID
        {
            get { return HttpContext.Current.Request["CallRecordID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CallRecordID"].ToString()); }
        }
        //汽车通信息
        private string RequestQCTUserName
        {
            get { return HttpContext.Current.Request["QCTUserName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QCTUserName"].ToString()); }
        }

        //WantCarMasterID: escape(WantCarMasterID),
        //            WantCarMasterName: escape(WantCarMasterName),
        //            WantCarSerialID: escape(WantCarSerialID),
        //            WantCarSerialName: escape(WantCarSerialName),
        //            WantCarID: escape(WantCarID),
        //            WantCarName: escape(WantCarName),
        //            PlanBuyCarTime: escape(PlanBuyCarTime)
        private string WantCarMasterID
        {
            get { return HttpContext.Current.Request["WantCarMasterID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["WantCarMasterID"].ToString()); }
        }
        private string WantCarMasterName
        {
            get { return HttpContext.Current.Request["WantCarMasterName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["WantCarMasterName"].ToString()); }
        }
        private string WantCarSerialID
        {
            get { return HttpContext.Current.Request["WantCarMasterName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["WantCarSerialID"].ToString()); }
        }
        private string WantCarSerialName
        {
            get { return HttpContext.Current.Request["WantCarSerialName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["WantCarSerialName"].ToString()); }
        }
        private string WantCarID
        {
            get { return HttpContext.Current.Request["WantCarID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["WantCarID"].ToString()); }
        }
        private string WantCarName
        {
            get { return HttpContext.Current.Request["WantCarName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["WantCarName"].ToString()); }
        }
        private string PlanBuyCarTime
        {
            get { return HttpContext.Current.Request["PlanBuyCarTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PlanBuyCarTime"].ToString()); }
        }
        public int userId;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            CheckData(out msg);
            if (msg == "")
            {
                userId = BLL.Util.GetLoginUserID();

                switch (Action)
                {
                    case "saveinfo":
                        saveinfo(out msg);
                        break;
                    case "subinfo":
                        subinfo(out msg);
                        break;
                }
            }
            if (msg == "")
            {
                msg = "{\"Result\":true}";
            }
            context.Response.Write(msg);
        }
        protected void CheckData(out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(RequestTaskID))
            {
                msg = "{\"Result\":false,\"Msg\":\"任务不能为空\"}";
            }
            else
            {
                long taskid = 0;
                if (long.TryParse(RequestTaskID, out taskid))
                {
                    if (string.IsNullOrEmpty(RequestIsReturnVisit) || string.IsNullOrEmpty(RequestFailReson))
                    {
                        msg = "{\"Result\":false,\"Msg\":\"参数不正确！\"}";
                    }
                }
                else
                {
                    msg = "{\"Result\":false,\"Msg\":\"任务id数据格式不正确！\"}";
                }
            }

        }

        protected void saveinfo(out string msg)
        {
            msg = "";
            //处理订单信息，修改任务状态，插入任务操作日志
            DealOrder(1, out msg);
        }
        /// <summary>
        /// ordertype 1是保存，2是提交
        /// </summary>
        /// <param name="ordertype"></param>
        protected void DealOrder(int ordertype, out string msg)
        {
            msg = "";
            long taskid = 0;
            if (long.TryParse(RequestTaskID, out taskid))
            {
                Entities.GroupOrderTask model = BLL.GroupOrderTask.Instance.GetGroupOrderTask(taskid);
                if (model != null)
                {
                    if (model.TaskStatus != (int)Entities.GroupTaskStatus.Processing && model.TaskStatus != (int)Entities.GroupTaskStatus.NoProcess)
                    {
                        msg = "{\"Result\":false,\"Msg\":\"任务不处于处理状态！\"}";
                    }
                    else
                    {
                        //本地保存订单信息，修改订单状态
                        Entities.GroupOrder groupordermodel = BLL.GroupOrder.Instance.GetGroupOrder(taskid);
                        if (groupordermodel != null)
                        {
                            #region 保存或提交订单信息，修改任务状态，插入任务操作状态
                            //保存
                            if (ordertype == 1)
                            {
                                model.LastUpdateTime = System.DateTime.Now;
                                model.LastUpdateUserID = userId;
                                model.TaskStatus = (int)Entities.GroupTaskStatus.Processing;

                            }
                            //提交
                            else if (ordertype == 2)
                            {
                                model.LastUpdateTime = System.DateTime.Now;
                                model.LastUpdateUserID = userId;
                                model.TaskStatus = (int)Entities.GroupTaskStatus.Processed;
                                model.SubmitTime = System.DateTime.Now;
                            }
                            int IsReturnVisit = 0;
                            if (int.TryParse(RequestIsReturnVisit, out IsReturnVisit))
                            {
                                groupordermodel.IsReturnVisit = IsReturnVisit;
                            }
                            int FailReson = 0;
                            if (int.TryParse(RequestFailReson, out FailReson))
                            {
                                groupordermodel.FailReasonID = FailReson;
                            }
                            int sex = 0;
                            if (int.TryParse(RequestSex, out sex))
                            {
                                groupordermodel.UserGender = sex;
                            }
                            groupordermodel.CallRecord = RequestRemark;
                            groupordermodel.UserName = RequestQCTUserName;
                            groupordermodel.LastUpdateTime = System.DateTime.Now;
                            groupordermodel.LastUpdateUserID = userId;

                            //意向车型，预计购车时间
                            int _WantCarMasterID = -2;
                            if (int.TryParse(WantCarMasterID, out _WantCarMasterID))
                            {
                                groupordermodel.WantCarMasterID = _WantCarMasterID;
                            }
                            int _WantCarSerialID = -2;
                            if (int.TryParse(WantCarSerialID, out _WantCarSerialID))
                            {
                                groupordermodel.WantCarSerialID = _WantCarSerialID;
                            }
                            int _WantCarID = -2;
                            if (int.TryParse(WantCarID, out _WantCarID))
                            {
                                groupordermodel.WantCarID = _WantCarID;
                            }
                            int _PlanBuyCarTime = -2;
                            if (int.TryParse(PlanBuyCarTime, out _PlanBuyCarTime))
                            {
                                groupordermodel.PlanBuyCarTime = _PlanBuyCarTime;
                            }
                            groupordermodel.WantCarMasterName = WantCarMasterName;
                            groupordermodel.WantCarSerialName = WantCarSerialName;
                            groupordermodel.WantCarName = WantCarName;

                            //更新任务状态
                            BLL.GroupOrderTask.Instance.Update(model);
                            //对于保存不用匹配客户池客户所以在此处更新订单信息，提交要在匹配客户池客户后拿到custid后更新
                            if (ordertype == 1)
                            {
                                BLL.GroupOrder.Instance.Update(groupordermodel);
                            }
                            //插入或合并任务操作日志
                            DealLog(ordertype);
                            #endregion

                            #region 提交处理
                            if (ordertype == 2)
                            {
                                if (!string.IsNullOrEmpty(groupordermodel.CustomerTel))
                                {
                                    #region 根据电话号码判断 客户池是否存在，存在不更新客户，不存在插入

                                    string CustID = string.Empty;
                                    //根据电话找客户，如果有多个去匹配那个名称一致的
                                    DataTable dtcust = BLL.CustBasicInfo.Instance.GetCustBasicInfosByTel(groupordermodel.CustomerTel);
                                    if (dtcust != null && dtcust.Rows.Count > 0)
                                    {
                                        if (dtcust.Rows.Count == 1)
                                        {
                                            //如果找到一个，就取CustID
                                            CustID = dtcust.Rows[0]["CustID"].ToString();
                                        }
                                        else if (dtcust.Rows.Count > 1)
                                        {
                                            //多个客户，查找有团购订单类型的客户
                                            DataRow[] rows = dtcust.Select("CustName='" + groupordermodel.CustomerName.Trim() + "'");
                                            if (rows.Length > 0)
                                            {
                                                CustID = rows[0]["CustID"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //根据电话没有找到客户，要插入个人客户信息表
                                        CustID = AddNewCustBaseInfo(groupordermodel);
                                    }

                                    #endregion

                                    #region 提交要匹配客户池客户所以在匹配客户池客户后拿到custid后更新订单
                                    groupordermodel.CustID = CustID;
                                    BLL.GroupOrder.Instance.Update(groupordermodel);
                                    #endregion

                                    #region 在客户历史记录里加入团购订单联系记录
                                    Entities.CustHistoryInfo custhistroyinfomodel = new Entities.CustHistoryInfo();
                                    custhistroyinfomodel.TaskID = RequestTaskID;
                                    //业务类型，2为团购订单
                                    custhistroyinfomodel.BusinessType = 2;
                                    //录音
                                    if (!string.IsNullOrEmpty(RequestCallRecordID))
                                    {
                                        long callrecordid = 0;
                                        if (long.TryParse(RequestCallRecordID, out callrecordid))
                                        {
                                            custhistroyinfomodel.CallRecordID = callrecordid;

                                        }
                                    }
                                    //呼出
                                    custhistroyinfomodel.RecordType = 2;
                                    custhistroyinfomodel.CustID = CustID;
                                    custhistroyinfomodel.CreateTime = System.DateTime.Now;
                                    custhistroyinfomodel.CreateUserID = userId;
                                    //功能废弃
                                    BLL.CustHistoryInfo.Instance.Insert(custhistroyinfomodel);
                                    #endregion

                                    #region 回写易湃
                                    string errMsg = "";
                                    ReWriteOrderData(taskid, out errMsg);
                                    #endregion

                                    if (errMsg != "")
                                    {
                                        msg = "{\"Result\":false,\"Msg\":\"调用易湃接口失败！" + errMsg + "\"}";
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            msg = "{\"Result\":false,\"Msg\":\"任务对应订单不存在！\"}";
                        }
                    }
                }
                else
                {
                    msg = "{\"Result\":false,\"Msg\":\"任务不存在！\"}";
                }
            }
        }

        /// <summary>
        /// 在个人基本信息表中添加一个个人信息
        /// </summary>
        /// <param name="groupordermodel"></param>
        /// <returns></returns>
        private string AddNewCustBaseInfo(Entities.GroupOrder groupordermodel)
        {
            string CustID = "";
            #region 插入客户池
            Entities.CustBasicInfo newcustmodel = new Entities.CustBasicInfo();
            newcustmodel.Status = 0;
            newcustmodel.Sex = groupordermodel.UserGender;
            newcustmodel.CustName = groupordermodel.CustomerName;
            newcustmodel.CustCategoryID = 4;
            newcustmodel.ProvinceID = groupordermodel.ProvinceID;
            newcustmodel.CityID = groupordermodel.CityID;
            newcustmodel.AreaID = groupordermodel.AreaID;
            newcustmodel.CreateTime = System.DateTime.Now;
            newcustmodel.CreateUserID = userId;
            //功能废弃 插入客户基本信息 废弃
            CustID = null;// BLL.CustBasicInfo.Instance.Insert(newcustmodel);
            Entities.CustTel custtelmodel = new Entities.CustTel();
            custtelmodel.CreateTime = System.DateTime.Now;
            custtelmodel.CreateUserID = userId;
            custtelmodel.CustID = CustID;
            custtelmodel.Tel = groupordermodel.CustomerTel;
            //插入电话
            BLL.CustTel.Instance.Insert(custtelmodel);
            //插入bugcarinfo
            Entities.BuyCarInfo bugcarmodel = new Entities.BuyCarInfo();
            bugcarmodel.CustID = CustID;
            bugcarmodel.Type = 4;
            bugcarmodel.CarBrandId = 0;
            bugcarmodel.CarSerialId = 0;
            bugcarmodel.CarTypeID = 0;
            bugcarmodel.Status = 0;
            bugcarmodel.CreateTime = System.DateTime.Now;
            bugcarmodel.CreateUserID = userId;
            bugcarmodel.UserName = RequestQCTUserName;
            BLL.BuyCarInfo.Instance.Insert(bugcarmodel);
            #endregion
            return CustID;
        }

        /// <summary>
        /// ordertype 1是保存，2是提交
        /// </summary>
        /// <param name="ordertype"></param>
        protected void DealLog(int ordertype)
        {
            //判断是否有通话记录，如果有通话则有不为空;
            if (string.IsNullOrEmpty(HistoryLogID))
            {
                Entities.GroupOrderTaskOperationLog logmodel = new Entities.GroupOrderTaskOperationLog();
                logmodel.CreateUserID = userId;
                logmodel.CreateTime = System.DateTime.Now;
                logmodel.Remark = RequestRemark;
                logmodel.TaskID = long.Parse(RequestTaskID);
                if (ordertype == 1)
                {
                    logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Save;
                    logmodel.TaskStatus = (int)Entities.GroupTaskStatus.Processing;
                }
                else if (ordertype == 2)
                {
                    logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Submit;
                    logmodel.TaskStatus = (int)Entities.GroupTaskStatus.Processed;
                }
                BLL.GroupOrderTaskOperationLog.Instance.Insert(logmodel);
            }
            else
            {
                int historylogid = 0;
                if (int.TryParse(HistoryLogID, out historylogid))
                {
                    Entities.GroupOrderTaskOperationLog logmodel = BLL.GroupOrderTaskOperationLog.Instance.GetGroupOrderTaskOperationLog(historylogid);
                    logmodel.Remark = RequestRemark;
                    if (ordertype == 1)
                    {
                        logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Save;
                        logmodel.TaskStatus = (int)Entities.GroupTaskStatus.Processing;

                    }
                    else if (ordertype == 2)
                    {
                        logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Submit;
                        logmodel.TaskStatus = (int)Entities.GroupTaskStatus.Processed;
                    }
                    BLL.GroupOrderTaskOperationLog.Instance.Update(logmodel);
                }
            }
        }

        protected void subinfo(out string msg)
        {
            msg = "";
            //处理订单信息，修改任务状态，插入任务操作日志，合并客户池，插入客户历史联系记录，调用团购订单接口
            DealOrder(2, out msg);

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 回写团购订单
        /// <summary>
        /// 回写团购订单
        /// </summary>
        private void ReWriteOrderData(long taskid, out string msg)
        {
            msg = "";
            try
            {
                ReUpdate(taskid);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Loger.Log4Net.Debug("回写团购订单订单出错:", ex);
                UnhandledExceptionFunction(null, new UnhandledExceptionEventArgs(ex, false));
            }
        }

        private void ReUpdate(long taskid)
        {
            BLL.Loger.Log4Net.Info("开始调用易湃团购回写接口");
            #region 定义变量

            string errorMsg = "";
            int retService = 0;
            BitAuto.ISDC.CC2012.WebService.GroupOrderHelper OrderHelper = new BitAuto.ISDC.CC2012.WebService.GroupOrderHelper();
            Entities.UpdateOrderData updateDateMode = null;
            #endregion


            #region 调用口

            Entities.QueryGroupOrder query = new Entities.QueryGroupOrder();
            query.TaskID = taskid;
            List<Entities.GroupOrder> list = BLL.GroupOrder.Instance.GetGroupOrderList(query);
            if (list != null)
            {
                foreach (Entities.GroupOrder newModel in list)
                {
                    retService = OrderHelper.SetTGCarOrderState(newModel, ref errorMsg);
                    if (retService == 1)
                    {
                        BLL.Loger.Log4Net.Info("回写团购订单成功！易湃订单ID为：" + newModel.OrderID.Value);
                    }
                    #region 插入更新无主订单数据日志表
                    updateDateMode = new Entities.UpdateOrderData();
                    updateDateMode.TaskID = newModel.TaskID.ToString();
                    updateDateMode.YPOrderID = newModel.OrderID;
                    updateDateMode.UpdateType = retService;
                    updateDateMode.IsUpdate = retService; // 1 成功了，不用处理，-1 需要重新处理
                    updateDateMode.UpdateErrorMsg = errorMsg;
                    updateDateMode.CreateTime = DateTime.Now;
                    updateDateMode.CreateUserID = userId;
                    updateDateMode.APIType = 4;//4为团购订单类型
                    BLL.UpdateOrderData.Instance.Insert(updateDateMode);
                    #endregion

                }
            }
            #endregion
        }
        #endregion
        #region 异常处理
        private void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            BLL.Loger.Log4Net.Debug("回写团购订单出错", e);

            string errorMsg = e.Message;
            string stackTrace = e.StackTrace;
            string source = e.Source;

            string mailBody = string.Format("错位信息：{0}<br/>错误Source：{1}<br/>错误StackTrace：{2}<br/>IsTerminating:{3}<br/>",
                errorMsg, source, stackTrace, args.IsTerminating);
            string subject = "客户呼叫中心系统——回写团购订单出错";
            string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
            }
        }
        #endregion
    }
}