/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 10:14:49
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request
{
    public class RequestPushUserDto : RequestBaseTokenDto
    {
        [Necessary(MtName = "UserName")]
        public string UserName { get; set; }

        [Necessary(MtName = "Mobile")]
        public string Mobile { get; set; }

        [Necessary(MtName = "ContactsPerson")]
        public string ContactsPerson { get; set; }

        [Necessary(MtName = "CorporationName")]
        public string CorporationName { get; set; }

        [Necessary(MtName = "OrganizeId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OrganizeId { get; set; }
    }
}