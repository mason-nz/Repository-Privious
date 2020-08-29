/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 13:35:22
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class TwoBarCodeHistory
    {
        public int RecID { get; set; }
        public string OrderID { get; set; }
        public int MediaType { get; set; }
        public int MediaID { get; set; }

        public string URL { get; set; }
        public string TwoBarUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
    }
}