using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using BitAuto.ISDC.CC2012.WebService;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class UCEditMember : System.Web.UI.UserControl
    {
        #region internal

        public string TaskID = string.Empty;

        public Entities.ProjectTask_DMSMember Member = null;
        public bool IsShowSurvey = true;//是否显示问卷调查模块
        public int MemberID = -1;
        public string OriginalDMSMemberID = "";
        public string SyncStatus = "";

        public string ControlName = "新易湃会员";
        public string Lat = "", Lng = "";

        //页面初始化时的地区
        public string InitialProvinceID = "";
        public string InitialCityID = "";
        public string InitialCountyID = "";

        #endregion

        public UCEditMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        /// <summary>
        /// 新的会员
        /// </summary>
        public UCEditMember(string taskId)
            : this()
        {
            this.TaskID = taskId;
        }

        //public bool isDisabled = true;

        /// <summary>
        /// 已有的会员
        /// </summary>
        public UCEditMember(Entities.ProjectTask_DMSMember member)
            : this()
        {
            this.Member = member;
            this.TaskID = member.PTID;
            this.MemberID = member.MemberID;
            if (!string.IsNullOrEmpty(member.MemberCode))
            {
                spanMemberID.InnerText = member.MemberCode;
                liMemberID.Style.Remove("display");
                if (member.SyncStatus != null)
                {
                    spanMemberSyncStatus.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(member.SyncStatus.Value);
                    liMemberSyncStatus.Style.Remove("display");
                }
                liMemberCooperated.Style.Remove("display");
                li400.Style.Remove("display");
            }
            
            if (string.IsNullOrEmpty(member.OriginalDMSMemberID) == false &&
                member.SyncStatus != (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected)//会员状态!=打回
            {
                selMemberType.Attributes.Add("disabled", "disabled");//不能编辑会员类型 
                //isDisabled = false;

            }
            SyncStatus = member.SyncStatus.ToString();
            this.tfMemberID.Value = member.MemberID.ToString();
            this.OriginalDMSMemberID = string.IsNullOrEmpty(member.OriginalDMSMemberID) ? "" : member.OriginalDMSMemberID;
            this.ControlName = "易湃会员(" + member.Name + ")";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string memberId = string.Empty;
                string originalDMSMemberID = string.Empty;
                if (this.Member != null)
                {
                    originalDMSMemberID = MemberID.ToString();
                    memberId = Member.MemberID.ToString();

                    this.tfMemberName.Value = Member.Name;
                    this.tfMemberAbbr.Value = Member.Abbr;
                    string[] splitPhone = BLL.Util.GetSplitStrArray(Member.Phone);
                    if ((!string.IsNullOrEmpty(Member.Phone)) && splitPhone.Length > 1)
                    {
                        for (int i = 0; i < splitPhone.Length && i < 3; i++)
                        {
                            (this.FindControl("tfPhone" + (i + 1)) as HtmlInputText).Value = splitPhone[i];
                        }
                    }
                    else
                    {
                        this.tfPhone1.Value = Member.Phone;
                    }
                    this.tfBrand.Value = Member.BrandIDs;
                    this.tfBrandName.Value = Member.BrandNames;

                    this.InitialProvinceID = Member.ProvinceID;
                    this.InitialCityID = Member.CityID;
                    this.InitialCountyID = Member.CountyID;

                    this.tfAddress.Value = Member.ContactAddress;
                    this.Lat = Member.Lantitude;
                    this.Lng = Member.Longitude;

                    string[] splitFax = BLL.Util.GetSplitStrArray(Member.Fax);
                    if ((!string.IsNullOrEmpty(Member.Fax)) && splitFax.Length > 0)
                    {
                        for (int i = 0; i < splitFax.Length && i < 2; i++)
                        {
                            (this.FindControl("tfFax" + (i + 1)) as HtmlInputText).Value = splitFax[i];
                        }
                    }
                    else
                    {
                        this.tfFax1.Value = Member.Fax;
                    }

                    this.tfCompanyWebSite.Value = Member.CompanyWebSite;
                    this.tfEmail.Value = Member.Email;
                    this.tfZipcode.Value = Member.Postcode;

                    this.txtSerialIds.Value = Member.SerialIds;
                    this.txtSerial.Value = Member.SerialNames;

                    this.tfTrafficInfo.Value = Member.TrafficInfo;
                    this.tfEnterpriseBrief.Value = Member.EnterpriseBrief;
                    this.tfNotes.Value = Member.Remarks;

                    this.selMemberType.Value = Member.MemberType;

                    if (!string.IsNullOrEmpty(Member.OriginalDMSMemberID))
                    {
                        originalDMSMemberID = Member.OriginalDMSMemberID;
                        BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(Member.OriginalDMSMemberID));
                        if (DMSMember != null)
                        {
                            if (DMSMember.Cooperated == 1)
                            {
                                string temp = "";
                                //易湃会员
                                DataTable dtCYT = BitAuto.YanFa.Crm2009.BLL.CYTMember.Instance.GetCYTMemberLastByMemberCode(DMSMember.MemberCode, -1);
                                if (dtCYT != null && dtCYT.Rows.Count > 0)
                                {
                                    dtCYT.DefaultView.Sort = "createtime Desc";
                                    foreach (DataRow item in dtCYT.DefaultView.ToTable().Rows)
                                    {
                                        temp += (Convert.ToDateTime(item["begintime"]).ToString("yyyy-MM-dd")
                                             + "至" + Convert.ToDateTime(item["endtime"]).ToString("yyyy-MM-dd") + "<br/>");
                                    }
                                    temp = temp.Substring(0, temp.LastIndexOf("<br/>") > 0 ? temp.LastIndexOf("<br/>") : 0);
                                }
                                this.spanMemberCooperated.InnerHtml = temp;
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
                }
                //if (IsShowSurvey)
                //{
                //    Control ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditSurvey.ascx", this.TaskID, originalDMSMemberID, memberId);
                //    this.PlaceSurvey.Controls.Add(ctl);
                //}
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
            System.Reflection.ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

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