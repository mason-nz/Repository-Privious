using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject
{
    public partial class DataExport1 : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string eiid = BLL.Util.GetCurrentRequestStr("hidEIID");
            string eName = HttpContext.Current.Server.UrlDecode(BLL.Util.GetCurrentRequestStr("hidEName"));

            if (eiid != string.Empty)
            {
                DataTable dt = BLL.ExamInfo.Instance.GetScoreListByEIID(eiid);
               // ExprotExcel(dt, "考试【" + eName + "】的成绩单");
                BLL.Util.ExportToCSV("考试【" + eName + "】的成绩单", dt);
            }
        }

        
    }
}