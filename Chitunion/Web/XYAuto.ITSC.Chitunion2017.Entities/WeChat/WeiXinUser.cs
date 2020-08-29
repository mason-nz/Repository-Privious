/********************************************************
*创建人：hant
*创建时间：2018/1/17 13:16:57 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeChat
{
    public class WeiXinUser
    {
        public int WeiXinUserID { get; set; }
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string language { get; set; }
        public string headimgurl { get; set; }
        public DateTime subscribe_time { get; set; }
        public string unionid { get; set; }
        public string remark { get; set; }
        public int groupid { get; set; }
        public string tagid_list { get; set; }
        public int UserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public DateTime AuthorizeTime { get; set; } 

        public string QRcode { get; set; }
        public string Inviter { get; set; }

        public string InvitationQR { get; set; }
        public int Status { get; set; }
        public int Category { get; set; }//用户分类（29001—广告主；29002—媒体主）
        public int RegisterFrom { get; set; }//注册来源：PC，资源管理系统
        public int RegisterType { get; set; }//注册方式：帐号密码，微信
        public int Source { get; set; }
        public int AdvertiserUserId { get; set; }
        public long PromotionChannelID { get; set; } = 0;

        public string RegisterIp { get; set; }

        /// <summary>
        /// 用户类型,引用LE_WeixinInfo表主键ID
        /// </summary>
        public int UserType { get; set; }
    }
}
