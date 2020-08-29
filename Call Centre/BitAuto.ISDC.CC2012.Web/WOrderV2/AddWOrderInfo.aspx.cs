using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class AddWOrderInfo : PageBase
    {
        //参数请求类
        public WOrderRequest RequestInfo = null;
        //网页标题
        public string PageTitle = "新增工单";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RequestInfo = WOrderRequest.GetWOrderRequestFromRequest();
                if (RequestInfo == null)
                {
                    BLL.Util.CloseCurrentPageAfterAlert(Response, "传入参数不正确，终止操作！");
                }
                else
                {
                    //设置标题
                    //SetPageTitle();
                }
            }
        }
        /// 设置标题
        /// <summary>
        /// 设置标题
        /// </summary>
        private void SetPageTitle()
        {
            string m_name = BLL.Util.GetEnumOptText(typeof(ModuleSourceEnum), (int)RequestInfo.ModuleSource);
            string c_name = BLL.Util.GetEnumOptText(typeof(CallSourceEnum), (int)RequestInfo.CallSource);
            PageTitle = "新增工单（" + m_name + " - " + c_name + "）";
            PageTitle = PageTitle.Replace("无 - ", "");
            PageTitle = PageTitle.Replace(" - IM", "");
        }
    }
}