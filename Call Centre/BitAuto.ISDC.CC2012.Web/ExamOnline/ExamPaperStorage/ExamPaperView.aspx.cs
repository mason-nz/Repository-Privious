using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpertPdf.HtmlToPdf;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage
{
    public partial class ExamPaperView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string RequestEPID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["epid"]))
                {
                    return Request["epid"];
                }
                else
                {
                    return "1";
                }
            }
        }

        public string ISPDF
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["isPdf"]))
                {
                    return Request["isPdf"];
                }
                else
                {
                    return "0";
                }
            }
        }
        public string ExamperName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ISPDF != "1")
                {
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                }

                if (!string.IsNullOrEmpty(RequestEPID))
                {
                    Entities.ExamPaper model = BLL.ExamPaper.Instance.GetExamPaper(Convert.ToInt32(RequestEPID));
                    ExamperName = model.Name;
                    Entities.ExamPaperInfo ExamPaperInfo = BLL.ExamPaper.Instance.GetExamPaperInfo(Convert.ToInt32(RequestEPID));
                    this.ExamPaperView1.ExamPaperInfo = ExamPaperInfo;
                }
            }
        }

        
    }
}