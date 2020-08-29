using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.Entities;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.QrLogin;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Dal.EmployeeInfo;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.WebService.Common;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{

    public class QrLoginProvider : VerifyOperateBase
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        /// <summary>
        /// 获取登录二维码
        /// </summary>
        /// <param name="reqPostLoginQrDto"></param>
        /// <returns></returns>
        public RespQrLoginDto GetLoginQr(ReqPostLoginQrDto reqPostLoginQrDto)
        {
            var respDto = new RespQrLoginDto();
            var valType = reqPostLoginQrDto.LoginType == LoginType.媒体主
                ? UserCategoryEnum.媒体主 : UserCategoryEnum.广告主;
            var code = ITSC.Chitunion2017.Common.Util.GenerateRandomCode(5);
            //先对称加密，然后前端轮询校验是否登录的时候直接获取，解密
            var sign = Encrypt(code);
            var reqKeyValue = new ReqKeyValueDto()
            {
                t = ReqMessageType.Pc端登录,
                v = $"{(int)valType}|{sign}"
            };

            var sceneStr = JsonConvert.SerializeObject(reqKeyValue);
            //设置二维码场景值（ReqMessageType.Pc端登录） 接收事件之后判断
            respDto.Ticket = sign;
            respDto.Url = CreateQrImage(sceneStr, reqKeyValue);

            return respDto;
        }

        /// <summary>
        /// 生成登录二维码（临时的，字符串）
        /// </summary>
        /// <param name="sceneStr"></param>
        /// <returns></returns>
        public string CreateQrImage(string sceneStr)
        {
            var accessToken = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetAccessToken(appId, appSecret);
            var postUrl = $"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accessToken}";
            var postData =
               "{\"expire_seconds\":604800,\"action_name\":\"QR_STR_SCENE\",\"action_info\":{\"scene\":{\"scene_str\":\"" + sceneStr + "\"}}}";
            var result = new DoHttpClient().PostByJson(postUrl, postData);
            var resultStr = result.Result;
            var qrCodeResult = JsonConvert.DeserializeObject<CreateQrCodeResult>(resultStr);

            return Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
        }

        /// <summary>
        /// 生成登录二维码（临时的，字符串）
        /// </summary>
        /// <param name="sceneStr"></param>
        /// <param name="reqKeyValueDto"></param>
        /// <returns></returns>
        public string CreateQrImage(string sceneStr, ReqKeyValueDto reqKeyValueDto)
        {
            var qrCodeResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(appId, GetWxLoginCacheMinutes * 60, (int)reqKeyValueDto.t,
               QrCode_ActionName.QR_STR_SCENE, sceneStr);
            Loger.Log4Net.Info($"CreateQrImage :{JsonConvert.SerializeObject(qrCodeResult)}");
            return Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
        }

        public ReqKeyValueDto VerifyScenceStr(string sceneStr)
        {
            try
            {
                Loger.Log4Net.Info($" 扫码事件推送 VerifyScenceStr 进行反序列化校验，场景sceneStr：{sceneStr}");
                return
            JsonConvert.DeserializeObject<ReqKeyValueDto>(sceneStr);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Info($" 扫码事件推送 VerifyScenceStr 反序列化失败，场景sceneStr：{sceneStr}");
                Loger.Log4Net.Info($" 扫码事件推送 VerifyScenceStr 反序列化失败：{exception.Message}");
                return null;
            }
        }

        /// <summary>
        /// 校验扫码之后是否登录成功
        /// </summary>
        /// <param name="reqVerifyQrLoginDto"></param>
        /// <returns></returns>
        public ReturnValue VerifyQrLogin(ReqVerifyQrLoginDto reqVerifyQrLoginDto)
        {
            var retValue = VerifyOfNecessaryParameters(reqVerifyQrLoginDto);
            if (retValue.HasError)
                return retValue;

            //判断cache 是否存在tiket的value

            //先对称加密，然后前端轮询校验是否登录的时候直接获取，解密 每次都得解密根据解密之后的key查询cache
            var ticketKey = DesEncrypt(reqVerifyQrLoginDto.Ticket);
            if (string.IsNullOrWhiteSpace(ticketKey))
            {
                return CreateFailMessage(retValue, "5005", "登录失败");
            }

            var chcheValue = BLL.Util.GetCodeByCache(ticketKey);
            if (string.IsNullOrWhiteSpace(chcheValue))
            {
                return CreateFailMessage(retValue, "5006", "登录失败");
            }

            //查找用户信息，是否是有效的用户
            var userId = chcheValue.Split(new string[] { "$@$" }, StringSplitOptions.None)[1];
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(userId.ToInt());
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "5007", $"用户信息不存在:{userId.ToInt()}");
            }
            if (userInfo.Status != 0)
            {
                return CreateFailMessage(retValue, "1", $"用户信息异常", new
                {
                    Ticket = ITSC.Chitunion2017.Common.Util.GenerateRandomCode(10)
                });
            }
            var ticket = chcheValue.Split(new string[] { "$@$" }, StringSplitOptions.None)[0];

            retValue.ReturnObject = new
            {
                Ticket = ticket
            };

            return retValue;
        }

        /// <summary>
        /// 扫码之后的回调逻辑
        /// </summary>
        /// <returns></returns>
        public ReturnValue SimulationLogin(ReqSimulationLoginDto reqSimulationLoginDto)
        {
            //todo
            //1.用户相关操作
            //2.模拟用户登录
            //3.写入cache 凭证（前端轮询校验是否已经登录）

            var retValue = new ReturnValue();
            if (string.IsNullOrWhiteSpace(reqSimulationLoginDto.WeixinOpendId) ||
                string.IsNullOrWhiteSpace(reqSimulationLoginDto.Ticket))
            {
                return CreateFailMessage(retValue, "5003", "请输入参数WeixinOpendId，Ticket");
            }
            var loginType = reqSimulationLoginDto.Ticket.Split('|')[0];
            var userInfo = InputUserInfo(reqSimulationLoginDto.WeixinOpendId, reqSimulationLoginDto.EventKey, loginType.ToInt());
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "5001", "SimulationLogin 用户操作相关信息失败，未获取到用户信息");
            }
            //模拟登录
            var userId = userInfo.UserID;
            if (loginType.ToInt() == (int)UserCategoryEnum.广告主)
            {
                userId = userInfo.AdvertiserUserId;
            }
            //添加角色
            InsertRole(userId, loginType.ToInt());
            //todo:loginCookie信息为空 因为用户可能被锁定，不能返回cookie信息
            var loginCookie = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId) ?? "loginCookie信息为空";
            Loger.Log4Net.Info($" SimulationLogin Passport ,loginType={loginType},userId={userId}返回cookie信息:{loginCookie}");

            //if (string.IsNullOrWhiteSpace(loginCookie))
            //{
            //    return CreateFailMessage(retValue, "5002", "Passport 用户模拟登录失败，未返回登录相关cookie");
            //}
            retValue.ReturnObject = loginCookie;
            //写入凭证(每次都得解密根据解密之后的key查询cache)
            var signValue = reqSimulationLoginDto.Ticket.Split('|');
            var signCode = DesEncrypt(signValue[1]);
            BLL.Util.InsertCache(signCode, $"{retValue.ReturnObject}$@${userId}$@${signCode}", GetWxLoginCacheMinutes);

            return retValue;
        }

        public string Encrypt(string value)
        {
            return XYAuto.Utils.Security.DESEncryptor.Encrypt(value, GetWxLoginEncryptKey);
        }

        public string DesEncrypt(string value)
        {
            try
            {
                return XYAuto.Utils.Security.DESEncryptor.Decrypt(value, GetWxLoginEncryptKey);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.Info($" QrLoginProvider DesEncrypt 失败:{exception.Message}{exception.StackTrace ?? string.Empty}");
                return string.Empty;
            }
        }

        #region 属性配置

        public string GetWxLoginEncryptKey
        {
            get { return ConfigurationUtil.GetAppSettingValue("WxLoginEncryptKey", true); }
        }
        public int GetWxLoginCacheMinutes
        {
            get { return ConfigurationUtil.GetAppSettingValue("WxLoginCacheMinutes", true).ToInt(1); }
        }
        public bool GetPushUserLogin
        {
            get { return Convert.ToBoolean(ConfigurationUtil.GetAppSettingValue("WxPushUserLogin", true)); }
        }

        #endregion

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="weixinOpendId"></param>
        /// <param name="eventKey"></param>
        /// <param name="loginType"></param>
        private ITSC.Chitunion2017.Entities.WeChat.WeiXinUser InputUserInfo(string weixinOpendId, string eventKey, int loginType)
        {
            ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();
            //通过扫描关注

            #region 扫码事件
            var openId = weixinOpendId;
            Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson userInfo;

#if DEBUG
            userInfo = new UserInfoJson() { openid = "oab7i0mmtJnZAFDR97sjy_F5K1Qg", tagid_list = new int[] { } };
#endif

            userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
            if (userInfo == null)
            {
                while (true)
                {
                    userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
                    if (userInfo != null)
                    {
                        break;
                    }
                }
            }
            if (ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.IsExistOpneId(userInfo.openid, (UserCategoryEnum)loginType))
            {

                Loger.Log4Net.Info("UpdateStatusByOpneId：" + userInfo.openid);
                ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(0, DateTime.Now, userInfo.openid);
                var user = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(userInfo.openid);
                var userId = user.UserID;
                if (loginType == (int)UserCategoryEnum.广告主)
                {
                    userId = user.AdvertiserUserId;
                }
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            }
            else
            {
                #region 插入微信用户
                try
                {
                    wxuser.subscribe = userInfo.subscribe;
                    wxuser.openid = userInfo.openid;
                    wxuser.nickname = userInfo.nickname;
                    wxuser.sex = userInfo.sex;
                    wxuser.city = userInfo.city;
                    wxuser.country = userInfo.country;
                    wxuser.province = userInfo.province;
                    wxuser.language = userInfo.language;
                    wxuser.headimgurl = userInfo.headimgurl;
                    wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfo.subscribe_time);
                    wxuser.unionid = userInfo.unionid;
                    wxuser.remark = userInfo.remark;
                    wxuser.groupid = userInfo.groupid;
                    wxuser.tagid_list = string.Join(",", userInfo.tagid_list);
                    wxuser.UserID = 0;
                    wxuser.CreateTime = DateTime.Now;
                    wxuser.LastUpdateTime = wxuser.CreateTime;
                    //wxuser.QRcode = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode(appId, userInfo.openid);
                    //wxuser.InvitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(wxuser.QRcode, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
                    wxuser.Status = userInfo.subscribe == 1 ? 0 : -1;
                    wxuser.RegisterFrom = (int)RegisterFromEnum.自助pc端;
                    wxuser.RegisterType = (int)RegisterTypeEnum.微信;
                    if (!string.IsNullOrEmpty(eventKey))
                    {
                        try
                        {
                            var qrJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ReqKeyValueDto>(eventKey.Replace("qrscene_", ""));
                            if (qrJson != null)
                            {
                                wxuser.Source = (int)qrJson.t;
                                wxuser.Inviter = qrJson.v;
                            }
                        }
                        catch (Exception ex)
                        {
                            wxuser.Inviter = eventKey.Replace("qrscene_", "");
                        }

                    }
                    bool insertUser = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(wxuser, (UserCategoryEnum)loginType);
                    if (!insertUser)
                    {
                        Loger.Log4Net.Error($" InputUserInfo WeiXinUserOperation 用户信息入库失败:{JsonConvert.SerializeObject(wxuser)}");
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error($"InputUserInfo 插入微信用户:{JsonConvert.SerializeObject(wxuser)}" +
                                        $" 错误：{ex.Message}{ex.StackTrace ?? String.Empty}");
                }
                #endregion
            }
            #endregion

            //因为里面的逻辑最好不要修改，最终查询一次用户信息就好，
            return ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(userInfo.openid);
        }

        public ReturnValue PushUserLogin(int userId)
        {
            var retValue = new ReturnValue();
            var requestUrl = ConfigurationManager.AppSettings["PushLoginApiUrl"];

            requestUrl += $"?action=simulationlogin&userId={userId}";

            Loger.Log4Net.Info($" PushUserLogin 请求参数:{requestUrl}");

            var jsonParams = new { action = "simulationlogin", userId = userId };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(jsonParams), Encoding.UTF8, "application/json");

            var response = new System.Net.Http.HttpClient().PostAsync(requestUrl, content);
            var resultStr = response.Result.Content.ReadAsStringAsync().Result;

            //var doHttpClient = new DoHttpClient(new System.Net.Http.HttpClient());
            //var result = new DoPostApiLogClient(requestUrl, string.Empty)
            //    .GetPostResult<RespBaseDto<dynamic>>(s => doHttpClient.Get(requestUrl).Result, Loger.Log4Net.Info);

            Loger.Log4Net.Info($" PushUserLogin 请求返回结果:{resultStr}");
            var result = JsonConvert.DeserializeObject<RespBaseDto<dynamic>>(resultStr);

            if (result == null)
            {
                return CreateFailMessage(retValue, "5050", $"PushUserLogin 失败 result is null 未知错误");
            }
            if (result.Status != 0)
            {
                return CreateFailMessage(retValue, "5051", $"PushUserLogin 失败:{JsonConvert.SerializeObject(result)}");
            }
            return retValue;
        }

        public void InsertRole(int userId, int loginType)
        {
            var roleIdInfo = ConfigurationManager.AppSettings["AddUserRoleIDs"];
            var chituSysId = ConfigurationManager.AppSettings["ChituSysID"];
            var list = new List<RoleInfoKeyValue>();
            roleIdInfo.Split('|').ToList().ForEach(s =>
            {
                var sp = s.Split(':');
                var info = new RoleInfoKeyValue
                {
                    CategoryId = sp[0].ToInt(),
                    RoleId = sp[1]
                };
                list.Add(info);
            });

            var roleId = list.Where(t => t.CategoryId == loginType).Select(t => t.RoleId).ToList().FirstOrDefault();
            EmployeeInfo.Instance.InsertUserDetailAndRoleInfo(userId, "", roleId, chituSysId, userId);
        }
    }

    internal class RoleInfoKeyValue
    {
        public int CategoryId { get; set; }

        public string RoleId { get; set; }
    }

    internal class RespBaseDto<T>
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }
}
