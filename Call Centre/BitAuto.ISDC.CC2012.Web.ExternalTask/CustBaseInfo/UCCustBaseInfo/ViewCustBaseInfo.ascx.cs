using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using BitAuto.YanFa.SysRightManager.Common;

namespace BitAuto.ISDC.CC2012.Web.ExternalTask.CustBaseInfo.UCCustBaseInfo
{
    public partial class ViewCustBaseInfo : System.Web.UI.UserControl
    {
        private string custId;
        public string CustID
        {
            get
            {
                return custId;
            }
            set
            {
                custId = value;
            }
        }
        public string CustName;
        public string Sex;
        public string Address;
        public string PlaceStr;
        public string Tels;
        public string Email;
        public string DataSourceStr;
        public string CustCategoryStr;
        public string AreaStr;
        public string CreateTime;
        public string CreateUserName;
        public string ModifyTime;
        public string ModifyUserName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustBaseInfoBind();
            }
        }
        private void CustBaseInfoBind()
        {
            Entities.CustBasicInfo custBasicInfo = BLL.CustBasicInfo.Instance.GetCustBasicInfo(CustID);
            if (custBasicInfo != null)
            {
                Control ctl;
                if (custBasicInfo.CustCategoryID == 3)
                {
                    ctl = this.LoadControl("~/CustCategory/DealerInfoView.ascx", CustID);
                }
                else
                {
                    ctl = this.LoadControl("~/CustCategory/BuyCarInfoView.ascx", CustID);
                }
                this.PlaceHolderCustCategory.Controls.Add(ctl);

                CustName = custBasicInfo.CustName;
                if (custBasicInfo.Sex == 1)
                {
                    Sex = "男";
                }
                else if (custBasicInfo.Sex == 2)
                {
                    Sex = "女";
                }
                Address = custBasicInfo.Address;
                if (custBasicInfo.ProvinceID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    PlaceStr += BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(custBasicInfo.ProvinceID.ToString());
                }
                if (custBasicInfo.CityID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    PlaceStr += "," + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(custBasicInfo.CityID.ToString());
                }
                if (custBasicInfo.CountyID != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    PlaceStr += "," + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(custBasicInfo.CountyID.ToString());
                }
                AreaStr = BLL.Util.GetEnumOptText(typeof(Entities.EnumArea), (int)custBasicInfo.AreaID);
                DataSourceStr = BLL.Util.GetEnumOptText(typeof(Entities.EnumDataSource), (int)custBasicInfo.DataSource);
                CreateTime = custBasicInfo.CreateTime.ToString();
                CreateUserName = UserInfo.GerTrueName((int)custBasicInfo.CreateUserID);
                if (custBasicInfo.ModifyTime != Entities.Constants.Constant.DATE_INVALID_VALUE)
                {
                    ModifyTime = custBasicInfo.ModifyTime.ToString();
                }
                ModifyUserName = UserInfo.GerTrueName((int)custBasicInfo.ModifyUserID);
                switch ((int)custBasicInfo.CustCategoryID)
                {
                    case 1:
                        CustCategoryStr = "已购车";
                        break;
                    case 2:
                        CustCategoryStr = "未购车";
                        break;
                    case 3:
                        CustCategoryStr = "经销商";
                        break;
                }
                DataTable dtTel = BLL.CustTel.Instance.GetCustTel(CustID);
                for (int i = 0; i < dtTel.Rows.Count; i++)
                {
                    Tels += dtTel.Rows[i]["Tel"].ToString() + ",";
                }
                if (Tels != null && Tels.Length > 0)
                {
                    Tels = Tels.Substring(0, Tels.Length - 1);
                }
                DataTable dtEmail = BLL.CustEmail.Instance.GetCustEmail(CustID);
                for (int i = 0; i < dtEmail.Rows.Count; i++)
                {
                    if (dtEmail.Rows[i]["Email"].ToString() != "")
                    {
                        Email += dtEmail.Rows[i]["Email"].ToString() + ",";
                    }
                }

                if (Email != null && Email.Length > 0)
                {
                    Email = Email.Substring(0, Email.Length - 1);
                }
            }
        }
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