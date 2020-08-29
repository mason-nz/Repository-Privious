/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 11:10:23
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel
{
    public class TrGroupInfo
    {
        public int ArticleId { get; set; }
        public int GroupId { get; set; }
        public int TaskId { get; set; }
        public int CarBrandId { get; set; }
        public int CSID { get; set; }
    }

    public class TrGroupArticle
    {
        public int ArticleId { get; set; }
        public int GroupId { get; set; }
        public int XyAttr { get; set; }//1:头部   2:腰部
    }

    public class TaskSchedulerDto
    {
        public int ArticleId { get; set; }
        public int GroupId { get; set; }
        public string Title { get; set; }
        public int Resource { get; set; }
        public string CategoryName { get; set; }
        public int SerialId { get; set; }
        public string SerialName { get; set; }
        public string BrandName { get; set; }
        public int TaskStatus { get; set; }
        public string TaskStatusName { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public string Category { get; set; }
        public string HeadImg { get; set; }
        public string Content { get; set; }

        public string Abstract { get; set; }

        [JsonIgnore]
        public int XyAttr { get; set; }//1:头部   2:腰部
    }
}