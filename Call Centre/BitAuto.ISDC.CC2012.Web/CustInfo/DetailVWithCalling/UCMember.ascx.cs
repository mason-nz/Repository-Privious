using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.YanFa.Crm2009.Entities.Constants;
using BitAuto.YanFa.Crm2009.Entities;
using System.Configuration;
namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailVWithCalling
{
    public partial class UCMember : System.Web.UI.UserControl
    {
        #region internal
        public string CRMMemberServerInfo = System.Configuration.ConfigurationManager.AppSettings["CRMMemberServerInfo"].ToString();
        public BitAuto.YanFa.Crm2009.Entities.DMSMember Member = null;
        public string MemberID = "";
        public string ControlName = "";
        public string Lat = "", Lng = "";
        public string AddWorderv2Url_Phone = "";
        #endregion


        public UCMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        public UCMember(BitAuto.YanFa.Crm2009.Entities.DMSMember member)
            : this()
        {
            this.MemberID = member.ID.ToString();
            this.Member = member;
            this.ControlName = "易湃会员(" + Member.Name + ")";
        }

        protected string cTel = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Member != null)
                {
                    if (Member.Phone == "")
                    {
                        AddWorderv2Url_Phone = "href='javascript:void(0)' ";
                    }
                    else
                    {
                        AddWorderv2Url_Phone = "href='/WOrderV2/AddWOrderInfo.aspx?" +
                            BLL.WOrderRequest.AddWOrderComeIn_CallOut(Member.Phone, Member.CustID).ToString() + "' ";
                        AddWorderv2Url_Phone += "target='_blank'";
                    }

                    //根据会员找客户
                    Entities.QueryCrmCustInfo query = new Entities.QueryCrmCustInfo();
                    query.CustID = this.Member.CustID;
                    int total = 0;
                    System.Data.DataTable dt = BLL.CrmCustInfo.Instance.GetCC_CrmCustInfo(query, "", 1, 9999, out total);
                    int cartype = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        cartype = Convert.ToInt16(dt.Rows[0]["CarType"]);
                    }

                    if (this.Member.SyncStatus == 170002 && this.Member.MemberType != "9" && (cartype == 2 || cartype == 3))
                    {
                        this.aTaoChe1.Visible = true;
                        this.aTaoChe2.Visible = true;

                        string url = "";
                        url = "http://www.taoche.com/d" + this.Member.MemberCode;
                        this.aTaoChe1.HRef = url;

                        url = "http://pg.ucar.cn/tongjiapi/adminvendordetails.aspx?id=" + this.Member.MemberCode + "&sign=" + GetSign(this.Member.MemberCode);
                        this.aTaoChe2.HRef = url;
                    }

                    this.spanMemberName.InnerText = Member.Name;
                    this.spanSyncStatus.InnerText = BitAuto.YanFa.Crm2009.BLL.DMSMember.GetSyncStatusDescription(Member.SyncStatus);
                    if (Member.SyncStatus == (int)EnumDMSSyncStatus.CreateSuccessful)
                    {
                        this.spanMemberCode.InnerText = Member.MemberCode;
                        liMemberCode.Visible = true;
                        liMemberCode.Style.Add("display", "block");
                    }
                    this.spanMemberAbbr.InnerText = Member.Abbr;
                    this.spanPhone.InnerText = Member.Phone;

                    cTel = Member.Phone;

                    this.spanBrandName.InnerText = Member.BrandNames;

                    string area = "";
                    if (!string.IsNullOrEmpty(this.Member.ProvinceID))
                    {
                        area += BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.Member.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(this.Member.CityID))
                    {
                        area += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.Member.CityID);
                    }
                    if (!string.IsNullOrEmpty(this.Member.CountyID))
                    {
                        area += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.Member.CountyID);
                    }
                    this.spanArea.InnerText = area;

                    this.spanAddress.InnerText = Member.ContactAddress;

                    if (Member.MapCoordinateList.Count > 0)
                    {
                        foreach (BitAuto.YanFa.Crm2009.Entities.DMSMapCoordinate map in Member.MapCoordinateList)
                        {
                            if (map.MapProviderName.ToLower() == Constant.MapProviderName.ToLower())
                            {
                                this.Lat = Member.MapCoordinateList[0].Latitude;
                                this.Lng = Member.MapCoordinateList[0].Longitude;
                                break;
                            }
                        }
                    }
                    spanMemberType.InnerText = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberTypeText(Member.MemberType);
                    this.spanFax.InnerText = Member.Fax;
                    this.spanCompanyWebSite.InnerText = Member.CompanyWebSite;
                    this.spanEmail.InnerText = Member.Email;
                    this.spanZipcode.InnerText = Member.Postcode;

                    this.spanSerial.InnerText = Member.SerialNames;

                    this.spanTrafficInfo.InnerText = Member.TrafficInfo;
                    this.spanEnterpriseBrief.InnerText = Member.EnterpriseBrief;
                    this.spanNotes.InnerText = Member.Remarks;

                }
            }
        }

        //获取引用签名
        public string GetSign(string strMemberID)
        {
            string strKey = ConfigurationManager.AppSettings["CSTInfoShowKey"].ToString();
            return BLL.Util.MD5Encrypt(strMemberID + strKey).ToLower();
        }
    }
}