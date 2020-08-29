using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class UCCust : System.Web.UI.UserControl
    {
        private BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo;
        /// <summary>
        /// 客户信息
        /// </summary>
        public BitAuto.YanFa.Crm2009.Entities.CustInfo CustInfo { get { return custInfo; } set { custInfo = value; } }

        public string DeleteMemberID = string.Empty;

        public string CustID = "";
        public int CarType = -1;

        public string OfficeTel = "";
        public string ContactName = "";
        public string FirstMemberCode = "";
        public string FirstMemberName = "";

        public bool ListOfCooperationProjects = true;
        public bool ListOfCustUser = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (this.CustInfo == null) { return; }

                this.CustID = this.CustInfo.CustID;

                BindCustomerInfo();
                BindMemberList(this.CustInfo.CustID);
                SurveyListID.RequestCustID = this.CustID;
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
            ContactName = this.CustInfo.contactName;
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
            OfficeTel = this.CustInfo.Officetel;
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
            //正常的会员
            List<BitAuto.YanFa.Crm2009.Entities.DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(custID);
            if (list != null && list.Count > 0)
            {
                FirstMemberCode = list[0].MemberCode;
                FirstMemberName = list[0].Name;
            }

            foreach (BitAuto.YanFa.Crm2009.Entities.DMSMember m in list)
            {
                Control ctl = this.LoadControl("~/CRMStopCust/UCMember.ascx", m);
                this.PlaceHolder.Controls.Add(ctl);
            }

            //被停用删除的会员 易湃 
            string[] deleteMember = DeleteMemberID.Split(';');

            if (deleteMember.Length > 0)
            {
                string[] members = deleteMember[0].Split(',');

                for (int k = 0; k < members.Length; k++)
                {
                    if (members[k] != "")
                    {
                        BitAuto.YanFa.Crm2009.Entities.DMSMember m = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.CCGetDMSMember(members[k], "-1");

                        if (m != null)
                        {
                            Control ctl = this.LoadControl("~/CRMStopCust/UCMember.ascx", m);
                            this.PlaceHolder.Controls.Add(ctl);
                        }
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