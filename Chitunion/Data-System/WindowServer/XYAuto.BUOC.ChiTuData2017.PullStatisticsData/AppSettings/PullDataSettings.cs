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

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData.AppSettings
{
    internal class PullDataSettings
    {
        public static readonly PullDataSettings Instance = new PullDataSettings();

        public PullDataSettings()
        {
            ExcuteDateOffsetStat = GetSetting("ExcuteDateOffset_Stat").ToInt(-1);
            ExcuteDateOffsetDistribute = GetSetting("ExcuteDateOffset_Distribute").ToInt(-10);
            ExcuteDateOffsetDistributeDetail = GetSetting("ExcuteDateOffset_Distribute_Detail").ToInt(-10);
            //23:30分钟
            var atEveryDayHourRange = GetSetting("AtEveryDayHourRange_Stat").Split(':');

            AtEveryDayHourStartStat = atEveryDayHourRange[0].ToInt();
            AtEveryDayHourEndStat = atEveryDayHourRange[1].ToInt();

            var atEveryDayHourRangeDistributeDetail = GetSetting("AtEveryDayHourRange_DistributeDetail").Split(':');

            AtEveryDayHourStartDistributeDetail = atEveryDayHourRangeDistributeDetail[0].ToInt();
            AtEveryDayHourEndDistributeDetail = atEveryDayHourRangeDistributeDetail[1].ToInt();
            //
            DistributeQueryDateOffset = GetSetting("DistributeQueryDateOffset").ToInt(10);

            //检测分发数据是否产生
            var atEveryDayRangeVerifyDistributeDetail = GetSetting("AtEveryDayRange_DistributeDetail_VerifyIsExist").Split(':');

            AtEveryDayRangeDistributeDetailVerifyIsExistStart = atEveryDayRangeVerifyDistributeDetail[0].ToInt(9);
            AtEveryDayRangeDistributeDetailVerifyIsExistEnd = atEveryDayRangeVerifyDistributeDetail[1].ToInt(0);
        }

        /// <summary>
        /// 普通拉取时间间隔：60分钟
        /// </summary>
        public int NormalIntverlTime { get; set; }

        /// <summary>
        /// 时间（day）的偏移量(青鸟汽车大全)，拉取前一天的数据同步
        /// </summary>
        public int ExcuteDateOffsetStat { get; set; }

        /// <summary>
        /// 分发明细 偏移量（拉取明细-25 到指定的节点时间）
        /// </summary>
        public int ExcuteDateOffsetDistributeDetail { get; set; }

        /// <summary>
        /// 获取物料统计信息偏移量（-25 到指定的节点时间）
        /// </summary>
        public int ExcuteDateOffsetDistribute { get; set; }

        /// <summary>
        /// 每个小时的{0}分开始执行
        /// </summary>
        public int AtEveryHourMinutesStart { get; set; }

        public int AtEveryDayHourStartStat { get; set; }
        public int AtEveryDayHourEndStat { get; set; }

        public int AtEveryDayHourStartDistributeDetail { get; set; }
        public int AtEveryDayHourEndDistributeDetail { get; set; }

        /// <summary>
        /// 查询物料分发明细7天维度的
        /// </summary>
        public int DistributeQueryDateOffset { get; set; }

        public int AtEveryDayRangeDistributeDetailVerifyIsExistStart { get; set; }
        public int AtEveryDayRangeDistributeDetailVerifyIsExistEnd { get; set; }

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