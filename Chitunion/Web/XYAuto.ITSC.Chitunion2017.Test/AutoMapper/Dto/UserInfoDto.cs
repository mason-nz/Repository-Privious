using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Dto
{
    public class UserInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string IsAuth { get; set; }

        public int Sex { get; set; }
    }

    public class ResponseUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsAuth { get; set; }
        public int LastUpdateUserId { get; set; }
        public string Sex { get; set; }
    }
}
