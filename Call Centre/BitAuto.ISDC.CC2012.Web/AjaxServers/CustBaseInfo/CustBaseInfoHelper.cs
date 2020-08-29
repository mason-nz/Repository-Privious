using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using System.Text;
using BitAuto.ISDC.CC2012.WebService;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo
{
    public class CustBaseInfoHelper
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action
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
        public string TaskType
        {
            get
            {
                if (Request["TaskType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string TaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CustID
        {
            get
            {
                if (Request["CustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CRMCustID
        {
            get
            {
                if (Request["CRMCustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CRMCustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string TemplateID
        {
            get
            {
                if (Request["TemplateID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TemplateID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string Tels
        {
            get
            {
                if (Request["Tels"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Tels"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string SendContent
        {
            get
            {
                if (Request["SendContent"] != null)
                {
                    return HttpUtility.UrlDecode(Request["SendContent"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //省 市 区
        public string ProvinceID
        {
            get
            {
                if (Request["ProvinceID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["ProvinceID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CityID
        {
            get
            {
                if (Request["CityID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CityID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CountyID
        {
            get
            {
                if (Request["CountyID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CountyID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="msg"></param>
        public void SendSMSToPeople(out string msg)
        {
            msg = string.Empty;
            try
            {
                string[] telArry = Tels.Split(',');
                if (telArry.Length == 0)
                {
                    msg = "电话号码不能为空";
                    msg = "{result:'false',msg:'" + msg + "'}";
                    return;
                }
                if (telArry.Length > 2)
                {
                    msg = "电话号码不能超过3个";
                    msg = "{result:'false',msg:'" + msg + "'}";
                    return;
                }
                foreach (string str in telArry)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string telstr = str.Trim();
                        if (!BLL.Util.IsHandset(telstr))
                        {
                            msg = "电话(" + telstr + ")不符合规则";
                            msg = "{result:'false',msg:'" + msg + "'}";
                            return;
                        }
                        //计算客户id
                        string custid = CustID;
                        if (string.IsNullOrEmpty(custid))
                        {
                            custid = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(telstr);
                        }

                        //插入发送日志
                        Entities.SendSMSLog smsLogModel = new Entities.SendSMSLog();
                        smsLogModel.CreateUserID = BLL.Util.GetLoginUserID();
                        smsLogModel.CustID = custid;
                        smsLogModel.Mobile = telstr;
                        smsLogModel.SendContent = SendContent;
                        smsLogModel.SendTime = DateTime.Now;
                        smsLogModel.TemplateID = CommonFunction.ObjectToInteger(TemplateID, -1);
                        BLL.SendSMSLog.Instance.Insert(smsLogModel);

                        //发送短信
                        int status = -1;
                        string md5 = SMSServiceHelper.Instance.MixMd5(telstr, SendContent);
                        int msgid = SMSServiceHelper.Instance.SendMsgImmediately(telstr, SendContent, DateTime.Now.AddHours(1), md5);
                        if (msgid > 0)
                        {
                            //插入发送短信记录
                            status = 0;
                            BLL.Util.InsertUserLog("给手机（" + telstr + "）发送短信成功");
                            msg = "{result:'true',msg:''}";
                        }
                        else
                        {
                            msg = BLL.Util.GetEnumOptText(typeof(Entities.SendSMSInfo), msgid);
                            BLL.Util.InsertUserLog("给手机（" + telstr + "）发送短信失败【错误信息：" + msg + "】");
                            msg = "{result:'false',msg:'" + msg + "'}";
                        }

                        //插入发送短信记录
                        Entities.SMSSendHistory smssendhistory = new Entities.SMSSendHistory();
                        smssendhistory.CreateUserID = BLL.Util.GetLoginUserID();
                        smssendhistory.CustID = custid;
                        smssendhistory.CRMCustID = CRMCustID;
                        Entities.EmployeeAgent agent = new Entities.EmployeeAgent();
                        agent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(Convert.ToInt32(smssendhistory.CreateUserID));
                        if (agent != null && agent.BGID > 0)
                        {
                            smssendhistory.BGID = agent.BGID;
                        }
                        smssendhistory.TemplateID = CommonFunction.ObjectToInteger(TemplateID, -1);
                        smssendhistory.TaskType = CommonFunction.ObjectToInteger(TaskType, -1);
                        smssendhistory.TaskID = TaskID;
                        smssendhistory.Phone = telstr;
                        smssendhistory.Content = SendContent;
                        smssendhistory.CreateTime = DateTime.Now;
                        smssendhistory.Status = status;
                        BLL.SMSSendHistory.Instance.Insert(smssendhistory);

                        //成功后返回
                        if (status == 0)
                        {
                            msg = "{result:'true',msg:'',SMSSendHistoryRecID:'" + smssendhistory.RecID + "'}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'false',msg:'" + ex.Message + "'}";
            }
        }

        /// 获取大区ID
        /// <summary>
        /// 获取大区ID
        /// </summary>
        /// <param name="msg"></param>
        /// 
        public void GetAreaDistrictID(out string msg)
        {
            msg = string.Empty;
            //修改大区查询逻辑 强斐 2014-12-17

            string pid = ProvinceID;
            string cid = CityID;
            string nid = CountyID;

            var info = BLL.Util.GetAreaInfoByPCC(pid, cid, nid);
            msg = info == null ? "" : info.District;
        }

        /// 根据号码生成链接到客户的链接
        /// <summary>
        /// 根据号码生成链接到客户的链接
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static string GetLinkToCustByTel(string tel)
        {
            string CustID = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(tel);
            if (string.IsNullOrEmpty(CustID))
            {
                return tel;
            }
            else
            {
                return "<a href='/TaskManager/CustInformation.aspx?CustID=" + CustID + "' target='_blank'>" + tel + "</a><input type='hidden' name='CustID' value='" + CustID + "'/>";
            }
        }
    }
}