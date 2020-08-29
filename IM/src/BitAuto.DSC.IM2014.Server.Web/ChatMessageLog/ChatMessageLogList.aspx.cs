using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM2014.Server.Web.ChatMessageLog
{
    public partial class ChatMessageLogList : System.Web.UI.Page
    {
        #region 参数
        private string BeginTime
        {
            get
            {
                return string.IsNullOrEmpty(Request["BeginTime"]) ? "" :
                HttpUtility.UrlDecode(Request["BeginTime"].ToString().Trim());
            }
        }

        private string EndTime
        {
            get
            {
                return string.IsNullOrEmpty(Request["EndTime"]) ? "" :
                HttpUtility.UrlDecode(Request["EndTime"].ToString().Trim());
            }
        }

        private string Receiver
        {
            get
            {
                return string.IsNullOrEmpty(Request["Receiver"]) ? "" :
                HttpUtility.UrlDecode(Request["Receiver"].ToString().Trim());
            }
        }
        private string Sender
        {
            get
            {
                return string.IsNullOrEmpty(Request["Sender"]) ? "" :
                HttpUtility.UrlDecode(Request["Sender"].ToString().Trim());
            }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int PageIndex = BLL.PageCommon.Instance.PageIndex;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {            
            //默认显示上周一周数据
            //上周一时间=today-(6+today.dayofweek)
            string sBeginTime = "", sEndTime = "";
            if (string.IsNullOrEmpty(BeginTime) && string.IsNullOrEmpty(EndTime))
            {
                DateTime LastMonday = DateTime.Now.AddDays(-(6 + Convert.ToDouble(DateTime.Now.DayOfWeek)));
                DateTime LastSunday = LastMonday.AddDays(6);

                sBeginTime = LastMonday.ToShortDateString() + " 00:00:00";
                sEndTime = LastSunday.ToShortDateString() + " 23:59:59";
            }
            else
            {
                sBeginTime = BeginTime + " 00:00:00";
                sEndTime = EndTime + " 23:59:59";
            }

            Entities.QueryChatMessageLog query = new Entities.QueryChatMessageLog();
            query.Sender = Sender;
            query.Receiver = Receiver;
            query.BeginTime = BeginTime;
            query.EndTime = EndTime;

            DataTable dt = BLL.ChatMessageLog.Instance.GetChatMessageLog(query, "",PageIndex , PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }}