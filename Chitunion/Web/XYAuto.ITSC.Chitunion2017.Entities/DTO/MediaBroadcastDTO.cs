using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Interaction;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// 直播详情
    /// </summary>
    public class MediaBroadcastDTO
    {
        public MediaBroadcastDTO()
        {

            MediaType = MediaTypeEnum.直播;
        }

        public MediaTypeEnum MediaType { get; set; }

        public MediaBroadcast MediaInfo { get; set; }

        public InteractionBroadcast InteractionInfo { get; set; }

        public UserInfoDTO UserInfo { get; set; }

        public List<ProvinceCityDTO> OverlayIDs { get; set; }


    }
}
