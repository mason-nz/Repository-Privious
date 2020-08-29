namespace XYAuto.ChiTu2018.Service.App.User.Dto.LoginForWeChat
{
    /// <summary>
    /// 注释：ReqLoginDto
    /// 作者：lix
    /// 日期：2018/6/8 16:06:09
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqLoginDto
    {
        public string mobile { get; set; }
        public string mobileCheckCode { get; set; }
        public string openid { get; set; } = string.Empty;
        public string unionid { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string headimgurl { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
        public string Ip { get; set; }
        public int from { get; set; } = 3008; //3008:android   3009：ios
    }
}
