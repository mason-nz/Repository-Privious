using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class Import_PCAPPDTO
    {
        public Entities.Media.MediaWeixin MediaWeixin { get; set; }
        public Entities.Media.MediaWeibo MediaWeibo { get; set; }
        public Entities.Media.MediaVideo MediaVideo { get; set; }
        public Entities.Media.MediaBroadcast MediaBroadcast { get; set; }
        public Entities.Media.MediaPcApp MediaPcApp { get; set; }
        public PublishBasicInfo PubBasicInfo { get; set; }
        public List<MediaAreaMapping> MappingList { get; set; }
        public List<PublishDetailInfo> PubDetailList { get; set; }
        public PublishExtendInfo PubExtend { get; set; }


        /* 互动参数 */
        public Entities.Interaction.InteractionWeixin InteractionWeixin { get; set; }
        public Entities.Interaction.InteractionBroadcast InteractionBroadcast { get; set; }
        public Entities.Interaction.InteractionVideo InteractionVideo { get; set; }
        public Entities.Interaction.InteractionWeibo InteractionWeibo { get; set; }
    }
}
