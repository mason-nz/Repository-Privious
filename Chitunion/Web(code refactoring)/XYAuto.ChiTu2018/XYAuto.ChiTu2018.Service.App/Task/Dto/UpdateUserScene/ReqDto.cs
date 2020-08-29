using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.App.Task.Dto.UpdateUserScene
{
    /// <summary>
    /// 注释：<NAME>
    /// 作者：lihf
    /// 日期：2018/5/9 20:06:45
    /// </summary>
    public class ReqDto
    {
        public int UserID = -2;
        public string OpenId { get; set; }
        public List<Scene> SceneInfo { get; set; }
    }
    public class Scene
    {
        public int SceneID { get; set; }
        public string SceneName { get; set; }
    }
}
