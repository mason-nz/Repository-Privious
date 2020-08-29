using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.EasyPass;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab
{
    public partial class OrderInfoListForHB_AjaxList : PageBase
    {
        public string Tel
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("tel");
            }
        }
        public string Keyid
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("keyid");
            }
        }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            List<OrderData> data = null;
            if (!string.IsNullOrEmpty(Tel))
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
                {
                    //缓存取数据
                    data = Session["OrderInfoListForHB_AjaxList_" + Keyid] as List<OrderData>;
                }
                if (data == null)
                {
                    //查询数据
                    BLL.OrderInfoListForHB server = new BLL.OrderInfoListForHB();
                    data = server.QueryOrderInfo(Tel, null, null, 10000);
                    //缓存
                    Session["OrderInfoListForHB_AjaxList_" + Keyid] = data;
                }
            }
            if (data != null)
            {
                PageSize = 10;
                litPagerDown.Visible = true;
                //分页
                int start = (PageCommon.Instance.PageIndex - 1) * PageSize;
                int end = PageCommon.Instance.PageIndex * PageSize - 1;
                List<OrderData> result = Pages(data, start, end);

                OrderDataList.DataSource = result;
                OrderDataList.DataBind();
                RecordCount = data.Count;
                litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, PageCommon.Instance.PageIndex, 1010);
            }
            else
            {
                OrderDataList.DataSource = null;
                OrderDataList.DataBind();
                litPagerDown.Text = "";
            }
        }

        private List<OrderData> Pages(List<OrderData> data, int start, int end)
        {
            if (start < 0)
                start = 0;
            if (end > data.Count - 1)
            {
                end = data.Count - 1;
            }

            List<OrderData> result = new List<OrderData>();
            for (int i = start; i <= end; i++)
            {
                result.Add(data[i]);
            }
            return result;
        }

        public string GetLinkString(string url, string orderid, string type)
        {
            try
            {
                switch (type)
                {
                    case "7":
                        string EPEmbedCCHBugCar_URL = ConfigurationUtil.GetAppSettingValue("EPEmbedCCHBugCar_URL");
                        string EPEmbedCCHBuyCar_APPID = ConfigurationUtil.GetAppSettingValue("EPEmbedCCHBuyCar_APPID");
                        return "<a  href=\"javascript:void(0)\" urlstr=\"" + url + "\" onclick=\"GoToEpURL(this,'" + EPEmbedCCHBugCar_URL + "','" + EPEmbedCCHBuyCar_APPID + "');\">" + orderid + "</a>";
                    case "9":
                        return "<a  href=\"javascript:void(0)\" onclick=\"var obj = new Object();obj.businessType = 'yichechedai';obj.callbackurl='" + url + "';OtherBusinessLogin(obj);\">" + orderid + "</a>";
                    default:
                        if (url != "")
                        {
                            string link = "<a href=\"" + url + "\" target=\"_blank\">" + orderid + "</a>";
                            return link;
                        }
                        else
                        {
                            return orderid;
                        }
                }
            }
            catch
            {
                return orderid;
            }
        }
    }
}