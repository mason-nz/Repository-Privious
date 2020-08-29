using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.LeTask
{
    /// <summary>
    /// 注释：LeTaskBasicSupport
    /// 作者：lix
    /// 日期：2018/5/21 19:42:05
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public static class LeTaskBasicSupport
    {
        /// <summary>
        /// 从App端，分享文章详情页面到微信端的域名列表
        /// </summary>
        public static string DominArticleShare
        {
            get
            {
                return XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("DominArticleShare", false);
            }
        }

        /// <summary>
        /// 给物料url追加参数utm_term={code}
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string SetUrlParamsContent(string url, string code)
        {
            //http://news.chitunion.com/materiel/20171206/17472.html?utm_source=chitu&utm_term=chitunionm
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;
            string content;
            if (url.Contains("?"))
            {
                //代表不是域名，而是一个带参数的url地址
                content = $"{url}&utm_source=chitu&utm_term={code}";
            }
            else
            {
                content = $"{url}?utm_source=chitu&utm_term={code}";
            }
            return content;
        }

        /// <summary>
        /// 切换随机域名？
        /// </summary>
        /// <param name="orderurl"></param>
        /// <returns></returns>
        public static string GetDomainByRandom_ShareArticle(string orderurl)
        {
            string dominUrl = GetDomainByRandom();
            //ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"[GetDomainByRandom_ShareArticle]dominUrl:{dominUrl}");
            if (!string.IsNullOrEmpty(dominUrl) && orderurl.Contains("ct_m"))
                orderurl = $"http://{dominUrl}{orderurl.Substring(orderurl.IndexOf("ct_m", StringComparison.Ordinal) - 1)}";

            return orderurl;
        }

        /// <summary>
        /// 随机获取配置文件中【DominArticleShare】的域名
        /// </summary>
        /// <returns>随机返回一个域名，若没有配置则返回null</returns>
        public static string GetDomainByRandom()
        {
            return GetDomainByRandom(new Random());
        }

        /// <summary>
        /// 随机获取配置文件中【DominArticleShare】的域名
        /// </summary>
        /// <returns>随机返回一个域名，若没有配置则返回null</returns>
        public static string GetDomainByRandom(Random r)
        {
            if (!string.IsNullOrEmpty(DominArticleShare))
            {
                string[] list = DominArticleShare.Split(',');
                return list[r.Next(0, list.Length)];
            }
            return string.Empty;
        }
    }
}
