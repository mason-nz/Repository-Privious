using System;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.User
{
    /// <summary>
    /// 注释：ReqPostWxUserOperationDto
    /// 作者：lix
    /// 日期：2018/6/8 14:50:37
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class PsReqPostWxUserOperationDto
    {
        [Necessary(MtName = "openid")]
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string language { get; set; }
        public string headimgurl { get; set; }
        public DateTime subscribe_time { get; set; }
        [Necessary(MtName = "unionid")]
        public string unionid { get; set; }
        public string remark { get; set; }
        public int groupid { get; set; }
        public string tagid_list { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public DateTime AuthorizeTime { get; set; }

        public string QRcode { get; set; }
        public string Inviter { get; set; }

        public string InvitationQR { get; set; }

        [Necessary(MtName = "RegisterFrom", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入RegisterFrom")]
        public int RegisterFrom { get; set; }//注册来源：PC，资源管理系统
        [Necessary(MtName = "RegisterType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入RegisterType")]
        public int RegisterType { get; set; }//注册方式：帐号密码，微信

        [Necessary(MtName = "Source", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入微信用户的来源Source")]
        public int Source { get; set; }
        [Necessary(MtName = "PromotionChannelId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入PromotionChannelId")]
        public long PromotionChannelId { get; set; }

        [Necessary(MtName = "RegisterIp")]
        public string RegisterIp { get; set; }
    }
}
