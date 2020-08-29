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
    /// 微博详情
    /// </summary>
    public class MediaWeiboDTO
    {
        public MediaWeiboDTO()
        {

            MediaType = MediaTypeEnum.微博;
        }

        public MediaTypeEnum MediaType { get; set; }

        public MediaWeibo MediaInfo { get; set; }

        public InteractionWeibo InteractionInfo {get; set;}

        public UserInfoDTO UserInfo { get; set; }

        public List<ProvinceCityDTO> OverlayIDs { get; set; }


    }

}
