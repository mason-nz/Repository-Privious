using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    public partial class OrderInfo : System.Web.UI.Page
    {
        public int page_seize = 5;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                List<OrderData> data = null;
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
                {
                    //缓存取数据
                    data = Session["OrderInfoListForHB_AjaxList_" + Keyid] as List<OrderData>;
                }
                if (data == null)
                {
                    //查询数据
                    BLL.OrderInfoListForHB server = new BLL.OrderInfoListForHB();
                    data = server.QueryOrderInfo(Tel, DateTime.Today.AddYears(-10), DateTime.Today.AddDays(1));
                    //缓存
                    Session["OrderInfoListForHB_AjaxList_" + Keyid] = data;
                }
                if (data != null && data.Count > 0)
                {
                    //分页
                    int start = (PageCommon.Instance.PageIndex - 1) * page_seize;
                    int end = PageCommon.Instance.PageIndex * page_seize - 1;
                    List<OrderData> result = Pages(data, start, end);

                    OrderDataList.DataSource = result;
                    OrderDataList.DataBind();

                    litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 3, data.Count, page_seize, PageCommon.Instance.PageIndex, 5);
                }
                else
                {
                    Response.Write("没有找到匹配的订单数据！");
                    Response.End();
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("查询订单信息出现异常：" + ex.Message);
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
                        return "<a  href=\"javascript:void(0)\" onclick=\"CarFinancialLoginToUrl('" + url + "','1');\">" + orderid + "</a>";
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