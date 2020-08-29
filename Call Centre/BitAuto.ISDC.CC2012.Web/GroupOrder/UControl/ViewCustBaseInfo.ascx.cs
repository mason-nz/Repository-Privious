using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
namespace BitAuto.ISDC.CC2012.Web.GroupOrder.UControl
{
    public partial class ViewCustBaseInfo : System.Web.UI.UserControl
    {
        private long Transfer = 0;
        public long RequestTaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    if (long.TryParse(Request["TaskID"], out Transfer))
                    {
                        return Transfer;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
            }
        }
        public string CustName;
        public string Sex;
        public string PlaceStr = string.Empty;
        public string Tels = string.Empty;
        public string AreaStr;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustBaseInfoBind();
            }
        }
        private void CustBaseInfoBind()
        {
            Entities.GroupOrder custBasicInfo = BLL.GroupOrder.Instance.GetGroupOrderCustInfo(RequestTaskID);
            if (custBasicInfo != null)
            {


                CustName = custBasicInfo.CustomerName;
                if (custBasicInfo.UserGender == 1)
                {
                    Sex = "先生";
                }
                else if (custBasicInfo.UserGender == 2)
                {
                    Sex = "女士";
                }

                if (custBasicInfo.ProvinceID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    PlaceStr += BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(custBasicInfo.ProvinceID.ToString());
                }
                if (custBasicInfo.CityID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    PlaceStr += " " + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(custBasicInfo.CityID.ToString());
                }
                //大区查询 强斐 2014-12-17
                BitAuto.YanFa.Crm2009.Entities.AreaInfo info = BLL.Util.GetAreaInfoByPCC(
                                           BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToString(custBasicInfo.ProvinceID),
                                           BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToString(custBasicInfo.CityID), null);
                AreaStr = info == null ? "" : info.DistinctName;
                Tels = custBasicInfo.CustomerTel;
                lblUserName.Text = custBasicInfo.UserName;
            }
        }
    }
}