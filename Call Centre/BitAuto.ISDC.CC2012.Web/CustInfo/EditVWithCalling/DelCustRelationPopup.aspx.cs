using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class DelCustRelationPopup : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("TID"); }
        }

        public string CustID
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustID"); }
        }
        /// <summary>
        /// 建立删除客户关系的客户ID串
        /// </summary>
        public string DelRelationCustIDs
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("DelRelationCustIDs"); }
        }

        /// <summary>
        /// 弹出框名称
        /// </summary>
        public string PopupName
        {
            get
            {
                string str = (Request["PopupName"] + "").Trim();
                if (string.IsNullOrEmpty(str)) { str = "AnonymousPopup"; }
                return str;
            }
        }

        /// <summary>
        /// 客户修改姓名
        /// </summary>
        public string RequestCustName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustName"); }
        }
        /// <summary>
        /// 客户省份名称
        /// </summary>
        public string RequestCustProvinceName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustProvinceName"); }
        }
        /// <summary>
        /// 客户城市名称
        /// </summary>
        public string RequestCustCityName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustCityName"); }
        }
        /// <summary>
        /// 客户区县名称
        /// </summary>
        public string RequestCustCountyName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustCountyName"); }
        }
        /// <summary>
        /// 客户主营品牌名称
        /// </summary>
        public string RequestCustBrandName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustBrandName"); }
        }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string RequestCustAddress
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustAddress"); }
        }

        /// <summary>
        /// 客户联系人
        /// </summary>
        public string RequestCustLinkName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustLinkName"); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TID))
            {
                BindDelRemark(TID);
            }
        }

        private void BindDelRemark(string tid)
        {
            string remark = BLL.ProjectTask_DelCustRelation.Instance.GetRemarkByTID(tid);
            if (remark != string.Empty)
            {
                tfDescriptionWhenDelete.InnerText = remark;
            }
            else
            {
                string temp = string.Empty;
                Entities.ProjectTask_Cust model = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(tid);
                if (model != null)
                {
                    if (model.CarType == 1 || model.CarType == 3)//新车或，新车/二手车
                    {
                        temp += @"客户所在地域:" + RequestCustProvinceName + " " + RequestCustCityName + " " + RequestCustCountyName + " " + @"

客户名称：" + RequestCustName + @"

客户所属品牌：" + RequestCustBrandName;
                    }
                    else if (model.CarType == 2)//二手车
                    {
                        temp += @"客户名称:" + RequestCustName + " " + @"

客户地址：" + RequestCustAddress + @"

联系人：" + RequestCustLinkName;
                    }
                }


                //                temp += @"客户所在地域:" + RequestCustProvinceName + " " + RequestCustCityName + " " + RequestCustCountyName + " " + @"
                //
                //客户名称：" + RequestCustName;
                //                Entities.CC_Custs model = BLL.CC_Custs.Instance.GetCC_Custs(tid);
                //                if (model != null && (model.CarType == 1 || model.CarType == 3))
                //                {
                //                    temp += @"
                //
                //客户所属品牌：" + RequestCustBrandName;
                //                }
                tfDescriptionWhenDelete.InnerText = temp;

                //                Entities.CC_Custs model = BLL.CC_Custs.Instance.GetCC_Custs(tid);
                //                if (model != null)
                //                {
                //                    Entities.QueryCC_Cust_Brand queryBrandInfo = new Entities.QueryCC_Cust_Brand();
                //                    queryBrandInfo.TID = model.TID;
                //                    int o;
                //                    string s = "";
                //                    DataTable dt = BLL.CC_Cust_Brand.Instance.GetCC_Cust_Brand(queryBrandInfo, "", 1, 10000, out o);
                //                    if (dt != null && dt.Rows.Count > 0)
                //                    {
                //                        for (int i = 0; i < dt.Rows.Count; i++)
                //                        {
                //                            s += dt.Rows[i]["name"] + " ";
                //                        }
                //                    }

                //                    tfDescriptionWhenDelete.InnerText = @"客户所在地域:" +
                //                        Crm2009.BLL.MainBrand.Instance.GetAreaName(model.ProvinceID) + " " +
                //                        Crm2009.BLL.MainBrand.Instance.GetAreaName(model.CityID) + " " +
                //                        Crm2009.BLL.MainBrand.Instance.GetAreaName(model.CountyID) + " " + @"
                //
                //客户名称：" + model.CustName + @"
                //
                //客户所属品牌：" + s;
                //                }
                //                else
                //                {
                //                    tfDescriptionWhenDelete.InnerText = @"客户所在地域:
                //
                //客户名称：
                //
                //客户所属品牌：";
                //                }
            }
        }
    }
}