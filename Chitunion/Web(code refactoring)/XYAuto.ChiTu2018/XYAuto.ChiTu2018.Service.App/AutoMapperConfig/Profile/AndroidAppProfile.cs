using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;

namespace XYAuto.ChiTu2018.Service.App.AutoMapperConfig.Profile
{
    /// <summary>
    /// 注释：AndroidApp
    /// 作者：lix
    /// 日期：2018/5/21 18:56:09
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AndroidAppProfile : AutoMapper.Profile
    {
        public AndroidAppProfile()
        {
            CreateMap<ReqReportAppDto, Entities.Chitunion2017.App_Device>()
                .ForMember(desc => desc.EMEI, opt => opt.MapFrom(s => s.IMEI))
                .ForMember(desc => desc.EMSI, opt => opt.MapFrom(s => s.IMSI));
        }
    }
}
