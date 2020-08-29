/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 15:20:21
/// </summary>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Service.App.Task.Dto;
using XYAuto.ChiTu2018.Service.App.Task.Dto.GetOrderByStatus;
using XYAuto.ChiTu2018.Service.App.Task.Dto.GetOrderInfo;
using XYAuto.ChiTu2018.Service.App.Task.Dto.GetUserSceneByUserId;
using XYAuto.ChiTu2018.Service.App.Task.Dto.SubmitOrderUrl;
using XYAuto.CTUtils.Config;
using XYAuto.CTUtils.Log;
using ReqDto = XYAuto.ChiTu2018.Service.App.Task.Dto.GetDataByPage.ReqDto;
using ResDto = XYAuto.ChiTu2018.Service.App.Task.Dto.GetUnionAndUserId.ResDto;

namespace XYAuto.ChiTu2018.Service.App.Task
{
    public class LeTaskInfoService
    {
        #region 初始化
        private LeTaskInfoService() { }
        private static readonly Lazy<LeTaskInfoService> Linstance = new Lazy<LeTaskInfoService>(() => new LeTaskInfoService());
        public static LeTaskInfoService Instance => Linstance.Value;
        #endregion
        protected static string DominArticleShare = ConfigurationUtil.GetAppSettingValue("DominArticleShare", false);
        private static Random r = new Random();
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>
        public TaskResDto GetDataByPage(ReqDto reqDto)
        {
            var modelList = new List<TaskInfo>();
            foreach (var task in new LeTaskInfoBO().GetDataByPage(reqDto.PageIndex, reqDto.PageSize, reqDto.SceneID).ToList<LE_TaskInfo>())
            {
                var model = new TaskInfo()
                {
                    TaskId = task.RecID,
                    TaskName = task.TaskName,
                    MaterialUrl = task.MaterialUrl,
                    BillingRuleName = task.BillingRuleName,
                    Synopsis = task.Synopsis,
                    ImgUrl = task.ImgUrl,
                    TotalPrice = (decimal) task.TaskAmount,
                    IsForward = new LEADOrderInfoBO().GetListByTaskidUserId(task.RecID, reqDto.UserID).Count > 0 ? 1 : 0,
                    CPCPrice = (decimal) task.CPCPrice
                };

                if (model.MaterialUrl.Contains("ct_m"))
                {
                    model.MaterialUrl = ConfigurationUtil.GetAppSettingValue("DominArticle") + model.MaterialUrl.Substring(model.MaterialUrl.IndexOf("ct_m", StringComparison.Ordinal) - 1);
                }
                //获取订单
                var orderList = new LEADOrderInfoBO().GetListByUserID(task.RecID, reqDto.UserID);
                if (orderList.Count > 0)
                {
                    var query = from t in orderList
                                select t.RecID;

                    var sum = new LEAccountBalanceBO().GetListByTaskIDUserID(string.Join(",", query.ToArray())).Sum(x => x.TotalMoney);
                    if (sum != null)
                        model.TotalAmount = (decimal) sum;
                    model.IsForward = 1;
                }
                modelList.Add(model);
            }

            return new TaskResDto() {
                TaskInfo = modelList
            };
        }
        
        /// <summary>
        /// 根据ID获取分享信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public object GetShareOrderInfo(int orderId)
        {
            var model = new LEADOrderInfoBO().GetModelByRecId(orderId);
            return new Dto.GetShareOrderInfo.ResDto()
            {
                Price = model.TotalAmount.ToString(),
                Status = model.Status.ToString(),
                OrderUrl = model.OrderUrl
            };
        }
        
