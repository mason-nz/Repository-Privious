using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.YanFa.Crm2009.Entities.Constants;
using BitAuto.YanFa.Crm2009.Entities;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class UCMember : System.Web.UI.UserControl
    {
        public BitAuto.YanFa.Crm2009.Entities.DMSMember Member = null;
        public string ControlName = "";
        public string Lat = "", Lng = "";

        public string MemberCode = "";
        public string MemberName = "";
        public string CustID = "";
        public string Phone = "";

        public UCMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        public UCMember(BitAuto.YanFa.Crm2009.Entities.DMSMember member)
            : this()
        {
            this.Member = member;
            this.ControlName = "易湃会员(" + Member.Name + ")";
            if (member.Status == -1)
            {
                this.ControlName += " -已停用删除";
            }
            MemberCode = this.Member.MemberCode;
            MemberName = this.Member.Name;
            CustID = this.Member.CustID;
            Phone = this.Member.Phone;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (this.Member != null)
                {
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
    }
}