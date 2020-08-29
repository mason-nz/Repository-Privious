using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers
{
    public partial class TagManagementList : System.Web.UI.Page
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusiTypeID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("busitypeid");
            }
        }

        /// <summary>
        /// 状态 “,”分割
        /// </summary>
        public string StrStatus
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("status");
            }
        }

        public int totalCount = 0;
        public int PageSize = 20;
        public int GroupLength = 8;
        public int PageIndex = BLL.PageCommon.Instance.PageIndex;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD1021"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                BindData();
            }
        }
        private void BindData()
        {
            string strBusiTypeId = "";
            try
            {
                strBusiTypeId = Int32.Parse(BusiTypeID).ToString();
            }
            catch { }

            Entities.QueryWOrderTag query = new Entities.QueryWOrderTag();
            query.BusiTypeID = strBusiTypeId;
            query.Status = StrStatus;

            DataTable dt = BLL.WOrderTag.Instance.GetLevelData(query, "", PageIndex, PageSize, out totalCount);
            rpt.DataSource = dt;
            rpt.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }



        public string GetZYTitle(string value, string type)
        {
            if (type == "1")
            {
                if (value.Trim() == "1")
                {
                    return "在用";
                }
                else if (value.Trim() == "0")
                {
                    return "停用";
                }
                else if (value.Trim() == "-1")
                {
                    return "已删除";
                }
            }

            return "";
        }

    }
}