using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class MediaPCAPPDTO
    {
        public MediaPCAPPDTO()
        {

            MediaType = MediaTypeEnum.APP;
        }

        public MediaTypeEnum MediaType { get; set; }

        public MediaPcApp MediaInfo { get; set; }

        public UserInfoDTO UserInfo { get; set; }

        public List<ProvinceCityDTO> OverlayIDs { get; set; }

    }
}
