using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.LuckDrawActivity;

namespace XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity
{
    /// <summary>
    /// 注释：LuckDraw
    /// 作者：zhanglb
    /// 日期：2018/5/23 17:25:28
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LuckDraw
    {
        #region Instance
        public static readonly LuckDraw Instance = new LuckDraw();
        #endregion

        /// <summary>
        /// 获取剩余抽奖次数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>-1:分享文章；0:签到；大于0：抽奖</returns>
        public Dictionary<string, int> GetDrawRemainder(int userId)
        {
            //分享订单数
            var orderCount = Dal.LETask.LeAdOrderInfo.Instance.GetUserOrderCount(userId);
            //满足签到订单数
            var signOrderCount = Utils.Config.ConfigurationUtil.GetAppSettingValue("SignOrderCount", false) ?? "5";
            if (orderCount < Convert.ToInt32(signOrderCount))
            {
                return new Dictionary<string, int> { { "DrawRemainder", -1 } };
            }
            if (!Dal.WechatSign.WechatSign.Instance.IsDaySign(userId, DateTime.Now))
            {
                return new Dictionary<string, int> { { "DrawRemainder", 0 } };
            }
            //最大抽奖次数
            var maxDrawCount = Utils.Config.ConfigurationUtil.GetAppSettingValue("DrawCount", false) ?? "3";
            //有效抽奖次数
            var count = orderCount - Convert.ToInt32(signOrderCount);
            //已抽奖次数
            var remainder = Dal.LuckDrawActivity.LuckDraw.Instance.GetDrawRemainder(userId, DateTime.Now);
            //剩余抽奖次数
            var drawRemainder = Convert.ToInt32(maxDrawCount) - remainder;
            if (drawRemainder > 0 && count > remainder)
            {

                return new Dictionary<string, int> { { "DrawRemainder", drawRemainder } };
            }
            return new Dictionary<string, int> { { "DrawRemainder", -1 } };
        }
        /// <summary>
        /// 获取用户今日剩余抽奖次数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        private static int GetTodayDrawCount(int userId)
        {
            var remainder = Dal.LuckDrawActivity.LuckDraw.Instance.GetDrawRemainder(userId, DateTime.Now);
            var maxDrawCount = Utils.Config.ConfigurationUtil.GetAppSettingValue("DrawCount", false) ?? "3"; ;
            return Convert.ToInt32(maxDrawCount) - remainder;
        }

        /// <summary>
        /// 获取抽奖活动基本配置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetActivityBasicInfo(int userId)
        {
            var dicAll = new Dictionary<string, object>();
            var dt1 = Dal.LuckDrawActivity.LuckDraw.Instance.GetActivityInfo();
            if (dt1 == null || dt1.Rows.Count <= 0) return dicAll;
            var dr = dt1.Rows[0];

            var maxDrawCount = Utils.Config.ConfigurationUtil.GetAppSettingValue("DrawCount", false) ?? "3";
            var orderCount = Dal.LETask.LeAdOrderInfo.Instance.GetUserOrderCount(userId);
            var signOrderCount = Utils.Config.ConfigurationUtil.GetAppSettingValue("SignOrderCount", false) ?? "5";
            //可抽奖次数
            var drawCount = orderCount - Convert.ToInt32(signOrderCount);
            //已抽奖次数
            var remainder = Dal.LuckDrawActivity.LuckDraw.Instance.GetDrawRemainder(userId, DateTime.Now);
            //剩余抽奖次数
            var drawRemainder = Convert.ToInt32(maxDrawCount) - remainder;
            var isLuckDraw = drawRemainder > 0 && drawCount > remainder;
            var dtStart = Convert.ToDateTime(dr["StartTime"]);
            var dtEnd = Convert.ToDateTime(dr["EndTime"]);
            dicAll.Add("StartTime", dtStart.Month + "月" + dtStart.Day + "日" + $"{dtStart.Hour:D2}" + ":" + $"{dtStart.Minute:D2}");
            dicAll.Add("EndTime", dtEnd.Month + "月" + dtEnd.Day + "日" + $"{dtEnd.Hour:D2}" + ":" + $"{dtEnd.Minute:D2}");
            dicAll.Add("BonusBase", Convert.ToDecimal(dr["bonusBase"]) + Convert.ToDecimal(dr["price"]) * Convert.ToDecimal(dr["drawNum"]));
            dicAll.Add("DrawRemainder", GetTodayDrawCount(userId));
            dicAll.Add("MaxDrawCount", Convert.ToInt32(maxDrawCount));
            dicAll.Add("isLuckDraw", isLuckDraw);
            dicAll.Add("IsSign", Dal.WechatSign.WechatSign.Instance.IsDaySign(userId, DateTime.Now));
            var list = ITSC.Chitunion2017.BLL.LETask.V2_3.LE_Task.Instance.GetDataByPageV2_5(new ITSC.Chitunion2017.Entities.DTO.V2_3.TaskResDTO()
            {
                UserID = userId,
                SceneID = -2,
                PageIndex = -2,
                PageSize = 15
            });
            dicAll.Add("List", list);
            List<Dictionary<string, object>> dicList = new SupportClass.EquatableList<Dictionary<string, object>>();
            var dt2 = Dal.LuckDrawActivity.LuckDraw.Instance.GetPrizeList(Convert.ToInt32(dr["ActivityId"]));
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                for (var i = 0; i < dt2.Rows.Count; i++)
                {
                    var dic = new Dictionary<string, object>();
                    var dr2 = dt2.Rows[i];
                    dic.Add("PrizeId", dr2["PrizeId"]);
                    dic.Add("AwardName", dr2["AwardName"].ToString().Insert(4, "<br/>"));
                    dicList.Add(dic);
                }
            }
            dicAll.Add("PrizeList", dicList);
            return dicAll;
        }
        /// <summary>
        /// 获取用户获奖记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <returns></returns>
        public Dictionary<string, object> GetAwardRecord(int userId, int pageIndex, int pageSize)
        {
            var dicAll = new Dictionary<string, object>();

            var totalCount = 0;
            var dt = Dal.LuckDrawActivity.LuckDraw.Instance.GetAwardRecord(userId, pageIndex, pageSize, out totalCount);
            var dicList = new List<Dictionary<string, object>>();
            dicAll.Add("AwardRecord", dicList);
            dicAll.Add("TotalCount", totalCount);
            if (dt == null || dt.Rows.Count <= 0) return dicAll;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var dr = dt.Rows[i];
                var dic = new Dictionary<string, object>
                {
                    {"AwardName", dr["AwardName"]},
                    {"DrawPrice", dr["DrawPrice"]},
                    {"DrawTime", dr["DrawTime"]}
                };
                dicList.Add(dic);
            }
            return dicAll;
        }

        /// <summary>
        /// 获取获奖名单(假数据)
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetAwardeeMoniList()
        {
            //var awardCount = ConfigurationManager.AppSettings["AwardCount"];
            //if (string.IsNullOrWhiteSpace(awardCount))
            //{
            //    awardCount = "50";
            //}
            List<Dictionary<string, object>> dicList = new SupportClass.EquatableList<Dictionary<string, object>>();
            var dt = Dal.LuckDrawActivity.LuckDraw.Instance.GetAwardeeMoniList(50);
            if (dt == null || dt.Rows.Count <= 0) return dicList;
            for (var i = 0; i < dt.Rows.Count; i++)
            {

                var dr = dt.Rows[i];
                var dic = new Dictionary<string, object>
                {
                    {"NickName", dr["NickName"]},
                    {"Mobile", dr["Mobile"]},
                    {"DrawTime", ""},
                    {"DrawDescribe", dr["DrawDescribe"]}
                };
                dicList.Add(dic);
            }
            return dicList;
        }

        /// <summary>
        /// 获取获奖名单
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetAwardeeList()
        {
            var awardCount = ConfigurationManager.AppSettings["AwardCount"];
            if (string.IsNullOrWhiteSpace(awardCount))
            {
                awardCount = "50";
            }
            List<Dictionary<string, object>> dicList = new SupportClass.EquatableList<Dictionary<string, object>>();
            var dt = Dal.LuckDrawActivity.LuckDraw.Instance.GetAwardeeList(Convert.ToInt32(awardCount));
            if (dt == null || dt.Rows.Count <= 0) return dicList;
            for (var i = 0; i < dt.Rows.Count; i++)
            {

                var dr = dt.Rows[i];
                var dic = new Dictionary<string, object>
                {
                    {"NickName", dr["nickname"]},
                    {"Mobile", dr["Mobile"]},
                    {"DrawTime", dr["DrawTime"]},
                    {"DrawDescribe", dr["DrawDescribe"]}
                };
                dicList.Add(dic);
            }
            return dicList;
        }

        readonly object _locker = new object();
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public Dictionary<string, object> LotteryDraw(int userId, out int errorMsg)
        {
            lock (_locker)
            {
                errorMsg = 0;
                if (!Dal.WechatSign.WechatSign.Instance.IsDaySign(userId, DateTime.Now))
                {
                    errorMsg = -1;
                    return null;
                }
                var dt1 = Dal.LuckDrawActivity.LuckDraw.Instance.GetActivityInfo();
                if (dt1 == null || dt1.Rows.Count <= 0)
                {
                    Loger.Log4Net.Info("未找到活动数据");
                    errorMsg = -2;
                    return null;
                }
                decimal bonusBase = Convert.ToDecimal(dt1.Rows[0]["BonusBase"]);
                int drawNum = Convert.ToInt32(dt1.Rows[0]["DrawNum"]);
                decimal price = Convert.ToDecimal(dt1.Rows[0]["Price"]);

                var orderCount = Dal.LETask.LeAdOrderInfo.Instance.GetUserOrderCount(userId);
                var signOrderCount = ConfigurationManager.AppSettings["SignOrderCount"];
                if (string.IsNullOrWhiteSpace(signOrderCount))
                {
                    signOrderCount = "5";
                }
                //最大抽奖次数
                var maxDrawCount = ConfigurationManager.AppSettings["DrawCount"];
                if (string.IsNullOrWhiteSpace(maxDrawCount))
                {
                    maxDrawCount = "3";
                }
                //可抽奖次数
                var drawCount = orderCount - Convert.ToInt32(signOrderCount);
                //已抽奖次数
                var remainder = Dal.LuckDrawActivity.LuckDraw.Instance.GetDrawRemainder(userId, DateTime.Now);
                //剩余抽奖次数
                var drawRemainder = Convert.ToInt32(maxDrawCount) - remainder;
                Dictionary<string, object> dic = new Dictionary<string, object>
            {
                {"isLuckDraw", false},
                {"PrizeId", 0},
                {"DrawRemainder", drawRemainder},
                {"DrawPrice", 0},
                {"BonusBase", (bonusBase + price * (drawNum))}
            };
                //判断抽奖是否已达上限
                if (drawRemainder > 0 && drawCount > remainder)
                {
                    var drawRecord = new DrawRecord { ActivityId = Convert.ToInt32(dt1.Rows[0]["ActivityId"]) };
                    var listPrize = Util.DataTableToList<Prize>(Dal.LuckDrawActivity.LuckDraw.Instance.GetPrizeList(drawRecord.ActivityId));
                    if (listPrize != null && listPrize.Count > 0)
                    {
                        var count = Dal.LuckDrawActivity.LuckDraw.Instance.GetDrawRemainder(userId);
                        if (count <= 0)
                        {
                            Prize prize = listPrize.FirstOrDefault(t => t.AwardName == "随机现金红包");
                            if (prize != null)
                            {
                                drawRecord.PrizeId = prize.PrizeId;
                                drawRecord.UserId = userId;
                                drawRecord.DrawPrice = CalculationDrawPrice(prize, bonusBase, drawNum, price);
                                drawRecord.DrawDescribe = prize.AwardName + drawRecord.DrawPrice + "元";
                                drawRecord.Status = 0;
                            }
                            else
                            {
                                Loger.Log4Net.Info("未找到随机现金红包");
                                errorMsg = -2;
                                return null;
                            }
                        }
                        else
                        {
                            Random rd = new Random();
                            int a = rd.Next(1, 1000);
                            foreach (var item in listPrize)
                            {
                                if (a <= item.StartSection || a > item.EndSection) continue;
                                var dtPrize = Dal.LuckDrawActivity.LuckDraw.Instance.GetSumDrawInfo(item.PrizeId);
                                Prize prize = null;
                                if (dtPrize != null && dtPrize.Rows.Count > 0)
                                {

                                    if (item.WinningMaxNum <= Convert.ToInt32(dtPrize.Rows[0]["SumCount"]) || Convert.ToDecimal(dtPrize.Rows[0]["SumDrawPrice"]) >= item.WinningMaxPrice)
                                    {
                                        prize = listPrize.FirstOrDefault(t => t.AwardName == "谢谢参与");
                                        if (prize == null)
                                        {
                                            Loger.Log4Net.Info("未找到谢谢参与");
                                            errorMsg = -2;
                                            return null;
                                        }
                                        Loger.Log4Net.Info($"奖项ID{item.PrizeId}到达上限 ");
                                    }
                                    else
                                    {
                                        prize = item;
                                    }
                                }
                                else
                                {
                                    prize = item;
                                }
                                drawRecord.PrizeId = prize.PrizeId;
                                drawRecord.UserId = userId;
                                drawRecord.DrawPrice = CalculationDrawPrice(prize, bonusBase, drawNum, price);
                                if (prize.AwardName.Contains("随机"))
                                {
                                    drawRecord.DrawDescribe = prize.AwardName + drawRecord.DrawPrice + "元";
                                }
                                else
                                {
                                    drawRecord.DrawDescribe = prize.AwardName;
                                }
                                drawRecord.Status = 0;
                                break;
                            }
                        }
                        var strMsg = Dal.LuckDrawActivity.LuckDraw.Instance.LotteryDraw(drawRecord, Convert.ToInt32(maxDrawCount));
                        if (string.IsNullOrEmpty(strMsg))
                        {
                            int lastRemainder = drawRemainder - 1;
                            if (lastRemainder > 0 && drawCount > (remainder + 1))
                            {
                                dic["isLuckDraw"] = true;
                            }
                            dic["PrizeId"] = drawRecord.PrizeId;
                            dic["DrawRemainder"] = lastRemainder;
                            dic["DrawPrice"] = drawRecord.DrawPrice;
                            dic["BonusBase"] = (bonusBase + price * (drawNum + 1));
                            return dic;
                        }
                        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"插入抽奖收益失败：{strMsg}");
                    }
                }
                else
                {
                    return dic;
                }
                Loger.Log4Net.Info($"抽奖已达到上限 drawRemainder:{drawRemainder} drawCount:{drawCount} remainder:{remainder}");
                errorMsg = -2;
                return null;
            }
        }
        /// <summary>
        /// 计算抽奖金额
        /// </summary>
        /// <param name="prize">奖项类</param>
        /// <param name="bonusBase">奖金基数</param>
        /// <param name="drawNum">抽奖次数</param>
        /// <param name="price">抽奖一次向奖金累加的金额</param>
        /// <returns></returns>
        private decimal CalculationDrawPrice(Prize prize, decimal bonusBase, int drawNum, decimal price)
        {
            Random rd = new Random();
            int drawMinPrice = Convert.ToInt32(prize.DrawMinPrice * 100);
            int drawMaxPrice = Convert.ToInt32(prize.DrawMaxPrice * 100);
            int a = rd.Next(drawMinPrice, drawMaxPrice);
            decimal cash = Math.Round((decimal)(a * 0.01), 2, MidpointRounding.AwayFromZero);
            decimal drawPrice = Math.Round((prize.DrawRatio * (bonusBase + price * drawNum)) + cash, 2, MidpointRounding.AwayFromZero);
            return drawPrice;

        }

    }
}
