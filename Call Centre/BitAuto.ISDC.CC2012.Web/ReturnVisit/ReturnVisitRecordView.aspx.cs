using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class ReturnVisitRecordView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string RVID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["RVID"]))
                {
                    return Request["RVID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                if (!string.IsNullOrEmpty(RVID))
                {
                    FillPage(RVID);
                }
            }
        }

        protected void FillPage(string RVID)
        {
            DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetVisitInfoByRVID(RVID);
            if (dt != null && dt.Rows.Count > 0)
            {
                this.lblCustName.Text = dt.Rows[0]["CustName"].ToString();
                this.lblCustID.Text = dt.Rows[0]["CustID"].ToString();
                this.lblMemberName.Text = dt.Rows[0]["Name"].ToString();
                this.lblLinkMan.Text = dt.Rows[0]["CName"].ToString();
                this.lblVisitType.Text = dt.Rows[0]["DictName"].ToString();
                if (dt.Rows[0]["businessline"] != DBNull.Value && dt.Rows[0]["businessline"].ToString() != "-2" && dt.Rows[0]["businessline"].ToString() != "0")
                {
                    int number = 0;
                    if (int.TryParse(dt.Rows[0]["businessline"].ToString(), out number))
                    {
                        if (number == 1)
                        {
                            this.lblBussinesLine.Text = "新车";
                        }
                        else if (number == 2)
                        {
                            this.lblBussinesLine.Text = "二手车";
                        }
                        else if (number == 3)
                        {
                            this.lblBussinesLine.Text = "新车 二手车";
                        }
                        else if (number == 4)
                        {
                            this.lblBussinesLine.Text = "易卡";
                        }
                        else if (number == 5)
                        {
                            this.lblBussinesLine.Text = "新车 易卡";
                        }
                        else if (number == 6)
                        {
                            this.lblBussinesLine.Text = "二手车 易卡";
                        }
                        else if (number == 7)
                        {
                            this.lblBussinesLine.Text = "新车 二手车 易卡";
                        }

                    }

                }
                lblVisitPerson.Text = dt.Rows[0]["truename"].ToString();
                lblVisitDate.Text = Convert.ToDateTime(dt.Rows[0]["begintime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                lblRemark.Text = dt.Rows[0]["Remark"].ToString();

            }


        }

    }
}