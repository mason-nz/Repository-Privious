using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WechatInvite
{
    public class BeInvitedInfo
    {
        public int RecID { get; set; }
        public string Nickname { get; set; } = "";
        public string HeadImgurl { get; set; } = "";
        public DateTime InviteTime { get; set; }
        public decimal RedEvesPrice { get; set; }
        public int RedEvesStatus { get; set; }
    }
}
