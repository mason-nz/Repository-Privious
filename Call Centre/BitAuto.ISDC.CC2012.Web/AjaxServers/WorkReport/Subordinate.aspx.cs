using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.WorkReport
{
    public partial class Subordinate : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string txtName
        {
            get
            {
                return Request["txtName"] == null ? "" :
                HttpUtility.UrlDecode(Request["txtName"].ToString().Trim());
            }
        }
        private string SearchType
        {
            get
            {
                return Request["SearchType"] == null ? "" :
                HttpUtility.UrlDecode(Request["SearchType"].ToString().Trim());
            }
        }
        private string HasRead
        {
            get
            {
                return Request["HasRead"] == null ? "" :
                HttpUtility.UrlDecode(Request["HasRead"].ToString().Trim());
            }
        }
        private string HasReply
        {
            get
            {
                return Request["HasReply"] == null ? "" :
                HttpUtility.UrlDecode(Request["HasReply"].ToString().Trim());
            }
        }

        public int PageSize = 20;
        public int GroupLength = 8;
        public int Page = BLL.PageCommon.Instance.PageIndex;
        public string WpUrl = CommonFunction.ObjectToString(ConfigurationManager.AppSettings["WpUrl"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            YanFa.Crm2009.Entities.QueryWorkReport query = new YanFa.Crm2009.Entities.QueryWorkReport();
            if (!string.IsNullOrEmpty(txtName))
            {
                query.CreateUserName = txtName;
            }
            if (!string.IsNullOrEmpty(SearchType))
            {
                query.Types = SearchType;
            }

            if (HasReply == "1")
            {
                query.HasReply = true;
            }
            else if (HasReply == "0")
            {
                query.HasReply = false;
            }

            if (HasRead == "1")
            {
                query.HasRead = true;
            }
            else if (HasRead == "0")
            {
                query.HasRead = false;
            }
            query.RecipientUserID = BLL.Util.GetLoginUserID();

            int totalCount = 0;
            IList<YanFa.Crm2009.Entities.WorkReport> list = YanFa.Crm2009.BLL.WorkReport.Instance.GetList(query, "(CASE WHEN wrr.VisitTime IS NULL THEN 0 ELSE 1 END), wr.PostTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out totalCount);
            repeaterTableList.DataSource = list;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //标题
        protected string GetTitle(YanFa.Crm2009.Entities.WorkReport report)
        {
            string str = "<a href=\"http://" + WpUrl + "/WorkReport/View.aspx?RecID=" + report.RecID + "&Source=1&From=CC\" target=\"_blank\"";
            str += ">" + YanFa.Crm2009.BLL.WorkReport.Instance.GetTitle(report) + "</a>";
            IList<YanFa.Crm2009.Entities.AttachFile> listAttach = BitAuto.YanFa.Crm2009.BLL.AttachFile.Instance.Get(report);
            foreach (YanFa.Crm2009.Entities.AttachFile item in listAttach)
            {
                str += " <a href=\"http://" + WpUrl + item.FilePath + "\" target=\"_blank\" title=\"" + item.FileName + "\"><img src=\"/images/" + GetFileIconName(item.FileType) + ".gif\" style=\"border:0\" alt=\"" + item.FileName + "\" /></a>";
            }
            return str;
        }
        //附件图标
        protected string GetFileIconName(string fileExt)
        {
            string iconName = string.Empty;
            switch (fileExt)
            {
                case "gif":
                case "jpg":
                case "png":
                    iconName = "img";
                    break;
                case "ppt":
                case "pptx":
                    iconName = "huandeng";
                    break;
                case "avi":
                case "flv":
                    iconName = "meiti";
                    break;
                case "pdf":
                    iconName = "pdf";
                    break;
                case "xls":
                case "xlsx":
                    iconName = "table";
                    break;
                case "txt":
                    iconName = "text";
                    break;
                case "vsd":
                    iconName = "visio";
                    break;
                case "htm":
                case "html":
                    iconName = "web";
                    break;
                case "doc":
                case "docx":
                    iconName = "word";
                    break;
                case "rar":
                case "zip":
                case "7z":
                    iconName = "zip";
                    break;
                default:
                    iconName = "weizhi";
                    break;
            }

            return iconName;
        }
        //时间       
        public string DateTimeToString(DateTime? value)
        {
            DateTime date = CommonFunction.ObjectToDateTime(value, new DateTime());
            if (date == new DateTime())
            {
                return "";
            }
            else
            {
                return date.ToString("yyyy-MM-dd HH:mm");
            }
        }
        //审阅状态
        public string GetStatus(YanFa.Crm2009.Entities.WorkReport report)
        {
            if (!YanFa.Crm2009.BLL.WorkReport.Instance.IsViewed(report, BLL.Util.GetLoginUserID()))
            {
                return "未审阅";
            }
            else
            {
                return "已审阅";
            }
        }
    }
}