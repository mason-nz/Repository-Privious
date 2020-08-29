/********************************************************
*创建人：lixiong
*创建时间：2017/10/24 16:37:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response;

namespace XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile
{
    public class PullStatisticsProfile : AutoMapper.Profile
    {
        public PullStatisticsProfile()
        {
            CreateMap<RespMaterielDetailDto, Entities.Distribute.MaterielDistributeQingNiaoAgent>()
                .ForMember(desc => desc.MaterielId,
                    map => map.MapFrom(s => s.TaskId))
                .ForMember(desc => desc.ArticleId,
                    map => map.MapFrom(s => s.RecId))
                .ForMember(desc => desc.Type,
                    map => map.MapFrom(s => s.TypeId))
                .ForMember(desc => desc.DistributeDate,
                    map => map.MapFrom(s => s.SendTime));

            CreateMap<Entities.Distribute.MaterielDistributeDetailed, Entities.QingNiao.ChituMaterialStat>();
        }

        public static int GetHeadArticleId(RespMaterielDetailDto dto)
        {
            if (dto == null)
                return -2;
            if (dto.TypeId == 1)
            {
                return dto.RecId;
            }
            else if (dto.TypeId == 2)
            {
                return dto.RecId;
            }
            return -1;
        }
    }
}