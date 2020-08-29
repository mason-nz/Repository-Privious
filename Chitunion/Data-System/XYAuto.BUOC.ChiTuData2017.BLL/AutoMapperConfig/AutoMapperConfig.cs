/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 17:53:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;

namespace XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            //初始化Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<DistributeProfile>();
                cfg.AddProfile<PullStatisticsProfile>();
                cfg.AddProfile<StatSpiderProfile>();
            });
        }
    }
}