using System;
using System.Collections.Generic;
using System.Linq;
using XYAuto.ChiTu2018.BO.HD;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.BO.Wechat;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;
using XYAuto.ChiTu2018.Infrastructure.Extensions;

namespace XYAuto.ChiTu2018.Service.App.LuckDrawActivity
{
    /// <summary>
    /// 注释：LuckDrawService
    /// 作者：lix
    /// 日期：2018/6/11 14:07:37
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppLuckDrawService
    {

        #region 初始化
        private readonly LEADOrderInfoBO _leadOrderInfoBo;
        private readonly WechatSignBO _wechatSignBo;
        private readonly HdLuckDrawRecordBO _hdLuckDrawRecordBo;
        private readonly HdLuckDrawActivityBO _hdLuckDrawActivityBo;
        private readonly HdLuckDrawPrizeBO _hdLuckDrawPrizeBo;
        private AppLuckDrawService()
        {
            _leadOrderInfoBo = new LEADOrderInfoBO();
            _wechatSignBo = new WechatSignBO();
            _hdLuckDrawRecordBo = new HdLuckDrawRecordBO();
            _hdLuckDrawActivityBo = new HdLuckDrawActivityBO();
            _hdLuckDrawPrizeBo = new HdLuckDrawPrizeBO();
        }
        private static readonly Lazy<AppLuckDrawService> Linstance = new Lazy<AppLuckDrawService>(() => new AppLuckDrawService());
        public static AppLuckDrawService Instance => Linstance.Value;
        #endregion

        #region 配置

        /// <summary>
        /// 满足签到订单数。默认值：5
        /// </summary>
        private int SignOrderCount => XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("SignOrderCount", false).ToInt(5);
        /// <summary>
        /// 最大抽奖次数。默认值：3
        /// </summary>
        private int MaxDrawCount => XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("DrawCount", false).ToInt(3);

        private int AwardCount => XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("AwardCount", false).ToInt(50);

        #endregion

        /// <summary>
        /// 获取剩余抽奖次数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>-1:分享文章；0:签到；大于0：抽奖</returns>
        public Dictionary<string, int> GetDrawRemainder(int userId)
        {
            //分享订单数
            var orderCount = _leadOrderInfoBo.GetUserOrderCount(userId);
            if (orderCount < SignOrderCount)
            {
                return new Dictionary<string, int> { { "DrawRemainder", -1 } };
            }
            if (!_wechatSignBo.IsDaySign(userId, DateTime.Now))
            {
                return new Dictionary<string, int> { { "DrawRemainder", 0 } };
            }
            //有效抽奖次数
            var count = orderCount - MaxDrawCount;
            //已抽奖次数
            var remainder = _hdLuckDrawRecordBo.GetDrawRemainder(userId, DateTime.Now);
            //剩余抽奖次数
            var drawRemainder = MaxDrawCount - remainder;

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
        private int GetTodayDrawCount(int userId)
        {
            var remainder = _hdLuckDrawRecordBo.GetDrawRemainder(userId, DateTime.Now);
            return MaxDrawCount - remainder;
        }

        /// <summary>
        /// 获取抽奖活动基本配置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetActivityBasicInfo(int userId)
        {
            var dicAll = new Dictionary<string, object>();
            var activityList = _hdLuckDrawActivityBo.GetActivityInfo();
            if (!activityList.Any())
                return dicAll;
            var activityInfo = activityList[0];
            var orderCount = _leadOrderInfoBo.GetUserOrderCount(userId);
            //可抽奖次数
            var drawCount = orderCount - SignOrderCount;
            //已抽奖次数
            var remainder = _hdLuckDrawRecordBo.GetDrawRemainder(userId, DateTime.Now);
            //剩余抽奖次数
            var drawRemainder = MaxDrawCount - remainder;
            var isLuckDraw = drawRemainder > 0 && drawCount > remainder;
            var dtStart = activityInfo.StartTime;
            var dtEnd = activityInfo.EndTime;
            dicAll.Add("StartTime", dtStart.Month + "月" + dtStart.Day + "日" + $"{dtStart.Hour:D2}" + ":" + $"{dtStart.Minute:D2}");
            dicAll.Add("EndTime", dtEnd.Month + "月" + dtEnd.Day + "日" + $"{dtEnd.Hour:D2}" + ":" + $"{dtEnd.Minute:D2}");
            dicAll.Add("BonusBase", activityInfo.BonusBase + activityInfo.Price * activityInfo.DrawNum);
            dicAll.Add("DrawRemainder", GetTodayDrawCount(userId));
            dicAll.Add("MaxDrawCount", MaxDrawCount);
            dicAll.Add("isLuckDraw", isLuckDraw);
            dicAll.Add("IsSign", _wechatSignBo.IsDaySign(userId, DateTime.Now));
            //查询最新的任务推荐
            var list = new LeTaskInfoBO().GetDataByPage(1, 15, -2);
            dicAll.Add("List", list);
            var prizeList = _hdLuckDrawPrizeBo.GetPrizeList(activityInfo.ActivityId);
            dicAll.Add("PrizeList", prizeList);
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
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

            var list = new HdLuckDrawRecordMoniBO().GetAwardeeMoniList(50);
            
            list.ForEach(s =>
            {
                var dic = new Dictionary<string, object>
                {
                    {"NickName", s.NickName},
                    {"Mobile", s.Mobile},
                    {"DrawTime", s.DrawTime},
                    {"DrawDescribe", s.DrawDescribe}
                };
                dicList.Add(dic);
            });

            return dicList;
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
            var drawRecordList = _hdLuckDrawRecordBo.GetAwardRecord(userId, pageIndex, pageSize, out totalCount);
            dicAll.Add("AwardRecord", drawRecordList);
            dicAll.Add("TotalCount", totalCount);

            return dicAll;
        }


        /// <summary>
        /// 获取获奖名单
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetAwardeeList()
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            var list = _hdLuckDrawRecordBo.GetAwardeeList(AwardCount);
            list.ForEach(s =>
            {
                var dic = new Dictionary<string, object>
                {
                    {"NickName", s.NickName},
                    {"Mobile", s.Mobile},
                    {"DrawTime", s.DrawTime},
                    {"DrawDescribe", s.DrawDescribe}
                };
                dicList.Add(dic);
            });

            return dicList;
        }

        public object GetSumDrawInfo(int prizeId)
        {
            return _hdLuckDrawRecordBo.GetSumDrawInfo(prizeId);
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
                if (!_wechatSignBo.IsDaySign(userId, DateTime.Now))
                {
                    errorMsg = -1;
                    return null;
                }
                var activityList = _hdLuckDrawActivityBo.GetActivityInfo();
                if (!activityList.Any())
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("未找到活动数据");
                    errorMsg = -2;
                    return null;
                }
                var activityInfo = activityList[0];
                var orderCount = _leadOrderInfoBo.GetUserOrderCount(userId);
                //可抽奖次数
                var drawCount = orderCount - SignOrderCount;
                //已抽奖次数
                var remainder = _hdLuckDrawRecordBo.GetDrawRemainder(userId, DateTime.Now);
                //剩余抽奖次数
                var drawRemainder = MaxDrawCount - remainder;
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    {"isLuckDraw", false},
                    {"PrizeId", 0},
                    {"DrawRemainder", drawRemainder},
                    {"DrawPrice", 0},
                    {"BonusBase", activityInfo.BonusBase + activityInfo.Price * activityInfo.DrawNum}
                };
                //判断抽奖是否已达上限
                if (drawRemainder > 0 && drawCount > remainder)
                {
                    var drawRecord = new HD_LuckDrawRecord() { ActivityId = activityInfo.ActivityId };
                    var prizeList = _hdLuckDrawPrizeBo.GetPrizeList(activityInfo.ActivityId);
                    if (prizeList.Any())
                    {
                        var count = _hdLuckDrawRecordBo.GetDrawRemainder(userId);
                        if (count <= 0)
                        {
                            var prize = prizeList.FirstOrDefault(t => t.AwardName == "随机现金红包");
                            if (prize != null)
                            {
                                drawRecord.PrizeId = prize.PrizeId;
                                drawRecord.UserId = userId;
                                drawRecord.DrawPrice = CalculationDrawPrice(prize, activityInfo.BonusBase, activityInfo.DrawNum, activityInfo.Price);
                                drawRecord.DrawDescribe = prize.AwardName + drawRecord.DrawPrice + "元";
                                drawRecord.Status = 0;
                            }
                            else
                            {
                                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("未找到随机现金红包");
                                errorMsg = -2;
                                return null;
                            }
                        }
                        else
                        {
                            Random rd = new Random();
                            int a = rd.Next(1, 1000);
                            foreach (var item in prizeList)
                            {
                                if (a <= item.StartSection || a > item.EndSection) continue;
                                var dtPrize = _hdLuckDrawRecordBo.GetSumDrawInfo(item.PrizeId);
                                HD_LuckDrawPrize prize = null;
                                if (dtPrize != null)
                                {
                                    if (item.WinningMaxNum <= Convert.ToInt32(dtPrize.SumCount) || Convert.ToDecimal(dtPrize.SumDrawPrice) >= item.WinningMaxPrice)
                                    {
                                        prize = prizeList.FirstOrDefault(t => t.AwardName == "谢谢参与");
                                        if (prize == null)
                                        {
                                            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("未找到谢谢参与");
                                            errorMsg = -2;
                                            return null;
                                        }
                                        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"奖项ID{item.PrizeId}到达上限 ");
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
                                drawRecord.DrawPrice = CalculationDrawPrice(prize, activityInfo.BonusBase, activityInfo.DrawNum, activityInfo.Price);
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
                        var strMsg = _hdLuckDrawRecordBo.LotteryDraw(drawRecord, MaxDrawCount);
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
                            dic["BonusBase"] = (activityInfo.BonusBase + activityInfo.Price * (activityInfo.DrawNum + 1));
                            return dic;
                        }
                        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"插入抽奖收益失败：{strMsg}");
                    }
                }
                else
                {
                    return dic;
                }
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"抽奖已达到上限 drawRemainder:{drawRemainder} drawCount:{drawCount} remainder:{remainder}");
                errorMsg = -2;
                return null;
            }
        }

        /// <summary>
        /// 获取最新的任务
        /// </summary>
        /// <param name="dicAll"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetTaskList(Dictionary<string, object> dicAll)
        {
            if (dicAll.ContainsKey("List"))
                return dicAll;
            //查询最新的任务推荐
            var list = new LeTaskInfoBO().GetDataByPage(1, 15, -2);
            dicAll.Add("List", list);
            return dicAll;
        }

        /// <summary>
        /// 计算抽奖金额
        /// </summary>
        /// <param name="prize">奖项类</param>
        /// <param name="bonusBase">奖金基数</param>
        /// <param name="drawNum">抽奖次数</param>
        /// <param name="price">抽奖一次向奖金累加的金额</param>
        /// <returns></returns>
        private decimal CalculationDrawPrice(HD_LuckDrawPrize prize, decimal bonusBase, int drawNum, decimal price)
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
