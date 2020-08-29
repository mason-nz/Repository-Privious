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
    public partial class KnowledgeCollection : BitAuto.ISDC.CC2012.Web.Base.PageBase
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

        public void BindData()
        {
            QueryKLFavorites query = new QueryKLFavorites();
            query.UserId = BLL.Util.GetLoginUserID();
            query.Type = 0;

            DataTable dt = BLL.Personalization.Instance.GetCollectedKnowledgeData(query, " CreateDate DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();


            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);

        }

        //分类
        public string getCategory(string kcid)
        {
            string categoryStr = string.Empty;
            int _kcid;
            if (int.TryParse(kcid, out _kcid))
            {
                Entities.KnowledgeCategory model = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(_kcid);
                if (model != null)
                {
                    categoryStr = model.Name;
                }
            }
            return categoryStr;
        }

        //标题字数控制
        public string getTitle(string title)
        {
            string Title = string.Empty;

            if (title.Length > 40)
            {
                Title = title.Substring(0, 40) + "......";
            }
            else
            {
                Title = title;
            }

            return Title;
        }
        //内容字数控制
        public string getContent(string content)
        {
            string Content = string.Empty;

            if (content.Length > 200)
            {
                Content = content.Substring(0, 200) + "......";
            }
            else
            {
                Content = content;
            }

            return Content;
        }
    }
}