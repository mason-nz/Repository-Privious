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
    public partial class MyWorkReport : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string SearchType
        {
            get
            {
                return Request["SearchType"] == null ? "" :
                HttpUtility.UrlDecode(Request["SearchType"].ToString().Trim());
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
            query.CreateUser = BLL.Util.GetLoginUserID();
            if (!string.IsNullOrEmpty(SearchType))
            {
                query.Types = SearchType;
            }
            int totalCount = 0;
            //查询数据 
            IList<YanFa.Crm2009.Entities.WorkReport> list = YanFa.Crm2009.BLL.WorkReport.Instance.GetList(query, "wr.[Status] desc,(case when wr.[Status]=1 then wr.[CreateTime] when wr.[Status]=0 then wr.[PostTime] else null end) desc", BLL.PageCommon.Instance.PageIndex, PageSize, out totalCount);
            repeaterTableList.DataSource = list;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //标题
        protected string GetTitle(YanFa.Crm2009.Entities.WorkReport report)
        {
            string title = string.Empty;
            if (report.Status == YanFa.Crm2009.Entities.WorkReportStatus.Draft)
            {
                title += "<span style=\"float:right;color:#999;margin-right:10px;\">草稿</span> <a href=\"http://" + WpUrl + "/WorkReport/Edit.aspx?RecID=" + report.RecID + "&From=CC&BtnID=btnsearch\" target=\"_blank\">" + YanFa.Crm2009.BLL.WorkReport.Instance.GetTitle(report) + "</a>";
            }
            else
            {
                title += "<a href=\"http://" + WpUrl + "/WorkReport/View.aspx?RecID=" + report.RecID + "&Source=0&From=CC\" target=\"_blank\">" + YanFa.Crm2009.BLL.WorkReport.Instance.GetTitle(report) + "</a>";
            }
            IList<YanFa.Crm2009.Entities.AttachFile> listAttach = BitAuto.YanFa.Crm2009.BLL.AttachFile.Instance.Get(report);
            foreach (YanFa.Crm2009.Entities.AttachFile item in listAttach)
            {
                title += " <a href=\"http://" + WpUrl + item.FilePath + "\" target=\"_blank\" title=\"" + item.FileName + "\"><img src=\"/images/" + GetFileIconName(item.FileType) + ".gif\" style=\"border:0\" alt=\"" + item.FileName + "\" /></a>";
            }
            return title;
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
        //操作
        protected string GetOpearte(YanFa.Crm2009.Entities.WorkReport report)
        {
            string strOperate = string.Empty;
            if (report.Status == YanFa.Crm2009.Entities.WorkReportStatus.Draft)
            {
                strOperate += "<a href=\"http://" + WpUrl + "/WorkReport/Edit.aspx?RecID=" + report.RecID + "&From=CC&BtnID=btnsearch\" target=\"_blank\">编辑</a>";
                strOperate += "&nbsp;&nbsp;<a href=\"javascript:DeleteReport(" + report.RecID + ");\">删除</a>";
            }
            else
            {
                //查看来源：0我的工作报告；1下属工作报告；3所有报告
                strOperate += "<a href=\"http://" + WpUrl + "/WorkReport/View.aspx?RecID=" + report.RecID + "&From=CC\" target=\"_blank\">查看</a>";
                if (!YanFa.Crm2009.BLL.WorkReport.Instance.IsView(report.RecID))
                {
                    strOperate += "&nbsp;&nbsp;<a href=\"javascript:revokeReport(" + report.RecID + ");\">撤回</a>";
                }
                if (report.Type == YanFa.Crm2009.Entities.WorkReportType.Daily)
                {
                    //strOperate += "&nbsp;&nbsp;<a href=\"javascript:ExportReport(" + report.RecID + ");\">导出</a>";
                }
            }
            return strOperate;
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
    }
}