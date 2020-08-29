using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustCategory
{
    public partial class BuyCarInfo : System.Web.UI.UserControl
    {
        private string custid = string.Empty;
        /// <summary>
        /// 客户ＩＤ
        /// </summary>
        public string CustID
        {
            get
            {
                return custid;
            }
            set
            {
                custid = value;
            }
        }
        private string view = string.Empty;
        /// <summary>
        /// 1,是查看，0是编辑
        /// </summary>
        //public string View
        //{
        //    get
        //    {
        //        return view;
        //    }
        //    set
        //    {
        //        view = value;
        //    }
        //}

        private string custtype = string.Empty;
        /// <summary>
        /// 已购车，未购车
        /// </summary>
        public string CustType
        {
            get
            {
                return custtype;
            }
            set
            {
                custtype = value;
            }
        }
        //汽车品牌
        public int CarBrandID = 0;
        //汽车系列
        public int CarSerialID = 0;

        public int CarTypeID = 0;

        public string RequestFrom
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("From");
            }
        }

        public Entities.BuyCarInfo BuyCarInfoModel;

        //呼入弹屏时，CTI送过来的电话
        public string RequestCalledNum
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CalledNum");
            }
        }
        public string logmsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //初始化
                GetBuyCarModel(CustID);
                //BindCarBrand();
            }
        }
        /// <summary>
        /// 绑定汽车品牌
        /// </summary>
        //private void BindCarBrand()
        //{
        //    DataTable dt = BLL.BuyCarInfo.Instance.GetALLCarBrand();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            this.selCarBrandID.Items.Add(new ListItem(dt.Rows[i]["name"].ToString(), dt.Rows[i]["Brandid"].ToString()));
        //        }
        //    }
        //}
        private void GetBuyCarModel(string CustID)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            if (!string.IsNullOrEmpty(CustID))
            {

                BuyCarInfoModel = BLL.BuyCarInfo.Instance.GetBuyCarInfo(CustID);
                if (BuyCarInfoModel != null)
                {
                    //汽车品牌
                    CarBrandID = Convert.ToInt32(BuyCarInfoModel.CarBrandId);
                    //汽车系列
                    CarSerialID = Convert.ToInt32(BuyCarInfoModel.CarSerialId);
                    CarTypeID = Convert.ToInt32(BuyCarInfoModel.CarTypeID);
                }
            }
            if (BuyCarInfoModel == null)
            {
                BuyCarInfoModel = new Entities.BuyCarInfo();
            }

            logmsg = "（已" + sw.Elapsed.TotalSeconds.ToString("0.00") + "s）；";
            sw.Stop();
        }
    }
}