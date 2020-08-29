using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using XYAuto.ChiTu2018.Service.App.AutoMapperConfig.Profile;

namespace XYAuto.ChiTu2018.Service.App.AutoMapperConfig
{
    /// <summary>
    /// 注释：MapperConfig
    /// 作者：lix
    /// 日期：2018/5/21 18:55:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class MapperConfig
    {
        public static void Configure()
        {
            //初始化Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AndroidAppProfile>();
                cfg.AddProfile<UserProfile>();
            });
        }
    }
}
