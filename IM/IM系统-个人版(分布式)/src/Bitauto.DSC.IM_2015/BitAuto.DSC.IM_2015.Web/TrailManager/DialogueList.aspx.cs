using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.TrailManager
{
    public partial class DialogueList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!Page.IsPostBack)
            {
                BindSourceType();
            }
        }

        private void BindSourceType()
        {
            var list = BLL.Util.GetAllSourceHaveYiCheTotalType(true);
            ddSourceType.DataSource = list;
            ddSourceType.DataValueField = "SourceTypeValue";
            ddSourceType.DataTextField = "SourceTypeName";
            ddSourceType.DataBind();


            var sublist = BLL.Util.GetYiCheSourceType("100",true);
            ddSubSourceType.DataSource = sublist;
            ddSubSourceType.DataValueField = "SourceTypeValue";
            ddSubSourceType.DataTextField = "SourceTypeName";
            ddSubSourceType.DataBind();
        }
    }
}