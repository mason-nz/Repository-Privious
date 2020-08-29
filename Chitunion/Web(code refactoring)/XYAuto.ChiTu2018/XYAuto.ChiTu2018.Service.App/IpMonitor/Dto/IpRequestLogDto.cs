/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.IpMonitor
* 类 名 称 ：IpRequestLogDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/5 10:47:43
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.Dto.IpMonitor
{
    public class IpRequestLogDto
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
