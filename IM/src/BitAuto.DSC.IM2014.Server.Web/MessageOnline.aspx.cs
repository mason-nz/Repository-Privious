using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class MessageOnline : System.Web.UI.Page
    {
        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            Entities.QueryUserMessage query = new Entities.QueryUserMessage();

            int count = 0;
            DataTable dt = BLL.UserMessage.Instance.GetUserMessage(query, " CreateTime desc ",
                                                                                   BLL.PageCommon.Instance.PageIndex,
                                                                                   PageSize, out count);
            //repeaterTableList.DataSource = dt;
            //repeaterTableList.DataBind();

            RecordCount = count;
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }
        protected string getType(string type)
        {
            int _type = 0;
            if (!int.TryParse(type, out _type)) { return ""; }
            return BLL.Util.GetEnumOptText(typeof(Entities.LeaveMessageType), _type);
        }
        public string GetValue(string strvalue)
        {
            return HttpUtility.UrlEncode(strvalue);
        }

    }
}