/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 11:52:11
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData.AppSettings
{
    internal class PullDataSettings
    {
        public static readonly PullDataSettings Instance = new PullDataSettings();

        public PullDataSettings()
        {
            NormalIntverlTime = GetSetting("NormalIntverlTime").ToInt(60);
            GdtAccountId = GetSetting("GDT_Account_ID").ToInt();
            GdtFundType = GetSetting("GdtFundType");
            GdtClientId = GetSetting("GDT_Client_ID").ToInt();
            AtEveryHourMinutesStart = GetSetting("AtEveryHourMinutesStart").ToInt();
            AtEveryDayHourStart = GetSetting("AtEveryDayHourStart").ToInt();
            //23:30分钟
            var atEveryDayHourRange = GetSetting("AtEveryDayHourRange").Split(':');

            AtEveryDayHourStart = atEveryDayHourRange[0].ToInt();
            AtEveryDayHourEnd = atEveryDayHourRange[1].ToInt();
        }

        /// <summary>
        /// 普通拉取时间间隔：60分钟
        /// </summary>
        public int NormalIntverlTime { get; set; }

        /// <summary>
        /// 每个小时的{0}分开始执行
        /// </summary>
        public int AtEveryHourMinutesStart { get; set; }

        public int AtEveryDayHourStart { get; set; }
        public int AtEveryDayHourEnd { get; set; }

        public string GdtFundType { get; set; }

        public int GdtAccountId { get; set; }
        public int GdtClientId { get; set; }

        private string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}