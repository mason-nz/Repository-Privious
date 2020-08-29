/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 19:50:43
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi
{
    public class ZhySignatureProvider
    {
        public static string GetSignatureText(Dictionary<string, object> dicParams)
        {
            var dicParm = GetSortDicValue(dicParams);
            Loger.ZhyLogger.Info($"加密串：{dicParm}");
            return HttpUtility.UrlEncode(SignUtility.GetSignature(dicParm), Encoding.UTF8);
        }

        public static string GetSortDicValue(Dictionary<string, object> dicParams)
        {
            var dictSortedAsc = dicParams.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            string signature = string.Empty;
            foreach (var item in dictSortedAsc)
            {
                signature += item.Value + "";
            }
            return signature;
        }
    }
}