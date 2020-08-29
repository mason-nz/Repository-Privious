using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class MonitorRealAgentBatchTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt = DefaultChannelHandler.StateManager.GetAllAgentsStatus4Monotor();
            int n = 0;
            foreach (DataRow row in dt.Rows)
            {
                n += Convert.ToInt32(row[2]);
            }
            lbStatus.Text = string.Format("当期总坐席个数{0}，当期总会话个数{1},当前内存中对象数{2}", dt.Rows.Count, n, DefaultChannelHandler.StateManager.GetAllCometClients().Count);
            this.gdView.DataSource = dt;
            this.gdView.DataBind();
        }
    }
}