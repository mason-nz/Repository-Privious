using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder
{
    public partial class EditCustBaseInfo : System.Web.UI.UserControl
    {
        public string RequstTaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    return Request["TaskID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public string ProvinceID;
        public string CityID;
        public string CountyID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlAreaBind();
                //ddlDataSourceBind();
                if (!string.IsNullOrEmpty(RequstTaskID))
                {
                    //判断数据来源
                    Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(RequstTaskID));
                    if (model != null)
                    {
                        //未购车,试驾
                        if (model.Source == 1 || model.Source == 3)
                        {
                            this.rdoNoCar.Checked = true;
                            Entities.OrderNewCar custBasicInfo = BLL.OrderNewCar.Instance.GetOrderNewCar(Convert.ToInt32(RequstTaskID));
                            if (custBasicInfo != null)
                            {

                                this.txtCustName.Value = custBasicInfo.UserName;
                                this.txtAddress.Value = custBasicInfo.UserAddress;
                                ProvinceID = custBasicInfo.ProvinceID.ToString();
                                CityID = custBasicInfo.CityID.ToString();
                                CountyID = custBasicInfo.CountyID.ToString();
                                //ddlDataSource.Value = "易湃";
                                if (custBasicInfo.UserGender == 1)
                                {
                                    rdoMan.Checked = true;
                                }
                                else
                                {
                                    rdoWomen.Checked = true;
                                }
                                ddlArea.Value = custBasicInfo.AreaID.ToString();
                                txtTel1.Value = custBasicInfo.UserPhone;
                                txtTel2.Value = custBasicInfo.UserMobile;
                                txtEmail.Value = custBasicInfo.UserMail;
                            }
                        }
                        //已购车
                        else
                        {
                            this.rdoHavCar.Checked = true;
                            Entities.OrderRelpaceCar custBasicInfo = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(Convert.ToInt32(RequstTaskID));
                            if (custBasicInfo != null)
                            {

                                this.txtCustName.Value = custBasicInfo.UserName;
                                this.txtAddress.Value = custBasicInfo.UserAddress;
                                ProvinceID = custBasicInfo.ProvinceID.ToString();
                                CityID = custBasicInfo.CityID.ToString();
                                CountyID = custBasicInfo.CountyID.ToString();
                                //ddlDataSource.Value = "易湃";
                                if (custBasicInfo.UserGender == 1)
                                {
                                    rdoMan.Checked = true;
                                }
                                else
                                {
                                    rdoWomen.Checked = true;
                                }
                                ddlArea.Value = custBasicInfo.AreaID.ToString();
                                txtTel1.Value = custBasicInfo.UserPhone;
                                txtTel2.Value = custBasicInfo.UserMobile;
                                txtEmail.Value = custBasicInfo.UserMail;
                            }

                        }
                        ///更具已购车未购车表确定Type
                        Entities.OrderBuyCarInfo modelOrderbuyCar = null;
                        modelOrderbuyCar = BLL.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(Convert.ToInt32(RequstTaskID));
                        if (modelOrderbuyCar != null)
                        {
                            if (modelOrderbuyCar.Type == 2)
                            {
                                this.rdoNoCar.Checked = true;
                                this.rdoHavCar.Checked = false;
                            }
                            else
                            {
                                this.rdoHavCar.Checked = true;
                                this.rdoNoCar.Checked = false;

                            }
                        }


                    }
                }
            }
        }

        private void ddlAreaBind()
        {
            ddlArea.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.EnumArea));
            ddlArea.DataTextField = "name";
            ddlArea.DataValueField = "value";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("请选择", "0"));
        }
        //private void ddlDataSourceBind()
        //{
        //    ddlDataSource.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.EnumDataSource));
        //    ddlDataSource.DataTextField = "name";
        //    ddlDataSource.DataValueField = "value";
        //    ddlDataSource.DataBind();
        //    ddlDataSource.Items.Insert(0, new ListItem("请选择", "0"));
        //}


    }
}