using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;
using System.Diagnostics;
using BitAuto.Utils;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    /// <summary>
    /// WorkOrderOperHandler 的摘要说明
    /// </summary>
    public class WorkOrderOperHandler : IHttpHandler, IRequiresSessionState
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

        private string PID
        {
            get
            {
                if (Request["PID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["PID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string BGID
        {
            get
            {
                if (Request["BGID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["BGID"].ToString());
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

        private string UseScope
        {
            get
            {
                if (Request["UseScope"] != null)
                {
                    return HttpUtility.UrlDecode(Request["UseScope"].ToString());
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

        private string RevertData
        {
            get
            {
                if (Request["RevertData"] != null)
                {
                    return Request["RevertData"].ToString();
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
        //OperType=1：添加；OperType=2：转出
        private string OperType
        {
            get
            {
                if (Request["OperType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["OperType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string CrmCustID
        {
            get
            {
                if (Request["CrmCustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CrmCustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string LastProcessDate
        {
            get
            {
                if (Request["LastProcessDate"] != null)
                {
                    return HttpUtility.UrlDecode(Request["LastProcessDate"].ToString());
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

        private string CallRecID
        {
            get
            {
                if (Request["CallRecID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CallRecID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string CustBasicInfo
        {
            get
            {
                if (Request["CustBasicInfo"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustBasicInfo"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private int TaskSource
        {
            get
            {
                if (Request["TaskSource"] != null)
                {
                    return CommonFunction.ObjectToInteger(HttpUtility.UrlDecode(Request["TaskSource"].ToString()));
                }
                else
                {
                    return 0;
                }
            }
        }

        private string IsCustBasic  //IsCustBasic=true,需要验证客户联系人是否在个人库
        {
            get
            {
                if (Request["IsCustBasic"] != null)
                {
                    return HttpUtility.UrlDecode(Request["IsCustBasic"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string ActivityIDs
        {
            get
            {
                if (Request["ActivityIDs"] != null)
                {
                    return HttpUtility.UrlDecode(Request["ActivityIDs"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string DemandID
        {
            get
            {
                if (Request["DemandID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["DemandID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private DateTime operTime;
        private int operUserID;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = string.Empty;
            string where = string.Empty;

            operTime = DateTime.Now;
            operUserID = BLL.Util.GetLoginUserID();

            switch (Action)
            {
                case "dealerSubmit":
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    DealerSubmit(out msg);
                    stopwatch.Stop();
                    BLL.Loger.Log4Net.Info(string.Format("【经销商工单提交Step12——总耗时】：{0}毫秒", stopwatch.Elapsed.TotalMilliseconds));
                    break;

                case "personalSubmit":
                    Stopwatch stopwatch2 = new Stopwatch();
                    stopwatch2.Start();
                    PersonalSubmit(out msg);
                    stopwatch2.Stop();
                    BLL.Loger.Log4Net.Info(string.Format("【个人工单提交Step7——总耗时】：{0}毫秒", stopwatch2.Elapsed.TotalMilliseconds));
                    break;

                case "GetCategory1":
                    where = " and level=1";
                    if (UseScope != string.Empty)
                    {
                        where += " and UseScope in (" + BLL.Util.SqlFilterByInCondition(UseScope) + ")";
                    }
                    msg = BLL.WorkOrderCategory.Instance.GetWorkCategoryJsonBySql(where);
                    break;
                case "GetCategory2":
                    if (PID != string.Empty)
                    {
                        where += " and pid=" + PID;
                        msg = BLL.WorkOrderCategory.Instance.GetWorkCategoryJsonBySql(where);
                    }
                    break;
                case "GetCategory":
                    if (PID != string.Empty)
                    {
                        where += " and pid=" + PID;
                        msg = BLL.WorkOrderCategory.Instance.GetWorkCategoryJsonBySql(where);
                    }
                    break;
                case "GetCustUser":
                    GetCustUser(out msg);
                    break;
                case "GetWorkOrderTag":
                    if (PID != string.Empty && BGID != string.Empty)
                    {
                        where += " and pid=" + PID;
                        where += " and bgid=" + BGID;
                        msg = BLL.WorkOrderTag.Instance.GetWorkOrderTagJsonBySql(where);
                    }
                    break;
                case "GetWorkOrderRecordUrl":
                    if (OrderID != string.Empty)
                    {
                        DataTable dt = null;
                        //只取录音地址
                        dt = BLL.WorkOrderInfo.Instance.GetWorkOrderRecordUrl_OrderID(OrderID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            msg = "{'result':'true','RecordURL':'" + dt.Rows[0]["AudioURL"] + "'}";
                        }
                        else
                        {
                            msg = "{'result':'false'}";
                        }
                    }
                    break;
            }
            context.Response.Write(msg);
        }

        private void DealerSubmit(out string msg)
        {
            msg = string.Empty;
            try
            {
                BLL.Loger.Log4Net.Info("【经销商工单提交Step1——参数验证】CallID IS:" + CallID);

                #region 验证
                validate(out msg);
                if (LastProcessDate != string.Empty)
                {
                    DateTime dtLastProcessDate;
                    if (DateTime.TryParse(LastProcessDate, out dtLastProcessDate))
                    {
                        if (dtLastProcessDate < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            msg += "最晚处理日期不能小于当前时间！<br/>";
                        }
                    }
                }

                if (msg != string.Empty)
                {
                    msg = "{'result':'false','msg':'" + msg + "'}";

                    return;
                }
                #endregion

                #region 插入工单表
                BLL.Loger.Log4Net.Info("【经销商工单提交Step2——绑定工单信息】");
                Entities.WorkOrderInfo model_WorkOrderInfo = bindWorkOrderInfo(out msg);

                if (msg != string.Empty)
                {
                    return;
                }

                string orderCustID = string.Empty;
                //根据CRMCustName查询获取CustID
                BLL.Loger.Log4Net.Info("【经销商工单提交Step3——根据CRMCustName查询获取CustID开始】");
                DataSet ds_CustID = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustIDByNameToCC(model_WorkOrderInfo.CustName);
                BLL.Loger.Log4Net.Info("【经销商工单提交Step3——根据CRMCustName查询获取CustID结束】");
                if (ds_CustID != null && ds_CustID.Tables.Count > 0 && ds_CustID.Tables[0].Rows.Count > 0)
                {
                    model_WorkOrderInfo.CRMCustID = orderCustID = ds_CustID.Tables[0].Rows[0]["CustID"].ToString();
                }

                //OperType=1：添加（状态：已完成）；OperType=2：转出（状态：待审核）
                model_WorkOrderInfo.WorkOrderStatus = OperType == "1" ? (int)Entities.WorkOrderStatus.Completed : (int)Entities.WorkOrderStatus.Pending;

                model_WorkOrderInfo.WorkCategory = 2;//工单类型-经销商；add lxw 13.9.11

                model_WorkOrderInfo.DemandID = DemandID;
                BLL.Loger.Log4Net.Info("【经销商工单提交Step4——WorkOrderInfo入库开始】");
                model_WorkOrderInfo.OrderID = BLL.WorkOrderInfo.Instance.Insert(model_WorkOrderInfo);
                BLL.Loger.Log4Net.Info("【经销商工单提交Step4——WorkOrderInfo入库结束】OrderID=：" + OrderID);


                //回写IM库 
                BLL.Loger.Log4Net.Info("【经销商工单提交Step5——工单信息回写IM库开始】");
                UpdateCCWorkOrder2IM(model_WorkOrderInfo);
                BLL.Loger.Log4Net.Info("【经销商工单提交Step5——工单信息回写IM库结束】");
                #endregion

                #region 插入工单回复表
                BLL.Loger.Log4Net.Info("【经销商工单提交Step6——绑定工单回复信息】OrderID=" + model_WorkOrderInfo.OrderID + ",CallID=" + CallID);
                Entities.WorkOrderRevert model_Revert = bindWorkOrderRevert(out msg);

                model_Revert.OrderID = model_WorkOrderInfo.OrderID;
                if (CallID != string.Empty)
                {
                    model_Revert.CallID = Int64.Parse(CallID);
                }

                if (msg != string.Empty)
                {
                    return;
                }

                model_Revert.WorkOrderStatus = OperType == "1" ? "已完成" : "待审核";
                BLL.Loger.Log4Net.Info("【经销商工单提交Step7——根据部门获取部门名称开始】");
                model_Revert.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(operUserID);
                BLL.Loger.Log4Net.Info("【经销商工单提交Step7——根据部门获取部门名称结束】");

                BLL.Loger.Log4Net.Info("【经销商工单提交Step8——工单回复信息入库开始】");
                BLL.WorkOrderRevert.Instance.Insert(model_Revert);
                BLL.Loger.Log4Net.Info("【经销商工单提交Step8——工单回复信息入库结束】");
                #endregion

                #region 插入标签
                if (TagIDs != string.Empty)
                {
                    string[] array_TagIDs = TagIDs.Split(',');
                    for (int k = 0; k < array_TagIDs.Length; k++)
                    {
                        Entities.WorkOrderTagMapping model_Mapping = new Entities.WorkOrderTagMapping();
                        model_Mapping.OrderID = model_WorkOrderInfo.OrderID;
                        model_Mapping.TagID = int.Parse(array_TagIDs[k]);
                        model_Mapping.Status = 0;
                        model_Mapping.CreateTime = model_Mapping.ModifyTime = operTime;
                        model_Mapping.CreateUserID = model_Mapping.ModifyUserID = operUserID;
                        BLL.Loger.Log4Net.Info("【经销商工单提交Step9——插入标签开始】");
                        BLL.WorkOrderTagMapping.Instance.Insert(model_Mapping);
                        BLL.Loger.Log4Net.Info("【经销商工单提交Step9——插入标签结束】");
                    }
                }

                #endregion

                #region 把工单联系人插入个人用户
                Web.AjaxServers.CustBaseInfo.CustBasicInfo model_cbInfo = bindCustBasicInfo(out msg);
                if (msg != string.Empty)
                {
                    return;
                }
                Web.AjaxServers.CustBaseInfo.OperPopCustBasicInfo cbi = new Web.AjaxServers.CustBaseInfo.OperPopCustBasicInfo();
                BLL.Loger.Log4Net.Info("【经销商工单提交Step10——插入个人用户库信息开始】");
                cbi.InsertCustInfo(model_cbInfo, out msg);
                BLL.Loger.Log4Net.Info("【经销商工单提交Step10——插入个人用户库信息结束】");
                string ccCustID = string.Empty;
                string[] aMsg = msg.Split(',');
                //如果不存在会新增个人用户 返回ccCustID
                //如果存在会直接返回该联系人ccCustID
                if (aMsg.Length == 2 && aMsg[0].Split(':')[1] == "'true'")
                {
                    ccCustID = aMsg[1].Split(':')[1].Replace("'", "");
                }
                #endregion

                #region 插入访问记录
                if (ccCustID != string.Empty)
                {
                    BLL.Loger.Log4Net.Info("【经销商工单提交Step11——插入联系记录开始】");
                    long callid = CommonFunction.ObjectToLong(CallID, -2);
                    int userid = BLL.Util.GetLoginUserID();

                    BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.OperPopCustBasicInfo.OperCustVisitBusiness(ccCustID, model_WorkOrderInfo.OrderID, callid, (int)VisitBusinessTypeEnum.S1_工单, TaskSource, userid, model_WorkOrderInfo.ContactTel);
                    BLL.Loger.Log4Net.Info("【经销商工单提交Step11——插入联系记录结束】");
                }
                #endregion

                msg = "{'result':'true','OrderID':'" + model_WorkOrderInfo.OrderID + "','orderCustID':'" + orderCustID + "'}";
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【经销商工单提交】出现方法出现问题：" + ex.Message);
            }
        }

        private void PersonalSubmit(out string msg)
        {
            msg = string.Empty;
            try
            {
                BLL.Loger.Log4Net.Info("【个人工单提交Step1——数据验证】");
                #region 验证

                validate(out msg);

                if (msg != string.Empty)
                {
                    msg = "{'result':'false','msg':'" + msg + "'}";
                    return;
                }

                #endregion

                #region 插入工单表

                Entities.WorkOrderInfo model_WorkOrderInfo = bindWorkOrderInfo(out msg);

                if (msg != string.Empty)
                {
                    return;
                }

                //个人类型增加工单，状态默认-已完成
                model_WorkOrderInfo.WorkOrderStatus = (int)Entities.WorkOrderStatus.Completed;

                model_WorkOrderInfo.WorkCategory = 1;//工单类型-个人；add lxw 13.9.11
                BLL.Loger.Log4Net.Info("【个人工单提交Step2——添加工单开始】");
                model_WorkOrderInfo.OrderID = BLL.WorkOrderInfo.Instance.Insert(model_WorkOrderInfo);
                BLL.Loger.Log4Net.Info("【个人工单提交Step2——添加工单结束】OrderID=" + model_WorkOrderInfo.OrderID);
                //回写IM库  
                BLL.Loger.Log4Net.Info("【个人工单提交Step3——工单数据回写IM开始】");
                UpdateCCWorkOrder2IM(model_WorkOrderInfo);
                BLL.Loger.Log4Net.Info("【个人工单提交Step3——工单数据回写IM结束】");
                #endregion

                //如果有推荐活动，插入WorkOrderActivity表，add lxw 13.1.14
                #region 插入推荐活动

                if (ActivityIDs != "")
                {
                    string[] aIDs = ActivityIDs.Split(',');
                    for (int i = 0; i < aIDs.Length; i++)
                    {
                        Entities.WorkOrderActivity model_Activity = new Entities.WorkOrderActivity();
                        model_Activity.ActivityGUID = new Guid(aIDs[i]);
                        model_Activity.OrderID = model_WorkOrderInfo.OrderID;
                        model_Activity.CreateTime = operTime;
                        model_Activity.CreateUserID = operUserID;
                        BLL.Loger.Log4Net.Info("【个人工单提交Step4——WorkOrderActivity数据入库开始】");
                        BLL.WorkOrderActivity.Instance.Insert(model_Activity);
                        BLL.Loger.Log4Net.Info("【个人工单提交Step4——WorkOrderActivity数据入库结束】");
                    }
                }

                #endregion

                #region 插入工单回复表

                Entities.WorkOrderRevert model_Revert = bindWorkOrderRevert(out msg);

                if (msg != string.Empty)
                {
                    return;
                }

                model_Revert.OrderID = model_WorkOrderInfo.OrderID;
                model_Revert.ReceiverDepartName = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetDistrictNameByUserID(operUserID);
                model_Revert.WorkOrderStatus = "已完成";
                if (CallID != string.Empty)
                {
                    model_Revert.CallID = Int64.Parse(CallID);
                }
                BLL.Loger.Log4Net.Info("【个人工单提交Step5——WorkOrderRevert数据入库开始】");
                BLL.WorkOrderRevert.Instance.Insert(model_Revert);
                BLL.Loger.Log4Net.Info("【个人工单提交Step5——WorkOrderRevert数据入库结束】");
                #endregion

                #region 插入标签

                if (TagIDs != string.Empty)
                {
                    string[] array_TagIDs = TagIDs.Split(',');
                    for (int k = 0; k < array_TagIDs.Length; k++)
                    {
                        Entities.WorkOrderTagMapping model_Mapping = new Entities.WorkOrderTagMapping();
                        model_Mapping.OrderID = model_WorkOrderInfo.OrderID;
                        model_Mapping.TagID = int.Parse(array_TagIDs[k]);
                        model_Mapping.Status = 0;
                        model_Mapping.CreateTime = model_Mapping.ModifyTime = operTime;
                        model_Mapping.CreateUserID = model_Mapping.ModifyUserID = operUserID;
                        BLL.Loger.Log4Net.Info("【个人工单提交Step6——插入标签数据入库开始】");
                        BLL.WorkOrderTagMapping.Instance.Insert(model_Mapping);
                        BLL.Loger.Log4Net.Info("【个人工单提交Step6——插入标签数据入库结束】");
                    }
                }

                #endregion
                msg = "{'result':'true','OrderID':'" + model_WorkOrderInfo.OrderID + "'}";
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【个人工单提交】出现方法出现问题：" + ex.Message);
            }
        }

        private void UpdateCCWorkOrder2IM(Entities.WorkOrderInfo model_WorkOrderInfo)
        {
            try
            {
                //回写IM库，首先判断是 在线留言还是会话新增工单。
                int imtype = -2;
                int id = -2;
                if (model_WorkOrderInfo.CSID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    //说明是 会话新增工单
                    imtype = 1;
                    id = model_WorkOrderInfo.CSID;
                }

                if (model_WorkOrderInfo.LYID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    //说明是 留言新增工单
                    imtype = 2;
                    id = model_WorkOrderInfo.LYID;
                }

                if (imtype > -2)
                {
                    string retval = "";
                    if (model_WorkOrderInfo.SYSType == "isIM")
                    {
                        if (model_WorkOrderInfo.CarType == 1)
                        {
                            retval = WebService.IM.IMepUtilsServiceHelper.Instance.UpdateCCWorkOrderToIM(imtype, id, model_WorkOrderInfo.OrderID);
                        }
                        else if (model_WorkOrderInfo.CarType == 2)
                        {
                            retval = WebService.IM.IMtcUtilsServiceHelper.Instance.UpdateCCWorkOrderToIM(imtype, id, model_WorkOrderInfo.OrderID);
                        }
                    }
                    else if (model_WorkOrderInfo.SYSType == "isIM2")
                    {
                        retval = WebService.IM.IMUtilsServiceHelper.Instance.UpdateCCWorkOrderToIM(imtype, id, model_WorkOrderInfo.OrderID);
                    }
                    if (string.IsNullOrEmpty(retval))
                    {
                        if (imtype == 1)
                        {
                            BLL.Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]DelaerSubmit 回写IM会话表成功，工单号:" + model_WorkOrderInfo.OrderID + ",会话ID:" + model_WorkOrderInfo.CSID);
                        }
                        else if (imtype == 2)
                        {
                            BLL.Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]DelaerSubmit 回写IM留言表成功，工单号:" + model_WorkOrderInfo.OrderID + ",留言ID:" + model_WorkOrderInfo.LYID);
                        }
                    }
                    else
                    {
                        if (imtype == 1)
                        {
                            BLL.Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]DelaerSubmit 回写IM会话表失败，工单号:" + model_WorkOrderInfo.OrderID + ",会话ID:" + model_WorkOrderInfo.CSID);
                            BLL.Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]DelaerSubmit 回写IM会话表出错:" + retval);
                        }
                        else if (imtype == 2)
                        {
                            BLL.Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]DelaerSubmit 回写IM留言表失败，工单号:" + model_WorkOrderInfo.OrderID + ",留言ID:" + model_WorkOrderInfo.LYID);
                            BLL.Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]DelaerSubmit 回写IM留言表出错:" + retval);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]UpdateCCWorkOrder2IM 出错!errorMessage:" + ex.Message);
                Loger.Log4Net.Info("[WorkOrderOperHandlerr.ashx.cs]UpdateCCWorkOrder2IM 出错!errorStackTrace:" + ex.StackTrace);
            }

        }

        //验证
        private void validate(out string msg)
        {
            msg = string.Empty;

            Web.AjaxServers.ValidateDataFormat validate = new Web.AjaxServers.ValidateDataFormat();
            validate.Validate(ValidateData, out msg);

            if (msg != string.Empty)
            {
                return;
            }
        }

        //绑定工单表信息
        private Entities.WorkOrderInfo bindWorkOrderInfo(out string msg)
        {
            msg = string.Empty;

            Entities.WorkOrderInfo model_WorkOrderInfo = new Entities.WorkOrderInfo();

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.WorkOrderInfo> conver = new BLL.ConverToEntitie<Entities.WorkOrderInfo>(model_WorkOrderInfo);
            errMsg = conver.Conver(OperData);

            if (errMsg != "")
            {
                msg = "{'result':'false','msg':'给对应实体赋值时出错，操作失败！'}";
                return model_WorkOrderInfo;
            }

            //生成工单ID放在数据库Insert存储过程中 update lxw 13.10.23

            //int maxCount = BLL.WorkOrderInfo.Instance.GetMax();
            //model_WorkOrderInfo.OrderID = "WO" + DateTime.Now.Year.ToString() + (++maxCount).ToString().PadLeft(10, '0');

            model_WorkOrderInfo.CreateUserID = model_WorkOrderInfo.ModifyUserID = operUserID;
            model_WorkOrderInfo.CreateTime = model_WorkOrderInfo.ModifyTime = operTime;
            model_WorkOrderInfo.Status = 0;

            return model_WorkOrderInfo;
        }

        //绑定工单回复表信息
        private Entities.WorkOrderRevert bindWorkOrderRevert(out string msg)
        {
            msg = string.Empty;

            string errMsg = string.Empty;

            Entities.WorkOrderRevert model_Revert = new Entities.WorkOrderRevert();
            BLL.ConverToEntitie<Entities.WorkOrderRevert> converRevert = new BLL.ConverToEntitie<Entities.WorkOrderRevert>(model_Revert);

            errMsg = converRevert.Conver(RevertData);

            if (errMsg != "")
            {
                msg = "{'result':'false','msg':'给对应实体赋值时出错，操作失败！'}";
                return model_Revert;
            }
            model_Revert.CreateTime = operTime;
            model_Revert.CreateUserID = operUserID;

            return model_Revert;
        }

        //绑定个人用户库信息表信息 add lxw 13.11.25
        private Web.AjaxServers.CustBaseInfo.CustBasicInfo bindCustBasicInfo(out string msg)
        {
            msg = string.Empty;

            Web.AjaxServers.CustBaseInfo.CustBasicInfo model = new Web.AjaxServers.CustBaseInfo.CustBasicInfo();

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Web.AjaxServers.CustBaseInfo.CustBasicInfo> conver = new BLL.ConverToEntitie<Web.AjaxServers.CustBaseInfo.CustBasicInfo>(model);
            errMsg = conver.Conver(CustBasicInfo);

            if (errMsg != "")
            {
                msg = "{'result':'false','msg':'给对应实体赋值时出错，操作失败！'}";
                return model;
            }

            model.OperID = operUserID;
            model.OperTime = operTime;

            return model;
        }

        /// <summary>
        /// 获取客户下的负责人员
        /// </summary>
        /// <param name="msg"></param>
        private void GetCustUser(out string msg)
        {
            msg = "{totalcount:0,msg:'此客户下无相关负责人'}";
            if (!string.IsNullOrEmpty(CrmCustID.Trim()))
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustUserMapping query = new YanFa.Crm2009.Entities.QueryCustUserMapping();
                query.CustID = CrmCustID;
                query.UserStatus = 0;
                int totalCount = 0;
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.GetCustUserMapping(query, "", 1, 100, out totalCount);

                if (totalCount == 1)
                {
                    msg = "{totalcount:1,userinfo:{userid:'" + dt.Rows[0]["UserID"].ToString() + "',username:'" + dt.Rows[0]["TrueName"].ToString() + "'},msg:''}";
                }
                else if (totalCount > 1)
                {
                    msg = "{totalcount:" + totalCount + ",msg:'此客户下的有多个负责人'}";
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