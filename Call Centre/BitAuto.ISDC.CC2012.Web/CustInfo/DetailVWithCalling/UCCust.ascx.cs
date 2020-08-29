using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.YanFa.Crm2009;
using System.Data;
using System.Reflection;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailVWithCalling
{
    public partial class UCCust : System.Web.UI.UserControl
    {
        private BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo;
        /// <summary>
        /// 客户信息
        /// </summary>
        public BitAuto.YanFa.Crm2009.Entities.CustInfo CustInfo { get { return custInfo; } set { custInfo = value; } }

        public string AddWorderv2Url = "";
        public string AddWorderv2Url_Phone = "";

        #region internal

        public string CustID = "";
        public int CarType = -1;

        #endregion

        public bool ListOfCooperationProjects = true;
        public bool ListOfCustUser = true;

        public bool ListOfReturnVisit = true;
        public bool ListOfBusinessLicense = true;
        public bool ListOfBusinessBrandLicense = true;
        public bool ListOfBusinessScaleInfo = true;
        public bool ListOfTaskRecord = true;
        public bool ListOfWorkOrder = true;
        //控制添回回访按钮
        public bool AddReturnVistCustButton = false;

        public string FirstMemberCode = "";
        public string FirstMemberName = "";

        protected string officeTel = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.CustInfo == null) { return; }
                this.CustID = this.CustInfo.CustID;
                //查询第一个经销商
                List<BitAuto.YanFa.Crm2009.Entities.DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(CustID);
                if (list != null && list.Count > 0)
                {
                    FirstMemberCode = list[0].MemberCode;
                    FirstMemberName = list[0].Name;
                }
                AddWorderv2Url = "/WOrderV2/AddWOrderInfo.aspx?" + BLL.WOrderRequest.AddWOrderComeIn_CRMCustID(CustID).ToString();

                if (this.CustInfo.Officetel == "")
                {
                    AddWorderv2Url_Phone = "href='javascript:void(0)' ";
                }
                else
                {
                    AddWorderv2Url_Phone = "href='/WOrderV2/AddWOrderInfo.aspx?" + BLL.WOrderRequest.AddWOrderComeIn_CallOut(this.CustInfo.Officetel, this.CustInfo.CustID, this.CustInfo.contactName, -1, FirstMemberCode, -1).ToString() + "' ";
                    AddWorderv2Url_Phone += "target='_blank'";
                }

                BindCustomerInfo();
                BindMemberList(this.CustInfo.CustID);
                SurveyListID.RequestCustID = this.CustID;
                string sysId = ConfigurationUtil.GetAppSettingValue("ThisSysID");
                int userId = BLL.Util.GetLoginUserID();
                AddReturnVistCustButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT1410", sysId, userId);
            }
        }

        private void BindCustomerInfo()
        {
            CarType = this.CustInfo.CarType;
            spanCustName.InnerText = this.CustInfo.CustName;
            spanCustAbbr.InnerText = this.CustInfo.AbbrName;
            spanAddress.InnerText = this.CustInfo.Address;

            BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo queryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo();
            queryBrandInfo.CustID = this.CustInfo.CustID;
            int o;
            string s = "";
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetCustBrandInfo(queryBrandInfo, "", 1, 10000, out o);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += dt.Rows[i]["name"] + " ";
                }
            }
            spanBrandName.InnerText = s;
            spanContactName.InnerText = this.CustInfo.contactName;
            spanFax.InnerText = this.CustInfo.Fax;
            if (!string.IsNullOrEmpty(this.CustInfo.IndustryID))
            {
                spanCustIndustry.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.IndustryID));
            }
            if (!string.IsNullOrEmpty(this.CustInfo.LevelID))
            {
                spanCustLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.LevelID));
            }

            // 集团 厂商 没有 [所属集团 所属厂商]; 没有新增会员按钮
            if (this.CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Company).ToString() || this.CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc).ToString())
            {
                liPid.Visible = false;
                liCustPid.Visible = false;
            }
            else
            {
                if (this.CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SynthesizedShop).ToString())
                {
                    liCustPid.Visible = false;
                    //综合店 没有厂商 
                }
            }

            spanNotes.InnerText = this.CustInfo.Notes;
            spanOfficeTel.InnerText = this.CustInfo.Officetel;
            officeTel = this.CustInfo.Officetel;

            if (!string.IsNullOrEmpty(this.CustInfo.Pid))
            {
                BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustInfo.Pid);
                if (pmodel != null)
                {
                    spanPidName.InnerText = pmodel.CustName;
                }
            }
            if (!string.IsNullOrEmpty(this.CustInfo.CustPid))
            {
                BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustInfo.CustPid);
                if (pmodel != null)
                {
                    spanCustPidName.InnerText = pmodel.CustName;
                }
            }
            string provinceCity = "";
            if (!string.IsNullOrEmpty(this.CustInfo.ProvinceID))
            {
                provinceCity += BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.CustInfo.ProvinceID);
            }
            if (!string.IsNullOrEmpty(this.CustInfo.CityID))
            {
                provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.CustInfo.CityID);
            }
            if (!string.IsNullOrEmpty(this.CustInfo.CountyID))
            {
                provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.CustInfo.CountyID);
            }
            spanArea.InnerText = provinceCity;
            if (!string.IsNullOrEmpty(this.CustInfo.ShopLevel))
            {
                spanShopLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.ShopLevel));
            }
            if (!string.IsNullOrEmpty(this.CustInfo.TypeID))
            {
                spanCustType.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.TypeID));
            }
            spanZipcode.InnerText = this.CustInfo.zipcode;

        }

        private void BindMemberList(string custID)
        {
            List<BitAuto.YanFa.Crm2009.Entities.DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(custID);
            foreach (BitAuto.YanFa.Crm2009.Entities.DMSMember m in list)
            {
                Control ctl = this.LoadControl("~/CustInfo/DetailVWithCalling/UCMember.ascx", m);
                this.PlaceHolder.Controls.Add(ctl);
            }
        }

        private void BindCstMemberList(string custID)
        {
            List<BitAuto.YanFa.Crm2009.Entities.CstMember> list = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCSTMemberList(custID);
            if (list != null)
            {
                foreach (BitAuto.YanFa.Crm2009.Entities.CstMember m in list)
                {
                    if (m != null)
                    {
                        Control ctl = this.LoadControl("~/CustInfo/DetailV/UCCstMember.ascx", m);
                        this.PlaceHolder.Controls.Add(ctl);
                    }
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
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }
            return ctl;
        }
    }
}