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
using Newtonsoft.Json.Linq;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ADOrderInfoV1_1BAKController : ApiController
    {
        int currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
        private static string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");

        #region 提交、修改主订单      
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult AddOrUpdate_ADOrderInfo([FromBody] JObject requestObj)
        {
            JSONAddOrUpadteRequest r = requestObj.ToObject<JSONAddOrUpadteRequest>();
            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo...JSONAddOrUpadteRequest->" + listd + "******");
            }
            catch (Exception ex)
            { }
            Entities.EnumAddModify optType = r.optType;
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo...begin...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType);
         
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
            List<JSONADDetailInfo> jdetails = new List<JSONADDetailInfo>();
            jdetails = r.ADDetails;
            Common.JsonResult jr = new Common.JsonResult();
            jr = WebAPI.Common.Util.GetJsonDataByResult(null, "操作成功", 0);
            try
            {
                switch (optType)
                {
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.ADD:
                        string orderid = string.Empty;
                        AddOrderInfo(jorder, jdetails, out orderid);
                        jr = WebAPI.Common.Util.GetJsonDataByResult(null, orderid, 0);
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.Modify:
                        string msg = string.Empty;
                        ModifyOrderInfo(jorder, jdetails, out msg);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****ModifyOrderInfo 出错errormsg:" + msg);
                            jr = WebAPI.Common.Util.GetJsonDataByResult(null, msg, -1);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo 出错errormsg:" + ex.Message);
                jr = WebAPI.Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo...end...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType);
            return jr;
        }

        public void AddOrderInfo(JSONADOrderInfo orderInfo, List<JSONADDetailInfo> listDetail, out string orderid)
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
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrderInfo 客户ID解码解密出错：->" + ex.Message);
            }

            orderid = BLL.ADOrderInfo.Instance.Insert(order);
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成订单：" + orderid);
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
            string cartids = "";
            //子订单定义：同一个媒体的所有广告位归类位一个子订单          
            IEnumerable<IGrouping<int, JSONADDetailInfo>> query = listDetail.GroupBy(x => x.MediaID);
            decimal iprice = 0;//主订单金额
            foreach (IGrouping<int, JSONADDetailInfo> info in query)
            {
                List<JSONADDetailInfo> lgroup = info.ToList<JSONADDetailInfo>();//分组后的集合

                decimal itotal = 0;
                int ipos = 0;
                string suborderid = string.Empty;
                foreach (JSONADDetailInfo item in lgroup)
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
                        BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成子订单：" + suborderid);
                    }

                    //生成广告位
                    Entities.ADDetailInfo demodel = new Entities.ADDetailInfo();
                    demodel.OrderID = orderid;
                    demodel.SubOrderID = suborderid;
                    demodel.MediaType = orderInfo.MediaType;
                    demodel.MediaID = item.MediaID;
                    demodel.AdjustPrice = item.AdjustPrice;
                    demodel.AdjustDiscount = item.AdjustDiscount;
                    demodel.ADLaunchDays = item.ADLaunchDays;
                    demodel.CreateUserID = currentUserID;
                    demodel.PubDetailID = item.PubDetailID;
                    #region 根据广告位ID、媒体类型获取刊例基础信息
                    System.Data.DataTable pubDetailDT = BLL.ADOrderInfo.Instance.p_GetPubDetailInfo_SelectV1_1(orderInfo.MediaType, item.PubDetailID);
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
                        else if (orderInfo.MediaType == (int)Entities.EnumMediaType.WeChat)
                        {
                            demodel.ADLaunchDays = item.ADScheduleInfos.Count;
                            //微信价格=原始价格*销售折扣*排期个数
                            demodel.AdjustPrice = demodel.OriginalPrice * demodel.SaleDiscount * item.ADScheduleInfos.Count;
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
                    BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成广告位：" + detailid);

                    #region 排期保存
                    //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                    if (orderInfo.MediaType == (int)Entities.EnumMediaType.APP)
                    {
                        cartids += item.PubDetailID + ",";
                        int cpd = 0;
                        if (int.TryParse(demodel.ADLaunchIDs, out cpd) && cpd == 11001)
                        {
                            if (item.ADScheduleInfos.Count > 0)
                            {
                                List<JSONADScheduleInfo> list_JSONADScheduleInfo = new List<JSONADScheduleInfo>();
                                foreach (JSONADScheduleInfo2 sc in item.ADScheduleInfos)
                                {
                                    list_JSONADScheduleInfo.Add(new JSONADScheduleInfo()
                                    {
                                        ADDetailID = detailid,
                                        OrderID = orderid,
                                        SubOrderID = suborderid,
                                        MediaID = item.MediaID,
                                        PubID = demodel.PubID,
                                        CreateTime = DateTime.Now,
                                        CreateUserID = currentUserID,
                                        BeginData = sc.BeginData,
                                        EndData = sc.EndData
                                    });
                                }
                                System.Data.DataTable mydt = BLL.Util.ListToDataTable<JSONADScheduleInfo>(list_JSONADScheduleInfo);
                                BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            }
                        }
                    }
                    else if (orderInfo.MediaType == (int)Entities.EnumMediaType.WeChat)
                    {
                        cartids += item.PubDetailID + ",";
                        if (item.ADScheduleInfos.Count > 0)
                        {
                            List<JSONADScheduleInfo> list_JSONADScheduleInfo = new List<JSONADScheduleInfo>();
                            foreach (JSONADScheduleInfo2 sc in item.ADScheduleInfos)
                            {
                                list_JSONADScheduleInfo.Add(new JSONADScheduleInfo()
                                {
                                    ADDetailID = detailid,
                                    OrderID = orderid,
                                    SubOrderID = suborderid,
                                    MediaID = item.MediaID,
                                    PubID = demodel.PubID,
                                    CreateTime = DateTime.Now,
                                    CreateUserID = currentUserID,
                                    BeginData = sc.BeginData,
                                    EndData = sc.EndData
                                });
                            }
                            System.Data.DataTable mydt = BLL.Util.ListToDataTable<JSONADScheduleInfo>(list_JSONADScheduleInfo);
                            BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
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
            BLL.CartInfo.Instance.ClearAll_CartInfoV1_1(orderInfo.MediaType, currentUserID, cartids);
            #endregion
        }        
        public static void ModifyOrderInfo(JSONADOrderInfo orderInfo, List<JSONADDetailInfo> listDetail, out string msg)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]ModifyOrderInfo->订单号:" + orderInfo.OrderID + ",媒体类型:" + orderInfo.MediaType);
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
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****ModifyOrderInfo 客户ID解码解密出错：->" + ex.Message);
            }
            
            order.UploadFileURL = orderInfo.UploadFileURL;
            if (!string.IsNullOrEmpty(orderInfo.UploadFileURL))
            {
                List<string> listurl = new List<string>();
                listurl.Add(orderInfo.UploadFileURL);
                BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(listurl, Chitunion2017.Common.UserInfo.GetLoginUserID(), Entities.Enum.UploadFileEnum.OrderManage, order.RecID, "ADOrderInfo");
            }

            string orderid = orderInfo.OrderID;
            BLL.ADOrderInfo.Instance.Update(order);
            #endregion
            
            //删除排期子订单广告位
            BLL.ADOrderInfo.Instance.p_ADOrderAllInfo_DeleteV1_1(orderid, order.MediaType, false);

            
            #region 遍历广告位 按媒体ID分组
            //自媒体记录MediaID串，APP记录PublishDetailID
            string cartids = "";
            //子订单定义：同一个媒体的所有广告位归类位一个子订单            
            IEnumerable<IGrouping<int, JSONADDetailInfo>> query = listDetail.GroupBy(x => x.MediaID);
            decimal iprice = 0;//主订单金额
            foreach (IGrouping<int, JSONADDetailInfo> info in query)
            {
                List<JSONADDetailInfo> lgroup = info.ToList<JSONADDetailInfo>();//分组后的集合

                decimal itotal = 0;
                int ipos = 0;
                string suborderid = "";
                foreach (JSONADDetailInfo item in lgroup)
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
                    demodel.MediaType = orderInfo.MediaType;
                    demodel.MediaID = item.MediaID;
                    demodel.PubDetailID = item.PubDetailID;

                    #region 根据广告位ID、媒体类型获取刊例基础信息
                    System.Data.DataTable pubDetailDT = BLL.ADOrderInfo.Instance.p_GetPubDetailInfo_SelectV1_1(orderInfo.MediaType, item.PubDetailID);
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
                        else if (orderInfo.MediaType == (int)Entities.EnumMediaType.WeChat)
                        {
                            demodel.ADLaunchDays = item.ADScheduleInfos.Count;
                            //微信价格=原始价格*销售折扣*排期个数
                            demodel.AdjustPrice = demodel.OriginalPrice * demodel.SaleDiscount * demodel.ADLaunchDays * item.ADScheduleInfos.Count;
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

                    #region 排期保存&&记录购物车广告位ID
                    //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                    if (orderInfo.MediaType == (int)Entities.EnumMediaType.APP)
                    {
                        cartids += item.PubDetailID + ",";
                        int cpd = 0;
                        if (int.TryParse(demodel.ADLaunchIDs, out cpd) && cpd == 11001)
                        {                            
                            if (item.ADScheduleInfos.Count > 0)
                            {                                
                                List<JSONADScheduleInfo> list_JSONADScheduleInfo = new List<JSONADScheduleInfo>();
                                foreach (JSONADScheduleInfo2 sc in item.ADScheduleInfos)
                                {
                                    list_JSONADScheduleInfo.Add(new JSONADScheduleInfo()
                                    {
                                        ADDetailID = detailid,
                                        OrderID = orderid,
                                        SubOrderID = suborderid,
                                        MediaID = item.MediaID,
                                        PubID = demodel.PubID,
                                        CreateTime = DateTime.Now,
                                        CreateUserID = Chitunion2017.Common.UserInfo.GetLoginUserID(),
                                        BeginData = sc.BeginData,
                                        EndData = sc.EndData
                                    });
                                }
                                System.Data.DataTable mydt = BLL.Util.ListToDataTable<JSONADScheduleInfo>(list_JSONADScheduleInfo);
                                BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            }
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****AddOrderInfo APP排期CPD转换int类型出错：->" + demodel.ADLaunchIDs);
                        }
                    }
                    else if (orderInfo.MediaType == (int)Entities.EnumMediaType.WeChat)
                    {
                        cartids += item.PubDetailID + ",";
                        if (item.ADScheduleInfos.Count > 0)
                        {
                            List<JSONADScheduleInfo> list_JSONADScheduleInfo = new List<JSONADScheduleInfo>();
                            foreach (JSONADScheduleInfo2 sc in item.ADScheduleInfos)
                            {
                                list_JSONADScheduleInfo.Add(new JSONADScheduleInfo()
                                {
                                    ADDetailID = detailid,
                                    OrderID = orderid,
                                    SubOrderID = suborderid,
                                    MediaID = item.MediaID,
                                    PubID = demodel.PubID,
                                    CreateTime = DateTime.Now,
                                    CreateUserID = Chitunion2017.Common.UserInfo.GetLoginUserID(),
                                    BeginData = sc.BeginData,
                                    EndData = sc.EndData
                                });
                            }
                            System.Data.DataTable mydt = BLL.Util.ListToDataTable<JSONADScheduleInfo>(list_JSONADScheduleInfo);
                            BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
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
            BLL.CartInfo.Instance.ClearAll_CartInfoV1_1(orderInfo.MediaType, Chitunion2017.Common.UserInfo.GetLoginUserID(), cartids);
            #endregion
        }
        public static string EncryptAndUrlEncode2CustomerID(int customerid)
        {
            string retval = string.Empty;
            try
            {
                //加密
                retval = XYAuto.Utils.Security.DESEncryptor.Encrypt(customerid.ToString(), LoginPwdKey);
                //编码
                retval = System.Web.HttpUtility.UrlEncode(retval, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****EncryptAndUrlEncode2CustomerID 客户ID加密编码出错：->" + ex.Message);
            }

            return retval;
        }
        #endregion

        #region 根据项目号查看项目
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetByOrderID_ADOrderInfo(string orderid)
        {            
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo...begin...orderid->" + orderid + "******");
            Common.JsonResult jr = new Common.JsonResult();
            //参数验证
            if (string.IsNullOrEmpty(orderid))
            {                
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",订单号是必填项");
                return WebAPI.Common.Util.GetJsonDataByResult(null, "订单号是必填项", -1);
            }

            #region 数据权限
            string vmsg = string.Empty;
            //BLL.Util.GetSqlRightStr(Entities.EnumResourceType.ADOrderInfo, "", "", currentUserID, out vmsg);
            //if (!string.IsNullOrEmpty(vmsg))
            //{
            //    jr.Status = -1;
            //    jr.Message = msg;
            //    BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",数据权限:"+ vmsg);
            //    return jr;
            //}
            #endregion

            JSONQueryResultADOrerInfo jsonResult = new JSONQueryResultADOrerInfo();
            //查询主订单信息
            Entities.ADOrderInfo ordermodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(orderid);
            if (ordermodel == null)
            {
                vmsg = "订单号:" + orderid + "不存在";
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",不存在");
                return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
            }
            JSONQueryADOrerInfo orderinfo = new JSONQueryADOrerInfo();
            orderinfo.OrderID = ordermodel.OrderID;
            orderinfo.MediaType = ordermodel.MediaType;
            orderinfo.OrderName = ordermodel.OrderName;
            orderinfo.BeginTime = ordermodel.BeginTime;
            orderinfo.EndTime = ordermodel.EndTime;
            orderinfo.Note = ordermodel.Note;
            orderinfo.UploadFileURL = ordermodel.UploadFileURL;
            if (!string.IsNullOrEmpty(ordermodel.UploadFileURL))
            {
                orderinfo.UploadFileName = BLL.Util.GetFileNameByUpload(ordermodel.UploadFileURL);
            }
            orderinfo.TotalAmount = ordermodel.TotalAmount;
            orderinfo.CreateTime = ordermodel.CreateTime;
            orderinfo.Status = ordermodel.Status;
            orderinfo.CreateUserID = ordermodel.CreateUserID;
            try
            {
                //加密
                orderinfo.CustomerID = XYAuto.Utils.Security.DESEncryptor.Encrypt(ordermodel.CustomerID.ToString(), LoginPwdKey);
                //编码
                orderinfo.CustomerID = System.Web.HttpUtility.UrlEncode(orderinfo.CustomerID, Encoding.UTF8);                           
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo 客户ID加密编码出错：->" + ex.Message);
            }
            orderinfo.CreatorName = ordermodel.CreatorName;
            orderinfo.CustomerName = ordermodel.CustomerName;
            orderinfo.CreatorUserName = ordermodel.CreatorUserName;
            orderinfo.CustomerUserName = ordermodel.CustomerUserName;

            //查询多条子订单信息
            List<JSONQuerySubADInfo> suborders = new List<JSONQuerySubADInfo>();
            DataTable dt = BLL.SubADInfo.Instance.GetSubADInfoByOrderID(orderid);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    Entities.SubADInfo m = BLL.SubADInfo.Instance.DataRowToModel(row);
                    JSONQuerySubADInfo suborder = new JSONQuerySubADInfo();
                    suborder.OrderID = m.OrderID;
                    suborder.SubOrderID = m.SubOrderID;
                    //suborder.MediaType = m.MediaType;
                    suborder.MediaID = m.MediaID;
                    suborder.TotalAmount = m.TotalAmount;
                    suborder.Status = m.Status;
                    suborder.CreateTime = m.CreateTime;
                    suborder.CreateUserID = m.CreateUserID;

                    //子订单所属明细                    
                    DataTable dt2 = BLL.ADDetailInfo.Instance.GetADDetailInfoBySubOrderID(m.SubOrderID);
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        List<JSONQueryAPPDetail> appdetails = new List<JSONQueryAPPDetail>();
                        List<JSONQuerySelfDetail> selfdetails = new List<JSONQuerySelfDetail>();

                        //判断APP还是自媒体
                        if (orderinfo.MediaType == (int)Entities.EnumMediaType.APP)
                        {
                            //遍历子订单所属明细
                            foreach (System.Data.DataRow row2 in dt2.Rows)
                            {
                                Entities.ADDetailInfo a = BLL.ADDetailInfo.Instance.DataRowToModel(row2);
                                JSONQueryAPPDetail appdetail = new JSONQueryAPPDetail();
                                appdetail.OrderID = a.OrderID;
                                appdetail.SubOrderID = a.SubOrderID;
                                appdetail.MediaType = a.MediaType;
                                appdetail.MediaID = a.MediaID;
                                appdetail.PubID = a.PubID;
                                appdetail.PublishDetailID = a.PubDetailID;
                                appdetail.ADDetailID = a.RecID;

                                //根据广告位查询APP扩展表信息
                                System.Data.DataTable appdt = BLL.ADOrderInfo.Instance.GetAPPMediaDetail_PubDetailID(a.PubDetailID);
                                if (appdt != null)
                                {
                                    appdetail.Name = appdt.Rows[0]["Name"].ToString();
                                    appdetail.HeadIconURL = appdt.Rows[0]["HeadIconURL"].ToString();
                                    appdetail.AdPosition = appdt.Rows[0]["AdPosition"].ToString();
                                    appdetail.AdForm = appdt.Rows[0]["AdForm"].ToString();
                                    appdetail.Style = appdt.Rows[0]["Style"].ToString();
                                    appdetail.CarouselCount = Convert.ToInt32(appdt.Rows[0]["CarouselCount"]);
                                    appdetail.PlayPosition = appdt.Rows[0]["PlayPosition"].ToString();
                                    if (appdt.Rows[0]["SysPlatform"] != null && appdt.Rows[0]["SysPlatform"].ToString() != "")
                                    {
                                        string[] sysps = appdt.Rows[0]["SysPlatform"].ToString().Split(',');
                                        string SysPlatform = "";
                                        foreach (string sys in sysps)
                                        {
                                            if (sys == "12001")
                                            {
                                                SysPlatform += "Android,";
                                            }
                                            if (sys == "12002")
                                            {
                                                SysPlatform += "IOS,";
                                            }
                                        }
                                        SysPlatform = SysPlatform.Substring(0, SysPlatform.Length - 1);
                                        appdetail.SysPlatform = SysPlatform;
                                    }
                                }

                                appdetail.OriginalPrice = a.OriginalPrice;
                                appdetail.AdjustPrice = a.AdjustPrice;
                                appdetail.AdjustDiscount = a.AdjustDiscount;
                                appdetail.PurchaseDiscount = a.PurchaseDiscount;
                                appdetail.SaleDiscount = a.SaleDiscount;
                                appdetail.ADLaunchDays = a.ADLaunchDays;
                                if (orderinfo.Status == (int)Entities.EnumOrderStatus.Draft || orderinfo.Status == (int)Entities.EnumOrderStatus.PendingAudit)
                                {
                                    appdetail.Amount = a.OriginalPrice * a.SaleDiscount * a.ADLaunchDays;
                                }
                                else
                                {
                                    appdetail.Amount = a.AdjustPrice * a.ADLaunchDays;
                                }
                                int itmp = 0;
                                if (int.TryParse(a.ADLaunchIDs, out itmp))
                                {
                                    appdetail.CPDCPM = itmp;
                                }
                                appdetail.CreateTime = a.CreateTime;
                                appdetail.CreateUserID = a.CreateUserID;

                                //是否是APP CPD广告
                                //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                                List<JSONADScheduleInfo> ADScheduleInfos = new List<JSONADScheduleInfo>();
                                //查询订单明细所属排期信息
                                System.Data.DataTable dt3 = BLL.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(a.RecID);
                                if (dt3 != null && dt3.Rows.Count > 0)
                                {
                                    //遍历明细所属CPD排期
                                    foreach (System.Data.DataRow row3 in dt3.Rows)
                                    {
                                        Entities.ADScheduleInfo adsmodel = BLL.ADScheduleInfo.Instance.DataRowToModel(row3);
                                        JSONADScheduleInfo sc = new JSONADScheduleInfo();
                                        sc.ADDetailID = adsmodel.ADDetailID;
                                        sc.OrderID = adsmodel.OrderID;
                                        sc.SubOrderID = adsmodel.SubOrderID;
                                        sc.MediaID = adsmodel.MediaID;
                                        sc.PubID = adsmodel.PubID;
                                        sc.BeginData = adsmodel.BeginData;
                                        sc.EndData = adsmodel.EndData;
                                        sc.CreateTime = adsmodel.CreateTime;
                                        sc.CreateUserID = adsmodel.CreateUserID;

                                        ADScheduleInfos.Add(sc);
                                    }
                                }


                                appdetail.ADScheduleInfos = ADScheduleInfos;
                                appdetails.Add(appdetail);
                            }
                        }
                        else
                        {
                            //遍历子订单所属明细
                            foreach (System.Data.DataRow row2 in dt2.Rows)
                            {
                                Entities.ADDetailInfo a = BLL.ADDetailInfo.Instance.DataRowToModel(row2);
                                JSONQuerySelfDetail selfdetail = new JSONQuerySelfDetail();
                                //selfdetail.OrderID = a.OrderID;
                                //selfdetail.SubOrderID = a.SubOrderID;
                                //selfdetail.MediaType = a.MediaType;
                                //selfdetail.MediaID = a.MediaID;
                                selfdetail.PubID = a.PubID;
                                selfdetail.PublishDetailID = a.PubDetailID;
                                selfdetail.ADDetailID = a.RecID;


                                System.Data.DataTable selfDt;
                                //获取自媒体广告位信息
                                selfDt = BLL.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypePubDetailIDV1_1(a.MediaType, a.PubDetailID);
                                if (selfDt != null && selfDt.Rows.Count > 0)
                                {
                                    //selfdetail.Name = selfDt.Rows[0]["Name"].ToString();
                                    //selfdetail.Number = selfDt.Rows[0]["Number"].ToString();
                                    suborder.Name = selfDt.Rows[0]["Name"].ToString();
                                    suborder.Number = selfDt.Rows[0]["Number"].ToString();
                                    selfdetail.PubBeginTime = selfDt.Rows[0]["PubBeginTime"] != null && selfDt.Rows[0]["PubBeginTime"].ToString() != "" ? 
                                                                DateTime.Parse(selfDt.Rows[0]["PubBeginTime"].ToString()) : Entities.Constants.Constant.DATE_INVALID_VALUE;
                                    selfdetail.PubEndTime = selfDt.Rows[0]["PubEndTime"] != null && selfDt.Rows[0]["PubEndTime"].ToString() != "" ?
                                                                DateTime.Parse(selfDt.Rows[0]["PubEndTime"].ToString()) : Entities.Constants.Constant.DATE_INVALID_VALUE;
                                    selfdetail.Source = selfDt.Rows[0]["Source"].ToString();
                                    selfdetail.IsAuth = selfDt.Rows[0]["IsAuth"].ToString();
                                    if (selfDt.Rows[0]["expired"] != null && selfDt.Rows[0]["expired"].ToString() != "")
                                    {
                                        selfdetail.expired = Convert.ToInt32(selfDt.Rows[0]["expired"].ToString());
                                    }                                    
                                    selfdetail.ADMasterImage = selfDt.Rows[0]["ADMasterImage"].ToString();
                                    selfdetail.ADMasterTitle = selfDt.Rows[0]["ADMasterTitle"].ToString();
                                    selfdetail.ADPosition = selfDt.Rows[0]["AdPosition"].ToString();                                    
                                    selfdetail.CreateType = selfDt.Rows[0]["CreateType"].ToString();                                    
                                }                               

                                selfdetail.OriginalPrice = a.OriginalPrice;
                                selfdetail.AdjustPrice = a.AdjustPrice;
                                selfdetail.AdjustDiscount = a.AdjustDiscount;
                                selfdetail.PurchaseDiscount = a.PurchaseDiscount;
                                selfdetail.SaleDiscount = a.SaleDiscount;
                                selfdetail.ADLaunchDays = a.ADLaunchDays;
                                selfdetail.CreateTime = a.CreateTime;
                                selfdetail.CreateUserID = a.CreateUserID;

                                DataTable dt_ADSchedule = BLL.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(a.RecID);
                                selfdetail.ADSchedule = new List<JSONQeruySelfADSchedule>();

                                foreach (DataRow row_ADSchedule in dt_ADSchedule.Rows)
                                {
                                    selfdetail.ADSchedule.Add(new JSONQeruySelfADSchedule()
                                    {
                                        BeginData = Convert.ToDateTime(row_ADSchedule["BeginData"]),
                                        EndData = DateTime.Now
                                    });
                                }                                

                                selfdetails.Add(selfdetail);
                            }
                        }
                        suborder.SelfDetails = selfdetails;
                        suborder.APPDetails = appdetails;
                        suborders.Add(suborder);
                    }
                    else
                    {
                        jr.Status = 3;
                        jr.Message = "子订单明细没有数据";
                        BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",子订单明细没有数据");
                    }
                }
            }
            else
            {
                jr.Status = 2;
                jr.Message = "子订单没有数据";
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",子订单没有数据");
            }

            jsonResult.ADOrderInfo = orderinfo;
            jsonResult.SubADInfos = suborders;

            jr.Status = 0;
            jr.Message = "执行成功";
            jr.Result = jsonResult;
            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo->" + listd + "******");
            }
            catch (Exception ex)
            { }
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetByOrderID_ADOrderInfo...end...orderid->" + orderid + "******");
            return jr;
        }
        #endregion

        #region 根据订单号查看订单
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001BUT20001")]
        public Common.JsonResult GetBySubOrderID_ADOrderInfo(string suborderid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo...begin...suborderid->" + suborderid + "******");

            Common.JsonResult jr = new Common.JsonResult();
            //参数验证
            if (string.IsNullOrEmpty(suborderid))
            {
                jr.Status = 1;
                jr.Message = "订单号是必填项";
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",订单号是必填项");
                return jr;
            }

            #region 数据权限
            string msg = string.Empty;
            //BLL.Util.GetSqlRightStr(Entities.EnumResourceType.ADOrderInfo, "", "", currentUserID, out msg);
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    jr.Status = -1;
            //    jr.Message = msg;
            //    BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",数据权限:" + msg);
            //    return jr;
            //}
            #endregion

            //查询子订单信息            
            Entities.SubADInfo m = new Entities.SubADInfo();
            try
            {
                m = BLL.SubADInfo.Instance.GetSubADInfo(suborderid);
            }
            catch (Exception ex)
            {
                jr.Status = 99;
                jr.Message = "查询子订单信息出错:" + ex.Message;
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",查询子订单信息出错:" + ex.Message);
                return jr;
            }

            if (m == null)
            {
                jr.Status = 1;
                jr.Message = "子订单号:" + suborderid + "不存在";
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo orderid->" + suborderid + ",不存在");
                return jr;
            }

            JSONQuerySubADInfo suborder = new JSONQuerySubADInfo();
            suborder.OrderID = m.OrderID;
            suborder.SubOrderID = m.SubOrderID;
            //suborder.MediaType = m.MediaType;
            suborder.MediaID = m.MediaID;
            suborder.TotalAmount = m.TotalAmount;
            suborder.Status = m.Status;
            suborder.CreateTime = m.CreateTime;
            suborder.CreateUserID = m.CreateUserID;

            DataTable dt_ADOrderOperateInfo = BLL.ADOrderInfo.Instance.GeADOrderOperateInfoV1_1(m.OrderID, suborderid);
            suborder.OperateInfo = new List<JSONQuerySubOrderOperateInfo>();
            if (dt_ADOrderOperateInfo != null && dt_ADOrderOperateInfo.Rows.Count > 0)
            {
                foreach (DataRow row_ADOrderOperateInfo in dt_ADOrderOperateInfo.Rows)
                {
                    suborder.OperateInfo.Add(new JSONQuerySubOrderOperateInfo()
                    {
                        Creator = Convert.ToString(row_ADOrderOperateInfo["Creator"]),
                        CreateTime = Convert.ToDateTime(row_ADOrderOperateInfo["CreateTime"]),
                        LastOrderStatus = Convert.ToString(row_ADOrderOperateInfo["LastOrderStatus"]),
                        OrderStatus = Convert.ToString(row_ADOrderOperateInfo["OrderStatus"])
                    });
                }
            }

            //子订单所属明细           
            DataTable dt2 = BLL.ADDetailInfo.Instance.GetADDetailInfoBySubOrderID(m.SubOrderID);

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                List<JSONQueryAPPDetail> appdetails = new List<JSONQueryAPPDetail>();
                List<JSONQuerySelfDetail> selfdetails = new List<JSONQuerySelfDetail>();

                //判断APP还是自媒体
                if (m.MediaType == (int)Entities.EnumMediaType.APP)
                {
                    //遍历子订单所属明细
                    foreach (System.Data.DataRow row2 in dt2.Rows)
                    {
                        Entities.ADDetailInfo a = BLL.ADDetailInfo.Instance.DataRowToModel(row2);
                        JSONQueryAPPDetail appdetail = new JSONQueryAPPDetail();
                        appdetail.OrderID = a.OrderID;
                        appdetail.SubOrderID = a.SubOrderID;
                        appdetail.MediaType = a.MediaType;
                        appdetail.MediaID = a.MediaID;
                        appdetail.PubID = a.PubID;
                        appdetail.PublishDetailID = a.PubDetailID;
                        appdetail.ADDetailID = a.RecID;

                        //根据广告位查询APP扩展表信息
                        System.Data.DataTable appdt = BLL.ADOrderInfo.Instance.GetAPPMediaDetail_PubDetailID(a.PubDetailID);
                        if (appdt != null)
                        {
                            appdetail.Name = appdt.Rows[0]["Name"].ToString();
                            appdetail.HeadIconURL = appdt.Rows[0]["HeadIconURL"].ToString();
                            appdetail.AdPosition = appdt.Rows[0]["AdPosition"].ToString();
                            appdetail.AdForm = appdt.Rows[0]["AdForm"].ToString();
                            appdetail.Style = appdt.Rows[0]["Style"].ToString();
                            appdetail.CarouselCount = Convert.ToInt32(appdt.Rows[0]["CarouselCount"]);
                            appdetail.PlayPosition = appdt.Rows[0]["PlayPosition"].ToString();
                            if (appdt.Rows[0]["SysPlatform"] != null && appdt.Rows[0]["SysPlatform"].ToString() != "")
                            {
                                string[] sysps = appdt.Rows[0]["SysPlatform"].ToString().Split(',');
                                string SysPlatform = "";
                                foreach (string sys in sysps)
                                {
                                    if (sys == "12001")
                                    {
                                        SysPlatform += "Android,";
                                    }
                                    if (sys == "12002")
                                    {
                                        SysPlatform += "IOS,";
                                    }
                                }
                                SysPlatform = SysPlatform.Substring(0, SysPlatform.Length - 1);
                                appdetail.SysPlatform = SysPlatform;
                            }
                        }

                        appdetail.OriginalPrice = a.OriginalPrice;
                        appdetail.AdjustPrice = a.AdjustPrice;
                        appdetail.AdjustDiscount = a.AdjustDiscount;
                        appdetail.PurchaseDiscount = a.PurchaseDiscount;
                        appdetail.SaleDiscount = a.SaleDiscount;
                        appdetail.ADLaunchDays = a.ADLaunchDays;
                        if (suborder.Status == (int)Entities.EnumOrderStatus.Draft || suborder.Status == (int)Entities.EnumOrderStatus.PendingAudit)
                        {
                            appdetail.Amount = a.OriginalPrice * a.SaleDiscount * a.ADLaunchDays;
                        }
                        else
                        {
                            appdetail.Amount = a.AdjustPrice * a.ADLaunchDays;
                        }

                        int itmp = 0;
                        if (int.TryParse(a.ADLaunchIDs, out itmp))
                        {
                            appdetail.CPDCPM = itmp;
                        }
                        appdetail.CreateTime = a.CreateTime;
                        appdetail.CreateUserID = a.CreateUserID;

                        //是否是APP CPD广告
                        //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                        List<JSONADScheduleInfo> ADScheduleInfos = new List<JSONADScheduleInfo>();
                        //查询订单明细所属排期信息
                        System.Data.DataTable dt3 = BLL.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(a.RecID);
                        if (dt3 != null && dt3.Rows.Count > 0)
                        {
                            //遍历明细所属CPD排期
                            foreach (System.Data.DataRow row3 in dt3.Rows)
                            {
                                Entities.ADScheduleInfo adsmodel = BLL.ADScheduleInfo.Instance.DataRowToModel(row3);
                                JSONADScheduleInfo sc = new JSONADScheduleInfo();
                                sc.ADDetailID = adsmodel.ADDetailID;
                                sc.OrderID = adsmodel.OrderID;
                                sc.SubOrderID = adsmodel.SubOrderID;
                                sc.MediaID = adsmodel.MediaID;
                                sc.PubID = adsmodel.PubID;
                                sc.BeginData = adsmodel.BeginData;
                                sc.EndData = adsmodel.EndData;
                                sc.CreateTime = adsmodel.CreateTime;
                                sc.CreateUserID = adsmodel.CreateUserID;

                                ADScheduleInfos.Add(sc);
                            }
                        }


                        appdetail.ADScheduleInfos = ADScheduleInfos;
                        appdetails.Add(appdetail);
                    }
                }
                else
                {
                    //遍历子订单所属明细
                    foreach (System.Data.DataRow row2 in dt2.Rows)
                    {
                        Entities.ADDetailInfo a = BLL.ADDetailInfo.Instance.DataRowToModel(row2);
                        JSONQuerySelfDetail selfdetail = new JSONQuerySelfDetail();
                        //selfdetail.OrderID = a.OrderID;
                        //selfdetail.SubOrderID = a.SubOrderID;
                        //selfdetail.MediaType = a.MediaType;
                        //selfdetail.MediaID = a.MediaID;
                        selfdetail.PubID = a.PubID;
                        selfdetail.PublishDetailID = a.PubDetailID;
                        selfdetail.ADDetailID = a.RecID;

                        System.Data.DataTable selfDt;
                        //获取自媒体广告位信息
                        selfDt = BLL.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypePubDetailIDV1_1(a.MediaType, a.PubDetailID);
                        if (selfDt != null && selfDt.Rows.Count > 0)
                        {
                            //selfdetail.Name = selfDt.Rows[0]["Name"].ToString();
                            //selfdetail.Number = selfDt.Rows[0]["Number"].ToString();
                            suborder.Name = selfDt.Rows[0]["Name"].ToString();
                            suborder.Number = selfDt.Rows[0]["Number"].ToString();
                            selfdetail.PubBeginTime = selfDt.Rows[0]["PubBeginTime"] != null && selfDt.Rows[0]["PubBeginTime"].ToString() != "" ?
                                                                DateTime.Parse(selfDt.Rows[0]["PubBeginTime"].ToString()) : Entities.Constants.Constant.DATE_INVALID_VALUE;
                            selfdetail.PubEndTime = selfDt.Rows[0]["PubEndTime"] != null && selfDt.Rows[0]["PubEndTime"].ToString() != "" ?
                                                        DateTime.Parse(selfDt.Rows[0]["PubEndTime"].ToString()) : Entities.Constants.Constant.DATE_INVALID_VALUE;
                            selfdetail.IsAuth = selfDt.Rows[0]["IsAuth"].ToString();
                            if (selfDt.Rows[0]["expired"] != null && selfDt.Rows[0]["expired"].ToString() != "")
                            {
                                selfdetail.expired = Convert.ToInt32(selfDt.Rows[0]["expired"].ToString());
                            }
                            selfdetail.Source = selfDt.Rows[0]["Source"].ToString();
                            selfdetail.ADMasterImage = selfDt.Rows[0]["ADMasterImage"].ToString();
                            selfdetail.ADMasterTitle = selfDt.Rows[0]["ADMasterTitle"].ToString();
                            selfdetail.ADPosition = selfDt.Rows[0]["AdPosition"].ToString();
                            selfdetail.CreateType = selfDt.Rows[0]["CreateType"].ToString();
                        }                                               

                        selfdetail.OriginalPrice = a.OriginalPrice;
                        selfdetail.AdjustPrice = a.AdjustPrice;
                        selfdetail.AdjustDiscount = a.AdjustDiscount;
                        selfdetail.PurchaseDiscount = a.PurchaseDiscount;
                        selfdetail.SaleDiscount = a.SaleDiscount;
                        selfdetail.ADLaunchDays = a.ADLaunchDays;
                        selfdetail.CreateTime = a.CreateTime;
                        selfdetail.CreateUserID = a.CreateUserID;

                        DataTable dt_ADSchedule = BLL.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(a.RecID);
                        selfdetail.ADSchedule = new List<JSONQeruySelfADSchedule>();

                        foreach (DataRow row_ADSchedule in dt_ADSchedule.Rows)
                        {
                            selfdetail.ADSchedule.Add(new JSONQeruySelfADSchedule()
                            {
                                BeginData = Convert.ToDateTime(row_ADSchedule["BeginData"]),
                                EndData = DateTime.Now
                            });
                        }                                             

                        selfdetails.Add(selfdetail);
                    }
                }
                suborder.APPDetails = appdetails;
                suborder.SelfDetails = selfdetails;                
            }
            else
            {
                jr.Status = 3;
                jr.Message = "子订单明细没有数据";
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo orderid->" + suborderid + ",子订单明细没有数据");
            }

            //查询主订单信息
            Entities.ADOrderInfo ordermodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(suborder.OrderID);
            if (ordermodel == null)
            {
                jr.Status = 1;
                jr.Message = "订单号:" + suborder.OrderID + "不存在";
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo orderid->" + suborder.OrderID + ",不存在");
                return jr;
            }

            JSONQueryADOrerInfo orderinfo = new JSONQueryADOrerInfo();
            orderinfo.OrderID = ordermodel.OrderID;
            orderinfo.OrderName = ordermodel.OrderName;
            orderinfo.BeginTime = ordermodel.BeginTime;
            orderinfo.EndTime = ordermodel.EndTime;
            orderinfo.Note = ordermodel.Note;
            orderinfo.UploadFileURL = ordermodel.UploadFileURL;
            if (!string.IsNullOrEmpty(ordermodel.UploadFileURL))
            {
                orderinfo.UploadFileName = BLL.Util.GetFileNameByUpload(ordermodel.UploadFileURL);
            }
            orderinfo.MediaType = ordermodel.MediaType;
            orderinfo.TotalAmount = ordermodel.TotalAmount;
            orderinfo.Status = ordermodel.Status;
            orderinfo.CreateTime = ordermodel.CreateTime;
            orderinfo.CreateUserID = ordermodel.CreateUserID;
            try
            {
                //加密
                orderinfo.CustomerID = XYAuto.Utils.Security.DESEncryptor.Encrypt(ordermodel.CustomerID.ToString(), LoginPwdKey);
                //编码
                orderinfo.CustomerID = System.Web.HttpUtility.UrlEncode(orderinfo.CustomerID, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo 客户ID加密编码出错：->" + ex.Message);
            }
            orderinfo.CreatorName = ordermodel.CreatorName;
            orderinfo.CustomerName = ordermodel.CustomerName;
            orderinfo.CreatorUserName = ordermodel.CreatorUserName;
            orderinfo.CustomerUserName = ordermodel.CustomerUserName;

            List<JSONQuerySubADInfo> suborders = new List<JSONQuerySubADInfo>();
            suborders.Add(suborder);
            JSONQueryResultADOrerInfo jsonResult = new JSONQueryResultADOrerInfo();
            jsonResult.ADOrderInfo = orderinfo;
            jsonResult.SubADInfos = suborders;

            jr.Status = 0;
            jr.Message = "执行成功";
            jr.Result = jsonResult;

            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo->" + listd + "******");
            }
            catch (Exception ex)
            { }

            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****GetBySubOrderID_ADOrderInfo...end...suborderid->" + suborderid + "******");
            return jr;
        }
        #endregion        

        #region 删除项目
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult DeleteADOrderInfoByOrderID(string orderid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateStatus_ADOrderInfo begin...orerid->" + orderid + "******");
            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数
                string vermsg = string.Empty;                
                if (string.IsNullOrEmpty(orderid))
                {
                    vermsg += "主订单号为必填项!\n";
                }
                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);
                #endregion                
                //更改主订单状态
                ADOrderInfo.Instance.UpdateStatus_ADOrder(orderid, (int)Entities.EnumOrderStatus.Deleted);
                //更改子订单状态
                SubADInfo.Instance.UpdateStatusByOrderID_SubADInfo(orderid, (int)Entities.EnumOrderStatus.Deleted);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1ControllerUpdateStatus_ADOrderInfo...orerid->" + orderid + ",errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;

                return jr;
            }

            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateStatus_ADOrderInfo end...orerid->" + orderid + "******");
            jr.Message = "更新成功";
            return jr;
        }

        #endregion

        #region 信息页查询
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult QuerytAuditInfo(int PageIndex,int PageSize)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******QuerytAuditInfo begin...currentUserID:" + currentUserID);
            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数
                string vermsg = string.Empty;
                
                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);
                #endregion
                //currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
                Chitunion2017.Common.UserRole userrole = Chitunion2017.Common.UserInfo.GetUserRole();
                int totalCount = 0;
                DataTable dt_WeChatOperateMsg = BLL.WeChatOperateMsg.Instance.GetWeChatOperateMsg(currentUserID, PageIndex, PageSize, out totalCount);
                JSONWeChatOperateMsg jsonResult_JSONWeChatOperateMsg = new JSONWeChatOperateMsg();
                jsonResult_JSONWeChatOperateMsg.TotalCount = totalCount;
                jsonResult_JSONWeChatOperateMsg.List = new List<JSONQueryWeChatOperateMsg>();
                if (dt_WeChatOperateMsg != null && dt_WeChatOperateMsg.Rows.Count > 0)
                {
                    foreach (DataRow row_WeChatOperateMsg in dt_WeChatOperateMsg.Rows)
                    {
                        Entities.WeChatOperateMsg model_WeChatOperateMsg = BLL.WeChatOperateMsg.Instance.DataRowToModel(row_WeChatOperateMsg);
                        JSONQueryWeChatOperateMsg jw = null;
                        //媒体主
                        if (userrole.IsMedia)
                        {
                            if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.已通过)
                            {                                
                                jw = new JSONQueryWeChatOperateMsg()
                                {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.发布刊例,
                                    Msg1 = "【媒体】",
                                    Msg2 = string.Format("恭喜，您提交的微信公众号[{0}]资料已通过审核，赶紧去<a class='red' href={1}>发布刊例</a>吧。", model_WeChatOperateMsg.MediaName,
                                        string.Format("/PublishManager/addEditPublishForWeiChat.html?MediaID={0}&entrance=1", model_WeChatOperateMsg.MediaID))
                                };
                            }
                            else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.驳回)
                            {
                                jw = new JSONQueryWeChatOperateMsg()
                                {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.修改,
                                    Msg1= "【媒体】",
                                    Msg2 = string.Format("您提交的微信公众号[{0}]资料未通过审核，您可以<a class='red' href={1}>修改</a>之后再次提交审核。", model_WeChatOperateMsg.MediaName,
                                        string.Format("/mediamanager/addWeChatmedia.html?WxID={0}&wxae=0", model_WeChatOperateMsg.MediaID))
                                };
                            }
                            else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                            {
                                jw = new JSONQueryWeChatOperateMsg()
                                {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.看看,
                                    Msg1 = "【广告】",
                                    Msg2 = string.Format("您好，您的微信公众号[{0}]的刊例[{1}]还有{2}天即将过期，赶紧去<a class='red' href={3}>看看</a>吧。", model_WeChatOperateMsg.MediaName,
                                    model_WeChatOperateMsg.PublishName,
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                        string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                };
                            }
                        }
                        else if (userrole.IsAE)
                        {
                            if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.已通过)
                            {                                
                                jw = new JSONQueryWeChatOperateMsg()
                                {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.无链接,
                                    Msg1 = "【广告】",
                                    Msg2 = string.Format("恭喜，您提交的微信公众号[{0}]的刊例已通过审核。", model_WeChatOperateMsg.MediaName),
                                    MsgUrl = string.Empty
                                };
                            }
                            else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.驳回)
                            {
                                jw = new JSONQueryWeChatOperateMsg()
                                {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.修改,
                                    Msg1 = "【广告】",
                                    Msg2 = string.Format("您好，您提交的微信公众号[{0}]的刊例未通过审核，您可以<a class='red' href={1}>修改</a>之后再次提交审核。", model_WeChatOperateMsg.MediaName,
                                        string.Format("/PublishManager/addEditPublishForWeiChat.html?PubID={0}&isAdd=1", model_WeChatOperateMsg.PublishID))
                                };
                            }
                            else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                            {
                                jw = new JSONQueryWeChatOperateMsg()
                                {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.看看,
                                    Msg1 = "【广告】",
                                    Msg2 = string.Format("您好，您的微信公众号[{0}]的刊例[{1}]还有{2}天即将过期，赶紧去<a class='red' href={2}>看看</a>吧。", model_WeChatOperateMsg.MediaName, model_WeChatOperateMsg.PublishName,
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                    string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                };
                            }
                        }
                        else //if (userrole.IsYY)
                        {
                            if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.待审核)
                            {                                
                                jw = new JSONQueryWeChatOperateMsg(){
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.去审,
                                    Msg1 = "【广告】",
                                    Msg2 = string.Format("刚刚有[{0}]提交了刊例要审核，立即<a class='red' href={1}>去审</a>", model_WeChatOperateMsg.SubmitUserName,
                                        string.Format("/PublishManager/advertisementCheck.html?PubID={0}", model_WeChatOperateMsg.PublishID))
                                };
                            }
                            else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.媒体审核 && model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.待审核)
                            {                                
                                jw = new JSONQueryWeChatOperateMsg() {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.去审,
                                    Msg1 = "【媒体】",
                                    Msg2 = string.Format("刚刚有[{0}]提交了媒体需要审核，立即<a class='red' href={1}>去审</a>。", model_WeChatOperateMsg.SubmitUserName,
                                        string.Format("/MediaManager/mediaaudit.html?mediaId={0}&MediaType=14001", model_WeChatOperateMsg.MediaID))
                                };
                            }
                            else if (model_WeChatOperateMsg.MsgType == (int)Entities.EnumWeChatOperateMsg.刊例过期)
                            {                                
                                jw = new JSONQueryWeChatOperateMsg() {
                                    CreateTime = model_WeChatOperateMsg.CreateTime,
                                    MsgType = (int)Entities.EnumWeChatOperateMsg.看看,
                                    Msg1 = "【广告】",
                                    Msg2 = string.Format("[{0}]下的公众号[{1}]的刊例[{2}]还有{3}天即将过期，赶紧去<a class='red' href={4}>看看</a>吧。", model_WeChatOperateMsg.SubmitUserName,
                                    model_WeChatOperateMsg.MediaName, model_WeChatOperateMsg.PublishName,
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                    model_WeChatOperateMsg.OptType == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0,
                                    string.Format("/PublishManager/periodication-see.html?PubID={0}&MediaType=14001", model_WeChatOperateMsg.PublishID))
                                };
                            }
                        }

                        if (jw != null)
                        {
                            jsonResult_JSONWeChatOperateMsg.List.Add(jw);
                        }
                    }

                    try
                    {
                        string listd = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult_JSONWeChatOperateMsg);
                        BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****QuerytAuditInfo->" + listd + "******");
                    }
                    catch (Exception ex)
                    { }
                    return Common.Util.GetJsonDataByResult(jsonResult_JSONWeChatOperateMsg, "操作成功");
                }
                else
                {
                    return Common.Util.GetJsonDataByResult(null, "没有数据", -1);
                }               
                
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]QuerytAuditInfo...->errormsg:" + ex.Message);                
                return Common.Util.GetJsonDataByResult(null, "出错："+ ex.Message, -1);
            }
        }

        #endregion

        #region 信息页更新为已读
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult UpdateAuditInfo()
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******UpdateAuditInfo begin...******");
            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数
                string vermsg = string.Empty;

                //if (!string.IsNullOrEmpty(vermsg))
                //    return Common.Util.GetJsonDataByResult(null, vermsg, -1);
                #endregion
                int totalCount = 0;
                vermsg = BLL.WeChatOperateMsg.Instance.p_WeChatOperateMsg_UpdateReadV1_1(2, currentUserID, out totalCount);
                
                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                return Common.Util.GetJsonDataByResult(null, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]UpdateAuditInfo...->errormsg:" + ex.Message);               
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }            
        }

        #endregion

        #region 根据微信号或名称模类查询V1.1
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult QueryWeChat_NumerOrName(string NumberORName, int AuditStatus)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]******QueryWeChat_NumerOrNameV1_1 begin...******");
            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数
                string vermsg = string.Empty;
                if (string.IsNullOrEmpty(NumberORName))
                    vermsg += "参数NumberORName为必填项/n";

                if (AuditStatus != (int)Entities.EnumWeChatOperateMsg.待审核 &&
                    AuditStatus != (int)Entities.EnumWeChatOperateMsg.已通过 &&
                    AuditStatus != (int)Entities.EnumWeChatOperateMsg.驳回 &&
                    AuditStatus != -2 &&
                    AuditStatus != -4)
                    vermsg += "参数AuditStatus错误/n";

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);
                #endregion
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                vermsg = BLL.ADOrderInfo.Instance.QueryWeChat_NumerOrNameV1_1(currentUserID, NumberORName, AuditStatus, out dicList);

                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                try
                {
                    string listd = Newtonsoft.Json.JsonConvert.SerializeObject(Common.Util.GetJsonDataByResult(dicList, "操作成功"));
                    BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]*****QueryWeChat_NumerOrNameV1_1->" + listd + "******");
                }
                catch (Exception ex)
                { }

                return Common.Util.GetJsonDataByResult(dicList, "操作成功");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoV1_1Controller]QueryWeChat_NumerOrNameV1_1...->errormsg:" + ex.Message);
                return Common.Util.GetJsonDataByResult(null, "出错：" + ex.Message, -1);
            }
        }
        #endregion


        #region JSON结果接口返回类
        public class JsonResult
        {
            public int error_code { get; set; }
            public string msg { get; set; }
            //public string data { get;set;}
            public object data { get; set; }
        }
        #endregion

        #region 主订单实体类
        public class JSONADOrderInfo
        {
            private string _OrderID = string.Empty;
            public string OrderID
            {
                get { return _OrderID; }
                set { _OrderID = value; }
            }

            public int MediaType { get; set; }
            public string OrderName { get; set; }
            public int Status { get; set; }
            private DateTime _BeginTime = Entities.Constants.Constant.DATE_INVALID_VALUE;
            public DateTime BeginTime
            {
                get { return _BeginTime; }
                set { _BeginTime = value; }
            }

            private DateTime _EndTime = Entities.Constants.Constant.DATE_INVALID_VALUE;
            public DateTime EndTime
            {
                get { return _EndTime; }
                set { _EndTime = value; }
            }

            private string _Note = string.Empty;
            public string Note
            {
                get { return _Note; }
                set { _Note = value; }
            }

            private string _UploadFileURL = string.Empty;
            public string UploadFileURL
            {
                get { return _UploadFileURL; }
                set { _UploadFileURL = value; }
            }


            private string _CustomerID= "gt86ZRCRjng%3d";
            public string CustomerID
            {
                get { return _CustomerID; }
                set { _CustomerID = value; }
            }

        }
        #endregion
        #region 子订单实体类
        public class JSONSubADInfo
        {
            public int MediaType { get; set; }
            public int MediaID { get; set; }
            public int Status { get; set; }
            public List<JSONADDetailInfo> ADDetailInfos { get; set; }
            //public string OrderID { get; set; }
            //public string SubOrderID { get; set; }
            //public decimal TotalAmount { get; set; }
            //public DateTime CreateTime { get; set; }
            //public int CreateUserID { get; set; }
        }
        #endregion

        #region 子订单详情实体类
        public class JSONADDetailInfo
        {           
            private int _MediaID = Entities.Constants.Constant.INT_INVALID_VALUE;
            public int MediaID
            {
                get { return _MediaID; }
                set { _MediaID = value; }
            }

            private int _PubDetailID = Entities.Constants.Constant.INT_INVALID_VALUE;
            public int PubDetailID
            {
                get { return _PubDetailID; }
                set { _PubDetailID = value; }
            }

            public decimal AdjustPrice { get; set; }
            public decimal AdjustDiscount { get; set; }
            public int ADLaunchDays { get; set; }
            public List<JSONADScheduleInfo2> ADScheduleInfos { get; set; }            
        }
        #endregion

        #region 排期实体类
        public class JSONADScheduleInfo
        {
            public int ADDetailID { get; set; }
            public string OrderID { get; set; }
            public string SubOrderID { get; set; }
            public int MediaID { get; set; }
            public int PubID { get; set; }
            public DateTime BeginData { get; set; }
            public DateTime EndData { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
        }
        public class JSONADScheduleInfo2
        {            
            public DateTime BeginData { get; set; }
            public DateTime EndData { get; set; }            
        }
        #endregion

        #region 主订单查看结果映射实体
        public class JSONResultADOrderInfo
        {
            public JSONADOrderInfo ADOrderInfo { get; set; }

            public List<JSONSubADInfo> SubADInfos { get; set; }

        }
        #endregion

        #region 子订单查看结果映射实体
        public class JSONResultSubADInfo
        {
            public JSONADOrderInfo ADOrderInfo { get; set; }

            public JSONSubADInfo SubADInfo { get; set; }

        }
        #endregion

        #region 主订单驳回原因实体类
        public class JSONADOrderOperInfo
        {
            public string OrderID { get; set; }
            public int OrderStatus { get; set; }
            public int OptType { get; set; }
            public string RejectMsg { get; set; }
            public DateTime CreateTime { get; set; }
        }
        #endregion

        #region 格式化媒体ID、广告位实体
        public class JSONmediadetail
        {
            public int MediaID { get; set; }
            public List<JSONADDetailInfo> Details { get; set; }
        }
        #endregion

        #region 提交、修改订单实体
        public class JSONAddOrUpadteRequest
        {
            public Entities.EnumAddModify optType { get; set; }
            public JSONADOrderInfo ADOrderInfo { get; set; }
            public List<JSONADDetailInfo> ADDetails { get; set; }
            public bool CheckSelfModel(out string msg)
            {
                StringBuilder sb = new StringBuilder();
                msg = string.Empty;
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
                    #region ADOrderInfo数据验证
                    if (!Enum.IsDefined(typeof(Entities.EnumMediaType), ADOrderInfo.MediaType))
                    {
                        sb.Append("媒体类型值不能为:"+ ADOrderInfo.MediaType +"!\n");
                    }

                    if (!Enum.IsDefined(typeof(Entities.EnumOrderStatus), ADOrderInfo.Status))
                    {
                        sb.Append("订单状态错误!不能为:" + ADOrderInfo.Status);
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
                    if (string.IsNullOrEmpty(ADOrderInfo.CustomerID))
                    {
                        sb.Append("广告主名称为必填项!\n");
                    }
                    if (ADDetails != null && ADDetails.Count > 0)
                    {
                        if (ADDetails.Count > 50)
                        {
                            sb.Append("广告位数量不能超过50!\n");
                        }
                        #region 广告位ID不能重复
                        if (ADOrderInfo.MediaType != (int)Entities.EnumMediaType.APP)
                        {
                            bool brepeat1 = ADDetails.GroupBy(l => l.PubDetailID).Where(g => g.Count() > 1).Count() > 0;
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
                        #region 排期精确到天不能重复
                        foreach (JSONADDetailInfo addetail in ADDetails)
                        {
                            if (addetail.ADScheduleInfos == null || addetail.ADScheduleInfos.Count == 0)
                            {
                                sb.Append("广告位至少应该有1个排期0!\n");
                            }
                            else if (addetail.ADScheduleInfos.Count > 3)
                            {
                                sb.Append("广告位排期最多3个!\n");
                            }
                            else
                            {
                                bool brepeat1 = addetail.ADScheduleInfos.GroupBy(l => l.EndData.ToShortDateString()).Where(g => g.Count() > 1).Count() > 0;
                                if (brepeat1)
                                {
                                    sb.Append("排期精确到天不能重复!\n");
                                }
                            }
                            
                        }
                        #endregion
                    }
                    else
                    {
                        sb.Append("没有广告位信息!\n");
                    }

                    //修改订单验证
                    if (optType == Entities.EnumAddModify.Modify)
                    {
                        if (string.IsNullOrEmpty(ADOrderInfo.OrderID))
                        {
                            sb.Append("修改订单操作订单号是必填项!\n");
                        }                        
                    }

                    #endregion
                }

                msg = sb.ToString();
                return msg.Length.Equals(0);
            }
        }
        #endregion

        #region 订单查看返回结果实体类（主订单、子订单查看通用）
        public class JSONQueryResultADOrerInfo
        {
            public JSONQueryADOrerInfo ADOrderInfo { get; set; }
            public List<JSONQuerySubADInfo> SubADInfos { get; set; }
        }
        public class JSONQuerySubADInfo
        {
            public string OrderID { get; set; }
            public string SubOrderID { get; set; }
            //public int MediaType { get; set; }
            public int MediaID { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public decimal TotalAmount { get; set; }
            public int Status { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
            public List<JSONQuerySelfDetail> SelfDetails { get; set; }
            public List<JSONQueryAPPDetail> APPDetails { get; set; }
            public List<JSONQuerySubOrderOperateInfo> OperateInfo { get; set; }
        }

        public class JSONQuerySubOrderOperateInfo
        {
            public string LastOrderStatus { get; set; }
            public string OrderStatus { get; set; }
            public string Creator { get; set; }
            public DateTime CreateTime { get; set; }
        }
        public class JSONQueryADOrerInfo
        {
            public string OrderID { get; set; }
            public int MediaType { get; set; }
            public string OrderName { get; set; }
            public int Status { get; set; }
            public DateTime BeginTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Note { get; set; }
            public string UploadFileURL { get; set; }
            public string UploadFileName { get; set; }
            public decimal TotalAmount { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
            public string RejectMsg { get; set; }
            public string CustomerID { get; set; }
            public string CreatorName { get; set; }
            public string CustomerName { get; set; }
            public string CreatorUserName { get; set; }
            public string CustomerUserName { get; set; }
        }
        public class JSONQuerySelfDetail
        {
            //public string OrderID { get; set; }
            //public string SubOrderID { get; set; }
            //public int MediaType { get; set; }
            //public int MediaID { get; set; }
            //public string Name { get; set; }
            //public string Number { get; set; }
            public string IsAuth { get; set; }
            public string Source { get; set; }
            public int PubID { get; set; }
            public DateTime PubBeginTime { get; set; }
            public DateTime PubEndTime { get; set; }
            public int PublishDetailID { get; set; }
            public int ADDetailID { get; set; }
            public string ADMasterImage { get; set; }
            public string ADMasterTitle { get; set; }
            public string ADPosition { get; set; }
            public string CreateType { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal AdjustPrice { get; set; }
            public decimal AdjustDiscount { get; set; }
            public decimal PurchaseDiscount { get; set; }
            public decimal SaleDiscount { get; set; }
            public int ADLaunchDays { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
            public int expired { get; set; }
            public List<JSONQeruySelfADSchedule> ADSchedule { get; set; }
        }
        public class JSONQeruySelfADSchedule
        {
            public DateTime BeginData { get; set; }
            public DateTime EndData { get; set; }
        }
        
        public class JSONQueryAPPDetail
        {
            public string OrderID { get; set; }
            public string SubOrderID { get; set; }
            public int MediaType { get; set; }
            public int MediaID { get; set; }
            public int PubID { get; set; }
            public int PublishDetailID { get; set; }
            public int ADDetailID { get; set; }
            public string Name { get; set; }
            public string HeadIconURL { get; set; }
            public string AdPosition { get; set; }
            public string AdForm { get; set; }
            public string Style { get; set; }
            public int CarouselCount { get; set; }
            public string PlayPosition { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal AdjustPrice { get; set; }
            public decimal AdjustDiscount { get; set; }
            public decimal PurchaseDiscount { get; set; }
            public decimal SaleDiscount { get; set; }
            public int ADLaunchDays { get; set; }
            public decimal Amount { get; set; }
            public int CPDCPM { get; set; }
            public string SysPlatform { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
            public List<JSONADScheduleInfo> ADScheduleInfos { get; set; }
        }
        #endregion

        #region 信息页实体
        public class JSONQueryWeChatOperateMsg
        {
            public int MsgType { get; set; }
            public string Msg1 { get; set; }
            public string Msg2 { get; set; }
            public string MsgUrl { get; set; }
            public DateTime CreateTime { get; set; }            
        }
        public class JSONWeChatOperateMsg
        {
            public List<JSONQueryWeChatOperateMsg> List { get; set; }
            public int TotalCount { get; set; }

        }
        #endregion
    }
}
