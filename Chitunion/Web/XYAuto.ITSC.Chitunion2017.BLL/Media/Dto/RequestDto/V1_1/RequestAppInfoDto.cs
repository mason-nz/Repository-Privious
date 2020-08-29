/********************************************************
*创建人：lixiong
*创建时间：2017/6/5 12:16:56
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestInfoDto
    {
        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int BusinessType { get; set; }
    }

    public class RequestAppInfoDto : RequestInfoDto
    {
        public int MediaId { get; set; }
        public int BaseMediaId { get; set; }
    }
}