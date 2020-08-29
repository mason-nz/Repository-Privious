namespace XYAuto.ChiTu2018.Service.App.Task.Dto.SubmitOrderUrl
{
    /// <summary>
    /// 注释：ReqDto
    /// 作者：lihf
    /// 日期：2018/5/11 11:46:47
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqDto
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public int ChannelId { get; set; }

        public string OrderUrl { get; set; }
        public string PromotionChannelID { get; set; }
    }
}
