using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Profile;

namespace XYAuto.ITSC.Chitunion2017.BLL.ImportData
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            //初始化Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ImportMediaProfile>();
                cfg.AddProfile<InteractionProfile>();
                cfg.AddProfile<PublishInfoProfile>();
            });
        }
    }
}
