using System;
using System.Text.RegularExpressions;
using System.Web;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.Service.OAuth2
{
    /// <summary>
    /// 注释：OAuth2Service
    /// 作者：lihf
    /// 日期：2018/5/14 17:44:14
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class OAuth2Service
    {
        private OAuth2Service() { }
        private static readonly Lazy<OAuth2Service> Linstance = new Lazy<OAuth2Service>(() => new OAuth2Service());

        public static OAuth2Service Instance => Linstance.Value;        

        public long GetPromotionChannelId(string url)
        {
            Log4NetHelper.Default().Info($"WeiXinVisvitBll GetPromotionChannelID 开始 url: {url}");
            long dictid = 0;
            var tmpChannelCode = string.Empty;
            try
            {
                var val = GetQueryString("channel", url);
                if (!string.IsNullOrEmpty(val))
                    tmpChannelCode = val;

                Log4NetHelper.Default().Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} ChannelCode:{tmpChannelCode}");
                //var dicList = new LePromotionChannelDictBO().GetList(x => true);
                var dic = new LePromotionChannelDictBO().GetModel(x => x.ChannelCode == tmpChannelCode);//dicList.Find(x => x.ChannelCode == tmpChannelCode);
                if (dic != null)
                    dictid = dic.DictID;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} error:{ex}");
            }
            Log4NetHelper.Default().Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} ChannelCode:{tmpChannelCode} DictID:{dictid}");
            Log4NetHelper.Default().Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} OK");
            return dictid;
        }
        /// <summary>  
        /// 获取url字符串参数，返回参数值字符串  
        /// </summary>  
        /// <param name="name">参数名称</param>  
        /// <param name="url">url字符串</param>  
        /// <returns></returns>  
        public string GetQueryString(string name, string url)
        {
            try
            {
                Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
                MatchCollection mc = re.Matches(url);
                foreach (Match m in mc)
                {
                    if (m.Result("$2").Equals(name))
                    {
                        return HttpUtility.UrlDecode(m.Result("$3"));
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("获取url字符串参数，返回参数值字符串[GetQueryString]出错", ex);
                return string.Empty;
            }
        }        
    }
}
