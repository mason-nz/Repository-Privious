using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class KnowledgeViewForUsers : PageBase
    {
        public string KID
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Request["kid"] != null)
                    {
                        return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["kid"].ToString());
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch
                {
                    return "";
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int kid = 0;
            if (!IsPostBack)
            { 
                if (!string.IsNullOrEmpty(KID) && int.TryParse(KID, out kid))
                {
                    int userID = BLL.Util.GetLoginUserID();

                    //设置为已读
                    BLL.KLReadTag.Instance.SetReadTag(kid, userID, 1);
                }
            }
        }
    }
}