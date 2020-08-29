namespace XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Response
{
    public class RespBaseChituDto<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回状态值的说明
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回Json对象
        /// </summary>
        public T Result { get; set; }

    }
}
