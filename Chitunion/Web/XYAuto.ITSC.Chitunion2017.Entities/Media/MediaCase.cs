using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    /// <summary>
    /// 媒体案列信息 2017-04-22 张立彬
    /// </summary>
    public class MediaCase
    {
        /// <summary>
        /// 媒体类型
        /// </summary>
        public EnumMediaType MediaType { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public int MediaID { get; set; }
        /// <summary>
        /// 案例内容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 0预览，1正式
        /// </summary>
        public int CaseStatus { get; set; }
    }
}
