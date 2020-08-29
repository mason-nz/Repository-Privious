using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;

namespace XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Profile
{
    internal class MediaWeiXinProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            //媒体实体
            Mapper.CreateMap<RequestMediaPublicParam, Entities.Media.MediaWeixin>();
            Mapper.CreateMap<RequestMediaWeiXinDto, Entities.Media.MediaWeixin>();
            //互动参数
            Mapper.CreateMap<RequestMediaPublicParam, Entities.Interaction.InteractionWeixin>();
            Mapper.CreateMap<RequestMediaWeiXinDto, Entities.Interaction.InteractionWeixin>();
        }
    }
}
