/********************************************************
*创建人：hant
*创建时间：2017/12/18 14:01:47 
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
    public class AppInfo
    {
        public int AppID { get; set; }
        public string AppName { get; set; }
        public string AppKey { get; set; }
        public int ChannelID { get; set; }
        public string Introducce { get; set; }
        public string CallBackUrl { get; set; }
        public string LinkName { get; set; }
        public string LinkTel { get; set; }
        public int UserID { get; set; }
        public DateTime ValidDate { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
