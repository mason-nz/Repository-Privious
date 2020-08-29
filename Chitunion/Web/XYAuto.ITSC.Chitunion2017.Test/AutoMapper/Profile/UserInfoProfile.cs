using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Profile;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Dto;

namespace XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Profile
{
    public class UserInfoProfile : global::AutoMapper.Profile
    {
        protected override void Configure()
        {
            //int->string
            CreateMap<int, string>().ConvertUsing(Convert.ToString);
            //string -> bool
            CreateMap<string, bool>().ConvertUsing(Converters.ConvertToBool);

            //CreateMap<string, bool>().ConvertUsing(s => s == "Y");

            Mapper.CreateMap<UserInfoDto, ResponseUserDto>().ForMember(s => s.LastUpdateUserId,
                opt => opt.MapFrom(s => s.UserId + 1));

        }
    }
}
