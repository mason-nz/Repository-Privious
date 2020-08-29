using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;

namespace XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto
{
    public class RequestADOrderInfoDto
    {
        public int optType { get; set; } = -2;
        public RequestADOrderDto ADOrderInfo { get; set; }
        public List<RequestMediaOrderInfoDto> MediaOrderInfos { get; set; }
        public List<RequestADDetailDto> ADDetails { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.EnumAddModify), optType))
                sb.Append("操作类型参数错误!");

            if (MediaOrderInfos == null || MediaOrderInfos.Count == 0)
                sb.Append("媒体的需求附件数量为0!\n");
            else
            {
                if (MediaOrderInfos.GroupBy(x => x.MediaType).Where(g => g.Count() > 1).Count() > 0)
                    sb.Append("需求附件-媒体类型有重复!\n");

                foreach (var item in MediaOrderInfos)
                {
                    if (string.IsNullOrEmpty(item.Note))
                    {
                        try
                        {
                            //广告主为必填项
                            if (Chitunion2017.Common.UserInfo.GetLoginUserRoleIDs().Contains("SYS001RL00002"))
                            {
                                sb.Append($"媒体类型[{item.MediaType}]需求说明为必填项!\n");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }                        

                    //if (string.IsNullOrEmpty(item.UploadFileURL))
                    //    sb.Append($"媒体类型[{item.MediaType}]补充附件为必填项!\n");
                }
            }

            if (ADOrderInfo == null)
                sb.Append("没有项目信息!");
            else
            {
                #region ADOrderInfo数据验证                
                if (!Enum.IsDefined(typeof(Entities.EnumOrderStatus), ADOrderInfo.Status))
                    sb.Append("项目状态错误!不能为:" + ADOrderInfo.Status);
                if (string.IsNullOrEmpty(ADOrderInfo.OrderName))
                    sb.Append("项目名称为必填项!\n");
                if (string.IsNullOrEmpty(ADOrderInfo.CustomerID))
                    sb.Append("广告主名称为必填项!\n");

                if (ADDetails == null || ADDetails.Count == 0)
                    sb.Append("没有广告位!\n");
                else
                {
                    if (ADDetails.Count > 50)
                        sb.Append("广告位数量不能超过50!\n");

                    //从购物车提交项目要验证CartID
                    if (optType == (int)Entities.EnumAddModify.ADD)
                    {
                        foreach (var item in ADDetails)
                        {
                            if (item.CartID == -2 || item.CartID == 0)
                            {
                                sb.Append("提交操作购物车CartID是必填项!\n");
                                msg = sb.ToString();
                                return msg.Length.Equals(0);
                            }
                        }

                    }

                    #region 微信广告位验证
                    var queryWeChat = ADDetails.Where(x => x.MediaType == 14001);
                    var itemGroupsWeChat = queryWeChat.GroupBy(x => x.PubDetailID);
                    foreach (var itemGroup in itemGroupsWeChat)
                    {
                        if (itemGroup.Count() > 3)
                            sb.Append("微信同一广告位最多投放3个!\n");

                        List<RequestADScheduleInfoDto> allADScheduleInfos = new List<RequestADScheduleInfoDto>();
                        foreach (var item in itemGroup)
                        {
                            if (item.ADScheduleInfos == null || item.ADScheduleInfos.Count == 0 || item.ADScheduleInfos.Count > 1)
                            {
                                sb.Append("广告位必须且只能有1个排期!\n");
                                msg = sb.ToString();
                                return false;
                            }
                            allADScheduleInfos.AddRange(item.ADScheduleInfos);
                        }
                        if (allADScheduleInfos.GroupBy(l => l.BeginData.Date).Where(g => g.Count() > 1).Count() > 0)
                        {
                            sb.Append("排期精确到天不能重复!\n");
                            msg = sb.ToString();
                            return false;
                        }
                    }
                    #region 原未按排期拆单前验证
                    //bool brepeat1 = queryWeChat.GroupBy(l => l.PubDetailID).Where(g => g.Count() > 1).Count() > 0;
                    //if (brepeat1)
                    //{
                    //    sb.Append("自媒体广告位ID不能重复!\n");
                    //}
                    //else
                    //{
                    //    foreach (var addetail in queryWeChat)
                    //    {
                    //        if (addetail.ADScheduleInfos == null || addetail.ADScheduleInfos.Count == 0)
                    //        {
                    //            sb.Append("广告位至少应该有1个排期!\n");
                    //        }
                    //        else if (addetail.ADScheduleInfos.Count > 3 && addetail.MediaType == 14001)
                    //        {
                    //            sb.Append("广告位排期最多3个!\n");
                    //        }
                    //        else
                    //        {
                    //            if (addetail.ADScheduleInfos.GroupBy(l => l.BeginData.Date).Where(g => g.Count() > 1).Count() > 0)
                    //            {
                    //                sb.Append("排期精确到天不能重复!\n");
                    //            }
                    //        }

                    //    }
                    //}
                    #endregion

                    #endregion
                    #region APP广告位验证
                    var queryAPP = ADDetails.Where(x => x.MediaType == 14002);
                    /**
                     * 广告位定义：广告位ID+区域ID
                     * 1广告位自己的排期不可以有交集
                     * 2不同广告位所属排期不可以有交集                   
                    **/
                    var itemGroups = queryAPP.GroupBy(l => l.PubDetailID);
                    foreach (var itemGroup in itemGroups)
                    {
                        List<RequestADScheduleInfoDto> allADScheduleInfos = new List<RequestADScheduleInfoDto>();
                        var queryGroup2 = itemGroup.GroupBy(x => x.SaleAreaID);
                        foreach (var itemGroup2 in queryGroup2)
                        {
                            foreach (var item in itemGroup2)
                            {
                                if (item.ADScheduleInfos == null || item.ADScheduleInfos.Count == 0 || item.ADScheduleInfos.Count > 1)
                                {
                                    sb.Append("广告位必须且只能有1个排期!\n");
                                    msg = sb.ToString();
                                    return false;
                                }
                                //将同一广告位同一区域的排期汇总
                                allADScheduleInfos.AddRange(item.ADScheduleInfos);
                                #region 原一个广告位多个排期验证逻辑
                                //if (item.ADScheduleInfos.Count > 1)
                                //{
                                //    foreach (var itemADS in item.ADScheduleInfos)
                                //    {

                                //        if (itemADS.BeginData.Date > itemADS.EndData.Date)
                                //        {
                                //            sb.Append("广告位排期结束时间要大于开始时间!\n");
                                //            msg = sb.ToString();
                                //            return false;
                                //        }
                                //        if (itemADS.BeginData.Date <= DateTime.Now.Date)
                                //        {
                                //            sb.Append("排期开始日期必须大于当天!\n");
                                //            msg = sb.ToString();
                                //            return false;
                                //        }
                                //        if (itemADS.EndData.Date <= DateTime.Now.Date)
                                //        {
                                //            sb.Append("排期结束日期必须大于当天!\n");
                                //            msg = sb.ToString();
                                //            return false;
                                //        }
                                //        if (itemADS.BeginData.Date.AddMonths(6) < itemADS.EndData.Date)
                                //        {
                                //            sb.Append("排期时间跨度不能超过6个月!\n");
                                //            msg = sb.ToString();
                                //            return false;
                                //        }


                                //        var queryADS = from t in item.ADScheduleInfos
                                //                       where t.BeginData != itemADS.BeginData && t.EndData != itemADS.EndData
                                //                        && ((itemADS.BeginData.Date >= t.BeginData.Date && itemADS.BeginData.Date <= t.EndData.Date)
                                //                            ||
                                //                            (itemADS.BeginData.Date <= t.BeginData.Date && itemADS.BeginData.Date >= t.EndData.Date)
                                //                            ||
                                //                            (itemADS.BeginData.Date <= t.BeginData.Date && itemADS.EndData.Date >= t.BeginData.Date)
                                //                            ||
                                //                            (itemADS.BeginData.Date <= t.EndData.Date && itemADS.EndData.Date >= t.EndData.Date)
                                //                           )
                                //                       select t;
                                //        if (queryADS.Count() > 0)
                                //        {
                                //            sb.Append("广告位排期有交叉!\n");
                                //            msg = sb.ToString();
                                //            return false;
                                //        }

                                //        queryADS = from t in item.ADScheduleInfos
                                //                   where t.BeginData == itemADS.BeginData && t.EndData == itemADS.EndData
                                //                   select t;
                                //        if (queryADS.Count() > 1)
                                //        {
                                //            sb.Append("广告位排期不能有重复!\n");
                                //            msg = sb.ToString();
                                //            return false;
                                //        }
                                //    }
                                //}
                                #endregion                                
                            }
                            #region 验证汇总后的广告位排期
                            foreach (var itemADS in allADScheduleInfos)
                            {
                                if (itemADS.BeginData.Date > itemADS.EndData.Date)
                                {
                                    sb.Append("广告位排期结束时间要大于开始时间!\n");
                                    msg = sb.ToString();
                                    return false;
                                }
                                if (itemADS.BeginData.Date <= DateTime.Now.Date)
                                {
                                    sb.Append("排期开始日期必须大于当天!\n");
                                    msg = sb.ToString();
                                    return false;
                                }
                                if (itemADS.EndData.Date <= DateTime.Now.Date)
                                {
                                    sb.Append("排期结束日期必须大于当天!\n");
                                    msg = sb.ToString();
                                    return false;
                                }
                                if (itemADS.BeginData.Date.AddMonths(6) < itemADS.EndData.Date)
                                {
                                    sb.Append("排期时间跨度不能超过6个月!\n");
                                    msg = sb.ToString();
                                    return false;
                                }


                                var queryADS = from t in allADScheduleInfos
                                               where t.BeginData != itemADS.BeginData && t.EndData != itemADS.EndData
                                                && ((itemADS.BeginData.Date >= t.BeginData.Date && itemADS.BeginData.Date <= t.EndData.Date)
                                                    ||
                                                    (itemADS.BeginData.Date <= t.BeginData.Date && itemADS.BeginData.Date >= t.EndData.Date)
                                                    ||
                                                    (itemADS.BeginData.Date <= t.BeginData.Date && itemADS.EndData.Date >= t.BeginData.Date)
                                                    ||
                                                    (itemADS.BeginData.Date <= t.EndData.Date && itemADS.EndData.Date >= t.EndData.Date)
                                                   )
                                               select t;
                                if (queryADS.Count() > 0)
                                {
                                    sb.Append("广告位排期有交叉!\n");
                                    msg = sb.ToString();
                                    return false;
                                }

                                queryADS = from t in allADScheduleInfos
                                           where t.BeginData == itemADS.BeginData && t.EndData == itemADS.EndData
                                           select t;
                                if (queryADS.Count() > 1)
                                {
                                    sb.Append("广告位排期不能有重复!\n");
                                    msg = sb.ToString();
                                    return false;
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }


                //修改订单验证
                if (optType == (int)Entities.EnumAddModify.Modify && string.IsNullOrEmpty(ADOrderInfo.OrderID))
                    sb.Append("修改订单操作订单号是必填项!\n");

                #endregion
            }

            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
    public class RequestADOrderDto
    {
        public bool isCRM { get; set; } = false;
        public string OrderID { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public string CustomerID { get; set; } = string.Empty;
        [JsonIgnore]
        public int CustomerIDINT { get; set; } = -2;
        public string MarketingPolices { get; set; } = string.Empty;
        public string UploadFileURL { get; set; } = string.Empty;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public string CRMCustomerID { get; set; } = string.Empty;
        public string CustomerText { get; set; } = string.Empty;
    }
    public class RequestMediaOrderInfoDto
    {
        public int MediaType { get; set; } = -2;
        public string Note { get; set; } = string.Empty;
        public string UploadFileURL { get; set; } = string.Empty;
    }
    public class RequestADDetailDto
    {
        [JsonIgnore]
        public int RecID { get; set; } = -2;
        public int CartID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public int PubDetailID { get; set; } = -2;
        public decimal AdjustPrice { get; set; } = 0;
        public decimal AdjustDiscount { get; set; }
        public int ADLaunchDays { get; set; } = 0;
        public int Holidays { get; set; } = 0;
        public int SaleAreaID { get; set; } = -2;
        public int ChannelID { get; set; } = -2;
        public decimal CostReferencePrice { get; set; } = 0;
        public List<RequestADScheduleInfoDto> ADScheduleInfos { get; set; }
    }
    public class RequestADScheduleInfoDto
    {
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
    }
   
}
