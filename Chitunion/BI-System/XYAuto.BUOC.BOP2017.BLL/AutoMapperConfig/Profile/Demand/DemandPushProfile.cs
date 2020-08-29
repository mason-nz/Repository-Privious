/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 13:17:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Response;
using XYAuto.BUOC.BOP2017.Entities.Dto.Demand;

namespace XYAuto.BUOC.BOP2017.BLL.AutoMapperConfig.Profile.Demand
{
    public class DemandPushProfile : AutoMapper.Profile
    {
        public DemandPushProfile()
        {
            //需求json解析-品牌车型
            CreateMap<CarserialInfoDto, CarserialInfo>();
            CreateMap<CarInfoDto, DemandCarSerielDto>();
            //需求json解析-城市
            CreateMap<CityDto, CityItem>();
            CreateMap<AreaInfoDto, DemandCitysDto>();

            //用户导入配置(AccessToken 这些参数不做校验，只是为了满足我逻辑)
            CreateMap<RespExcelUserOrganizeDto, RequestPushUserDto>()
                .ForMember(desc => desc.AccessToken, opt => opt.MapFrom(s => "TEST_AccessToken"))
                .ForMember(desc => desc.Appid, opt => opt.MapFrom(s => 1))
                .ForMember(desc => desc.P, opt => opt.MapFrom(s => "TEST_AccessToken_P"))
                .ForMember(desc => desc.Sign, opt => opt.MapFrom(s => "TEST_Sign"));
        }
    }
}