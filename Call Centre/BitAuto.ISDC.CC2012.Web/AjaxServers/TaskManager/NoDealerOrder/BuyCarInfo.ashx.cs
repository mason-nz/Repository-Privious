using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{

    /// <summary>
    /// BuyCarInfo 的摘要说明
    /// </summary>
    public class BuyCarInfo : IHttpHandler, IRequiresSessionState
    {
        #region 属性定义
        //请求处理程序
        private string RequestAction
        {
            get
            {
                return HttpContext.Current.Request["Action"].ToString();
            }
        }
        //品牌id
        private string RequestCarBrandID
        {
            get
            {
                return HttpContext.Current.Request["CarBrandID"].ToString();
            }
        }

        private int Age
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Age"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Age"]);
            }
        }
        private int Vocation
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Vocation"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Vocation"]);
            }
        }
        private string IDCard
        {
            get
            {
                return HttpContext.Current.Request["IDCard"] == null ? string.Empty : HttpContext.Current.Request["IDCard"];
            }
        }
        private int InCome
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["InCome"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["InCome"]);
            }
        }
        private int Marriage
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Marriage"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Marriage"]);
            }
        }
        //private int CarBrandID
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(HttpContext.Current.Request["CarBrandID"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarBrandID"]);
        //    }
        //}
        //private int CarSerialID
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(HttpContext.Current.Request["CarSerialID"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarSerialID"]);
        //    }
        //}

        private int carbrandid = 0;
        private int CarBrandID
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["CarBrandID"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarBrandID"]);
            //}
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["CarBrandID"]))
                {
                    return carbrandid;
                }
                else if (int.TryParse(HttpContext.Current.Request["CarBrandID"], out carbrandid))
                {
                    return carbrandid;
                }
                else
                {
                    return carbrandid;
                }
            }
        }
        private int carserialid = 0;
        private int CarSerialID
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["CarSerialID"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarSerialID"]);
            //}
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["CarSerialID"]))
                {
                    return carserialid;
                }
                else if (int.TryParse(HttpContext.Current.Request["CarSerialID"], out carserialid))
                {
                    return carserialid;
                }
                else
                {
                    return carserialid;
                }
            }
        }

        private int cartypeid = 0;
        private int CarTypeID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["CarTypeID"]))
                {
                    return cartypeid;
                }
                else if (int.TryParse(HttpContext.Current.Request["CarTypeID"], out cartypeid))
                {
                    return cartypeid;
                }
                else
                {
                    return cartypeid;
                }
            }
        }

        private string CarName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarName"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CarName"]);
            }
        }
        private int IsAttestation
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["IsAttestation"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["IsAttestation"]);
            }
        }
        private int DriveAge
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["DriveAge"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["DriveAge"]);
            }
        }
        private string UserName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["UserName"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserName"]);
            }
        }

        private string CarNo
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarNo"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CarNo"]);
            }
        }
        private string Remark
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Remark"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Remark"]);
            }
        }
        //已购车未购车
        private int Type
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Type"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Type"]);
            }
        }
        //客户id
        private string TaskID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TaskID"]) == true ? string.Empty : HttpContext.Current.Request["TaskID"];
            }
        }

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            string msg = string.Empty;
            switch (RequestAction)
            {
                case "GetCarSerialInfo":
                    //根据品牌id取车系
                    GetCarSerial(out msg);
                    break;
                case "SubBuyCarInfoInfo":
                    subbugcarinfo(out msg);
                    break;
                case "GetALLCarBrand":
                    GetALLCarBrand(out msg);
                    break;
            }
            context.Response.Write(msg);
        }
        private void subbugcarinfo(out string msg)
        {
            msg = string.Empty;
            Entities.OrderBuyCarInfo Model = new Entities.OrderBuyCarInfo();
            Model.Age = Age;
            Model.CarBrandId = CarBrandID;
            Model.CarSerialId = CarSerialID;
            Model.CarTypeID = CarTypeID;
            Model.CarName = CarName;
            Model.CarNo = CarNo;
            Model.DriveAge = DriveAge;
            Model.Income = InCome;
            Model.IsAttestation = IsAttestation;
            Model.Marriage = Marriage;
            Model.IDCard = IDCard;
            Model.Remark = Remark;
            Model.Status = 0;
            Model.Vocation = Vocation;
            Model.UserName = UserName;
            Model.Type = Type;
            Model.TaskID = Convert.ToInt32(TaskID);
            Model.CreateTime = System.DateTime.Now;
            Model.CreateUserID = (int?)BLL.Util.GetLoginUserID();
            Model.LastModifyTime = System.DateTime.Now;
            Model.LastModifyUserID = (int?)BLL.Util.GetLoginUserID();
            try
            {

                if (BLL.OrderBuyCarInfo.Instance.IsExistsByTaskID(Model.TaskID))
                {
                    BLL.OrderBuyCarInfo.Instance.Update(Model);
                    BLL.Util.InsertUserLog("更新任务id为【" + Model.TaskID + "】的客户分类为已购车未购车信息。");
                }
                else
                {
                    BLL.OrderBuyCarInfo.Instance.Insert(Model);
                    BLL.Util.InsertUserLog("插入任务id为【" + Model.TaskID + "】的客户分类为已购车未购车信息。");
                }



                msg = "success";
            }
            catch (Exception ex)
            {
                msg = "已购车未购车信息保存失败，失败原因：" + ex.Message.ToString();
            }
        }
        /// <summary>
        /// 取车系
        /// </summary>
        /// <param name="msg"></param>
        private void GetCarSerial(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(RequestCarBrandID))
            {
                int CarBrandID = Convert.ToInt32(RequestCarBrandID);
                DataTable dt = BLL.BuyCarInfo.Instance.GetCarSerial(CarBrandID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{'SerialID':'" + dt.Rows[i]["serialid"] + "',";
                    msg += "'Name':'" + dt.Rows[i]["name"] + "'},";
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = msg.Substring(0, msg.Length - 1);
                    msg += "]}";
                }
            }
        }
        /// <summary>
        /// 取所有品牌
        /// </summary>
        /// <param name="msg"></param>
        private void GetALLCarBrand(out string msg)
        {
            msg = string.Empty;


            DataTable dt = BLL.BuyCarInfo.Instance.GetALLCarBrand();
            if (dt != null && dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{'Brandid':'" + dt.Rows[i]["Brandid"] + "',";
                msg += "'Name':'" + dt.Rows[i]["name"] + "'},";
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                msg = msg.Substring(0, msg.Length - 1);
                msg += "]}";
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}