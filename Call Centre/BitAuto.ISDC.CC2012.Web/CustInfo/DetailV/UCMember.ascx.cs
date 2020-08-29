using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.WebService;
using System.Data;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class UCMember : System.Web.UI.UserControl
    {
        #region internal

        //public int TaskID = -1;

        public Entities.ProjectTask_DMSMember Member = null;
        //public int MemberID = -1;
        //public string OriginalDMSMemberID = "";

        public string ControlName = "";
        public string Lat = "", Lng = "";

        ////页面初始化时的地区
        //public string InitialProvinceID = "";
        //public string InitialCityID = "";
        //public string InitialCountyID = "";

        #endregion


        public UCMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        public UCMember(Entities.ProjectTask_DMSMember member)
            : this()
        {
            //this.MemberID = member.ID;
            //this.tfMemberID.Value = member.ID.ToString();
            //this.OriginalDMSMemberID = string.IsNullOrEmpty(member.OriginalDMSMemberID) ? "" : member.OriginalDMSMemberID;
            this.Member = member;
            this.ControlName = "易湃会员(" + Member.Name + ")";
            if (!string.IsNullOrEmpty(member.MemberCode))
            {
                spanMemberID.InnerText = member.MemberCode;
                liMemberID.Style.Remove("display");
                if (member.SyncStatus != null)
                {
                    spanMemberSyncStatus.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(member.SyncStatus.Value);
                    liMemberSyncStatus.Style.Remove("display");
                    li400.Style.Remove("display");
                }
                liMemberCooperated.Style.Remove("display");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string originalDMSMemberID = string.Empty;
                if (this.Member != null)
                {
                    originalDMSMemberID = Member.MemberID.ToString();
                    this.spanMemberName.InnerText = Member.Name;
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

                    this.Lat = Member.Lantitude;
                    this.Lng = Member.Longitude;
                    //if (Member.MemberType == "1")
                    //{
                    //    spanMemberType.InnerText = "4S";
                    //}
                    //else if (Member.MemberType == "2")
                    //{
                    //    spanMemberType.InnerText = "特许经销商";
                    //}
                    //else if (Member.MemberType == "3")
                    //{
                    //    spanMemberType.InnerText = "综合店";
                    //}
                    //else if (Member.MemberType == "4")
                    //{
                    //    spanMemberType.InnerText = "车易达";
                    //}
                    //else if (Member.MemberType == "5")
                    //{
                    //    spanMemberType.InnerText = "二手车中心";
                    //}
                    spanMemberType.InnerText = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberTypeText(Member.MemberType);
                    this.spanFax.InnerText = Member.Fax;
                    this.spanCompanyWebSite.InnerText = Member.CompanyWebSite;
                    this.spanEmail.InnerText = Member.Email;
                    this.spanZipcode.InnerText = Member.Postcode;

                    this.spanSerial.InnerText = Member.SerialNames;

                    this.spanTrafficInfo.InnerText = Member.TrafficInfo;
                    this.spanEnterpriseBrief.InnerText = Member.EnterpriseBrief;
                    this.spanNotes.InnerText = Member.Remarks;

                    if (!string.IsNullOrEmpty(Member.OriginalDMSMemberID))
                    {
                        originalDMSMemberID = Member.OriginalDMSMemberID;
                        BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(Member.OriginalDMSMemberID));
                        if (DMSMember != null)
                        {
                            if (DMSMember.Cooperated == 1 && DMSMember.CooperateStatus == 0)
                            {
                                this.spanMemberCooperated.InnerText = "有排期";
                            }
                            else if (DMSMember.Cooperated <= 0)
                            {
                                this.spanMemberCooperated.InnerText = "无排期";
                            }
                            else if (DMSMember.CooperateStatus == 1 || DMSMember.CooperateStatus == 2)//销售+试用
                            {
                                this.spanMemberCooperated.InnerText = "合作中";
                            }
                            if (!string.IsNullOrEmpty(DMSMember.MemberCode))
                            {
                                int memberCode = 0;
                                if (int.TryParse(DMSMember.MemberCode, out memberCode))
                                {
                                    DealerInfoServiceHelper soapClient = new DealerInfoServiceHelper();
                                    DataSet ds = soapClient.GetDealer400(memberCode);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        span400.InnerText = ds.Tables[0].Rows[0][2].ToString();
                                    }
                                }
                            }

                        }
                    }
                    //int AnswerCount = BLL.CC_TaskQuestionnaire.Instance.GetMaxAnswerCount(originalDMSMemberID);
                    //if (AnswerCount != 0)
                    //{
                    //    Control ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCCurrentSurvey.ascx", Member.TID, originalDMSMemberID);
                    //    this.PlaceSurvey.Controls.Add(ctl);
                    //}
                }

            }
        }

        /// <summary>
        /// 重写LoadControl，带参数。
        /// </summary>
        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }
    }
}