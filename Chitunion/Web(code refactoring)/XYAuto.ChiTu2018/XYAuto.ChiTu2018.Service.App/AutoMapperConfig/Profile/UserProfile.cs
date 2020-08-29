using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.User;

namespace XYAuto.ChiTu2018.Service.App.AutoMapperConfig.Profile
{
    /// <summary>
    /// 注释：UserProfile
    /// 作者：lix
    /// 日期：2018/6/8 15:16:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<PsReqPostWxUserOperationDto, Entities.Extend.User.WeiXinUserOperateDo>();
        }
    }
}
