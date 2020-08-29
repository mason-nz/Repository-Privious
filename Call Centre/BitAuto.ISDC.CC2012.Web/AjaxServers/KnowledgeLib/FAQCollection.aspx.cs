using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class FAQCollection : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3501"))//"个人功能—个人收藏"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }

        #region 数据列表绑定
        public void BindData()
        {
            RecordCount = 0;

            QueryKLFavorites query = new QueryKLFavorites();
            query.UserId = BLL.Util.GetLoginUserID();
            query.Type = 1;

            DataTable dt = BLL.Personalization.Instance.GetCollectedFAQData(query, "CreateDate DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            Rt_FAQ.DataSource = dt;
            Rt_FAQ.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
        }
        #endregion

       
    }
}