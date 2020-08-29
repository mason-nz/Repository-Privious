using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.ITSC.Chitunion2017.Web.RecommendManager
{
    public partial class Broadcast : System.Web.UI.Page
    {
        public DataTable dts = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dts = BLL.HomeCategory.Instance.SelectSelectedCategory((int)Entities.Enum.MediaType.Broadcast, 0);
            }
        }
    }
}