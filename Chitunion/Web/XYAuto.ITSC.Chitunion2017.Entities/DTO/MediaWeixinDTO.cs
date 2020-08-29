using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Interaction;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// 微信详情
    /// </summary>
    public class MediaWeixinDTO
    {
        public MediaWeixinDTO(){

            MediaType = MediaTypeEnum.微信;
        }

        public MediaTypeEnum MediaType{get;set;}

        public MediaWeixin MediaInfo { get; set; }

        public InteractionWeixin InteractionInfo { get; set;}

        public UserInfoDTO UserInfo { get; set; }

        public List<ProvinceCityDTO> OverlayIDs { get; set; }

    }


}
