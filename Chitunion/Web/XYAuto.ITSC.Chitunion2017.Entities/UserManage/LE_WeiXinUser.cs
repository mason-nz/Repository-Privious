using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.UserManage
{
    public class LE_WeiXinUser
    {
        public int WeiXinUserID { get; set; } = -2;
        public int subscribe { get; set; } = -2;
        public string openid { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
        public int sex { get; set; } = -2;
        public string city { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string headimgurl { get; set; } = string.Empty;
        public DateTime subscribe_time { get; set; } = new DateTime(1900, 1, 1);
        public string unionid { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int groupid { get; set; } = -2;
        public string tagid_list { get; set; } = string.Empty;
        public int UserID { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public DateTime LastUpdateTime { get; set; } = new DateTime(1900, 1, 1);
        public DateTime AuthorizeTime { get; set; } = new DateTime(1900, 1, 1);
        public string QRcode { get; set; } = string.Empty;
        public string Inviter { get; set; } = string.Empty;
        public string InvitationQR { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public int Source { get; set; } = -2;
        public int AdvertiserUserId { get; set; } = -2;
    }
}
