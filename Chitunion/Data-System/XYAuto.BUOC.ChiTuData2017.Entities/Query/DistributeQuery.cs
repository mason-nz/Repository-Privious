/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 11:17:17
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Query
{
    public class DistributeQuery<T> : QueryPageBase<T>
    {
        public int DistributeId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int MaterielId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Date { get; set; }
        public int Source { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}