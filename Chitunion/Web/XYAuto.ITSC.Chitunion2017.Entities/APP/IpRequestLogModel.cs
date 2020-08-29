/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.Entities.APP
* 类 名 称 ：IpRequestLogModel
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/25 10:39:38
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.APP
{
    public class IpRequestLogModel
    {
        public int RecID { get; set; }
        public string IP { get; set; }
        public string Url { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public string CreateTime { get; set; }
    }
}
