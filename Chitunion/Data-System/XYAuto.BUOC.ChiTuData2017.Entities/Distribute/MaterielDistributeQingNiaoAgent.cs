/********************************************************
*创建人：lixiong
*创建时间：2017/10/16 16:34:06
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
    public class MaterielDistributeQingNiaoAgent
    {
        public int Id { get; set; }

        public int MaterielId { get; set; }

        public int ArticleId { get; set; }
        public int Type { get; set; }//1：头部文章 2：腰部文章
        public DateTime DistributeDate { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
    }
}