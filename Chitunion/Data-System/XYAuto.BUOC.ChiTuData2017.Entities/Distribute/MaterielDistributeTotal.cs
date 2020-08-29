/********************************************************
*创建人：lixiong
*创建时间：2017/9/21 14:31:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    public class MaterielDistributeTotal
    {
        public int TotalSessionNumber { get; set; }
        public int TotalInquiryNumber { get; set; }
        public int TotalTelConnectNumber { get; set; }


        public int TotalForwardNumber { get; set; }

        public int TotalMateriel { get; set; }

        public int TotalDistribute { get; set; }

        public int TotalClue { get; set; }
    }
}