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

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ADOrderInfoController : ApiController
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
        //public Common.JsonResult AddOrUpdate_ADOrderInfo([FromBody] JSONAddOrUpadteRequest r)
        public Common.JsonResult AddOrUpdate_ADOrderInfo([FromBody] JObject r2)
        {           
            JSONAddOrUpadteRequest r = r2.ToObject<JSONAddOrUpadteRequest>();
            try
            {
                string listd = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...JSONAddOrUpadteRequest->" + listd + "******");
            }
            catch (Exception ex)
            { }
            Entities.EnumAddModify optType = r.optType;
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType);
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

            #region AE提交修改订单，验证广告主是否存在
            //gt86ZRCRjng,CustomerID为-2表示不验证广告主是否在存在
            if (r.ADOrderInfo.CustomerID == "gt86ZRCRjng%3d")
            { }
            else
            {
                string tmp1 = "";
                //解码
                tmp1 = System.Web.HttpUtility.UrlDecode(r.ADOrderInfo.CustomerID, Encoding.UTF8);
                
                int custid;
                try
                {
                    //解密
                    custid = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(tmp1, LoginPwdKey));
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType +",解密出错："+ ex.Message);
                    return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
                }
                
                vmsg = BLL.ADOrderInfo.Instance.p_ADMaster_IsExist(currentUserID, custid);
                if (!string.IsNullOrEmpty(vmsg))
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, vmsg, -1);
                }
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
                        string orderid = "";
                        AddOrderInfo(jorder, jdetails, out orderid);
                        jr = WebAPI.Common.Util.GetJsonDataByResult(null, orderid, 0);
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.Modify:
                        string msg = "";
                        ModifyOrderInfo(jorder, jdetails, out msg);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****ModifyOrderInfo 出错errormsg:" + msg);
                            jr = WebAPI.Common.Util.GetJsonDataByResult(null, msg, -1);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo 出错errormsg:" + ex.Message);
                jr = WebAPI.Common.Util.GetJsonDataByResult(null, "出错:" + ex.Message, -1);
            }
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...end...optType->" + optType + "，MediaTypeMediaType->" + r.ADOrderInfo.MediaType);
            return jr;
        }

        #region 测试
        //public Common.JsonResult AddOrUpdate_ADOrderInfo(Entities.EnumAddModify optType, string orderInfo, string subDetailInfo)
        //{
        //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->" + optType + "******");
        //    //订单插入记录的初始状态是草稿
        //    //修改订单不影响订单状态
        //    Common.JsonResult jr = new Common.JsonResult();
        //    jr.Message = "操作成功";
        //    //验证参数
        //    if (!Enum.IsDefined(typeof(Entities.EnumAddModify), optType))
        //    {
        //        jr.Status = 2;
        //        jr.Message = "参数optType：" + optType + ",值不存在";
        //        return jr;
        //    }


        //    JSONADOrderInfo jorder = new JSONADOrderInfo();
        //    List<JSONADDetailInfo> jdetails = new List<JSONADDetailInfo>();
        //    try
        //    {
        //        //反序列JSON到对象
        //        jorder = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONADOrderInfo>(orderInfo);
        //        jdetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JSONADDetailInfo>>(subDetailInfo);                
        //    }
        //    catch (Exception ex)
        //    {
        //        jr.Status = 2;
        //        jr.Message = ex.Message;
        //        return jr;
        //    }

        //    //DateTime dt = new DateTime();
        //    //if (!string.IsNullOrEmpty(jorder.BeginTime))
        //    //{
        //    //    if (!DateTime.TryParse(jorder.BeginTime, out dt))
        //    //    {
        //    //        jr.Status = 2;
        //    //        jr.Message = "主订单执行周期开始时间转换日期出错";
        //    //        return jr;
        //    //    }
        //    //}
        //    //if (!string.IsNullOrEmpty(jorder.EndTime))
        //    //{
        //    //    if (!DateTime.TryParse(jorder.EndTime, out dt))
        //    //    {
        //    //        jr.Status = 2;
        //    //        jr.Message = "主订单执行周期结束时间转换日期出错";
        //    //        return jr;
        //    //    }
        //    //}

        //    try
        //    {
        //        switch (optType)
        //        {
        //            case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.ADD:
        //                AddOrderInfo(jorder, jdetails);
        //                break;
        //            case XYAuto.ITSC.Chitunion2017.Entities.EnumAddModify.Modify:
        //                string msg = "";
        //                ModifyOrderInfo(jorder, jdetails,out msg);
        //                if (!string.IsNullOrEmpty(msg))
        //                {
        //                    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****ModifyOrderInfo 出错errormsg:" + msg);
        //                    jr.Status = 3;
        //                    jr.Message = msg;
        //                    return jr;
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo 出错errormsg:"+ ex.Message);
        //        jr.Status = 4;
        //        jr.Message = ex.Message;
        //        return jr;
        //    }

        //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->" + optType + "******");
        //    return jr;
        //}
        #endregion
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
            order.CRMCustomerID = orderInfo.CRMCustomerID;
            order.CustomerText = orderInfo.CustomerText;
            try
            {
                //解码
                orderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(orderInfo.CustomerID, Encoding.UTF8);
                //解密
                order.CustomerID = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(orderInfo.CustomerID, LoginPwdKey));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrderInfo 客户ID解码解密出错：->" + ex.Message);
            }

            orderid = BLL.ADOrderInfo.Instance.Insert(order);
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成订单：" + orderid);
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

                    //生成子订单
                    Entities.SubADInfo submodel = new Entities.SubADInfo();
                    submodel.OrderID = orderid;
                    submodel.MediaType = orderInfo.MediaType;
                    submodel.MediaID = info.Key;
                    submodel.Status = orderInfo.Status;
                    submodel.CreateTime = DateTime.Now;
                    submodel.CreateUserID = currentUserID;

                    suborderid = BLL.SubADInfo.Instance.Insert(submodel);
                    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成子订单：" + suborderid);

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
                    //更新子工单金额
                    BLL.SubADInfo.Instance.UpdateTotalAmmount_SubADInfo(demodel.AdjustPrice, suborderid);


                    int detailid = 0;
                    detailid = BLL.ADDetailInfo.Instance.Insert(demodel);
                    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成广告位：" + detailid);

                    #region APP-CPD排期
                    //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
                    if (item.MediaType == (int)Entities.EnumMediaType.APP)
                    {
                        cartids += item.PubDetailID + ",";
                        int cpd = 0;
                        if (int.TryParse(demodel.ADLaunchIDs, out cpd) && cpd == 11001)
                        {
                            //生成排期信息
                            /*
                            List<Entities.ADScheduleInfo> scmodelList = new List<Entities.ADScheduleInfo>();
                            Entities.ADScheduleInfo scmodel = new Entities.ADScheduleInfo();
                            scmodel.ADDetailID = detailid;
                            scmodel.OrderID = orderid;
                            scmodel.SubOrderID = suborderid;
                            scmodel.MediaID = item.MediaID;
                            scmodel.PubID = demodel.PubID;
                            scmodel.CreateTime = DateTime.Now;
                            scmodel.CreateUserID = currentUserID;

                            DateTime dt = new DateTime();
                            dt = DateTime.Now;
                            foreach (JSONADScheduleInfo sc in item.ADScheduleInfos)
                            {
                                scmodel.BeginData = sc.BeginData;
                                scmodel.EndData = sc.EndData;
                                BLL.ADScheduleInfo.Instance.Insert(scmodel);
                                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrUpdate_ADOrderInfo...begin...optType->ADD，MediaTypeMediaType->" + orderInfo.MediaType + ",生成排期：" + detailid);
                            }
                            */
                            if (item.ADScheduleInfos != null && item.ADScheduleInfos.Count > 0)
                            {
                                foreach (JSONADScheduleInfo sc in item.ADScheduleInfos)
                                {
                                    sc.ADDetailID = detailid;
                                    sc.OrderID = orderid;
                                    sc.SubOrderID = suborderid;
                                    sc.MediaID = item.MediaID;
                                    sc.PubID = demodel.PubID;
                                    sc.CreateTime = DateTime.Now;
                                    sc.CreateUserID = currentUserID;
                                    sc.BeginData = sc.BeginData;
                                    sc.EndData = sc.EndData;
                                }
                                System.Data.DataTable mydt = BLL.Util.ListToDataTable<JSONADScheduleInfo>(item.ADScheduleInfos);
                                BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            }
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrderInfo APP排期CPD转换int类型出错：->" + demodel.ADLaunchIDs);
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
            InsertMediaOrder(listDetail, orderid);
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
        public void ModifyOrderInfo(JSONADOrderInfo orderInfo, List<JSONADDetailInfo> listDetail, out string msg)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]ModifyOrderInfo->订单号:" + orderInfo.OrderID + ",媒体类型:" + orderInfo.MediaType);
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
            order.CRMCustomerID = orderInfo.CRMCustomerID;
            order.CustomerText = orderInfo.CustomerText;
            try
            {
                //解码
                orderInfo.CustomerID = System.Web.HttpUtility.UrlDecode(orderInfo.CustomerID, Encoding.UTF8);
                //解密
                order.CustomerID = Convert.ToInt32(XYAuto.Utils.Security.DESEncryptor.Decrypt(orderInfo.CustomerID, LoginPwdKey));
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****ModifyOrderInfo 客户ID解码解密出错：->" + ex.Message);
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
                    //生成子订单
                    Entities.SubADInfo submodel = new Entities.SubADInfo();
                    submodel.OrderID = orderid;
                    submodel.MediaType = orderInfo.MediaType;
                    submodel.MediaID = info.Key;
                    submodel.Status = order.Status;
                    submodel.CreateTime = order.CreateTime;
                    submodel.CreateUserID = order.CreateUserID;

                    suborderid = BLL.SubADInfo.Instance.Insert(submodel);

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
                    //更新子工单金额
                    BLL.SubADInfo.Instance.UpdateTotalAmmount_SubADInfo(demodel.AdjustPrice, suborderid);

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
                            /*
                            Entities.ADScheduleInfo scmodel = new Entities.ADScheduleInfo();
                            scmodel.ADDetailID = detailid;
                            scmodel.OrderID = orderid;
                            scmodel.SubOrderID = suborderid;
                            scmodel.MediaID = item.MediaID;
                            scmodel.PubID = demodel.PubID;
                            scmodel.CreateTime = DateTime.Now;
                            scmodel.CreateUserID = order.CreateUserID;

                            DateTime dt = new DateTime();
                            dt = DateTime.Now;
                            foreach (JSONADScheduleInfo sc in item.ADScheduleInfos)
                            {
                                scmodel.BeginData = sc.BeginData;
                                scmodel.EndData = sc.EndData;
                                BLL.ADScheduleInfo.Instance.Insert(scmodel);
                            }
                             */

                            if (item.ADScheduleInfos != null && item.ADScheduleInfos.Count > 0)
                            {
                                foreach (JSONADScheduleInfo sc in item.ADScheduleInfos)
                                {
                                    sc.ADDetailID = detailid;
                                    sc.OrderID = orderid;
                                    sc.SubOrderID = suborderid;
                                    sc.MediaID = item.MediaID;
                                    sc.PubID = demodel.PubID;
                                    sc.CreateTime = DateTime.Now;
                                    sc.CreateUserID = currentUserID;
                                    sc.BeginData = sc.BeginData;
                                    sc.EndData = sc.EndData;
                                }
                                System.Data.DataTable mydt = BLL.Util.ListToDataTable<JSONADScheduleInfo>(item.ADScheduleInfos);
                                BLL.ADScheduleInfo.Instance.Insert_BulkCopyToDB(mydt);
                            }
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****AddOrderInfo APP排期CPD转换int类型出错：->" + demodel.ADLaunchIDs);
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
            InsertMediaOrder(listDetail, orderid);
            #region 清空购物车-当前订单相关的
            if (cartids.EndsWith(","))
            {
                cartids = cartids.Substring(0, cartids.Length - 1);
            }
            //清空购物车
            ClearCartInfo(orderInfo.MediaType, cartids);
            #endregion
        }
        #region 生成项目媒体
        public void InsertMediaOrder(List<JSONADDetailInfo> listDetail, string orderid)
        {
            var queryADDetails = from x in listDetail
                                 select x.MediaType;
            var detailMediaType = queryADDetails.Distinct();
           
            foreach (var item in detailMediaType)
            {
                int recid = MediaOrderInfo.Instance.Insert(new Entities.MediaOrderInfo()
                {
                    MediaType = item,
                    Note = "注：补充中间表内容",
                    UploadFileURL = string.Empty,
                    OrderID = orderid,
                    CreateTime = DateTime.Now,
                    CreateUserID = currentUserID
                });               
            }           
        }
        #endregion
        #region 测试用
        //public Common.JsonResult AddOrUpdate_ADOrderInfo(JSONAddOrUpadteRequest jrequest)
        //{

        //    int optType = jrequest.optType;
        //    //订单插入记录的初始状态是草稿
        //    //修改订单不影响订单状态
        //    Common.JsonResult jr = new Common.JsonResult();
        //    jr.Message = "操作成功";
        //    return jr;
        //    //验证参数
        //    if (!Enum.IsDefined(typeof(Entities.EnumAddModify), optType))
        //    {
        //        jr.Status = 2;
        //        jr.Message = "参数optType：" + optType + ",值不存在";
        //        return jr;
        //    }


        //    JSONADOrderInfo jorder = new JSONADOrderInfo();
        //    List<JSONADDetailInfo> jdetails = new List<JSONADDetailInfo>();
        //    try
        //    {
        //        //反序列JSON到对象
        //        //jorder = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONADOrderInfo>(orderInfo);
        //        //jdetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JSONADDetailInfo>>(subDetailInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        jr.Status = 2;
        //        jr.Message = ex.Message;
        //        return jr;
        //    }

        //    DateTime dt = new DateTime();
        //    if (!DateTime.TryParse(jorder.BeginTime, out dt))
        //    {
        //        jr.Status = 2;
        //        jr.Message ="主订单执行周期开始时间转换日期出错";
        //        return jr;
        //    }
        //    if (!DateTime.TryParse(jorder.EndTime, out dt))
        //    {
        //        jr.Status = 2;
        //        jr.Message = "主订单执行周期结束时间转换日期出错";
        //        return jr;
        //    }
        //    try
        //    {
        //        //生成主订单               
        //        Entities.ADOrderInfo morder = new Entities.ADOrderInfo();
        //        morder.EndTime = DateTime.Now;
        //        morder.MediaType = 1;
        //    }
        //    catch (Exception ex)
        //    {               
        //    }
        //    return jr;
        //}







        //public JsonResult AddOrUpdate_ADOrderInfo(int optType, string orderInfo, string subDetailInfo)
        //{
        //    JsonResult jsonResult = new JsonResult();
        //    jsonResult.error_code = 0;

        //    if (optType == 1)
        //    {
        //        jsonResult.msg = "提交订单执行成功";
        //    }
        //    else if (optType == 2)
        //    {
        //        jsonResult.msg = "修改订单执行成功";
        //    }
        //    else
        //    {
        //        jsonResult.error_code = 1;
        //        jsonResult.msg = "参数optType错误";
        //    }
        //    try
        //    {
        //        Entities.ADOrderInfo model = new Entities.ADOrderInfo();
        //        model.BeginTime = DateTime.Now;
        //        model.EndTime = DateTime.Now;
        //        model.MediaType = 1;
        //        BLL.ADOrderInfo.Instance.InsertADOrderInfo(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonResult.error_code = 2;
        //        jsonResult.msg = ex.Message;
        //    }
        //    return jsonResult;
        //}
        #endregion
        #endregion

        #region 根据主订单号查询订单详情
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetByOrderID_ADOrderInfo(string orderid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo...begin...orderid->" + orderid + "******");
            Common.JsonResult jr = new Common.JsonResult();
            //参数验证
            if (string.IsNullOrEmpty(orderid))
            {
                jr.Status = 1;
                jr.Message = "订单号是必填项";
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",订单号是必填项");
                return jr;
            }

            #region 数据权限
            //string msg = string.Empty;
            //BLL.Util.GetSqlRightStr(Entities.EnumResourceType.ADOrderInfo, "", "", currentUserID, out msg);
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    jr.Status = -1;
            //    jr.Message = msg;
            //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",数据权限:"+ msg);
            //    return jr;
            //}
            #endregion

            JSONQueryResultADOrerInfo jsonResult = new JSONQueryResultADOrerInfo();
            //查询主订单信息
            Entities.ADOrderInfo ordermodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(orderid);
            if (ordermodel == null)
            {
                jr.Status = 1;
                jr.Message = "订单号:" + orderid + "不存在";
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",不存在");
                return jr;
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
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo 客户ID加密编码出错：->" + ex.Message);
            }
            orderinfo.CreatorName = ordermodel.CreatorName;
            orderinfo.CustomerName = ordermodel.CustomerName;
            orderinfo.CreatorUserName = ordermodel.CreatorUserName;
            orderinfo.CustomerUserName = ordermodel.CustomerUserName;
            orderinfo.CRMCustomerID = ordermodel.CRMCustomerID;
            orderinfo.CustomerText = ordermodel.CustomerText;

            //查询多条子订单信息
            List<JSONQuerySubADInfo> suborders = new List<JSONQuerySubADInfo>();
            System.Data.DataTable dt = new System.Data.DataTable();
            Entities.QuerySubADInfo querysub = new Entities.QuerySubADInfo();
            querysub.OrderID = orderid;
            int icount = 0;
            dt = BLL.SubADInfo.Instance.GetSubADInfo(querysub, "a.CreateTime DESC ", 1, 99999, out icount);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    Entities.SubADInfo m = BLL.SubADInfo.Instance.DataRowToModel(row);
                    JSONQuerySubADInfo suborder = new JSONQuerySubADInfo();
                    suborder.OrderID = m.OrderID;
                    suborder.SubOrderID = m.SubOrderID;
                    suborder.MediaType = m.MediaType;
                    suborder.MediaID = m.MediaID;
                    suborder.TotalAmount = m.TotalAmount;
                    suborder.Status = m.Status;
                    suborder.CreateTime = m.CreateTime;
                    suborder.CreateUserID = m.CreateUserID;

                    //子订单所属明细
                    Entities.QueryADDetailInfo query2 = new Entities.QueryADDetailInfo();
                    query2.SubOrderID = m.SubOrderID;
                    System.Data.DataTable dt2 = BLL.ADDetailInfo.Instance.GetADDetailInfo(query2, " a.CreateTime DESC ", 1, 99999, out icount);
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
                                selfdetail.OrderID = a.OrderID;
                                selfdetail.SubOrderID = a.SubOrderID;
                                selfdetail.MediaType = a.MediaType;
                                selfdetail.MediaID = a.MediaID;
                                selfdetail.PubID = a.PubID;
                                selfdetail.PublishDetailID = a.PubDetailID;
                                selfdetail.ADDetailID = a.RecID;


                                System.Data.DataTable selfDt;
                                if (a.PubDetailID <= 0)
                                {
                                    //没有广告位
                                    selfDt = BLL.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypeMediaID(a.MediaType, a.MediaID);
                                    if (selfDt != null && selfDt.Rows.Count > 0)
                                    {
                                        selfdetail.Name = selfDt.Rows[0]["Name"].ToString();
                                        selfdetail.Number = selfDt.Rows[0]["Number"].ToString();
                                        selfdetail.HeadIconURL = selfDt.Rows[0]["HeadIconURL"].ToString();
                                        selfdetail.ADType = string.Empty;
                                        selfdetail.AdPosition = string.Empty;
                                        selfdetail.AdForm = string.Empty;
                                        if (orderinfo.MediaType == (int)Entities.EnumMediaType.SinaWeibo)
                                        {
                                            selfdetail.Sign = selfDt.Rows[0]["Sign"].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    //获取自媒体广告位信息
                                    selfDt = BLL.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypePubDetailID(a.MediaType, a.PubDetailID);
                                    if (selfDt != null && selfDt.Rows.Count > 0)
                                    {
                                        selfdetail.Name = selfDt.Rows[0]["Name"].ToString();
                                        selfdetail.Number = selfDt.Rows[0]["Number"].ToString();
                                        selfdetail.HeadIconURL = selfDt.Rows[0]["HeadIconURL"].ToString();
                                        selfdetail.ADType = selfDt.Rows[0]["ADType"].ToString();
                                        selfdetail.AdPosition = selfDt.Rows[0]["AdPosition"].ToString();
                                        selfdetail.AdForm = selfDt.Rows[0]["AdForm"].ToString();
                                        if (orderinfo.MediaType == (int)Entities.EnumMediaType.SinaWeibo)
                                        {
                                            selfdetail.Sign = selfDt.Rows[0]["Sign"].ToString();
                                        }
                                    }
                                }

                                selfdetail.OriginalPrice = a.OriginalPrice;
                                selfdetail.AdjustPrice = a.AdjustPrice;
                                selfdetail.AdjustDiscount = a.AdjustDiscount;
                                selfdetail.PurchaseDiscount = a.PurchaseDiscount;
                                selfdetail.SaleDiscount = a.SaleDiscount;
                                selfdetail.ADLaunchDays = a.ADLaunchDays;
                                selfdetail.CreateTime = a.CreateTime;
                                selfdetail.CreateUserID = a.CreateUserID;

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
                        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",子订单明细没有数据");
                    }
                }
            }
            else
            {
                jr.Status = 2;
                jr.Message = "子订单没有数据";
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",子订单没有数据");
            }

            jsonResult.ADOrderInfo = orderinfo;
            jsonResult.SubADInfos = suborders;

            jr.Status = 0;
            jr.Message = "执行成功";
            jr.Result = jsonResult;

            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo...end...orderid->" + orderid + "******");
            return jr;
        }

        #region 测试用
        //public Common.JsonResult GetByOrderID_ADOrderInfo(string orderid)
        //{
        //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo...begin...orderid->" + orderid + "******");
        //    Common.JsonResult jr = new Common.JsonResult();
        //    //参数验证
        //    if (string.IsNullOrEmpty(orderid))
        //    {
        //        jr.Status = 1;
        //        jr.Message = "订单号是必填项";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",订单号是必填项");
        //        return jr;
        //    }

        //    JSONResultADOrderInfo jsonResult = new JSONResultADOrderInfo();
        //    //查询主订单信息
        //    Entities.ADOrderInfo ordermodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(orderid);
        //    if (ordermodel == null)
        //    {
        //        jr.Status = 1;
        //        jr.Message = "订单号:"+ orderid + "不存在";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",不存在");
        //        return jr;
        //    }
        //    JSONADOrderInfo orderinfo = new JSONADOrderInfo();
        //    orderinfo.OrderID = ordermodel.OrderID;
        //    orderinfo.OrderName = ordermodel.OrderName;
        //    orderinfo.BeginTime = ordermodel.BeginTime;
        //    orderinfo.EndTime = ordermodel.EndTime;
        //    orderinfo.Note = ordermodel.Note;
        //    orderinfo.UploadFileURL = ordermodel.UploadFileURL;
        //    orderinfo.MediaType = ordermodel.MediaType;
        //    orderinfo.TotalAmount = ordermodel.TotalAmount;
        //    orderinfo.Status = ordermodel.Status;
        //    orderinfo.CreateTime = ordermodel.CreateTime;
        //    orderinfo.CreateUserID = ordermodel.CreateUserID;

        //    //查询多条子订单信息
        //    List<JSONSubADInfo> suborders = new List<JSONSubADInfo>();
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    Entities.QuerySubADInfo querysub = new Entities.QuerySubADInfo();
        //    querysub.OrderID = orderid;
        //    int icount = 0;
        //    dt = BLL.SubADInfo.Instance.GetSubADInfo(querysub, "a.CreateTime DESC ", 1, 99999, out icount);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        foreach (System.Data.DataRow row in dt.Rows)
        //        {
        //            Entities.SubADInfo m = BLL.SubADInfo.Instance.DataRowToModel(row);
        //            JSONSubADInfo suborder = new JSONSubADInfo();
        //            suborder.OrderID = m.OrderID;
        //            suborder.SubOrderID = m.SubOrderID;
        //            suborder.MediaType = m.MediaType;
        //            suborder.MediaID = m.MediaID;
        //            suborder.TotalAmount = m.TotalAmount;
        //            suborder.Status = m.Status;
        //            suborder.CreateTime = m.CreateTime;
        //            suborder.CreateUserID = m.CreateUserID;

        //            //子订单所属明细
        //            Entities.QueryADDetailInfo query2 = new Entities.QueryADDetailInfo();
        //            query2.SubOrderID = m.SubOrderID;
        //            System.Data.DataTable dt2= BLL.ADDetailInfo.Instance.GetADDetailInfo(query2," a.CreateTime DESC ",1,99999,out icount);
        //            if (dt2 != null && dt2.Rows.Count > 0)
        //            {
        //                List<JSONADDetailInfo> details = new List<JSONADDetailInfo>();
        //                //遍历子订单所属明细
        //                foreach (System.Data.DataRow row2 in dt2.Rows)
        //                {
        //                    Entities.ADDetailInfo a = BLL.ADDetailInfo.Instance.DataRowToModel(row2);
        //                    JSONADDetailInfo detail = new JSONADDetailInfo();
        //                    detail.OrderID = a.OrderID;
        //                    detail.SubOrderID = a.SubOrderID;
        //                    detail.MediaType = a.MediaType;
        //                    detail.MediaID = a.MediaID;
        //                    detail.PubID = a.PubID;
        //                    detail.ADLaunchIDs = a.ADLaunchIDs;
        //                    detail.ADLaunchStr = a.ADLaunchStr;
        //                    detail.OriginalPrice = a.OriginalPrice;
        //                    detail.AdjustPrice = a.AdjustPrice;
        //                    detail.AdjustDiscount = a.AdjustDiscount;
        //                    detail.PurchaseDiscount = a.PurchaseDiscount;
        //                    detail.SaleDiscount = a.SaleDiscount;
        //                    detail.ADLaunchDays = a.ADLaunchDays;
        //                    detail.CreateTime = a.CreateTime;
        //                    detail.CreateUserID = a.CreateUserID;

        //                    //是否是APP CPD广告
        //                    //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
        //                    List<JSONADScheduleInfo> ADScheduleInfos = new List<JSONADScheduleInfo>();
        //                    if (a.MediaType == (int)Entities.EnumMediaType.APP)
        //                    {
        //                        //查询订单明细所属排期信息
        //                        System.Data.DataTable dt3 = BLL.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(a.RecID);
        //                        if (dt3 != null && dt3.Rows.Count > 0)
        //                        {
        //                            //遍历明细所属CPD排期
        //                            foreach (System.Data.DataRow row3 in dt3.Rows)
        //                            {
        //                                Entities.ADScheduleInfo adsmodel = BLL.ADScheduleInfo.Instance.DataRowToModel(row3);
        //                                JSONADScheduleInfo sc = new JSONADScheduleInfo();
        //                                sc.ADDetailID = adsmodel.ADDetailID;
        //                                sc.OrderID = adsmodel.OrderID;
        //                                sc.SubOrderID = adsmodel.SubOrderID;
        //                                sc.MediaID = adsmodel.MediaID;
        //                                sc.PubID = adsmodel.PubID;
        //                                sc.BeginData = adsmodel.BeginData;
        //                                sc.EndData = adsmodel.EndData;
        //                                sc.CreateTime = adsmodel.CreateTime;
        //                                sc.CreateUserID = adsmodel.CreateUserID;

        //                                ADScheduleInfos.Add(sc);
        //                            }                                    
        //                        }
        //                    }


        //                    detail.ADScheduleInfos = ADScheduleInfos;
        //                    details.Add(detail);
        //                }
        //                suborder.ADDetailInfos = details;
        //                suborders.Add(suborder);                                          
        //            }
        //            else
        //            {
        //                jr.Status = 3;
        //                jr.Message = "子订单明细没有数据";
        //                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",子订单明细没有数据");      
        //            }
        //        }
        //    }
        //    else
        //    {
        //        jr.Status = 2;
        //        jr.Message = "子订单没有数据";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo orderid->" + orderid + ",子订单没有数据");                
        //    }

        //    jsonResult.ADOrderInfo = orderinfo;
        //    jsonResult.SubADInfos = suborders;

        //    jr.Status = 0;
        //    jr.Message = "执行成功";
        //    //r2.data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
        //    jr.Result = jsonResult;

        //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetByOrderID_ADOrderInfo...end...orderid->" + orderid + "******");
        //    return jr;
        //}
        //public Common.JsonResult GetByOrderID_ADOrderInfo(string orderid)
        //{
        //    JSONResultADOrderInfo jsonResult = new JSONResultADOrderInfo();

        //    //查询主订单信息
        //    JSONADOrderInfo orderinfo = new JSONADOrderInfo();
        //    orderinfo.OrderID = "100";
        //    orderinfo.OrderName = "测试订单";
        //    orderinfo.BeginTime = DateTime.Now.ToString();
        //    orderinfo.EndTime = DateTime.Now.ToString();
        //    orderinfo.Note = "测试";
        //    orderinfo.UploadFileURL = "127.0.0.1/temp";
        //    orderinfo.MediaType = 1;  

        //    orderinfo.Status = 1;


        //    //查询多条子订单信息
        //    List<JSONSubADInfo> suborders = new List<JSONSubADInfo>();
        //    for (int i = 0; i < 1; i++)
        //    {
        //        JSONSubADInfo suborder = new JSONSubADInfo();
        //        suborder.OrderID = "sub" + i.ToString();
        //        suborder.SubOrderID = "sub:" + i.ToString();
        //        suborder.MediaType = i;
        //        suborder.Status = i;

        //        List<JSONADDetailInfo> details = new List<JSONADDetailInfo>();
        //        //遍历子订单所属明细
        //        for (int j = 0; j < 2; j++)
        //        {
        //            JSONADDetailInfo detail = new JSONADDetailInfo();
        //            detail.OrderID = "detail" + j.ToString();
        //            detail.OriginalPrice = 10 + j;

        //            List<JSONADScheduleInfo> ADScheduleInfos = new List<JSONADScheduleInfo>();
        //            //遍历明细所属CPD排期
        //            for (int k = 0; k < 3; k++)
        //            {
        //                JSONADScheduleInfo sc = new JSONADScheduleInfo();
        //                sc.BeginData = DateTime.Now.ToString();
        //                sc.EndData = DateTime.Now.ToString();
        //                sc.MediaID = k;
        //                sc.PubID = k + 1;

        //                ADScheduleInfos.Add(sc);
        //            }
        //            detail.ADScheduleInfos = ADScheduleInfos;
        //            details.Add(detail);
        //        }
        //        suborder.ADDetailInfos = details;
        //        suborders.Add(suborder);
        //    }

        //    jsonResult.ADOrderInfo = orderinfo;
        //    jsonResult.SubADInfos = suborders;

        //    Common.JsonResult r2 = new Common.JsonResult();
        //    r2.Status = 0;
        //    r2.Message = "执行成功";
        //    //r2.data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
        //    r2.Result = jsonResult;
        //    return r2;
        //}
        #endregion
        #endregion

        #region 根据子订单号接口查看子订单及主订单部分信息
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001BUT20001")]
        public Common.JsonResult GetBySubOrderID_ADOrderInfo(string suborderid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo...begin...suborderid->" + suborderid + "******");

            Common.JsonResult jr = new Common.JsonResult();
            //参数验证
            if (string.IsNullOrEmpty(suborderid))
            {
                jr.Status = 1;
                jr.Message = "订单号是必填项";
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",订单号是必填项");
                return jr;
            }

            #region 数据权限
            string msg = string.Empty;
            //BLL.Util.GetSqlRightStr(Entities.EnumResourceType.ADOrderInfo, "", "", currentUserID, out msg);
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    jr.Status = -1;
            //    jr.Message = msg;
            //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",数据权限:" + msg);
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
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",查询子订单信息出错:" + ex.Message);
                return jr;
            }

            if (m == null)
            {
                jr.Status = 1;
                jr.Message = "子订单号:" + suborderid + "不存在";
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo orderid->" + suborderid + ",不存在");
                return jr;
            }

            JSONQuerySubADInfo suborder = new JSONQuerySubADInfo();
            suborder.OrderID = m.OrderID;
            suborder.SubOrderID = m.SubOrderID;
            suborder.MediaType = m.MediaType;
            suborder.MediaID = m.MediaID;
            suborder.TotalAmount = m.TotalAmount;
            suborder.Status = m.Status;
            suborder.CreateTime = m.CreateTime;
            suborder.CreateUserID = m.CreateUserID;

            //子订单所属明细
            Entities.QueryADDetailInfo query2 = new Entities.QueryADDetailInfo();
            query2.SubOrderID = m.SubOrderID;
            int icount = 0;
            System.Data.DataTable dt2 = BLL.ADDetailInfo.Instance.GetADDetailInfo(query2, " a.CreateTime DESC ", 1, 99999, out icount);

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                List<JSONQueryAPPDetail> appdetails = new List<JSONQueryAPPDetail>();
                List<JSONQuerySelfDetail> selfdetails = new List<JSONQuerySelfDetail>();

                //判断APP还是自媒体
                if (suborder.MediaType == (int)Entities.EnumMediaType.APP)
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
                        selfdetail.OrderID = a.OrderID;
                        selfdetail.SubOrderID = a.SubOrderID;
                        selfdetail.MediaType = a.MediaType;
                        selfdetail.MediaID = a.MediaID;
                        selfdetail.PubID = a.PubID;
                        selfdetail.PublishDetailID = a.PubDetailID;
                        selfdetail.ADDetailID = a.RecID;

                        System.Data.DataTable selfDt;
                        if (a.PubDetailID <= 0)
                        {
                            //没有广告位
                            selfDt = BLL.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypeMediaID(a.MediaType, a.MediaID);
                            if (selfDt != null && selfDt.Rows.Count > 0)
                            {
                                selfdetail.Name = selfDt.Rows[0]["Name"].ToString();
                                selfdetail.Number = selfDt.Rows[0]["Number"].ToString();
                                selfdetail.HeadIconURL = selfDt.Rows[0]["HeadIconURL"].ToString();
                                selfdetail.ADType = string.Empty;
                                selfdetail.AdPosition = string.Empty;
                                selfdetail.AdForm = string.Empty;
                                if (selfdetail.MediaType == (int)Entities.EnumMediaType.SinaWeibo)
                                {
                                    selfdetail.Sign = selfDt.Rows[0]["Sign"].ToString();
                                }
                            }
                        }
                        else
                        {
                            //获取自媒体广告位信息
                            selfDt = BLL.ADOrderInfo.Instance.GetSelfMediaDetail_MediaTypePubDetailID(a.MediaType, a.PubDetailID);
                            if (selfDt != null && selfDt.Rows.Count > 0)
                            {
                                selfdetail.Name = selfDt.Rows[0]["Name"].ToString();
                                selfdetail.Number = selfDt.Rows[0]["Number"].ToString();
                                selfdetail.HeadIconURL = selfDt.Rows[0]["HeadIconURL"].ToString();
                                selfdetail.ADType = selfDt.Rows[0]["ADType"].ToString();
                                selfdetail.AdPosition = selfDt.Rows[0]["AdPosition"].ToString();
                                selfdetail.AdForm = selfDt.Rows[0]["AdForm"].ToString();
                                if (selfdetail.MediaType == (int)Entities.EnumMediaType.SinaWeibo)
                                {
                                    selfdetail.Sign = selfDt.Rows[0]["Sign"].ToString();
                                }
                            }
                        }



                        selfdetail.OriginalPrice = a.OriginalPrice;
                        selfdetail.AdjustPrice = a.AdjustPrice;
                        selfdetail.AdjustDiscount = a.AdjustDiscount;
                        selfdetail.PurchaseDiscount = a.PurchaseDiscount;
                        selfdetail.SaleDiscount = a.SaleDiscount;
                        selfdetail.ADLaunchDays = a.ADLaunchDays;
                        selfdetail.CreateTime = a.CreateTime;
                        selfdetail.CreateUserID = a.CreateUserID;



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
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo orderid->" + suborderid + ",子订单明细没有数据");
            }

            //查询主订单信息
            Entities.ADOrderInfo ordermodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(suborder.OrderID);
            if (ordermodel == null)
            {
                jr.Status = 1;
                jr.Message = "订单号:" + suborder.OrderID + "不存在";
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo orderid->" + suborder.OrderID + ",不存在");
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
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo 客户ID加密编码出错：->" + ex.Message);
            }
            orderinfo.CreatorName = ordermodel.CreatorName;
            orderinfo.CustomerName = ordermodel.CustomerName;
            orderinfo.CreatorUserName = ordermodel.CreatorUserName;
            orderinfo.CustomerUserName = ordermodel.CustomerUserName;
            orderinfo.CRMCustomerID = ordermodel.CRMCustomerID;
            orderinfo.CustomerText = ordermodel.CustomerText;

            List<JSONQuerySubADInfo> suborders = new List<JSONQuerySubADInfo>();
            suborders.Add(suborder);
            JSONQueryResultADOrerInfo jsonResult = new JSONQueryResultADOrerInfo();
            jsonResult.ADOrderInfo = orderinfo;
            jsonResult.SubADInfos = suborders;

            jr.Status = 0;
            jr.Message = "执行成功";
            jr.Result = jsonResult;

            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo...end...suborderid->" + suborderid + "******");
            return jr;
        }
        #region 测试
        //public Common.JsonResult GetBySubOrderID_ADOrderInfo(string suborderid)
        //{
        //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo...begin...suborderid->" + suborderid + "******");
        //    Common.JsonResult jr = new Common.JsonResult();
        //    //参数验证
        //    if (string.IsNullOrEmpty(suborderid))
        //    {
        //        jr.Status = 1;
        //        jr.Message = "订单号是必填项";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",订单号是必填项");
        //        return jr;
        //    }

        //    //查询子订单信息            
        //    Entities.SubADInfo m = new Entities.SubADInfo();
        //    try
        //    {
        //        m = BLL.SubADInfo.Instance.GetSubADInfo(suborderid);
        //    }
        //    catch (Exception ex)
        //    {
        //        jr.Status = 99;
        //        jr.Message = "查询子订单信息出错:" + ex.Message;
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo suborderid->" + suborderid + ",查询子订单信息出错:"+ ex.Message);
        //        return jr;
        //    }

        //    if (m == null)
        //    {
        //        jr.Status = 1;
        //        jr.Message = "子订单号:" + suborderid + "不存在";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo orderid->" + suborderid + ",不存在");
        //        return jr;
        //    }

        //    JSONSubADInfo suborder = new JSONSubADInfo();
        //    suborder.OrderID = m.OrderID;
        //    suborder.SubOrderID = m.SubOrderID;
        //    suborder.MediaType = m.MediaType;
        //    suborder.MediaID = m.MediaID;
        //    suborder.TotalAmount = m.TotalAmount;
        //    suborder.Status = m.Status;
        //    suborder.CreateTime = m.CreateTime;
        //    suborder.CreateUserID = m.CreateUserID;

        //    //子订单所属明细
        //    Entities.QueryADDetailInfo query2 = new Entities.QueryADDetailInfo();
        //    query2.SubOrderID = m.SubOrderID;
        //    int icount = 0;
        //    System.Data.DataTable dt2 = BLL.ADDetailInfo.Instance.GetADDetailInfo(query2, " a.CreateTime DESC ", 1, 99999, out icount);
        //    if (dt2 != null && dt2.Rows.Count > 0)
        //    {
        //        List<JSONADDetailInfo> details = new List<JSONADDetailInfo>();
        //        //遍历子订单所属明细
        //        foreach (System.Data.DataRow row2 in dt2.Rows)
        //        {
        //            Entities.ADDetailInfo a = BLL.ADDetailInfo.Instance.DataRowToModel(row2);
        //            JSONADDetailInfo detail = new JSONADDetailInfo();
        //            detail.OrderID = a.OrderID;
        //            detail.SubOrderID = a.SubOrderID;
        //            detail.MediaType = a.MediaType;
        //            detail.MediaID = a.MediaID;
        //            detail.PubID = a.PubID;
        //            detail.ADLaunchIDs = a.ADLaunchIDs;
        //            detail.ADLaunchStr = a.ADLaunchStr;
        //            detail.OriginalPrice = a.OriginalPrice;
        //            detail.AdjustPrice = a.AdjustPrice;
        //            detail.AdjustDiscount = a.AdjustDiscount;
        //            detail.PurchaseDiscount = a.PurchaseDiscount;
        //            detail.SaleDiscount = a.SaleDiscount;
        //            detail.ADLaunchDays = a.ADLaunchDays;
        //            detail.CreateTime = a.CreateTime;
        //            detail.CreateUserID = a.CreateUserID;

        //            //是否是APP CPD广告
        //            //CPD要保存排期信息ADLaunchIDs=11001为CPD广告
        //            List<JSONADScheduleInfo> ADScheduleInfos = new List<JSONADScheduleInfo>();
        //            if (a.MediaType == (int)Entities.EnumMediaType.APP)
        //            {
        //                //查询订单明细所属排期信息
        //                System.Data.DataTable dt3 = BLL.ADScheduleInfo.Instance.GetADScheduleInfoByDetailID(a.RecID);
        //                if (dt3 != null && dt3.Rows.Count > 0)
        //                {
        //                    //遍历明细所属CPD排期
        //                    foreach (System.Data.DataRow row3 in dt3.Rows)
        //                    {
        //                        Entities.ADScheduleInfo adsmodel = BLL.ADScheduleInfo.Instance.DataRowToModel(row3);
        //                        JSONADScheduleInfo sc = new JSONADScheduleInfo();
        //                        sc.ADDetailID = adsmodel.ADDetailID;
        //                        sc.OrderID = adsmodel.OrderID;
        //                        sc.SubOrderID = adsmodel.SubOrderID;
        //                        sc.MediaID = adsmodel.MediaID;
        //                        sc.PubID = adsmodel.PubID;
        //                        sc.BeginData = adsmodel.BeginData;
        //                        sc.EndData = adsmodel.EndData;
        //                        sc.CreateTime = adsmodel.CreateTime;
        //                        sc.CreateUserID = adsmodel.CreateUserID;

        //                        ADScheduleInfos.Add(sc);
        //                    }
        //                }
        //            }


        //            detail.ADScheduleInfos = ADScheduleInfos;
        //            details.Add(detail);
        //        }
        //        suborder.ADDetailInfos = details;
        //    }
        //    else
        //    {
        //        jr.Status = 3;
        //        jr.Message = "子订单明细没有数据";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo orderid->" + suborderid + ",子订单明细没有数据");
        //    }

        //    //查询主订单信息
        //    Entities.ADOrderInfo ordermodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(suborder.OrderID);
        //    if (ordermodel == null)
        //    {
        //        jr.Status = 1;
        //        jr.Message = "订单号:" + suborder.OrderID + "不存在";
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo orderid->" + suborder.OrderID + ",不存在");
        //        return jr;
        //    }
        //    JSONADOrderInfo orderinfo = new JSONADOrderInfo();
        //    orderinfo.OrderID = ordermodel.OrderID;
        //    orderinfo.OrderName = ordermodel.OrderName;
        //    orderinfo.BeginTime = ordermodel.BeginTime;
        //    orderinfo.EndTime = ordermodel.EndTime;
        //    orderinfo.Note = ordermodel.Note;
        //    orderinfo.UploadFileURL = ordermodel.UploadFileURL;
        //    orderinfo.MediaType = ordermodel.MediaType;
        //    orderinfo.TotalAmount = ordermodel.TotalAmount;
        //    orderinfo.Status = ordermodel.Status;
        //    orderinfo.CreateTime = ordermodel.CreateTime;
        //    orderinfo.CreateUserID = ordermodel.CreateUserID;

        //    List<JSONSubADInfo> suborders = new List<JSONSubADInfo>();
        //    suborders.Add(suborder);
        //    JSONResultADOrderInfo jsonResult = new JSONResultADOrderInfo();
        //    jsonResult.ADOrderInfo = orderinfo;
        //    jsonResult.SubADInfos = suborders;

        //    jr.Status = 0;
        //    jr.Message = "执行成功";
        //    //r2.data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
        //    jr.Result = jsonResult;

        //    BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****GetBySubOrderID_ADOrderInfo...end...suborderid->" + suborderid + "******");
        //    return jr;
        //}
        //public JsonResult GetBySubOrderID_ADOrderInfo(string suborderid)
        //{           
        //    //根据子订单号查询主订单信息
        //    JSONADOrderInfo orderinfo = new JSONADOrderInfo();
        //    orderinfo.OrderID = "100";
        //    orderinfo.MediaType = 1;
        //    orderinfo.Note = "测试";
        //    orderinfo.OrderName = "测试订单";
        //    orderinfo.Status = 1;
        //    orderinfo.UploadFileURL = "127.0.0.1/temp";
        //    orderinfo.BeginTime = DateTime.Now;
        //    orderinfo.EndTime = DateTime.Now;

        //    //根据子订单号查询子订单信息
        //    JSONSubADInfo suborder = new JSONSubADInfo();
        //    suborder.OrderID = "sub-orderid";
        //    suborder.SubOrderID = "sub-suborderid:";
        //    suborder.MediaType = 1;
        //    suborder.Status = 1;
        //    suborder.CreateTime = DateTime.Now;

        //    //根据子订单号查询所属多条明细信息
        //    List<JSONADDetailInfo> details = new List<JSONADDetailInfo>();
        //    //遍历子订单所属明细
        //    for (int j = 0; j < 2; j++)
        //    {
        //        JSONADDetailInfo detail = new JSONADDetailInfo();
        //        detail.OrderID = "detail" + j.ToString();
        //        detail.OriginalPrice = 10 + j;

        //        List<JSONADScheduleInfo> ADScheduleInfos = new List<JSONADScheduleInfo>();
        //        //遍历明细所属CPD排期
        //        for (int k = 0; k < 3; k++)
        //        {
        //            JSONADScheduleInfo sc = new JSONADScheduleInfo();
        //            sc.BeginData = DateTime.Now;
        //            sc.EndData = DateTime.Now;
        //            sc.MediaID = k;
        //            sc.PubID = k + 1;

        //            ADScheduleInfos.Add(sc);
        //        }
        //        detail.ADScheduleInfos = ADScheduleInfos;
        //        details.Add(detail);
        //    }
        //    suborder.ADDetailInfos = details;

        //    JSONResultSubADInfo jsonResult = new JSONResultSubADInfo();
        //    jsonResult.ADOrderInfo = orderinfo;
        //    jsonResult.SubADInfo = suborder;


        //    JsonResult r2 = new JsonResult();
        //    r2.error_code = 0;
        //    r2.msg = "执行成功";
        //    //r2.data = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult);
        //    r2.data = jsonResult;
        //    return r2;
        //}
        #endregion
        #endregion

        #region 根据子订号更改子订单状态接口
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult CancelOrDelete_SubADInfo(string suborderid, Entities.EnumInterfaceOrderStatus status)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****CancelOrDelete_SubADInfo...begin...suborderid->" + suborderid + "******");
            #region 功能点权限验证
            Common.JsonResult jrValidate = FunctionPointValidate_CancelOrDelete_SubADInfo(status);
            if (jrValidate.Status == 200)
            {
                return jrValidate;
            }
            #endregion

            Common.JsonResult jr = new Common.JsonResult();
            #region 验证参数
            string vermsg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.EnumInterfaceOrderStatus), status))
            {
                vermsg += "状态status：" + status + ",值不存在!\n";
            }
            if (string.IsNullOrEmpty(suborderid))
            {
                vermsg += "子订单号为必填项!\n";
            }
            if (Entities.EnumInterfaceOrderStatus.OrderFinished == status)
            {
                string pmsg = BLL.SubADInfo.Instance.p_SubADInfoOrderFeedbackData_Select(suborderid);
                if (!string.IsNullOrEmpty(pmsg))
                {
                    vermsg += pmsg + "\n";
                }
            }
            //AE权限验证
            //string tmpmsg = BLL.SubADInfo.Instance.p_SubADInfoStatus_UpdatePrivilege(currentUserID, suborderid, (int)status);
            //if (!string.IsNullOrEmpty(tmpmsg))
            //{
            //    vermsg += tmpmsg + "\n";
            //}
            if (!string.IsNullOrEmpty(vermsg))
                return Common.Util.GetJsonDataByResult(null, vermsg, -1);

            #endregion

            try
            {
                //取子订单当前状态
                Entities.SubADInfo model = BLL.SubADInfo.Instance.GetSubADInfo(suborderid);
                if (model != null)
                {
                    if (model.Status == (int)Entities.EnumInterfaceOrderStatus.OrderFinished)
                    {
                        jr.Message = "订单已完成，不允许再修改";
                        return jr;
                    }

                    if (model.Status == (int)Entities.EnumInterfaceOrderStatus.Executed)
                    {
                        if ((status == Entities.EnumInterfaceOrderStatus.Executing) || (status == Entities.EnumInterfaceOrderStatus.PendingExecute))
                        {
                            jr.Message = "订单执行完毕，不允许重复执行";
                            return jr;
                        }
                    }
                }
                else
                {
                    jr.Message = "订单：" + suborderid + "不存在";
                    return jr;
                }
                BLL.SubADInfo.Instance.UpdateStatus_SubADInfo(suborderid, (int)status);
                UpdateStatusADOrder(model.OrderID, (Entities.EnumOrderStatus)status);
                Entities.ADOrderOperateInfo model_ADOrderOperateInfo = new Entities.ADOrderOperateInfo() {
                    OptType=27003,
                    OrderID=model.OrderID,
                    SubOrderID=suborderid,
                    CreateUserID=currentUserID,
                    CurrentStatus=model.Status,
                    OrderStatus=(int)status
                };
                BLL.ADOrderOperateInfo.Instance.Insert(model_ADOrderOperateInfo);
                jr.Message = "操作成功";
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****CancelOrDelete_SubADInfo...出错，errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;
            }
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****CancelOrDelete_SubADInfo...end...suborderid->" + suborderid + "******");
            return jr;
        }
        #region 功能点权限验证
        public Common.JsonResult FunctionPointValidate_CancelOrDelete_SubADInfo(Entities.EnumInterfaceOrderStatus status)
        {
            string moduleID = "";
            switch (status)
            {
                case XYAuto.ITSC.Chitunion2017.Entities.EnumInterfaceOrderStatus.Cancel:
                    moduleID = "SYS001BUT20006";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumInterfaceOrderStatus.PendingExecute:
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumInterfaceOrderStatus.Executing:
                    moduleID = "SYS001BUT20005";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumInterfaceOrderStatus.Executed:
                    moduleID = "SYS001BUT20008";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumInterfaceOrderStatus.OrderFinished:
                    moduleID = "SYS001BUT20009";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumInterfaceOrderStatus.Deleted:
                    moduleID = "SYS001BUT20004";
                    break;
                default:
                    break;
            }
            Common.JsonResult jr = new Common.JsonResult();
            if (Chitunion2017.Common.UserInfo.CheckRight(moduleID, Chitunion2017.Common.UserInfo.SYSID))
            {
                jr = Common.Util.GetJsonDataByResult(false, "功能权限验证成功", 0);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
            }
            return jr;
        }
        #endregion
        public void UpdateStatusADOrder(string orderid, Entities.EnumOrderStatus status)
        {
            switch (status)
            {
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.Draft:
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.PendingAudit:
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.Reject:
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.Cancel:
                    //所有子订单都取消后更改项目为取消状态
                    BLL.SubADInfo.Instance.UpdateStatusADOrder_OrderID(orderid);
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.PendingExecute:
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.Executing:
                    BLL.ADOrderInfo.Instance.UpdateStatus_ADOrder(orderid, (int)status);
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.Executed:
                    //全部子订单都执行完毕后（除取消订单），项目改为执行完毕
                    BLL.SubADInfo.Instance.UpdateStatusADOrder_OrderID(orderid, (int)status);
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.OrderFinished:
                    //全部子订单都完成后（除取消订单），项目改为完成
                    BLL.SubADInfo.Instance.UpdateStatusADOrder_OrderID(orderid, (int)status);
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumOrderStatus.Deleted:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 审核主订单
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult Review_ADOrderInfo(string orderid, Entities.EnumOptType optType, string rejectMsg)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****Review_ADOrderInfo...begin...orderid->" + orderid + "******");
            Common.JsonResult jr = new Common.JsonResult();
            #region 验证参数
            string vermsg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.EnumOptType), optType))
            {
                vermsg += "参optType：" + optType + ",值不存在!\n";
            }
            if (string.IsNullOrEmpty(orderid))
            {
                vermsg += "主订单号为必填项!\n";
            }
            if (optType == Entities.EnumOptType.Reject && string.IsNullOrEmpty(rejectMsg))
            {
                vermsg += "驳回操作，驳回原因为必填项!\n";
            }
            //查询项目状态是否为待审
            Entities.ADOrderInfo admodel = new Entities.ADOrderInfo();
            admodel = BLL.ADOrderInfo.Instance.GetADOrderInfo(orderid);
            if (admodel != null && admodel.Status != (int)Entities.EnumOrderStatus.PendingAudit)
            {
                vermsg += "当前状态不允许审核操作!\n";
            }
            if (!string.IsNullOrEmpty(vermsg))
                return Common.Util.GetJsonDataByResult(null, vermsg, -1);

            #endregion
            try
            {
                //订单，审核通过：待执行，没通过：已驳回
                Entities.ADOrderOperateInfo model = new Entities.ADOrderOperateInfo();
                model.OptType = (int)optType;
                model.RejectMsg = rejectMsg;
                model.OrderID = orderid;
                model.CreateUserID = currentUserID;
                model.CurrentStatus = admodel.Status;
                switch (optType)
                {
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumOptType.Passed:
                        model.OrderStatus = (int)Entities.EnumOrderStatus.PendingExecute;
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumOptType.Reject:
                        model.OrderStatus = (int)Entities.EnumOrderStatus.Reject;
                        break;
                    default:
                        break;
                }
                BLL.ADOrderOperateInfo.Instance.Insert(model);

                //修改主订单状态
                BLL.ADOrderInfo.Instance.UpdateStatus_ADOrder(orderid, model.OrderStatus);

                //更改子订单状态
                SubADInfo.Instance.UpdateStatusByOrderID_SubADInfo(orderid, (int)model.OrderStatus);
                jr.Message = "操作成功";
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****Review_ADOrderInfo 出错,errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;
            }

            BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****Review_ADOrderInfo...end...orderid->" + orderid + "******");
            return jr;
        }
        #endregion

        #region 根据主订单号查询驳回原因
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001BUT20003")]
        public Common.JsonResult GetByOrderID_RejectMsg(string orderid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetByOrderID_RejectMsg begin...orderid->" + orderid + "******");
            Common.JsonResult jr = new Common.JsonResult();

            try
            {
                //查询驳回原因
                Entities.ADOrderOperateInfo model;
                model = BLL.ADOrderOperateInfo.Instance.GetADOrderOperateInfo_ByOrderID(orderid);

                if (model != null)
                {
                    JSONADOrderOperInfo jop = new JSONADOrderOperInfo();
                    jop.OrderID = orderid;
                    jop.RejectMsg = model.RejectMsg;
                    jop.CreateTime = model.CreateTime;
                    jop.OptType = model.OptType;
                    jop.OrderStatus = model.OrderStatus;

                    jr.Message = "订单：" + orderid + "查询驳回原因成功";
                    jr.Result = jop;
                }
                else
                {
                    jr.Status = 2;
                    jr.Message = "订单：" + orderid + "没有获取到驳回原因";
                }
            }
            catch (Exception ex)
            {
                jr.Status = 2;
                jr.Message = "订单：" + orderid + ",出错errormsg:" + ex.Message;
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetByOrderID_RejectMsg begin...orderid->" + orderid + ",出错errormsg:" + ex.Message);
            }
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetByOrderID_RejectMsg end...orderid->" + orderid + "******");
            return jr;
        }
        #endregion

        #region 根据广告位ID获取CPD排期信息
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetByDetailID_CPDScheduleInfo(int addetailinfoid)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetByDetailID_CPDScheduleInfo begin...addetailinfoid->" + addetailinfoid + "******");
            Common.JsonResult jr = new Common.JsonResult();
            //获取排期信息

            try
            {
                System.Data.DataTable dt;
                dt = BLL.ADScheduleInfo.Instance.GetADScheduleInfo_ByADDetailID(addetailinfoid);
                if (dt.Rows.Count == 0)
                {
                    jr.Message = "没有获取到排期信息";
                    return jr;
                }

                List<JSONADScheduleInfo> adSchedules = new List<JSONADScheduleInfo>();

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    JSONADScheduleInfo adschedule = new JSONADScheduleInfo();
                    if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                    {
                        adschedule.MediaID = int.Parse(row["MediaID"].ToString());
                    }
                    if (row["PubID"] != null && row["PubID"].ToString() != "")
                    {
                        adschedule.PubID = int.Parse(row["PubID"].ToString());
                    }
                    if (row["BeginData"] != null && row["BeginData"].ToString() != "")
                    {
                        adschedule.BeginData = DateTime.Parse(row["BeginData"].ToString());
                    }
                    if (row["EndData"] != null && row["EndData"].ToString() != "")
                    {
                        adschedule.EndData = DateTime.Parse(row["EndData"].ToString());
                    }
                    if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                    {
                        adschedule.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                    }
                    adSchedules.Add(adschedule);
                }
                jr.Message = "获取CPD广告位排期信息成功";
                jr.Result = adSchedules;
            }
            catch (Exception ex)
            {
                jr.Message = "出错,errormsg:" + ex.Message;
                jr.Status = 2;

                BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetByDetailID_CPDScheduleInfo...addetailinfoid->" + addetailinfoid + "出错,errormsg:" + ex.Message);
                return jr;
            }

            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetByDetailID_CPDScheduleInfo end...addetailinfoid->" + addetailinfoid + "******");
            return jr;
        }
        #endregion

        #region 刊例审核
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult Review_Publish(Entities.EnumMediaType mediaType, int publishID, Entities.EnumOptType optType, string rejectMsg)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******Review_Publish begin...publishID->" + publishID + ",optType->" + optType + "******");
            #region 功能点权限验证
            Common.JsonResult jrValidate = FunctionPointValidate_Review_Publish(mediaType);
            if (jrValidate.Status == 200)
            {
                return jrValidate;
            }
            #endregion

            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数
                string vermsg = string.Empty;
                if (!Enum.IsDefined(typeof(Entities.EnumMediaType), mediaType))
                {
                    vermsg = "参数媒体类型：" + mediaType + ",值不存在!\n";
                }
                if (!Enum.IsDefined(typeof(Entities.EnumOptType), optType))
                {
                    vermsg = "参数optType：" + optType + ",值不存在!\n";
                }
                if (optType == Entities.EnumOptType.Reject && string.IsNullOrEmpty(rejectMsg))
                {
                    vermsg = "驳回操作，驳回原因是必填项!\n";
                }
                //查询是否为待审
                if (BLL.PublishAuditInfo.Instance.GetStatus_PublishID(publishID) != (int)Entities.EnumPublishStatus.PendingAudit)
                {
                    vermsg = "当前状态不允许审核操作!\n";
                }
                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);
                #endregion
                //保存
                Entities.PublishAuditInfo puaiModel = new Entities.PublishAuditInfo();
                puaiModel.CreateUserID = currentUserID;
                puaiModel.MediaType = (int)mediaType;
                puaiModel.PublishID = publishID;
                puaiModel.OptType = (int)optType;

                switch (optType)
                {
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumOptType.Passed:
                        puaiModel.PubStatus = (int)Entities.EnumPublishStatus.Passed;
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumOptType.Reject:
                        puaiModel.PubStatus = (int)Entities.EnumPublishStatus.Reject;
                        puaiModel.RejectMsg = rejectMsg;
                        break;
                    default:
                        break;
                }

                BLL.PublishAuditInfo.Instance.Insert(puaiModel);

                //修改刊例主信息表status为“已通过”或“驳回”
                Entities.EnumPublishStatus pubstatus = Entities.EnumPublishStatus.Passed;
                switch (optType)
                {
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumOptType.Passed:
                        pubstatus = Entities.EnumPublishStatus.Passed;
                        break;
                    case XYAuto.ITSC.Chitunion2017.Entities.EnumOptType.Reject:
                        pubstatus = Entities.EnumPublishStatus.Reject;
                        break;
                    default:
                        break;
                }

                BLL.PublishAuditInfo.Instance.UpdateStatusByPubID_PublishBasic(publishID, (int)pubstatus);

            }
            catch (Exception ex)
            {
                jr.Status = 2;
                jr.Message = ex.Message;
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]******Review_Publish end...publishID->" + publishID + ",optType->" + optType + ",errormsg:" + ex.Message);
                return jr;
            }


            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******Review_Publish end...publishID->" + publishID + ",optType->" + optType + "******");
            jr.Message = "操作成功";
            return jr;
        }

        #region 功能点权限验证
        public Common.JsonResult FunctionPointValidate_Review_Publish(Entities.EnumMediaType mediaType)
        {
            string moduleID = "";
            switch (mediaType)
            {
                case XYAuto.ITSC.Chitunion2017.Entities.EnumMediaType.WeChat:
                    //Chitunion2017.Common.UserInfo.CheckRight(moduleID, Chitunion2017.Common.UserInfo.SYSID);
                    moduleID = "SYS001BUT500105";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumMediaType.APP:
                    moduleID = "SYS001BUT500505";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumMediaType.SinaWeibo:
                    moduleID = "SYS001BUT500205";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumMediaType.Video:
                    moduleID = "SYS001BUT500305";
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumMediaType.Broadcast:
                    moduleID = "SYS001BUT500405";
                    break;
                default:
                    break;
            }
            Common.JsonResult jr = new Common.JsonResult();
            if (Chitunion2017.Common.UserInfo.CheckRight(moduleID, Chitunion2017.Common.UserInfo.SYSID))
            {
                jr = Common.Util.GetJsonDataByResult(false, "功能权限验证成功", 0);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
            }
            return jr;
        }
        #endregion
        #endregion

        #region 根据主订单号更改主订单状态
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult UpdateStatus_ADOrderInfo(string orderid, Entities.EnumOrderStatus status)
        {
            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******UpdateStatus_ADOrderInfo begin...orerid->" + orderid + ",status->" + status + "******");
            Common.JsonResult jr = new Common.JsonResult();
            try
            {
                #region 验证参数
                string vermsg = string.Empty;
                if (!Enum.IsDefined(typeof(Entities.EnumOrderStatus), status))
                {
                    vermsg += "状态status：" + status + ",值不存在!\n";
                }
                if (string.IsNullOrEmpty(orderid))
                {
                    vermsg += "主订单号为必填项!\n";
                }
                if (!string.IsNullOrEmpty(vermsg))
                    return Common.Util.GetJsonDataByResult(null, vermsg, -1);

                #endregion
                //更改主订单状态
                ADOrderInfo.Instance.UpdateStatus_ADOrder(orderid, (int)status);
                //更改子订单状态
                SubADInfo.Instance.UpdateStatusByOrderID_SubADInfo(orderid, (int)status);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoControllerUpdateStatus_ADOrderInfo...orerid->" + orderid + ",errormsg:" + ex.Message);
                jr.Status = 2;
                jr.Message = ex.Message;

                return jr;
            }

            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******UpdateStatus_ADOrderInfo end...orerid->" + orderid + ",status->" + status + "******");
            jr.Message = "更新成功";
            return jr;
        }

        #endregion

        #region 根据个人姓名、公司名称或手机号模糊查询广告主
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetADMaster(string NameOrMobile,int IsAEAuth)
        {

            BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetADMaster begin...currentUserID->" + currentUserID + ",NameOrMobile->" + NameOrMobile + "******");
            try
            {
                System.Data.DataTable dt = BLL.ADOrderInfo.Instance.p_ADMaster_Select(currentUserID, NameOrMobile, IsAEAuth);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return WebAPI.Common.Util.GetJsonDataByResult(null, "没有数据", -1);
                }

                List<JSONADMaster> admasterlist = new List<JSONADMaster>();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    JSONADMaster admaster = new JSONADMaster();
                    admaster.UserID = Convert.ToString(row["UserID"]);
                    //加密
                    admaster.UserID = XYAuto.Utils.Security.DESEncryptor.Encrypt(admaster.UserID, LoginPwdKey);
                    //编码
                    admaster.UserID = System.Web.HttpUtility.UrlEncode(admaster.UserID, Encoding.UTF8);

                    admaster.UserName = Convert.ToString(row["UserName"]);
                    admaster.Mobile = Convert.ToString(row["Mobile"]);
                    admaster.TrueName = Convert.ToString(row["TrueName"]);
                    admasterlist.Add(admaster);
                }
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetADMaster end...currentUserID->" + currentUserID + ",NameOrMobile->" + NameOrMobile + "******");
                return WebAPI.Common.Util.GetJsonDataByResult(admasterlist, "", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]******GetADMaster end...currentUserID->" + currentUserID + ",NameOrMobile->" + NameOrMobile + ",errorMsg:" + ex.Message);
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion


        #region JSON根据个人姓名、公司名称或手机号模糊查询广告主
        public class JSONADMaster
        {
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string Mobile { get; set; }
            public string TrueName { get; set; }
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
            public string OrderID { get; set; }
            public int MediaType { get; set; }
            public string OrderName { get; set; }
            public int Status { get; set; }
            public DateTime BeginTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Note { get; set; }
            public string UploadFileURL { get; set; }
            public string CustomerID { get; set; }
            public string CRMCustomerID { get; set; } = string.Empty;
            public string CustomerText { get; set; } = string.Empty;
            //public decimal TotalAmount { get; set; }
            //public DateTime CreateTime { get; set; }
            //public int CreateUserID { get; set; }
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
            public int MediaType { get; set; }
            public int MediaID { get; set; }
            public int PubDetailID { get; set; }
            public decimal AdjustPrice { get; set; }
            public decimal AdjustDiscount { get; set; }
            public int ADLaunchDays { get; set; }
            public List<JSONADScheduleInfo> ADScheduleInfos { get; set; }
        }
        #endregion

        #region CPD排期实体类
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
                        foreach (JSONADDetailInfo detail in ADDetails)
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
                            //if (string.IsNullOrEmpty(ADOrderInfo.UploadFileURL))
                            //{
                            //    sb.Append("补充附件为必填项!\n");
                            //}
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
            public int MediaType { get; set; }
            public int MediaID { get; set; }
            public decimal TotalAmount { get; set; }
            public int Status { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
            public List<JSONQuerySelfDetail> SelfDetails { get; set; }
            public List<JSONQueryAPPDetail> APPDetails { get; set; }
        }
        public class JSONQueryADOrerInfo
        {
            public string CRMCustomerID { get; set; } = string.Empty;
            public string CustomerText { get; set; } = string.Empty;
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
            public string OrderID { get; set; }
            public string SubOrderID { get; set; }
            public int MediaType { get; set; }
            public int MediaID { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public string Sign { get; set; }
            public string HeadIconURL { get; set; }
            public int PubID { get; set; }
            public int PublishDetailID { get; set; }
            public int ADDetailID { get; set; }
            public string ADType { get; set; }
            public string AdPosition { get; set; }
            public string AdForm { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal AdjustPrice { get; set; }
            public decimal AdjustDiscount { get; set; }
            public decimal PurchaseDiscount { get; set; }
            public decimal SaleDiscount { get; set; }
            public int ADLaunchDays { get; set; }
            public DateTime CreateTime { get; set; }
            public int CreateUserID { get; set; }
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

        #region 根据条件查询订单列表
        /// <summary>
        /// 2017-02-24 张立彬
        /// 根据条件查询订单信息
        /// </summary>
        /// <param name="orderType">订单类型（0:主订单；1：子订单）</param>
        /// <param name="orderNum">  订单编号（模糊查找）</param>
        /// <param name="CustomerId">  广告主ID（精确查找）</param>
        /// <param name="demandDescribe">需求名称（模糊查询</param>
        /// <param name="mediaType">资源类型 （全部为0）</param>
        /// <param name="creater">创建人(模糊查询）</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        //[HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        //public Common.JsonResult SelectOrderInfo(int orderType, string orderNum, string CustomerID, string demandDescribe,  int mediaType, string creater, int orderState, int pagesize = 20, int PageIndex = 1)
        //{
        //    Dictionary<string, object> dic = new Dictionary<string, object>();
        //    try
        //    {
        //        //TODO: 代码走查 - 代码优化：bool变量,Add = masj,Date = 2017-05-10
        //        bool resultOrder = AuthorizationCommon.OrderSelectVerification(orderType);
        //        if (!resultOrder)
        //        {
        //            return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
        //        }
        //        dic = BLL.ADOrderInfo.Instance.SelectOrderInfo(orderType, orderNum == null ? orderNum : orderNum.Trim(), demandDescribe == null ? demandDescribe : demandDescribe.Trim(), mediaType, creater == null ? creater : creater.Trim(), pagesize, PageIndex, orderState, CustomerID,0,0);
        //    }
        //    catch (Exception ex)
        //    {
        //        BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****SelectOrderInfo ->orderNum:" + orderNum + " ->orderType:" + orderType + ",查询订单信息出错:" + ex.Message);
        //        throw ex;
        //    }
        //    return Common.Util.GetJsonDataByResult(dic, "Success");
        //}
        /// <summary>
        /// 2017-03-21 张立彬
        /// 根据项目ID查询订单信息
        /// </summary>
        /// <param name="orderID">项目ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectSubOrderByOrderID(string orderID)
        {

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                bool resultOrder = AuthorizationCommon.OrderSelectVerification(1);
                if (!resultOrder)
                {
                    return Common.Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                }
                list = BLL.ADOrderInfo.Instance.SelectSubOrderByOrderID(orderID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ADOrderInfoController]*****SelectSubOrderByOrderID ->orderID:" + orderID + ",查询订单信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(list, "Success");
        }
        #endregion
    }
}
