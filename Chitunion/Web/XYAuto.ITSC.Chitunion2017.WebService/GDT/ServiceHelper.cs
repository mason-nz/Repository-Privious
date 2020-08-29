using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.WebService.Common;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Ads;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Fund;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.OAuth;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Report;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT
{
    public class ServiceHelper
    {
        #region Instance

        public static readonly ServiceHelper Instance = new ServiceHelper();
        private string GDT_URL = WebConfigurationManager.AppSettings["GDT_URL"];//广点通-接口url前缀
        private string GDT_GDT_AuthorizeUrl = WebConfigurationManager.AppSettings["GDT_AuthorizeUrl"];//广点通-Authorization域名
        private string GDT_API_VERSION = WebConfigurationManager.AppSettings["GDT_API_VERSION"];//广点通-接口版本
        private int GDT_Client_ID = WebConfigurationManager.AppSettings["GDT_Client_ID"] == null ? -1 : int.Parse(WebConfigurationManager.AppSettings["GDT_Client_ID"].ToString());//广点通-客户端ID

        //private string GDT_Access_Token = WebConfigurationManager.AppSettings["GDT_Access_Token"];//广点通-访问令牌
        private int GDT_Account_ID = WebConfigurationManager.AppSettings["GDT_Account_ID"] == null ? -1 : int.Parse(WebConfigurationManager.AppSettings["GDT_Account_ID"].ToString());//广点通-账户ID

        private string GDT_Client_Secret = WebConfigurationManager.AppSettings["GDT_Client_Secret"];//广点通-Client_Secret

        public string GDT_Access_Token
        {
            get
            {
                return CacheHelper<string>.Get(System.Web.HttpRuntime.Cache, () => $"gdt_access_token", GetGdtAccessToken, null, 30);
                //var testAccessToken = WebConfigurationManager.AppSettings["GDT_Access_Token"];//广点通-访问令牌
            }
        }

        #endregion Instance

        #region 测试专用（账号服务）

        public string AddAdvertiser_Test(string corporation_name, string website)
        {
            //string url = "advertiser/add";
            string url = GetCommonURL("/advertiser/add");
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("corporation_name", (corporation_name));
            paras.Add("certification_image_id", "1308:f35510517eefec24c188be6120e72de2");
            paras.Add("website", website);
            paras.Add("icp_image_id", "1308:8c15fd8541f9d943362eae5f88b1f22e");
            paras.Add("system_industry_id", "21474836686");

            HttpWebResponse hwr = Common.HttpHelper.CreatePostHttpResponse(GDT_URL + GDT_API_VERSION + url, paras, null, (int)RequestContentType.JSON);
            string msg = Common.HttpHelper.GetResponseString(hwr);
            return msg;
        }

        /// <summary>
        /// 添加图片信息
        /// </summary>
        /// <param name="accountId">推广帐号 id，有操作权限的帐号 id</param>
        /// <returns>返回Json字符串</returns>
        public string AddImagesInfo(int accountId)
        {
            string url = GetCommonURL("/images/add");
            Dictionary<string, string> paras = new Dictionary<string, string>();
            string imagePath = System.AppDomain.CurrentDomain.BaseDirectory + "Images/logo2.png";
            paras.Add("account_id", GDT_Account_ID.ToString());
            paras.Add("signature", Common.Util.GetMD5HashFromFile(imagePath));

            return Common.HttpHelper.CreatePostHttpResponseByMultipart(GDT_URL + GDT_API_VERSION + url, 10000, "file", imagePath, paras);
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="account_id">推广帐号 id，有操作权限的帐号 id</param>
        /// <returns>返回Json字符串</returns>
        public string GetImagesInfo(int account_id)
        {
            string url = GetCommonURL("/images/get");
            if (account_id > 0)
            {
                url += "&account_id=" + account_id;
            }
            HttpWebResponse hwr = Common.HttpHelper.CreateGetHttpResponse(GDT_URL + GDT_API_VERSION + url);
            string msg = Common.HttpHelper.GetResponseString(hwr);
            return msg;
        }

        #endregion 测试专用（账号服务）

        #region 测试专用（广告投放）

        /// <summary>
        /// 添加推广计划
        /// </summary>
        /// <param name="account_id">账号ID</param>
        /// <param name="campaign_name">推广计划名称</param>
        /// <param name="campaign_type">推广计划类型（枚举EnumCampaign_type）</param>
        /// <returns>返回json数据</returns>
        public string AddCampaigns_Test(int account_id, string campaign_name, string campaign_type)
        {
            string url = GetCommonURL("/campaigns/add");
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("account_id", account_id.ToString());
            paras.Add("campaign_name", campaign_name);
            paras.Add("campaign_type", campaign_type);
            if (campaign_type != "CAMPAIGN_TYPE_WECHAT_MOMENTS")
            {
                paras.Add("daily_budget", "10000");
            }

            if (campaign_type == "CAMPAIGN_TYPE_WECHAT_MOMENTS" ||
                campaign_type == "CAMPAIGN_TYPE_WECHAT_OFFICIAL_ACCOUNTS")
            {
                paras.Add("product_type", "PRODUCT_TYPE_LINK_WECHAT");
            }

            HttpWebResponse hwr = Common.HttpHelper.CreatePostHttpResponse(GDT_URL + GDT_API_VERSION + url, paras, null, (int)RequestContentType.JSON);
            return Common.HttpHelper.GetResponseString(hwr);
        }

        /// <summary>
        /// 添加广告组
        /// </summary>
        /// <param name="account_id">账号ID</param>
        /// <param name="campaign_id">推广计划ID</param>
        /// <param name="adgroup_name">广告组名称</param>
        /// <param name="site_set">投放站点集合，格式如：["site1","site1","site1"]</param>
        /// <param name="product_type">标的物类型</param>
        /// <param name="begin_date">开始投放日期</param>
        /// <param name="end_date">结束投放日期</param>
        /// <param name="bid_amount">广告出价，单位为分</param>
        /// <param name="optimization_goal">广告优化目标类型</param>
        /// <param name="billing_event">计费类型</param>
        /// <returns>返回json数据</returns>
        public string AddAdgroups_Test(int account_id, int campaign_id, string adgroup_name,
            string[] site_set, string product_type, string begin_date, string end_date,
            int bid_amount, string optimization_goal, string billing_event)
        {
            string url = GetCommonURL("/adgroups/add");
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("account_id", account_id.ToString());
            paras.Add("campaign_id", campaign_id.ToString());
            paras.Add("adgroup_name", adgroup_name);
            string site_set_result = string.Empty;
            foreach (string item in site_set)
            {
                site_set_result += "\"" + item + "\",";
            }
            if (!string.IsNullOrWhiteSpace(site_set_result))
            {
                site_set_result = "[" + site_set_result.TrimEnd(',') + "]";
            }
            paras.Add("site_set", site_set_result);
            paras.Add("product_type", product_type);
            paras.Add("begin_date", begin_date);
            paras.Add("end_date", end_date);
            paras.Add("bid_amount", bid_amount.ToString());
            paras.Add("optimization_goal", optimization_goal);
            paras.Add("billing_event", billing_event);
            paras.Add("targeting_id", "201663");

            HttpWebResponse hwr = Common.HttpHelper.CreatePostHttpResponse(GDT_URL + GDT_API_VERSION + url, paras, null, (int)RequestContentType.JSON);
            return Common.HttpHelper.GetResponseString(hwr);
        }

        /// <summary>
        /// 创建定向
        /// </summary>
        /// <param name="account_id">账号ID</param>
        /// <param name="targeting_name">定向名称</param>
        /// <returns>返回json数据</returns>
        public string AddTargetings_Test(int account_id, string targeting_name)
        {
            string url = GetCommonURL("/targetings/add");
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("account_id", account_id.ToString());
            paras.Add("targeting_name", targeting_name);

            HttpWebResponse hwr = Common.HttpHelper.CreatePostHttpResponse(GDT_URL + GDT_API_VERSION + url, paras, null, (int)RequestContentType.JSON);
            return Common.HttpHelper.GetResponseString(hwr);
        }

        #endregion 测试专用（广告投放）

        #region 帐号服务

        /// <summary>
        /// 获取广告主信息,如代理商不填写account_id，则获取代理商下全部子客户的信息
        /// </summary>
        /// <param name="reportDto">account_id</param>
        /// <returns></returns>
        public RespPageInfo<List<RespAccountInfoDto>> GetAdvertiserInfo(ReqReportDto reportDto)
        {
            var requestUrl = $"{GetCommonURL("/advertiser/get")}" +
                      $"&page={reportDto.Page}&page_size={reportDto.PageSize}";
            if (reportDto.AccountId > 0)
            {
                requestUrl += "&account_id=" + reportDto.AccountId;
            }
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;
            var result = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespAccountInfoDto>>>>
                (s =>
                {
                    var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                    return Common.HttpHelper.GetResponseString(hwr);
                }, Loger.GdtLogger.Info);

            return result.Data;
        }

        /// <summary>
        ///  获取资金账户信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<RespFundDto> GetFundsInfo(int accountId)
        {
            string url = GetCommonURL("/funds/get");
            if (accountId > 0)
            {
                url += "&account_id=" + accountId;
            }
            url = GDT_URL + GDT_API_VERSION + url;

            var result = new DoPostApiLogClient(url, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespFundDto>>>>
               (s =>
               {
                   var hwr = Common.HttpHelper.CreateGetHttpResponse(url);
                   return Common.HttpHelper.GetResponseString(hwr);
               }, Loger.GdtLogger.Info);

            return result.Data == null ? null : result.Data.List;
        }

        /// <summary>
        /// 获取今日消耗,只支持今天或昨天的数据查询
        /// </summary>
        /// <param name="reportDto">account_id,level,date</param>
        /// <returns></returns>
        public RespPageInfo<List<RespRealtimeCostDto>> GetRealtimeCost(ReqReportDto reportDto)
        {
            var requestUrl = $"{GetCommonURL("/realtime_cost/get")}&account_id={reportDto.AccountId}&level={reportDto.Level}" +
                             $"&date={reportDto.Date}&page={reportDto.Page}&page_size={reportDto.PageSize}";
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;

            var result = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespRealtimeCostDto>>>>
                (s =>
                {
                    var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                    return Common.HttpHelper.GetResponseString(hwr);
                }, Loger.GdtLogger.Info);

            return result.Data;
        }

        /// <summary>
        ///  获取资金账户日结明细
        /// </summary>
        /// <returns></returns>
        public List<FundStatementsDailyDto> GetFundStatementsDaily(ReqFundDto fundDto)
        {
            var requestUrl = $"{GetCommonURL("/fund_statements_daily/get")}&account_id={fundDto.AccountId}&fund_type={fundDto.FundType}" +
                             $"&date={fundDto.Date}";
            if (fundDto.TradeType != GdtTradeTypeEnum.None)
            {
                requestUrl += $"&trade_type={fundDto.TradeType}";
            }
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;

            var result = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<FundStatementsDailyDto>>>>
                (s =>
                {
                    var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                    return Common.HttpHelper.GetResponseString(hwr);
                }, Loger.GdtLogger.Info);

            return result.Data == null ? null : result.Data.List;
        }

        /// <summary>
        /// 获取资金账户流水
        /// </summary>
        /// <param name="fundDetaileDto"></param>
        public RespPageInfo<List<FundStatementsDailyDto>> GetFundStatementsDetailed(ReqFundDetaileDto fundDetaileDto)
        {
            var requestUrl = $"{GetCommonURL("/fund_statements_detailed/get")}&account_id={fundDetaileDto.AccountId}&fund_type={fundDetaileDto.FundType}" +
                         $"&date_range={JsonConvert.SerializeObject(fundDetaileDto.DateRange)}&page={fundDetaileDto.Page}&page_size={fundDetaileDto.PageSize}";

            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;
            var baseResult = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<FundStatementsDailyDto>>>>
                  (s =>
                  {
                      var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                      return Common.HttpHelper.GetResponseString(hwr);
                  }, Loger.GdtLogger.Info);

            return baseResult.Data;
        }

        #endregion 帐号服务

        #region 授权Authorization

        public string GetGdtAccessToken()
        {
            var info = Dal.GDT.GdtAccessToken.Instance.GetInfo((int)AuditRelationTypeEnum.Gdt, GDT_Client_ID);
            if (info == null) return string.Empty;
            return info.AccessToken;
        }

        public string GetToAuthorizeUrl(string redirectUri, string state = "", string scope = "")
        {
            var requestUrl = $"{GDT_GDT_AuthorizeUrl}?client_id={GDT_Client_ID}&redirect_uri={redirectUri}";
            if (!string.IsNullOrWhiteSpace(state))
                requestUrl += $"&state={state}";
            if (!string.IsNullOrWhiteSpace(scope))
                requestUrl += $"&scope={scope}";
            return requestUrl;
        }

        public string ToAuthorize(string redirectUri, string state = "", string scope = "")
        {
            var requestUrl = $"{GDT_GDT_AuthorizeUrl}?client_id={GDT_Client_ID}&redirect_uri={redirectUri}";
            if (!string.IsNullOrWhiteSpace(state))
                requestUrl += $"&state={state}";
            if (!string.IsNullOrWhiteSpace(scope))
                requestUrl += $"&scope={scope}";
            var result = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<object>
              (s =>
              {
                  var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                  return Common.HttpHelper.GetResponseString(hwr);
              }, Loger.GdtLogger.Info);

            return result.ToString();
        }

        public RespTokenDto GetAccessTokenByAuthorizationCode(string authorizationCode, string redirectUri)
        {
            return GetAccessToken(GdtGrantTypeEnum.authorization_code, authorizationCode, string.Empty, redirectUri);
        }

        public RespTokenDto GetAccessTokenByRefreshToken(string refreshToken)
        {
            return GetAccessToken(grantType: GdtGrantTypeEnum.refresh_token, authorizationCode: string.Empty, refreshToken: refreshToken, redirectUri: string.Empty);
        }

        /// <summary>
        /// 获取 Access Token 或刷新 Access Token
        /// </summary>
        /// <param name="grantType">请求的类型，可选值：authorization_code（授权码方式获取 token）、refresh_token（刷新 token）</param>
        /// <param name="authorizationCode">OAuth 认证 code，当 grant_type='authorization_code' 时必填</param>
        /// <param name="refreshToken">应用 refresh token，当 grant_type='refresh_token' 时必填</param>
        /// <param name="redirectUri">应用回调地址，当 grant_type='authorization_code'时，redirect_uri 为必传参数，仅支持 http 和 https，不支持指定端口号，且传入的地址需要与获取authorization_code时，传入的回调地址保持一致</param>
        /// <returns></returns>
        private RespTokenDto GetAccessToken(GdtGrantTypeEnum grantType, string authorizationCode, string refreshToken, string redirectUri)
        {
            string requestUrl;
            if (grantType == GdtGrantTypeEnum.authorization_code)
            {
                requestUrl = $"{GDT_URL}oauth/token?client_id={GDT_Client_ID}&client_secret={GDT_Client_Secret}&grant_type={grantType}" +
                                $"&authorization_code={authorizationCode}&redirect_uri={redirectUri}";
            }
            else
            {
                requestUrl = $"{GDT_URL}oauth/token?client_id={GDT_Client_ID}&client_secret={GDT_Client_Secret}&grant_type={grantType}" +
                                $"&refresh_token={refreshToken}";
            }

            var baseResult = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespTokenDto>>
               (s =>
               {
                   var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                   return Common.HttpHelper.GetResponseString(hwr);
               }, Loger.GdtLogger.Info);

            return baseResult.Data;
        }

        #endregion 授权Authorization

        #region 报表

        /// <summary>
        ///  获取日报表
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public RespPageInfo<List<RespReportDto>> GetReportDaily(ReqReportDto reportDto)
        {
            var requestUrl = $"{GetCommonURL("/daily_reports/get")}&level={reportDto.Level}&account_id={reportDto.AccountId}" +
                             $"&date_range={JsonConvert.SerializeObject(reportDto.DateRange)}&page={reportDto.Page}&page_size={reportDto.PageSize}";
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;
            var baseResult = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespReportDto>>>>
                  (s =>
                  {
                      var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                      return Common.HttpHelper.GetResponseString(hwr);
                  }, Loger.GdtLogger.Info);

            return baseResult.Data;
        }

        /// <summary>
        /// 获取小时报表
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        public RespPageInfo<List<RespReportDto>> GetReportHourly(ReqReportDto reportDto)
        {
            var requestUrl = $"{GetCommonURL("/hourly_reports/get")}&level={reportDto.Level}&account_id={reportDto.AccountId}" +
                           $"&date={reportDto.Date}&page={reportDto.Page}&page_size={reportDto.PageSize}";
            if (reportDto.GroupBy != null && reportDto.GroupBy.Length > 0)
            {
                requestUrl += $"&group_by={JsonConvert.SerializeObject(reportDto.GroupBy)}";
            }
            if (reportDto.Filtering != null && reportDto.Filtering.Count > 0)
            {
                requestUrl += $"&filtering={JsonConvert.SerializeObject(reportDto.Filtering)}";
            }
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;
            var baseResult = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespReportDto>>>>
                 (s =>
                 {
                     var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                     return Common.HttpHelper.GetResponseString(hwr);
                 }, Loger.GdtLogger.Info);

            return baseResult.Data;
        }

        #endregion 报表

        #region 广告相关

        /// <summary>
        /// 获取广告组列表
        /// </summary>
        /// <param name="reportDto">account_id,adgroup_id</param>
        public RespPageInfo<List<RespAdGroupDto>> GetAdGroupList(ReqReportDto reportDto)
        {
            var requestUrl = $"{GetCommonURL("/adgroups/get")}&account_id={reportDto.AccountId}" +
                         $"&page={reportDto.Page}&page_size={reportDto.PageSize}";
            if (reportDto.AdGroupId > 0)
                requestUrl += $"&adgroup_id={reportDto.AdGroupId}";
            if (reportDto.Filtering != null && reportDto.Filtering.Count > 0)
                requestUrl += $"&filtering={ JsonConvert.SerializeObject(reportDto.Filtering)}";
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;
            var baseResult = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespAdGroupDto>>>>
                 (s =>
                 {
                     var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                     return Common.HttpHelper.GetResponseString(hwr);
                 }, Loger.GdtLogger.Info);

            return baseResult.Data;
        }

        /// <summary>
        /// 获取推广计划列表
        /// </summary>
        /// <param name="reportDto">account_id，campaign_id</param>
        public RespPageInfo<List<RespAdCampaignDto>> GetAdCampaignsList(ReqReportDto reportDto)
        {
            var requestUrl = $"{GetCommonURL("/campaigns/get")}&account_id={reportDto.AccountId}" +
                           $"&page={reportDto.Page}&page_size={reportDto.PageSize}";
            if (reportDto.CampaignId > 0)
                requestUrl += $"&campaign_id={reportDto.CampaignId}";
            if (reportDto.Filtering != null && reportDto.Filtering.Count > 0)
            {
                requestUrl += $"&filtering={JsonConvert.SerializeObject(reportDto.Filtering)}";
            }
            requestUrl = GDT_URL + GDT_API_VERSION + requestUrl;
            var baseResult = new DoPostApiLogClient(requestUrl, string.Empty).GetPostResult<RespBaseDto<RespPageInfo<List<RespAdCampaignDto>>>>
                 (s =>
                 {
                     var hwr = Common.HttpHelper.CreateGetHttpResponse(requestUrl);
                     return Common.HttpHelper.GetResponseString(hwr);
                 }, Loger.GdtLogger.Info);

            return baseResult.Data;
        }

        #endregion 广告相关

        /// <summary>
        /// 根据接口中的URL，拼接调用接口通用参数
        /// </summary>
        /// <param name="url">接口调用url地址，如/funds/get</param>
        /// <returns>返回组装好的Get方式URL</returns>
        private string GetCommonURL(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                url += "?access_token=" + GDT_Access_Token;
                url += "&timestamp=" + Common.Util.ConvertDateTimeInt(DateTime.Now);
                url += "&nonce=" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            }
            return url;
        }
    }
}