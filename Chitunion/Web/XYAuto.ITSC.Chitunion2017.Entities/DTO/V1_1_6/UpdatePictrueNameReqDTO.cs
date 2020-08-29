using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class UpdatePictrueNameReqDTO
    {
        /// <summary>
        /// 图片id
        /// </summary>
        public int PicID { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string PicName { get; set; }
    }
}
