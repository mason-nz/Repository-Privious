/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 20:43:07
/// </summary>

namespace XYAuto.ChiTu2018.Service.Task.Dto.GetDataByPage
{
    public class ReqDto
    {
        public string OpenID { get; set; }
        public int PageIndex { get; set; } = -2;
        public int PageSize { get; set; } = -2;
        public int SceneID { get; set; }
        public int UserID { get; set; }
    }
}
