/********************************************************
*创建人：lixiong
*创建时间：2017/9/13 16:29:54
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    public class MaterielTemp
    {
        public int CSID { get; set; }
        public int SceneId { get; set; }
        public string Title { get; set; }
        public int ArticleId { get; set; }
        public int Resource { get; set; }
        public string Content { get; set; }

        public int ReadNum { get; set; }
    }
}