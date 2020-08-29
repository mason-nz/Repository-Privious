using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.WebService;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// 新版电话控件对应的逻辑处理
    /// <summary>
    /// 新版电话控件对应的逻辑处理
    /// 强斐
    /// 2016-8-2
    /// </summary>
    public class CommonCallHandler : IHttpHandler, IRequiresSessionState
    {
        //操作类型
        private string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action");
            }
        }
        //电话号码
        private string Phone
        {
            get { return BLL.Util.GetCurrentRequestStr("Phone"); }
        }
        //话务主数据
        private string JsonData
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("JsonData");
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            bool result = false;
            string msg = "";
            string data = "";
            switch (Action)
            {
                case "CallSaveEvent":
                    result = CallSaveEvent(out msg, out data);
                    break;
                case "SMSSaveEvent":
                    result = SMSSaveEvent(out msg, out data);
                    break;
                case "InsertBasicInfo":
                    result = InsertBasicInfo(out msg, out data);
                    break;
            }

            dic["result"] = result.ToString().ToLower();
            dic["message"] = "'" + msg + "'";
            dic["data"] = data; //可以是字符串，可以是对象
            context.Response.Write(BLL.Util.DictionaryToJson(dic));
        }


        /// 保存用户信息
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool InsertBasicInfo(out string msg, out string data)
        {
            msg = "";
            data = "''";
            try
            {
                int loginuserid = BLL.Util.GetLoginUserID();
                CommonCallJsonData jsondata = CommonCallJsonData.GetCommonCallJsonData(JsonData);
                data = "'" + SaveCustBasicInfo(Phone, jsondata.PageData, loginuserid) + "'";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 保存话务信息
        /// <summary>
        /// 保存话务信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CallSaveEvent(out string msg, out string data)
        {
            msg = "";
            data = "''";
            try
            {
                int loginuserid = BLL.Util.GetLoginUserID();
                CommonCallJsonData jsondata = CommonCallJsonData.GetCommonCallJsonData(JsonData);
                if (jsondata == null)
                {
                    msg = "参数解析错误！";
                    return false;
                }
                LogForWebForCall("电话", "保存话务信息：jsondata" + JsonData);
                LogForWebForCall("电话", "页面数据：" + jsondata.PageData.ToString());
                LogForWebForCall("电话", "话务数据：" + jsondata.CallData.ToString());

                int bgid = jsondata.PageData.BGID_Out;
                int scid = jsondata.PageData.SCID_Out;
                if (bgid <= 0)
                {
                    //当前分组
                    bgid = BLL.SurveyCategory.Instance.GetSelfBGIDByUserID(loginuserid);
                }
                if (scid <= 0)
                {
                    //当前分类
                    scid = BLL.SurveyCategory.Instance.GetSelfSCIDByUserID(loginuserid);
                }
                LogForWebForCall("电话", "分组分类=" + bgid + "-" + scid);

                //更新CustBasicInfo 3表
                jsondata.CBID = SaveCustBasicInfo(jsondata.CallData.Phone_Out, jsondata.PageData, loginuserid);
                LogForWebForCall("电话", "保存个人用户信息：电话=" + jsondata.CallData.Phone_Out + "；CBID=" + jsondata.CBID);

                //新增或者更新CallRecord_ORIG_Business表
                BLL.CallRecord_ORIG_Business.Instance.UpdateBusinessDataByCallID(jsondata.CallData.CallID_Out, jsondata.PageData.TaskID_Out, bgid, scid, loginuserid, ref msg);
                LogForWebForCall("电话", "新增或者更新CallRecord_ORIG_Business表：话务ID=" + jsondata.CallData.CallID_Out + "；任务ID=" + jsondata.PageData.TaskID_Out);

                //如果接通：新增或者更新CallRecordInfo表
                if (jsondata.CallData.IsEstablished_Out)
                {
                    SaveCallRecordInfo(jsondata, loginuserid, bgid, scid);
                    LogForWebForCall("电话", "接通-新增或者更新CallRecordInfo表：话务ID=" + jsondata.CallData.CallID_Out + "；任务ID=" + jsondata.PageData.TaskID_Out);
                }
                //如果任务存在，添加访问记录
                if (jsondata.PageData.TaskID_Out != "")
                {
                    SaveCustPhoneVisitBusiness(jsondata.PageData, jsondata.CallData.Phone_Out, jsondata.CallData.CallType_Out, jsondata.CallData.CallID_Out, loginuserid);
                    LogForWebForCall("电话", "添加访问记录：电话=" + jsondata.CallData.Phone_Out + "；任务ID=" + jsondata.PageData.TaskID_Out);
                }
                LogForWebForCall("电话", "END\r\n");
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 保存短信信息
        /// <summary>
        /// 保存短信信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SMSSaveEvent(out string msg, out string data)
        {
            msg = "";
            data = "''";
            try
            {
                int loginuserid = BLL.Util.GetLoginUserID();
                CommonCallJsonData jsondata = CommonCallJsonData.GetCommonCallJsonData(JsonData);
                if (jsondata == null)
                {
                    msg = "参数解析错误！";
                    return false;
                }
                LogForWebForCall("短信", "保存短信信息：jsondata" + JsonData);
                LogForWebForCall("短信", "页面数据：" + jsondata.PageData.ToString());
                LogForWebForCall("短信", "短信数据：" + jsondata.SMSData.ToString());

                int bgid = jsondata.PageData.BGID_Out;
                int scid = jsondata.PageData.SCID_Out;
                if (bgid <= 0)
                {
                    //当前分组
                    bgid = BLL.SurveyCategory.Instance.GetSelfBGIDByUserID(loginuserid);
                }
                if (scid <= 0)
                {
                    //当前分类
                    scid = BLL.SurveyCategory.Instance.GetSelfSCIDByUserID(loginuserid);
                }
                LogForWebForCall("短信", "分组分类=" + bgid + "-" + scid);

                //更新CustBasicInfo 3表
                jsondata.CBID = SaveCustBasicInfo(jsondata.SMSData.Phone_Out, jsondata.PageData, loginuserid);
                LogForWebForCall("短信", "保存个人用户信息：电话=" + jsondata.SMSData.Phone_Out + "；CBID=" + jsondata.CBID);

                //发送短信
                int sendstatus = SendSMS(jsondata, loginuserid, out msg);
                //新增或者更新SMSSendHistory表 
                SaveSMSSendHistory(jsondata, loginuserid, bgid, sendstatus, out data);
                LogForWebForCall("短信", "发送短信：电话=" + jsondata.SMSData.Phone_Out + "；内容=" + jsondata.SMSData.Content_Out.Length + "；任务ID=" + jsondata.PageData.TaskID_Out);

                //更新SendSMSLog表
                SaveSendSMSLog(jsondata, loginuserid);
                LogForWebForCall("短信", "更新SendSMSLog表：电话=" + jsondata.SMSData.Phone_Out);

                //如果任务存在，添加访问记录
                if (jsondata.PageData.TaskID_Out != "")
                {
                    SaveCustPhoneVisitBusiness(jsondata.PageData, jsondata.SMSData.Phone_Out, (int)CallSourceEnum.C05_短信, -1, loginuserid);
                    LogForWebForCall("短信", "添加访问记录：电话=" + jsondata.SMSData.Phone_Out + "；任务ID=" + jsondata.PageData.TaskID_Out);
                }
                LogForWebForCall("短信", "END\r\n");
                return sendstatus == 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        /// 保存个人信息
        /// <summary>
        /// 保存个人信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        public string SaveCustBasicInfo(string phone, PageDataInfo pagedata, int loginuserid)
        {
            //查询数据库
            string cbid = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(phone);
            if (string.IsNullOrEmpty(cbid))
            {
                //新增
                Entities.CustBasicInfo model = new Entities.CustBasicInfo();
                model.CustName = pagedata.CBName_Out != "" ? pagedata.CBName_Out : "未知";
                model.Sex = pagedata.CBSex_Out > 0 ? pagedata.CBSex_Out : 1;
                model.CustCategoryID = (int)pagedata.CustType_Out;
                int pID = 0, cID = 0;
                BLL.PhoneNumDataDict.GetAreaId(phone, out pID, out cID);
                model.ProvinceID = pID == 0 ? -2 : pID;
                model.CityID = cID == 0 ? -2 : cID;
                model.CountyID = -1;
                //废弃字段
                model.Address = null;
                model.DataSource = null;
                model.CallTime = null;
                model.Status = 0;
                //基础字段
                model.CreateTime = DateTime.Now;
                model.CreateUserID = loginuserid;
                model.ModifyTime = null;
                model.ModifyUserID = null;
                cbid = BLL.CustBasicInfo.Instance.Insert(model);

                //插入电话信息
                Entities.CustTel model_Tel = new Entities.CustTel();
                model_Tel.CustID = cbid;
                model_Tel.Tel = phone;
                model_Tel.CreateTime = DateTime.Now;
                model_Tel.CreateUserID = loginuserid;
                BLL.CustTel.Instance.Insert(model_Tel);
            }
            else
            {
                //修改( 值有效才更新 )
                Entities.CustBasicInfo model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(cbid);
                if (!string.IsNullOrEmpty(pagedata.CBName_Out))
                {
                    model.CustName = pagedata.CBName_Out;
                }
                if (pagedata.CBSex_Out > 0)
                {
                    model.Sex = pagedata.CBSex_Out;
                }
                model.CustCategoryID = (int)pagedata.CustType_Out;
                //废弃字段
                model.Address = null;
                model.DataSource = null;
                model.CallTime = null;
                model.Status = 0;
                //基础字段
                model.ModifyTime = DateTime.Now;
                model.ModifyUserID = loginuserid;
                BLL.CustBasicInfo.Instance.Update(model);
            }
            //删除经销商信息（个人类型 或者 有值）
            if (!string.IsNullOrEmpty(pagedata.CBMemberName_Out) ||
                pagedata.CustType_Out == Entities.CustTypeEnum.T01_个人)
            {
                BLL.DealerInfo.Instance.Delete(cbid);
            }
            //更新经销商（经销商类型 且 有值）
            if (!string.IsNullOrEmpty(pagedata.CBMemberName_Out) &&
                  pagedata.CustType_Out == Entities.CustTypeEnum.T02_经销商)
            {
                //插入经销商信息
                Entities.DealerInfo model_Dealer = new Entities.DealerInfo();
                model_Dealer.CustID = cbid;
                model_Dealer.MemberCode = pagedata.CBMemberCode_Out;
                model_Dealer.Name = pagedata.CBMemberName_Out;
                model_Dealer.Status = 0;
                model_Dealer.CreateTime = DateTime.Now;
                model_Dealer.CreateUserID = loginuserid;
                BLL.DealerInfo.Instance.Insert(model_Dealer);
            }
            return cbid;
        }
        /// 保存来电去电表
        /// <summary>
        /// 保存来电去电表
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        /// <param name="bgid"></param>
        /// <param name="scid"></param>
        public void SaveCallRecordInfo(CommonCallJsonData jsondata, int loginuserid, int bgid, int scid)
        {
            if (jsondata.CallData.CallID_Out <= 0) return;
            CallRecordInfoInfo info = BLL.CallRecordInfo.Instance.GetCallRecordInfoInfo(jsondata.CallData.CallID_Out);
            bool isadd = true;
            if (info == null)
            {
                //新增
                info = new CallRecordInfoInfo();
                isadd = true;
            }
            else
            {
                //更新
                isadd = false;
            }
            //赋值
            info.SessionID = jsondata.CallData.SessionID_Out;
            info.ExtensionNum = jsondata.CallData.ExtensionNum_Out;
            info.CallStatus = jsondata.CallData.CallType_Out;
            if (info.CallStatus == 1)
            {
                //呼入
                //         被叫  主叫
                // 呼入 坐席  用户
                info.PhoneNum = GetTel(CallUserType.坐席, jsondata.CallData.ExtensionNum_Out, jsondata.CallData.Zhujiao_Out, jsondata.CallData.Beijiao_Out);
                info.ANI = GetTel(CallUserType.用户, jsondata.CallData.ExtensionNum_Out, jsondata.CallData.Zhujiao_Out, jsondata.CallData.Beijiao_Out);
            }
            else
            {
                //呼出
                //         被叫  主叫
                // 呼出 用户  坐席
                info.PhoneNum = GetTel(CallUserType.用户, jsondata.CallData.ExtensionNum_Out, jsondata.CallData.Zhujiao_Out, jsondata.CallData.Beijiao_Out);
                info.ANI = GetTel(CallUserType.坐席, jsondata.CallData.ExtensionNum_Out, jsondata.CallData.Zhujiao_Out, jsondata.CallData.Beijiao_Out);
            }
            info.BeginTime = jsondata.CallData.BeginTime_Out;
            info.EndTime = jsondata.CallData.EndTime_Out;
            info.TallTime = jsondata.CallData.TallTime_Out;
            info.AudioURL = jsondata.CallData.AudioURL_Out;
            info.CustID = jsondata.CBID;
            info.CustName = jsondata.PageData.CBName_Out;
            info.Contact = jsondata.PageData.CBName_Out;
            info.TaskTypeID = (int)jsondata.PageData.TaskType_Out;
            info.TaskID = jsondata.PageData.TaskID_Out;
            info.SkillGroup = jsondata.CallData.SkillGroup_Out;
            info.BGID = bgid;
            info.SCID = scid;
            info.CallID = jsondata.CallData.CallID_Out;
            info.CreateTime = DateTime.Now;
            info.CreateUserID = loginuserid;
            //入库
            if (isadd)
            {
                CommonBll.Instance.InsertComAdoInfo(info);
            }
            else
            {
                CommonBll.Instance.UpdateComAdoInfo(info);
            }
        }
        /// 计算主被叫，计算结果不受话务总表影响
        /// <summary>
        /// 计算主被叫，计算结果不受话务总表影响
        /// </summary>
        /// <param name="type"></param>
        /// <param name="extensionnum"></param>
        /// <param name="tel1"></param>
        /// <param name="tel2"></param>
        /// <returns></returns>
        private static string GetTel(CallUserType type, string extensionnum, string tel1, string tel2)
        {
            if (type == CallUserType.用户)
            {
                return extensionnum == tel1 ? tel2 : tel1;
            }
            else if (type == CallUserType.坐席)
            {
                return extensionnum == tel1 ? tel1 : tel2;
            }
            else return "";
        }
        /// 保存业务记录信息
        /// <summary>
        /// 保存业务记录信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        private void SaveCustPhoneVisitBusiness(PageDataInfo PageData, string phone, int callsource, long callid, int loginuserid)
        {
            //号码访问表 CustPhoneVisitBusiness
            CustPhoneVisitBusinessInfo visitinfo = new CustPhoneVisitBusinessInfo();
            visitinfo.PhoneNum = phone;
            visitinfo.TaskID = PageData.TaskID_Out;
            visitinfo.BusinessType = (int)PageData.BusinessType_Out; //任务类型：-1 不存在   0 其他非CC系统 ，1：工单  3：客户核实  4：其他任务  5：YJK 6：CJK  7:易团购 
            visitinfo.TaskSource = callsource; //任务来源 0：未知 1：呼入 2：呼出 3,4：IM 
            visitinfo.CallID = callid;//最后一次话务ID
            BLL.CustPhoneVisitBusiness.Instance.InsertOrUpdateCustPhoneVisitBusiness(visitinfo, loginuserid);
        }

        /// 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        private int SendSMS(CommonCallJsonData jsondata, int loginuserid, out string msg)
        {
            msg = "";
            string md5 = SMSServiceHelper.Instance.MixMd5(jsondata.SMSData.Phone_Out, jsondata.SMSData.Content_Out);
            int msgid = SMSServiceHelper.Instance.SendMsgImmediately(jsondata.SMSData.Phone_Out, jsondata.SMSData.Content_Out, DateTime.Now.AddHours(1), md5);
            if (msgid > 0)
            {
                return 0;
            }
            else
            {
                msg = BLL.Util.GetEnumOptText(typeof(Entities.SendSMSInfo), msgid);
                return -1;
            }
        }
        /// 保存数据表
        /// <summary>
        /// 保存数据表
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        private void SaveSMSSendHistory(CommonCallJsonData jsondata, int loginuserid, int bgid, int sendstatus, out string data)
        {
            //插入发送短信记录
            Entities.SMSSendHistory smssendhistory = new Entities.SMSSendHistory();
            smssendhistory.BGID = bgid;
            smssendhistory.TemplateID = jsondata.SMSData.TemplateID_Out;
            smssendhistory.Phone = jsondata.SMSData.Phone_Out;
            smssendhistory.Content = jsondata.SMSData.Content_Out;
            smssendhistory.Status = sendstatus;
            smssendhistory.CustID = jsondata.CBID;
            smssendhistory.CRMCustID = jsondata.PageData.CRMCustID_Out.ToString();
            smssendhistory.TaskType = (int)jsondata.PageData.TaskType_Out;
            smssendhistory.TaskID = jsondata.PageData.TaskID_Out;
            smssendhistory.CreateTime = DateTime.Now;
            smssendhistory.CreateUserID = loginuserid;
            BLL.SMSSendHistory.Instance.Insert(smssendhistory);
            data = "'" + smssendhistory.RecID + "'";
        }
        /// 保存数据表
        /// <summary>
        /// 保存数据表
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        private void SaveSendSMSLog(CommonCallJsonData jsondata, int loginuserid)
        {
            Entities.SendSMSLog smsLogModel = new Entities.SendSMSLog();
            smsLogModel.TemplateID = jsondata.SMSData.TemplateID_Out;
            smsLogModel.CustID = jsondata.CBID;
            smsLogModel.Mobile = jsondata.SMSData.Phone_Out;
            smsLogModel.SendTime = DateTime.Now;
            smsLogModel.SendContent = jsondata.SMSData.Content_Out;
            smsLogModel.CreateUserID = loginuserid;
            BLL.SendSMSLog.Instance.Insert(smsLogModel);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static void LogForWebForCall(string func, string msg)
        {
            BLL.Util.LogForWebForModule("话务相关", func, msg);
        }
    }
}