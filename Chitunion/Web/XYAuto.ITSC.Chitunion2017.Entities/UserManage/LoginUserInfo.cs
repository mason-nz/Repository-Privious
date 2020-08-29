using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.UserManage
{
    public class LoginUserInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public int TypeID { get; set; }
        public int Category { get; set; }
        public int Source { get; set; }
        public int RegisterType { get; set; }
        public string HeadImgUrl { get; set; }
    }
}
