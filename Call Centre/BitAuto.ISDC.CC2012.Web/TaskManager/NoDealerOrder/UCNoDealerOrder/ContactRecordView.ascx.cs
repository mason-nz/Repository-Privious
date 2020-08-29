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
    public partial class ContactRecord : System.Web.UI.UserControl
    {
        #region 属性

        public string TaskID = string.Empty;
        public string Source = string.Empty;

        public string CarType = string.Empty;
        public string DMSMemberName = string.Empty;
        public string OrderRemark = string.Empty;
        public string CreateTime = string.Empty;
        public string CallRecord = string.Empty;

        public string CarColor = string.Empty;
        public string CarTypeNow = string.Empty;
        public string CarColorNow = string.Empty;
        public string CarBuyTime = string.Empty;
        public string ReplacementCarLocationID = string.Empty;
        public string ReplacementCarUsedMiles = string.Empty;
        public string CarPrice = string.Empty;

        public string OldCarType = string.Empty; //历史新车车型
        public string PlaceStr = string.Empty;//历史地区

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


        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //新车和试驾
                if (Source == "1" || Source == "3")
                {
                    bindNewCarData();
                }
                else if (Source == "2")
                {
                    bindRelpaceCarData();
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
            }
        }
        private void bindRelpaceCarData()
        {
            int _taskID;
            if (int.TryParse(TaskID, out _taskID))
            {
                Entities.OrderRelpaceCar model = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(_taskID);
                if (model != null)
                {
                    string carMaster = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(model.CarMasterID.ToString()));
                    string carSerial = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(model.CarSerialID.ToString()));
                    string carType = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(int.Parse(model.CarTypeID.ToString()));
                    CarType = carMaster + "-" + carSerial + "-" + carType;

                    string carTypeNow = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(int.Parse(model.RepCarTypeId.ToString()));
                    string carMasterNow = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(model.RepCarMasterID.ToString()));
                    string carSerialNow = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(model.RepCarSerialID.ToString()));
                    CarTypeNow = carMasterNow + "-" + carSerialNow + "-" + carTypeNow;

                    CarColorNow = model.ReplacementCarColor;
                    CarColor = model.CarColor;
                    CarBuyTime = model.ReplacementCarBuyYear.ToString() + "年" + model.ReplacementCarBuyMonth.ToString() + "月";
                    ReplacementCarLocationID = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.RepCarProvinceID.ToString());
                    ReplacementCarUsedMiles = model.ReplacementCarUsedMiles.ToString();
                    CarPrice = model.SalePrice.ToString();

                    //经销商ｉＤ
                    if (model.DealerID != Constant.INT_INVALID_VALUE)
                    {
                        if (int.TryParse(model.DealerID.ToString(), out DealerId))
                        {
                        }
                    }

                    DMSMemberName = model.DMSMemberName;
                    OrderRemark = model.OrderRemark;
                    CreateTime = model.OrderCreateTime.ToString();
                    CallRecord = model.CallRecord;
                }
            }
        }
        private void bindNewCarData()
        {
            int _taskID;
            if (int.TryParse(TaskID, out _taskID))
            {
                Entities.OrderNewCar model = BLL.OrderNewCar.Instance.GetOrderNewCar(_taskID);
                if (model != null)
                {
                    string carType = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(int.Parse(model.CarTypeID.ToString()));
                    string carMaster = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(model.CarMasterID.ToString()));
                    string carSerial = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(model.CarSerialID.ToString()));
                    CarType = carMaster + "-" + carSerial + "-" + carType;

                    #region 历史车型和地区

                    Entities.QueryOrderNewCarLog query = new Entities.QueryOrderNewCarLog();
                    query.YPOrderID = model.YPOrderID;
                    int totalCount = 0;
                    DataTable dt = BLL.OrderNewCarLog.Instance.GetOrderNewCarLog(query, "", 1, 10, out totalCount);
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        #region 历史车型

                        int CarMasterID = 0;
                        int CarSerialID = 0;
                        int brandID = 0;
                        BLL.CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(int.Parse(dt.Rows[0]["CarID"].ToString()), out CarSerialID, out CarMasterID,out brandID);

                        string oldCarType = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(int.Parse(dt.Rows[0]["CarID"].ToString()));
                        string oldCarMaster = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(CarMasterID);
                        string oldCarSerial = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(CarSerialID);
                        OldCarType = oldCarMaster + "-" + oldCarSerial + "-" + oldCarType;

                        #endregion

                        #region 历史地区

                        int ProvinceID = Entities.Constants.Constant.INT_INVALID_VALUE;
                        int CityID = Entities.Constants.Constant.INT_INVALID_VALUE;
                        int CountyID = Entities.Constants.Constant.INT_INVALID_VALUE;



                        string locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaStrByAreaID(dt.Rows[0]["LocationID"].ToString());
                        switch (locationName.Split(',').Length)
                        {
                            case 1:
                                ProvinceID = int.Parse(locationName.Split(',')[0]);
                                break;
                            case 2:
                                ProvinceID = int.Parse(locationName.Split(',')[0]);
                                CityID = int.Parse(locationName.Split(',')[1]);
                                break;
                            case 3:
                                ProvinceID = int.Parse(locationName.Split(',')[0]);
                                CityID = int.Parse(locationName.Split(',')[1]);
                                CountyID = int.Parse(locationName.Split(',')[2]);
                                break;
                            default:
                                break;
                        }


                        if (ProvinceID != Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            PlaceStr += BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProvinceID.ToString());
                        }
                        if (CityID != Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            PlaceStr += "," + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(CityID.ToString());
                        }
                        if (CountyID != Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            PlaceStr += "," + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(CountyID.ToString());
                        }

                        #endregion
                    }

                    #endregion


                    if (model.CarColor != "-1" && model.CarColor != "null" && model.CarColor != null)
                    {
                        CarColor = model.CarColor;
                    }
                    else
                    {
                        CarColor = "";
                    }


                    //经销商ｉＤ
                    if (model.DealerID != Constant.INT_INVALID_VALUE)
                    {
                        if (int.TryParse(model.DealerID.ToString(), out DealerId))
                        {
                        }
                    }

                    DMSMemberName = model.DMSMemberName;
                    OrderRemark = model.OrderRemark;
                    CreateTime = model.OrderCreateTime.ToString();
                    CallRecord = model.CallRecord;
                }
            }
        }
    }
}