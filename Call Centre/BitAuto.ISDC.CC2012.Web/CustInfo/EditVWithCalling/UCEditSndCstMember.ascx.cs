using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Collections;
using BitAuto.YanFa.DMSInterface;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class UCEditSndCstMember : System.Web.UI.UserControl
    {
        #region internal

        public int TaskID = -1;

        public BitAuto.YanFa.Crm2009.Entities.CstMember Member = null;
        public int MemberID = -1;
        string OriginalCSTMemberID = "";
        public string ControlName = "车商通会员";
        public string Lat = "", Lng = "";


        //页面初始化时的地区
        public string InitialProvinceID = "";
        public string InitialCityID = "";
        public string InitialCountyID = "";
        #endregion

        public UCEditSndCstMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        /// <summary>
        /// 新的会员
        /// </summary>
        public UCEditSndCstMember(int taskId)
            : this()
        {
            this.TaskID = taskId;
        }

        /// <summary>
        /// 已有的会员
        /// </summary>
        public UCEditSndCstMember(BitAuto.YanFa.Crm2009.Entities.CstMember member)
            : this()
        {
            this.Member = member;
            //this.TaskID = (int)member.TID;
            this.MemberID = member.ID;
            if (!string.IsNullOrEmpty(member.VendorCode))
            {

            }

            this.OriginalCSTMemberID = string.IsNullOrEmpty(member.CSTRecID) ? "" : member.CSTRecID;
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
                //客户类型


                string memberId = string.Empty;
                string originalDMSMemberID = string.Empty;
                if (this.Member != null)
                {
                    #region 车商通会员基本信息
                    originalDMSMemberID = MemberID.ToString();
                    memberId = Member.ID.ToString();
                    this.tfCstMemberID.Value = Member.ID.ToString();
                    this.tfRecID.Value = Member.CSTRecID.ToString();
                    this.tfCstMemberFullName.Value = Member.FullName;
                    this.tfCstMemberShortName.Value = Member.ShortName;
                    this.tfCstMemberPostCode.Value = Member.PostCode;
                    this.selCstMemberType.Value = Member.VendorClass.ToString();
                    //this.tfTrafficInfo.Value = Member.TrafficInfo;
                    //this.tfVendorCode.Value = Member.VendorCode;
                    this.tfCstMemberAddress.Value = Member.Address;
                    this.sltSuperId.Value = Member.SuperId.ToString();
                    this.selCstMemberType.Value = Member.VendorClass.ToString();
                    DataTable dt = BitAuto.YanFa.Crm2009.BLL.CSTMember_Brand.Instance.Get_CSTMember_MainBrand(Member.CSTRecID);
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        this.tfCstMemberBrand.Value += dr["BrandId"].ToString();
                        if (i != dt.Rows.Count - 1)
                        {
                            this.tfCstMemberBrand.Value += ",";
                        }
                    }
                    if (this.tfCstMemberBrand.Value.Length > 0)
                    {
                        this.tfCstMemberBrandName.Value = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandNames(this.tfCstMemberBrand.Value);
                    }

                    this.InitialProvinceID = Member.ProvinceID;
                    this.InitialCityID = Member.CityID;
                    this.InitialCountyID = Member.CountyID;
                    #endregion

                    #region 车商通会员联系人信息
                    BitAuto.YanFa.Crm2009.Entities.CSTLinkMan linkManInfo = BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.GetModelByCSTRecID(Member.CSTRecID);
                    if (linkManInfo != null)
                    {
                        //this.tfLinkManDepartment.Value = linkManInfo.Department;
                        this.tfLinkManEmail.Value = linkManInfo.Email;
                        this.tfLinkManMobile.Value = linkManInfo.Mobile;
                        //this.tfLinkManPosition.Value = linkManInfo.Position;
                        this.tfLinkManName.Value = linkManInfo.Name;
                    }
                    #endregion

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
                if (dt != null && dt.Rows.Count > 0)
                {
                    Cache.Insert("SuperVerdor", dt, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
                }
            }
            //DataTable dt = BitAuto.YanFa.DMSInterface.CstMemberServiceHandler.GetSuperVendor(out msg);
            if (dt != null && dt.Rows.Count > 0)
            {
                sltSuperId.DataSource = dt;
                sltSuperId.DataTextField = "Name";
                sltSuperId.DataValueField = "DVTID";
                sltSuperId.DataBind();
                sltSuperId.Items.Insert(0, new ListItem("请选择", "-1"));
            }
        }

        private void BindContrats()
        {
            int ID = BLL.ProjectTask_CSTMember.Instance.GetIDByCSTRecID(Member.CSTRecID);
            string TID = BLL.ProjectTask_CSTMember.Instance.GetIDByCSTTID(ID);
            Entities.QueryProjectTask_Cust_Contact queryContract = new Entities.QueryProjectTask_Cust_Contact();
            queryContract.PTID = TID;
            int total = 0;
            DataTable dt = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(queryContract, "", 1, 1000, out total);
            sltCstLinkMan.DataSource = dt;
            sltCstLinkMan.DataTextField = "CName";
            sltCstLinkMan.DataValueField = "ID";
            sltCstLinkMan.DataBind();
            sltCstLinkMan.Items.Insert(0, new ListItem("请选择", "-1"));
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