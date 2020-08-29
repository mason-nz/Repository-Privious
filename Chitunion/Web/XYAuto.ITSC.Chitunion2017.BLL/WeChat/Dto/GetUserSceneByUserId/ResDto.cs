using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.GetUserSceneByUserId
{
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
