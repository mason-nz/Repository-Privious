using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder
{
    public partial class BuyCarInfo : System.Web.UI.UserControl
    {
        public string Type = string.Empty;
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["TaskID"]))
                {
                    return Request["TaskID"];
                }
                else
                {
                    return "";
                }
            }
        }
        //汽车品牌
        public int CarBrandID = 0;
        //汽车系列
        public int CarSerialID = 0;
        public int CarTypeID = 0;
        //易湃订单ID
        public string YPOrderID = string.Empty;


        public Entities.OrderBuyCarInfo BuyCarInfoModel = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化
                GetBuyCarModel(TaskID);
            }
        }
        private void GetBuyCarModel(string TaskID)
        {
            if (!string.IsNullOrEmpty(TaskID))
            {
                long _taskID = Convert.ToInt32(TaskID);
                //数据来源
                Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(_taskID);
                if (model != null)
                {
                    Type = model.Source.ToString();
                }

                BuyCarInfoModel = BLL.OrderBuyCarInfo.Instance.GetOrderBuyCarInfo(_taskID);
                if (BuyCarInfoModel != null)
                {
                    //汽车品牌
                    CarBrandID = Convert.ToInt32(BuyCarInfoModel.CarBrandId);
                    //汽车系列
                    CarSerialID = Convert.ToInt32(BuyCarInfoModel.CarSerialId);
                    CarTypeID = Convert.ToInt32(BuyCarInfoModel.CarTypeID);

                }

                if (BuyCarInfoModel == null)
                {
                    BuyCarInfoModel = new Entities.OrderBuyCarInfo();
                }

                //1,3都从新车里取数据，modify by qizq 2013-7-19
                switch (model.Source)
                {
                    case 1:
                        Entities.OrderNewCar model_newCar = new Entities.OrderNewCar();
                        model_newCar = BLL.OrderNewCar.Instance.GetOrderNewCar(_taskID);
                        if (model_newCar != null)
                        {
                            //易湃订单ID
                            YPOrderID = model_newCar.YPOrderID.ToString();
                        }
                        break;
                    case 3:
                        Entities.OrderNewCar model_newCar1 = new Entities.OrderNewCar();
                        model_newCar1= BLL.OrderNewCar.Instance.GetOrderNewCar(_taskID);
                        if (model_newCar1 != null)
                        {
                            //易湃订单ID
                            YPOrderID = model_newCar1.YPOrderID.ToString();
                        }
                        break;
                    case 2:
                        Entities.OrderRelpaceCar model_relpaceCar = new Entities.OrderRelpaceCar();
                        model_relpaceCar = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(_taskID);
                        if (model_relpaceCar != null)
                        {
                            //易湃订单ID
                            YPOrderID = model_relpaceCar.YPOrderID.ToString();
                        }
                        break;
                }
            }
        }
    }
}