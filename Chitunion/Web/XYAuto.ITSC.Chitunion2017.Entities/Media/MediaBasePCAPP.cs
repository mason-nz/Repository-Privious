using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    //媒体-媒体信息
    public class MediaBasePCAPP : OtherRelevantInfo
    {
        public int RecID { get; set; }

        //app名称
        public string Name { get; set; }

        //头像的URL地址
        public string HeadIconURL { get; set; }

        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        //是否预约
        public int DailyLive { get; set; }

        //媒体介绍
        public string Remark { get; set; }

        //数据状态
        public int Status { get; set; }

        public DateTime LastUpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public int LastUpdateUserID { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public int BaseMediaID { get; set; }
        public int AdTemplateId { get; set; }
    }
}