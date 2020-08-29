/********************************************************
*创建人：lixiong
*创建时间：2017/10/17 10:50:08
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto
{
    public class QingNiaoAgentQueryDto : CreatePublishQueryBase
    {
        public int DistributeQueryDateOffset { get; set; } = 7;
    }
}