using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class MediaExistsDTO
    {
        public int MediaID { get; set; }
        /// <summary>
        /// 是否存在
        /// </summary>
        public bool IsExists { get; set; }
        /// <summary>
        /// 刊例ID
        /// </summary>
        public int PubID { get; set; }
        /// <summary>
        /// 广告位数量
        /// </summary>
        public int ADCount { get; set; }
    }
}
