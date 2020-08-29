/********************************************************
*创建人：lixiong
*创建时间：2017/9/26 10:14:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Infrastruction.ErrorException
{
    public class PullDataSettingsException : Exception
    {
        public PullDataSettingsException(string message = "请配置相关参数")
            : base(message)
        {
        }
    }
}