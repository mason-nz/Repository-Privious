/********************************************************
*创建人：lixiong
*创建时间：2017/10/27 17:35:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Dto.RequestDto
{
    public class RequestGetMaterielInfoDto
    {
        [Necessary(MtName = "MaterielId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int MaterielId { get; set; }

        [Necessary(MtName = "DistributeType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int DistributeType { get; set; }
    }
}