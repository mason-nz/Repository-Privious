using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ConsultManager
{
    public partial class ConsultSecondCarView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestRecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }
        private string RequestRecordType
        {
            get { return HttpContext.Current.Request["RecordType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecordType"].ToString()); }
        }
        public string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int recID;
            if (int.TryParse(RequestRecID, out recID))
            {
                ConsultSecondCar model = BLL.ConsultSecondCar.Instance.GetConsultSecondCar(recID);
                if (model != null)
                {
                    ConsultID.InnerText = "二手车";
                    RecordType.InnerText = RequestRecordType == "1" ? "呼入" : "呼出";
                    switch (model.QuestionType)
                    {
                        case 70001: QuestionType.InnerText = "买车";
                            break;
                        case 70002: QuestionType.InnerText = "卖车";
                            break;
                        case 70003: QuestionType.InnerText = "删除";
                            break;
                    }
                    //关注车型和系列 品牌和系列 
                    int brandID;
                    int serialID;
                    string brandSerialName = string.Empty;
                    if (int.TryParse(model.CarBrandId.ToString(), out brandID))
                    {
                        brandSerialName = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(brandID) + "-";
                        //DataTable dt = BLL.BuyCarInfo.Instance.GetCarBrandName(brandID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    brandSerialName += dt.Rows[0]["Name"].ToString() + "-";
                        //}
                    }
                    if (int.TryParse(model.CarSerialId.ToString(), out serialID))
                    {
                        brandSerialName += BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(serialID) + "-";
                        //DataTable dt = BLL.BuyCarInfo.Instance.GetCarSerialName(serialID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    brandSerialName += dt.Rows[0]["Name"].ToString() + "-";
                        //}
                    }
                    if (model.CarName != "" && model.CarName != "-2" && model.CarName != null)
                    {
                        brandSerialName += model.CarName + "-";
                    }
                    CarBrand.InnerText = brandSerialName.TrimEnd('-');
                    //出售车型和系列 品牌和系列 
                    int sellbrandID;
                    int sellserialID;
                    string sellbrandSerialName = string.Empty;
                    if (int.TryParse(model.SaleCarBrandId.ToString(), out sellbrandID))
                    {
                        sellbrandSerialName = BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(sellbrandID) + "-";
                        //DataTable dt = BLL.BuyCarInfo.Instance.GetCarBrandName(sellbrandID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    sellbrandSerialName += dt.Rows[0]["Name"].ToString();
                        //}
                    }
                    if (int.TryParse(model.SaleCarSerialId.ToString(), out sellserialID))
                    {
                        sellbrandSerialName += BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(sellserialID) + "-";
                        //DataTable dt = BLL.BuyCarInfo.Instance.GetCarSerialName(sellserialID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    sellbrandSerialName += dt.Rows[0]["Name"].ToString() + "-";
                        //}
                    }
                    if (model.SaleCarName != "" && model.SaleCarName != "-2" && model.SaleCarName != null)
                    {
                        sellbrandSerialName += model.SaleCarName;
                    }
                    SaleCar.InnerText = sellbrandSerialName.TrimEnd('-');
                    CallRecord.InnerText = model.CallRecord;
                }
            }
        }

        public string ShowCallRecord()
        {
            string returnStr = string.Empty;
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                Entities.CallRecordInfo callRecordInfo = BLL.CallRecordInfo.Instance.GetCallRecordInfoByTaskID(RequestTaskID);
                if (callRecordInfo != null)
                {
                    returnStr = "<a href=\"" + callRecordInfo.AudioURL + "\" style=\"vertical-align:middle\">  <img  src=\"/Images/callTel.png\" border=\"0\" /></a>";
                }
            }
            return returnStr;
        }
    }
}