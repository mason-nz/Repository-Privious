using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyMapping
{
    public partial class ExportFilter : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string SIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SIID"));
            }
        }
        public string ProjectID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ProjectID"));
            }
        }
        //1、2-代表是（数据清洗）任务的导出；3-代表客户回访的问卷导出；4-代表是其他任务的问卷导出
        public string TypeID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("typeID"));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userID, "SYS024BUT5010"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}