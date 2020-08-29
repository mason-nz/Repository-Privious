using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatSign;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.WechatSign;

namespace XYAuto.ITSC.Chitunion2017.BLL.WechatSign
{
    public class WechatSign
    {
        public static readonly WechatSign Instance = new WechatSign();
        /// <summary>
        /// 微信签到
        /// </summary>
        /// <returns></returns>
        public Tuple<decimal, string, bool, int, int> DaySign()
        {
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            DateTime dtNow = DateTime.Now;
            DataTable dt = Dal.WechatSign.WechatSign.Instance.GetSignNumber(userId, dtNow);
            WechatSignInfo info = new WechatSignInfo { SignUserID = userId };
            int orderCount = Dal.LETask.LeAdOrderInfo.Instance.GetUserOrderCount(userId);
            var signOrderCount = ConfigurationManager.AppSettings["SignOrderCount"];
            if (string.IsNullOrWhiteSpace(signOrderCount))
            {
                signOrderCount = "5";
            }
            if (dt == null || dt.Rows.Count <= 0)
            {
                info.SignNumber = 1;
            }
            else
            {
                DateTime dtSign = Convert.ToDateTime(dt.Rows[0]["SignDate"]);
                TimeSpan dSpan = Convert.ToDateTime(dtNow.ToShortDateString()).Subtract(dtSign);
                if (dSpan.Days > 0)
                {
                    if (dSpan.Days == 1 && Convert.ToInt32(dt.Rows[0]["SignNumber"]) < 7)
                    {
                        info.SignNumber = Convert.ToInt32(dt.Rows[0]["SignNumber"]) + 1;
                    }
                    else
                    {
                        info.SignNumber = 1;
                    }
                }
                else
                {
                    return new Tuple<decimal, string, bool,int ,int>(0, "您今天已签到", false, orderCount, Convert.ToInt32(signOrderCount));
                }
            }
            if (orderCount < Convert.ToInt32(signOrderCount))
            {
                return new Tuple<decimal, string, bool, int, int>(0, $"分享{signOrderCount}篇文章后才可签到哦！", false, orderCount, Convert.ToInt32(signOrderCount));
            }
            info.Ip = ITSC.Chitunion2017.BLL.Util.GetIP($"用户{userId}签到"); ;
            if (!(!string.IsNullOrEmpty(info.Ip) && BLL.IpBlacklist.IpBlacklist.Instance.IsBlackIp(info.Ip, userId)))
            {
                var price = ConfigurationManager.AppSettings["DaySignPrice"];
                if (string.IsNullOrWhiteSpace(price))
                {
                    price = "0.1,0.2,0.25,0.3,0.35,0.4,0.5";
                }
                string[] priceList = price.Split(',');
                info.SignPrice = Convert.ToDecimal(priceList[info.SignNumber - 1]);
            }

            BLL.Loger.Log4Net.Info("签到信息：" + Json.JsonSerializerBySingleData(info));
            if (Dal.WechatSign.WechatSign.Instance.InsetDaySign(info) <= 0)
            {
                return new Tuple<decimal, string, bool, int, int>(0, "签到失败，请重试", false, orderCount, Convert.ToInt32(signOrderCount) );
            };
            var isLuckDraw = false;
            var dt1 = Dal.LuckDrawActivity.LuckDraw.Instance.GetActivityInfo();
            if (dt1 == null || dt1.Rows.Count <= 0) return new Tuple<decimal, string, bool, int, int>(info.SignPrice, "", isLuckDraw, orderCount, Convert.ToInt32(signOrderCount));
            var dr = dt1.Rows[0];
            var luckDrawStartDate = Convert.ToDateTime(dr["StartTime"]);
            var luckDrawEndDate = Convert.ToDateTime(dr["EndTime"]);
            if (dtNow >= luckDrawStartDate && dtNow <= luckDrawEndDate)
            {
                if (true)
                {
                    isLuckDraw = true;
                }
            }
            return new Tuple<decimal, string, bool, int, int>(info.SignPrice, "", isLuckDraw, orderCount, Convert.ToInt32(signOrderCount));
        }
        /// <summary>
        /// 根据年月查询签到日期
        /// </summary>
        /// <param name="SignYear">签到年份</param>
        /// <param name="SignMonth">签到月份</param>
        /// <returns></returns>
        public WxSignRespDTO DaySignList(int SignYear, int SignMonth)
        {
            WxSignRespDTO dto = new WxSignRespDTO();
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            string nowDate = SignYear + "-" + SignMonth.ToString().PadLeft(2, '0');
            string lastDate = Convert.ToDateTime(nowDate).AddMonths(-1).ToString("yyyy-MM");
            string mextDate = Convert.ToDateTime(nowDate).AddMonths(1).ToString("yyyy-MM");
            DataTable dt = Dal.WechatSign.WechatSign.Instance.SelectDaySignListByMonth(userId, lastDate, mextDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                dto.SignDayList = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dto.SignDayList.Add(Convert.ToDateTime(dt.Rows[i]["SignDate"]).ToString("yyyy-M-d"));
                }
            }
            dto.IsSign = Dal.WechatSign.WechatSign.Instance.IsDaySign(userId, DateTime.Now);
            dto.TotalPrice = Dal.WechatSign.WechatSign.Instance.GetTotalPriceByUserID(userId, (int)ProfitTypeEnum.签到红包统计);
            return dto;
        }
    }
}
