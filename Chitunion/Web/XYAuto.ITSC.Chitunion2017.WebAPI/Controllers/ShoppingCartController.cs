using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ShoppingCartController : ApiController
    {
        int currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();

        #region 添加、删除、清空购物车
        /// <summary>
        /// 添加、删除、清空购物车
        /// </summary>
        /// <param name="optcart"></param>
        /// <returns></returns>
        [HttpPost]
        public Common.JsonResult Operate_ShoppingCart([FromBody]JSONOptCart jsoncart)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartController]******Operate_ShoppingCart begin******");           
            #region 参数校验
            string vmsg = string.Empty;
            if (!jsoncart.CheckSelfModel(out vmsg))
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion

            try
            {
                switch (jsoncart.OptType)
                {
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumJSONCartOptType.ADD:
                        AddCartInfo(jsoncart, currentUserID);
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumJSONCartOptType.Delete:
                        DeleteCartInfo(jsoncart, currentUserID);
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumJSONCartOptType.ClearAll:
                        ClearAllCartInfo(jsoncart.MediaType, currentUserID);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartController]******Operate_ShoppingCart 出错：" + ex.Message);
                vmsg = "出错：" + ex.Message;
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            
            BLL.Loger.Log4Net.Info("[ShoppingCartController]******Operate_ShoppingCart end******");
            return WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功");
        }

        #region 新增
        //前台购物车新增不能清空再增 因为 前台没有购物车中的全部信息 更没有自媒体后填的广告位信息
        public void AddCartInfo(JSONOptCart jsoncart,int createuserid)
        {
            if (jsoncart.IDs == null)
                return;

            if (jsoncart.MediaType == (int)Entities.EnumMediaType.APP)
            {                                                
                foreach (JSONAPP app in jsoncart.IDs)
                {
                    //查询媒体类型、广告位ID、创建人是否存在
                    if (BLL.CartInfo.Instance.IsExistsDetailID(jsoncart.MediaType, createuserid, app.PublishDetailID))
                    {}
                    else
                    {
                        Entities.CartInfo cartmodel = new Entities.CartInfo();
                        cartmodel.MediaType = jsoncart.MediaType;
                        cartmodel.MediaID = app.MediaID;
                        cartmodel.PubDetailID = app.PublishDetailID;
                        cartmodel.IsSelected = 1;
                        cartmodel.CreateUserID = currentUserID;                        
                        BLL.CartInfo.Instance.Insert(cartmodel);
                    }
                }                
            }
            else
            {
                foreach (JSONAPP app in jsoncart.IDs)
                {
                    //查询媒体类型、媒体ID、创建人是否存在
                    if (BLL.CartInfo.Instance.IsExistsMediaID(jsoncart.MediaType, createuserid, app.MediaID))
                    { }
                    else
                    {
                        Entities.CartInfo cartmodel = new Entities.CartInfo();
                        cartmodel.MediaType = jsoncart.MediaType;
                        cartmodel.MediaID = app.MediaID;
                        cartmodel.IsSelected = 1;
                        cartmodel.CreateUserID = currentUserID;
                        BLL.CartInfo.Instance.Insert(cartmodel);
                    }
                }                                      
            }            
        }        
        #endregion
        #region 删除
        public void DeleteCartInfo(JSONOptCart jsoncart, int createuserid)
        {
            if (jsoncart.IDs == null)
                return;

            string ids = "";
            if (jsoncart.MediaType == (int)Entities.EnumMediaType.APP)
            {
                foreach (JSONAPP app in jsoncart.IDs)
                {
                    ids += app.PublishDetailID + ",";
                }
                ids = ids.Substring(0, ids.Length - 1);
                BLL.CartInfo.Instance.Delete_APPMedia(jsoncart.MediaType, currentUserID, ids);
            }
            else
            {
                foreach (JSONAPP app in jsoncart.IDs)
                {
                    ids += app.MediaID + ",";
                }
                ids = ids.Substring(0, ids.Length - 1);
                BLL.CartInfo.Instance.Delete_SelfMedia(jsoncart.MediaType, currentUserID, ids);
            }            
        }
        #endregion
        #region 清空
        public void ClearAllCartInfo(int mediatype, int createuserid)
        {
            BLL.CartInfo.Instance.ClearAll_CartInfo(mediatype, currentUserID);
        }
        public void ClearAllCartInfo(int mediatype, int createuserid,string ids)
        {
            BLL.CartInfo.Instance.ClearAll_CartInfo(mediatype, currentUserID, ids);
        }
        #endregion
        #endregion

        #region 点击投放时，把当前购物车里面的内容添加到后台缓存
        [HttpPost]
        public Common.JsonResult Delivery_ShoppingCart([FromBody]JSONoptdelivery josnde)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartController]******Delivery_ShoppingCart begin******");
            #region 参数校验
            string vmsg = string.Empty;
            if (!josnde.CheckSelfModel(out vmsg))
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion
            try
            {
                AddCartInfo(josnde, currentUserID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartController]******Delivery_ShoppingCart 出错errormsg:" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, "出错:"+ ex.Message, -1);
            }
            
            BLL.Loger.Log4Net.Info("[ShoppingCartController]******Delivery_ShoppingCart end******");
            return WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功", 0);
        }

        #region 新增
        public void AddCartInfo(JSONoptdelivery jsoncart, int createuserid)
        {
            if (jsoncart.IDs == null)
                return;

            //先清空
            ClearAllCartInfo(jsoncart.MediaType, createuserid);
            foreach (JSONID id in jsoncart.IDs)
            {
                Entities.CartInfo cartmodel = new Entities.CartInfo();
                cartmodel.MediaType = jsoncart.MediaType;
                cartmodel.MediaID = id.MediaID;
                cartmodel.PubDetailID = id.PublishDetailID;                
                cartmodel.IsSelected = Convert.ToByte(id.IsSelected == true ? 1 : 0);
                cartmodel.CreateUserID = currentUserID;
                BLL.CartInfo.Instance.Insert(cartmodel);
            }            
        }
        #endregion
        #endregion

        #region 购物车添加广告位,选择广告位后(保存并添加帐户操作)
        //APP、自媒体在点保存并添加帐户按钮时 要重新保存购物车
        [HttpPost]
        public Common.JsonResult SaveSelfMedia_ShoppingCart([FromBody]JSONselfmedia r)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartController]******SaveSelfMedia_ShoppingCart begin******");
            Common.JsonResult jr = new Common.JsonResult();
            jr.Message = "操作成功";

            if (r.IDs == null)
            {
                jr.Message = "IDs参不能为空或Null";
                jr.Status = 2;
                return jr;
            }

            try
            {
                //先清购物车 
                ClearAllCartInfo(r.MediaType, currentUserID);

                foreach (JSONself s in r.IDs)
                {
                    Entities.CartInfo carmodel = new Entities.CartInfo();
                    carmodel.MediaType = r.MediaType;
                    carmodel.MediaID = s.MediaID;
                    carmodel.IsSelected = 1;
                    carmodel.CreateUserID = currentUserID;
                    if (!string.IsNullOrEmpty(s.PublishDetailID))
                    {
                        carmodel.PubDetailID = Convert.ToInt32(s.PublishDetailID);
                    }

                    BLL.CartInfo.Instance.Insert(carmodel);
                }               

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartController]******SaveSelfMedia_ShoppingCart,errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartController]******SaveSelfMedia_ShoppingCart end******");
            return jr;
        }

        #region 测试用
        //public Common.JsonResult SaveSelfMedia_ShoppingCart(string optselfmedia)
        //{
        //    BLL.Loger.Log4Net.Info("[ShoppingCartController]******SaveSelfMedia_ShoppingCart begin******");
        //    Common.JsonResult jr = new Common.JsonResult();
        //    jr.Message = "操作成功";
        //    jr.Status = 0;
        //    JSONselfmedia self = new JSONselfmedia();

        //    //反序列化JSON串到对象，验证输入参数合法性
        //    try
        //    {
        //        self = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONselfmedia>(optselfmedia);

        //        jr.Result = self;
        //    }
        //    catch (Exception ex)
        //    {
        //        BLL.Loger.Log4Net.Info("[ShoppingCartController]******SaveSelfMedia_ShoppingCart,errormsg:" + ex.Message);
        //        jr.Status = 2;
        //        jr.Message = ex.Message;
        //    }

        //    BLL.Loger.Log4Net.Info("[ShoppingCartController]******SaveSelfMedia_ShoppingCart end******");
        //    return jr;
        //}
        #endregion
        #endregion

        #region 获取当前购物车里面的内容
        [HttpGet]
        public Common.JsonResult GetInfo_ShoppingCart(int MediaType)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartController]******GetInfo_ShoppingCart begin******");
            Common.JsonResult jr = new Common.JsonResult();
            jr.Message = "操作成功";

            //获取购物车信息
            System.Data.DataTable dt = BLL.CartInfo.Instance.GetCartInfo_MediaTypeUserID(MediaType, currentUserID);
            if (dt == null || dt.Rows.Count == 0)
            {
                jr.Message = "购物车时没有任何信息";
                jr.Status = 2;
                return jr;
            }

            JSONcartinfo cart = new JSONcartinfo();
            cart.MediaType = MediaType;
            List<JSONAPPMedia> appList = new List<JSONAPPMedia>();
            List<JSONSelfMedia2> selfList = new List<JSONSelfMedia2>();
            
            
            try
            {
                if (MediaType == (int)Entities.EnumMediaType.APP)
                {
                    cart.SelfMedia = null;
                    decimal totalamount = 0;
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        JSONAPPMedia app = new JSONAPPMedia();
                        if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                        {
                            app.MediaID = int.Parse(row["MediaID"].ToString());
                        }
                        if (row["PubDetailID"] != null && row["PubDetailID"].ToString() != "")
                        {
                            app.PublishDetailID = int.Parse(row["PubDetailID"].ToString());
                        }
                        if (row["Name"] != null && row["Name"].ToString() != "")
                        {
                            app.Name = row["Name"].ToString();
                        }
                        if (row["AdPosition"] != null && row["AdPosition"].ToString() != "")
                        {
                            app.AdPosition = row["AdPosition"].ToString();
                        }
                        if (row["AdForm"] != null && row["AdForm"].ToString() != "")
                        {
                            app.AdForm = row["AdForm"].ToString();
                        }
                        if (row["Style"] != null && row["Style"].ToString() != "")
                        {
                            app.Style = row["Style"].ToString();
                        }
                        if (row["CarouselCount"] != null && row["CarouselCount"].ToString() != "")
                        {
                            app.CarouselCount = int.Parse(row["CarouselCount"].ToString());
                        }
                        if (row["PlayPosition"] != null && row["PlayPosition"].ToString() != "")
                        {
                            app.PlayPosition = row["PlayPosition"].ToString();
                        }

                        if (row["SysPlatform"] != null && row["SysPlatform"].ToString() != "")
                        {
                            string[] s = row["SysPlatform"].ToString().Split(',');
                            string sys = "";
                            foreach (string item in s)
                            {
                                if (item == "12001")
                                {
                                    sys += "Android,";
                                }
                                else
                                {
                                    sys += "IOS,";
                                }
                            }
                            sys = sys.Substring(0, sys.Length - 1);
                            app.SysPlatform = sys;
                        }
                        if (row["Price"] != null && row["Price"].ToString() != "")
                        {
                            app.Price = Convert.ToDecimal(row["Price"].ToString());
                        }
                        if (row["BeginPlayDays"] != null && row["BeginPlayDays"].ToString() != "")
                        {
                            app.BeginPlayDays = Convert.ToInt32(row["BeginPlayDays"].ToString());
                        }
                        if (row["CPDCPM"] != null && row["CPDCPM"].ToString() != "")
                        {
                            app.CPDCPM = Convert.ToInt32(row["CPDCPM"].ToString());
                        }

                        if (row["IsSelected"] != null && row["IsSelected"].ToString() != "")
                        {
                            app.IsSelected = Convert.ToByte(row["IsSelected"]);
                        }

                        if (row["PublishStatus"] != null && row["PublishStatus"].ToString() != "")
                        {
                            app.PublishStatus = Convert.ToInt32(row["PublishStatus"].ToString());
                        }
                        if (row["expired"] != null && row["expired"].ToString() != "")
                        {
                            app.expired = Convert.ToInt32(row["expired"].ToString());
                        }

                        app.Amount = app.Price * app.BeginPlayDays;
                        appList.Add(app);
                        totalamount += app.Amount;
                    }

                    cart.TotalAmount = totalamount;
                }
                else
                {
                    cart.APP = null;
                    decimal totalamount = 0;
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        JSONSelfMedia2 self = new JSONSelfMedia2();
                        if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                        {
                            self.MediaID = int.Parse(row["MediaID"].ToString());
                        }
                        if (row["PubDetailID"] != null && row["PubDetailID"].ToString() != "")
                        {
                            self.PublishDetailID = int.Parse(row["PubDetailID"].ToString());
                        }
                        if (row["Number"] != null && row["Number"].ToString() != "")
                        {
                            self.Number = row["Number"].ToString();
                        }
                        if (row["Name"] != null && row["Name"].ToString() != "")
                        {
                            self.Name = row["Name"].ToString();
                        }
                        if (MediaType == (int)Entities.EnumMediaType.SinaWeibo)
                        {
                            if (row["Sign"] != null && row["Sign"].ToString() != "")
                            {
                                self.Sign = row["Sign"].ToString();
                            }
                        }

                        if (row["HeadIconURL"] != null && row["HeadIconURL"].ToString() != "")
                        {
                            self.HeadIconURL = row["HeadIconURL"].ToString();
                        }
                        if (row["ADTypeName"] != null && row["ADTypeName"].ToString() != "")
                        {
                            self.ADTypeName = row["ADTypeName"].ToString();
                        }

                        if (row["Price"] != null && row["Price"].ToString() != "")
                        {
                            self.Price = Convert.ToDecimal(row["Price"].ToString());
                        }
                        if (row["IsSelected"] != null && row["IsSelected"].ToString() != "")
                        {
                            self.IsSelected = Convert.ToByte(row["IsSelected"]);
                        }
                        if (row["PublishStatus"] != null && row["PublishStatus"].ToString() != "")
                        {
                            self.PublishStatus = Convert.ToInt32(row["PublishStatus"].ToString());
                        }
                        if (row["expired"] != null && row["expired"].ToString() != "")
                        {
                            self.expired = Convert.ToInt32(row["expired"].ToString());
                        }
                        selfList.Add(self);
                        totalamount += self.Price;
                    }

                    cart.TotalAmount = totalamount;
                }
                cart.APP = appList;
                cart.SelfMedia = selfList;
                jr.Result = cart;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartController]******GetInfo_ShoppingCart,errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartController]******GetInfo_ShoppingCart end******");
            return jr;
        }
        #endregion        



        #region 添加、删除、清空购物车实体
        public class JSONOptCart
        {
            /// <summary>
            /// 1:新增，2：删除，3：清空
            /// </summary>
            private Entities.EnumJSONCartOptType _OptType;
            public Entities.EnumJSONCartOptType OptType
            {
                get { return _OptType; }
                set { _OptType = value; }
            }            

            /// <summary>
            /// 媒体类型
            /// 微信：14001，APP：14002，微博：14003，视频：14004，直播：14005
            /// </summary>
            private int _MediaType;
            public int MediaType
            {
                get { return _MediaType; }
                set { _MediaType = value; }
            }

            /// <summary>
            /// 自媒体或APP媒体ID、广告位ID对象List（自媒体没有广告位可不填）
            /// </summary>           
            public List<JSONAPP> IDs { get; set; }

            public bool CheckSelfModel(out string msg)
            {
                StringBuilder sb = new StringBuilder();
                msg = "";
                if (!Enum.IsDefined(typeof(Entities.EnumJSONCartOptType), OptType))
                {
                    sb.Append("操作类型参数错误!");
                }
                
                if (!Enum.IsDefined(typeof(Entities.EnumMediaType), MediaType))
                {
                    sb.Append("媒体类型不存在!");
                }
                if (Entities.EnumJSONCartOptType.ClearAll != OptType)
                {
                    if (IDs == null || IDs.Count == 0)
                    {
                        sb.Append("媒体ID、广告位ID对象数组不能为空!");
                    }
                    else
                    {
                        if (IDs.Count > 50)
                        {
                            sb.Append("媒体ID广告位ID对象数量不能超过50!");
                        }
                        else
                        {
                            foreach (JSONAPP item in IDs)
                            {
                                if (item.MediaID == 0)
                                {
                                    sb.Append("媒体ID不存在为0的!");
                                }
                            }
                        }
                    }
                }
                
                msg = sb.ToString();
                return msg.Length.Equals(0);
            }
        }
        public class JSONAPP 
        {
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
        }
        #endregion
        #region 点击投放时，把当前购物车里面的内容添加到后台缓存实体
        public class JSONoptdelivery
        {
            public int MediaType { get; set; }
            public List<JSONID> IDs { get; set; }
            public bool CheckSelfModel(out string msg)
            {
                StringBuilder sb = new StringBuilder();
                msg = "";                

                if (!Enum.IsDefined(typeof(Entities.EnumMediaType), MediaType))
                {
                    sb.Append("媒体类型不存在!");
                }
                if (IDs == null || IDs.Count == 0)
                {
                    sb.Append("媒体ID、广告位ID对象数组不能为空!");
                }
                else
                {
                    if (IDs.Count > 50)
                    {
                        sb.Append("媒体ID广告位ID对象数量不能超过50!");
                    }
                    else
                    {
                        foreach (JSONID item in IDs)
                        {
                            if (item.MediaID == 0)
                            {
                                sb.Append("媒体ID不能为0!");
                            }

                            if (MediaType == (int)Entities.EnumMediaType.APP)
                            {
                                if (item.PublishDetailID == 0)
                                {
                                    sb.Append("APP广告位ID不能为0!");
                                }
                            }
                        }
                    }
                }
                msg = sb.ToString();
                return msg.Length.Equals(0);
            }
        }

        public class JSONID
        {
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
            public bool IsSelected { get; set; }
        }
        #endregion        
        #region 自媒体添加广告位实体
        public class JSONself
        {
            public int MediaID { get; set; }
            public string PublishDetailID { get; set; }
        }
        public class JSONselfmedia
        {
            public int MediaType { get; set; }
            public List<JSONself> IDs { get; set; }
            
        }
        #endregion
        #region 获取当前购物车里面的内容实体
        public class JSONcartinfo
        {
            public int MediaType { get; set; }
            public decimal TotalAmount { get; set; }
            public List<JSONSelfMedia2> SelfMedia { get; set; }
            public List<JSONAPPMedia> APP { get; set; }
        }

        public class JSONAPPMedia
        {
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
            public string Name { get; set; }
            public string AdPosition { get; set; }
            public string AdForm { get; set; }
            public string Style { get; set; }
            public int CarouselCount { get; set; }
            public string PlayPosition { get; set; }
            public string SysPlatform { get; set; }
            public int BeginPlayDays { get; set; }
            public decimal Price { get; set; }
            public decimal Amount { get; set; }
            public int CPDCPM { get; set; }
            public byte IsSelected { get; set; }
            public int PublishStatus { get; set; }
            public int expired { get; set; }
        }
        public class JSONSelfMedia2
        {
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
            public string Number { get; set; }
            public string Sign { get; set; }
            public string Name { get; set; }
            public string HeadIconURL { get; set; }
            public string ADTypeName { get; set; }
            public decimal Price { get; set; }
            public byte IsSelected { get; set; }
            public int PublishStatus { get; set; }
            public int expired { get; set; }
        }
        #endregion
    }

   
}
