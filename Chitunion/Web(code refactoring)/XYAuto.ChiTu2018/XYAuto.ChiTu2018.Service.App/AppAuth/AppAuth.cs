namespace XYAuto.ChiTu2018.Service.App.AppAuth
{
    /// <summary>
    /// APP调用Api接口签名配置类（从web.config配置文件[ChiTuApp_API_AppIDSecret]中读取）
    /// </summary>
    public class AppAuth
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string Appid { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态（0-正常，-1-停用）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 请求同一个API时，当前请求url有效期（秒）
        /// </summary>
        public int VerifyTimeOut { get; set; } = 180;
    }
}
