using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Publish
{
    /// <summary>
    /// ls
    /// </summary>
    public class PublishExtendInfo
    {
        public int PubID { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        public int MediaID { get; set; }
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
        public string ThrMonitor { get; set; }
        public string SysPlatform { get; set; }
        public string Style { get; set; }
        public bool IsDispatching { get; set; }
        public string ADShow { get; set; }
        public string ADRemark { get; set; }
        public string AcceptBusinessIDs { get; set; }
        public string NotAcceptBusinessIDs{ get; set; }
        public DateTime CreateTime{ get; set; }
        public int CreateUserID{ get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int LastUpdateUserID { get; set; }


        public string AcceptBusinessNames { get; set; }
        public string NotAcceptBusinessNames { get; set; }

    }
}
