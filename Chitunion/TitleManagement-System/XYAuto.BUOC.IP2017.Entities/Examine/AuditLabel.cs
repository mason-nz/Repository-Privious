using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.Examine
{
    /// <summary>
    /// 审核或修改媒体车型标签条件
    /// </summary>
    public class AuditLabel
    {
        /// <summary>
        /// 批次ID
        /// </summary>
        public int BatchID { get; set; }
        /// <summary>
        /// 操作类型（1审核 2修改）
        /// </summary>
        public int OperateType { get; set; }
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
    public class IpLabel
    {
        /// <summary>
        /// 母IP主键
        /// </summary>
        public int PIPID { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int DictId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string DictName { get; set; }
        /// <summary>
        /// 子ip集合
        /// </summary>
        public List<SonIpLabel> SubIP { get; set; }

    }
    public class SonIpLabel
    {
        /// <summary>
        ///子IP主键
        /// </summary>
        public int SubIPID { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int DictId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string DictName { get; set; }
        /// <summary>
        /// 子ip下标签集合
        /// </summary>
        public List<BasicLabelInfo> Label { get; set; }
    }
}
