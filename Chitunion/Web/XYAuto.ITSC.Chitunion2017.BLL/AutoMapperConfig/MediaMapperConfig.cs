using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig
{
    public class MediaMapperConfig
    {
        public static void Configure()
        {
            //初始化Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MediaInfoProfile>();
                cfg.AddProfile<MediaAppInfoProfile>();
                cfg.AddProfile<MaterielProfile>();
                cfg.AddProfile<AdTemplateProfile>();
                cfg.AddProfile<GdtUserInfoProfile>();
                cfg.AddProfile<GdtServerProfile>();
                cfg.AddProfile<OrderInfoProfile>();
            });
        }
    }
}