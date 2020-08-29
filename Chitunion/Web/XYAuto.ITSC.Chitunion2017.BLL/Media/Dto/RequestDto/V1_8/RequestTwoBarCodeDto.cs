/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 11:52:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8
{
    public class RequestTwoBarCodeDto
    {
        public List<TwoBarCodeDto> Item { get; set; }
    }

    public class TwoBarCodeDto
    {
        [Necessary(MtName = "OrderId")]
        public string OrderId { get; set; }

        public int MediaType { get; set; } = (int)Entities.Enum.MediaType.WeiXin;

        [Necessary(MtName = "MediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int MediaId { get; set; }

        [Necessary(MtName = "Url")]
        public string Url { get; set; }

        public string TwoBarUrl { get; set; }
    }

    public class RequestGetChannelDto
    {
        //[Necessary(MtName = "ChannelId")]
        public int ChannelId { get; set; }

        [Necessary(MtName = "MediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int MediaId { get; set; }

        [Necessary(MtName = "AdPosition1", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdPosition1 { get; set; }

        [Necessary(MtName = "AdPosition2", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdPosition2 { get; set; } = 7002;

        [Necessary(MtName = "AdPosition3", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdPosition3 { get; set; }

        [Necessary(MtName = "CooperateDate")]
        public string CooperateDate { get; set; }
    }
}