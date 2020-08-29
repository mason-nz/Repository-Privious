using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.App.Task.Dto.GetUserSceneByUserId
{
    /// <summary>
    /// 注释：ResDto
    /// 作者：lihf
    /// 日期：2018/5/14 14:41:06
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ResDto
    {
        public List<Category> CategoryList;
        public bool IsSkip { get; set; } = false;
    }
    public class Category
    {
        public int SceneID { get; set; } = -2;
        public string SceneName { get; set; } = string.Empty;
        public int IsSelected { get; set; } = 0;
        public int Counts { get; set; } = 0;
    }
}
