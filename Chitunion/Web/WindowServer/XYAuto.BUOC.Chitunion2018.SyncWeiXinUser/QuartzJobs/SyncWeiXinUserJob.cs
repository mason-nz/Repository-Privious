/********************************************************
*创建人：hant
*创建时间：2018/1/31 14:00:14 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Quartz;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.Chitunion2018.SyncWeiXinUser.Common;

namespace XYAuto.BUOC.Chitunion2018.SyncWeiXinUser.QuartzJobs
{
    public class SyncWeiXinUserJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Loger.InfoLog.Info("SyncWeiXinUserJob 开始");
                string nextOpenId = null;
                var stop = false;
                List<UserInfoJson> userInfoList = new List<UserInfoJson>();
                var accessToken = AccessTokenContainer.GetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"]);
                while (!stop)
                {
                    var result = UserApi.Get(accessToken, nextOpenId);
                    nextOpenId = result.next_openid;

                    try
                    {
                        foreach (var id in result.data.openid)
                        {
                            var userInfoResult = UserApi.Info(accessToken, id);
                            userInfoList.Add(userInfoResult);
                            ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();
                            if (!XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.IsExistOpneId(userInfoResult.openid))
                            {
                                wxuser.subscribe = userInfoResult.subscribe;
                                wxuser.openid = userInfoResult.openid;
                                wxuser.nickname = userInfoResult.nickname;
                                wxuser.sex = userInfoResult.sex;
                                wxuser.city = userInfoResult.city;
                                wxuser.country = userInfoResult.country;
                                wxuser.province = userInfoResult.province;
                                wxuser.language = userInfoResult.language;
                                wxuser.headimgurl = userInfoResult.headimgurl;
                                wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfoResult.subscribe_time);
                                wxuser.unionid = userInfoResult.unionid;
                                wxuser.remark = userInfoResult.remark;
                                wxuser.groupid = userInfoResult.groupid;
                                wxuser.tagid_list = string.Join(",", userInfoResult.tagid_list);
                                wxuser.UserID = 0;
                                wxuser.CreateTime = DateTime.Now;
                                wxuser.LastUpdateTime = wxuser.CreateTime;
                                //wxuser.QRcode = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode(ConfigurationManager.AppSettings["WeixinAppId"], userInfoResult.openid);
                                //wxuser.InvitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(wxuser.QRcode, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
                                wxuser.Status = 0;
                                ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.InsetWeiXinAndUserInfo(wxuser);
                            }
                            else
                            {
                                wxuser.city = userInfoResult.city;
                                wxuser.country = userInfoResult.country;
                                wxuser.groupid = userInfoResult.groupid;
                                wxuser.headimgurl = userInfoResult.headimgurl;
                                wxuser.language = userInfoResult.language;
                                wxuser.nickname = userInfoResult.nickname;
                                wxuser.province = userInfoResult.province;
                                wxuser.remark = userInfoResult.remark;
                                wxuser.sex = userInfoResult.sex;
                                wxuser.subscribe = userInfoResult.subscribe;
                                wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfoResult.subscribe_time);
                                wxuser.tagid_list = string.Join(",", userInfoResult.tagid_list);
                                wxuser.openid = userInfoResult.openid;
                                wxuser.LastUpdateTime = DateTime.Now;
                                ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateSync(wxuser);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.InfoLog.Error("SyncWeiXinUserJob 错误：" + ex.ToString());
                        continue;
                    }

                    if (nextOpenId == null)
                    {
                        stop = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.InfoLog.Error("SyncWeiXinUserJob 错误：" + ex.ToString());
            }
        }
    }
}
