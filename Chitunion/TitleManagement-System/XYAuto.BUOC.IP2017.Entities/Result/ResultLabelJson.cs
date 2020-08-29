using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.Examine;

namespace XYAuto.BUOC.IP2017.Entities.Result
{
    public class ResultLabelJson
    {
        /// <summary>
        /// 结果ID
        /// </summary>
        public int MediaResultID { get; set; }
        /// <summary>
        /// 任务类型(2001：媒体 2002：子品牌 2003：车型)
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public List<BasicLabelInfo> Category { get; set; }
        /// <summary>
        /// 市场场景
        /// </summary>
        public List<BasicLabelInfo> MarketScene { get; set; }
        /// <summary>
        /// 分发场景
        /// </summary>
        public List<BasicLabelInfo> DistributeScene { get; set; }

        public List<IpLabel> IPLabel { get; set; }
    }
}
