using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeChat
{
    public class LE_WXUserScene
    {
        public int RecID { get; set; } = -2;
        public int UserID { get; set; } = -2;
        public int SceneID { get; set; } = -2;
        public string SceneName { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int Status { get; set; } = -2;
    }
}
