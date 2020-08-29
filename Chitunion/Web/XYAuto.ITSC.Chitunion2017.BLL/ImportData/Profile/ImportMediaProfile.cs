using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData.Profile
{
    public class ImportMediaProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //string->int
            CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            //int->string
            CreateMap<int, string>().ConvertUsing(Convert.ToString);
            //string -> bool
            CreateMap<string, bool>().ConvertUsing(Converters.ConvertToBool);

            Mapper.CreateMap<ImportMediaWeiXinDto, Entities.Media.MediaWeixin>();
            Mapper.CreateMap<ImportMediaWeiBoDto, Entities.Media.MediaWeibo>();
            Mapper.CreateMap<ImportMediaVideoDto, Entities.Media.MediaVideo>();
            Mapper.CreateMap<ImportMediaBroadcastDto, Entities.Media.MediaBroadcast>();
            Mapper.CreateMap<ImportMediaAppDto, Entities.Media.MediaPcApp>();

        }

      
    }

    public class InteractionProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //string->int
            CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            //int->string
            CreateMap<int, string>().ConvertUsing(Convert.ToString);
            //string -> bool
            CreateMap<string, bool>().ConvertUsing(Converters.ConvertToBool);

            /* 互动参数 */
            Mapper.CreateMap<ImportMediaWeiXinDto, Entities.Interaction.InteractionWeixin>();
            Mapper.CreateMap<ImportMediaWeiBoDto, Entities.Interaction.InteractionWeibo>();
            Mapper.CreateMap<ImportMediaVideoDto, Entities.Interaction.InteractionWeibo>();
            Mapper.CreateMap<ImportMediaBroadcastDto, Entities.Interaction.InteractionBroadcast>();
        }
    }

    public class PublishInfoProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //string->int
            CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            //int->string
            CreateMap<int, string>().ConvertUsing(Convert.ToString);
            //string -> bool
            CreateMap<string, bool>().ConvertUsing(Converters.ConvertToBool);

            /* 刊例详情 */
            Mapper.CreateMap<Entities.Publish.PublishDetailInfo, Entities.Publish.PublishDetailInfo>();
            /* 刊例 */
            Mapper.CreateMap<ImportMediaWeiXinDto, Entities.Publish.PublishBasicInfo>();
            Mapper.CreateMap<ImportMediaWeiBoDto, Entities.Publish.PublishBasicInfo>();
            Mapper.CreateMap<ImportMediaVideoDto, Entities.Publish.PublishBasicInfo>();
            Mapper.CreateMap<ImportMediaBroadcastDto, Entities.Publish.PublishBasicInfo>();
            Mapper.CreateMap<ImportMediaAppDto, Entities.Publish.PublishBasicInfo>();
        }
    }

    public class Converters
    {
        /// <summary>
        /// 特定的excel 格式转换为bool
        /// </summary>
        /// <param name="bools">Y / N</param>
        /// <returns></returns>
        public static bool ConvertToBool(string bools)
        {
            return bools.Equals("Y", StringComparison.OrdinalIgnoreCase);
        }
    }
}
