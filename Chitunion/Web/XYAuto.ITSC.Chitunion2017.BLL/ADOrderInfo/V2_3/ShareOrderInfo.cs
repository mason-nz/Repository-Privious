/********************************************************
*创建人：hant
*创建时间：2018/1/17 19:19:22 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat.Dto.VerifyUser;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.WeChat;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class ShareOrderInfo
    {
        public static readonly ShareOrderInfo Instance = new ShareOrderInfo();

        /// <summary>
        /// 获取订单分享信息
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public Entities.DTO.V2_3.ShareOrderInfo GetInfoByOrderId(int OrderID)
        {
            return Dal.ShareOrderInfo.Instance.GetInfoByOrderId(OrderID);
        }

        /// <summary>
        /// 判断用户是否领取任务
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsOrder(int TaskId, int UserId)
        {
            return Dal.ShareOrderInfo.Instance.IsOrder(TaskId, UserId);
        }

        /// <summary>
        /// 领取任务获取推广链接
        /// </summary>
        /// <param name="TaskId">任务ID</param>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public string GetOrderUrl(int TaskId, int UserId)
        {
            string orderUrl = string.Empty;
            DataSet ds = GetOrderByTaskIdAndUserId(TaskId, UserId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                orderUrl = ds.Tables[0].Rows[0]["OrderUrl"].ToString();
            }
            else
            {
                string MaterialUrl = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetMaterialUrlByTaskId(TaskId);
                var code = Common.Util.GenerateRandomCode(10);
                orderUrl = SetUrlParamsContent(MaterialUrl, code);
            }
            return orderUrl;
        }

        public DataSet GetOrderByTaskIdAndUserId(int TaskId, int UserId)
        {
            return Dal.ShareOrderInfo.Instance.GetOrderByTaskIdAndUserId(TaskId, UserId);
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="OrderUrl"></param>
        /// <param name="TaskId"></param>
        /// <param name="UserId"></param>
        /// <param name="ChannelId"></param>
        /// <returns></returns>

        public bool SubmitOrder(string OrderUrl, int TaskId, int UserId, int ChannelId, string PromotionChannelID, string ip)
        {
            try
            {
                string postdate = $"ShareType={(int)LeShareDetailTypeEnum.物料}&TaskType={(int)LeTaskTypeEnum.ContentDistribute}&TaskId=" + TaskId + "&UserId=" + UserId + "&ChannelId=" + ChannelId + "&OrderUrl=" + System.Web.HttpUtility.UrlEncode(OrderUrl);
                postdate += "&IP=" + ip;
                long promotionId = 0;
                if (HttpContext.Current.Request.Cookies["promotionName"] != null)
                {
                    string promotionName = HttpContext.Current.Request.Cookies["promotionName"].Value;
                    Loger.Log4Net.Info("SubmitOrder获取cookie值：" + promotionName);
                    if (!string.IsNullOrEmpty(promotionName))
                    {
                        promotionId = XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetByChanneID(promotionName);
                    }
                }
                else
                {
                    promotionId = XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetByChanneID(PromotionChannelID);
                    Loger.Log4Net.Info("SubmitOrder未获取到cookie值");
                }
                postdate += "&PromotionChannelID=" + promotionId;
                Loger.Log4Net.Info("SubmitOrder postdate" + postdate);
                string geturl = PostWebRequest(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost"), postdate);
                Loger.Log4Net.Info("SubmitOrder geturl" + geturl);
                Entities.WeChat.OrderUrl url = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.WeChat.OrderUrl>(geturl);
                Loger.Log4Net.Info("SubmitOrder url" + url.Status + url.Message);
                if (url.Status == 0)
                {
                    var inviteRedOrderCount = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("InviteRedOrderCount");
                    var orderCount = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetUserTotalOrderCount();
                    if (orderCount == Convert.ToInt32(inviteRedOrderCount))
                    {
                        Dal.WechatInvite.WechatInvite.Instance.UpdateRedEves(UserId);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("SubmitOrder" + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 生成订单并验证是否新用户订单
        /// </summary>
        /// <param name="OrderUrl"></param>
        /// <param name="TaskId"></param>
        /// <param name="UserId"></param>
        /// <param name="ChannelId"></param>
        /// <param name="PromotionChannelID"></param>
        /// <param name="ip"></param>
        public VerifyUserDto AddOrderVerify(int userId)
        {


           
            if (ActivityVerifyBll.Instance.IsNewUserByUserId(userId))
            {
                var configText = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("OneYuanTiXianActivity");
                var jObject = JObject.Parse(configText);
                int num = ActivityVerifyBll.Instance.GetUserOrderNum(new VerifyUserModel { UserID = userId });
                if (num == ConvertHelper.GetInteger(jObject["OrderNum"]))
                {
                    XYAuto.ITSC.Chitunion2017.BLL.Profit.Profit.Instance.AddProfit(userId, Entities.Enum.ProfitTypeEnum.新用户首次分享奖励, "新用户首次分享奖励", ConvertHelper.GetDecimal(jObject["Price"]), null, 1);
                }

                return new VerifyUserDto { IsNewUser = true, OrderNum = num };
            }

            return new VerifyUserDto();
        }

        private string SetUrlParamsContent(string url, string code)
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
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        public string PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                //if (!postUrl.StartsWith("http://"))
                //    return "";

                byte[] byteArray = Encoding.Default.GetBytes(paramData);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"PostWebRequest:postUrl->{postUrl}paramData->{paramData} 发生异常", ex);
                return null;

            }
            return ret;
        }

        public string CreateGetHttpResponse(string url, string data)
        {
            string geturl = url + "?url=" + data;
            HttpWebRequest request = WebRequest.Create(geturl) as HttpWebRequest;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string GetDomainByRandom_ShareArticle(string orderurl)
        {
            string dominUrl = XYAuto.ITSC.Chitunion2017.BLL.Util.GetDomainByRandom();
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"[GetDomainByRandom_ShareArticle]dominUrl:{dominUrl}");
            if (!string.IsNullOrEmpty(dominUrl) && orderurl.Contains("ct_m"))
                orderurl = $"http://{dominUrl}{orderurl.Substring(orderurl.IndexOf("ct_m", StringComparison.Ordinal) - 1)}";

            return orderurl;
        }
    }
}
