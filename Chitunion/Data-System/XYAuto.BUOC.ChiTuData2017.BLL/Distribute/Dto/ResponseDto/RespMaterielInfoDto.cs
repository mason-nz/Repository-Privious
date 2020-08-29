/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 16:59:01
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Dto.ResponseDto
{
    public class RespMaterielInfoDto
    {
        public int MaterielId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public int Source { get; set; }
        public string DistributeTypeName { get; set; }

        public string SerialName { get; set; }
        public string BrandName { get; set; }
        public string Ip { get; set; }
        public string ChildIp { get; set; }
        public string AssembleUser { get; set; }
        public DateTime AssembleTime { get; set; }
        public string DistributeUser { get; set; }
        public DateTime DistributeTime { get; set; }
        public string Channel { get; set; }
        public string Scene { get; set; }
        public string ArticleTitle { get; set; }
        public string DistributeUrl { get; set; }
    }
}