using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TongPop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TaskID = "";
        public string ProvinceID = "";
        public string CityID = "";
        public string Mobile = "";
        public string UserName = "";

        public int MasterBrandID = 0;
        public int SerialID = 0;

        protected void Page_Load(object sender, EventArgs e)
        { 
            TaskID = BLL.Util.GetCurrentRequestQueryStr("TaskID");
            ProvinceID = BLL.Util.GetCurrentRequestQueryStr("ProvinceID");
            CityID = BLL.Util.GetCurrentRequestQueryStr("CityID");
            Mobile = BLL.Util.GetCurrentRequestQueryStr("Mobile");
            UserName = BLL.Util.GetCurrentRequestQueryStr("UserName");
            MasterBrandID = BLL.Util.GetCurrentRequestQueryInt("BrandID");
            SerialID = BLL.Util.GetCurrentRequestQueryInt("SerialID");
            //CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(CarTypeID, out SerialID, out MasterBrandID);

        }
    }
}