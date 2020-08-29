using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class CommonTestController : ApiController
    {
        int currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
        private string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");

        #region 测试用
        //public JSONQueryResultADOrerInfo Test([FromBody]JSONQueryResultADOrerInfo r)
        [HttpPost]
        public Common.JsonResult Test([FromBody] JSONAddOrUpadteRequest r)
        {
            return WebAPI.Common.Util.GetJsonDataByResult(r, "", 0);
        }
        #endregion
        #region 提交、修改主订单
        /// <summary>
        /// 提交修改订单，提交跟修改区别:就是对状态字段操作不同
        /// 修改不更新状态，提交设置状态为待审
        /// 提交（即新增）：状态有（草稿、待审核）
        /// 修改：不对状态做操作（状态按原先状态值传递即可）
        /// </summary>
        /// <param name="optType"></param>
        /// <param name="orderInfo"></param>
        /// <param name="subDetailInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult AddOrUpdate_ADOrderInfo([FromBody] JSONAddOrUpadteRequest r)
        {
            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo...JSONAddOrUpadteRequest->" + listd + "******");
            }
            catch (Exception ex)
            { }
            Entities.EnumAddModify optType = r.optType;
            BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo...begin...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType);
            //订单插入记录的初始状态是草稿
            //修改订单不影响订单状态            
            #region 参数校验
            string vmsg = string.Empty;
            if (!r.CheckSelfModel(out vmsg))
            {   
                //修改订单草稿状态下 没有广告位 要删除项目
                if (r.optType == Entities.EnumAddModify.Modify && r.ADOrderInfo.Status == (int)Entities.EnumOrderStatus.Draft && vmsg.Contains("没有广告位"))
                {                                        
                    return WebAPI.Common.Util.GetJsonDataByResult(null, BLL.ADOrderInfo.Instance.p_ADOrderAllInfo_Delete(r.ADOrderInfo.OrderID, r.ADOrderInfo.MediaType, true), -1);
                }
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            #endregion

            JSONADOrderInfo jorder = new JSONADOrderInfo();
            jorder = r.ADOrderInfo;
            List<JSONADDetailInfo2> jdetails = new List<JSONADDetailInfo2>();
            jdetails = r.ADDetails;
            Common.JsonResult jr = new Common.JsonResult();
            jr = WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功", 0);
            try
            {
                switch (optType)
                {
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.ADD:
                        string orderid = "";
                        AddOrderInfo(jorder, jdetails, out orderid);
                        jr = WebAPI.Common.Util.GetJsonDataByResult(null, orderid, 0);
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.Modify:
                        string msg = "";
                        ModifyOrderInfo(jorder, jdetails, out msg);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            BLL.Loger.Log4Net.Info("[CommonTestController]*****ModifyOrderInfo 出错errormsg:" + msg);
                            jr = WebAPI.Common.Util.GetJsonDataByResult(null, msg, -1);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo 出错errormsg:" + ex.Message);
                jr = WebAPI.Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }
            BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo...end...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType);
            return jr;
        }        
        public void AddOrderInfo(JSONADOrderInfo orderInfo, List<JSONADDetailInfo2> listDetail, out string orderid)
        {
            #region 生成主订单
            Entities.ADOrderInfo order = new Entities.ADOrderInfo();
            order.MediaType = orderInfo.MediaType;
            order.OrderName = orderInfo.OrderName;
            order.BeginTime = orderInfo.BeginTime;
            order.EndTime = orderInfo.EndTime;
            order.Note = orderInfo.Note;
            order.UploadFileURL = orderInfo.UploadFileURL;
            order.CreateTime = DateTime.Now;
            order.CreateUserID = currentUserID;
            order.Status = orderInfo.Status;
            try
            {
                //解码
                orderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(orderInfo.CustomerID, Encoding.UTF8);
                //解密
                order.CustomerID = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(orderInfo.CustomerID, LoginPwdKey));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrderInfo 客户ID解码解密出错：->" + ex.Message);
            }

            orderid = BLL.ADOrderInfo.Instance.Insert(order);
            BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成订单：" + orderid);
            if (!string.IsNullOrEmpty(orderInfo.UploadFileURL))
            {
                Entities.ADOrderInfo modelnew = BLL.ADOrderInfo.Instance.GetADOrderInfo(orderid);
                if (modelnew != null)
                {
                    List<string> listurl = new List<string>();
                    listurl.Add(orderInfo.UploadFileURL);
                    BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(listurl, currentUserID, Entities.Enum.UploadFileEnum.OrderManage, modelnew.RecID, "ADOrderInfo");
                }
            }
            #endregion

            #region 遍历广告位 按媒体ID分组
            //自媒体记录MediaID串，APP记录PublishDetailID
            string cartids = "";
            //子订单定义：同一个媒体的所有广告位归类位一个子订单          
            IEnumerable<IGrouping<int, JSONADDetailInfo2>> query = listDetail.GroupBy(x => x.MediaID);
            decimal iprice = 0;//主订单金额
            foreach (IGrouping<int, JSONADDetailInfo2> info in query)
            {
                List<JSONADDetailInfo2> lgroup = info.ToList<JSONADDetailInfo2>();//分组后的集合

                decimal itotal = 0;
                int ipos = 0;
                string suborderid = "";
                foreach (JSONADDetailInfo2 item in lgroup)
                {
                    ipos++;

                    if (ipos == 1)
                    {
                        //生成子订单
                        Entities.SubADInfo submodel = new Entities.SubADInfo();
                        submodel.OrderID = orderid;
                        submodel.MediaType = orderInfo.MediaType;
                        submodel.MediaID = info.Key;
                        submodel.Status = orderInfo.Status;
                        submodel.CreateTime = DateTime.Now;
                        submodel.CreateUserID = currentUserID;

                        suborderid = BLL.SubADInfo.Instance.Insert(submodel);
                        BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成子订单：" + suborderid);
                    }

                    //生成广告位
                    Entities.ADDetailInfo demodel = new Entities.ADDetailInfo();
                    demodel.OrderID = orderid;
                    demodel.SubOrderID = suborderid;
                    demodel.MediaType = item.MediaType;
                    demodel.MediaID = item.MediaID;
                    demodel.AdjustPrice = item.AdjustPrice;
                    demodel.AdjustDiscount = item.AdjustDiscount;
                    demodel.ADLaunchDays = item.ADLaunchDays;
                    demodel.CreateUserID = currentUserID;
                    demodel.PubDetailID = item.PubDetailID;
                    #region 根据广告位ID、媒体类型获取刊例基础信息
                    System.Data.DataTable pubDetailDT = BLL.ADOrderInfo.Instance.p_GetPubDetailInfo_Select(item.MediaType, item.PubDetailID);
                    if (pubDetailDT != null && pubDetailDT.Rows.Count > 0)
                    {
                        if (pubDetailDT.Rows[0]["PubID"] != null && pubDetailDT.Rows[0]["PubID"].ToString() != "")
                        {
                            demodel.PubID = int.Parse(pubDetailDT.Rows[0]["PubID"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["OriginalPrice"] != null && pubDetailDT.Rows[0]["OriginalPrice"].ToString() != "")
                        {
                            demodel.OriginalPrice = decimal.Parse(pubDetailDT.Rows[0]["OriginalPrice"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["PurchaseDiscount"] != null && pubDetailDT.Rows[0]["PurchaseDiscount"].ToString() != "")
                        {
                            demodel.PurchaseDiscount = decimal.Parse(pubDetailDT.Rows[0]["PurchaseDiscount"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["SaleDiscount"] != null && pubDetailDT.Rows[0]["SaleDiscount"].ToString() != "")
                        {
                            demodel.SaleDiscount = decimal.Parse(pubDetailDT.Rows[0]["SaleDiscount"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["ADLaunchIDs"] != null && pubDetailDT.Rows[0]["ADLaunchIDs"].ToString() != "")
                        {
                            demodel.ADLaunchIDs = pubDetailDT.Rows[0]["ADLaunchIDs"].ToString();
                        }
                        if (pubDetailDT.Rows[0]["ADLaunchStr"] != null && pubDetailDT.Rows[0]["ADLaunchStr"].ToString() != "")
                        {
                            demodel.ADLaunchStr = pubDetailDT.Rows[0]["ADLaunchStr"].ToString();
                        }

                        //广告位价格=销售折扣*成本价
                        //demodel.OriginalPrice = demodel.OriginalPrice * demodel.SaleDiscount;
                    }
                    #endregion

                    #region 计算价格
                    //计算价格草稿、待审状态下 成交价=销售价*折扣
                    if (orderInfo.Status == (int)Entities.EnumOrderStatus.Draft || orderInfo.Status == (int)Entities.EnumOrderStatus.PendingAudit)
                    {
                        if (orderInfo.MediaType == (int)Entities.EnumMediaType.APP)
                        {
                            demodel.AdjustPrice = demodel.OriginalPrice * demodel.SaleDiscount * demodel.ADLaunchDays;
                        }
                        else
                        {
                            demodel.AdjustPrice = demodel.OriginalPrice * demodel.SaleDiscount;
                        }
                    }
                    itotal += demodel.AdjustPrice;
                    #endregion
                    if (ipos == lgroup.Count)
                    {
                        //更新子工单金额
                        BLL.SubADInfo.Instance.UpdateTotalAmmount_SubADInfo(itotal, suborderid);
                    }


                    int detailid = 0;
                    detailid = BLL.ADDetailInfo.Instance.Insert(demodel);
                    BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成广告位：" + detailid);

                    #region APP-CPD排期
                    //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                    if (item.MediaType == (int)Entities.EnumMediaType.APP)
                    {
                        cartids += item.PubDetailID + ",";
                        int cpd = 0;
                        if (int.TryParse(demodel.ADLaunchIDs, out cpd) && cpd == 11001)
                        {
                            //生成排期信息                                                    
                            if (item.ADScheduleInfos.Count > 0)
                            {
                                List<Entities.ADScheduleInfo> list_ads = new List<Entities.ADScheduleInfo>();
                                foreach (JSONADScheduleInfo2 sc in item.ADScheduleInfos)
                                {
                                    Entities.ADScheduleInfo ads = new Entities.ADScheduleInfo();
                                    ads.ADDetailID = detailid;
                                    ads.OrderID = orderid;
                                    ads.SubOrderID = suborderid;
                                    ads.MediaID = item.MediaID;
                                    ads.PubID = demodel.PubID;
                                    ads.CreateTime = DateTime.Now;
                                    ads.CreateUserID = currentUserID;
                                    ads.BeginData = sc.BeginData;
                                    ads.EndData = sc.EndData;
                                    list_ads.Add(ads);
                                }
                                System.Data.DataTable mydt = BLL.Util.ListToDataTable<Entities.ADScheduleInfo>(list_ads);
                                BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            }
                            //if (item.ADScheduleInfos.EndsWith(";"))
                            //    item.ADScheduleInfos = item.ADScheduleInfos.Substring(0, item.ADScheduleInfos.Length - 1);
                            //foreach (string str in item.ADScheduleInfos.Split(';'))
                            //{
                            //    List<Entities.ADScheduleInfo> list_ads = new List<Entities.ADScheduleInfo>();
                            //    string[] sc = str.Split(',');
                            //    Entities.ADScheduleInfo ads = new Entities.ADScheduleInfo();
                            //    ads.ADDetailID = detailid;
                            //    ads.OrderID = orderid;
                            //    ads.SubOrderID = suborderid;
                            //    ads.MediaID = item.MediaID;
                            //    ads.PubID = demodel.PubID;
                            //    ads.CreateTime = DateTime.Now;
                            //    ads.CreateUserID = currentUserID;
                            //    ads.BeginData = Convert.ToDateTime(sc[0]);
                            //    ads.EndData = Convert.ToDateTime(sc[1]);
                            //    list_ads.Add(ads);
                            //    System.Data.DataTable mydt = BLL.Util.ListToDataTable<Entities.ADScheduleInfo>(list_ads);
                            //    BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            //}
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrderInfo APP排期CPD转换int类型出错：->" + demodel.ADLaunchIDs);
                        }
                    }
                    else
                    {
                        //自媒体记录MediaID
                        cartids += item.MediaID + ",";
                    }
                    #endregion
                }
                //计算主订单金额
                iprice += itotal;
            }
            #endregion
            //更新主订单金额
            BLL.ADOrderInfo.Instance.UpdateTotalAmount_ADOrder(orderid, iprice);

            #region 清空购物车-当前订单相关的
            if (cartids.EndsWith(","))
            {
                cartids = cartids.Substring(0, cartids.Length - 1);
            }
            ClearCartInfo(orderInfo.MediaType, cartids);
            #endregion
        }
        public void ClearCartInfo(int mediaType, string cartids)
        {
            ShoppingCartController cart = new ShoppingCartController();
            cart.ClearAllCartInfo(mediaType, currentUserID, cartids);
        }
        public void ModifyOrderInfo(JSONADOrderInfo orderInfo, List<JSONADDetailInfo2> listDetail, out string msg)
        {
            BLL.Loger.Log4Net.Info("[CommonTestController]ModifyOrderInfo->订单号:" + orderInfo.OrderID + ",媒体类型:" + orderInfo.MediaType);
            msg = string.Empty;

            #region 查询并更新主订单
            Entities.ADOrderInfo order = new Entities.ADOrderInfo();
            order = BLL.ADOrderInfo.Instance.GetADOrderInfo(orderInfo.OrderID);
            if (order == null)
            {
                msg = "订单号：" + orderInfo.OrderID + "，不存在";
                return;
            }
            if (order.Status == (int)Entities.EnumOrderStatus.Draft || order.Status == (int)Entities.EnumOrderStatus.PendingAudit)
            {
                //可修改订单。
            }
            else
            {
                msg = "当前状态不允许修改订单";
                return;
            }
            //更新主订单
            order.MediaType = orderInfo.MediaType;
            order.OrderName = orderInfo.OrderName;
            order.BeginTime = orderInfo.BeginTime;
            order.EndTime = orderInfo.EndTime;
            order.Status = orderInfo.Status;
            order.Note = orderInfo.Note;
            try
            {
                //解码
                orderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(orderInfo.CustomerID, Encoding.UTF8);
                //解密
                order.CustomerID = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(orderInfo.CustomerID, LoginPwdKey));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CommonTestController]*****ModifyOrderInfo 客户ID解码解密出错：->" + ex.Message);
            }
            
            order.UploadFileURL = orderInfo.UploadFileURL;
            if (!string.IsNullOrEmpty(orderInfo.UploadFileURL))
            {
                List<string> listurl = new List<string>();
                listurl.Add(orderInfo.UploadFileURL);
                BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(listurl, currentUserID, Entities.Enum.UploadFileEnum.OrderManage, order.RecID, "ADOrderInfo");
            }

            string orderid = orderInfo.OrderID;
            BLL.ADOrderInfo.Instance.Update(order);
            #endregion
            
            //删除排期子订单广告位
            BLL.ADOrderInfo.Instance.p_ADOrderAllInfo_Delete(orderid, order.MediaType, false);

            
            #region 遍历广告位 按媒体ID分组
            //自媒体记录MediaID串，APP记录PublishDetailID
            string cartids = "";
            //子订单定义：同一个媒体的所有广告位归类位一个子订单            
            IEnumerable<IGrouping<int, JSONADDetailInfo2>> query = listDetail.GroupBy(x => x.MediaID);
            decimal iprice = 0;//主订单金额
            foreach (IGrouping<int, JSONADDetailInfo2> info in query)
            {
                List<JSONADDetailInfo2> lgroup = info.ToList<JSONADDetailInfo2>();//分组后的集合

                decimal itotal = 0;
                int ipos = 0;
                string suborderid = "";
                foreach (JSONADDetailInfo2 item in lgroup)
                {
                    ipos++;
                    //先生成子工单
                    if (ipos == 1)
                    {
                        //生成子订单
                        Entities.SubADInfo submodel = new Entities.SubADInfo();
                        submodel.OrderID = orderid;
                        submodel.MediaType = orderInfo.MediaType;
                        submodel.MediaID = info.Key;
                        submodel.Status = order.Status;
                        submodel.CreateTime = order.CreateTime;
                        submodel.CreateUserID = order.CreateUserID;

                        suborderid = BLL.SubADInfo.Instance.Insert(submodel);
                    }

                    //生成广告位
                    Entities.ADDetailInfo demodel = new Entities.ADDetailInfo();
                    demodel.OrderID = orderid;
                    demodel.SubOrderID = suborderid;
                    demodel.MediaType = item.MediaType;
                    demodel.MediaID = item.MediaID;
                    demodel.PubDetailID = item.PubDetailID;

                    #region 根据广告位ID、媒体类型获取刊例基础信息
                    System.Data.DataTable pubDetailDT = BLL.ADOrderInfo.Instance.p_GetPubDetailInfo_Select(item.MediaType, item.PubDetailID);
                    if (pubDetailDT != null && pubDetailDT.Rows.Count > 0)
                    {
                        if (pubDetailDT.Rows[0]["PubID"] != null && pubDetailDT.Rows[0]["PubID"].ToString() != "")
                        {
                            demodel.PubID = int.Parse(pubDetailDT.Rows[0]["PubID"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["OriginalPrice"] != null && pubDetailDT.Rows[0]["OriginalPrice"].ToString() != "")
                        {
                            demodel.OriginalPrice = decimal.Parse(pubDetailDT.Rows[0]["OriginalPrice"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["PurchaseDiscount"] != null && pubDetailDT.Rows[0]["PurchaseDiscount"].ToString() != "")
                        {
                            demodel.PurchaseDiscount = decimal.Parse(pubDetailDT.Rows[0]["PurchaseDiscount"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["SaleDiscount"] != null && pubDetailDT.Rows[0]["SaleDiscount"].ToString() != "")
                        {
                            demodel.SaleDiscount = decimal.Parse(pubDetailDT.Rows[0]["SaleDiscount"].ToString());
                        }
                        if (pubDetailDT.Rows[0]["ADLaunchIDs"] != null && pubDetailDT.Rows[0]["ADLaunchIDs"].ToString() != "")
                        {
                            demodel.ADLaunchIDs = pubDetailDT.Rows[0]["ADLaunchIDs"].ToString();
                        }
                        if (pubDetailDT.Rows[0]["ADLaunchStr"] != null && pubDetailDT.Rows[0]["ADLaunchStr"].ToString() != "")
                        {
                            demodel.ADLaunchStr = pubDetailDT.Rows[0]["ADLaunchStr"].ToString();
                        }

                        //广告位价格=销售折扣*成本价
                        //demodel.OriginalPrice = demodel.OriginalPrice * demodel.SaleDiscount;
                    }
                    #endregion

                    demodel.AdjustPrice = item.AdjustPrice;
                    demodel.AdjustDiscount = item.AdjustDiscount;
                    demodel.ADLaunchDays = item.ADLaunchDays;
                    demodel.CreateUserID = order.CreateUserID;


                    #region 计算价格
                    //计算价格草稿、待审状态下 成交价=销售价*折扣
                    if (orderInfo.Status == (int)Entities.EnumOrderStatus.Draft || orderInfo.Status == (int)Entities.EnumOrderStatus.PendingAudit)
                    {
                        if (orderInfo.MediaType == (int)Entities.EnumMediaType.APP)
                        {
                            demodel.AdjustPrice = demodel.OriginalPrice * demodel.SaleDiscount * demodel.ADLaunchDays;
                        }
                        else
                        {
                            demodel.AdjustPrice = demodel.OriginalPrice * demodel.SaleDiscount;
                        }
                    }
                    itotal += demodel.AdjustPrice;
                    #endregion
                    if (ipos == lgroup.Count)
                    {
                        //更新子工单金额
                        BLL.SubADInfo.Instance.UpdateTotalAmmount_SubADInfo(itotal, suborderid);
                    }

                    int detailid = 0;
                    detailid = BLL.ADDetailInfo.Instance.Insert(demodel);

                    #region APP-CPD排期
                    //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                    if (item.MediaType == (int)Entities.EnumMediaType.APP)
                    {
                        cartids += item.PubDetailID + ",";
                        int cpd = 0;
                        if (int.TryParse(demodel.ADLaunchIDs, out cpd) && cpd == 11001)
                        {
                            //生成排期信息                           
                            if (item.ADScheduleInfos.Count > 0)
                            {
                                List<Entities.ADScheduleInfo> list_ads = new List<Entities.ADScheduleInfo>();
                                foreach (JSONADScheduleInfo2 sc in item.ADScheduleInfos)
                                {
                                    Entities.ADScheduleInfo ads = new Entities.ADScheduleInfo();
                                    ads.ADDetailID = detailid;
                                    ads.OrderID = orderid;
                                    ads.SubOrderID = suborderid;
                                    ads.MediaID = item.MediaID;
                                    ads.PubID = demodel.PubID;
                                    ads.CreateTime = DateTime.Now;
                                    ads.CreateUserID = currentUserID;
                                    ads.BeginData = sc.BeginData;
                                    ads.EndData = sc.EndData;
                                    list_ads.Add(ads);
                                }
                                System.Data.DataTable mydt = BLL.Util.ListToDataTable<Entities.ADScheduleInfo>(list_ads);
                                BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            }

                            //foreach (string str in item.ADScheduleInfos.Split(';'))
                            //{
                            //    List<Entities.ADScheduleInfo> list_ads = new List<Entities.ADScheduleInfo>();
                            //    foreach (string sc in str.Split(','))
                            //    {
                            //        Entities.ADScheduleInfo ads = new Entities.ADScheduleInfo();
                            //        ads.ADDetailID = detailid;
                            //        ads.OrderID = orderid;
                            //        ads.SubOrderID = suborderid;
                            //        ads.MediaID = item.MediaID;
                            //        ads.PubID = demodel.PubID;
                            //        ads.CreateTime = DateTime.Now;
                            //        ads.CreateUserID = currentUserID;
                            //        ads.BeginData = Convert.ToDateTime(sc[0].ToString());
                            //        ads.EndData = Convert.ToDateTime(sc[1].ToString());
                            //        list_ads.Add(ads);
                            //    }
                            //    System.Data.DataTable mydt = BLL.Util.ListToDataTable<Entities.ADScheduleInfo>(list_ads);
                            //    BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            //}
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info("[CommonTestController]*****AddOrderInfo APP排期CPD转换int类型出错：->" + demodel.ADLaunchIDs);
                        }
                    }
                    else
                    {
                        cartids += item.MediaID + ",";
                    }
                    #endregion
                }
                //计算主订单金额
                iprice += itotal;
            }

            #endregion
            //更新主订单金额
            BLL.ADOrderInfo.Instance.UpdateTotalAmount_ADOrder(orderid, iprice);

            #region 清空购物车-当前订单相关的
            if (cartids.EndsWith(","))
            {
                cartids = cartids.Substring(0, cartids.Length - 1);
            }
            //清空购物车
            ClearCartInfo(orderInfo.MediaType, cartids);
            #endregion
        }
        #endregion

        
        #region 提交、修改订单实体
        public class JSONAddOrUpadteRequest
        {
            public Entities.EnumAddModify optType { get; set; }
            public JSONADOrderInfo ADOrderInfo { get; set; }
            public List<JSONADDetailInfo2> ADDetails { get; set; }
            public bool CheckSelfModel(out string msg)
            {
                StringBuilder sb = new StringBuilder();
                msg = "";
                if (!Enum.IsDefined(typeof(Entities.EnumAddModify), optType))
                {
                    sb.Append("操作类型参数错误!");
                }
                if (ADOrderInfo == null)
                {
                    sb.Append("没有项目信息!");
                }
                else
                {

                    if (ADOrderInfo.MediaType == 0)
                    {
                        sb.Append("媒体类型值不能为0!\n");
                    }

                    if (!Enum.IsDefined(typeof(Entities.EnumOrderStatus), ADOrderInfo.Status))
                    {
                        sb.Append("订单状态错误!不能为:" + ADOrderInfo.Status);
                    }

                    #region 广告位记录必须有，但是自媒体可以没有选择具体的广告位值
                    if (ADDetails == null || ADDetails.Count == 0)
                    {
                        sb.Append("没有广告位信息!\n");
                    }
                    else
                    {
                        if (ADDetails.Count > 50)
                        {
                            sb.Append("广告位数量不能超过50!\n");
                        }
                        foreach (JSONADDetailInfo2 detail in ADDetails)
                        {
                            if (ADOrderInfo.MediaType != detail.MediaType)
                            {
                                sb.Append("广告位ID：" + detail.PubDetailID + ",媒体类型：" + detail.MediaType + "与项目类型:" + ADOrderInfo.MediaType + "必须相同!\n");
                            }
                        }
                    }
                    #endregion

                    #region 修改订单验证
                    if (optType == Entities.EnumAddModify.Modify)
                    {
                        if (string.IsNullOrEmpty(ADOrderInfo.OrderID))
                        {
                            sb.Append("修改订单操作订单号是必填项!\n");
                        }
                        if (ADOrderInfo.Status != (int)Entities.EnumOrderStatus.Draft)
                        {
                            DateTime tmpdate = new DateTime(1990, 1, 1);
                            if (ADOrderInfo.BeginTime < tmpdate || ADOrderInfo.EndTime < tmpdate)
                            {
                                sb.Append("执行周期时间格式不对!\n");
                            }                           

                            if (string.IsNullOrEmpty(ADOrderInfo.OrderName))
                            {
                                sb.Append("订单名称为必填项!\n");
                            }
                            if (string.IsNullOrEmpty(ADOrderInfo.Note))
                            {
                                sb.Append("需求说明为必填项!\n");
                            }
                            if (string.IsNullOrEmpty(ADOrderInfo.UploadFileURL))
                            {
                                sb.Append("补充附件为必填项!\n");
                            }
                            if (ADDetails != null && ADDetails.Count > 0)
                            {
                                #region 自媒体广告位媒体ID不能重复,APP广告位媒体ID和广告位ID不能重复
                                if (ADOrderInfo.MediaType != (int)Entities.EnumMediaType.APP)
                                {
                                    bool brepeat1 = ADDetails.GroupBy(l => l.MediaID).Where(g => g.Count() > 1).Count() > 0;
                                    if (brepeat1)
                                    {
                                        sb.Append("自媒体广告位媒体ID不能重复!\n");
                                    }
                                }
                                else
                                {
                                    bool brepeat2 = ADDetails.GroupBy(l => new { l.MediaID, l.PubDetailID }).Where(g => g.Count() > 1).Count() > 0;
                                    if (brepeat2)
                                    {
                                        sb.Append("APP广告位媒体ID和广告位ID不能重复!\n");
                                    }
                                }
                                #endregion
                            }
                            
                        }
                    }
                    #endregion
                }

                msg = sb.ToString();
                return msg.Length.Equals(0);
            }
        }
        #endregion

        #region 主订单实体类
        public class JSONADOrderInfo
        {
            public string OrderID { get; set; }
            public int MediaType { get; set; }
            public string OrderName { get; set; }
            public int Status { get; set; }
            public DateTime BeginTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Note { get; set; }
            public string UploadFileURL { get; set; }
            public string CustomerID { get; set; }            
        }
        #endregion        
       

        #region 子订单详情实体类
        public class JSONADDetailInfo2
        {
            public int MediaType { get; set; }
            public int MediaID { get; set; }
            public int PubDetailID { get; set; }
            public decimal AdjustPrice { get; set; }
            public decimal AdjustDiscount { get; set; }
            public int ADLaunchDays { get; set; }
            public List<JSONADScheduleInfo2> ADScheduleInfos { get; set; }
            //public string ADScheduleInfos { get; set; }
        }
        #endregion

        #region CPD排期实体类
        public class JSONADScheduleInfo2
        {            
            public DateTime BeginData { get; set; }
            public DateTime EndData { get; set; }            
        }       
        #endregion
    }
}
