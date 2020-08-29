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
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ShoppingCartV1_1BAKController : ApiController
    {
        int currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();

        #region 添加购物车
        /// <summary>
        /// 添加购物车
        /// </summary>     
        /// <returns></returns>
        [HttpPost]
        public Common.JsonResult AddShoppingCart([FromBody]JSONAddCart jsoncart)
        {
            /**
             * 1购物车中最多3个广告位
             * 2每个广告位最多3个排期
             * 3广告位排期 天不能重复
             * **/
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCart begin******");
            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(jsoncart);
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]*****AddShoppingCart->" + listd + "******");
            }
            catch (Exception ex)
            { }
            #region 参数校验
            string vmsg = string.Empty;
            if (!jsoncart.CheckSelfModel(out vmsg))
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCart 验证出错：" + vmsg);
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion

            try
            {

                foreach (JSONAPP app in jsoncart.IDs)
                {
                    Entities.CartInfo cartmodel = new Entities.CartInfo();
                    cartmodel.MediaType = jsoncart.MediaType;
                    cartmodel.MediaID = app.MediaID;
                    cartmodel.PubDetailID = app.PublishDetailID;
                    cartmodel.IsSelected = 1;
                    cartmodel.CreateUserID = currentUserID;
                    cartmodel.ADSchedule = app.ADSchedule;
                    int cartid;
                    vmsg = BLL.CartInfo.Instance.InsertV1_1(cartmodel,out cartid);
                    if (!string.IsNullOrEmpty(vmsg))
                    {
                        BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCart 验证出错：" + vmsg);
                        return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                    }                    
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCart 出错：" + ex.Message);
                vmsg = "出错：" + ex.Message;
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCart end******");
            return WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功");
        }                
        #endregion
        #region 删除购物车
        [HttpGet]
        public Common.JsonResult DeleteShoppingCart(int mediatype,string cartids)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******DeleteShoppingCart begin******");
            #region 参数校验
            if (mediatype!=14001)
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, "媒体类型不正确", -1);
            }
            if (string.IsNullOrEmpty(cartids))
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, "购物车ID串不能为空", -1);
            }
            #endregion

            try
            {
                //根据购物车Cartid删除广告位记录、删除排期信息
                string vmsg = string.Empty;                
                //vmsg = BLL.CartInfo.Instance.DeleteV1_1(mediatype, currentUserID, cartids);
                if (!string.IsNullOrEmpty(vmsg))
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******DeleteShoppingCart 出错：" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******DeleteShoppingCart end******");
            return WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功");
        }
        #endregion
        #region 点击投放时，把当前购物车里面的内容添加到后台缓存
        [HttpPost]
        public Common.JsonResult Delivery_ShoppingCart([FromBody]JSONoptdelivery josnde)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******Delivery_ShoppingCart begin******");
            #region 参数校验
            string vmsg = string.Empty;
            if (!josnde.CheckSelfModel(out vmsg))
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion
            try
            {
                #region 修改购物车投放信息
                foreach (JSONID id in josnde.IDs)
                {
                    Entities.CartInfo cartmodel = new Entities.CartInfo();
                    cartmodel.CartID = id.CartID;
                    cartmodel.MediaType = josnde.MediaType;
                    cartmodel.MediaID = id.MediaID;
                    cartmodel.PubDetailID = id.PublishDetailID;
                    cartmodel.IsSelected = id.IsSelected;

                    BLL.CartInfo.Instance.Update(cartmodel);
                }
                #endregion
                if (!string.IsNullOrEmpty(josnde.OrderID))
                {                    
                    #region 添加到待审项目
                    Entities.ADOrderInfo order = new Entities.ADOrderInfo();
                    order = BLL.ADOrderInfo.Instance.GetADOrderInfo(josnde.OrderID);
                    if (order == null)
                    {
                        return WebAPI.Common.Util.GetJsonDataByResult(null, "更新项目失败,项目号:" + josnde.OrderID + "不存在!", -1);
                    }
                    //更新项目逻辑
                    //1拿到项目
                    ADOrderInfoV1_1BAKController.JSONADOrderInfo orderInfo = new ADOrderInfoV1_1BAKController.JSONADOrderInfo() {
                        OrderID = order.OrderID,
                        MediaType = order.MediaType,
                        OrderName = order.OrderName,
                        Status = order.Status,
                        BeginTime = order.BeginTime,
                        EndTime = order.EndTime,
                        Note = order.Note,
                        UploadFileURL = order.UploadFileURL,
                        //CustomerID = ADOrderInfoV1_1BAKController.EncryptAndUrlEncode2CustomerID(order.CustomerID)
                    };
                    
                    //2拿到广告位
                    Entities.QueryADDetailInfo queryADDetailInfo = new Entities.QueryADDetailInfo() {
                        OrderID=order.OrderID
                    };
                    int icount = 0;
                    DataTable dtADDetailInfo = BLL.ADDetailInfo.Instance.GetADDetailInfo(queryADDetailInfo, " a.CreateTime DESC ", 1, 99999, out icount);
                    List<ADOrderInfoV1_1BAKController.JSONADDetailInfo> listDetail = new List<ADOrderInfoV1_1BAKController.JSONADDetailInfo>();

                    foreach (DataRow rowADDetailInfo in dtADDetailInfo.Rows)
                    {
                        Entities.ADDetailInfo entityADDetailInfo = BLL.ADDetailInfo.Instance.DataRowToModel(rowADDetailInfo);
                        //广告位排期
                        List<ADOrderInfoV1_1BAKController.JSONADScheduleInfo2> listADSchedule = new List<ADOrderInfoV1_1BAKController.JSONADScheduleInfo2>();
                        Entities.QueryADScheduleInfo query_ADScheduleInfo = new Entities.QueryADScheduleInfo() {
                            ADDetailID=entityADDetailInfo.RecID
                        };
                        DataTable dtADSchedule = BLL.ADScheduleInfo.Instance.GetADScheduleInfo(query_ADScheduleInfo, "a.BeginData asc", 1, 9999, out icount);
                        foreach (DataRow rowADSchedule in dtADSchedule.Rows)
                        {
                            Entities.ADScheduleInfo entityADSchedule = BLL.ADScheduleInfo.Instance.DataRowToModel(rowADSchedule);
                            listADSchedule.Add(new ADOrderInfoV1_1BAKController.JSONADScheduleInfo2() {
                                BeginData=entityADSchedule.BeginData,
                                EndData=entityADSchedule.EndData
                            });
                        }

                        listDetail.Add(new ADOrderInfoV1_1BAKController.JSONADDetailInfo() {
                            MediaID=entityADDetailInfo.MediaID,
                            PubDetailID=entityADDetailInfo.PubDetailID,
                            AdjustPrice=entityADDetailInfo.AdjustPrice,
                            AdjustDiscount=entityADDetailInfo.AdjustDiscount,
                            ADLaunchDays=entityADDetailInfo.ADLaunchDays,
                            ADScheduleInfos=listADSchedule
                        });
                    }

                    //3拿购物车中广告位
                    foreach (JSONID entityID in josnde.IDs)
                    {
                        if(entityID.IsSelected==1)
                        {
                            //排期查询
                            Entities.QueryCartScheduleInfo query_CartScheduleInfo = new Entities.QueryCartScheduleInfo()
                            {
                                CartID = entityID.CartID
                            };

                            List<ADOrderInfoV1_1BAKController.JSONADScheduleInfo2> listADSchedule = new List<ADOrderInfoV1_1BAKController.JSONADScheduleInfo2>();
                            DataTable dt_CartScheduleInfo = BLL.CartScheduleInfo.Instance.GetCartScheduleInfo(query_CartScheduleInfo, " a.BeginTime ASC ", 1, 9999, out icount);
                            foreach (DataRow row_CartScheduleInfo in dt_CartScheduleInfo.Rows)
                            {
                                Entities.CartScheduleInfo entityCartScheduleInfo = BLL.CartScheduleInfo.Instance.DataRowToModel(row_CartScheduleInfo);
                                listADSchedule.Add(new ADOrderInfoV1_1BAKController.JSONADScheduleInfo2()
                                {
                                    BeginData = entityCartScheduleInfo.BeginTime,
                                    EndData = Entities.Constants.Constant.DATE_INVALID_VALUE
                                });
                            }

                            listDetail.Add(new ADOrderInfoV1_1BAKController.JSONADDetailInfo()
                            {
                                MediaID = entityID.MediaID,
                                PubDetailID = entityID.PublishDetailID,                                
                                ADScheduleInfos = listADSchedule
                            });
                        }
                    }

                    //合成后总的广告位不能重复
                    if (listDetail.GroupBy(l => new { l.MediaID, l.PubDetailID }).Where(g => g.Count() > 1).Count() > 0)
                    {
                        IEnumerable<IGrouping<int, ADOrderInfoV1_1BAKController.JSONADDetailInfo>> query = listDetail.GroupBy(x => x.PubDetailID).Where(g => g.Count() > 1);
                        foreach (IGrouping<int, ADOrderInfoV1_1BAKController.JSONADDetailInfo> info in query)
                        {
                            vmsg += info.Key + ",";
                        }                       
                        if (vmsg.Contains(","))
                            vmsg = vmsg.Substring(0, vmsg.Length - 1);

                        BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]*****Delivery_ShoppingCart广告位重复vmsg->" + vmsg + "******");
                        return WebAPI.Common.Util.GetJsonDataByResult(null, "广告位ID重复：" + vmsg + "!\n", -1);
                    }
                    //4项目中的广告位和购物车选中广告位合成
                    //ADOrderInfoV1_1BAKController.ModifyOrderInfo(orderInfo, listDetail,out vmsg);
                    if (!string.IsNullOrEmpty(vmsg))
                    {
                        return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                    }
                    #endregion
                }                
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******Delivery_ShoppingCart 出错errormsg:" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******Delivery_ShoppingCart end******");
            return WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功", 0);
        }
        
        #endregion
        #region 获取当前购物车里面的内容
        [HttpGet]
        public Common.JsonResult GetInfo_ShoppingCart(int MediaType)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******GetInfo_ShoppingCart begin******");
            Common.JsonResult jr = new Common.JsonResult();
            jr.Message = "操作成功";

            //获取购物车信息
            System.Data.DataTable dt = BLL.CartInfo.Instance.ADCartInfoDetail_SelectV1_1(MediaType, currentUserID);
            if (dt == null || dt.Rows.Count == 0)
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, "购物车时没有任何信息", -1);
            }

            JSONcartinfo cart = new JSONcartinfo();
            cart.MediaType = MediaType;
            List<JSONAPPMedia> appList = new List<JSONAPPMedia>();
            List<JSONMediaOwner> selfList = new List<JSONMediaOwner>();


            try
            {
                if (MediaType == (int)Entities.EnumMediaType.APP)
                {
                    #region APP
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
                    #endregion
                }
                else
                {
                    cart.APP = null;
                    decimal totalamount = 0;                    
                    var query = from t in dt.AsEnumerable()
                                group t by new { t1 = t.Field<string>("Source") } into CartInfo
                                select new
                                {
                                    Source=CartInfo.Key.t1,
                                    Details=CartInfo
                                };

                    foreach (var detail in query)
                    {
                        //按媒体主维度分组
                        JSONMediaOwner self = new JSONMediaOwner();
                        self.Medias = new List<JSONSelfMedia2>();
                        self.MediaOwner = detail.Source;
                        List<DataRow> dataRows = detail.Details.ToList();
                        foreach (DataRow row in dataRows)
                        {
                            JSONSelfMedia2 s = new JSONSelfMedia2();
                            #region 赋值
                            if (row["CartID"] != null && row["CartID"].ToString() != "")
                            {
                                s.CartID = Convert.ToInt32(row["CartID"]);
                            }
                            if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                            {
                                s.MediaID = Convert.ToInt32(row["MediaID"]);
                            }
                            if (row["PubDetailID"] != null && row["PubDetailID"].ToString() != "")
                            {
                                s.PublishDetailID = Convert.ToInt32(row["PubDetailID"]);
                            }
                            if (row["IsSelected"] != null && row["IsSelected"].ToString() != "")
                            {
                                s.IsSelected = Convert.ToByte(row["IsSelected"]);
                            }
                            if (row["IsAuth"] != null && row["IsAuth"].ToString() != "")
                            {
                                s.IsAuth = row["IsAuth"].ToString();
                            }
                            if (row["ADMasterImage"] != null && row["ADMasterImage"].ToString() != "")
                            {
                                s.ADMasterImage = row["ADMasterImage"].ToString();
                            }
                            if (row["MediaStatus"] != null && row["MediaStatus"].ToString() != "")
                            {
                                s.MediaStatus = Convert.ToInt32(row["MediaStatus"]);
                            }
                            if (row["PublishStatus"] != null && row["PublishStatus"].ToString() != "")
                            {
                                s.PublishStatus = Convert.ToInt32(row["PublishStatus"]);
                            }
                            if (row["ADMasterTitle"] != null && row["ADMasterTitle"].ToString() != "")
                            {
                                s.ADMasterTitle = row["ADMasterTitle"].ToString();
                            }
                            if (row["PubBeginTime"] != null && row["PubBeginTime"].ToString() != "")
                            {
                                s.PubBeginTime = Convert.ToDateTime(row["PubBeginTime"]);
                            }
                            if (row["PubEndTime"] != null && row["PubEndTime"].ToString() != "")
                            {
                                s.PubEndTime = Convert.ToDateTime(row["PubEndTime"]);
                            }
                            if (row["expired"] != null && row["expired"].ToString() != "")
                            {
                                s.expired = Convert.ToInt32(row["expired"].ToString());
                            }
                            if (row["ADPosition"] != null && row["ADPosition"].ToString() != "")
                            {
                                s.ADPosition = row["ADPosition"].ToString();
                            }
                            if (row["CreateType"] != null && row["CreateType"].ToString() != "")
                            {
                                s.CreateType = row["CreateType"].ToString();
                            }
                            if (row["Price"] != null && row["Price"].ToString() != "")
                            {
                                s.Price = Convert.ToDecimal(row["Price"].ToString());
                            }
                            if (row["TotalAmmount"] != null && row["TotalAmmount"].ToString() != "")
                            {
                                s.TotalAmmount = Convert.ToDecimal(row["TotalAmmount"].ToString());
                            }
                            #endregion
                            #region 查询排期
                            DataTable dtt = BLL.CartInfo.Instance.ADCartScheduleInfo_Select(s.CartID);
                            List<JSONCartADSchedule> cartScheduleList = new List<JSONCartADSchedule>();
                            if (dtt != null && dtt.Rows.Count > 0)
                            {
                                foreach (DataRow cartRow in dtt.Rows)
                                {
                                    cartScheduleList.Add(
                                        new JSONCartADSchedule() {
                                            RecID = Convert.ToInt32(cartRow["RecID"]),
                                            BeginTime = Convert.ToDateTime(cartRow["BeginTime"])
                                        }
                                        );
                                }
                            }                            
                            s.ADSchedule = cartScheduleList;
                            #endregion
                            self.Medias.Add(s);
                            totalamount += s.Price;
                        }
                        selfList.Add(self);
                    }
                                                                              
                    cart.TotalAmount = totalamount;
                }
                cart.APP = appList;
                cart.SelfMedia = selfList;

                return WebAPI.Common.Util.GetJsonDataByResult(cart, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******GetInfo_ShoppingCart,errormsg:" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, "出错："+ ex.Message,-1);
            }
            
        }
        #endregion
        #region 添加修改删除排期
        [HttpPost]
        public Common.JsonResult ADScheduleOpt_ShoppingCart([FromBody]JSONADScheduleOpt jsoncart)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******ADScheduleOpt_ShoppingCart begin******");
            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(jsoncart);
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]*****ADScheduleOpt_ShoppingCart->" + listd + "******");
            }
            catch (Exception ex)
            { }
            #region 参数校验
            string vmsg = string.Empty;
            if (!jsoncart.CheckSelfModel(out vmsg))
            {
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion

            try
            {
                int RecIDNew = 0;
                vmsg = BLL.CartInfo.Instance.CartScheduleInfo_Oper(jsoncart.OptType, jsoncart.CartID,jsoncart.RecID, jsoncart.BeginTime, currentUserID,out RecIDNew);
                if (!string.IsNullOrEmpty(vmsg))
                {                    
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                }

                if (jsoncart.OptType == 1)
                { return WebAPI.Common.Util.GetJsonDataByResult(null, RecIDNew.ToString()); }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******ADScheduleOpt_ShoppingCart 出错：" + ex.Message);
                vmsg = "出错：" + ex.Message;
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******ADScheduleOpt_ShoppingCart end******");
            return WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功");
        }
        #endregion
        #region 根据媒体类型查询AE待审项目列表
        [HttpGet]
        public Common.JsonResult OrderIDName_FuzzyQuery(int MediaType)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******OrderIDName_FuzzyQuery begin******>" + MediaType);
            
            #region 参数校验
            string vmsg = string.Empty;
            //if (!jsoncart.CheckSelfModel(out vmsg))
            //{
            //    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            //}
            #endregion

            try
            {
                //业务逻辑
                DataTable dt = BLL.CartInfo.Instance.OrderIDName_Select(MediaType, currentUserID, out vmsg);
                if (!string.IsNullOrEmpty(vmsg))
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                }

                if (dt != null && dt.Rows.Count > 0)
                { }
                else
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", 0);
                }

                List<JSONOrderIDName> list_idname = new List<JSONOrderIDName>();
                foreach (DataRow row in dt.Rows)
                {
                    list_idname.Add(new JSONOrderIDName()
                    {
                        OrderID = Convert.ToString(row["OrderID"]),
                        OrderName = Convert.ToString(row["OrderName"]),
                    });
                }                                             
                return WebAPI.Common.Util.GetJsonDataByResult(list_idname, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******OrderIDName_FuzzyQuery 出错：" + ex.Message);
                vmsg = "出错：" + ex.Message;
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }

            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******OrderIDName_FuzzyQuery end******");            
        }
        #endregion
        #region 购物车中当前选择的广告位在项目中是否已存在
        [HttpGet]
        public Common.JsonResult PubDetailVertify_ADOrderOrCart(string OrderID,string PublishDetailIDs)
        {
            BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******PubDetailVertify_ADOrderOrCart OrderID->" + OrderID + ",PublishDetailIDs->" + PublishDetailIDs);

            #region 参数校验
            string vmsg = string.Empty;
            if (string.IsNullOrEmpty(OrderID))
                vmsg = "项目号不能为空!";

            if (string.IsNullOrEmpty(PublishDetailIDs))
                vmsg = "广告位ID串不能为空!";
            
            if(!string.IsNullOrEmpty(vmsg))
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);

            #endregion

            try
            {
                //业务逻辑
                vmsg = BLL.CartInfo.Instance.PubDetailVertify_ADOrderOrCart(OrderID, PublishDetailIDs);
                if (!string.IsNullOrEmpty(vmsg))
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                }
                
                return WebAPI.Common.Util.GetJsonDataByResult(null, "没有冲突的广告位");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******PubDetailVertify_ADOrderOrCart 出错：" + ex.Message);
                vmsg = "出错：" + ex.Message;
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
        }
        #endregion



        #region 添加购物车实体
        public class JSONAddCart
        {            
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
                        foreach (JSONAPP item in IDs)
                        {
                            if (item.MediaID == 0 || item.MediaID==-2)
                            {
                                sb.Append("媒体ID不存在!");
                                break;
                            }

                            if (item.PublishDetailID == 0 || item.PublishDetailID == -2)
                            {
                                sb.Append("广告位ID不存在!");
                                break;
                            }
                           

                            //if (item.ADSchedule.Contains(","))
                            //{
                            //    string[] arrayADSchedule = item.ADSchedule.Split(',');
                            //    if (arrayADSchedule.Length > 3)
                            //    {
                            //        sb.Append("广告位最多3个排期!");
                            //        break;
                            //    }
                            //    foreach (string arr in arrayADSchedule)
                            //    {
                            //        DateTime dttmp = new DateTime(1990, 1, 1);
                            //        if (!DateTime.TryParse(arr, out dttmp))
                            //        {
                            //            sb.Append("广告位排期转换日期出错!");
                            //            break;
                            //        }
                            //    }
                            //    if (arrayADSchedule.Length >= 2)
                            //    {
                            //        List<string> list_date = new List<string>();
                            //        for (int i = 0; i < arrayADSchedule.Length; i++)
                            //        {
                            //            list_date.Add(Convert.ToDateTime(arrayADSchedule[i]).ToShortDateString());
                            //        }

                            //        IEnumerable<IGrouping<string, string>> query = list_date.GroupBy(x=>x);
                            //        foreach (IGrouping<string, string> info in query)
                            //        {
                            //            List<string> lgroup = info.ToList<string>();//分组后的集合
                            //            if (lgroup.Count > 1)
                            //            {
                            //                sb.Append("选择的排期不能重复(精确到天)!");
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    DateTime dttmp = new DateTime(1990, 1, 1);
                            //    if (!DateTime.TryParse(item.ADSchedule, out dttmp))
                            //    {
                            //        sb.Append("广告位排期转换日期出错!");
                            //        break;
                            //    }
                            //}
                        }
                    }
                }

                /**
             * 1购物车中最多3个广告位
             * 2每个广告位最多3个排期
             * 3广告位排期 天不能重复
             * **/

                msg = sb.ToString();
                return msg.Length.Equals(0);
            }
        }
        public class JSONAPP
        {
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
            public DateTime ADSchedule { get; set; }
        }
        #endregion     
        #region 点击投放时，把当前购物车里面的内容添加到后台缓存实体
        public class JSONoptdelivery
        {
            public string OrderID { get; set; }
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
            public int CartID { get; set; }
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
            public byte IsSelected { get; set; }
        }
        #endregion                
        #region 获取当前购物车里面的内容实体
        public class JSONcartinfo
        {
            public int MediaType { get; set; }
            public decimal TotalAmount { get; set; }
            public List<JSONMediaOwner> SelfMedia { get; set; }
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
        public class JSONMediaOwner
        {
            public string MediaOwner { get; set; }
            public List<JSONSelfMedia2> Medias { get; set; }
        }
        public class JSONSelfMedia2
        {
            public int CartID { get; set; }
            public int MediaID { get; set; }
            public int PublishDetailID { get; set; }
            public string IsAuth { get; set; }
            public string ADMasterImage { get; set; }
            public string ADMasterTitle { get; set; }
            public string ADPosition { get; set; }
            public string CreateType { get; set; }
            public decimal Price { get; set; }
            public decimal TotalAmmount { get; set; }
            public byte IsSelected { get; set; }
            public int PublishStatus { get; set; }
            public int MediaStatus { get; set; }
            public int expired { get; set; }
            public DateTime PubBeginTime { get; set; }
            public DateTime PubEndTime { get; set; }
            public List<JSONCartADSchedule> ADSchedule { get; set; }
        }

        public class JSONCartADSchedule
        {
            public int RecID { get; set; }
            public DateTime BeginTime { get; set; }
        }
        #endregion
        #region 添加修改删除排实体
        public class JSONADScheduleOpt
        {
            public int OptType { get; set; }
            private int _CartID=-2;

            public int CartID
            {
                get { return _CartID; }
                set { _CartID = value; }
            }

            private int _RecID=-2;

            public int RecID
            {
                get { return _RecID; }
                set { _RecID = value; }
            }

            public DateTime BeginTime { get; set; }
            public bool CheckSelfModel(out string msg)
            {
                StringBuilder sb = new StringBuilder();
                msg = "";

                if (OptType!=1 && OptType!=2 && OptType!=3)
                {
                    sb.Append("参数操作类型OptType错误!");
                }
                

                msg = sb.ToString();
                return msg.Length.Equals(0);
            }
        }
        #endregion
        #region 模糊查询AE待审项目实体
        public class JSONOrderIDName
        {
            public string OrderID { get; set; }
            public string OrderName { get; set; }
        }
        #endregion
    }
}
