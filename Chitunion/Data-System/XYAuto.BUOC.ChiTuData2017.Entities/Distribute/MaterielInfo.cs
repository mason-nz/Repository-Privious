/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 14:47:37
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    public class MaterielInfo
    {
        public int MaterielId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int Source { get; set; }
        public string SourceName { get; set; }
        public string SerialName { get; set; }
        public string BrandName { get; set; }
        public string Ip { get; set; }
        public string ChildIp { get; set; }

        public string Channel { get; set; }
        public string SceneName { get; set; }
        public string IpList { get; set; }
        public DateTime AssembleTime { get; set; }//组装时间
        public string AssembleUser { get; set; }
        public string DistributeUserAgent { get; set; }//经纪人分发人
        public string DistributeUserQwy { get; set; }//全网域分发人

        public DateTime DistributeDateAgent { get; set; }//经纪人分发时间
        public DateTime DistributeDateQWY { get; set; }//全网域分发时间

        public string ArticleTitle { get; set; }

        public string DistributeUrl { get; set; }
    }
}