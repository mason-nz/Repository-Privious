using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// 用户DTO ls
    /// </summary>
    public class UserInfoDTO
    {
        public UserTypeEnum Type { get; set; }
        public string TypeName { get; set; }
        public string TrueName { get; set; }
        public string Mobile { get; set; }
    }
}
