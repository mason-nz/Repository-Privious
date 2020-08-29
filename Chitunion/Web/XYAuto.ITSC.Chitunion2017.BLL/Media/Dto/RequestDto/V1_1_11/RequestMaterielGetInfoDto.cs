/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 11:08:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11
{
    public class RequestMaterielQueryDto : CreatePublishQueryBase
    {
        public string MaterielName { get; set; }
        public string ContractNumber { get; set; }

        public int CarSerialId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public string SatrtDate { get; set; }
        public string EndDate { get; set; }
    }

    public class RequestMaterielGetInfoDto
    {
        public int MaterielId { get; set; }
        public int ThirdId { get; set; }
        public bool IsGetChannelInfo { get; set; } = true;
        public string ChannelIds { get; set; }
    }
}