using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using BitAuto.YanFa.DMSInterface;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class UCEditCstMember : System.Web.UI.UserControl
    {
        #region internal

        public string TaskID =string.Empty;
        public string typeId;
        public bool FullNameHavHistory = false;
        public Entities.ProjectTask_CSTMember Member = null;
        public bool IsShowSurvey = true;//是否显示问卷调查模块
        public int MemberID = -1;
        public string OriginalDMSMemberID = "";

        public string ControlName = "新车商通会员";
        public string Lat = "", Lng = "";

        //页面初始化时的地区
        public string InitialProvinceID = "";
        public string InitialCityID = "";
        public string InitialCountyID = "";
        public string CrmMember = "";
        #endregion

        public UCEditCstMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        /// <summary>
        /// 新的会员
        /// </summary>
        public UCEditCstMember(string taskId)
            : this()
        {
            this.TaskID = taskId;
        }

        /// <summary>
        /// 已有的会员
        /// </summary>
        public UCEditCstMember(Entities.ProjectTask_CSTMember member)
            : this()
        {
            this.Member = member;
            this.TaskID = member.PTID;
            this.MemberID = member.ID;
            if (!string.IsNullOrEmpty(member.VendorCode))
            {

            }

            this.OriginalDMSMemberID = string.IsNullOrEmpty(member.OriginalCSTRecID) ? "" : member.OriginalCSTRecID;
            this.ControlName = "车商通会员(" + member.FullName + ")";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.tfCstMemberShortName.Attributes.Add("onblur", "javascript:uCEditCstMemberHelper_" + ID + ".createMemberCode();");
                this.sltCstLinkMan.Attributes.Add("onchange", "uCEditCstMemberHelper_" + ID + ".getContactInfo()");
                BindSuperCompany();
                BindContrats();

                string memberId = string.Empty;
                string originalDMSMemberID = string.Empty;
                if (this.Member != null)
                {
                    if (!string.IsNullOrEmpty(this.Member.OriginalCSTRecID))
                    {
                        CrmMember = "Crm";
                        this.tfCstMemberAddress.Attributes.Add("disabled", "disabled");
                        this.tfCstMemberFullName.Attributes.Add("disabled", "disabled");
                        this.tfCstMemberPostCode.Attributes.Add("disabled", "disabled");
                        this.tfCstMemberShortName.Attributes.Add("disabled", "disabled");
                        this.tfLinkManEmail.Attributes.Add("disabled", "disabled");
                        this.tfLinkManMobile.Attributes.Add("disabled", "disabled");
                        //this.tfVendorCode.Attributes.Add("disabled", "disabled");
                        this.selCstMemberCity.Attributes.Add("disabled", "disabled");
                        this.selCstMemberCounty.Attributes.Add("disabled", "disabled");
                        this.selCstMemberProvince.Attributes.Add("disabled", "disabled");
                        this.sltSuperId.Attributes.Add("disabled", "disabled");
                        this.tfLinkManName.Attributes.Add("disabled", "disabled");
                        this.sltCstLinkMan.Attributes.Add("disabled", "disabled");
                    }
                    #region 车商通会员基本信息
                    originalDMSMemberID = MemberID.ToString();
                    memberId = Member.ID.ToString();
                    this.tfCstMemberID.Value = Member.ID.ToString();
                    this.tfCstMemberFullName.Value = Member.FullName;
                    this.tfCstMemberShortName.Value = Member.ShortName;
                    this.tfCstMemberPostCode.Value = Member.PostCode;
                    this.selCstMemberType.Value = Member.VendorClass.ToString();
                    //this.tfTrafficInfo.Value = Member.TrafficInfo;
                    //this.tfVendorCode.Value = Member.VendorCode;
                    this.tfCstMemberAddress.Value = Member.Address;
                    this.sltSuperId.Value = Member.SuperId.ToString();
                    this.selCstMemberType.Value = Member.VendorClass.ToString();


                    this.InitialProvinceID = Member.ProvinceID;
                    this.InitialCityID = Member.CityID;
                    this.InitialCountyID = Member.CountyID;
                    #endregion


                    string brandIds = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_BrandIDs(Member.ID);
                    this.tfCstMemberBrandName.Value = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandNames(brandIds);
                    this.tfCstMemberBrand.Value = brandIds;


                    #region 车商通会员联系人信息
                    Entities.ProjectTask_CSTLinkMan linkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkManModel(Member.ID);
                    if (linkManInfo != null)
                    {
                        //this.tfLinkManDepartment.Value = linkManInfo.Department;
                        this.tfLinkManEmail.Value = linkManInfo.Email;
                        this.tfLinkManMobile.Value = linkManInfo.Mobile;
                        //this.tfLinkManPosition.Value = linkManInfo.Position;
                        this.tfLinkManName.Value = linkManInfo.Name;
                    }
                    //如果是开通的会员，则不可编辑
                    bool isopen = false;
                    if (BLL.ProjectTask_CSTMember.Instance.IsOpenSuccessMember(Member.ID))
                    {
                        //是否有名称变更
                        if (!string.IsNullOrEmpty(Member.OriginalCSTRecID))
                        {
                            DataTable dtFullName = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(Member.OriginalCSTRecID, 7, "FullName");
                            if (dtFullName.Rows.Count > 0)
                            {
                                FullNameHavHistory = true;
                            }
                        }
                        this.sltSuperId.Disabled = true;
                        this.sltCstLinkMan.Disabled = true;
                        //this.tfLinkManDepartment.Disabled = true;
                        this.tfLinkManEmail.Disabled = true;
                        this.tfLinkManMobile.Disabled = true;
                        //this.tfLinkManPosition.Disabled = true;
                        this.tfLinkManName.Disabled = true;
                        //this.tfVendorCode.Disabled = true;
                        isopen = true;
                    }
                    #endregion

                    if (isopen)
                    {
                        Control ctl = this.LoadControl("~/CustInfo/DetailV/UCCstMemberUCount.ascx", Member.ID);

                        this.PlaceHolderCstMemberUCount.Controls.Add(ctl);
                    }
                }
            }
        }

        private void BindSuperCompany()
        {
            string msg = string.Empty;
            DataTable dt = new DataTable();
            if (Cache["SuperVerdor"] != null)
            {
                dt = (DataTable)Cache["SuperVerdor"];
            }
            else
            {
                string strMsg = string.Empty;
                dt = WebService.CstMemberServiceHelper.Instance.GetSuperVendor(out strMsg);
                Cache.Insert("SuperVerdor", dt, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
            }
            //DataTable dt = BitAuto.YanFa.DMSInterface.CstMemberServiceHandler.GetSuperVendor(out msg);
            sltSuperId.DataSource = dt;
            sltSuperId.DataTextField = "Name";
            sltSuperId.DataValueField = "DVTID";
            sltSuperId.DataBind();
            sltSuperId.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        private void BindContrats()
        {
            Entities.QueryProjectTask_Cust_Contact queryContract = new Entities.QueryProjectTask_Cust_Contact();
            queryContract.PTID = TaskID;
            int total = 0;
            DataTable dt = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(queryContract, "", 1, 1000, out total);
            sltCstLinkMan.DataSource = dt;
            sltCstLinkMan.DataTextField = "CName";
            sltCstLinkMan.DataValueField = "ID";
            sltCstLinkMan.DataBind();
            sltCstLinkMan.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        private int GetVendorClass(string type)
        {
            int itype = 5;
            switch (type)
            {
                case "20001":
                    itype = 4;
                    break;
                case "20003":
                    itype = 2;
                    break;
                case "20010":
                    itype = 1;
                    break;
                case "20011":
                    itype = 3;
                    break;
                case "20012":
                    itype = 4;
                    break;
                default:
                    itype = 5;
                    break;
            }
            return itype;
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