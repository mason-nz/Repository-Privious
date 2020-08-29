using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM2014.Server.Web.AjaxServers
{
    public partial class OnlineDialogList : System.Web.UI.Page
    {
        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBound();
            }
        }
        private void DataBound()
        {
            Entities.QueryAllocationAgent query = new Entities.QueryAllocationAgent();

            int count = 0;
            DataTable dt = BLL.AllocationAgent.Instance.GetAllocationList(query, " a.StartTime desc ",
                                                                                   BLL.PageCommon.Instance.PageIndex,
                                                                                PageSize, out count);

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            RecordCount = count;
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        public string GeTimeLength(string starttime, string agentendtime, string userendtime)
        {
            DateTime _starttime;
            DateTime.TryParse(starttime, out _starttime);
            DateTime _endtime = Convert.ToDateTime("9999-12-31 0:00:00");
            
            if (!string.IsNullOrEmpty(userendtime) &&  Convert.ToDateTime(userendtime) != Convert.ToDateTime("9999-12-31 0:00:00") && !string.IsNullOrEmpty(agentendtime) && Convert.ToDateTime(agentendtime) != Convert.ToDateTime("9999-12-31 0:00:00"))
            {
                if (DateTime.Compare(Convert.ToDateTime(userendtime), Convert.ToDateTime(agentendtime)) > 0)
                {
                    DateTime.TryParse(agentendtime, out _endtime);
                }
                else
                {
                    DateTime.TryParse(userendtime, out _endtime);
                }
            }
            else if (!string.IsNullOrEmpty(userendtime) && Convert.ToDateTime(userendtime) != Convert.ToDateTime("9999-12-31 0:00:00"))
            {
                DateTime.TryParse(userendtime, out _endtime);
            }
            else if (!string.IsNullOrEmpty(agentendtime) && Convert.ToDateTime(agentendtime) != Convert.ToDateTime("9999-12-31 0:00:00"))
            {
                DateTime.TryParse(agentendtime, out _endtime);
            }
            if (_endtime!= Convert.ToDateTime("9999-12-31 0:00:00"))
            {
                TimeSpan tsSpan = (TimeSpan)(_endtime - _starttime);
                int TatalSeconds = (int)tsSpan.TotalSeconds;
                if (TatalSeconds <= 60)
                {
                    return TatalSeconds + "秒";
                }
                else
                {
                    int Minutes = TatalSeconds / 60;
                    int seconds = TatalSeconds % 60;
                    return Minutes + "分" + seconds + "秒";
                }
            }
            else
            {
                return "";
            }
        }
    }
}