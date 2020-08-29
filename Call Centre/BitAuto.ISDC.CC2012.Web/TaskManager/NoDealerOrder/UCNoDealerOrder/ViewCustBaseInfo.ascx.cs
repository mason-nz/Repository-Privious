using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder
{
    public partial class ViewCustBaseInfo : System.Web.UI.UserControl
    {
        private int Transfer = 0;
        public string RequstTaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    if (int.TryParse(Request["TaskID"], out Transfer))
                    {
                        return Request["TaskID"].ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CustName;
        public string Sex;
        public string Address = string.Empty;
        public string PlaceStr = string.Empty;
        public string Tels = string.Empty;
        public string Email = string.Empty;
        public string DataSourceStr = string.Empty;
        public string CustCategoryStr = string.Empty;
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
            if (!string.IsNullOrEmpty(RequstTaskID))
            {
                //判断数据来源
                Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(RequstTaskID));
                if (model != null)
                {
                    //未购车，试驾
                    if (model.Source == 1 || model.Source == 3)
                    {
                        Entities.OrderNewCar custBasicInfo = BLL.OrderNewCar.Instance.GetOrderNewCar(Convert.ToInt32(RequstTaskID));
                        if (custBasicInfo != null)
                        {
                            CustName = custBasicInfo.UserName;
                            if (custBasicInfo.UserGender == 1)
                            {
                                Sex = "先生";
                            }
                            else if (custBasicInfo.UserGender == 2)
                            {
                                Sex = "女士";
                            }
                            Address = custBasicInfo.UserAddress;
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
                            CustCategoryStr = "未购车";
                            if (!string.IsNullOrEmpty(custBasicInfo.UserMobile) && !string.IsNullOrEmpty(custBasicInfo.UserPhone))
                            {
                                Tels = custBasicInfo.UserPhone + "," + custBasicInfo.UserMobile;
                            }
                            else if (!string.IsNullOrEmpty(custBasicInfo.UserMobile))
                            {
                                Tels = custBasicInfo.UserMobile;
                            }
                            else
                            {
                                Tels = custBasicInfo.UserPhone;
                            }


                            Email = custBasicInfo.UserMail;
                        }
                    }
                    //已购车
                    else
                    {
                        Entities.OrderRelpaceCar custBasicInfo = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(Convert.ToInt32(RequstTaskID));
                        if (custBasicInfo != null)
                        {
                            CustName = custBasicInfo.UserName;
                            if (custBasicInfo.UserGender == 1)
                            {
                                Sex = "先生";
                            }
                            else if (custBasicInfo.UserGender == 2)
                            {
                                Sex = "女士";
                            }
                            Address = custBasicInfo.UserAddress;
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
                            CustCategoryStr = "已购车";
                            if (!string.IsNullOrEmpty(custBasicInfo.UserMobile) && !string.IsNullOrEmpty(custBasicInfo.UserPhone))
                            {
                                Tels = custBasicInfo.UserPhone + "," + custBasicInfo.UserMobile;
                            }
                            else if (!string.IsNullOrEmpty(custBasicInfo.UserMobile))
                            {
                                Tels = custBasicInfo.UserMobile;
                            }
                            else
                            {
                                Tels = custBasicInfo.UserPhone;
                            }
                            Email = custBasicInfo.UserMail;
                        }

                    }

                    ///更具已购车未购车表确定Type
                    Entities.OrderBuyCarInfo modelOrderbuyCar = null;
                    modelOrderbuyCar = BLL.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(Convert.ToInt32(RequstTaskID));
                    if (modelOrderbuyCar != null)
                    {
                        if (modelOrderbuyCar.Type == 2)
                        {
                            CustCategoryStr = "未购车";
                        }
                        else
                        {
                            CustCategoryStr = "已购车";
                        }
                    }
                }
            }
        }
        //private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        //{
        //    List<Type> constParamTypes = new List<Type>();
        //    foreach (object constParam in constructorParameters)
        //    {
        //        constParamTypes.Add(constParam.GetType());
        //    }

        //    UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

        //    // Find the relevant constructor
        //    ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

        //    //And then call the relevant constructor
        //    if (constructor == null)
        //    {
        //        throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
        //    }
        //    else
        //    {
        //        constructor.Invoke(ctl, constructorParameters);
        //    }

        //    // Finally return the fully initialized UC
        //    return ctl;
        //}
    }
}