/********************************************************
*创建人：hant
*创建时间：2018/1/12 17:56:45 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    public class SceneInfoUser
    {
        public int UserId { get; set; }
         public List<UserScene> SceneInfo { get; set; }
    }

    public class UserScene
    {
        public int SceneID { get; set; } = -2;
        public string SceneName { get; set; } = string.Empty;
    }
}
