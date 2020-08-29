using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.Open.ComponentAPIs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class WeixinOAuth
    {
        public static readonly BLL.WeixinOAuth Instance = new BLL.WeixinOAuth();
        ILog logger = LogManager.GetLogger(typeof(WeixinOAuth));
        string domainPath = ConfigurationManager.AppSettings["CatchDomain"];
        public static readonly string component_AppId = ConfigurationManager.AppSettings["Component_Appid"];
        public static readonly string component_Secret = ConfigurationManager.AppSettings["Component_Secret"];

        #region 权限
        public int AddOAuthHistory(OAuthHistory his)
        {
            return Dal.WeixinOAuth.Instance.AddOAuthHistory(his);
        }

        public int AddOAuthDetail(List<OAuthDetail> list)
        {
            return Dal.WeixinOAuth.Instance.AddOAuthDetail(list);
        }

        public List<OAuthDetail> GetOAuthDetail(int wxID)
        {
            var list = Dal.WeixinOAuth.Instance.GetOAuthDetail(wxID);
            return list == null ? new List<OAuthDetail>() : list;
        }

        public List<string> GetWeixinAuthorityList(int wxID)
        {
            return Dal.WeixinOAuth.Instance.GetWeixinAuthorityList(wxID);
        }
        #endregion

        #region 微信
        public WeixinInfo GetWeixinInfo(int wxID) {
            return Dal.WeixinOAuth.Instance.GetWeixinInfo(wxID);
        }

        private WeixinInfo GetWeixinInfoByWxNumber(string wxNumber)
        {
            return Dal.WeixinOAuth.Instance.GetWeixinInfoByWxNumber(wxNumber,0,-1);
        }

        public WeixinInfo GetWeixinInfoByAppID(string appID)
        {
            return Dal.WeixinOAuth.Instance.GetWeixinInfoByAppID(appID);
        }

        public int GetWxIDByWxNumber(string wxNumber)
        {
            return Dal.WeixinOAuth.Instance.GetWxIDByWxNumber(wxNumber);
        }
        public int GetWxIDByBiz(string biz)
        {
            return Dal.WeixinOAuth.Instance.GetWxIDByBiz(biz);
        }

        public bool CheckHasRight(int oauthID, int mediaID, out int wxID) {
            return Dal.WeixinOAuth.Instance.CheckHasRight(oauthID,mediaID, out wxID) > 0;
        }

        /// <summary>
        /// 添加微信信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddWeixinInfo(WeixinInfo info)
        {
           // info.Status = 1;//未审核
            return Dal.WeixinOAuth.Instance.AddWeixinInfo(info);
        }

        public int UpdateWeixinInfo(WeixinInfo info)
        {
            return Dal.WeixinOAuth.Instance.UpdateWeixinInfo(info);
        }

        public int UpdateTokenInfo(string appID, string accessToken, string refreshAccessToken, DateTime getTokenTime)
        {
            return Dal.WeixinOAuth.Instance.UpdateTokenInfo(appID, accessToken, refreshAccessToken, getTokenTime);
        }

        public List<WeixinInfo> GetWeixinList()
        {
            return Dal.WeixinOAuth.Instance.GetWeixinList();
        }

        public int UpdateVerifyTicket(string appID, string verifyTicket, DateTime ticketTime)
        {
            return Dal.WeixinOAuth.Instance.UpdateVerifyTicket(appID, verifyTicket, ticketTime);
        }

        #endregion

        #region 图文
        public int AddArticle(Article info)
        {
            return Dal.WeixinOAuth.Instance.AddArticle(info);
        }

        public int UpdateArticle(Article info)
        {
            return Dal.WeixinOAuth.Instance.UpdateArticle(info);
        }

        public List<Article> GetArticleListByDate(int wxID,int dayInterval)
        {
            return Dal.WeixinOAuth.Instance.GetArticleListByDate(wxID,dayInterval);
        }
        #endregion

        #region Component
        public int UpdateAccessToken(string appID, string accessToken, DateTime accessTokenTime)
        {
            return Dal.WeixinOAuth.Instance.UpdateAccessToken(appID, accessToken, accessTokenTime);
        }

        public Component GetComponentDetail(string appID)
        {
            return Dal.WeixinOAuth.Instance.GetComponentDetail(appID);
        }
        #endregion

        #region 统计
        public int ClearReadStatistic(int wxID)
        {
            return Dal.WeixinOAuth.Instance.ClearReadStatistic(wxID);
        }

        public int AddUpdateStatistic(UpdateStatistic_Weixin info)
        {
            return Dal.WeixinOAuth.Instance.AddUpdateStatistic(info);
        }

        public int AddReadStatistic(ReadStatistic_Weixin info) {
            return Dal.WeixinOAuth.Instance.AddReadStatistic(info);
        }

        public List<ReadStatistic_Weixin> GetReadStatistic_Weixin(int wxID, string dateStr)
        {
            return Dal.WeixinOAuth.Instance.GetReadStatistic_Weixin(wxID, dateStr);
        }
        #endregion

        #region 志帅API
        public GetMediaDetailStatisticResDTO GetDetailStatisticByMediaID(int mediaID)
        {
            try
            {
                string wxNumber = Dal.WeixinOAuth.Instance.GetWxNumberByMediaID(mediaID);
                GetMediaDetailStatisticResDTO res = new GetMediaDetailStatisticResDTO();
                var obj = this.GetDetailStatisticByWxNumber(wxNumber);
                res.ReadForWeb = new ReadInfo();
                res.DayUpdateForWeb = new List<StatisticItem>();
                res.HourUpdateForWeb = new List<StatisticItem>();
                for (int i = 30; i > 0; i--)//30天
                    res.DayUpdateForWeb.Add(new StatisticItem() { Key = DateTime.Now.AddDays(-i).ToString("MM-dd"), Value = i%2 });
                for (int i = 0; i < 24; i++)//24小时
                    res.HourUpdateForWeb.Add(new StatisticItem() { Key = i.ToString(), Value = i%2 });
                if (obj.Data != null)
                {
                    res.ReadForWeb = obj.Data.ReadForWeb;
                    if (obj.Data.DayUpdateForWeb != null && obj.Data.DayUpdateForWeb.Count > 0)
                    {
                        obj.Data.DayUpdateForWeb.ForEach(o =>
                        {
                            if (res.DayUpdateForWeb.Exists(r => r.Key.Equals(o.Day.ToString("MM-dd"))))
                            {
                                res.DayUpdateForWeb.Find(r => r.Key.Equals(o.Day.ToString("MM-dd"))).Value = o.ArticleCount;
                            }
                        });
                    }
                    if (obj.Data.HourUpdateForWeb != null && obj.Data.HourUpdateForWeb.Count > 0)
                    {
                        obj.Data.HourUpdateForWeb.ForEach(o =>
                        {
                            if (res.HourUpdateForWeb.Exists(r => r.Key.Equals(o.Hour.Hour.ToString())))
                            {
                                res.HourUpdateForWeb.Find(r => r.Key.Equals(o.Hour.Hour.ToString())).Value = o.ArticleCount;
                            }
                        });
                    }
                }

                int wxID = 0;
                if (this.CheckHasRight(37007, mediaID, out wxID))
                {
                    #region 有权限的话 只有多图文的这样
                    var list = this.GetReadStatistic_Weixin(wxID, DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                    if (list != null && list.Count > 0)
                    {
                        foreach (var one in list)
                        {
                            switch (one.ArticleType)
                            {
                                case 6001:
                                    res.ReadForWeb.ReadAvgSingleCount = one.AverageReading;
                                    res.ReadForWeb.ReadHighestSingleCount = one.MaxReading;
                                    break;
                                case 6002:
                                    res.ReadForWeb.ReadAvgFirstCount = one.AverageReading;
                                    res.ReadForWeb.ReadHighestFirstCount = one.MaxReading;
                                    break;
                                case 6003:
                                    res.ReadForWeb.ReadAvgSencondCount = one.AverageReading;
                                    res.ReadForWeb.ReadHighestSencondCount = one.MaxReading;
                                    break;
                                case 6004:
                                    res.ReadForWeb.ReadAvgThirdCount = one.AverageReading;
                                    res.ReadForWeb.ReadHighestThirdCount = one.MaxReading;
                                    break;
                            }
                        }
                    }
                    #endregion
                }
                return res;
            }
            catch (Exception ex) {
                logger.Error("抓取错误", ex);
                return null;
            }
        }

        private CatchArticleResultDTO GetDetailStatisticByWxNumber(string wxNumber)
        {
            try
            {
                string url = string.Format("{0}/API/WeChat/GetWeChatArticleStatisByWxId?wxid={1}&type={2}", domainPath, wxNumber, 0);
                var request = HttpWebRequest.Create(url) as HttpWebRequest;
                var response = request.GetResponse() as HttpWebResponse;
                string str = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    str = sr.ReadToEnd();
                }
                response.Close();
                request.Abort();
                var obj = JsonConvert.DeserializeObject<CatchArticleResultDTO>(str);
                return obj;
            }
            catch(Exception ex) {
                logger.Error("抓取错误", ex);
                return null;
            }
        }

        /// <summary>
        /// 验证码+Url方式抓取数据调用
        /// </summary>
        /// <param name="yzm"></param>
        /// <param name="getUrl"></param>
        /// <returns></returns>
        public CatchWeixinModel GetCatchDateByYZM(string yzm, string getUrl, ref bool yzmError)
        {
            try
            {
                string url = domainPath + "/API/WeChat/GetRealTimeWeChatInfoByBiz";
                string data = JsonConvert.SerializeObject(new { validateCode = yzm, requestUrl = HttpUtility.UrlEncode(getUrl) });
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "post";
                request.ContentType = "application/json";
                using (var stream = request.GetRequestStream())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    stream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string str = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    str = sr.ReadToEnd();
                }
                response.Close();
                request.Abort();
                var res = JsonConvert.DeserializeObject<CatchWeixinResultDTO>(str);
                if (res.WeChatData == null && res.Message.Equals("该网址不包含验证码")) {
                    yzmError = true;
                }
                return res.WeChatData;
            }
            catch (Exception ex)
            {
                logger.Error("抓取错误", ex);
                return null;
            }
        }

        /// <summary>
        /// ①微信授权补充信息调用②手工添加方式抓取数据调用
        /// </summary>
        /// <param name="wxNumber"></param>
        /// <param name="biz"></param>
        /// <returns></returns>
        public CatchWeixinModel GetCatchData(string wxNumber, string biz = "")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(wxNumber))
                {
                    #region 授权方式补充信息
                    string url = domainPath + "/API/WeChat/GetWeChatInfoByWxId?wxId=" + wxNumber;
                    HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = 3000;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    string str = string.Empty;
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                        str = sr.ReadToEnd();
                    }
                    response.Close();
                    request.Abort();
                    var res = JsonConvert.DeserializeObject<CatchWeixinResultDTO>(str);
                    if (res.WeChatData == null && !string.IsNullOrWhiteSpace(biz))
                    {
                        url =  domainPath + "/API/WeChat/GetWeChatInfoByBiz?biz=" + biz;
                        request = HttpWebRequest.Create(url) as HttpWebRequest;
                        request.Timeout = 3000;
                        response = request.GetResponse() as HttpWebResponse;
                        using (var stream = response.GetResponseStream())
                        {
                            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                            str = sr.ReadToEnd();
                        }
                        response.Close();
                        request.Abort();
                        res = JsonConvert.DeserializeObject<CatchWeixinResultDTO>(str);
                    }
                    return res.WeChatData;
                    #endregion
                }
                else
                {
                    string url = domainPath + "/API/WeChat/GetWeChatInfoByBiz?biz=" + biz;
                    var request = HttpWebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = 3000;
                    var response = request.GetResponse() as HttpWebResponse;
                    string str = string.Empty;
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                        str = sr.ReadToEnd();
                    }
                    response.Close();
                    request.Abort();
                    var res = JsonConvert.DeserializeObject<CatchWeixinResultDTO>(str);
                    return res.WeChatData;
                }
            }
            catch (Exception ex)
            {
                logger.Error("抓取错误", ex);
                return null;
            }
        }

        #endregion

        #region 一般处理程序、服务公用方法
        /// <summary>
        /// ①用抓取的数据补充扫码授权的数据
        /// ②更新验证码方式的数据
        /// ③更新手工方式的数据
        /// </summary>
        /// <param name="wxCatch"></param>
        /// <returns></returns>
        public int UpdateWxInfo(CatchWeixinModel wxCatch,WeixinInfo wxInfo = null, SourceTypeEnum source = SourceTypeEnum.默认)
        {
            int rowcount = 0;
            //抓来的数据肯定有微信号
            if (wxInfo == null)
            { //region ashx进来的
                wxInfo = this.GetWeixinInfoByWxNumber(wxCatch.WxId);
                if (wxInfo == null)
                {//新增
                    wxInfo = new Entities.WeixinOAuth.WeixinInfo() { CreateTime = DateTime.Now, Status = 1, SourceType = (int)source };
                }
                else
                {
                    if (wxInfo.Status.Equals(-1))
                        wxInfo.Status = 1;
                }
            }
            if (wxInfo.SourceType != (int)SourceTypeEnum.扫码授权)
            {
                #region 扫描授权不更新这些字段，手工的SourceType被更新
                //wxInfo.SourceType = (int)SourceTypeEnum.验证码;
                wxInfo.WxNumber = wxCatch.WxId;
                wxInfo.OriginalID = wxCatch.UserName;
                wxInfo.NickName = wxCatch.NickName;
                if (!string.IsNullOrWhiteSpace(wxCatch.HeadImg))
                    wxInfo.HeadImg = domainPath + "/" + wxCatch.HeadImg;
                else
                    wxInfo.HeadImg = "http://www.chitunion.com/images/default_touxiang.png";
                if (!string.IsNullOrWhiteSpace(wxCatch.QrCode))
                    wxInfo.QrCodeUrl = domainPath + "/" + wxCatch.QrCode;
                wxInfo.OAuthStatus = (int)OAuthStatusEnum.未授权;
                #endregion
            }
            #region 补充信息 来源不变
            if (string.IsNullOrWhiteSpace(wxInfo.Biz))
                wxInfo.Biz = wxCatch.Biz;
            DateTime dt = Entities.Constants.Constant.DATE_INVALID_VALUE;
            //全称
            wxInfo.FullName = wxCatch.Name;
            //注册时间
            if (!string.IsNullOrWhiteSpace(wxCatch.RegTime) && DateTime.TryParse(wxCatch.RegTime, out dt))
            {
                wxInfo.RegTime = dt;
            }
            //简介
            wxInfo.Summary = wxCatch.VerifyInfo.Length > 250 ? (wxCatch.VerifyInfo.Substring(0, 248) + "...") : wxCatch.VerifyInfo;
            //统一社会信用代码
            wxInfo.CreditCode = wxCatch.RegisteredId;
            //前置许可经营范围
            wxInfo.BusinessScope = wxCatch.FrontBusinessType;
            //企业类型
            wxInfo.EnterpriseType = wxCatch.EnterpriseType;
            //企业成立日期
            wxInfo.EnterpriseCreateDate = wxCatch.EnterpriseEstablishmentDate;
            //企业营业期限
            wxInfo.EnterpriseBusinessTerm = wxCatch.EnterpriseExpiredDate;
            //企业认证日期
            wxInfo.EnterpriseVerifyDate = wxCatch.VerifyDate;
            #endregion
            wxInfo.ModifyTime = DateTime.Now;
            if (wxInfo.RecID.Equals(0))
            {
                rowcount = this.AddWeixinInfo(wxInfo);
                wxInfo.RecID = rowcount;
            }
            else
            {
                wxInfo.ModifyTime = DateTime.Now;
                rowcount = this.UpdateWeixinInfo(wxInfo);
            }
            logger.Info(wxInfo.RecID + "更新基础信息" + (rowcount > 0 ? "成功" : "失败"));
            if (wxInfo.SourceType != (int)SourceTypeEnum.扫码授权 && wxInfo.RecID > 0)
            {
                var iw = BLL.Interaction.InteractionWeixin.Instance.GetEntityByWxID(wxInfo.RecID);
                if (iw == null)
                {
                    iw = new Entities.Interaction.InteractionWeixin() { WxID = wxInfo.RecID, MeidaType = (int)MediaTypeEnum.微信, CreateTime = DateTime.Now, LastUpdateTime = DateTime.Now };
                }
                SaveInteraction(iw, wxInfo.WxNumber, false);
            }
            return wxInfo.RecID;
        }

        /// <summary>
        /// ①保存多图文基础信息（二次统计时候用）
        /// ②保存多图文统计信息
        /// ③互动参数补充
        /// </summary>
        /// <param name="wxID"></param>
        /// <param name="wxAppID"></param>
        /// <param name="wxToken"></param>
        /// <returns></returns>
        public int SaveArticleInfo(int wxID, string wxAppID, string wxToken, Entities.Interaction.InteractionWeixin iw)
        {
            var oldList = this.GetArticleListByDate(wxID, -30);
            List<Entities.WeixinOAuth.Article> newList = new List<Entities.WeixinOAuth.Article>();
            int rowcount = 0;
            List<string> multipleList = new List<string>();//文章数 文章包含多图文

            #region 保存多图文统计基础数据
            for (int i = 1; i < 31; i++)//30天
            {
                DateTime pubDate = DateTime.Now.AddDays(-i).Date;
                if (!oldList.Exists(a => a.PubDate.Date.Equals(pubDate)))
                {
                    #region 新增
                    try
                    {
                        var res = Senparc.Weixin.MP.AdvancedAPIs.AnalysisApi.GetArticleTotal(wxToken, pubDate.ToString("yyyy-MM-dd"), pubDate.ToString("yyyy-MM-dd"));
                        if (res.list != null && res.list.Count > 0)
                        {
                            foreach (var item in res.list)
                            {
                                Entities.WeixinOAuth.Article article = new Entities.WeixinOAuth.Article()
                                {
                                    MsgID = item.msgid,
                                    AppID = wxAppID,
                                    PubDate = DateTime.Parse(item.ref_date),
                                    ArticleType = Entities.Constants.Constant.ArticleTypeAdd + (item.msgid.Contains("_") ? int.Parse(item.msgid.Split('_')[1]) : 0),
                                    IntReadUserCount = item.details.Select(a => a.int_page_read_user).Max(),
                                    IntReadCount = item.details.Select(a => a.int_page_read_count).Max(),
                                    OriReadUserCount = item.details.Select(a => a.ori_page_read_user).Max(),
                                    OriReadCount = item.details.Select(a => a.ori_page_read_count).Max(),
                                    ShareUserCount = item.details.Select(a => a.share_user).Max(),
                                    ShareCount = item.details.Select(a => a.share_count).Max(),
                                    CreateTime = DateTime.Now
                                };
                                if (this.AddArticle(article) > 0)
                                {
                                    if (!multipleList.Exists(m => m.Equals(item.msgid.Split('_')[0])))
                                        multipleList.Add(item.msgid.Split('_')[0]);
                                    newList.Add(article);
                                    logger.Info("新增文章" + article.MsgID);
                                    rowcount++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex);
                    }
                    #endregion
                }
                else if (i <= 7)
                {
                    #region 微信统计7天累计 故更新7天及以内
                    try
                    {
                        var res = Senparc.Weixin.MP.AdvancedAPIs.AnalysisApi.GetArticleTotal(wxToken, pubDate.ToString("yyyy-MM-dd"), pubDate.ToString("yyyy-MM-dd"));
                        if (res.list != null && res.list.Count > 0)
                        {
                            foreach (var item in res.list)
                            {
                                Entities.WeixinOAuth.Article article = new Entities.WeixinOAuth.Article()
                                {
                                    MsgID = item.msgid,
                                    IntReadUserCount = item.details.Select(a => a.int_page_read_user).Max(),
                                    IntReadCount = item.details.Select(a => a.int_page_read_count).Max(),
                                    OriReadUserCount = item.details.Select(a => a.ori_page_read_user).Max(),
                                    OriReadCount = item.details.Select(a => a.ori_page_read_count).Max(),
                                    ShareUserCount = item.details.Select(a => a.share_user).Max(),
                                    ShareCount = item.details.Select(a => a.share_count).Max(),
                                };
                                if (this.UpdateArticle(article) > 0)
                                {
                                    if (!multipleList.Exists(m => m.Equals(item.msgid.Split('_')[0])))
                                        multipleList.Add(item.msgid.Split('_')[0]);
                                    newList.Add(article);
                                    logger.Info("更新文章" + article.MsgID);
                                    rowcount++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex);
                    }
                    #endregion
                }
            }
            #endregion

            #region 保存多图文统计信息
            this.ClearReadStatistic(wxID);
            //单图文
            Entities.WeixinOAuth.ReadStatistic_Weixin single = new Entities.WeixinOAuth.ReadStatistic_Weixin()
            {
                WxID = wxID,
                ArticleType = 6001,
                AverageReading = newList.Count == 0 ? 0 : (int)newList.Where(a => !a.MsgID.Contains("_") || a.MsgID.Split('_')[1].Equals(0)).Average(a => a.IntReadCount),
                MaxReading = newList.Count == 0 ? 0 : (int)newList.Where(a => !a.MsgID.Contains("_") || a.MsgID.Split('_')[1].Equals(0)).Max(a => a.IntReadCount),
                CreateTime = DateTime.Now
            };
            rowcount = this.AddReadStatistic(single);
            logger.Info("WxID:" + wxID + "增加单图文统计信息" + (rowcount > 0 ? "成功" : "失败"));
            //第一条
            Entities.WeixinOAuth.ReadStatistic_Weixin one = new Entities.WeixinOAuth.ReadStatistic_Weixin()
            {
                WxID = wxID,
                ArticleType = 6002,
                AverageReading = newList.Count == 0 ? 0 : (int)newList.Where(a => a.MsgID.Contains("_") && a.MsgID.Split('_')[1].Equals(1)).Average(a => a.IntReadCount),
                MaxReading = newList.Count == 0 ? 0 : (int)newList.Where(a => a.MsgID.Contains("_") && a.MsgID.Split('_')[1].Equals(1)).Max(a => a.IntReadCount),
                CreateTime = DateTime.Now
            };
            rowcount = this.AddReadStatistic(one);
            logger.Info("WxID:" + wxID + "增加多图文第一条统计信息" + (rowcount > 0 ? "成功" : "失败"));
            //第二条
            Entities.WeixinOAuth.ReadStatistic_Weixin two = new Entities.WeixinOAuth.ReadStatistic_Weixin()
            {
                WxID = wxID,
                ArticleType = 6003,
                AverageReading = newList.Count == 0 ? 0 : (int)newList.Where(a => a.MsgID.Contains("_") && a.MsgID.Split('_')[1].Equals(2)).Average(a => a.IntReadCount),
                MaxReading = newList.Count == 0 ? 0 : (int)newList.Where(a => a.MsgID.Contains("_") && a.MsgID.Split('_')[1].Equals(2)).Max(a => a.IntReadCount),
                CreateTime = DateTime.Now
            };
            rowcount = this.AddReadStatistic(two);
            logger.Info("WxID:" + wxID + "增加多图文第二条统计信息" + (rowcount > 0 ? "成功" : "失败"));
            //第三-N条
            Entities.WeixinOAuth.ReadStatistic_Weixin three = new Entities.WeixinOAuth.ReadStatistic_Weixin()
            {
                WxID = wxID,
                ArticleType = 6004,
                AverageReading = newList.Count == 0 ? 0 : (int)newList.Where(a => a.MsgID.Contains("_") && a.MsgID.Split('_')[1].Equals(3)).Average(a => a.IntReadCount),
                MaxReading = newList.Count == 0 ? 0 : (int)newList.Where(a => a.MsgID.Contains("_") && a.MsgID.Split('_')[1].Equals(3)).Max(a => a.IntReadCount),
                CreateTime = DateTime.Now
            };
            rowcount = this.AddReadStatistic(three);
            logger.Info("WxID:" + wxID + "增加多图文第3-N条统计信息" + (rowcount > 0 ? "成功" : "失败"));
            #endregion

            #region 互动参数补充
            iw.ReferReadCount = newList.Count == 0 ? 0 : (int)newList.Average(a => a.IntReadCount);//平均阅读量
            iw.MoreReadCount = newList.Count == 0 ? 0 : newList.Count(a => a.IntReadCount > 100000);//10W＋阅读文章数
            iw.MaxinumReading = newList.Count == 0 ? 0 : newList.Max(a => a.IntReadCount);//最高阅读量
            iw.UpdateCount = multipleList.Count == 0 ? 0 : multipleList.Count * 7 / 30;//周更新次数
            #endregion

            return rowcount;
        }

        /// <summary>
        /// 保存互动信息
        /// </summary>
        /// <param name="wxID">微信基表ID</param>
        /// <param name="wxNumber">微信号</param>
        /// <param name="hasRight">是否有群发通知权限</param>
        public void SaveInteraction(Entities.Interaction.InteractionWeixin iw, string wxNumber, bool hasRight)
        {
            int rowcount = 0;
            var statisticResult = this.GetDetailStatisticByWxNumber(wxNumber);
            if (statisticResult != null && statisticResult.Data != null && statisticResult.Data.ReadForWeb != null)
            {
                iw.AveragePointCount = statisticResult.Data.ReadForWeb.LikeAvgCount30Day;
                iw.OrigArticleCount = statisticResult.Data.ReadForWeb.Original;
                if (!hasRight)
                {
                    //有权限的话 平均阅读数、最高阅读数、周更新次数从微信接口获取 
                    iw.MaxinumReading = statisticResult.Data.ReadForWeb.ReadMaxCount30Day;
                    iw.ReferReadCount = statisticResult.Data.ReadForWeb.ReadAvgCount30Day;
                    iw.UpdateCount = statisticResult.Data.ReadForWeb.WeekUpdateCount30Day;
                    iw.MoreReadCount = statisticResult.Data.ReadForWeb.ReadCountGreaterThan10W;
                }
            }
            if (iw.RecID.Equals(0))
            {
                rowcount = BLL.Interaction.InteractionWeixin.Instance.Insert(iw);
            }
            else
            {
                rowcount = BLL.Interaction.InteractionWeixin.Instance.UpdateByWxID(iw);
            }
            logger.Info("WxID:" + iw.WxID + (iw.RecID.Equals(0) ? "新增" : "更新") + "互动参数" + (rowcount > 0 ? "成功" : "失败"));
        }

        public string InvokeAPIGetBiz(string wxAccessToken) {
            string res = string.Empty;
            try
            {
                var news = MediaApi.GetNewsMediaList(wxAccessToken, 0, 1);
                string str = JsonConvert.SerializeObject(news);
                Regex reg = new Regex(@"biz=\D{16}");
                Match match = reg.Match(str);
                if (match.Groups.Count > 0)
                {
                    if (match.Groups[0].Value.Length > 4)
                    {
                        string biz = match.Groups[0].Value.Substring(4);
                        if (!string.IsNullOrWhiteSpace(biz))
                        {
                            res = biz;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("调用微信接口获取Biz错误", ex);
            }
            return res;
        }

        public int InvokeAPIGetFansCount(string wxAccessToken) {

            int res = 0;
            try
            {
                var userResult = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Get(wxAccessToken, string.Empty);
                res = userResult.total;
            }
            catch (Exception ex)
            {
                logger.Error("调用微信接口获取粉丝数错误", ex);
            }
            return res;
        }

        #endregion

        /// <summary>
        /// 通过appid、secret、verifyticket获取第三方AccessToken
        /// </summary>
        /// <returns></returns>
        public static string GetComponentAcessToken()
        {
            string component_AccessToken = string.Empty;
            DateTime component_AccessTokenTime = new DateTime(1900, 1, 1);

            var component = BLL.WeixinOAuth.Instance.GetComponentDetail(component_AppId);
            if (component != null)
            {
                component_AccessToken = component.AccessToken;
                component_AccessTokenTime = component.AccessTokenTime;
            }
            if (!string.IsNullOrWhiteSpace(component_AccessToken) && component_AccessTokenTime >= DateTime.Now.AddHours(0).AddMinutes(-50))
            {
                return component_AccessToken;
            }

            while (string.IsNullOrEmpty(component.VerifyTicket))
            {
                Thread.Sleep(60 * 1000);
                component = BLL.WeixinOAuth.Instance.GetComponentDetail(component_AppId);
            }
            var accessTokenObj = ComponentApi.GetComponentAccessToken(component_AppId, component_Secret, component.VerifyTicket);
            component_AccessToken = accessTokenObj.component_access_token;
            component_AccessTokenTime = DateTime.Now;
            BLL.WeixinOAuth.Instance.UpdateAccessToken(component_AppId, component_AccessToken, component_AccessTokenTime);
            return component_AccessToken;
        }

    }
}
