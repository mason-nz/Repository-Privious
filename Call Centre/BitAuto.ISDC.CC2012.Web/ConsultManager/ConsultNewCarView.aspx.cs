using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ConsultManager
{
    public partial class ConsultNewCarView : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
                ConsultNewCar model = BLL.ConsultNewCar.Instance.GetConsultNewCar(recID);
                if (model != null)
                {
                    ConsultID.InnerText = "新车";
                    RecordType.InnerText = RequestRecordType == "1" ? "呼入" : "呼出";
                    DealerName.InnerText = model.DealerName;
                    BuyCarBudget.InnerText = model.BuyCarBudget;
                    Activity.InnerText = model.Activity;
                    switch (model.BuyCarTime)
                    {
                        case 50001: BuyCarTime.InnerText = "一周内";
                            break;
                        case 50002: BuyCarTime.InnerText = "一月内";
                            break;
                        case 50003: BuyCarTime.InnerText = "半年内";
                            break;
                        case 50004: BuyCarTime.InnerText = "无计划";
                            break;
                    }
                    switch (model.BuyOrDisplace)
                    {
                        case 1: BuyOrDisplace.InnerText = "新购";
                            break;
                        case 2: BuyOrDisplace.InnerText = "置换";
                            break;
                    }
                    CallRecord.InnerText = model.CallRecord;
                    switch (model.AcceptTel)
                    {
                        case 1: AcceptTel.InnerText = "接受";
                            break;
                        case 0: AcceptTel.InnerText = "不接受";
                            break;
                    }
                    //车型和系列 品牌和系列 
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

                    CreateUserID.InnerText = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(model.CreateUserID.ToString()));
                    CreateTime.InnerText = model.CreateTime.ToString();
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
                    returnStr = "<a href=\"" + callRecordInfo.AudioURL + "\"  style=\"vertical-align:middle\">  <img  src=\"/Images/callTel.png\"  border=\"0\" /></a>";
                }
            }
            return returnStr;
        }
    }
}