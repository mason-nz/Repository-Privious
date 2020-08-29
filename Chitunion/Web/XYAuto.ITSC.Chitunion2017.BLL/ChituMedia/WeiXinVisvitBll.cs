using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class WeiXinVisvitBll
    {
        public static readonly WeiXinVisvitBll Instance = new WeiXinVisvitBll();
        //#region 初始化
        //private WeiXinVisvitBll() { }

        //public static WeiXinVisvitBll instance = null;
        //public static readonly object padlock = new object();

        //public static WeiXinVisvitBll Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (padlock)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new WeiXinVisvitBll();
        //                }
        //            }
        //        }
        //        return instance;
        //    }
        //}
        //#endregion
        /// <summary>
        /// 添加用户有效访问日志信息
        /// </summary>
        /// <returns></returns>
        public int AddWeiXinVisvitInfo(WeiXinVisvitModel WXModel)
        {
            return WeiXinVisvitDa.Instance.AddWeiXinVisvitInfo(WXModel);
        }

        public long GetPromotionChannelID(string url)
        {
            Loger.Log4Net.Info($"WeiXinVisvitBll GetPromotionChannelID 开始 url: {url}");
            long dictid = 0;
            string tmpChannelCode = string.Empty;
            try
            {
                //string tmpChannelCode = "ctlmcaidan";
                string val = BLL.Util.GetQueryString("channel", url);
                if (!string.IsNullOrEmpty(val))
                {
                    tmpChannelCode = val;
                }
                //if (url.Contains("?"))
                //{
                //    var array1 = url.Split('?');
                //    if (array1.Length > 1)
                //    {
                //        var tmp1 = array1[1];
                //        if (tmp1.Contains("="))
                //        {
                //            var array2 = tmp1.Split('=');
                //            if (array2.Length > 1)
                //            {
                //                tmpChannelCode = array2[1];
                //            }
                //        }
                //    }
                //}
                Loger.Log4Net.Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} ChannelCode:{tmpChannelCode}");
                var dicList = WeiXinVisvitDa.Instance.GetDictList();
                var dic = dicList.Find(x => x.ChannelCode == tmpChannelCode);
                if (dic != null)
                    dictid = dic.DictID;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} error:{ex}");
            }
            Loger.Log4Net.Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} ChannelCode:{tmpChannelCode} DictID:{dictid}");
            Loger.Log4Net.Info($"WeiXinVisvitBll GetPromotionChannelID url: {url} OK");
            return dictid;
        }
        /// <summary>
        /// 根据渠道编码获取渠道ID
        /// </summary>
        /// <param name="ChannelCode"></param>
        /// <returns></returns>
        public Int64 GetByChanneID(string ChannelCode)
        {
            return WeiXinVisvitDa.Instance.GetByChanneID(ChannelCode);
        }
        public void AddWeiXinVisvitInfo(int userId, string url, string userAgent = null)
        {
            Loger.Log4Net.Info($"WeiXinVisvitBll AddWeiXinVisvitInfo 开始 UserID: {userId},URL={url}");
            try
            {
                string tmpChannelCode = "ctlmcaidan";
                string val = BLL.Util.GetQueryString("channel", url);
                if (!string.IsNullOrEmpty(val))
                {
                    tmpChannelCode = val;
                }
                else if (HttpContext.Current.Request.Cookies["promotionName"] != null)
                {
                    tmpChannelCode = HttpContext.Current.Request.Cookies["promotionName"].Value;
                    Loger.Log4Net.Info("获取cookie值：" + tmpChannelCode);
                    //if (!string.IsNullOrEmpty(promotionName))
                    //{
                    //    tmpChannelCode = XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetByChanneID(promotionName);
                    //}
                }
                //if (url.Contains("?"))
                //{
                //    var array1 = url.Split('?');
                //    if (array1.Length > 1)
                //    {
                //        var tmp1 = array1[1];
                //        if (tmp1.Contains("="))
                //        {
                //            var array2 = tmp1.Split('=');
                //            if (array2.Length > 1)
                //            {
                //                tmpChannelCode = array2[1];
                //            }
                //        }
                //    }                    
                //}

                AddWeiXinVisvitInfo(new WeiXinVisvitModel()
                {
                    UserID = userId,
                    ChannelCode = tmpChannelCode,
                    Url = url,
                    LastUpdateTime = DateTime.Now,
                    Type = 1,
                    UserAgent = string.IsNullOrEmpty(userAgent) ? string.Empty : userAgent
                });

                //更新用户渠道
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"WeiXinVisvitBll AddWeiXinVisvitInfo UserID: {userId},URL={url}", ex);
            }
            Loger.Log4Net.Info($"WeiXinVisvitBll AddWeiXinVisvitInfo UserID: {userId},URL={url} OK");
        }
    }
}
