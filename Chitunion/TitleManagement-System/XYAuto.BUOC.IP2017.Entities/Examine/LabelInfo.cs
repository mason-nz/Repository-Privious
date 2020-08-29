using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.Examine
{
    /// <summary>
    /// zlb 2017-10-23
    /// 标签基本信息
    /// </summary>
    public class BasicLabelInfo
    {
        /// <summary>
        /// 标签类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int DictId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string DictName { get; set; }
        /// <summary>
        /// 标签创建人
        /// </summary>
        public string Creater { get; set; }
    }
    /// <summary>
    /// zlb  2017-10-23
    /// Ip信息
    /// </summary>
    public class IpLabelInfo
    {
        /// <summary>
        /// 批次ID
        /// </summary>
        public int BatchID { get; set; }
        /// <summary>
        /// ip主键
        /// </summary>
        public int PIPID { get; set; }
        /// <summary>
        /// 子Ip主键
        /// </summary>
        public int SubIPID { get; set; }
        /// <summary>
        /// IPID
        /// </summary>
        public int IpId { get; set; }
        /// <summary>
        /// IP名称
        /// </summary>
        public string IpName { get; set; }
        /// <summary>
        /// 子IPID
        /// </summary>
        public int SonIpId { get; set; }
        /// <summary>
        /// 子IP名称
        /// </summary>
        public string SonIpName { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string LabelName { get; set; }
        /// <summary>
        /// Ip标签创建人
        /// </summary>
        public string IpCreater { get; set; }
        /// <summary>
        /// 子Ip标签创建人
        /// </summary>
        public string SopIpCreater { get; set; }
    }

}
