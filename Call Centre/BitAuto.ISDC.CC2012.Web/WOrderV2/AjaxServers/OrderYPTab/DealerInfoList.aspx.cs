using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers
{
    public partial class DealerInfoList : System.Web.UI.Page
    {
        public string DealerName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("DealerName");
            }
        }

        //TODO:CodeReview 2016-08-10 这块若想实现不分页的话，可以在存储过程中，通过传入的PageSize参数做判断，如-1
        public int PageSize = -1;
        public string DealerPageURL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DealerName))
            {
                DealerPageURL = ConfigurationUtil.GetAppSettingValue("DealerPageURL");

                int totalCount = 0;

                DataTable dt = BLL.DealerInfo.Instance.GetMemberInfo(DealerName, "", "", "", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DealerList.DataSource = dt;
                    DealerList.DataBind();
                }
            }
        }

        public string GetCopyParaStr(object funnName, object memberCode, object dealerType, object address)
        {
            //Eval("membertype").ToString() == "1" ? "4s" : Eval("membertype").ToString() == "2" ? "特许经销商" : "综合店"
            string strMembertype = CommonFunction.ObjectToString(dealerType);
            if (strMembertype == "1")
            {
                strMembertype = "4s";
            }
            else if (strMembertype == "2")
            {
                strMembertype = "特许经销商";
            }
            else
            {
                strMembertype = "综合店";
            }
            return "javascript:CopyDealerInfoToOrder(\"" + funnName + "\",\"" + memberCode + "\",\"" + strMembertype + "\",\"" + address + "\");";
        }
    }
}