using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ADPositionDTO
    {
        public int ADDetailID { get; set; }
        public int PubID { get; set; }
        public string AdLegendURL { get; set; }
        public string AdPosition { get; set; }
        public string AdForm { get; set; }
        public int DisplayLength { get; set; }
        public bool CanClick { get; set; }
        public int CarouselCount { get; set; }
        public string PlayPosition { get; set; }
        public int DailyExposureCount { get; set; }
        public bool CPM { get; set; }
        public bool CarouselPlay { get; set; }
        public int DailyClickCount { get; set; }
        public bool CPM2 { get; set; }
        public bool CarouselPlay2 { get; set; }
        public List<int> ThrMonitor { get; set; }
        public List<int> SysPlatform { get; set; }
        public string Style { get; set; }
        public bool IsDispatching { get; set; }
        public string ADShow { get; set; }
        public string ADRemark { get; set; }
        public List<int> AcceptBusinessIDs { get; set; }
        public List<int> NotAcceptBusinessIDs { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }

        //Detail表信息
        public int SaleType { get; set; }
        public decimal Price { get; set; }
        public bool IsCarousel { get; set; }
        public int BeginPlayDays { get; set; }
        private PublishStatusEnum publishstatus = PublishStatusEnum.新建;
        public PublishStatusEnum PublishStatus {
            get { return publishstatus; }
            set { publishstatus = value; }
        }


        public bool CheckSelfModel(out string msg)
        {

            StringBuilder sb = new StringBuilder();
            if (PubID.Equals(0))
                sb.AppendLine("缺少刊例ID");
            if (string.IsNullOrWhiteSpace(AdLegendURL))
                sb.AppendLine("缺少广告位图例");
            if (string.IsNullOrWhiteSpace(AdPosition))
                sb.AppendLine("缺少广告位置");
            if (string.IsNullOrWhiteSpace(AdForm))
                sb.AppendLine("缺少广告形式");
            if (DisplayLength > 300)
                sb.AppendLine("显示时长过长");
            if (CarouselCount > 20)
                sb.AppendLine("轮播数过多");
            if (PlayPosition != null && PlayPosition.Length > 10)
                sb.AppendLine("位置字数过多");
            if (ThrMonitor != null && ThrMonitor.Count > 0)
            {
                if (ThrMonitor.Any(t => !System.Enum.IsDefined(typeof(ThrMonitorEnum), t)))
                    sb.AppendLine("第三方监测错误");
            }
            if (SysPlatform == null || SysPlatform.Count.Equals(0))
            {//至少选择一个
                sb.AppendLine("缺少系统平台");
            }
            else
            {
                if (SysPlatform.Any(s => !System.Enum.IsDefined(typeof(SysPlatformEnum), s)))
                    sb.AppendLine("系统平台错误");
            }
            if (SaleType.Equals(0))
                sb.AppendLine("缺少售卖方式");
            else if (!System.Enum.IsDefined(typeof(SaleTypeEnum), SaleType))
                sb.AppendLine("售卖方式错误");
            if (Price.Equals(0))
                sb.AppendLine("缺少价格");
            if (AcceptBusinessIDs != null && AcceptBusinessIDs.Count > 0)
            {
                if (AcceptBusinessIDs.Any(i => !System.Enum.IsDefined(typeof(BusinessEnum), i)))
                    sb.AppendLine("接受行业错误");
            }
            if (NotAcceptBusinessIDs != null && NotAcceptBusinessIDs.Count > 0)
            {
                if (NotAcceptBusinessIDs.Any(i => !System.Enum.IsDefined(typeof(BusinessEnum), i)))
                    sb.AppendLine("不接受行业错误");
                else if (AcceptBusinessIDs != null && AcceptBusinessIDs.Count > 0 && NotAcceptBusinessIDs.Intersect(AcceptBusinessIDs).Count() > 0)
                    sb.AppendLine("不接受行业错误");
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
