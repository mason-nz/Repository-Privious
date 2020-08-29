using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetMediaListBResDTO
    {
        public List<MediaItemBDTO> List { get; set; }
        public int Total { get; set; }
    }

    public class MediaItemBDTO
    {
        public int MediaID { get; set; }
        public int WxID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string HeadImg { get; set; }
        public string CategoryNames { get; set; }
        public int FansCount { get; set; }
        private string mediatypename = "微信公众号";

        public string MediaTypeName
        {
            get { return mediatypename; }
            set { mediatypename = value; }
        }

        public int OAuthType { get; set; }
        public string OAuthTypeName { get; set; }
        public string OAuthStatusName { get; set; }
        public string LevelTypeName { get; set; }
        public int PublishStatus { get; set; }
        public string PublishStatusName { get; set; }
        public int AuditStatus { get; set; }
        public string CreateTime { get; set; }
        public string CreateUserName { get; set; }
        public string SubmitUserName { get; set; }
        public string LastUpdateTime { get; set; }
        public string AuditTime { get; set; }
        public string RejectMsg { get; set; }
        public bool CanAddToCocommend { get; set; }
        public bool IsRange { get; set; }
        public bool hasOnPub { get; set; }
        public string AreaMapping { get; set; }
        public bool IsAreaMedia { get; set; }
        public List<CoverageAreaDto> AreaMedia { get; set; }//区域媒体
    }
}