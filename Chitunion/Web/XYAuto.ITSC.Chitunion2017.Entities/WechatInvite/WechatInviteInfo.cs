using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WechatInvite
{
    public class WechatInviteInfo
    {
        public int InviteUserID { get; set; }
        public int BeInvitedUserID { get; set; }
        public int RedEvesStatus { get; set; }
        public string IP { get; set; }
    }
}
