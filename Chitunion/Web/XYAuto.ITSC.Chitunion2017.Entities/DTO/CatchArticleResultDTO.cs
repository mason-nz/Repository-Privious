using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class CatchArticleResultDTO
    {
        public bool IsTrue { get; set; }
        public string Message { get; set; }
        public CatchArticleModel Data { get; set; }
    }

    public class CatchArticleModel
    {

        public ReadInfo ReadForWeb { get; set; }
        public List<DayUpdateInfo> DayUpdateForWeb { get; set; }
        public List<HourUpdateInfo> HourUpdateForWeb { get; set; }

    }

    public class ReadInfo
    {
        public string WxId { get; set; }
        public int ReadAvgSingleCount { get; set; }
        public int ReadHighestSingleCount { get; set; }
        public int ReadAvgFirstCount { get; set; }
        public int ReadHighestFirstCount { get; set; }
        public int ReadAvgSencondCount { get; set; }
        public int ReadHighestSencondCount { get; set; }
        public int ReadAvgThirdCount { get; set; }
        public int ReadHighestThirdCount { get; set; }
        public int Original { get; set; }
        public int ReadAvgCount30Day { get; set; }
        public int ReadMaxCount30Day { get; set; }
        public int LikeAvgCount30Day { get; set; }
        public int WeekUpdateCount30Day { get; set; }
        public int ReadCountGreaterThan10W { get; set; }
        public int NonOriginal { get; set; }
        public DateTime CreateTime { get; set; }

    }

    public class DayUpdateInfo
    {
        public string WxId { get; set; }
        public DateTime Day { get; set; }
        public int ArticleCount { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class HourUpdateInfo
    {
        public string WxId { get; set; }
        public DateTime Hour { get; set; }
        public int ArticleCount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
