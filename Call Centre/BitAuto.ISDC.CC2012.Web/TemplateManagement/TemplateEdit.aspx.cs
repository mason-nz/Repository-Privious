using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TemplateEdit : PageBase
    {
        public string TTCode
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["ttcode"]) ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ttcode"]);
            }
        }

        public string TPageName = "";
        public string BGID = "";
        public string CID = "";
        public string Desc = "";
        public string templateStatus = "";//模板状态
        public string IsShowBtn = "";//是否显示
        public string IsShowWorkOrderBtn = "";//是否显示
        public string IsShowSendMsgBtn = "";//是否显示
        public string IsShowQiCheTong = "";//是否显示汽车通按钮
        protected string IsShowSubmitOrder = "";//是否显示插入订单按钮  

        public string ALLNotEstablishReasonStr = "";
        public string ALLNotSuccessReasonStr = "";
        public string BoolStr = "1|是;0|否";

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024MOD510204") && !BLL.Util.CheckRight(userId, "SYS024MOD510205") && !BLL.Util.CheckRight(userId, "SYS024MOD5101"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
            ALLNotEstablishReasonStr = CallResult_ORIG_Task.GetNotEstablishReasonStr();
            ALLNotSuccessReasonStr = CallResult_ORIG_Task.GetNotSuccessReasonStr();

            if (!IsPostBack && TTCode != "")
            {
                Entities.QueryTPage query = new Entities.QueryTPage();
                query.TTCode = TTCode;

                int totalCount = 0;
                DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 999, out totalCount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    TPageName = dt.Rows[0]["TPName"].ToString();
                    BGID = dt.Rows[0]["BGID"].ToString();
                    CID = dt.Rows[0]["SCID"].ToString();
                    Desc = dt.Rows[0]["TPContent"].ToString();
                    IsShowBtn = dt.Rows[0]["IsShowBtn"].ToString();
                    IsShowWorkOrderBtn = dt.Rows[0]["IsShowWorkOrderBtn"].ToString();
                    IsShowSendMsgBtn = dt.Rows[0]["IsShowSendMsgBtn"].ToString();
                    IsShowQiCheTong = dt.Rows[0]["IsShowQiCheTong"].ToString();
                    IsShowSubmitOrder = dt.Rows[0]["IsShowSubmitOrder"].ToString();

                    templateStatus = BLL.TPage.Instance.getStatus(dt.Rows[0]["RecID"].ToString(), dt.Rows[0]["Status"].ToString()).ToString();
                }
            }
        }
    }
}