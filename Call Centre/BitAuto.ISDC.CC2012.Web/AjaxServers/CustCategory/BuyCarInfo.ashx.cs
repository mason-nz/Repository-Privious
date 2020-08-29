using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.WebService;
using BitAuto.ISDC.CC2012.WebService.BAA.QiCheTongService;
namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory
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
                return string.IsNullOrEmpty(HttpContext.Current.Request["Action"]) == true ? "" : HttpContext.Current.Request["Action"].ToString();
            }
        }
        //品牌id
        private string RequestCarBrandID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarBrandID"]) == true ? "" : HttpContext.Current.Request["CarBrandID"].ToString();
            }
        }
        private int _age = -2;
        private int Age
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["Age"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Age"]);
            //}

            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["Age"]))
                {
                    return _age;
                }
                else if (int.TryParse(HttpContext.Current.Request["Age"], out _age))
                {
                    return _age;
                }
                else
                {
                    return _age;
                }
            }
        }
        private int _vacation = -2;
        private int Vocation
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["Vocation"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Vocation"]);
            //}


            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["Vocation"]))
                {
                    return _vacation;
                }
                else if (int.TryParse(HttpContext.Current.Request["Vocation"], out _vacation))
                {
                    return _vacation;
                }
                else
                {
                    return _vacation;
                }
            }
        }
        private string IDCard
        {
            get
            {
                return HttpContext.Current.Request["IDCard"] == null ? string.Empty : HttpContext.Current.Request["IDCard"];
            }
        }
        private int _income = -2;
        private int InCome
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["InCome"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["InCome"]);
            //}


            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["InCome"]))
                {
                    return _income;
                }
                else if (int.TryParse(HttpContext.Current.Request["InCome"], out _income))
                {
                    return _income;
                }
                else
                {
                    return _income;
                }
            }
        }
        private int _marriage = -2;
        private int Marriage
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["Marriage"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Marriage"]);
            //}

            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["Marriage"]))
                {
                    return _marriage;
                }
                else if (int.TryParse(HttpContext.Current.Request["Marriage"], out _marriage))
                {
                    return _marriage;
                }
                else
                {
                    return _marriage;
                }
            }

        }
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

        private int _isattestation = -2;

        private int IsAttestation
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["IsAttestation"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["IsAttestation"]);
            //}

            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["IsAttestation"]))
                {
                    return _isattestation;
                }
                else if (int.TryParse(HttpContext.Current.Request["IsAttestation"], out _isattestation))
                {
                    return _isattestation;
                }
                else
                {
                    return _isattestation;
                }
            }


        }
        private int _driveage = -2;
        private int DriveAge
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["DriveAge"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["DriveAge"]);
            //}

            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["DriveAge"]))
                {
                    return _driveage;
                }
                else if (int.TryParse(HttpContext.Current.Request["DriveAge"], out _driveage))
                {
                    return _driveage;
                }
                else
                {
                    return _driveage;
                }
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
        private int _type;
        private int Type
        {
            //get
            //{
            //    return string.IsNullOrEmpty(HttpContext.Current.Request["Type"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["Type"]);
            //}

            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["Type"]))
                {
                    return _type;
                }
                else if (int.TryParse(HttpContext.Current.Request["Type"], out _type))
                {
                    return _type;
                }
                else
                {
                    return _type;
                }
            }
        }
        //客户id
        private string CustID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CustID"]) == true ? string.Empty : HttpContext.Current.Request["CustID"];
            }
        }

        #endregion

        #region 汽车通注册信息

        //手机号码
        private string RequestPhoneNumber
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["PhoneNumber"]) == true ? "" : HttpContext.Current.Request["PhoneNumber"].ToString();
            }
        }

        //客户ID
        private string RequestCustID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CustID"]) == true ? "" : HttpContext.Current.Request["CustID"].ToString();
            }
        }
        //任务ID
        private string RequestTaskID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TaskID"]) == true ? "" : HttpContext.Current.Request["TaskID"].ToString();
            }
        }
        //无主订单ID
        private string RequestYPOrderID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["YPOrderID"]) == true ? "" : HttpContext.Current.Request["YPOrderID"].ToString();
            }
        }

        private string QiCheTongServiceContent = System.Configuration.ConfigurationManager.AppSettings["QiCheTongSMSContent"];//汽车通手机号发送的信息内容

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
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
                //手机号注册汽车通
                case "RegisterCarTong":
                    registerCarTong(out msg);
                    break;
                case "":
                    msg = "错误请求";
                    break;
            }
            context.Response.Write(msg);
        }
        private void subbugcarinfo(out string msg)
        {
            msg = string.Empty;
            Entities.BuyCarInfo Model = new Entities.BuyCarInfo();
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
            Model.CustID = CustID;
            Model.CreateTime = System.DateTime.Now;
            Model.CreateUserID = (int?)BLL.Util.GetLoginUserID();
            try
            {
                if (BLL.DealerInfo.Instance.IsExistsByCustID(Model.CustID))
                {
                    BLL.DealerInfo.Instance.DeleteForSetStatus(Model.CustID);
                    //BLL.Util.InsertUserLog("删除客户id为" + Model.CustID + "，的车易通会员信息。");
                    BLL.DealerBrandInfo.Instance.Delete(Model.CustID);
                    //BLL.Util.InsertUserLog("删除客户id为" + Model.CustID + "，的经销商品牌信息。");
                }
                if (BLL.BuyCarInfo.Instance.IsExistsByCustID(Model.CustID))
                {
                    BLL.BuyCarInfo.Instance.Update(Model);
                    BLL.Util.InsertUserLog("更新客户id为" + Model.CustID + "，客户分类为个人信息。");
                }
                else
                {
                    BLL.BuyCarInfo.Instance.Insert(Model);
                    BLL.Util.InsertUserLog("插入客户id为" + Model.CustID + "，客户分类为个人信息。");
                }



                msg = "success";
            }
            catch (Exception ex)
            {
                msg = "个人信息保存失败，失败原因：" + ex.Message.ToString();
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        msg += "{'SerialID':'" + dt.Rows[i]["serialid"] + "',";
                        msg += "'Name':'" + dt.Rows[i]["name"] + "'},";
                    }

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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{'Brandid':'" + dt.Rows[i]["Brandid"] + "',";
                    msg += "'Name':'" + dt.Rows[i]["name"] + "'},";
                }


                msg = msg.Substring(0, msg.Length - 1);
                msg += "]}";
            }
        }

        //根据电话号码注册汽车通 add lxw 2013.4.9
        private void registerCarTong(out string msg)
        {
            msg = string.Empty;
            RegisterMobileUserInfo lst = null;
            try
            {
                //调用汽车通接口
                lst = WebService.QiCheTongHelper.Instance.RegisterMobileUser(RequestPhoneNumber);
            }
            catch (Exception ex)
            {
                msg = "{'result':'no','msg':'调用汽车通接口出错：" + ex.Message + "'}";
                //记录日志
                addRegisterLog(ex.Message);
                return;
            }
            //string RegisterResult = lst.GetType().GetField("RegisterResult").GetValue(lst).ToString();
            string RegisterResult = lst.RegisterResult.ToString();
            string registerMsg = string.Empty;

            registerMsg = verifyRegisterIsRight(RegisterResult);
            if (registerMsg == "成功")
            {
                //string mobile = lst.GetType().GetField("Mobile").GetValue(lst).ToString();
                //string pwd = lst.GetType().GetField("PassWord").GetValue(lst).ToString();
                string mobile = lst.Mobile;
                string pwd = lst.PassWord;

                //发送短信
                //com.bitauto.mobile.MsgService msgService = new com.bitauto.mobile.MsgService();
                string msgContent = string.Format(QiCheTongServiceContent, mobile, pwd);
                //string md5 = msgService.MixMd5("6116" + mobile + msgContent + "Ytt1TEy3hnYIgqTOOIGEc0MFL9wrN0yJJuUUPVfjyM+dkY3ei/8WUc8L7qFqgCbp");
                //int msgid = msgService.SendMsgImmediately("6116", mobile, msgContent, "", DateTime.Now, md5);
                string md5 = SMSServiceHelper.Instance.MixMd5(mobile, msgContent);
                int msgid = SMSServiceHelper.Instance.SendMsgImmediately(mobile, msgContent, DateTime.Now.AddHours(1), md5);
                getRegisterData(mobile, pwd, out msg, msgid.ToString());

                registerMsg += "，userid：" + msgid + "，pwd：" + pwd;
            }
            else
            {
                msg = "{'result':'no','msg':'" + registerMsg + "'}";
            }
            //记录日志
            addRegisterLog(registerMsg);
        }

        //注册汽车通成功返回数据
        private void getRegisterData(string mobile, string pwd, out string msg, string userid)
        {
            msg = "{'result':'yes','mobile':'" + mobile + "','pwd':'" + pwd + "','userid':'" + userid + "'}";
        }

        //根据返回值得到操作结果
        private string verifyRegisterIsRight(string registerResult)
        {
            string verifyResult = string.Empty;
            switch (registerResult)
            {
                case "Success": verifyResult = "成功";
                    break;
                case "InvalidUserName": verifyResult = "用户名无效";
                    break;
                case "InvalidPassword": verifyResult = "密码无效";
                    break;
                case "InvalidQuestion": verifyResult = "密码问题无效";
                    break;
                case "InvalidAnswer": verifyResult = "密码问题答案无效";
                    break;
                case "InvalidEmail": verifyResult = "电子邮件无效";
                    break;
                case "DuplicateUserName": verifyResult = "用户名已存在";
                    break;
                case "DuplicateEmail": verifyResult = "Email已存在";
                    break;
                case "Failed": verifyResult = "失败";
                    break;
                case "DuplicateMobile": verifyResult = "手机已经存在";
                    break;
                case "InvalidMobile": verifyResult = "手机号无效";
                    break;
            }

            return verifyResult;
        }

        //注册完记录日志
        private void addRegisterLog(string registerMsg)
        {
            //记录日志
            Entities.UpdateOrderData model = new Entities.UpdateOrderData();
            model.UpdateType = 1;
            model.UpdateErrorMsg = "汽车通注册：手机号码为 " + RequestPhoneNumber + " 注册完毕。返回信息：" + registerMsg;
            model.CreateTime = DateTime.Now;
            model.CreateUserID = BLL.Util.GetLoginUserID();
            model.IsUpdate = 1;
            model.APIType = 3;//3-代表汽车通注册信息；
            if (RequestCustID != "")
            {
                model.CustID = RequestCustID;
            }
            if (RequestTaskID != "")
            {
                model.TaskID = RequestTaskID;
            }
            if (RequestYPOrderID != "")
            {
                model.YPOrderID = int.Parse(RequestYPOrderID);
            }

            BLL.UpdateOrderData.Instance.Insert(model);
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