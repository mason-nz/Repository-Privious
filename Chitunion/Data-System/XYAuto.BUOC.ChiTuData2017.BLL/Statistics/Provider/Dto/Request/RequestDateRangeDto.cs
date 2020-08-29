/********************************************************
*创建人：lixiong
*创建时间：2017/11/30 18:55:56
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Request
{
    public class RequestDateRangeDto
    {
        public string StartDate { get; set; } = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        public string EndDate { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
    }
}