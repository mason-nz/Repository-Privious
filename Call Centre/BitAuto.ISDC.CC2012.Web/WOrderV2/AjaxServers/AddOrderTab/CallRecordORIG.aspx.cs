using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.AddOrderTab
{
    public partial class CallRecordORIG : PageBase
    {
        /// 电话号码列表
        /// <summary>
        /// 电话号码列表
        /// </summary>
        private string PhoneNums
        {
            get { return BLL.Util.GetCurrentRequestStr("PhoneNums"); }
        }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID
        private Random R = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            PageSize = 50;
            if (PhoneNums != "")
            {
                DataTable dt = BLL.CallRecord_ORIG.Instance.GetCustBaseInfo_TrafficRecord(PhoneNums, "cri.InitiatedTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            else
            {
                repeaterTableList.DataSource = null;
                repeaterTableList.DataBind();
            }
        }
        public string GetOperator(string AudioURL, string BusinessID, string BGID, string SCID)
        {
            //查看链接
            string operStr = string.Empty;
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(BusinessID, BGID, SCID);
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(BusinessID, url);
            operStr += BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, BusinessID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID, true);
            //替换查看按钮
            operStr = operStr.Replace("查看", "<img src='/Images/workorder/icon_view.png' title='查看' alt='查看' style='vertical-align:middle'>");
            if (operStr == "")
            {
                operStr += "<span style='display:inline-block;width:26px;'></span>";
            }
            //录音链接
            if (!string.IsNullOrEmpty(AudioURL))
            {
                operStr += "&nbsp;<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\""
                    + AudioURL + "\",\"/WOrderV2/PopLayer/PlayRecord.aspx\");' title='播放录音' ><img src='/Images/workorder/icon_hw.png' style='vertical-align:middle'/></a>  &nbsp;";
            }
            else
            {
                operStr += "&nbsp;<span style='display:inline-block;width:35px;'></span>";
            }
            return operStr;
        }
    }
}