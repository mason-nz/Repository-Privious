using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder
{
    public partial class UCOrderConsult : System.Web.UI.UserControl
    {

        #region 参数

        private string tid;
        /// <summary>
        ///  任务ID
        /// </summary>
        public string TID
        {
            get
            {
                return tid;
            }
            set
            {
                tid = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Source = "";

        /// <summary>
        /// 经销商ID
        /// </summary>
        public int DealerId = 0;

        /// <summary>
        /// 经销商Name
        /// </summary>
        public string DealerName = "";
        /// <summary>
        /// 经销商Name
        /// </summary>
        public string MemberID = "";
        /// <summary>
        /// 经销商Name
        /// </summary>
        public string CustID = "";


        #region 各个品牌、车型控件ID

        public string ddlNewBrand = "0";
        public string ddlNewSerial = "0";
        public string ddlNewName = "0";

        public string ddlWantBrand = "0";
        public string ddlWantSerial = "0";
        public string ddlWantName = "0";

        public string dllOldBrand = "0";
        public string dllOldSerial = "0";
        public string dllOldName = "0";

        public string ddlCarTypeID = "0";
        public string ddlRecCarTypeID = "0";
        public string nowColor = string.Empty;
        public string relColor = string.Empty;
        #endregion

        #region 省份、城市、区县ID

        public string Province = "-1";
        public string City = "-1";
        public string County = "-1";

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindCarColor();
                BindYearAndMonth();
                BindInfo();
                //bindColorData();//Modify=Masj Date=2013-08-01 由于在后台，通过接口绑定车身颜色相应时间较慢，先改完前端异步加载方式。
            }
        }

        private void BindYearAndMonth()
        {
            #region 绑定年份

            DateTime nowDt = DateTime.Now;
            for (int i = 0; i <= 30; i++)
            {
                dllRegDateYear.Items.Add(new ListItem(nowDt.AddYears(i * -1).Year.ToString() + "年", nowDt.AddYears(i * -1).Year.ToString()));
            }
            dllRegDateYear.Items.Insert(0, new ListItem("请选择年份", "-1"));

            #endregion

            #region 绑定月份

            for (int i = 1; i <= 12; i++)
            {
                dllRegDateMonth.Items.Add(new ListItem(i.ToString() + "月", i.ToString()));
            }
            dllRegDateMonth.Items.Insert(0, new ListItem("请选择月份", "-1"));

            #endregion

        }

        private void BindInfo()
        {
            #region 根据类型绑定联系记录控件
            //1新车，3试驾modify by qizq 2013-7-19
            if (Source == "1" || Source == "3")
            {
                //新车订单
                Entities.OrderNewCar newCarModel = BLL.OrderNewCar.Instance.GetOrderNewCar(long.Parse(TID));
                if (newCarModel != null)
                {
                    #region 绑定新车订单

                    this.ddlNewBrand = newCarModel.CarMasterID.ToString();
                    this.ddlNewSerial = newCarModel.CarSerialID.ToString();
                    this.ddlNewName = newCarModel.CarTypeID.ToString();
                    ///this.ddlNewCarColor.Value = newCarModel.CarColor;
                    this.ddlCarTypeID = newCarModel.CarTypeID.ToString();
                    nowColor = newCarModel.CarColor;

                    //经销商
                    this.hidNewCarDMSmemberCode.Value = newCarModel.DMSMemberCode;
                    //名称
                    this.txtNewDealer.Value = newCarModel.DMSMemberName;

                    this.labNewsRemark.InnerText = newCarModel.OrderRemark;
                    this.labNewCreatTime.InnerText = DateTime.Parse(newCarModel.OrderCreateTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    this.txtNewCallRecord.Value = newCarModel.CallRecord;
                    //经销商ｉＤ
                    if (newCarModel.DealerID != Constant.INT_INVALID_VALUE)
                    {
                        if (int.TryParse(newCarModel.DealerID.ToString(), out DealerId))
                        {
                        }
                    }
                    #region 隐藏域

                    this.hidDMSCode.Value = newCarModel.DMSMemberCode;
                    this.hidDMSName.Value = newCarModel.DMSMemberName;
                    //this.hidDMSLevel.Value = newCarModel.hidDMSLevel;
                    //this.hidDMSAddress.Value = newCarModel.hidDMSAddress;
                    //this.hidDMSTel.Value = newCarModel.hidDMSTel;
                    //this.hidDMSCity.Value = newCarModel.hidDMSCity;

                    #endregion

                    #endregion

                }
            }
            else if (Source == "2")
            {
                //置换订单
                Entities.OrderRelpaceCar replaceCarModel = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(long.Parse(TID));
                if (replaceCarModel != null)
                {
                    #region 绑定置换订单信息

                    this.ddlWantBrand = replaceCarModel.CarMasterID.ToString();
                    this.ddlWantSerial = replaceCarModel.CarSerialID.ToString();
                    this.ddlWantName = replaceCarModel.CarTypeID.ToString();
                    this.ddlCarTypeID = replaceCarModel.RepCarTypeId.ToString();
                    this.ddlRecCarTypeID = replaceCarModel.CarTypeID.ToString();
                    nowColor = replaceCarModel.ReplacementCarColor;
                    relColor = replaceCarModel.CarColor;

                    //经销商
                    this.hidWantDMSMemberCode.Value = replaceCarModel.DMSMemberCode;
                    // //名称
                    this.txtWantDealer.Value = replaceCarModel.DMSMemberName;

                    this.dllOldBrand = replaceCarModel.RepCarMasterID.ToString();
                    this.dllOldSerial = replaceCarModel.RepCarSerialID.ToString();
                    this.dllOldName = replaceCarModel.RepCarTypeId.ToString();

                    this.dllOldColor.Value = replaceCarModel.ReplacementCarColor;
                    this.dllRegDateYear.Value = replaceCarModel.ReplacementCarBuyYear.ToString();
                    this.dllRegDateMonth.Value = replaceCarModel.ReplacementCarBuyMonth.ToString();
                    //省份

                    Province = replaceCarModel.RepCarProvinceID.ToString();
                    City = replaceCarModel.RepCarCityID.ToString();
                    County = replaceCarModel.RepCarCountyID.ToString();

                    this.txtMileage.Value = replaceCarModel.ReplacementCarUsedMiles.ToString();
                    this.txtProSellPrice.Value = replaceCarModel.SalePrice.ToString();
                    this.labWantRemark.InnerText = replaceCarModel.OrderRemark;
                    this.labWantCreateTime.InnerText = DateTime.Parse(replaceCarModel.OrderCreateTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    this.txtReplaceCallRecord.Value = replaceCarModel.CallRecord;

                    this.hidDMSCode.Value = replaceCarModel.DMSMemberCode;
                    this.hidDMSName.Value = replaceCarModel.DMSMemberName;

                    //经销商ｉＤ
                    if (replaceCarModel.DealerID != Constant.INT_INVALID_VALUE)
                    {
                        if (int.TryParse(replaceCarModel.DealerID.ToString(), out DealerId))
                        {
                        }
                    }

                    #endregion
                }
            }
            //根据经销商id，取名称
            if (DealerId != 0)
            {
                BitAuto.YanFa.Crm2009.Entities.DMSMember DMSModel = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(DealerId.ToString());
                if (DMSModel != null)
                {
                    DealerName = DMSModel.Name;
                    MemberID = DMSModel.ID.ToString();
                    CustID = DMSModel.CustID;
                }
            }

            #endregion
        }

        private void BindCarColor()
        {
            //CarColor
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.CarColor));
            ddlNewCarColor.DataValueField = "name";
            ddlNewCarColor.DataTextField = "name";
            ddlNewCarColor.DataSource = dt;
            ddlNewCarColor.DataBind();

            ddlWantColor.DataValueField = "name";
            ddlWantColor.DataTextField = "name";
            ddlWantColor.DataSource = dt;
            ddlWantColor.DataBind();

            dllOldColor.DataValueField = "name";
            dllOldColor.DataTextField = "name";
            dllOldColor.DataSource = dt;
            dllOldColor.DataBind();

        }

        //颜色列表
        public void bindColorData()
        {
            if (ddlCarTypeID != "0")
            {
                DataTable dt = BLL.CarTypeAPI.Instance.GetCarColorByCarTypeID(int.Parse(ddlCarTypeID));
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dllOldColor.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Name"].ToString()));
                        ddlNewCarColor.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Name"].ToString()));
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ListItem oldColor = dllOldColor.Items.FindByText(nowColor);
                        if (oldColor != null)
                        {
                            oldColor.Selected = true;
                        }
                        ListItem newColor = ddlNewCarColor.Items.FindByText(nowColor);
                        if (newColor != null)
                        {
                            newColor.Selected = true;
                        }
                        ddlNewCarColor.Items.Insert(0, new ListItem("请选择颜色", "-1"));
                    }
                    else
                    {
                        dllOldColor.Items.Insert(0, nowColor);
                    }
                }
                else
                {
                    dllOldColor.Items.Insert(0, nowColor);
                }
            }
            else
            {
                dllOldColor.Items.Insert(0, nowColor);
            }

            if (ddlRecCarTypeID != "0")
            {
                DataTable dt = BLL.CarTypeAPI.Instance.GetCarColorByCarTypeID(int.Parse(ddlRecCarTypeID));
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddlWantColor.Items.Add(new ListItem(dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Name"].ToString()));
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ListItem wantColor = ddlWantColor.Items.FindByText(relColor);
                        if (wantColor != null)
                        {
                            wantColor.Selected = true;
                        }
                    }
                    else
                    {
                        ddlWantColor.Items.Insert(0, relColor);
                    }
                }
                else
                {
                    ddlWantColor.Items.Insert(0, relColor);
                }
            }
            else
            {
                ddlWantColor.Items.Insert(0, relColor);
            }

        }
    }
}