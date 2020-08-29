/********************************************************
*创建人：lixiong
*创建时间：2017/10/24 14:58:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response
{
    public class RespMaterielDetailDto
    {
        [JsonProperty("TaskID")]
        public int TaskId { get; set; }

        [JsonProperty("RecID")]
        public int RecId { get; set; }

        /// <summary>
        /// 1：头部文章 2：腰部文章
        /// </summary>
        [JsonProperty("TypeID")]
        public int TypeId { get; set; }

        public DateTime SendTime { get; set; }
    }
}