using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab
{
    public partial class TaskHistoryRecord : PageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            PageSize = 10;
            litPagerDown.Visible = true;
            if (PhoneNums != "")
            {
                DataTable dt = BLL.CallRecord_ORIG.Instance.GetCustBaseInfo_ServiceRecord(PhoneNums, "ModifyTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1011);
            }
            else
            {
                repeaterTableList.DataSource = null;
                repeaterTableList.DataBind();
                litPagerDown.Text = "";
            }
        }
        //操作
        public string GetOperator(string AudioURL, string TaskID, string BussinessType, string BGID, string SCID)
        {
            string operStr = string.Empty;
            VisitBusinessTypeEnum bt = (VisitBusinessTypeEnum)CommonFunction.ObjectToInteger(BussinessType);
            if (bt == VisitBusinessTypeEnum.S3_客户核实)
            {
                operStr += "<a target='_blank' href='/CRMStopCust/View.aspx?TaskID=" + TaskID + "' class='linkBlue'><img src='/Images/workorder/icon_view.png' title='查看' alt='查看' style='vertical-align:middle'></a>";
            }
            else if (bt == VisitBusinessTypeEnum.S0_其他系统 || bt == VisitBusinessTypeEnum.None)
            {
                string url = "";
                url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
                url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
                operStr += BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, TaskID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID, true);
                operStr = operStr.Replace("查看", "<img src='/Images/workorder/icon_view.png' title='查看' alt='查看' style='vertical-align:middle'>");
            }
            else
            {
                string url = "";
                url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
                url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
                operStr += "<a target='_blank' href='" + url + "' class='linkBlue'><img src='/Images/workorder/icon_view.png' title='查看' alt='查看' style='vertical-align:middle'></a>";
            }
            //录音文件
            if (!string.IsNullOrEmpty(AudioURL))
            {
                operStr += "&nbsp;<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\""
                    + AudioURL + "\",\"/WOrderV2/PopLayer/PlayRecord.aspx\");' title='播放录音' ><img src='/Images/workorder/icon_hw.png' style='vertical-align:middle'/></a>";
            }
            else
            {
                operStr += "&nbsp;<span style='display:inline-block;width:26px;'></span>";
            }
            return operStr;
        }
        //取工单状态
        public string GetStatusText(int bussinessType, string taskstatus, string crmstop_stopstatus, string crmstop_applytype)
        {
            return BLL.CallRecord_ORIG.Instance.GetStatusText(bussinessType, taskstatus, crmstop_stopstatus, crmstop_applytype);
        }
        //取分类
        public string GetCategoryFullName(string bussinessType, string BGID, string SCID)
        {
            VisitBusinessTypeEnum bt = (VisitBusinessTypeEnum)CommonFunction.ObjectToInteger(bussinessType);
            if (bt == VisitBusinessTypeEnum.S0_其他系统 || bt == VisitBusinessTypeEnum.None)
            {
                int _scid;
                int.TryParse(SCID, out _scid);
                Entities.SurveyCategory model = BitAuto.ISDC.CC2012.BLL.SurveyCategory.Instance.GetSurveyCategory(_scid);
                if (model != null)
                {
                    return model.Name;
                }
                else
                {
                    return "其他";
                }
            }
            else
            {
                return BLL.Util.GetEnumOptText(typeof(VisitBusinessTypeEnum), (int)bt);
            }
        }
    }
}