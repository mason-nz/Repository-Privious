using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder
{
    public partial class BuyCarInfoView : System.Web.UI.UserControl
    {
        private int Transfer = 0;

        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetBuyCarModel(TaskID);
            }

        }
        public string CreateTime = string.Empty;
        public string UpdateUser = string.Empty;
        public string UpdateTime = string.Empty;

        private void GetBuyCarModel(string TaskID)
        {

            if (!string.IsNullOrEmpty(TaskID))
            {


                OrderTask OrderTask = BLL.OrderTask.Instance.GetOrderTask(Convert.ToInt32(TaskID));
                if (OrderTask != null)
                {
                    CreateTime = Convert.ToDateTime(OrderTask.CreateTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }



                Entities.OrderBuyCarInfo BuyCarModel = BLL.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(Convert.ToInt32(TaskID));
                if (BuyCarModel != null)
                {
                    //年龄
                    if (BuyCarModel.Age > 0)
                    {
                        this.lblAge.Text = BuyCarModel.Age.ToString();
                    }
                    //身份证号码
                    this.lblIDCard.Text = BuyCarModel.IDCard;
                    //婚姻状态
                    if (BuyCarModel.Marriage != -2)
                    {
                        if (BuyCarModel.Marriage.ToString() == "1")
                        {
                            this.lblMarriage.Text = "已婚";
                        }
                        else
                        {
                            this.lblMarriage.Text = "未婚";
                        }
                    }
                    //车型名称
                    //this.lblCarName.Text = BuyCarModel.CarName;
                    //认证车主
                    if (BuyCarModel.IsAttestation >= 0)
                    {
                        if (BuyCarModel.IsAttestation == 1)
                        {
                            this.lbllegalize.Text = "是";
                        }
                        else if (BuyCarModel.IsAttestation == 0)
                        {
                            this.lbllegalize.Text = "否";
                        }
                    }
                    //驾龄
                    if (BuyCarModel.DriveAge > 0)
                    {
                        this.lblDriveAge.Text = BuyCarModel.DriveAge.ToString();
                    }
                    //用户名
                    this.lblUserName.Text = BuyCarModel.UserName.ToString();
                    //车牌号
                    this.lblCarNumber.Text = BuyCarModel.CarNo.ToString();
                    //备注
                    this.lblNote.Text = BuyCarModel.Remark;

                    //职业
                    if (BuyCarModel.Vocation >= 0)
                    {
                        this.lblVocation.Text = BLL.Util.GetEnumOptText(typeof(CustVocation), Convert.ToInt32(BuyCarModel.Vocation));
                    }
                    //收入
                    if (BuyCarModel.Income >= 0)
                    {
                        this.lblInCome.Text = BLL.Util.GetEnumOptText(typeof(CustInCome), Convert.ToInt32(BuyCarModel.Income));
                    }



                    UpdateTime = Convert.ToDateTime(BuyCarModel.LastModifyTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    UpdateUser = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(BuyCarModel.CreateUserID));

                    string carType = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(int.Parse(BuyCarModel.CarTypeID.ToString()));
                    string carMaster = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(int.Parse(BuyCarModel.CarBrandId.ToString()));
                    string carSerial = BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(int.Parse(BuyCarModel.CarSerialId.ToString()));
                    if (!string.IsNullOrEmpty(carMaster))
                    {
                        lblCarName.Text += carMaster;
                    }
                    if (!string.IsNullOrEmpty(carSerial))
                    {
                        lblCarName.Text += "-" + carSerial;
                    }
                    if (!string.IsNullOrEmpty(carType))
                    {
                        lblCarName.Text += "-" + carType;
                    }

                }

            }
        }
    }

}