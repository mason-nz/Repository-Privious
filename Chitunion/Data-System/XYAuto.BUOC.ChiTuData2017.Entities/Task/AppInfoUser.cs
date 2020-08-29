/********************************************************
*创建人：hant
*创建时间：2017/12/18 14:04:20 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Task
{
    public class AppInfoUser
    {
        public int AppUserID { get; set; }
        public int AppID { get; set; }
        public string UserIdentity { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
