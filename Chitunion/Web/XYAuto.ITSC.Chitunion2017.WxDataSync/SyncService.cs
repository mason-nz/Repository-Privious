using log4net;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.Open.ComponentAPIs;
using System;
using System.Configuration;
using System.Timers;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Interaction;
using XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth;

namespace XYAuto.ITSC.Chitunion2017.WxDataSync
{
    public class SyncService
    {
        private System.Timers.Timer timer = null;
        private readonly int serviceInterval = int.Parse(ConfigurationManager.AppSettings["ServiceInterval"]);
        private readonly ILog logger = LogManager.GetLogger(typeof(SyncService));

        public SyncService() {
            timer = new System.Timers.Timer(serviceInterval * 1000) { AutoReset = true };
            timer.Elapsed += (sender, eventArgs) => Run(sender, eventArgs);
        }

        public void Strat() {
            logger.Info("同步微信授权数据-----服务开始");
            timer.Start();
        }

        public void Stop() {
            logger.Info("同步微信授权数据-----服务结束");
            timer.Stop();
        }

        /// <summary>
        /// 获取有关联的微信列表 根据数据源类型
        /// ①扫码授权的从微信API获取 获取不到的用抓取数据
        /// ②验证码、手工的用抓取数据覆盖
        /// 
        /// </summary>
        public void Run(object obj,ElapsedEventArgs e) {

            if (e.SignalTime.Date.Hour != 00 || e.SignalTime.Minute != 30)
                return;
            var list = BLL.WeixinOAuth.Instance.GetWeixinList();
            logger.Info("共" + list.Count + "条数据待更新");
            if (list != null && list.Count > 0)
            {
                foreach (var wxInfo in list)
                {
                    try
                    {
                        var wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(wxInfo.WxNumber, wxInfo.Biz);
                        if (wxInfo.SourceType == (int)SourceTypeEnum.扫码授权)
                        {
                            #region 扫码授权
                            string wx_AccessToken = GetWxAccessToken(wxInfo);
                            if (string.IsNullOrWhiteSpace(wx_AccessToken))
                            {
                                logger.Info("WxID:" + wxInfo.RecID + "更新失败!没有AccessToken");
                                continue;
                            }
                            var rightList = BLL.WeixinOAuth.Instance.GetOAuthDetail(wxInfo.RecID);

                            #region 素材管理-获取Biz
                            if (rightList.Exists(r => r.OAuthID.Equals((int)WeixinAuthRightEnum.素材管理权限)))
                            {
                                string biz = BLL.WeixinOAuth.Instance.InvokeAPIGetBiz(wx_AccessToken);
                                if (string.IsNullOrWhiteSpace(biz))
                                    wxInfo.Biz = biz;
                            }
                            #endregion

                            #region 用户管理-获取粉丝数
                            if (rightList.Exists(r => r.OAuthID.Equals((int)WeixinAuthRightEnum.用户管理权限)))
                            {
                                int fanscount = BLL.WeixinOAuth.Instance.InvokeAPIGetFansCount(wx_AccessToken);
                                if (fanscount > wxInfo.FansCount)
                                    wxInfo.FansCount = fanscount;
                            }
                            #endregion

                            #region 补充信息
                            if (wxCatch != null)
                            {
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
                            }
                            #endregion

                            int rowcount = BLL.WeixinOAuth.Instance.UpdateWeixinInfo(wxInfo);
                            logger.Info("WxID:" + wxInfo.RecID + "更新基础信息" + (rowcount > 0 ? "成功" : "失败"));

                            #region 群发与通知-获取图文信息
                            var iw = BLL.Interaction.InteractionWeixin.Instance.GetEntityByWxID(wxInfo.RecID);
                            if (iw == null)
                            {
                                iw = new InteractionWeixin() { WxID = wxInfo.RecID, MeidaType = (int)MediaTypeEnum.微信, CreateTime = DateTime.Now, LastUpdateTime = DateTime.Now };
                            }
                            if (rightList.Exists(r => r.OAuthID.Equals((int)WeixinAuthRightEnum.群发与通知权限)))
                            {//有权限的话 平均阅读数、最高阅读数、周更新次数从微信接口获取 
                                BLL.WeixinOAuth.Instance.SaveArticleInfo(wxInfo.RecID, wxInfo.AppID, wx_AccessToken, iw);
                                BLL.WeixinOAuth.Instance.SaveInteraction(iw, wxInfo.WxNumber, true);
                            }
                            else
                            {
                                BLL.WeixinOAuth.Instance.SaveInteraction(iw, wxInfo.WxNumber, false);
                            }
                            #endregion

                            #endregion
                        }
                        else if (wxCatch != null)
                        {
                            BLL.WeixinOAuth.Instance.UpdateWxInfo(wxCatch, wxInfo);
                        }
                    }
                    catch (Exception ex) {
                        logger.Info("WxID:" + wxInfo.RecID + "更新失败!"+ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 获取授权公众号Token
        /// </summary>
        /// <returns></returns>
        private string GetWxAccessToken(WeixinInfo wx)
        {
            try
            {
                if (!string.IsNullOrEmpty(wx.AccessToken) && wx.GetTokenTime >= DateTime.Now.AddHours(0).AddMinutes(-50))
                    return wx.AccessToken;
                string cpAccessToken = BLL.WeixinOAuth.GetComponentAcessToken();
                var res = ComponentApi.ApiAuthorizerToken(cpAccessToken, BLL.WeixinOAuth.component_AppId, wx.AppID, wx.RefreshAccessToken);
                wx.GetTokenTime = DateTime.Now;
                wx.AccessToken = res.authorizer_access_token;
                wx.RefreshAccessToken = res.authorizer_refresh_token;
                BLL.WeixinOAuth.Instance.UpdateTokenInfo(wx.AppID, res.authorizer_access_token, res.authorizer_refresh_token, DateTime.Now);
                return res.authorizer_access_token;
            }
            catch (Exception ex) {
                logger.Info("WxID:" + wx.RecID + "获取公众号AccessToken异常：" + ex);
                return string.Empty;
            }
        }
    }
}
