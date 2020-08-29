using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.Handler
{
    /// 新增工单
    /// <summary>
    /// 新增工单
    /// </summary>
    public class AddWOrderHandler : IHttpHandler, IRequiresSessionState
    {
        //操作类型
        private string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action");
            }
        }
        //当前登录人
        private int LoginUserID
        {
            get
            {
                return BLL.Util.GetLoginUserID();
            }
        }
        //电话
        private string Phone
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Phone");
            }
        }
        //是否计算N
        private bool IsCalcN
        {
            get
            {
                return bool.Parse(HttpContext.Current.Request["IsCalcN"] == null ? "False" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsCalcN"].ToString()));
            }
        }
        //经销商ID
        private string MemberCode
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("MemberCode");
            }
        }
        //工单主数据
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
            //TODO:CodeReview 2016-08-10 Action参数的值，大小写匹配问题
            switch (Action)
            {
                case "GetCBInfoByPhone":
                    result = GetCBInfoByPhone(out msg, out data);
                    break;
                case "GetSelectOption":
                    result = GetSelectOption(out msg, out data);
                    break;
                case "GetDealerInfo":
                    result = GetDealerInfo(out msg, out data);
                    break;
                case "phonenumisnodisturb":
                    result = PhoneNumIsNoDisturb(out msg, out data);
                    break;
                case "SaveWOrderInfo":
                    result = SaveWOrderInfo(out msg, out data);
                    break;
            }

            dic["result"] = result.ToString().ToLower();
            dic["message"] = "'" + msg + "'";
            dic["data"] = data; //可以是字符串，可以是对象
            context.Response.Write(BLL.Util.DictionaryToJson(dic));
        }

        /// 判断电话号码是免打扰号码
        /// <summary>
        /// 判断电话号码是免打扰号码
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        private bool PhoneNumIsNoDisturb(out string msg, out string data)
        {
            msg = "";
            data = "";
            msg = "{PhoneStatus:'" + BLL.BlackWhiteList.Instance.PhoneNumIsNoDisturb(Phone).ToString() + "'}";
            return true;
        }
        /// 根据手机号码获取客户信息
        /// <summary>
        /// 根据手机号码获取客户信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool GetCBInfoByPhone(out string msg, out string data)
        {
            msg = "";
            data = "";

            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (IsCalcN && !string.IsNullOrEmpty(Phone))
            {
                //计算呼入次数
                int count = BLL.WOrderInfo.Instance.CalcCallInCountByPhone(Phone);
                dic.Add("N_count", count.ToString());
            }
            else
            {
                dic.Add("N_count", "0");
            }
            //查询客户信息
            DataTable dt = null;
            if (!string.IsNullOrEmpty(Phone))
            {
                dt = BLL.WOrderInfo.Instance.GetCBInfoByPhone(null, Phone);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dic.Add("CustID", "'" + dr["CustID"].ToString() + "'");
                dic.Add("CustName", "'" + dr["CustName"].ToString() + "'");
                dic.Add("Sex", dr["Sex"].ToString());
                dic.Add("CustCategoryID", dr["CustCategoryID"].ToString());
                dic.Add("ProvinceID", dr["ProvinceID"].ToString());
                dic.Add("CityID", dr["CityID"].ToString());
                dic.Add("CountyID", dr["CountyID"].ToString());
                dic.Add("MemberCode", "'" + dr["MemberCode"].ToString() + "'");
                dic.Add("MemberName", "'" + dr["MemberName"].ToString() + "'");
            }
            else
            {
                int pID = 0, cID = 0;
                BLL.PhoneNumDataDict.GetAreaId(Phone, out pID, out cID);
                if (pID <= 0) pID = -1;
                if (cID <= 0) cID = -1;
                dic.Add("CustID", "''");
                dic.Add("CustName", "''");
                dic.Add("Sex", "1"); //默认 先生
                dic.Add("CustCategoryID", "4"); //默认经销商
                dic.Add("ProvinceID", pID.ToString()); //通过手机号码查询
                dic.Add("CityID", cID.ToString());//通过手机号码查询
                dic.Add("CountyID", "-1");
                dic.Add("MemberCode", "''");
                dic.Add("MemberName", "''");
            }
            //转json
            data = BLL.Util.DictionaryToJson(dic);
            return true;
        }
        /// 获取下拉列表的选择项
        /// <summary>
        /// 获取下拉列表的选择项
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool GetSelectOption(out string msg, out string data)
        {
            msg = "";
            data = "";
            //业务线来源
            DataTable sourcedt = BLL.Util.GetEnumDataTable(typeof(WorkOrderDataSource));
            //工单类型
            DataTable categorydt = BLL.Util.GetEnumDataTable(typeof(WOrderCategoryEnum));
            //业务类型
            DataTable businessdt = BLL.WOrderBusiType.Instance.GetAllData("1"); //在用
            //访问分类
            DataTable visitdt = BitAuto.YanFa.Crm2009.BLL.DictInfo.Instance.GetDictInfoByDictType(101);
            //是否接通
            DataTable isjietongdt = BLL.Util.GetEnumDataTable(typeof(YesNO));
            //未接通原因
            DataTable nojietongdt = BLL.Util.GetEnumDataTable(typeof(NotEstablishReason));
            //投诉级别
            DataTable complaintdt = BLL.Util.GetEnumDataTable(typeof(ComplaintLevelEnum));
            //构造数据
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("sourcedt", BLL.Util.DataTableToJson(sourcedt, "value", "name"));
            dic.Add("categorydt", BLL.Util.DataTableToJson(categorydt, "value", "name"));
            dic.Add("businessdt", BLL.Util.DataTableToJson(businessdt, "RecID", "BusiTypeName"));
            dic.Add("visitdt", BLL.Util.DataTableToJson(visitdt, "DictID", "DictName"));
            dic.Add("isjietongdt", BLL.Util.DataTableToJson(isjietongdt, "value", "name"));
            dic.Add("nojietongdt", BLL.Util.DataTableToJson(nojietongdt, "value", "name"));
            dic.Add("complaintdt", BLL.Util.DataTableToJson(complaintdt, "value", "name"));
            //转json
            data = BLL.Util.DictionaryToJson(dic);
            return true;
        }
        /// 获取经销商信息
        /// <summary>
        /// 获取经销商信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool GetDealerInfo(out string msg, out string data)
        {
            msg = "";
            data = "";

            Dictionary<string, string> dic = new Dictionary<string, string>();
            //查询客户信息
            int totalCount;
            DataTable dt = BLL.DealerInfo.Instance.GetMemberInfo("", MemberCode.Trim(), "", "", 1, 1, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dic.Add("name", "'" + dr["name"].ToString() + "'");
                dic.Add("CustID", "'" + dr["CustID"].ToString() + "'");
                dic.Add("CustName", "'" + dr["CustName"].ToString() + "'");
                dic.Add("MemberCode", "'" + dr["MemberCode"].ToString() + "'");
            }
            else
            {
                dic.Add("name", "''");
                dic.Add("CustID", "''");
                dic.Add("CustName", "''");
                dic.Add("MemberCode", "''");
            }
            //转json
            data = BLL.Util.DictionaryToJson(dic);
            return true;
        }

        /// 保存工单
        /// <summary>
        /// 保存工单
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SaveWOrderInfo(out string msg, out string data)
        {
            msg = "";
            data = "''";
            try
            {
                int loginuserid = BLL.Util.GetLoginUserID();
                LogForWebForCall("保存工单开始 JsonData=" + JsonData);
                //查询登录人信息
                SysRightUserInfo sysinfo = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(loginuserid);
                if (sysinfo == null)
                {
                    msg = "查询不到登录人信息！";
                    return false;
                }
                //数据解析
                WOrderJsonData jsondata = WOrderJsonData.CreateWOrderJsonData(JsonData);
                if (jsondata != null)
                {
                    //保存个人信息（个人表 CustBasicInfo，电话表 CustTel，经销商表 DealerInfo）
                    BLL.WOrderInfo.Instance.SaveCustBasicInfo(jsondata, loginuserid);
                    LogForWebForCall("保存个人信息：电话=" + jsondata.CustBaseInfo.Phone_Out + "；CBID=" + jsondata.CBID);

                    //保存工单信息（工单主表 WOrderInfo，处理表 WOrderProcess，附件表 CommonAttachment）
                    BLL.WOrderInfo.Instance.SaveWOrderInfo(jsondata, sysinfo);
                    LogForWebForCall("保存工单信息：电话=" + jsondata.CustBaseInfo.Phone_Out + "；工单ID=" + jsondata.WOrderID);

                    //异步操作-绑定（来去电表 CallRecordInfo，话务业务表 CallRecord_ORIG_Business，工单关联数据表 WOrderData，短信表 SMSSendHistory）
                    BLL.WOrderInfo.Instance.SaveBindInfoASync(jsondata, loginuserid, sysinfo.TrueName);

                    //异步操作-日志类信息（号码访问表 CustPhoneVisitBusiness，话务结果表 CallResult_ORIG_Task）
                    BLL.WOrderInfo.Instance.SaveLogsInfoASync(jsondata, loginuserid, sysinfo.TrueName);

                    //CRM+IM接口调用
                    InterfaceCallASync(jsondata, sysinfo);
                }
                else
                {
                    msg = "参数错误，转换失败！";
                    return false;
                }
                LogForWebForCall("END\r\n");
                return true;
            }
            catch (Exception ex)
            {
                BLL.WOrderInfo.ErrorToLog4("保存工单", ex);
                msg = ex.Message;
                return false;
            }
        }

        /// 接口调用
        /// <summary>
        /// 接口调用
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="loginuserid"></param>
        public void InterfaceCall(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            try
            {
                //同步CRM
                if (jsondata.WOrderInfo.IsSysCRM_Out == 1 && !string.IsNullOrEmpty(jsondata.CRMCustID_Out))
                {
                    //jsondata.Common.CRMCustID_Out  crm的custid
                    //jsondata.WOrderInfo.VisitType_Out  访问类型id
                    //sysinfo 登录人信息，部门，姓名，id，员工编号等
                    InserCRMVisitInfo(jsondata, sysinfo);
                    LogForWebForCall("CRM添加访问记录");
                }
                //同步IM接口
                //1—会话，2—留言
                int imType = jsondata.Common.CallSource_Out == CallSourceEnum.C03_IM对话 ? 1 : 2;
                int csid = (int)jsondata.Common.RelatedCSID;
                string orderid = jsondata.WOrderID;
                if (jsondata.Common.ModuleSource_Out == ModuleSourceEnum.M05_IM个人)
                {
                    BitAuto.ISDC.CC2012.WebService.IM.IMUtilsServiceHelper server = new WebService.IM.IMUtilsServiceHelper();
                    server.UpdateCCWorkOrderToIM(imType, csid, orderid);
                    LogForWebForCall("M05_IM个人 回调接口");
                }
                else if (jsondata.Common.ModuleSource_Out == ModuleSourceEnum.M06_IM经销商_新车)
                {
                    BitAuto.ISDC.CC2012.WebService.IM.IMepUtilsServiceHelper server = new WebService.IM.IMepUtilsServiceHelper();
                    server.UpdateCCWorkOrderToIM(imType, csid, orderid);
                    LogForWebForCall("M06_IM经销商_新车 回调接口");
                }
                else if (jsondata.Common.ModuleSource_Out == ModuleSourceEnum.M07_IM经销商_二手车)
                {
                    BitAuto.ISDC.CC2012.WebService.IM.IMtcUtilsServiceHelper server = new WebService.IM.IMtcUtilsServiceHelper();
                    server.UpdateCCWorkOrderToIM(imType, csid, orderid);
                    LogForWebForCall("M07_IM经销商_二手车 回调接口");
                }
            }
            catch (Exception ex)
            {
                WOrderInfo.ErrorToLog4("接口调用", ex);
            }
        }
        public void InterfaceCallASync(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            Action<WOrderJsonData, SysRightUserInfo> action = new Action<WOrderJsonData, SysRightUserInfo>(InterfaceCall);
            action.BeginInvoke(jsondata, sysinfo, null, null);
        }

        /// CRM添加访问记录
        /// <summary>
        /// CRM添加访问记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InserCRMVisitInfo(WOrderJsonData jsondata, SysRightUserInfo sysinfo)
        {
            try
            {
                BitAuto.YanFa.Crm2009.Entities.ReturnVisit model = new YanFa.Crm2009.Entities.ReturnVisit();
                //必填
                model.BeginDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //访问日期
                if (DateTime.Now.Minute < 30)
                {
                    model.begintime = DateTime.Now.ToString("yyyy-MM-dd HH:00:00"); //访问开始时间
                    model.endtime = DateTime.Now.ToString("yyyy-MM-dd HH:30:00"); //访问结束时间
                }
                else
                {
                    model.begintime = DateTime.Now.ToString("yyyy-MM-dd HH:30:00"); //访问开始时间
                    model.endtime = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:00:00"); //访问结束时间
                }
                model.BusinessLine = sysinfo.BusinessLine; //所属业务
                model.CustID = jsondata.CRMCustID_Out;// 客户编号,1094888497
                model.RVType = 2; //访问方式
                model.Remark = jsondata.WOrderInfo.Content_Out; //描述
                model.VisitType = jsondata.WOrderInfo.VisitType_Out.ToString();//访问分类
                model.createUserID = sysinfo.UserID; //创建人
                model.ContactInfoUserID = jsondata.Common.RelatedContactID;//联系人ID
                model.status = 0;

                //非必填
                model.CreateuserDepart = sysinfo.DepartID; //部门,DP00522
                model.createtime = DateTime.Now; //创建时间
                model.MemberId = jsondata.CustBaseInfo.MemberCode_Out;// 会员id
                model.RVQuestionRemark = ""; //问题
                model.RVQuestionStatus = 0; //问题是否解决
                model.RVresult = ""; //访问结果

                int res = BitAuto.YanFa.Crm2009.BLL.ReturnVisit.Instance.InsertReturnVisit(model);

                Loger.Log4Net.Error("CRM添加访问记录返回结果：" + res);

                return res > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("CRM添加访问记录异常：" + ex.Message.ToString());
            }
            return false;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static void LogForWebForCall(string msg)
        {
            int loginuserid = BLL.Util.GetLoginUserID();
            string loginusername = BLL.Util.GetLoginRealName();
            BLL.WOrderInfo.LogForWebForCall(msg, loginuserid, loginusername);
        }
    }
}