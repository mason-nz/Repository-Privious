/********************************************************
*创建人：lixiong
*创建时间：2017/11/28 17:46:38
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.DataView
{
    public class RespDvYesterdayDto
    {
        public int HeadArticle { get; set; }
        public int HeadArticleAccount { get; set; }
        public int HeadAutoClean { get; set; }
        public int HeadAutoCleanAccount { get; set; }
        public int WaistArticle { get; set; }
        public int WaistArticleClean { get; set; }
        public int WaistArticleMatched { get; set; }
        public int WaistArticleUnmatched { get; set; }
        public int MaterialPackaged { get; set; }
        public int MaterialDistribute { get; set; }
        public int MaterialForward { get; set; }
        public int Clues { get; set; }
    }
}