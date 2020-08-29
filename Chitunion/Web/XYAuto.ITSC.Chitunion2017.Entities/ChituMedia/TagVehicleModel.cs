using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class TagVehicleModel
    {
        /// <summary>
        /// 主品牌名称
        /// </summary>
        public string MasterName { get; set; }
        /// <summary>
        /// 子品牌名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 车系名称
        /// </summary>
        public string SerialName { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string MediaTagName { get; set; }

        public int SerialID { get; set; }
    }

    public class TagVehicleInfoList
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string MediaTagName { get; set; }
        /// <summary>
        /// 车系ID
        /// </summary>
        public int SerialID { get; set; }
    }
}
