namespace XYAuto.ChiTu2018.Service.App.ThirdApi.Dto.Response
{
    /// <summary>
    /// 注释：RespChituBaseDto
    /// 作者：lix
    /// 日期：2018/5/23 17:44:40
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespChituBaseDto<T>
    {
        public T Result { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回状态值的说明
        /// </summary>
        public string Message { get; set; }
    }
}
