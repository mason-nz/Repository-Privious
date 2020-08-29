using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class AjaxList : PageBase
    {
        /// <summary>
        /// JSON字符串
        /// </summary>
        public string JsonStr
        {
            get
            {
                return HttpContext.Current.Request["JsonStr"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["JsonStr"].ToString());
            }

        }
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }
        //绑定数据
        public void BindData()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Entities.QueryCallRecord_ORIG query = new Entities.QueryCallRecord_ORIG();

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.QueryCallRecord_ORIG> conver = new BLL.ConverToEntitie<Entities.QueryCallRecord_ORIG>(query);
            errMsg = conver.Conver(JsonStr);

            if (errMsg != "")
            {
                return;
            }
            int RecordCount = 0;

            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;
            query.LoginID = _loginID;

            query.BeginTime = Request["tfBeginTime"];
            query.EndTime = Request["tfEndTime"];
            string tableEndName = BLL.Util.CalcTableNameByMonth(3, CommonFunction.ObjectToDateTime(query.BeginTime));
            DataTable dt = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByList(query, " c.CreateTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);
            string a = sw.Elapsed.ToString();

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            sw.Stop();
            string b = sw.Elapsed.ToString();
            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }
        //话务总时长
        public string GetTotalTime(int TallTime, int AfterWorkTime)
        {
            int time1 = TallTime == -2 ? 0 : TallTime;
            int time2 = AfterWorkTime == -2 ? 0 : AfterWorkTime;
            string result = "";
            result = (time1 + time2).ToString();

            return result;
        }
        public string GetViewUrl(string TaskID, string BGID, string SCID)
        {
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
            return BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, TaskID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID);
        }
        public string GetPhoneNum(string CallStatus, string PhoneNum)
        {
            //呼入，转接，拦截 都属于呼入电话
            if (CallStatus == "1" || CallStatus == "3" || CallStatus == "4")
            {
                return BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(PhoneNum);
            }
            else
            {
                return PhoneNum;
            }
        }
        public string GetANI(string CallStatus, string ANI)
        {
            //呼出
            if (CallStatus == "2")
            {
                return BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(ANI);
            }
            else
            {
                return ANI;
            }
        }
        public string GetCallStatus(string CallStatus)
        {
            string a = BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(CallStatus);
            if (a == "无")
                return "&nbsp;--&nbsp;";
            else return a;
        }
        public string GetOutBoundType(string CallStatus, string OutBoundType)
        {
            if (CallStatus == "2")
            {
                if (OutBoundType == "1")
                {
                    return "页面";
                }
                else if (OutBoundType == "2")
                {
                    return "客户端";
                }
                else if (OutBoundType == "4")
                {
                    return "自动";
                }
                else
                {
                    return "&nbsp;--&nbsp;";
                }
            }
            else
            {
                return "&nbsp;--&nbsp;";
            }
        }
    }
}