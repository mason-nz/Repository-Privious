using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ModifyPublishStatusDTO
    {
        /// <summary>
        /// BasicDetail表主键 APP广告位上下架用
        /// </summary>
        public string RecID { get; set; }

        /// <summary>
        /// 刊例ID
        /// </summary>
        public int PubID { get; set; }

        public PublishStatusEnum Status { get; set; }

        public bool CheckSelfModel (out string msg){

            StringBuilder sb = new StringBuilder();
            if (PubID == 0)
                sb.AppendLine("缺少刊例ID");
            if(!System.Enum.IsDefined(typeof(PublishStatusEnum),Status))
                sb.AppendLine("上下架状态错误");
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
