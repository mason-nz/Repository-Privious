using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ExternalTask.CustCategory
{
	public partial class DealerInfoView : System.Web.UI.UserControl
    {

        private string custid = string.Empty;
        /// <summary>
        /// 客户ＩＤ
        /// </summary>
        public string CustID
        {
            get
            {
                return custid;
            }
            set
            {
                custid = value;
            }
        }

        public DealerInfoView(string cId)
        {
            this.custid = cId;
        }
        public DealerInfoView()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetDealerInfoModel(CustID);
        }
        private void GetDealerInfoModel(string CustID)
        {
            if (!string.IsNullOrEmpty(CustID))
            {
                Entities.DealerInfo DealerInfoModel = null;
                DealerInfoModel = BLL.DealerInfo.Instance.GetDealerInfo(CustID);

                if (DealerInfoModel != null)
                {
                    this.lblMemberName.Text = DealerInfoModel.Name;
                    this.lblMemberID.Text = DealerInfoModel.MemberCode;
                    this.lblRemark.Text = DealerInfoModel.Remark;

                    DataTable dt = null;
                    QueryDealerBrandInfo query = new QueryDealerBrandInfo();
                    query.CustID = CustID;
                    int totalCount = 0;
                    dt = BLL.DealerBrandInfo.Instance.GetDealerBrandInfo(query, "", 1, 1, out totalCount);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lblBrand.Text += dt.Rows[i]["Name"].ToString() + ",";
                        }
                        if (lblBrand.Text != "")
                        {
                            lblBrand.Text = lblBrand.Text.Substring(0, lblBrand.Text.Length - 1);
                        }
                    }
                    if (DealerInfoModel.CityScope > 0)
                    {
                        lblCityScope.Text = BLL.Util.GetEnumOptText(typeof(CityScope), Convert.ToInt32(DealerInfoModel.CityScope));
                    }
                    if (DealerInfoModel.CarType > 0)
                    {
                        lblCarType.Text = BLL.Util.GetEnumOptText(typeof(CarType), Convert.ToInt32(DealerInfoModel.CarType));
                    }
                    if (DealerInfoModel.MemberStatus > 0)
                    {
                        lblMemberStatus.Text = BLL.Util.GetEnumOptText(typeof(MemberStatus), Convert.ToInt32(DealerInfoModel.MemberStatus));
                    }
                    if (DealerInfoModel.MemberType > 0)
                    {
                        lblMemberType.Text = BLL.Util.GetEnumOptText(typeof(DealerCategory), Convert.ToInt32(DealerInfoModel.MemberType));
                    }

                }
            }
        }

    }
}