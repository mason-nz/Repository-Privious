using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustBaseInfo
{
    public partial class CustBaseInfoList : PageBase
    {
        public string RequestCustTel
        {
            get { return Request["CustTel"] == null ? "" : HttpUtility.UrlDecode(Request["CustTel"].ToString()); }
        }

        //CC站点目录
        public string ExitAddress { get { return BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress"); } }

        //添加工单的参数
        public string Param = "";

        public bool DelButtonAuth = false;

        //qizq add 导入功能权限
        public bool DataImportButton = false;

        /// <summary>
        /// 导出按钮权限
        /// </summary>
        public bool DataExportButton = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DelButtonAuth = BLL.Util.CheckButtonRight("SYS024BUT2101");
                DataImportButton = BLL.Util.CheckButtonRight("SYS024BUT2102");
                DataExportButton = BLL.Util.CheckButtonRight("SYS024BUT2103");

                Param = BLL.WOrderRequest.AddWOrderComeIn_NoPhone(Entities.ModuleSourceEnum.M01_客户池).ToString();
            }
        }
    }
}