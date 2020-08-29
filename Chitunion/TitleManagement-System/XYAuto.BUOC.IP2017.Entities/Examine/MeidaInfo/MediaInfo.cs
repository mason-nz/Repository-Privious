using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.Entities.Examine.MeidaInfo
{
    /// <summary>
    /// zlb 2017-10-20
    /// 审核媒体类
    /// </summary>
    public class MediaInfo
    {
        /// <summary>
        ///审核批次ID
        /// </summary>
        public int BatchID { get; set; }
        /// <summary>
        /// 媒体账号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 媒体类型名称
        /// </summary>
        public string MediaTypeName { get; set; }
        /// <summary>
        /// 媒体账号
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImg { get; set; }
        /// <summary>
        /// 主页地址
        /// </summary>
        public string HomeUrl { get; set; }
    }
}
