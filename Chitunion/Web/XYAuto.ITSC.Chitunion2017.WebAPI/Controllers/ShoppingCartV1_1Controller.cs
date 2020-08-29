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
using XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ShoppingCartV1_1Controller : ApiController
    {        
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }

        }
        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #region 添加购物车
        /// <summary>
        /// 添加购物车
        /// </summary>     
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddShoppingCart([FromBody]JSONAddCart jsoncart)
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
                    vmsg = BLL.CartInfo.Instance.InsertV1_1_8(cartmodel,out cartid);
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
        #region APP添加购物车
        /// <summary>
        /// 添加购物车
        /// </summary>     
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult AddShoppingCartAPP([FromBody]RequestAPPAddShoppingDto reqDto)
        {
            /**
             * 广告位定义：广告位ID+售卖区域ID
             * 1排期时间跨度要在所属刊例执行周期内
             * 2排期时间跨度不能超过6个月
             * 3相同广告位排期时间不能有交差            
             * **/
            #region 参数校验
            string vmsg = string.Empty;
            if (!reqDto.CheckSelfModel(out vmsg))
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCartAPP 验证出错：" + vmsg);
                return Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion

            try
            {                                
                foreach (RequestAPPAddShppingIDDto reqID in reqDto.IDs)
                {
                    foreach (RequestAPPAddShppingADScheduleDto reqSchedule in reqID.ADSchedule)
                    {
                        Entities.CartInfo cartEntity = new Entities.CartInfo() {
                            MediaType=reqDto.MediaType,
                            MediaID=reqID.MediaID,
                            PubDetailID=reqID.PublishDetailID,
                            SaleArea=reqID.SaleAreaID,
                            BeginData=reqSchedule.BeginData,
                            EndData=reqSchedule.EndData,
                            ADLaunchDays=reqID.ADLaunchDays,
                            IsSelected=1,
                            CreateUserID=currentUserID                         
                        };

                        int cartid;
                        vmsg = CartInfo.Instance.InsertAPPV1_1_8(cartEntity, out cartid);
                        if (!string.IsNullOrEmpty(vmsg))
                        {
                            Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCartAPP 验证出错：" + vmsg);
                            return Common.Util.GetJsonDataByResult(null, vmsg, -1);
                        }
                    }
                }
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCartAPP end******");
                return Common.Util.GetJsonDataByResult(null, "操作成功");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******AddShoppingCartAPP 出错：" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }
                                    
        }
        #endregion
        #region 删除购物车
        [HttpGet]
        [ApiLog]
        public JsonResult DeleteShoppingCart(string cartids)
        {
            #region 参数校验           
            if (string.IsNullOrEmpty(cartids))
            {
                return Common.Util.GetJsonDataByResult(null, "购物车ID串不能为空", -1);
            }
            #endregion

            try
            {
                //根据购物车Cartid删除广告位记录、删除排期信息
                CartInfo.Instance.DeleteV1_1(currentUserID, cartids);
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******DeleteShoppingCart end******");
                return Common.Util.GetJsonDataByResult(null, "操作成功");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******DeleteShoppingCart 出错：" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }            
        }
        #endregion
        #region 购物车投放
        [HttpPost]
        [ApiLog]
        public JsonResult Delivery_ShoppingCart([FromBody]RequestDeliveryShoppingDto reqDto)
        {
            #region 参数校验
            string vmsg = string.Empty;
            if (!reqDto.CheckSelfModel(out vmsg))
            {
                return Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion
            try
            {
                #region 修改购物车投放信息
                foreach (RequestDeliveryShoppingIDDto reqID in reqDto.IDs)
                {
                    Entities.CartInfo cartEntity = new Entities.CartInfo() {
                        MediaType=reqID.MediaType,
                        CartID=reqID.CartID,
                        MediaID=reqID.MediaID,
                        PubDetailID=reqID.PublishDetailID,
                        SaleArea=reqID.SaleAreaID,
                        ADLaunchDays=reqID.ADLaunchDays,
                        IsSelected=reqID.IsSelected

                    };
                    vmsg = CartInfo.Instance.Delivery(cartEntity);
                    if(!string.IsNullOrEmpty(vmsg))
                        return Common.Util.GetJsonDataByResult(null, vmsg, -1);
                }
                #endregion
                #region 添加到待审项目
                if (!string.IsNullOrEmpty(reqDto.OrderID))
                {
                    if (reqDto.IDs.Where(p => p.IsSelected == 1).Count() == 0)
                        return Common.Util.GetJsonDataByResult(null, "请至少选择一个广告位添加到待审项目!", -1);

                    RequestADOrderDto resADOrder;
                    List<RequestMediaOrderInfoDto> resMOIList;
                    List<RequestADDetailDto> resADDetailsList;
                    ADOrderInfo.Instance.QueryAEOrderInfo(reqDto.OrderID, out resADOrder, out resMOIList, out resADDetailsList);
                    try
                    {
                        //加密
                        resADOrder.CustomerID = XYAuto.Utils.Security.DESEncryptor.Encrypt(resADOrder.CustomerIDINT.ToString(), LoginPwdKey);
                        //编码
                        resADOrder.CustomerID = System.Web.HttpUtility.UrlEncode(resADOrder.CustomerID, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Loger.Log4Net.Info("[Delivery_ShoppingCart]QueryAEOrderInfo 客户ID加密编码出错：->" + ex.Message);
                    }

                    //更新项目逻辑
                    //1拿到项目
                    if (resADOrder == null)
                        return Common.Util.GetJsonDataByResult(null, "更新项目失败,项目号:" + resADOrder.OrderID + "不存在!", -1);


                    //2拿到广告位
                    if (resADDetailsList.Count == 0)
                        return Common.Util.GetJsonDataByResult(null, "没有广告位,项目号:" + resADOrder.OrderID, -1);

                    foreach (RequestADDetailDto rowADDetailInfo in resADDetailsList)
                    {
                        rowADDetailInfo.ADScheduleInfos = ADScheduleInfo.Instance.QueryByADDetailID(rowADDetailInfo.RecID);
                    }

                    //3拿购物车中广告位
                    foreach (RequestDeliveryShoppingIDDto reqID in reqDto.IDs)
                    {
                        if (reqID.IsSelected == 1)
                        {
                            //排期查询
                            List<RequestADScheduleInfoDto> adSheduleList = CartScheduleInfo.Instance.QueryByCartID(reqID.CartID);

                            resADDetailsList.Add(new RequestADDetailDto()
                            {
                                CartID=reqID.CartID,
                                MediaType = reqID.MediaType,
                                MediaID = reqID.MediaID,
                                PubDetailID = reqID.PublishDetailID,
                                SaleAreaID=reqID.SaleAreaID,
                                ADScheduleInfos = adSheduleList
                            });
                        }
                    }

                    #region 合成后总的广告位不能重复(修改项目中验证)
                    //微信验证
                    //#region 按排期拆单前逻辑
                    ////var queryWeChat = resADDetailsList.Where(l => l.MediaType == 14001);
                    ////if (queryWeChat.GroupBy(l => new { l.MediaID, l.PubDetailID }).Where(g => g.Count() > 1).Count() > 0)
                    ////{
                    ////    IEnumerable<IGrouping<int, RequestADDetailDto>> query = queryWeChat.GroupBy(x => x.PubDetailID).Where(g => g.Count() > 1);
                    ////    foreach (IGrouping<int, RequestADDetailDto> info in query)
                    ////    {
                    ////        vmsg += info.Key + ",";
                    ////    }
                    ////    if (vmsg.Contains(","))
                    ////        vmsg = vmsg.Substring(0, vmsg.Length - 1);

                    ////    Loger.Log4Net.Info("[ShoppingCartV1_1Controller]*****Delivery_ShoppingCart广告位重复vmsg->" + vmsg + "******");
                    ////    return Common.Util.GetJsonDataByResult(null, "广告位ID重复：" + vmsg + "!\n", -1);
                    ////}
                    //#endregion

                    //var queryWeChat = resADDetailsList.Where(x => x.MediaType == 14001);
                    //var itemGroupsWeChat = queryWeChat.GroupBy(x => x.PubDetailID);
                    //foreach (var itemGroup in itemGroupsWeChat)
                    //{
                    //    if (itemGroup.Count() > 3)
                    //        vmsg += "微信同一广告位最多投放3个!\n";

                    //    List<RequestADScheduleInfoDto> allADScheduleInfos = new List<RequestADScheduleInfoDto>();
                    //    foreach (var item in itemGroup)
                    //    {
                    //        if (item.ADScheduleInfos == null || item.ADScheduleInfos.Count == 0 || item.ADScheduleInfos.Count > 1)
                    //        {
                    //            vmsg += "广告位必须且只能有1个排期!\n";
                    //        }
                    //        allADScheduleInfos.AddRange(item.ADScheduleInfos);
                    //    }
                    //    if (allADScheduleInfos.GroupBy(l => l.BeginData.Date).Where(g => g.Count() > 1).Count() > 0)
                    //    {
                    //        vmsg += "排期精确到天不能重复!\n";
                    //    }
                    //}
                    //#region APP广告位验证
                    //var queryAPP = resADDetailsList.Where(x => x.MediaType == 14002);
                    ///**
                    // * 广告位定义：广告位ID+区域ID
                    // * 1广告位自己的排期不可以有交集
                    // * 2不同广告位所属排期不可以有交集                   
                    //**/
                    //var itemGroups = queryAPP.GroupBy(l => l.PubDetailID);
                    //foreach (var itemGroup in itemGroups)
                    //{
                    //    var queryGroup2 = itemGroup.GroupBy(x => x.SaleAreaID);
                    //    foreach (var itemGroup2 in queryGroup2)
                    //    {
                    //        foreach (var item in itemGroup2)
                    //        {
                    //            if (item.ADScheduleInfos.Count > 1)
                    //            {
                    //                foreach (var itemADS in item.ADScheduleInfos)
                    //                {
                    //                    var queryADS = from t in item.ADScheduleInfos
                    //                                   where t.BeginData != itemADS.BeginData && t.EndData != itemADS.EndData
                    //                                    && ((Convert.ToDateTime(itemADS.BeginData.ToShortDateString()) >= Convert.ToDateTime(t.BeginData.ToShortDateString()) && Convert.ToDateTime(itemADS.BeginData.ToShortDateString()) <= Convert.ToDateTime(t.EndData.ToShortDateString()))
                    //                                        ||
                    //                                        (Convert.ToDateTime(itemADS.BeginData.ToShortDateString()) <= Convert.ToDateTime(t.BeginData.ToShortDateString()) && Convert.ToDateTime(itemADS.BeginData.ToShortDateString()) >= Convert.ToDateTime(t.EndData.ToShortDateString()))
                    //                                        ||
                    //                                        (Convert.ToDateTime(itemADS.BeginData.ToShortDateString()) <= Convert.ToDateTime(t.BeginData.ToShortDateString()) && Convert.ToDateTime(itemADS.EndData.ToShortDateString()) >= Convert.ToDateTime(t.BeginData.ToShortDateString()))
                    //                                        ||
                    //                                        (Convert.ToDateTime(itemADS.BeginData.ToShortDateString()) <= Convert.ToDateTime(t.EndData.ToShortDateString()) && Convert.ToDateTime(itemADS.EndData.ToShortDateString()) >= Convert.ToDateTime(t.EndData.ToShortDateString()))
                    //                                       )
                    //                                   select t;
                    //                    if (queryADS.Count() > 0)
                    //                    {
                    //                        return Common.Util.GetJsonDataByResult(null, "APP广告位排期有重复!", -1);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //#endregion
                    #endregion

                    //4项目中的广告位和购物车选中广告位合成
                    RequestADOrderInfoDto resADOrderDto = new RequestADOrderInfoDto()
                    {
                        ADOrderInfo = resADOrder,
                        MediaOrderInfos = resMOIList,
                        ADDetails = resADDetailsList,
                        optType = 2
                    };
                    #region 生成项目媒体空记录
                    //var queryADDetails = from x in resADDetailsList
                    //                     select x.MediaType;
                    //var detailMediaType = queryADDetails.Distinct();
                    //var mediaOrderMediaType = from x in resMOIList
                    //                          select x.MediaType;
                    //var exceptMediaType = detailMediaType.Except(mediaOrderMediaType.Distinct());
                    //foreach (var item in exceptMediaType)
                    //{
                    //    resMOIList.Add(new RequestMediaOrderInfoDto()
                    //    {
                    //        MediaType = item
                    //    });
                    //}
                    #endregion
                    string orderid = string.Empty;
                    ADOrderInfo.Instance.AddOrderInfo(resADOrderDto, out orderid, out vmsg, true);
                    if (!string.IsNullOrEmpty(vmsg))
                    {
                        return Common.Util.GetJsonDataByResult(null, vmsg, -1);
                    }
                }
                #endregion
                return Common.Util.GetJsonDataByResult(null, "操作成功");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******Delivery_ShoppingCart 出错errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }
        }
        
        #endregion
        #region 查询购物车        
        [HttpGet]
        [ApiLog]
        public JsonResult GetInfo_ShoppingCart()
        {
            try
            {
                ResponseQueryShoppingDto responseDto = new ResponseQueryShoppingDto();
                responseDto.SelfMedia = new List<ResponseQuerySelfMediaDto>();
                responseDto.APP = new List<ResponseQueryAPPMediaDto>();

                #region 微信                 
                List<ResponseQuerySelfItemDto> mediaList = CartInfo.Instance.ADCartInfoDetailWeChat_Select(currentUserID);
               
                if (mediaList.Count > 0)
                {
                    var query = mediaList.GroupBy(x => x.Source);
                    foreach (var queryitem in query)
                    {
                        ResponseQuerySelfMediaDto selfMediaEntity = new ResponseQuerySelfMediaDto()
                        {
                            MediaOwner = queryitem.Key,
                            Medias = queryitem.ToList()
                        };
                        foreach (var mediaitem in selfMediaEntity.Medias)
                        {
                            //广告位失效、下架、过期 则不计算金额
                            if (mediaitem.expired == 0 && (mediaitem.PublishStatus == (int)Entities.Enum.AppPublishStatus.已上架
                                                            || mediaitem.PublishStatus == (int)Entities.Enum.PublishStatusEnum.已上架)
                                )
                                responseDto.TotalAmount += mediaitem.TotalAmmount;

                            mediaitem.ADSchedule = CartInfo.Instance.ADCartScheduleInfoWeChat_Select<ResponseQuerySelfADScheduleDto>(mediaitem.CartID);
                        }
                        responseDto.SelfMedia.Add(selfMediaEntity);
                    }
                }
                #endregion
                #region APP                
                List<ResponseQueryAPPItemDto> appMediaList = CartInfo.Instance.p_ADCartInfoDetailAPP_Select(currentUserID);
                
                var queryAPP = appMediaList.GroupBy(x => x.Source);
                foreach (var queryitem in queryAPP)
                {
                    ResponseQueryAPPMediaDto appEntity = new ResponseQueryAPPMediaDto()
                    {
                        MediaOwner = queryitem.Key,
                        Medias = queryitem.ToList()
                    };
                    foreach (var mediaitem in appEntity.Medias)
                    {
                        //广告位失效、下架、过期 则不计算金额
                        if (mediaitem.expired == 0 && (mediaitem.PublishStatus == (int)Entities.Enum.AppPublishStatus.已上架
                                                        || mediaitem.PublishStatus == (int)Entities.Enum.PublishStatusEnum.已上架)
                            )
                            responseDto.TotalAmount += mediaitem.TotalAmmount;

                        mediaitem.ADSchedule = CartInfo.Instance.ADCartScheduleInfoWeChat_Select<ResponseQueryAPPADScheduleDto>(mediaitem.CartID);
                        foreach (var adScheduleItem in mediaitem.ADSchedule)
                        {
                            List<ResponseQueryHolidayDto> listHoliday = new List<ResponseQueryHolidayDto>();
                            adScheduleItem.Holidays = CartInfo.Instance.p_CALHolidays(adScheduleItem.BeginData, adScheduleItem.EndData, out listHoliday);
                            adScheduleItem.Holiday = listHoliday;
                        }
                    }                    
                    responseDto.APP.Add(appEntity);
                }
                #endregion
                
                if(mediaList.Count==0 && appMediaList.Count==0)
                    return Common.Util.GetJsonDataByResult(null, "购物车时没有任何信息", -1);

                return Common.Util.GetJsonDataByResult(responseDto, "操作成功");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******GetInfo_ShoppingCart,errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }

        }
        #endregion
        #region 添加修改删除排期
        [HttpPost]
        [ApiLog]
        public JsonResult ADScheduleOpt_ShoppingCart([FromBody]RequestADScheduleOptDto requestDto)
        {
            #region 参数校验
            string vmsg = string.Empty;
            if (!requestDto.CheckSelfModel(out vmsg))
                return Common.Util.GetJsonDataByResult(null, vmsg, -1);

            #endregion

            try
            {
                int RecIDNew = 0;
                if(requestDto.MediaType==14001)
                    vmsg = BLL.CartInfo.Instance.CartScheduleInfo_OperV1_1_8(requestDto.OptType, requestDto.CartID, requestDto.RecID, requestDto.BeginTime, currentUserID,out RecIDNew);
                else
                    vmsg = BLL.CartInfo.Instance.CartScheduleInfoAPP_OperV1_1_8(requestDto.OptType, requestDto.CartID, requestDto.RecID, requestDto.BeginTime, requestDto.EndTime,currentUserID, out RecIDNew);

                if (!string.IsNullOrEmpty(vmsg))
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);


                if (requestDto.OptType == 1)
                    return WebAPI.Common.Util.GetJsonDataByResult(null, RecIDNew.ToString());
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
        [ApiLog]
        public JsonResult OrderIDName_FuzzyQuery()
        {
            try
            {
                //业务逻辑
                string vmsg = string.Empty;
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                CartInfo.Instance.OrderIDName_SelectV1_1_8(currentUserID, out vmsg, out dicList);
                if (!string.IsNullOrEmpty(vmsg))
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);

                if (dicList?.Count > 0)
                {
                    return Common.Util.GetJsonDataByResult(dicList, "操作成功");
                }
                else
                {
                    return Common.Util.GetJsonDataByResult(null, "没有数据");
                }                                                           
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******OrderIDName_FuzzyQuery 出错：" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }
        #endregion
        #region 购物车中当前选择的广告位在项目中是否已存在
        [HttpPost]
        [ApiLog]
        public JsonResult PubDetailVertify_ADOrderOrCart([FromBody]RequestPubDetailVertifyDto requestDto)
        {
            Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******PubDetailVertify_ADOrderOrCart OrderID->" + requestDto.OrderID);            
            #region 参数校验
            string vmsg = string.Empty;
            if (string.IsNullOrEmpty(requestDto.OrderID))
                vmsg = "项目号不能为空!";           
            
            if(!string.IsNullOrEmpty(vmsg))
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);

            #endregion

            try
            {
                //业务逻辑
                foreach (RequestMediaDto reqMedia in requestDto.Media)
                {
                    vmsg = CartInfo.Instance.p_PubDetailVertify_ADOrderOrCartV1_1_8(requestDto.OrderID, reqMedia.CartID, reqMedia.MediaType, reqMedia.PublishDetailID, reqMedia.SaleAreaID);
                    if (!string.IsNullOrEmpty(vmsg))
                        return Common.Util.GetJsonDataByResult(null, vmsg, -1);

                }
                return Common.Util.GetJsonDataByResult(null, "没有冲突的广告位");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ShoppingCartV1_1Controller]******PubDetailVertify_ADOrderOrCart 出错：" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }
        #endregion
        #region 查询一段时间范围内的节假是天数及详情
        [HttpGet]
        [ApiLog]
        public JsonResult QueryHolidays(DateTime beginDate, DateTime endDate)
        {
            try
            {
                List<ResponseQueryHolidayDto> listHoliday = new List<ResponseQueryHolidayDto>();
                int holidays = CartInfo.Instance.p_CALHolidays(beginDate, endDate, out listHoliday);
                Dictionary<string, object> dicObj = new Dictionary<string, object>();
                dicObj.Add("Days", holidays);
                dicObj.Add("Holiday", listHoliday);
                
                return Common.Util.GetJsonDataByResult(dicObj, "操作成功");
            }
            catch (Exception ex)
            {
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
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
        
    }
}
