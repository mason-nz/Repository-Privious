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
    /// 视频详情
    /// </summary>
    public class MediaVideoDTO
    {
        public MediaVideoDTO()
        {

            MediaType = MediaTypeEnum.视频;
        }

        public MediaTypeEnum MediaType { get; set; }

        public MediaVideo MediaInfo{ get; set;}

        public InteractionVideo InteractionInfo{ get; set;}

       public UserInfoDTO UserInfo{ get; set;}

       public List<ProvinceCityDTO> OverlayIDs { get; set; }

    }
}
