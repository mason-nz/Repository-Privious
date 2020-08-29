using AutoMapper;
using XYAuto.BUOC.BOP2017.BLL.AutoMapperConfig.Profile.Demand;
using XYAuto.BUOC.BOP2017.BLL.AutoMapperConfig.Profile.GDT;

namespace XYAuto.BUOC.BOP2017.BLL.AutoMapperConfig
{
    public class MediaMapperConfig
    {
        public static void Configure()
        {
            //初始化Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<GdtUserInfoProfile>();
                cfg.AddProfile<GdtServerProfile>();
                cfg.AddProfile<DemandPushProfile>();
                cfg.AddProfile<DemandGroundProfile>();
            });
        }
    }
}