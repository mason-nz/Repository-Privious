using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder;
using System.Web.SessionState;
using BitAuto.Utils.Config;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    /// <summary>
    /// OrderConsultInfo 的摘要说明
    /// </summary>
    public class OrderConsultInfo : IHttpHandler, IRequiresSessionState
    {
        #region 参数

        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString());
            }
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string Source
        {
            get
            {
                return HttpContext.Current.Request["Source"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Source"].ToString());
            }
        }

        /// <summary>
        /// 保存或者提交 （保存：save  提交: sub）
        /// </summary>
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        /// <summary>
        /// 新车联系记录的字符串
        /// </summary>
        public string NewCarConsultInfoStr
        {
            get
            {
                return HttpContext.Current.Request["NewCarConsultInfo"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["NewCarConsultInfo"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        /// 置换联系记录的字符串
        /// </summary>
        public string ReplaceCarConsultInfoStr
        {
            get
            {
                return HttpContext.Current.Request["ReplaceCarConsultInfo"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["ReplaceCarConsultInfo"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        /// 未选经销商理由（枚举）
        /// </summary>
        public string NoDealerReasonID
        {
            get
            {
                return HttpContext.Current.Request["NoDealerReasonID"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["NoDealerReasonID"].ToString());
            }
        }

        /// <summary>
        /// 未选经销商理由备注
        /// </summary>
        public string NoDealerReason
        {
            get
            {
                return HttpContext.Current.Request["NoDealerReason"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["NoDealerReason"].ToString());
            }
        }

        //车型ID 用来得到对应的颜色列表
        public string CarTypeID
        {
            get
            {
                return HttpContext.Current.Request["CarTypeID"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CarTypeID"].ToString());
            }
        }
        public string RequestActionByCarID
        {
            get
            {
                return HttpContext.Current.Request["ActionByCarID"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["ActionByCarID"].ToString());
            }
        }

        #region 基本信息属性

        private int Age
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Age"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Age"]);
            }
        }
        private int Vocation
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Vocation"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Vocation"]);
            }
        }
        private string IDCard
        {
            get
            {
                return HttpContext.Current.Request["IDCard"] == null ? string.Empty : HttpContext.Current.Request["IDCard"];
            }
        }
        private int InCome
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["InCome"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["InCome"]);
            }
        }
        private int Marriage
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Marriage"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Marriage"]);
            }
        }
        private int CarBrandID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarBrandID"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarBrandID"]);
            }
        }
        private int CarSerialID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarSerialID"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarSerialID"]);
            }
        }
        private string CarName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarName"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CarName"]);
            }
        }
        private int IsAttestation
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["IsAttestation"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["IsAttestation"]);
            }
        }
        private int DriveAge
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["DriveAge"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["DriveAge"]);
            }
        }
        private string UserName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["UserName"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserName"]);
            }
        }

        private string CarNo
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarNo"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CarNo"]);
            }
        }
        private string Remark
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Remark"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Remark"]);
            }
        }
        //已购车未购车
        private int Type
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Type"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Type"]);
            }
        }
        #endregion

        //add by qizq 2013-1-4是否通话中
        private string IsCalling
        {
            get { return HttpContext.Current.Request["IsCalling"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsCalling"].ToString()); }
        }
        //本地录音表主键
        private string CallRecordID
        {
            get { return HttpContext.Current.Request["CallRecordID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CallRecordID"].ToString()); }
        }
        //处理记录表主键
        private string HistoryLogID
        {
            get { return HttpContext.Current.Request["HistoryLogID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["HistoryLogID"].ToString()); }
        }
        //

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";
            int userID = 0;

            try
            {
                if (RequestActionByCarID == "getColorTableByID")
                {
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                    bindColor(out msg);
                }

                else
                    if (BLL.Util.CheckButtonRight("SYS024BUT1201"))
                    {
                        CheckMsg(out msg);

                        if (msg == "")
                        {
                            userID = BitAuto.ISDC.CC2012.BLL.Util.GetLoginUserID();
                            Submit(out msg, userID);
                        }
                    }
                    else
                    {
                        msg = "您没有操作无主订单的权限！";
                    }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            if (msg == "")
            {
                msg = "success";
            }
            context.Response.Write(msg);
        }

        private void CheckMsg(out string msg)
        {
            msg = "";
            int intVal = 0;
            if (TaskID == string.Empty || !int.TryParse(TaskID, out intVal))
            {
                msg += "任务ID参数不正确";
                return;
            }
            if (Source == string.Empty || !int.TryParse(Source, out intVal))
            {
                msg += "订单类型参数不正确";
                return;
            }
            if (Action == string.Empty || (Action != "save" && Action != "sub"))
            {
                msg += "操作类型参数不正确";
                return;
            }
            if (NoDealerReasonID != string.Empty && !int.TryParse(NoDealerReasonID, out intVal))
            {
                msg += "没有选择经销商的原因参数不正确";
                return;
            }

        }

        private void Submit(out string msg, int userID)
        {
            //此方法内的逻辑可参考文档：$/A5信息系统研发/销售业务管理平台/客户关系管理/doc/Call Center/无主订单处理页面保存和提交逻辑.docx  ---- Add By Chybin At 2013-07-19

            msg = "";

            //System.Threading.Thread.Sleep(5000);

            #region 准备数据

            NewCarConsultInfo newInfo = null; //新车订单信息
            ReplaceCarConsultInfo replaceInfo = null; //置换订单信息
            Entities.OrderTask orderTaskModel = null;//无主订单任务信息
            Entities.OrderTaskOperationLog orderLog = null;//任务操作日志
            List<StringBuilder> listLogStr = new List<StringBuilder>(); //用户操作日志
            StringBuilder sblogstr = new StringBuilder();
            string logstr = "";

            #region 取得新车/置换无主订单信息更新后Model(此时不保存到数据库)

            Entities.OrderNewCar newModel = null;//新车订单实体类
            Entities.OrderRelpaceCar replaceModel = null;//置换订单实体类

            if (Source == "1" || Source == "3")
            {
                newInfo = (NewCarConsultInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(NewCarConsultInfoStr, typeof(NewCarConsultInfo));

                newModel = OrderNewSave.Save(newInfo, out msg, userID, int.Parse(TaskID));
                if (msg != "")
                {
                    return;
                }
            }
            else if (Source == "2")
            {
                replaceInfo = (ReplaceCarConsultInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(ReplaceCarConsultInfoStr, typeof(ReplaceCarConsultInfo));
                replaceModel = OrderReplaceSave.Save(replaceInfo, out msg, userID, long.Parse(TaskID));
                if (msg != "")
                {
                    return;
                }
            }

            #endregion

            #region 取得任务的更新后信息

            orderTaskModel = BLL.OrderTask.Instance.GetOrderTask(long.Parse(TaskID));
            if (orderTaskModel == null)
            {
                msg += "没有找到相关的任务信息";
                return;
            }

            if (orderTaskModel.TaskStatus != 2 && orderTaskModel.TaskStatus != 3)
            {
                msg += "当前任务状态不能保存和提交";
                return;
            }

            if (orderTaskModel.AssignUserID != userID)
            {
                msg += "此订单没有分配给你";
                return;
            }


            //修改状态
            if (Action == "save")
            {
                orderTaskModel.TaskStatus = (int)TaskStatus.Processing;
            }
            else if (Action == "sub")
            {
                orderTaskModel.TaskStatus = (int)TaskStatus.Processed;
                orderTaskModel.SubmitTime = DateTime.Now;
            }

            //是否已选择经销商 只有在提交时，才会改动该属性 -是否已选择经销商 lxw
            if (Action == "sub")
            {
                if (((Source == "1" || Source == "3") && newModel.DMSMemberCode != "") || (Source == "2" && replaceModel.DMSMemberCode != ""))
                {
                    orderTaskModel.IsSelectDMSMember = true;
                }
                else
                {
                    orderTaskModel.IsSelectDMSMember = false;
                }
            }

            if (Source == "1" || Source == "3")
            {
                orderTaskModel.UserName = newModel.UserName;
            }
            else if (Source == "2")
            {
                orderTaskModel.UserName = replaceModel.UserName;
            }

            if (NoDealerReasonID != "")
            {
                orderTaskModel.NoDealerReasonID = int.Parse(NoDealerReasonID);
            }
            orderTaskModel.NoDealerReason = NoDealerReason;


            #endregion

            #region 任务操作日志

            orderLog = new OrderTaskOperationLog();

            //modify by qizq 2013-1-4首先判断是否是通话中
            if (IsCalling == "1")
            {
                if (HistoryLogID == "")
                {
                    //通话中提交把本地录音主键付给实体
                    long CallRecordReCID = 0;
                    if (CallRecordID != "")
                    {
                        if (long.TryParse(CallRecordID, out CallRecordReCID))
                        {
                            orderLog.CallRecordID = CallRecordReCID;
                        }
                    }
                }
            }
            //


            orderLog.TaskID = int.Parse(TaskID);

            if (Action == "save")
            {
                orderLog.OperationStatus = (int)OperationStatus.Save;
            }
            else if (Action == "sub")
            {
                orderLog.OperationStatus = (int)OperationStatus.Submit;
            }
            orderLog.TaskStatus = orderTaskModel.TaskStatus;
            orderLog.CreateTime = DateTime.Now;
            orderLog.CreateUserID = userID;

            #endregion

            #region 如果是提交操作，更新客户信息、插入咨询类型、添加客户联系记录

            Entities.CustBasicInfo custmodel = null;//客户信息实体类
            Entities.CustTel telPhoneMode = null;//电话实体类
            Entities.CustTel telMobileMode = null;//电话实体类
            Entities.CustEmail emailMode = null;//邮件实体类
            Entities.ConsultOrderNewCar cNewCar = null; //新车咨询类型
            Entities.ConsultOrderRelpaceCar cReplaceCar = null; //置换车咨询类型
            Entities.CustHistoryInfo custHistInfo = null;//客户联系记录

            Entities.BuyCarInfo buyCarInfo = new Entities.BuyCarInfo();//已购车或未购车信息 lxw

            if (Action == "sub")
            {
                #region 更新或插入客户信息
                //代码失效，功能废弃，需求重新实现
                #endregion

                #region 插入咨询类型

                if (Source == "1" || Source == "3")
                {
                    cNewCar = new ConsultOrderNewCar();

                    #region 赋值

                    cNewCar.CarBrandId = newModel.CarMasterID;
                    cNewCar.CarSerialId = newModel.CarSerialID;
                    cNewCar.CarNameID = newModel.CarTypeID;
                    cNewCar.CarColor = newModel.CarColor;
                    cNewCar.DealerCode = newModel.DMSMemberCode;
                    cNewCar.DealerName = newModel.DMSMemberName;
                    cNewCar.OrderRemark = newModel.OrderRemark;
                    cNewCar.CallRecord = newModel.CallRecord;
                    cNewCar.CreateTime = DateTime.Now;
                    cNewCar.CreateUserID = userID;

                    #endregion


                }
                else if (Source == "2")
                {
                    cReplaceCar = new ConsultOrderRelpaceCar();

                    #region 赋值

                    cReplaceCar.WantBrandId = replaceModel.RepCarMasterID;
                    cReplaceCar.WantSerialId = replaceModel.RepCarSerialID;
                    cReplaceCar.WantNameID = replaceModel.RepCarTypeId;
                    cReplaceCar.WantCarColor = replaceModel.ReplacementCarColor;
                    cReplaceCar.WantDealerName = replaceModel.DMSMemberName;
                    cReplaceCar.WantDealerCode = replaceModel.DMSMemberCode;
                    cReplaceCar.CallRecord = replaceModel.CallRecord;
                    cReplaceCar.OldBrandId = replaceModel.CarMasterID;
                    cReplaceCar.OldSerialId = replaceModel.CarSerialID;
                    cReplaceCar.OldNameID = replaceModel.CarTypeID;
                    cReplaceCar.OldCarColor = replaceModel.CarColor;
                    cReplaceCar.RegisterDateYear = replaceModel.ReplacementCarBuyYear.ToString();
                    cReplaceCar.RegisterDateMonth = replaceModel.ReplacementCarBuyMonth.ToString();
                    cReplaceCar.RegisterProvinceID = replaceModel.RepCarProvinceID;
                    cReplaceCar.RegisterCityID = replaceModel.RepCarCityID;
                    cReplaceCar.RegisterCountyID = replaceModel.RepCarCountyID;
                    cReplaceCar.Mileage = (decimal)replaceModel.ReplacementCarUsedMiles;
                    cReplaceCar.PresellPrice = replaceModel.SalePrice;
                    cReplaceCar.OrderRemark = replaceModel.OrderRemark;
                    cReplaceCar.CreateTime = DateTime.Now;
                    cReplaceCar.CreateUserID = userID;

                    #endregion


                }

                #endregion

                #region 插入客户历史记录

                custHistInfo = new CustHistoryInfo();

                #region 赋值

                custHistInfo.TaskID = TaskID;
                if (Source == "1" || Source == "3")
                {
                    custHistInfo.ConsultID = 60010;//新车咨询类型
                }
                else
                {
                    custHistInfo.ConsultID = 60011;//置换咨询类型
                }
                custHistInfo.RecordType = 2;
                custHistInfo.QuestionQuality = (int)QuestionNature.NatureCommon;
                custHistInfo.ProcessStatus = (int)EnumTaskStatus.TaskStatusOver;
                custHistInfo.CreateTime = DateTime.Now;
                custHistInfo.CreateUserID = userID;
                custHistInfo.LastTreatmentTime = DateTime.Now;

                #endregion

                #endregion
            }

            #endregion

            #endregion

            #region 事务提交

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction(IsolationLevel.ReadUncommitted, "SampleTransaction");

            try
            {
                #region 保存新车/置换无主订单信息

                if (Source == "1" || Source == "3")
                {
                    BLL.OrderNewCar.Instance.Update(tran, newModel);
                }
                else if (Source == "2")
                {
                    BLL.OrderRelpaceCar.Instance.Update(tran, replaceModel);
                }

                #endregion

                #region 修改任务信息

                BLL.OrderTask.Instance.Update(tran, orderTaskModel);

                #endregion

                #region 任务操作日志

                //modify by qizq 2013-1-4不是在通话中，处理记录已存在更新处理记录
                if (IsCalling != "1" && HistoryLogID != "")
                {
                    //通话中提交把本地录音主键付给实体
                    long CallRecordReCID = 0;
                    if (CallRecordID != "")
                    {
                        if (long.TryParse(CallRecordID, out CallRecordReCID))
                        {
                            orderLog.CallRecordID = CallRecordReCID;
                        }
                    }
                    long HistoryLogIDLog = 0;
                    if (long.TryParse(HistoryLogID, out HistoryLogIDLog))
                    {
                        orderLog.RecID = HistoryLogIDLog;
                    }
                    BLL.OrderTaskOperationLog.Instance.Update(tran, orderLog);
                }
                else
                {
                    BLL.OrderTaskOperationLog.Instance.Insert(tran, orderLog);
                }
                //



                #endregion

                if (Action == "sub")
                {
                    string retCustID = "";

                    #region 更新或插入客户信息

                    if (custmodel.RecID == -2)
                    {
                        //新加的客户

                        retCustID = BLL.CustBasicInfo.Instance.Insert(tran, custmodel);

                        #region 记日志

                        sblogstr = new StringBuilder();
                        logstr = "";
                        logstr += "新加了客户‘" + custmodel.CustName + "’的信息【ID：" + retCustID + "】";

                        if (logstr != "")
                        {
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);
                        }

                        #endregion
                    }
                    else
                    {
                        //编辑客户信息
                        BLL.CustBasicInfo.Instance.Update(tran, custmodel);
                        retCustID = custmodel.CustID;
                    }
                    #endregion

                    #region 插入电话

                    if (telPhoneMode != null)
                    {
                        telPhoneMode.CustID = retCustID;
                        BLL.CustTel.Instance.Insert(tran, telPhoneMode);
                    }

                    if (telMobileMode != null)
                    {
                        telMobileMode.CustID = retCustID;
                        BLL.CustTel.Instance.Insert(tran, telMobileMode);
                    }

                    #endregion

                    #region 插入邮箱

                    if (emailMode != null)
                    {
                        emailMode.CustID = retCustID;
                        BLL.CustEmail.Instance.Insert(tran, emailMode);
                    }

                    #endregion

                    #region 插入咨询类型

                    int retDataID = 0;

                    if (cNewCar != null)
                    {
                        cNewCar.CustID = retCustID;
                        retDataID = BLL.ConsultOrderNewCar.Instance.Insert(tran, cNewCar);

                        #region 记日志

                        sblogstr = new StringBuilder();
                        logstr = "";
                        logstr += "新加了无主订单新车联系咨询记录【ID：" + retDataID + "】";

                        if (logstr != "")
                        {
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);
                        }

                        #endregion
                    }
                    if (cReplaceCar != null)
                    {
                        cReplaceCar.CustID = retCustID;
                        retDataID = BLL.ConsultOrderRelpaceCar.Instance.Insert(tran, cReplaceCar);

                        #region 记日志

                        sblogstr = new StringBuilder();
                        logstr = "";
                        logstr += "新加了无主订单置换联系咨询记录【ID：" + retDataID + "】";

                        if (logstr != "")
                        {
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);
                        }

                        #endregion
                    }

                    #endregion

                    #region 插入未购车或已购车记录 BuyCarInfo lxw

                    if (buyCarInfo != null && buyCarInfo.RecID == -2)
                    {
                        buyCarInfo.CustID = retCustID;
                        int recID = BLL.BuyCarInfo.Instance.Insert(tran, buyCarInfo);

                        #region 记日志

                        sblogstr = new StringBuilder();
                        logstr = "";
                        logstr += "新加了未购车已购车BuyCarInfo表的记录【ID：" + recID + "】";

                        if (logstr != "")
                        {
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);
                        }

                        #endregion
                    }

                    if (buyCarInfo != null && buyCarInfo.RecID != -2)
                    {
                        BLL.BuyCarInfo.Instance.Update(tran, buyCarInfo);

                        #region 记日志

                        sblogstr = new StringBuilder();
                        logstr = "";
                        logstr += "修改了未购车已购车BuyCarInfo表的记录【ID：" + buyCarInfo.RecID + "】";

                        if (logstr != "")
                        {
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);
                        }

                        #endregion
                    }

                    #endregion

                    #region 插入客户联系记录

                    if (custHistInfo != null)
                    {
                        custHistInfo.CustID = retCustID;
                        custHistInfo.ConsultDataID = retDataID;
                        BLL.CustHistoryInfo.Instance.Insert(tran, custHistInfo);
                    }

                    #endregion

                    #region 插入已购车或未购车信息

                    #endregion

                    #region 保存用户操作日志

                    foreach (StringBuilder sbStr in listLogStr)
                    {
                        BLL.Util.InsertUserLog(tran, sbStr.ToString());
                    }
                    #endregion
                }

                if (msg == "")
                {
                    tran.Commit();
                    if (Action == "sub")
                    {
                        #region 调用易湃接口，传回数据

                        string errorMsg = "";
                        int retService = 0;
                        short isSelectDMS = 0;

                        #region 调用接口

                        BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper OrderHelper = new BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper();
                        if (Source == "1")//新车
                        {
                            isSelectDMS = newModel.DMSMemberCode == "" ? (short)2 : (short)1; //1 选择了经销商  2 未选择经销商
                            retService = OrderHelper.SetNewCarOrder(newModel, isSelectDMS, NoDealerReason, ref errorMsg);
                        }
                        else if (Source == "2")//置换
                        {
                            isSelectDMS = replaceModel.DMSMemberCode == "" ? (short)2 : (short)1; //1 选择了经销商  2 未选择经销商
                            retService = OrderHelper.SetReplacementOrder(replaceModel, isSelectDMS, NoDealerReason, ref errorMsg);
                        }
                        else if (Source == "3")
                        {
                            isSelectDMS = newModel.DMSMemberCode == "" ? (short)2 : (short)1; //1 选择了经销商  2 未选择经销商
                            retService = OrderHelper.SetTestDriveOrder(newModel, isSelectDMS, NoDealerReason, ref errorMsg);
                        }

                        #endregion

                        #region 插入更新无主订单数据日志表

                        Entities.UpdateOrderData updateDateMode = new UpdateOrderData();
                        updateDateMode.TaskID = TaskID;

                        if (Source == "1" || Source == "3")
                        {
                            updateDateMode.YPOrderID = newModel.YPOrderID;
                        }
                        else if (Source == "2")
                        {
                            updateDateMode.YPOrderID = replaceModel.YPOrderID;
                        }

                        updateDateMode.UpdateType = retService;
                        updateDateMode.IsUpdate = retService; // 1 成功了，不用处理，-1 需要重新处理
                        if (retService == 1)
                        {
                            updateDateMode.UpdateDateTime = DateTime.Now;
                        }
                        updateDateMode.UpdateErrorMsg = errorMsg;
                        updateDateMode.CreateTime = DateTime.Now;
                        updateDateMode.CreateUserID = userID;
                        updateDateMode.APIType = 1;

                        BLL.UpdateOrderData.Instance.Insert(updateDateMode);

                        #endregion

                        if (retService == -1)
                        {
                            msg = "InterfaceErr|" + errorMsg;
                        }

                        #endregion
                    }
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                msg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }



            #endregion
        }

        //绑定颜色
        private void bindColor(out string msg)
        {
            msg = string.Empty;
            int _carTypeID;
            if (int.TryParse(CarTypeID, out _carTypeID))
            {
                DataTable dt = BLL.CarTypeAPI.Instance.GetCarColorByCarTypeID(_carTypeID);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    msg = "{'color':'";
                }
                string colorName = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    colorName += dt.Rows[i]["Name"].ToString() + ",";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg + colorName.TrimEnd(',') + "'}";
                }
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