        #region 获取临时订单URL
        /// <summary>
        /// 获取临时订单URL
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public object GetOrderUrl(int materialId, out string errorMsg)
        {
            errorMsg = string.Empty;
            var currentUserId = -2;
            try
            {
                currentUserId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            }
            catch (Exception)
            {
                currentUserId = 1299;
            }
            var task = new LeTaskInfoBO().GetModelByMaterialId(materialId);
            if (task == null)
            {
                errorMsg += $"物料ID:{materialId}对应任务不存在";
                return null;
            }

            var orderurl = string.Empty;            
            if (currentUserId < 0)
                orderurl = task.MaterialUrl;
            else
            {
                var order = new LEADOrderInfoBO().GetModelByTaskidUserId(task.RecID, currentUserId);
                if (order == null)
                {
                    var code = XYAuto.ITSC.Chitunion2017.Common.Util.GenerateRandomCode(10);
                    orderurl = SetUrlParamsContent(task.MaterialUrl, code);
                }
                else
                {
                    orderurl = order.OrderUrl;
                }
            }
            if (orderurl.Contains("ct_m"))
            {
                var index = orderurl.IndexOf("ct_m", StringComparison.Ordinal);//XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue         
                orderurl = ConfigurationManager.AppSettings.Get("DominArticle") + orderurl.Substring(index - 1);
            }

            return new Dto.GetOrderUrl.ResDto()
            {
                TaskId = task.RecID,
                OrderUrl = orderurl,
                Synopsis = task.Synopsis,
                TaskName = task.TaskName,
                ImgUrl = task.ImgUrl
            };
        }
        private string SetUrlParamsContent(string url, string code)
        {
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
        #endregion

        #region 提交订单

        public object SubmitOrderUrl(Dto.SubmitOrderUrl.ReqDto reqDto,out string errorMsg)
        {
            errorMsg = string.Empty;
            var currentUserId = -2;
            try
            {
                currentUserId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            }
            catch (Exception)
            {
                currentUserId = 1299;
            }
            var ip = GetIp($"用户{currentUserId}分享订单");
            reqDto.ChannelId = Convert.ToInt32(ConfigurationUtil.GetAppSettingValue("WeiXinChannelId"));
            reqDto.UserId = currentUserId;

            //判断用户是否有领取任务(生成过订单)
            var order = new LEADOrderInfoBO().GetModelByTaskidUserId(reqDto.TaskId, currentUserId);
            if (order == null)
            {
                var isOk = SubmitOrder(reqDto.OrderUrl, reqDto.TaskId, reqDto.UserId, reqDto.ChannelId,
                    reqDto.PromotionChannelID, ip);

                if (isOk)
                    return "操作成功";
                else
                {
                    errorMsg = "提交订单失败";
                    return null;
                }
            }
            else
            {
                errorMsg = $"订单已经存在，订单号：{order.RecID}";
                Log4NetHelper.Default().Info(errorMsg);
                return null;
            }
        }
        public bool SubmitOrder(string orderUrl, int taskId, int userId, int channelId, string promotionChannelCode, string ip)
        {
            try
            {
                var postdate = $"ShareType={(int)LeShareDetailTypeEnum.物料}&TaskType={(int)LeTaskTypeEnum.ContentDistribute}&TaskId=" + taskId + "&UserId=" + userId + "&ChannelId=" + channelId + "&OrderUrl=" + HttpUtility.UrlEncode(orderUrl);
                postdate += "&IP=" + ip;
                var promotionId = new LePromotionChannelDictBO().GetChanneIdByChanneCode(promotionChannelCode);//XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetByChanneID(PromotionChannelID);
                postdate += "&PromotionChannelID=" + promotionId;
                Log4NetHelper.Default().Info("SubmitOrder postdate" + postdate);
                var geturl = PostWebRequest(ConfigurationManager.AppSettings.Get("GetOrderUrlByPost"), postdate);// PostWebRequest(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost"), postdate);
                Log4NetHelper.Default().Info("SubmitOrder geturl" + geturl);
                var url = JsonConvert.DeserializeObject<ResReceiveByWxDto>(geturl);
                Log4NetHelper.Default().Info("SubmitOrder url" + url.Status + url.Message);
                return url.Status == 0;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("SubmitOrder 出错：" + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        public string PostWebRequest(string postUrl, string paramData)
        {
            var ret = string.Empty;
            try
            {
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
                Log4NetHelper.Default().Info($"PostWebRequest:postUrl->{postUrl}paramData->{paramData} 发生异常", ex);
                return null;

            }
            return ret;
        }
        public string GetIp(string functionName)
        {
            try
            {
                //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.ToString().Split(',')[0].Trim();
                }

                if (string.IsNullOrEmpty(result))
                {
                    //否则直接读取REMOTE_ADDR获取客户端IP地址
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(result) &&
                    Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                {
                    //BLL.Loger.Log4Net.Info($"获取当前客户端ID：UserHostAddress={result}");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info($"{functionName}获取IP出错:", ex);
            }
            return "0.0.0.0";
        }
        #endregion
        
        public string GetDomainByRandom_ShareArticle(string orderurl)
        {
            string dominUrl = GetDomainByRandom();
            Log4NetHelper.Default().Info($"[GetDomainByRandom_ShareArticle]dominUrl:{dominUrl}");
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
            if (!string.IsNullOrEmpty(DominArticleShare))
            {
                string[] list = DominArticleShare.Split(',');
                return list[r.Next(0, list.Length)];
            }
            return string.Empty;
        }
    }
}
