using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class MediaUserBll
    {
        #region 单例
        private MediaUserBll() { }

        public static MediaUserBll instance = null;
        public static readonly object padlock = new object();
        public readonly string WeiXinUrl = ConfigurationManager.AppSettings["WXAuditApiUrl"] + string.Empty;

        public static MediaUserBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MediaUserBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 批量启用、禁用

        /// <summary>
        /// 批量启用、禁用
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int UserEnableOrDisable(UserBatchQueryArgs queryArgs)
        {
            return MediaUserDa.Instance.UserEnableOrDisable(queryArgs);
        }

        #endregion

        #region 获取广告主、媒体主列表

        /// <summary>
        /// 获取广告主、媒体主列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public BasicResultDto GetMediaUserList(UserQueryArgs queryArgs)
        {
            EnumInfo position = SplitHelper.GetEnumDescriptionList<MediaUserOrder>(queryArgs.ApproveStatus + string.Empty);

            var resultList = MediaUserDa.Instance.GetMediaUserList(queryArgs, position);
            return new BasicResultDto { List = resultList.Item2, TotalCount = resultList.Item1 };
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 根据层级获取推广渠道列表
        /// </summary>
        /// <param name="Level"></param>
        /// <returns></returns>
        public List<PromotionChannelModel> GetPromotionChannelList(int Level)
        {
            return MediaUserDa.Instance.GetPromotionChannelList(Level);
        }
        #endregion

        #region 批量更新密码

        /// <summary>
        /// 批量更新密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int UserResetPassword(UserBatchQueryArgs queryArgs)
        {
            string pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash("123.abc" + ((int)((UsereCategory)Enum.Parse(typeof(UsereCategory), queryArgs.ListType)) + string.Empty) + Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey"), System.Text.Encoding.UTF8);

            return MediaUserDa.Instance.UserResetPassword(queryArgs, pwd);
        }

        #endregion

        #region 用户详细信息

        /// <summary>
        /// 根据用户ID获取用户详细信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public MediaUserDetialModel GetMediaUserDetailInfo(int UserID)
        {
            return MediaUserDa.Instance.GetMediaUserDetailInfo(UserID);

        }

        #endregion

        #region 更新用户状态

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int UserCertificationAudit(UserBatchQueryArgs queryArgs)
        {
            return MediaUserDa.Instance.UserCertificationAudit(queryArgs, UserInfo.GetLoginUser().UserID);
        }
        /// <summary>
        /// 向公众号发送审核通知
        /// </summary>
        /// <param name="queryArgs"></param>
        public void WeiXinAudit(UserBatchQueryArgs queryArgs)
        {
            UserTokenInfo userInfo = MediaUserDa.Instance.GetUserToken(queryArgs.UserID);
            string ApiUrl = string.Empty;

            if (userInfo != null && !string.IsNullOrEmpty(userInfo.openId))
            {

                try
                {
                    if (queryArgs.Status == 2)
                    {
                        ApiUrl = WeiXinUrl + $"/api/WeChatToChiTu/NoticePass?" + $"openId={userInfo.openId}&name={userInfo.TrueName}&result=审核通过";
                    }
                    if (queryArgs.Status == 3)
                    {
                        ApiUrl = WeiXinUrl + $"/api/WeChatToChiTu/NoticeNoPass?" + $"openId={userInfo.openId}&name={userInfo.TrueName}&result=审核未通过&reason={userInfo.Reason}";
                    }

                    Loger.Log4Net.Info($"调用接口：{ApiUrl}");
                    using (var http = new HttpClient(new HttpClientHandler()))
                    {
                        //await异步等待回应
                        var response = http.GetAsync(ApiUrl);

                        if (response.Result.IsSuccessStatusCode)
                        {
                            string responseJson = response.Result.Content.ReadAsStringAsync().Result;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("发送审核通知 出错：", ex);
                }
            }
        }
        #endregion

    }
}
