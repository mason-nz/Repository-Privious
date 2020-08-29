/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 11:52:11
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Configuration;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.AppSettings
{
    internal class PullDataSettings
    {
        public static readonly PullDataSettings Instance = new PullDataSettings();

        public PullDataSettings()
        {
            //23:30分钟
            var atEveryDayHourRange = GetSetting("AtEveryDayHourRange").Split(':');

            AtEveryDayHourStart = atEveryDayHourRange[0].ToInt();
            AtEveryDayHourEnd = atEveryDayHourRange[1].ToInt();
            //拉取智慧云接口数据偏移量（8天）
            PullDataQueryDateOffset = GetSetting("PullDataQueryDateOffset").ToInt(8);
            //如果还用原来的逻辑，应该是隔一天去拉取数据统计，现在马上要换成分发日期去获取
            ExcuteDateOffset = GetSetting("ExcuteDateOffset").ToInt(-35);

            ExcuteDateOffsetQingNiao = GetSetting("ExcuteDateOffset_QingNiao").ToInt(-1);
        }

        /// <summary>
        /// 普通拉取时间间隔：60分钟
        /// </summary>
        public int NormalIntverlTime { get; set; }

        public int AtEveryDayHourStart { get; set; }
        public int AtEveryDayHourEnd { get; set; }

        /// <summary>
        /// 拉取智慧云接口数据偏移量（8天）
        /// </summary>
        public int PullDataQueryDateOffset { get; set; }

        /// <summary>
        /// 如果还用原来的逻辑，应该是隔一天去拉取数据统计，现在马上要换成分发日期去获取
        /// </summary>
        public int ExcuteDateOffset { get; set; }

        public int ExcuteDateOffsetQingNiao { get; set; }

        private string GetSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception exception)
            {
                throw new PullDataSettingsException($"XYAuto.BUOC.ChiTuData2017.PullStatisticsData AppSettings 请配置相关参数，{exception.Message},{exception.StackTrace ?? string.Empty}");
            }
        }
    }
}