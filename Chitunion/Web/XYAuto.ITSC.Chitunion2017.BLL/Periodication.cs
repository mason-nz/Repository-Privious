using System;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017;
using XYAuto.ITSC.Chitunion2017.Entities;
using System.Collections.Generic;
using System.Linq;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    /// <summary>
    /// 2017-02-21 张立彬
    /// 刊例Dal
    /// </summary>
    public class Periodication
    {
        public static readonly BLL.Periodication Instance = new BLL.Periodication();
        #region V1.0
        /// <summary>
        /// 2017-02-22 张立彬
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubID">刊例ID</param>
        /// <returns></returns>
        public Entities.Periodication GetPublishInfoBymediaTypeAndPubID(int mediaType, int pubID)
        {
            string msg = "";
            int userID = Common.UserInfo.GetLoginUserID();
            string where = Util.GetSqlRightStr(EnumResourceType.MediaORPublish, "W", "CreateUserID", userID, out msg);
            DataSet ds = Dal.Periodication.Instance.GetPublishInfoBymediaTypeAndPubID(mediaType, pubID, where);
            Entities.Periodication per = new Entities.Periodication();
            #region 获取刊例信息
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {

                DataRow dw = ds.Tables[0].Rows[0];
                per.MediaID = dw["MediaID"] == DBNull.Value ? 0 : Convert.ToInt32(dw["MediaID"]);
                per.Name = dw["Name"] == DBNull.Value ? "" : dw["Name"].ToString();
                per.Number = dw["Number"] == DBNull.Value ? "" : dw["Number"].ToString();
                per.BeginTime = Convert.ToDateTime(dw["BeginTime"]);
                per.EndTime = Convert.ToDateTime(dw["EndTime"]);
                per.PurchaseDiscount = dw["PurchaseDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(dw["PurchaseDiscount"]);
                per.SaleDiscount = dw["SaleDiscount"] == DBNull.Value ? 0 : Convert.ToDecimal(dw["SaleDiscount"]);
                per.CheckTime = dw["LastUpdateTime"] == DBNull.Value ? "" : Convert.ToDateTime(dw["LastUpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                per.CheckUser = dw["UserName"] == DBNull.Value ? "" : dw["UserName"].ToString();
                per.CheckResult = dw["DictName"] == DBNull.Value ? "" : dw["DictName"].ToString();

                if (dw["publishStatus"].ToString() == "15004")
                {
                    if (dw["CheckedStatus"] == DBNull.Value)
                    {
                        per.CheckedStatus = "无";
                    }
                    else
                    {
                        per.CheckedStatus = dw["CheckedStatus"].ToString();
                    }
                }
                else
                {
                    per.CheckedStatus = ((PublishStatusEnum)Convert.ToInt32(dw["publishStatus"])).ToString();
                }
                #region 获取广告位信息集合
                List<PeriodicationFirst> listPerFiset = new List<PeriodicationFirst>();
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        string strFirst = ds.Tables[1].Rows[i]["ADPosition1"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["ADPosition1"].ToString();
                        Entities.PeriodicationFirst perFirst = new Entities.PeriodicationFirst();

                        List<Entities.PeriodicationMin> list = new List<Entities.PeriodicationMin>();

                        string firstPer = ds.Tables[1].Rows[i]["ADPosition1"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["ADPosition1"].ToString();
                        PeriodicationFirst FirstPer = listPerFiset.Where(t => t.First == firstPer).FirstOrDefault();
                        if (FirstPer != null)
                        {
                            PeriodicationMin perMin = new PeriodicationMin();
                            perMin.Combdimension = ds.Tables[1].Rows[i]["Combdimension"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["Combdimension"].ToString();
                            perMin.Second = ds.Tables[1].Rows[i]["ADPosition2"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["ADPosition2"].ToString();
                            perMin.Third = ds.Tables[1].Rows[i]["ADPosition3"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["ADPosition3"].ToString();
                            perMin.Price = ds.Tables[1].Rows[i]["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["Price"]);
                            FirstPer.SecondDescrit.Add(perMin);
                        }
                        else
                        {
                            PeriodicationFirst FirstPerNew = new PeriodicationFirst();
                            FirstPerNew.First = firstPer;
                            PeriodicationMin perMin = new PeriodicationMin();
                            perMin.Combdimension = ds.Tables[1].Rows[i]["Combdimension"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["Combdimension"].ToString();
                            perMin.Second = ds.Tables[1].Rows[i]["ADPosition2"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["ADPosition2"].ToString();
                            perMin.Third = ds.Tables[1].Rows[i]["ADPosition3"] == DBNull.Value ? "" : ds.Tables[1].Rows[i]["ADPosition3"].ToString();
                            perMin.Price = ds.Tables[1].Rows[i]["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[1].Rows[i]["Price"]);
                            FirstPerNew.SecondDescrit.Add(perMin);
                            listPerFiset.Add(FirstPerNew);
                        }
                    }
                }
                per.Detail = listPerFiset;
                #endregion
            }
            #endregion
            return per;
        }
        /// <summary>
        /// 2017-02-24 张立彬
        ///根据广告位ID查询APP媒体广告位信息
        /// </summary>
        /// <param name="ADDetailID">广告位ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetAppPublishInfoByAdvID(int ADDetailID)
        {
            string msg = "";
            int userID = Common.UserInfo.GetLoginUserID();
            string where = Util.GetSqlRightStr(EnumResourceType.MediaORPublish, "M", "CreateUserID", userID, out msg);
            DataTable dt = Dal.Periodication.Instance.GetAppPublishInfoByAdvID(ADDetailID, where);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("BeginTime", "");
            dic.Add("EndTime", "");
            dic.Add("AdLegendURL", "");
            dic.Add("AdPosition", "");
            dic.Add("AdForm", "");
            dic.Add("Style", "");
            dic.Add("DisplayLength", "");
            dic.Add("CanClick", "");
            dic.Add("IsDispatching", "");
            dic.Add("CarouselCount", "");
            dic.Add("PlayPosition", "");
            dic.Add("SysPlatform", "");
            dic.Add("DailyExposureCount", "");
            dic.Add("DailyClickCount", "");
            dic.Add("ThrMonitor ", "");
            dic.Add("SaleWay", "");
            dic.Add("BeginPlayDays", "");
            dic.Add("Price", "");
            dic.Add("AcceptBusinessIDs", "");
            dic.Add("NotAcceptBusinessIDs", "");
            dic.Add("ADShow", "");
            dic.Add("ADRemark", "");
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["BeginTime"] = dt.Rows[0]["BeginTime"];
                dic["EndTime"] = dt.Rows[0]["EndTime"];
                dic["AdLegendURL"] = dt.Rows[0]["AdLegendURL"];
                dic["AdPosition"] = dt.Rows[0]["AdPosition"];
                dic["AdForm"] = dt.Rows[0]["AdForm"];
                dic["Style"] = dt.Rows[0]["Style"];
                dic["DisplayLength"] = dt.Rows[0]["DisplayLength"];
                dic["CanClick"] = dt.Rows[0]["CanClick"];
                dic["IsDispatching"] = dt.Rows[0]["IsDispatching"];
                dic["CarouselCount"] = dt.Rows[0]["CarouselCount"];
                dic["PlayPosition"] = dt.Rows[0]["PlayPosition"];
                dic["SysPlatform"] = dt.Rows[0]["SysPlatform"];
                dic["DailyExposureCount"] = dt.Rows[0]["DailyExposureCount"];
                dic["DailyClickCount"] = dt.Rows[0]["DailyClickCount"];
                dic["ThrMonitor"] = dt.Rows[0]["ThrMonitor"];
                dic["SaleWay"] = dt.Rows[0]["SaleWay"];
                dic["BeginPlayDays"] = dt.Rows[0]["BeginPlayDays"];
                dic["Price"] = dt.Rows[0]["Price"];
                dic["AcceptBusinessIDs"] = dt.Rows[0]["AcceptBusinessIDs"];
                dic["NotAcceptBusinessIDs"] = dt.Rows[0]["NotAcceptBusinessIDs"];
                dic["ADShow"] = dt.Rows[0]["ADShow"];
                dic["ADRemark"] = dt.Rows[0]["ADRemark"];
            }
            return dic;


        }
        /// <summary>
        ///2017-03-01 张立彬
        /// 根据媒体ID或刊例ID查询刊例详情
        /// </summary>
        /// <param name="pubIDOrMediaID">媒体ID或刊例ID（mediaType==0时为刊例ID 否则为媒体ID）</param>
        ///  <param name="mediaType">媒体类型</param>
        /// <returns></returns>
        public List<PeriodicationDetaill> GetPublishBasicInfoByID(int pubIDOrMediaID, int mediaType)
        {
            List<PeriodicationDetaill> listPer = new List<PeriodicationDetaill>();
            DataTable dt = Dal.Periodication.Instance.GetPublishBasicInfoByID(pubIDOrMediaID, mediaType);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PeriodicationDetaill Per = new PeriodicationDetaill();
                    Per.ADDetailID = dt.Rows[i]["ADDetailID"] == null ? 0 : Convert.ToInt32(dt.Rows[i]["ADDetailID"].ToString());
                    Per.ADPosition1ID = dt.Rows[i]["ADPosition1ID"] == null ? "" : dt.Rows[i]["ADPosition1ID"].ToString();
                    Per.ADPosition2ID = dt.Rows[i]["ADPosition2ID"] == null ? "" : dt.Rows[i]["ADPosition2ID"].ToString();
                    Per.ADPosition3ID = dt.Rows[i]["ADPosition3ID"] == null ? "" : dt.Rows[i]["ADPosition3ID"].ToString();
                    Per.ADPosition1 = dt.Rows[i]["ADPosition1"] == null ? "" : dt.Rows[i]["ADPosition1"].ToString();
                    Per.ADPosition2 = dt.Rows[i]["ADPosition2"] == null ? "" : dt.Rows[i]["ADPosition2"].ToString();
                    Per.ADPosition3 = dt.Rows[i]["ADPosition3"] == null ? "" : dt.Rows[i]["ADPosition3"].ToString();
                    Per.Price = dt.Rows[i]["Price"];
                    listPer.Add(Per);
                }
            }
            return listPer;
        }
        /// <summary>
        /// 2017-03-02 张立彬
        ///根据刊例ID和其他条件查询APP刊例下广告位的信息列表
        /// </summary>
        /// <param name="pubID">刊例ID</param>
        /// <param name="publishStatus">发布状态</param>
        /// <param name="adPosition">广告位置</param>
        /// <param name="adForm">广告形式</param>
        /// <param name="style">广告样式</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        public ListTotal SelectAppAdvListByPubID(int pubID, int publishStatus, string adPosition, string adForm, string style, int pagesize, int PageIndex)
        {
            if (pagesize > Util.PageSize)
            {
                pagesize = Util.PageSize;
            }
            string msg = "";
            int userID = Common.UserInfo.GetLoginUserID();
            string where = Util.GetSqlRightStr(EnumResourceType.MediaORPublish, "M", "CreateUserID", userID, out msg);
            DataTable dt = Dal.Periodication.Instance.SelectAppAdvListByPubID(pubID, publishStatus, adPosition, adForm, style, pagesize, PageIndex, where);
            ListTotal lt = new ListTotal();

            if (dt != null && dt.Rows.Count > 0)
            {
                lt.TotalCount = Convert.ToInt32(dt.Columns["TotalCount"].Expression);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ADDetailID", dt.Rows[i]["ADDetailID"]);
                    dic.Add("AdLegendURL", dt.Rows[i]["AdLegendURL"]);
                    dic.Add("AdPosition", dt.Rows[i]["AdPosition"]);
                    dic.Add("AdForm", dt.Rows[i]["AdForm"]);
                    dic.Add("Style", dt.Rows[i]["Style"]);
                    dic.Add("DisplayLength", dt.Rows[i]["DisplayLength"]);
                    dic.Add("CanClick", dt.Rows[i]["CanClick"]);
                    dic.Add("IsDispatching", dt.Rows[i]["IsDispatching"]);
                    dic.Add("CarouselCount", dt.Rows[i]["CarouselCount"]);
                    dic.Add("PlayPosition", dt.Rows[i]["PlayPosition"]);
                    dic.Add("SysPlatform", dt.Rows[i]["SysPlatform"]);
                    dic.Add("DailyExposureCount", dt.Rows[i]["DailyExposureCount"]);
                    dic.Add("DailyClickCount", dt.Rows[i]["DailyClickCount"]);
                    dic.Add("ThrMonitor ", dt.Rows[i]["ThrMonitor"]);
                    dic.Add("SaleWay", dt.Rows[i]["SaleWay"]);
                    dic.Add("BeginPlayDays", dt.Rows[i]["BeginPlayDays"]);
                    dic.Add("Price", dt.Rows[i]["Price"]);
                    dic.Add("AcceptBusinessIDs", dt.Rows[i]["AcceptBusinessIDs"]);
                    dic.Add("NotAcceptBusinessIDs", dt.Rows[i]["NotAcceptBusinessIDs"]);
                    dic.Add("ADShow", dt.Rows[i]["ADShow"]);
                    dic.Add("ADRemark", dt.Rows[i]["ADRemark"]);
                    dic.Add("PublishStatus", dt.Rows[i]["PublishStatus"]);
                    dic.Add("CanAddToRecommend", dt.Rows[i]["CanAddToRecommend"]);
                    dic.Add("IsRange", dt.Rows[i]["IsRange"]);
                    lt.listDetail.Add(dic);
                }

            }
            return lt;
        }
        /// <summary>
        /// 2017-03-04 张立彬
        ///根据广告位ID查询APP刊例和广告位信息
        /// </summary>
        /// <param name="ADDetailID">广告位ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetAppPublishAdvInfoByAdvID(int ADDetailID)
        {
            string msg = "";
            int userID = Common.UserInfo.GetLoginUserID();
            string where = Util.GetSqlRightStr(EnumResourceType.MediaORPublish, "M", "CreateUserID", userID, out msg);
            DataTable dt = Dal.Periodication.Instance.GetAppPublishAdvInfoByAdvID(ADDetailID, where);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ADDetailID", "");
            dic.Add("PubID", "");
            dic.Add("AdLegendURL", "");
            dic.Add("AdPosition", "");
            dic.Add("AdForm", "");
            dic.Add("DisplayLength", "");
            dic.Add("CanClick", "");
            dic.Add("CarouselCount", "");
            dic.Add("PlayPosition", "");
            dic.Add("DailyExposureCount", "");
            dic.Add("CPM", "");
            dic.Add("CarouselPlay", "");
            dic.Add("DailyClickCount", "");
            dic.Add("CPM2", "");
            dic.Add("CarouselPlay2", "");
            dic.Add("ThrMonitor", "");
            dic.Add("SysPlatform", "");
            dic.Add("Style", "");
            dic.Add("IsDispatching", "");
            dic.Add("ADShow", "");
            dic.Add("ADRemark", "");
            dic.Add("AcceptBusinessIDs", "");
            dic.Add("NotAcceptBusinessIDs", "");
            dic.Add("IsCarousel", "");
            dic.Add("SaleType", "");
            dic.Add("Price", "");
            dic.Add("BeginPlayDays", "");

            if (dt != null && dt.Rows.Count > 0)
            {
                dic["ADDetailID"] = dt.Rows[0]["ADDetailID"];
                dic["PubID"] = dt.Rows[0]["PubID"];
                dic["AdLegendURL"] = dt.Rows[0]["AdLegendURL"];
                dic["AdPosition"] = dt.Rows[0]["AdPosition"];
                dic["AdForm"] = dt.Rows[0]["AdForm"];
                dic["DisplayLength"] = dt.Rows[0]["DisplayLength"];
                dic["CanClick"] = dt.Rows[0]["CanClick"];
                dic["CarouselCount"] = dt.Rows[0]["CarouselCount"];
                dic["PlayPosition"] = dt.Rows[0]["PlayPosition"];
                dic["DailyExposureCount"] = dt.Rows[0]["DailyExposureCount"];
                dic["CPM"] = dt.Rows[0]["CPM"];
                dic["CarouselPlay"] = dt.Rows[0]["CarouselPlay"];
                dic["DailyClickCount"] = dt.Rows[0]["DailyClickCount"];
                dic["CPM2"] = dt.Rows[0]["CPM2"];
                dic["CarouselPlay2"] = dt.Rows[0]["CarouselPlay2"];
                dic["ThrMonitor"] = dt.Rows[0]["ThrMonitor"];
                dic["SysPlatform"] = dt.Rows[0]["SysPlatform"];
                dic["Style"] = dt.Rows[0]["Style"];
                dic["IsDispatching"] = dt.Rows[0]["IsDispatching"];
                dic["ADShow"] = dt.Rows[0]["ADShow"];
                dic["ADRemark"] = dt.Rows[0]["ADRemark"];
                dic["AcceptBusinessIDs"] = dt.Rows[0]["AcceptBusinessIDs"];
                dic["NotAcceptBusinessIDs"] = dt.Rows[0]["NotAcceptBusinessIDs"];
                dic["IsCarousel"] = dt.Rows[0]["IsCarousel"];
                dic["SaleType"] = dt.Rows[0]["SaleType"];
                dic["Price"] = dt.Rows[0]["Price"];
                dic["BeginPlayDays"] = dt.Rows[0]["BeginPlayDays"];
            }
            return dic;
        }

        /// <summary>
        /// 2017-03-07 张立彬
        /// 根据媒体ID查询APP媒体刊例信息
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetAppMediaByMediaID(int MediaID)
        {
            string msg = "";
            int userID = Common.UserInfo.GetLoginUserID();
            string where = Util.GetSqlRightStr(EnumResourceType.MediaORPublish, "M", "CreateUserID", userID, out msg);
            DataTable dt = Dal.Periodication.Instance.GetAppMediaByMediaID(MediaID, where);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PubID", "");
            dic.Add("MediaName", "");
            dic.Add("MediaType", "");
            dic.Add("BeginTime", "");
            dic.Add("EndTime", "");
            dic.Add("SaleDiscount", "");
            dic.Add("PurchaseDiscount", "");
            dic.Add("StatusName", "");
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["PubID"] = dt.Rows[0]["PubID"];
                dic["MediaName"] = dt.Rows[0]["MediaName"];
                dic["MediaType"] = dt.Rows[0]["MediaType"];
                dic["BeginTime"] = dt.Rows[0]["BeginTime"];
                dic["EndTime"] = dt.Rows[0]["EndTime"];
                dic["SaleDiscount"] = dt.Rows[0]["SaleDiscount"];
                dic["PurchaseDiscount"] = dt.Rows[0]["PurchaseDiscount"];
                dic["StatusName"] = dt.Rows[0]["StatusName"];
            }
            return dic;
        }
        #endregion

        #region V1.1
        /// <summary>
        /// 根据刊例ID和媒体类型查询刊例信息 2017-04-21 张立彬
        /// </summary>
        /// <param name="PubID">刊例ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectWXPublishByIDAndType(int PubID, int MediaType)
        {
            string msg = "";
            int userID = Common.UserInfo.GetLoginUserID();
            // string where = Util.GetSqlRightStr(EnumResourceType.MediaORPublish, "W", "CreateUserID", userID, out msg);
            string where = "";
            DataSet ds = Dal.Periodication.Instance.SelectWXPublishByIDAndType(PubID, MediaType, where);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataSet dsChannel = null;
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dw = ds.Tables[0].Rows[0];
                dic.Add("MediaID", dw["MediaID"] == DBNull.Value ? "" : dw["MediaID"]);
                dic.Add("Name", dw["Name"] == DBNull.Value ? "" : dw["Name"]);
                dic.Add("Number", dw["Number"] == DBNull.Value ? "" : dw["Number"]);
                dic.Add("ADName", dw["ADName"] == DBNull.Value ? "" : dw["ADName"]);
                dic.Add("BeginTime", dw["BeginTime"] == DBNull.Value ? "" : Convert.ToDateTime(dw["BeginTime"]).ToString("yyyy-MM-dd"));
                dic.Add("EndTime", dw["EndTime"] == DBNull.Value ? "" : Convert.ToDateTime(dw["EndTime"]).ToString("yyyy-MM-dd"));
                dic.Add("PurchaseDiscount", dw["PurchaseDiscount"] == DBNull.Value ? 1 : dw["PurchaseDiscount"]);
                dic.Add("SaleDiscount", dw["SaleDiscount"] == DBNull.Value ? 1 : dw["SaleDiscount"]);
                dic.Add("Wx_Status", dw["Wx_Status"].ToString());
                dic.Add("CheckTime", dw["LastUpdateTime"] == DBNull.Value ? "" : Convert.ToDateTime(dw["LastUpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss"));
                dic.Add("CheckUser", dw["UserName"] == DBNull.Value ? "" : dw["UserName"]);
                dic.Add("CheckResult", dw["CheckResult"] == DBNull.Value ? "" : dw["CheckResult"]);


                if (dic["CheckResult"].ToString() == "驳回")
                {
                    dic.Add("RejectMsg", dw["RejectMsg"] == DBNull.Value ? "" : dw["RejectMsg"]);
                }
                else { dic.Add("RejectMsg", ""); }
                dic.Add("CustomerName", dw["CustomerName"] == DBNull.Value ? "" : dw["CustomerName"]);
                dic.Add("HeadIconURL", dw["HeadIconURL"] == DBNull.Value ? "" : dw["HeadIconURL"]);
                dic.Add("TrueName", dw["TrueName"].ToString());
                dic.Add("Mobile", dw["Mobile"]);
                dic.Add("Source", dw["Source"].ToString());
                dic.Add("OriginaReferencePrice", dw["OriginaReferencePrice"].ToString() == "" ? 0 : Convert.ToDecimal(dw["OriginaReferencePrice"]));
                #region 渠道信息
                if (dw["Source"].ToString() == "自营")
                {
                    dic.Add("ChannelInfo", new List<Dictionary<string, object>>());
                    List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
                    dsChannel = Dal.Periodication.Instance.SelectChannelInfoByMediaID(Convert.ToInt32(dic["MediaID"]), dic["BeginTime"].ToString(),dic["EndTime"].ToString());
                    if (dsChannel.Tables[0] != null && dsChannel.Tables[0].Rows.Count > 0 && dsChannel.Tables[1] != null && dsChannel.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsChannel.Tables[0].Rows.Count; i++)
                        {
                            Dictionary<string, object> dicChannel = new Dictionary<string, object>();
                            DataRow dr = dsChannel.Tables[0].Rows[i];
                            dicChannel.Add("CostID", dr["CostID"].ToString());
                            dicChannel.Add("ChannelName", dr["ChannelName"].ToString());
                            dicChannel.Add("CooperateBeginDate", dr["CooperateBeginDate"]);
                            dicChannel.Add("CooperateEndDate", dr["CooperateEndDate"]);
                            listDic.Add(dicChannel);
                        }
                        dic["ChannelInfo"] = listDic;
                    }

                }
                #endregion
                #region 上一次的刊例信息
                dic.Add("OldBeginTime", "");
                dic.Add("OldEndTime", "");
                dic.Add("OldPurchaseDiscount", "");
                dic.Add("OldSaleDiscount", "");
                //dic.Add("OldIsAppointment", "");
                //dic.Add("OldRemarks", "");
                #endregion


                #region 刊例备注
                //dic.Add("RemarksID", "");
                //dic.Add("Remarks", "");
                //dic.Add("OtherContent", "");
                //if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                //{
                //    string RemarksID = "";
                //    string Remarks = "";
                //    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //    {
                //        RemarksID += ds.Tables[2].Rows[i]["RemarkID"].ToString() + ",";
                //        if (ds.Tables[2].Rows[i]["RemarkID"].ToString() == "4009")
                //        {
                //            Remarks += ds.Tables[2].Rows[i]["OtherContent"].ToString() + "、";
                //            dic["OtherContent"] = ds.Tables[2].Rows[i]["OtherContent"].ToString();
                //        }
                //        else
                //        {
                //            Remarks += ds.Tables[2].Rows[i]["DictName"].ToString() + "、";
                //        }
                //    }

                //    dic["RemarksID"] = RemarksID.Remove(RemarksID.Count() - 1, 1);
                //    dic["Remarks"] = Remarks.Remove(Remarks.Count() - 1, 1);
                //}
                #endregion

                #region 刊例附件
                dic.Add("SingleFile", "");
                dic.Add("SingleFileName", "");
                dic.Add("UploadFileList", new List<Dictionary<string, object>>());
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listFile = new List<Dictionary<string, object>>();

                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        Dictionary<string, object> dic1 = new Dictionary<string, object>();
                        DataRow dw1 = ds.Tables[2].Rows[i];

                        string fileName = dw1["FileUrl"].ToString();
                        dic1.Add("UploadFile", fileName);
                        if (fileName.Contains("/"))
                        {
                            string fileName1 = fileName.Substring(fileName.LastIndexOf('/') + 1);
                            if (fileName1.Contains("$"))
                            {
                                dic1.Add("UploadFileName", fileName1.Substring(0, fileName1.LastIndexOf('$')) + "." + fileName1.Substring(fileName1.LastIndexOf('.') + 1));
                                listFile.Add(dic1);
                            }
                        }

                    }
                    dic["UploadFileList"] = listFile;
                    string SingleFileName = ds.Tables[2].Rows[0]["FileUrl"].ToString();
                    dic["SingleFile"] = SingleFileName;
                    if (SingleFileName.Contains("/"))
                    {
                        string SingleFileName1 = SingleFileName.Substring(SingleFileName.LastIndexOf('/') + 1);
                        if (SingleFileName1.Contains("$"))
                        {
                            dic["SingleFileName"] = SingleFileName1.Substring(0, SingleFileName1.LastIndexOf('$')) + "." + SingleFileName1.Substring(SingleFileName1.LastIndexOf('.') + 1);
                        }
                    }
                }
                #endregion
                OldPublishDTO publishtinfo = null;
                #region 加载上一次刊例信息
                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    object obj = ds.Tables[3].Rows[0]["RejectMsg"];
                    if (obj != null)
                    {
                        publishtinfo = JsonConvert.DeserializeObject<OldPublishDTO>(obj.ToString());
                        if (publishtinfo != null)
                        {
                            dic["OldBeginTime"] = publishtinfo.BeginTime;
                            dic["OldEndTime"] = publishtinfo.EndTime;
                            dic["OldPurchaseDiscount"] = publishtinfo.PurchaseDiscount;
                            dic["OldSaleDiscount"] = publishtinfo.SaleDiscount;
                            //dic["OldIsAppointment"] = publishtinfo.IsAppointment;
                            //dic["OldRemarks"] = publishtinfo.OrderRemarkName;
                        }

                    }
                }

                #endregion
                #region 刊例下广告位
                dic.Add("Detail", new Dictionary<string, object>());
                dic.Add("OldDetail", new List<Dictionary<string, object>>());
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                List<string> listNewRecID = new List<string>();

                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                {
                    DataRow dw1 = ds.Tables[1].Rows[j];
                    Dictionary<string, object> dic1 = new Dictionary<string, object>();
                    dic1.Add("RecID", dw1["RecID"]);
                    dic1.Add("Combdimension", dw1["Combdimension"] == DBNull.Value ? "" : dw1["Combdimension"]);
                    dic1.Add("ADPosition1Code", dw1["ADPosition1Code"] == DBNull.Value ? "" : dw1["ADPosition1Code"]);
                    dic1.Add("ADPosition2Code", dw1["ADPosition2Code"] == DBNull.Value ? "" : dw1["ADPosition2Code"]);
                    dic1.Add("ADPosition1", dw1["ADPosition1"] == DBNull.Value ? "" : dw1["ADPosition1"]);
                    dic1.Add("ADPosition2", dw1["ADPosition2"] == DBNull.Value ? "" : dw1["ADPosition2"]);
                    dic1.Add("Price", dw1["Price"] == DBNull.Value ? 0 : dw1["Price"]);
                    dic1.Add("SalePrice", dw1["SalePrice"] == DBNull.Value ? 0 : dw1["SalePrice"]);
                    dic1.Add("CostReferencePrice", dw1["CostReferencePrice"] == DBNull.Value ? 0 : dw1["CostReferencePrice"]);
                    dic1.Add("ImgUrl1", dw1["ImgUrl1"] == DBNull.Value ? "" : dw1["ImgUrl1"]);
                    dic1.Add("ImgUrl2", dw1["ImgUrl2"] == DBNull.Value ? "" : dw1["ImgUrl2"]);
                    dic1.Add("ImgUrl3", dw1["ImgUrl3"] == DBNull.Value ? "" : dw1["ImgUrl3"]);
                    dic1.Add("ImgUrl1-sl", "");
                    dic1.Add("ImgUrl2-sl", "");
                    dic1.Add("ImgUrl3-sl", "");
                    #region 上一次广告位信息
                    dic1.Add("OldPrice", "");
                    dic1.Add("OldSalePrice", "");

                    if (publishtinfo != null)
                    {
                        foreach (var item in publishtinfo.Details)
                        {
                            if (item.ADPosition == dw1["Combdimension"].ToString())
                            {
                                dic1["OldPrice"] = item.Price;
                                dic1["OldSalePrice"] = item.SalePrice;
                                listNewRecID.Add(dw1["Combdimension"].ToString());
                            }
                        }
                    }
                    #endregion

                    #region 图片信息
                    string ImgUrl1 = dic1["ImgUrl1"].ToString();
                    string ImgUrl2 = dic1["ImgUrl2"].ToString();
                    string ImgUrl3 = dic1["ImgUrl3"].ToString();
                    if (ImgUrl1 != "")
                    {
                        int LastIndex = ImgUrl1.LastIndexOf('.');
                        string SlImg = ImgUrl1.Substring(0, LastIndex) + "_sl." + ImgUrl1.Substring(LastIndex + 1, ImgUrl1.Length - LastIndex - 1);
                        dic1["ImgUrl1-sl"] = SlImg;
                    }
                    if (ImgUrl2 != "")
                    {
                        int LastIndex = ImgUrl2.LastIndexOf('.');
                        string SlImg = ImgUrl2.Substring(0, LastIndex) + "_sl." + ImgUrl2.Substring(LastIndex + 1, ImgUrl2.Length - LastIndex - 1);
                        dic1["ImgUrl2-sl"] = SlImg;
                    }
                    if (ImgUrl3 != "")
                    {
                        int LastIndex = ImgUrl3.LastIndexOf('.');
                        string SlImg = ImgUrl3.Substring(0, LastIndex) + "_sl." + ImgUrl3.Substring(LastIndex + 1, ImgUrl3.Length - LastIndex - 1);
                        dic1["ImgUrl3-sl"] = SlImg;
                    }
                    #endregion

                    #region 渠道信息
                    if (dw["Source"].ToString() == "自营")
                    {

                        dic1.Add("ChannelPriceInfo", new List<int>());
                        if (dsChannel.Tables[0] != null && dsChannel.Tables[0].Rows.Count > 0 && dsChannel.Tables[1] != null && dsChannel.Tables[1].Rows.Count > 0)
                        {
                            List<int> listPirce = new List<int>();
                            DataTable newdt = null;
                            DataRow[] dr = dsChannel.Tables[1].Select("ADPosition1=" + Convert.ToInt32(dic1["ADPosition1Code"]) + " AND ADPosition3=" + Convert.ToInt32(dic1["ADPosition2Code"]));
                            if (dr != null && dr.Count() > 0)
                            {
                                newdt = dr[0].Table.Clone();
                                for (int i = 0; i < dr.Length; i++)
                                {
                                    newdt.ImportRow(dr[i]);
                                }
                            }
                            List<Dictionary<string, object>> listChannel = dic["ChannelInfo"] as List<Dictionary<string, object>>;

                            for (int i = 0; i < listChannel.Count(); i++)
                            {
                                if (dr != null && dr.Count() > 0 && newdt != null)
                                {
                                    DataRow row = newdt.Select("CostID=" + Convert.ToInt32(listChannel[i]["CostID"])).FirstOrDefault();
                                    if (row != null)
                                    {
                                        listPirce.Add(Convert.ToInt32(row["SalePrice"]));
                                        listPirce.Add(Convert.ToInt32(row["CostPrice"]));
                                    }
                                    else
                                    {
                                        listPirce.Add(0);
                                        listPirce.Add(0);
                                    }
                                }
                                else
                                {
                                    listPirce.Add(0);
                                    listPirce.Add(0);
                                }
                            }


                            dic1["ChannelPriceInfo"] = listPirce;
                        }

                    }
                    #endregion

                    list.Add(dic1);
                }
                dic["Detail"] = list;
                if (publishtinfo != null)
                {
                    List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
                    foreach (var item in publishtinfo.Details)
                    {
                        if (listNewRecID.Where(t => t == item.ADPosition).Count() <= 0)
                        {
                            Dictionary<string, object> dicOldDetails = new Dictionary<string, object>();
                            dicOldDetails.Add("Combdimension", item.ADPosition);
                            dicOldDetails.Add("CombdimensionName", item.ADPositionName);
                            dicOldDetails.Add("Price", item.Price);
                            dicOldDetails.Add("SalePrice", item.SalePrice);
                            listDic.Add(dicOldDetails);
                        }
                    }
                    dic["OldDetail"] = listDic;
                }

                #endregion
            }
            return dic;
        }
        /// <summary>
        /// 查询购物车刊例信息 2017-04-27 张立彬
        /// </summary>
        /// <param name="MediaType"></param>
        /// <param name="MediaID"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectShoppingPublish(int MediaType, int MediaID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();

            DataSet ds = Dal.Periodication.Instance.SelectShoppingPublish(MediaType, MediaID);
            dicAll.Add("PublishInfo", new List<Dictionary<string, object>>());
            dicAll.Add("PublishDetail", new List<Dictionary<string, object>>());
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                List<Dictionary<string, object>> list1 = new List<Dictionary<string, object>>();
                List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
                string HeadImg = "";
                string QrCodeUrl = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    DataRow dw = ds.Tables[0].Rows[i];
                    dic.Add("PubID", dw["PubID"]);
                    dic.Add("Wx_Status", dw["Wx_Status"]);
                    dic.Add("MainTitle", dw["MainTitle"] == DBNull.Value ? dw["Subtitle"] : dw["MainTitle"]);
                    dic.Add("Subtitle", dw["Subtitle"]);
                    dic.Add("BeginTime", dw["BeginTime"]);
                    dic.Add("EndTime", dw["EndTime"]);
                    dic.Add("SaleStyle", dw["SaleStyle"]);
                    dic.Add("ServiceType", dw["ServiceType"]);
                    dic.Add("IsVerify", dw["IsVerify"]);
                    dic.Add("HeadImg", dw["HeadImg"] == DBNull.Value ? "" : dw["HeadImg"]);
                    dic.Add("QrCodeUrl", dw["QrCodeUrl"] == DBNull.Value ? "" : dw["QrCodeUrl"]);
                    dic.Add("DifferDay", dw["DifferDay"]);
                    dic.Add("RemarkName", dw["RemarkName"].ToString());

                    dic.Add("HeadImg-sl", dic["HeadImg"]);
                    dic.Add("QrCodeUrl-sl", dic["QrCodeUrl"]);
                    HeadImg = dic["HeadImg"].ToString();
                    QrCodeUrl = dic["QrCodeUrl"].ToString();
                    list1.Add(dic);
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    DataRow dw = ds.Tables[1].Rows[i];
                    dic.Add("Wx_Status", dw["Wx_Status"]);
                    dic.Add("ADDetailID", dw["ADDetailID"]);
                    dic.Add("PubID", dw["PubID"]);
                    dic.Add("ADPosition1", dw["ADPosition1"]);
                    dic.Add("ADPosition2", dw["ADPosition2"]);
                    dic.Add("SalePrice", dw["SalePrice"] == DBNull.Value ? "" : dw["SalePrice"]);
                    dic.Add("ImageUrl1", dw["ImageUrl1"]);
                    dic.Add("ImageUrl2", dw["ImageUrl2"]);
                    dic.Add("ImageUrl3", dw["ImageUrl3"]);
                    string ImgUrl1 = dic["ImageUrl1"].ToString();
                    string ImgUrl2 = dic["ImageUrl2"].ToString();
                    string ImgUrl3 = dic["ImageUrl3"].ToString();
                    dic.Add("ImgUrl1-sl", "");
                    dic.Add("ImgUrl2-sl", "");
                    dic.Add("ImgUrl3-sl", "");

                    dic.Add("HeadImg", HeadImg);
                    dic.Add("QrCodeUrl", QrCodeUrl);
                    dic.Add("HeadImg-sl", HeadImg);
                    dic.Add("QrCodeUrl-sl", QrCodeUrl);

                    if (ImgUrl1 != "")
                    {
                        int LastIndex = ImgUrl1.LastIndexOf('.');
                        string SlImg = ImgUrl1.Substring(0, LastIndex) + "_sl." + ImgUrl1.Substring(LastIndex + 1, ImgUrl1.Length - LastIndex - 1);
                        dic["ImgUrl1-sl"] = SlImg;
                    }
                    if (ImgUrl2 != "")
                    {
                        int LastIndex = ImgUrl2.LastIndexOf('.');
                        string SlImg = ImgUrl2.Substring(0, LastIndex) + "_sl." + ImgUrl2.Substring(LastIndex + 1, ImgUrl2.Length - LastIndex - 1);
                        dic["ImgUrl2-sl"] = SlImg;
                    }
                    if (ImgUrl3 != "")
                    {
                        int LastIndex = ImgUrl3.LastIndexOf('.');
                        string SlImg = ImgUrl3.Substring(0, LastIndex) + "_sl." + ImgUrl3.Substring(LastIndex + 1, ImgUrl3.Length - LastIndex - 1);
                        dic["ImgUrl3-sl"] = SlImg;
                    }
                    list2.Add(dic);
                }
                dicAll["PublishInfo"] = list1;
                dicAll["PublishDetail"] = list2;
            }


            List<Dictionary<string, object>> list4 = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> list5 = new List<Dictionary<string, object>>();
            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                DataRow dw = ds.Tables[2].Rows[i];
                dic.Add("DictId", dw["DictId"]);
                dic.Add("DictName", dw["DictName"]);
                list4.Add(dic);
            }
            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                DataRow dw = ds.Tables[3].Rows[i];
                dic.Add("DictId", dw["DictId"]);
                dic.Add("DictName", dw["DictName"]);
                list5.Add(dic);
            }
            dicAll.Add("AllADPosition1", list4);
            dicAll.Add("AllADPosition2", list5);
            if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
            {
                DataRow dw = ds.Tables[4].Rows[0];
                dicAll.Add("Subtitle", dw["Subtitle"]);
                dicAll.Add("HeadImg", dw["HeadImg"]);
                dicAll.Add("QrCodeUrl", dw["QrCodeUrl"]);
                dicAll.Add("HeadImg-sl", "");
                dicAll.Add("QrCodeUrl-sl", "");
                string ImgUrl1 = dicAll["HeadImg"].ToString();
                string ImgUrl2 = dicAll["QrCodeUrl"].ToString();
                if (ImgUrl1 != "")
                {

                    dicAll["HeadImg-sl"] = ImgUrl1;
                }
                if (ImgUrl2 != "")
                {

                    dicAll["QrCodeUrl-sl"] = ImgUrl2;
                }
            }
            return dicAll;
            #region
            //dic.Add("SelectADPosition1", "");
            //dic.Add("SelectADPosition2", "");
            //dic.Add("SalePrice", "");
            //var result = (from DataRow order in ds.Tables[1].Rows
            //              where order["PubID"].ToString() == PubId.ToString()
            //              orderby order["SalePrice"]
            //              select new { SelectADPosition1 = order["ADPosition1"], SelectADPosition2 = order["ADPosition2"], SalePrice = order["SalePrice"] }).FirstOrDefault();
            //if (result != null)
            //{
            //    dic["SelectADPosition1"] = result.SelectADPosition1;
            //    dic["SelectADPosition2"] = result.SelectADPosition2;
            //    dic["SalePrice"] = result.SalePrice;
            //var resultPubID = (from DataRow order in ds.Tables[1].Rows
            //                   where order["ADPosition1"].ToString() == result.SelectADPosition1.ToString() & order["ADPosition2"].ToString() == result.SelectADPosition2.ToString()
            //                   select new { PubId = order["PubID"].ToString() }).GroupBy(t => t.PubId).ToArray();
            #endregion
        }
        #endregion
        #region V1.1.1
        /// <summary>
        ///查询广告下的政策列表及对应的广告位信息集合 张立彬 2017-05-15
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <returns>政策列表集合</returns>
        public Dictionary<string, object> SelectPublishesByMediaID(int MediaType, int MediaID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();

            DataSet ds = Dal.Periodication.Instance.SelectPublishesByMediaID(MediaType, MediaID);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dw = ds.Tables[0].Rows[0];
                dicAll.Add("MediaID", dw["MediaID"]);
                dicAll.Add("HeadIconURL", dw["HeadIconURL"].ToString());
                dicAll.Add("Number", dw["Number"].ToString());
                dicAll.Add("Name", dw["Name"].ToString());
                dicAll.Add("ADName", dw["ADName"].ToString());
                dicAll.Add("PublishInfo", new List<Dictionary<string, object>>());
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listPublish = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicPublish = new Dictionary<string, object>();
                        DataRow dwPublish = ds.Tables[1].Rows[i];
                        dicPublish.Add("PubID", dwPublish["PubID"]);
                        dicPublish.Add("BeginTime", dwPublish["BeginTime"]);
                        dicPublish.Add("EndTime", dwPublish["EndTime"]);
                        dicPublish.Add("PurchaseDiscount", dwPublish["PurchaseDiscount"]);
                        dicPublish.Add("SaleDiscount", dwPublish["SaleDiscount"]);
                        dicPublish.Add("PublishStatus", dwPublish["PublishStatus"]);
                        dicPublish.Add("CheckResult", dwPublish["CheckResult"].ToString());
                        dicPublish.Add("RejectMsg", dwPublish["RejectMsg"].ToString());
                        dicPublish.Add("CheckTime", dwPublish["CheckTime"].ToString());
                        dicPublish.Add("CheckMan", dwPublish["CheckMan"].ToString());
                        dicPublish.Add("PublishDetail", new List<Dictionary<string, object>>());
                        dicPublish.Add("UploadFileList", new List<Dictionary<string, object>>());
                        if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                        {
                            DataRow[] drArr = ds.Tables[2].Select("PubID=" + dwPublish["PubID"]);
                            if (drArr != null)
                            {

                                List<Dictionary<string, object>> listDetails = new List<Dictionary<string, object>>();
                                for (int j = 0; j < drArr.Count(); j++)
                                {
                                    Dictionary<string, object> dicDetails = new Dictionary<string, object>();
                                    dicDetails.Add("ADDetailID", drArr[j]["ADDetailID"]);
                                    dicDetails.Add("ADPosition1", drArr[j]["ADPosition1"]);
                                    dicDetails.Add("ADPosition2", drArr[j]["ADPosition2"]);
                                    dicDetails.Add("ADPosition1Name", drArr[j]["ADPosition1Name"]);
                                    dicDetails.Add("ADPosition2Name", drArr[j]["ADPosition2Name"]);
                                    dicDetails.Add("Price", drArr[j]["Price"]);
                                    dicDetails.Add("SalePrice", drArr[j]["SalePrice"]);
                                    dicDetails.Add("ImageUrl1", drArr[j]["ImageUrl1"] == null ? "" : drArr[j]["ImageUrl1"].ToString());
                                    dicDetails.Add("ImageUrl2", drArr[j]["ImageUrl2"] == null ? "" : drArr[j]["ImageUrl2"].ToString());
                                    dicDetails.Add("ImageUrl3", drArr[j]["ImageUrl3"] == null ? "" : drArr[j]["ImageUrl3"].ToString());
                                    string ImgUrl1 = dicDetails["ImageUrl1"].ToString();
                                    string ImgUrl2 = dicDetails["ImageUrl2"].ToString();
                                    string ImgUrl3 = dicDetails["ImageUrl3"].ToString();
                                    dicDetails.Add("ImageUrl1-sl", "");
                                    dicDetails.Add("ImageUrl2-sl", "");
                                    dicDetails.Add("ImageUrl3-sl", "");
                                    if (ImgUrl1 != "")
                                    {
                                        int LastIndex = ImgUrl1.LastIndexOf('.');
                                        string SlImg = ImgUrl1.Substring(0, LastIndex) + "_sl." + ImgUrl1.Substring(LastIndex + 1, ImgUrl1.Length - LastIndex - 1);
                                        dicDetails["ImageUrl1-sl"] = SlImg;
                                    }
                                    if (ImgUrl2 != "")
                                    {
                                        int LastIndex = ImgUrl2.LastIndexOf('.');
                                        string SlImg = ImgUrl2.Substring(0, LastIndex) + "_sl." + ImgUrl2.Substring(LastIndex + 1, ImgUrl2.Length - LastIndex - 1);
                                        dicDetails["ImageUrl2-sl"] = SlImg;
                                    }
                                    if (ImgUrl3 != "")
                                    {
                                        int LastIndex = ImgUrl3.LastIndexOf('.');
                                        string SlImg = ImgUrl3.Substring(0, LastIndex) + "_sl." + ImgUrl3.Substring(LastIndex + 1, ImgUrl3.Length - LastIndex - 1);
                                        dicDetails["ImageUrl3-sl"] = SlImg;
                                    }
                                    listDetails.Add(dicDetails);
                                }
                                dicPublish["PublishDetail"] = listDetails;
                            }
                            DataRow[] drArr1 = ds.Tables[3].Select("PubID=" + dwPublish["PubID"]);
                            if (drArr1 != null)
                            {
                                List<Dictionary<string, object>> listFileList = new List<Dictionary<string, object>>();
                                for (int j = 0; j < drArr1.Count(); j++)
                                {
                                    Dictionary<string, object> dicFileList = new Dictionary<string, object>();
                                    dicFileList.Add("UploadFile", drArr1[j]["FileUrl"]);
                                    string fileName = drArr1[j]["FileUrl"].ToString();
                                    if (fileName.Contains("/"))
                                    {
                                        string fileName1 = fileName.Substring(fileName.LastIndexOf('/') + 1);
                                        if (fileName1.Contains("$"))
                                        {
                                            dicFileList.Add("UploadFileName", fileName1.Substring(0, fileName1.LastIndexOf('$')) + "." + fileName1.Substring(fileName1.LastIndexOf('.') + 1));
                                            listFileList.Add(dicFileList);
                                        }
                                    }
                                }
                                dicPublish["UploadFileList"] = listFileList;
                            }
                        }
                        listPublish.Add(dicPublish);
                    }
                    dicAll["PublishInfo"] = listPublish;
                }
            }
            return dicAll;
        }
        #endregion
        #region 1.1.4
        /// <summary>
        /// 2017-06-08 zlb
        /// 查询APP广告详情
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <param name="MediaID">订单ID</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectShoppingAppPublish(int TemplateID, int MediaID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();

            int userID = Common.UserInfo.GetLoginUserID();
            DataSet ds = Dal.Periodication.Instance.SelectShoppingAppPublish(TemplateID, MediaID, userID);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dwMediaInfo = ds.Tables[0].Rows[0];
                dicAll.Add("AdTemplateName", dwMediaInfo["AdTemplateName"].ToString());
                dicAll.Add("UserName", dwMediaInfo["UserName"].ToString().ToString());
                //dicAll.Add("CarouselCount", dwMediaInfo["CarouselCount"]);
                //dicAll.Add("SellingPlatform", dwMediaInfo["SellingPlatform"]);
                //dicAll.Add("SellingMode", dwMediaInfo["SellingMode"].ToString());
                dicAll.Add("HeadIconURL", dwMediaInfo["HeadIconURL"].ToString());
                dicAll.Add("Collection", dwMediaInfo["Collection"]);
                dicAll.Add("Defriend", dwMediaInfo["Defriend"]);
                dicAll.Add("MediaName", dwMediaInfo["MediaName"].ToString());
                dicAll.Add("DailyLive", dwMediaInfo["DailyLive"]);
                dicAll.Add("Location", dwMediaInfo["Location"].ToString());
                dicAll.Add("App_AdTemplate", dwMediaInfo["App_AdTemplate"]);
                dicAll.Add("Remark", dwMediaInfo["Remark"].ToString());
                dicAll.Add("AdDisplay", dwMediaInfo["AdDisplay"].ToString());
                dicAll.Add("AdDescription", dwMediaInfo["AdDescription"].ToString());
                dicAll.Add("ATRemarks", dwMediaInfo["ATRemarks"].ToString());

                #region 分割模板示列图
                dicAll.Add("AdLegendURL1", "");
                dicAll.Add("AdLegendURL2", "");
                dicAll.Add("AdLegendURL3", "");
                dicAll.Add("AdLegendURL1_sl", "");
                dicAll.Add("AdLegendURL2_sl", "");
                dicAll.Add("AdLegendURL3_sl", "");
                string AdLegendURL = dwMediaInfo["AdLegendURL"] == null ? "" : dwMediaInfo["AdLegendURL"].ToString().Contains(".") ? dwMediaInfo["AdLegendURL"].ToString() : "";
                if (AdLegendURL != "")
                {
                    if (AdLegendURL.Contains(","))
                    {
                        string[] URL = AdLegendURL.Split(',');
                        for (int j = 0; j < URL.Length; j++)
                        {
                            dicAll["AdLegendURL" + (j + 1)] = URL[j];
                            int LastIndex = URL[j].LastIndexOf('.');
                            string SlImg = URL[j].Substring(0, LastIndex) + "_sl." + URL[j].Substring(LastIndex + 1, URL[j].Length - LastIndex - 1);
                            dicAll["AdLegendURL" + (j + 1) + "_sl"] = SlImg;
                        }
                    }
                    else
                    {
                        dicAll["AdLegendURL1"] = AdLegendURL;
                        int LastIndex = AdLegendURL.LastIndexOf('.');
                        string SlImg = AdLegendURL.Substring(0, LastIndex) + "_sl." + AdLegendURL.Substring(LastIndex + 1, AdLegendURL.Length - LastIndex - 1);
                        dicAll["AdLegendURL1_sl"] = SlImg;
                    }

                }
                #endregion
                #region 下单备注
                dicAll.Add("RemarkName", "");
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    string OtherContent = "";
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        DataRow dwRemark = ds.Tables[1].Rows[i];
                        if (dwRemark["OtherContent"].ToString() != "")
                        {
                            OtherContent = dwRemark["OtherContent"].ToString();
                        }
                        else
                        {
                            dicAll["RemarkName"] += dwRemark["RemarkName"].ToString() + ",";
                        }

                    }
                    if (OtherContent != "")
                    {
                        dicAll["RemarkName"] += OtherContent;
                    }
                    else
                    {
                        dicAll["RemarkName"] = dicAll["RemarkName"].ToString().Substring(0, dicAll["RemarkName"].ToString().Length - 1);
                    }
                }
                #endregion
                #region 广告样式
                dicAll.Add("AdStyleList", new List<Dictionary<string, object>>());
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[2].Rows[i];
                        dicStyle.Add("AdStyle", dwStyle["AdStyle"].ToString());
                        listStyle.Add(dicStyle);
                    }
                    dicAll["AdStyleList"] = listStyle;
                }
                #endregion
                #region 售卖区域组
                dicAll.Add("CityGroupList", new List<Dictionary<string, object>>());
                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[3].Rows[i];
                        dicStyle.Add("GroupType", dwStyle["GroupType"].ToString());
                        listStyle.Add(dicStyle);
                    }
                    dicAll["CityGroupList"] = listStyle;
                }
                #endregion
                #region 售卖区域
                dicAll.Add("CityAreaList", new List<Dictionary<string, object>>());
                if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[4].Rows[i];
                        dicStyle.Add("SaleArea", dwStyle["SaleArea"]);
                        dicStyle.Add("FirstLetter", dwStyle["FirstLetter"].ToString());
                        dicStyle.Add("GroupType", dwStyle["GroupType"]);
                        dicStyle.Add("ProvinceID", dwStyle["ProvinceID"].ToString());
                        dicStyle.Add("CityID", dwStyle["CityID"].ToString());
                        dicStyle.Add("City", dwStyle["City"].ToString());
                        dicStyle.Add("Province", dwStyle["Province"].ToString());
                        listStyle.Add(dicStyle);
                    }
                    dicAll["CityAreaList"] = listStyle;
                }
                #endregion
                #region 行业分类
                dicAll.Add("CategoryName", "");
                if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        dicAll["CategoryName"] += ds.Tables[5].Rows[i]["DictName"].ToString() + ",";
                    }
                    dicAll["CategoryName"] = dicAll["CategoryName"].ToString().Substring(0, dicAll["CategoryName"].ToString().Length - 1);
                }
                #endregion
                #region 覆盖区域
                dicAll.Add("CoverArea", "");
                if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        dicAll["CoverArea"] += ds.Tables[6].Rows[i]["CoverArea"].ToString() + ",";
                    }
                    dicAll["CoverArea"] = dicAll["CoverArea"].ToString().Substring(0, dicAll["CoverArea"].ToString().Length - 1);
                }
                #endregion
                #region 广告单元价格信息
                dicAll.Add("PublishDetail", new List<Dictionary<string, object>>());
                if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[7].Rows[i];
                        dicStyle.Add("ADDetailID", dwStyle["ADDetailID"]);
                        dicStyle.Add("GroupType", dwStyle["GroupType"].ToString());
                        dicStyle.Add("PubID", dwStyle["PubID"]);
                        dicStyle.Add("ADStyle", dwStyle["ADStyle"].ToString());
                        dicStyle.Add("HySalePrice", dwStyle["HySalePrice"] == DBNull.Value ? "" : dwStyle["HySalePrice"]);
                        dicStyle.Add("SalePrice", dwStyle["SalePrice"]);
                        dicStyle.Add("CarouselNumber", dwStyle["CarouselNumber"]);
                        dicStyle.Add("SalePlatform", dwStyle["SalePlatform"]);
                        dicStyle.Add("SaleType", dwStyle["SaleType"]);
                        dicStyle.Add("SaleArea", dwStyle["SaleArea"]);
                        dicStyle.Add("ClickCount", dwStyle["ClickCount"]);
                        dicStyle.Add("ExposureCount", dwStyle["ExposureCount"]);
                        dicStyle.Add("BeginTime", dwStyle["BeginTime"]);
                        dicStyle.Add("EndTime", dwStyle["EndTime"]);
                        dicStyle.Add("DifferDay", dwStyle["DifferDay"]);
                        listStyle.Add(dicStyle);
                    }
                    dicAll["PublishDetail"] = listStyle;
                }
                #endregion

                #region 模板轮播数
                dicAll.Add("CarouselCountList", new List<Dictionary<string, object>>());
                if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[8].Rows[i];
                        dicStyle.Add("CarouselCount", dwStyle["CarouselCount"].ToString());
                        listStyle.Add(dicStyle);
                    }
                    dicAll["CarouselCountList"] = listStyle;
                }
                #endregion

                #region 模板销售平台
                dicAll.Add("SellingPlatformList", new List<Dictionary<string, object>>());
                if (ds.Tables[9] != null && ds.Tables[9].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[9].Rows[i];
                        dicStyle.Add("SellingPlatform", dwStyle["SellingPlatform"].ToString());
                        listStyle.Add(dicStyle);
                    }
                    dicAll["SellingPlatformList"] = listStyle;
                }
                #endregion


                #region 模板销售方式
                dicAll.Add("SellingModeList", new List<Dictionary<string, object>>());
                if (ds.Tables[10] != null && ds.Tables[10].Rows.Count > 0)
                {
                    List<Dictionary<string, object>> listStyle = new List<Dictionary<string, object>>();
                    for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                    {
                        Dictionary<string, object> dicStyle = new Dictionary<string, object>();
                        DataRow dwStyle = ds.Tables[10].Rows[i];
                        dicStyle.Add("SellingMode", dwStyle["SellingMode"].ToString());
                        listStyle.Add(dicStyle);
                    }
                    dicAll["SellingModeList"] = listStyle;
                }
                #endregion
            }
            return dicAll;
        }
        #endregion


    }
    public class ListTotal
    {
        public int TotalCount { get; set; }
        public List<Dictionary<string, object>> listDetail = new List<Dictionary<string, object>>();
    }




}