using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Demand.DemandHandle
{
    public partial class List : PageBase
    {
        public string demandDetailsUrl = Utils.Config.ConfigurationUtil.GetAppSettingValue("DemandDetailsUrl");
        public int totalCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListBind();
            }
        }

        private void ListBind()
        {
            BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery query = BLL.Util.BindQuery<BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery>(this.Context);
            if (query != null)
            {
                string str = "ProvinceID:" + query.ProvinceID + ",CityID:" + query.CityID + ",CountyID:" + query.CountyID + ",MemberName:" + query.MemberName + ",Department:" + query.Department
                    + ",KeFuName:" + query.KeFuName + ",UserID:" + query.UserID;
                BLL.Loger.Log4Net.Info(str);
                query.Where = " And YJKDemandInfo.Status<>" + (int)BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus.Revoke;
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandInfo(query, "YJKDemandInfo.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, BLL.PageCommon.Instance.PageSize, out totalCount, BLL.Util.GetLoginUserID());
                rplist.DataSource = dt;
                rplist.DataBind();
                litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, totalCount, BLL.PageCommon.Instance.PageSize, PageCommon.Instance.PageIndex, 1);
            }
        }

        public string GetRandomStr()
        {
            Random r = new Random();
            return r.Next(1, 1000).ToString();
        }
    }
}