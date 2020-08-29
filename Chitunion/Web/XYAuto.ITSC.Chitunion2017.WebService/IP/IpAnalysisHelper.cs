/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.WebService.IP
* 类 名 称 ：IpAnalysis
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/25 9:40:38
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.IP
{
    public class IpAnalysisHelper
    {
        /// <summary>
        /// 根据IP获取所在省、市
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public string GetAreaByIp(string IP)
        {

            HttpWebResponse hwr = Common.HttpHelper.CreateGetHttpResponse($"http://api.sys.xingyuanauto.com/ip/GetAreaByIp?ipStr={IP}");

            string msg = Common.HttpHelper.GetResponseString(hwr);

            return msg;
        }
    }
}
