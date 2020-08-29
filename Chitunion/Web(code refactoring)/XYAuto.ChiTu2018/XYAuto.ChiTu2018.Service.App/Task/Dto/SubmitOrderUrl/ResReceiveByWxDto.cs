namespace XYAuto.ChiTu2018.Service.App.Task.Dto.SubmitOrderUrl
{
    /// <summary>
    /// 注释：ResReceiveByWxDto
    /// 作者：lihf
    /// 日期：2018/5/14 10:10:48
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ResReceiveByWxDto
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }
    }
    public class Result
    {
        public string OrderUrl { get; set; }
        public int OrderId { get; set; }

        public string PasterUrl { get; set; }
    }
}
