using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 工单相关
    /// <summary>
    /// 工单相关
    /// </summary>
    public class WOrderInfo
    {
        public static WOrderInfo Instance = new WOrderInfo();
        Random random = new Random();

        /// 计算3个月内呼入来电次数
        /// <summary>
        /// 计算3个月内呼入来电次数
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int CalcCallInCountByPhone(string phone)
        {
            return Dal.WOrderInfo.Instance.CalcCallInCountByPhone(phone);
        }
        /// 查询个人用户信息
        /// <summary>
        /// 查询个人用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public DataTable GetCBInfoByPhone(string custid, string phone)
        {
            return Dal.WOrderInfo.Instance.GetCBInfoByPhone(custid, phone);
        }

        /// 日志记录
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="info"></param>
        public static void LogToLog4(string info)
        {
            Loger.Log4Net.Info("[****新增工单V2****] " + info);
        }
        public static void LogForWebForCall(string msg, int loginuserid, string loginusername)
        {
            BLL.Util.LogForWebForModule("话务相关", "工单", msg, loginuserid, loginusername);
        }
        /// 日志记录
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ex"></param>
        public static void ErrorToLog4(string name, Exception ex)
        {
            Loger.Log4Net.Error("[****新增工单V2****] " + name, ex);
        }

        /// 生成工单id
        /// <summary>
        /// 生成工单id
        /// </summary>
        /// <returns></returns>
        private string GetNewWOrderID()
        {
            string str = "WO" + DateTime.Now.ToString("yyMMddHHmmss") + random.Next(100, 999);
            return str;
        }

        /// 保存个人信息
        /// <summary>
        /// 保存个人信息
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveCustBasicInfo(WOrderJsonData jsondata, int loginuserid)
        {
            //构造个人主表数据
            Entities.CustBasicInfo model = new Entities.CustBasicInfo();
            model.CustName = jsondata.CustBaseInfo.CBName_Out;
            model.Sex = jsondata.CustBaseInfo.CBSex_Out;
            model.CustCategoryID = (int)jsondata.CustBaseInfo.CustTypeID_Out;
            model.ProvinceID = jsondata.CustBaseInfo.ProvinceID_Out;
            model.CityID = jsondata.CustBaseInfo.CityID_Out;
            model.CountyID = jsondata.CustBaseInfo.CountyID_Out;
            //废弃字段
            model.Address = null;
            model.DataSource = null;
            model.CallTime = null;
            model.Status = 0;

            //查询数据库
            string cbid = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(jsondata.CustBaseInfo.Phone_Out);
            if (!string.IsNullOrEmpty(cbid))
            {
                //更新
                model.CustID = cbid;
                model.ModifyTime = DateTime.Now;
                model.ModifyUserID = loginuserid;
                BLL.CustBasicInfo.Instance.Update(model);
            }
            else
            {
                //新增
                model.CreateTime = DateTime.Now;
                model.CreateUserID = loginuserid;
                model.ModifyTime = null;
                model.ModifyUserID = null;
                cbid = BLL.CustBasicInfo.Instance.Insert(model);

                //插入电话信息
                Entities.CustTel model_Tel = new Entities.CustTel();
                model_Tel.CustID = cbid;
                model_Tel.Tel = jsondata.CustBaseInfo.Phone_Out;
                model_Tel.CreateTime = DateTime.Now;
                model_Tel.CreateUserID = loginuserid;
                BLL.CustTel.Instance.Insert(model_Tel);
            }
            //存储
            jsondata.CBID = cbid;
            //删除经销商信息
            BLL.DealerInfo.Instance.Delete(cbid);
            //存储经销商信息
            if (jsondata.CustBaseInfo.CustTypeID_Out == CustTypeEnum.T02_经销商)
            {
                //插入经销商信息
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
        /// 保存工单信息
        /// <summary>
        /// 保存工单信息
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveWOrderInfo(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            //工单主表 WOrderInfo
            SaveWOrderMainInfo(jsondata, sysinfo);
            //保存WOrderProcess表
            SaveWOrderProcessInfo(jsondata, sysinfo);
            //附件表 CommonAttachment
            SaveCommonAttachment(jsondata, sysinfo);
        }
        /// 保存工单主表信息
        /// <summary>
        /// 保存工单主表信息
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
            //存储工单id
            jsondata.WOrderID = main.OrderID;
        }
        /// 保存工单处理记录
        /// <summary>
        /// 保存工单处理记录
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
            process.ProcessContent = sysinfo.TrueName + jsondata.Oper + "了工单 [" + jsondata.WOrderID + "]";
            process.Status = 0;
            process.CreateUserID = sysinfo.UserID;
            process.CreateUserNum = sysinfo.UserCode;
            process.CreateUserName = sysinfo.TrueName;
            process.CreateUserDeptName = sysinfo.MainDepartName;
            process.CreateTime = DateTime.Now;
            CommonBll.Instance.InsertComAdoInfo(process);
        }
        /// 保存附件信息
        /// <summary>
        /// 保存附件信息
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

        /// 保存绑定信息
        /// <summary>
        /// 保存绑定信息
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveBindInfo(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            try
            {
                LogForWebForCall("[保存绑定信息]Start", loginuserid, loginusername);
                //来去电表 CallRecordInfo，话务业务表 CallRecord_ORIG_Business，工单关联数据表 WOrderData，短信表 SMSSendHistory
                //话务信息
                if (jsondata.CustBaseInfo.CallIds_Out != null && jsondata.CustBaseInfo.CallIds_Out.Count > 0)
                {
                    foreach (string callid in jsondata.CustBaseInfo.CallIds_Out)
                    {
                        long callid_val = CommonFunction.ObjectToLong(callid);
                        //工单关联数据表 WOrderData
                        BLL.WOrderData.Instance.InsertWOrderDataForCC(jsondata.WOrderID, -1, callid_val, loginuserid);
                        LogForWebForCall("[保存绑定信息]工单关联数据表：工单ID=" + jsondata.WOrderID + "；话务ID=" + callid_val, loginuserid, loginusername);
                    }
                    string callids = string.Join(",", jsondata.CustBaseInfo.CallIds_Out.ToArray());
                    //来去电表 CallRecordInfo (CustID,CustName,TaskTypeID,TaskID)
                    BLL.CallAfterTaskProcess.Instance.UpdateCallRecordInfoAfterTask(callids, jsondata.CBID, jsondata.CustBaseInfo.CBName_Out, ProjectSource.S3_工单, jsondata.WOrderID);
                    //话务业务表 CallRecord_ORIG_Business (BusinessID)
                    BLL.CallAfterTaskProcess.Instance.UpdateCallRecord_ORIG_BusinessAfterTask(callids, jsondata.WOrderID);

                    LogForWebForCall("[保存绑定信息]来去电表和话务业务表：工单ID=" + jsondata.WOrderID + "；话务ID=" + callids + "；CBID=" + jsondata.CBID, loginuserid, loginusername);
                }
                //对话信息
                if ((jsondata.Common.CallSource_Out == CallSourceEnum.C03_IM对话 || jsondata.Common.CallSource_Out == CallSourceEnum.C04_IM留言)
                    && jsondata.Common.RelatedCSID > 0)
                {
                    //工单关联数据表 WOrderData
                    BLL.WOrderData.Instance.InserWOrderDataForIM(jsondata.WOrderID, -1, jsondata.Common.RelatedCSID, loginuserid);
                    LogForWebForCall("[保存绑定信息]工单关联数据表：工单ID=" + jsondata.WOrderID + "；会话ID=" + jsondata.Common.RelatedCSID, loginuserid, loginusername);
                }
                //未接来电
                if (jsondata.Common.ModuleSource_Out == ModuleSourceEnum.M04_未接来电 && jsondata.Common.RelatedMissID > 0)
                {
                    //保存工单id和cbid到未接来电表 CustomerVoiceMsg
                    BLL.CustomerVoiceMsg.Instance.UpdateCustomerVoiceMsgInfoAfterNewOrder(jsondata.Common.RelatedMissID, jsondata.WOrderID, jsondata.CBID);
                    LogForWebForCall("[保存绑定信息]保存工单id和cbid到未接来电表：RecID=" + jsondata.Common.RelatedMissID, loginuserid, loginusername);
                }
                //短信信息
                if (jsondata.CustBaseInfo.SmsIds_Out != null && jsondata.CustBaseInfo.SmsIds_Out.Count > 0)
                {
                    string smsids = string.Join(",", jsondata.CustBaseInfo.SmsIds_Out.ToArray());
                    //短信表 SMSSendHistory (CustID,CRMCustID,TaskType,TaskID)
                    BLL.CallAfterTaskProcess.Instance.UpdateSMSSendHistoryAfterTask(smsids, jsondata.CBID, jsondata.CRMCustID_Out, ProjectSource.S3_工单, jsondata.WOrderID);
                    LogForWebForCall("[保存绑定信息]短信表 SMSSendHistory：smsids=" + smsids, loginuserid, loginusername);
                }
            }
            catch (Exception ex)
            {
                LogForWebForCall("[保存绑定信息]保存绑定信息 " + ex.Message + ex.StackTrace, loginuserid, loginusername);
            }
            LogForWebForCall("[保存绑定信息]END\r\n", loginuserid, loginusername);
        }
        /// 保存绑定信息
        /// <summary>
        /// 保存绑定信息
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveBindInfoASync(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            Action<WOrderJsonData, int, string> func = new Action<WOrderJsonData, int, string>(SaveBindInfo);
            func.BeginInvoke(jsondata, loginuserid, loginusername, null, null);
        }

        ///  保存日志类信息
        /// <summary>
        ///  保存日志类信息
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveLogsInfo(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            try
            {
                LogForWebForCall("[保存日志类信息]Start", loginuserid, loginusername);
                //号码访问表 CustPhoneVisitBusiness
                CustPhoneVisitBusinessInfo visitinfo = new CustPhoneVisitBusinessInfo();
                visitinfo.PhoneNum = jsondata.CustBaseInfo.Phone_Out;
                visitinfo.TaskID = jsondata.WOrderID;
                visitinfo.BusinessType = (int)VisitBusinessTypeEnum.S1_工单; //任务类型：-1 不存在 0 其他非CC系统 ，1：工单  3：客户核实  4：其他任务  5：YJK 6：CJK  7:易团购 
                visitinfo.TaskSource = (int)jsondata.Common.CallSource_Out; //任务来源 0：未知 1：呼入 2：呼出 3,4：IM 
                visitinfo.CallID = jsondata.CustBaseInfo.CallIds_Out != null && jsondata.CustBaseInfo.CallIds_Out.Count > 0 ?
                    CommonFunction.ObjectToLong(jsondata.CustBaseInfo.CallIds_Out[jsondata.CustBaseInfo.CallIds_Out.Count - 1]) : -1;//最后一次话务ID
                BLL.CustPhoneVisitBusiness.Instance.InsertOrUpdateCustPhoneVisitBusiness(visitinfo, loginuserid);
                LogForWebForCall("[保存日志类信息]访问记录表：电话=" + jsondata.CustBaseInfo.Phone_Out + "；任务ID=" + jsondata.WOrderID, loginuserid, loginusername);

                //话务结果表 CallResult_ORIG_Task
                if (jsondata.WOrderInfo.IsHuifang && jsondata.WOrderInfo.IsJxs)
                {
                    BLL.CallResult_ORIG_Task.Instance.InseretOrUpdateOneData(jsondata.WOrderID, -1, ProjectSource.S3_工单,
                        jsondata.WOrderInfo.IsJieTong_Out, jsondata.WOrderInfo.NoJtReason_Out, -1, -1);
                    LogForWebForCall("[保存日志类信息]话务结果表 CallResult_ORIG_Task：工单ID=" + jsondata.WOrderID
                        + "；结果=" + jsondata.WOrderInfo.IsJieTong_Out + "-" + jsondata.WOrderInfo.NoJtReason_Out, loginuserid, loginusername);
                }
            }
            catch (Exception ex)
            {
                LogForWebForCall("[保存日志类信息]保存日志类信息 " + ex.Message + ex.StackTrace, loginuserid, loginusername);
            }
            LogForWebForCall("[保存日志类信息]END\r\n", loginuserid, loginusername);
        }
        ///  保存日志类信息
        /// <summary>
        ///  保存日志类信息
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveLogsInfoASync(WOrderJsonData jsondata, int loginuserid, string loginusername)
        {
            Action<WOrderJsonData, int, string> func = new Action<WOrderJsonData, int, string>(SaveLogsInfo);
            func.BeginInvoke(jsondata, loginuserid, loginusername, null, null);
        }

        /// 更新工单状态
        /// <summary>
        /// 更新工单状态
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
        /// 查询工单
        /// <summary>
        /// 查询工单
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public WOrderInfoInfo GetWOrderInfoInfo(string orderid)
        {
            return Dal.WOrderInfo.Instance.GetWOrderInfoInfo(orderid);
        }

        /// 按照查询条件查询(包含数据权限)
        /// <summary>
        /// 按照查询条件查询(包含数据权限)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetWorkOrderInfoForList(QueryWOrderV2DataInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WOrderInfo.Instance.GetWorkOrderInfoForList(query, order, currentPage, pageSize, out totalCount);
        }
        /// 工单记录导出数据查询
        /// <summary>
        /// 工单记录导出数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetExportWorkOrderList(QueryWOrderV2DataInfo query, string order)
        {
            return Dal.WOrderInfo.Instance.GetExportWorkOrderList(query, order);
        }
        /// 查询新旧工单-客户回访
        /// <summary>
        /// 查询新旧工单-客户回访
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

        /// 查询CRM访问分类
        /// <summary>
        /// 查询CRM访问分类
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
