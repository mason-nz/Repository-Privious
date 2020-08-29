/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 11:12:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1_11
{
    public class RespMaterielListDto
    {
        public int MaterielId { get; set; }
        public int ThirdId { get; set; }
        public string MaterielName { get; set; }
        public int ArticleFrom { get; set; }
        public string ArticleFromName { get; set; }
        public int SerialId { get; set; }
        public string SerialName { get; set; }
        public string BrandName { get; set; }
        public string ContractNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public int ChannelCount { get; set; }
    }
}