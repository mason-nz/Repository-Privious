/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 17:27:51
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request
{
    public class RequestDeletePageDto : RequestGroundPage
    {
        [Necessary(MtName = "GroundId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int GroundId { get; set; }
    }
}