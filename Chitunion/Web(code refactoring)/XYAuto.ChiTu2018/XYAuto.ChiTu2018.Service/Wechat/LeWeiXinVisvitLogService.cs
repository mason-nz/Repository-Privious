using System;
using System.Web;
using XYAuto.ChiTu2018.BO.Wechat;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.CTUtils.Html;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.Service.Wechat
{
    /// <summary>
    /// 注释：微信及移动端下，记录用户访问日志逻辑
    /// 作者：masj
    /// 日期：2018-05-09 
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeWeiXinVisvitLogService
    {
        private LeWeiXinVisvitLogService() { }
        private static readonly Lazy<LeWeiXinVisvitLogService> linstance = new Lazy<LeWeiXinVisvitLogService>(() => new LeWeiXinVisvitLogService());

        public static LeWeiXinVisvitLogService Instance => linstance.Value;


        /// <summary>
        /// 根据用户ID、页面当前URL、Request中的userAgent，记录访问日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="url">页面当前URL</param>
        /// <param name="userAgent">记录访问日志</param>
        public LE_WeiXinVisvit_Log AddWeiXinVisvitInfo(int userId, string url, string userAgent = null)
        {
            Log4NetHelper.Default().Info($"WeiXinVisvitBll AddWeiXinVisvitInfo 开始 UserID: {userId},URL={url}");
            try
            {
                string tmpChannelCode = "ctlmcaidan";
                string val = HtmlHelper.GetQueryStringByUrl("channel", url);
                if (!string.IsNullOrEmpty(val))
                {
                    tmpChannelCode = val;
                }
                else if (HttpContext.Current != null &&
                         HttpContext.Current.Request.Cookies["promotionName"] != null)
                {
                    tmpChannelCode = HttpContext.Current.Request.Cookies["promotionName"].Value;
                    Log4NetHelper.Default().Info("获取cookie值：" + tmpChannelCode);
                }

                var model = new LeWeiXinVisvitLogBO().Add(new LE_WeiXinVisvit_Log()
                {
                    UserID = userId,
                    ChannelCode = tmpChannelCode,
                    Url = url,
                    LastUpdateTime = DateTime.Now,
                    Type = 1,
                    UserAgent = string.IsNullOrEmpty(userAgent) ? string.Empty : userAgent
                });
                return model;
                //更新用户渠道
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error($"WeiXinVisvitBll AddWeiXinVisvitInfo UserID: {userId},URL={url}", ex);
                return null;
            }
            finally
            {
                Log4NetHelper.Default().Info($"WeiXinVisvitBll AddWeiXinVisvitInfo UserID: {userId},URL={url} OK");
            }
        }

    }
}
