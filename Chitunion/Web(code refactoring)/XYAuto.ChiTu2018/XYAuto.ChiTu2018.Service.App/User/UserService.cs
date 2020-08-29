/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User
* 类 名 称 ：UserService
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 19:12:47
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.Android;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Entities.Extend.User;
using XYAuto.ChiTu2018.Entities.User.Dto;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Infrastructure.MobileOperate;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.User;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Response.User;
using XYAuto.ChiTu2018.Service.App.User.Dto;
using XYAuto.ChiTu2018.Service.App.User.Dto.LoginForMobile;
using XYAuto.ChiTu2018.Service.App.User.Dto.LoginForWeChat;
using XYAuto.CTUtils.Config;
using XYAuto.CTUtils.Html;
using XYAuto.CTUtils.Log;
using XYAuto.CTUtils.Sys;

namespace XYAuto.ChiTu2018.Service.App.User
{
    public class UserService
    {
        private UserService() { }
        private static readonly Lazy<UserService> Linstance = new Lazy<UserService>(() => { return new UserService(); });

        public static UserService Instance => Linstance.Value;

        /// <summary>
        /// 获取手机号状态（0：不存在 1：该手机号码已注册，请更换手机号 2：该手机号已在pc端注册，是否做信息合并，-1：手机号格式不正确）
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetMobileStatus(string mobile, int userId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(mobile) || !VerifyHelper.IsHandset(mobile.Trim()))
            {
                dic.Add("MobileStatus", -1);
                return dic;
            }
            var userInfo = new UserInfoBO().GetUserInfoByMobile(mobile.Trim(), (int)UserInfoCategoryEnum.媒体主);
            if (userInfo == null)
            {
                dic.Add("MobileStatus", 0);
            }
            else
            {
                if (!string.IsNullOrEmpty(new UserInfoBO().GetMobileByUserId(userId)))
                {
                    dic.Add("MobileStatus", 1);
                }
                else
                {
                    dic.Add("MobileStatus", 2);
                }
            }
            return dic;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string AddUserInfo(ModifyAttestationReqDto modifyDto, int userId)
        {
            var oldUserId = 0;
            var validateQuery = ValidateUser(modifyDto, userId, out oldUserId);
            if (validateQuery != null && !validateQuery.Item1)
                return validateQuery.Item2;
            var dt = DateTime.Now;
            var userInfoAll = new UserInfoAll()
            {
                UserID = userId,
                Type = (int)UserTypeEnum.个人,
                TrueName = modifyDto.TrueName.Trim(),
                IdentityNo = modifyDto.IdentityNo.Trim(),
                Sex = modifyDto.Sex,
                AccountType = modifyDto.AccountType,
                AccountName = modifyDto.AccountName,
                Status = (int)UserDetailStatusEnum.已认证,
                Mobile = modifyDto.Mobile,
                CreateTime = dt,
                LastUpdateTime = dt,
                ApplyTime = dt,
                AuditTime = dt
            };
            if (oldUserId > 0)
                userInfoAll.UserID = oldUserId;
            UserInfoBO userInfoBo = new UserInfoBO();
            var errorMsg = userInfoBo.AddUserInfo(userInfoAll);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetHelper.Default().Error("AddUserInfo->保存用户信息错误：" + errorMsg);
                return "保存失败，请稍后重试！";
            }
            if (oldUserId <= 0) return "";
            try
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"合并用户：老用户{oldUserId };新用户{userId}");
                if (!userInfoBo.CleanUserInfoForM(oldUserId, userId)) return "保存失败，请稍后重试！";
                string queueName;
                XYAuto.ChiTu2018.Infrastructure.LeadsTask.LeadsTaskUtils.Instance.CreateQueue(false, out queueName);
                var mq = new MessageQueue(queueName)
                {
                    Formatter = new XmlMessageFormatter(new Type[] { typeof(AppUpdateUserIdDto) })
                };
                var updateUserId = new AppUpdateUserIdDto
                {
                    Source = 1,
                    OldUserId = oldUserId,
                    NewUserId = userId
                };
                mq.Send(updateUserId);
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(oldUserId);
                return "";
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"用户保存失败：", ex);
                return "保存失败，请稍后重试！";
            }
        }
        private Tuple<bool, string> ValidateUser(ModifyAttestationReqDto dto, int userId, out int oldUserId)
        {
            oldUserId = 0;

            if (dto == null || !Enum.IsDefined(typeof(UserBankAccountTypeEnum), dto.AccountType))
                return new Tuple<bool, string>(false, "参数错误");
            if (userId <= 0)
                return new Tuple<bool, string>(false, "请先登录");
            var userInfoBo = new UserInfoBO();
            var userInfoAll = userInfoBo.GetUserRelatedInfo(userId);
            if (userInfoAll == null)
                return new Tuple<bool, string>(false, "用户信息错误，请重新登录");
            if (string.IsNullOrEmpty(userInfoAll.Mobile))
            {
                if (string.IsNullOrWhiteSpace(dto.Mobile))
                    return new Tuple<bool, string>(false, "请填写手机号");
                if (!VerifyHelper.IsHandset(dto.Mobile.Trim()))
                    return new Tuple<bool, string>(false, "请填写正确的手机号");
                if ((string)CTUtils.Caching.HttpCacheHelper.GetCache(dto.Mobile.Trim()) != dto.CheckCode.ToString())
                    return new Tuple<bool, string>(false, "您输入的手机短信验证码不正确");
            }
            else
            {
                dto.Mobile = userInfoAll.Mobile;
            }
            if (!string.IsNullOrEmpty(userInfoAll.TrueName))
                dto.TrueName = userInfoAll.TrueName;
            if (userInfoAll.Sex >= 0)
                dto.Sex = userInfoAll.Sex;
            if (!string.IsNullOrEmpty(userInfoAll.IdentityNo))
                dto.IdentityNo = userInfoAll.IdentityNo;
            if (!string.IsNullOrEmpty(userInfoAll.AccountName))
            {
                dto.AccountName = userInfoAll.AccountName;
                dto.AccountType = userInfoAll.AccountType;
            }
            if (string.IsNullOrWhiteSpace(dto.TrueName))
                return new Tuple<bool, string>(false, "请填写姓名");
            if (dto.Sex != 0 && dto.Sex != 1)
                return new Tuple<bool, string>(false, "请选择性别");
            if (string.IsNullOrWhiteSpace(dto.IdentityNo))
                return new Tuple<bool, string>(false, "请输入身份证号码");
            if (string.IsNullOrWhiteSpace(dto.AccountName))
                return new Tuple<bool, string>(false, "请填写支付宝账号");
            if (!VerifyHelper.IsHandset(dto.Mobile.Trim()))
                return new Tuple<bool, string>(false, "手机号格式不正确");
            if (!VerifyHelper.CheckIDCard(dto.IdentityNo.Trim()))
                return new Tuple<bool, string>(false, "请输入正确的身份证号码");
            if (!VerifyHelper.IsHandset(dto.AccountName.Trim()) && !VerifyHelper.IsEmail(dto.AccountName.Trim()))
                return new Tuple<bool, string>(false, "支付宝账号格式不正确");

            var userInfo = userInfoBo.GetUserInfoByMobile(dto.Mobile.Trim(), (int)UserInfoCategoryEnum.媒体主, userId);
            var listUserId = new List<int>() { userId };
            if (!string.IsNullOrEmpty(userInfo?.Mobile))
            {
                var wxUser = new LEWeiXinUserBO().GetModelByUserId(userInfo.UserID);
                if (wxUser != null)
                    return new Tuple<bool, string>(false, "该手机号已注册，请更换手机号");
                else
                {
                    listUserId.Add(userInfo.UserID);
                    oldUserId = userInfo.UserID;
                }
            }
            if (userInfoBo.IsExistsIdentityNo(listUserId, (int)UserInfoCategoryEnum.媒体主, dto.IdentityNo.Trim()))
                return new Tuple<bool, string>(false, "您输入的身份证号已绑定其他账户");
            if (new LeUserBankAccountBO().IsExistsAccount(listUserId, (int)UserInfoCategoryEnum.媒体主, dto.AccountType, dto.AccountName.Trim()))
                return new Tuple<bool, string>(false, "该支付宝号已注册，请更换支付宝号");
            return null;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="isLogin">true 登录 false绑定</param>
        /// <returns></returns>
        public int SendValidateCode(AppReqMobileDto dto, bool isLogin)
        {
            if (string.IsNullOrEmpty(dto.Mobile) || !XYAuto.CTUtils.Sys.VerifyHelper.IsHandset(dto.Mobile))
            {
                return -12;
            }
            if (MobileUtils.GetMobileCheckCodeTimesByCache(dto.Mobile) > 2)
            {
                return -10;//上次发送过验证码给到这个手机号，还没有过超时时间
            }

            var code = MobileUtils.GetMobileCheckCodeByCache(dto.Mobile) ?? XYAuto.CTUtils.Sys.RandomHelper.GenerateRandomCode(4, GenerateRandomType.Num);
            var sendSmsContentTemp = CTUtils.Config.ConfigurationUtil.GetAppSettingValue(isLogin ? "SendSMS_LoginContentTemp" : "SendSMS_BindingContentTemp");
            var content = string.Format(sendSmsContentTemp, code);
            var smr = SMSUtils.Instance.SendMsgImmediately(dto.Mobile, content);
            if (Convert.ToBoolean(smr.Result))
            {
                MobileUtils.AddWebCacheByMobile(dto.Mobile, code);
                return 0;
            }
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"给{dto.Mobile}手机，发送短信内容：{content}，失败，具体接口返回内容为：{smr.Message}");
            return -11; //发送短信失败
        }
        /// <summary>
        /// 查询个人关联信息
        /// </summary>
        /// <returns></returns>
        public AppRespUseInfoDto GetUserRelatedInfo(int userId)
        {
            return new UserInfoBO().GetUserRelatedInfo(userId).MapTo<AppRespUseInfoDto>();
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="errormsg"></param>
        public RespLoginForWeChatDto LoginForWeChat(ReqLoginDto dto, ref string errormsg)
        {
            errormsg = string.Empty;
            var resp = new RespLoginForWeChatDto() { IsNewUser = "false", HomeUrl = ConfigurationUtil.GetAppSettingValue("Domin") + "/index.html?channel=dqappwdcticon" };

            if (dto == null)
            {
                errormsg = "请填写正确信息";
                return resp;
            }
            if (errormsg == null) throw new ArgumentNullException(nameof(errormsg));
            int sex = 0;
            int.TryParse(dto.sex, out sex);
            var retValue = Service.App.PublicService.PsWxUserAuthService.Instance.WeiXinUserOperation(new PsReqPostWxUserOperationDto()
            {
                unionid = dto.unionid,
                country = dto.country,
                openid = dto.openid,
                nickname = dto.nickname,
                city = dto.city,
                province = dto.province,
                language = dto.language,
                headimgurl = dto.headimgurl,
                sex = sex,
                LastUpdateTime = DateTime.Now,
                CreateTime = DateTime.Now,
                AuthorizeTime = DateTime.Now,
                subscribe_time = DateTime.Now,
                Source = GetWxSource(dto.from),//微信的来源
                RegisterIp = dto.Ip,
                RegisterFrom = dto.from,//(int)RegisterFromEnum.Android,
                RegisterType = (int)RegisterTypeEnum.微信,
                PromotionChannelId = 0
            });
            if (retValue.HasError)
            {
                errormsg = "登录失败，请稍后重试";
                return resp;
            }
            var respUserInfo = retValue.ReturnObject as PsRespUserOpeationDto;
            if (respUserInfo == null)
            {
                errormsg = "系统异常";
                return resp;
            }
            resp.CookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(respUserInfo.UserId);
            resp.IsNewUser = new AppDeviceBO().IsExist(respUserInfo.UserId) ? "false" : "true";

            return resp;
        }
        /// <summary>
        /// APP手机号登录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public RespLoginForWeChatDto LoginForAndroid(ReqMobileLoginDto dto, out string errorMsg)
        {
            errorMsg = string.Empty;
            const string channel = "dqappwdcticon";
            var resp = new RespLoginForWeChatDto() { IsNewUser = "false", HomeUrl = ConfigurationUtil.GetAppSettingValue("Domin") + "/index.html?channel=" + channel };
            if (dto == null || CTUtils.Sys.VerifyHelper.IsHandset(dto.mobile))
            {
                errorMsg = "请填写正确的手机号";
            }
            else if (string.IsNullOrEmpty(dto.mobileCheckCode))
            {
                errorMsg = "请填写验证码";
            }
            else if (MobileUtils.GetMobileCheckCodeByCache(dto.mobile) != dto.mobileCheckCode)
            {
                errorMsg = "您输入的手机短信验证码不正确";
            }
            else
            {
                var userInfo = new UserInfoBO().GetUserInfoByMobile(dto.mobile, (int)UserInfoCategoryEnum.媒体主);
                var content = string.Empty;
                var loginType = 0;
                var actionType = 0;
                if (userInfo != null)
                {
                    content = string.Format("用户{1}(ID:{0})登录成功。", userInfo.UserID, dto.mobile);
                    loginType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.登陆;
                    actionType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Select;
                }
                else
                {
                    userInfo = InsertUserMobile(dto, channel);
                    if (userInfo == null || userInfo.UserID <= 0)
                    {
                        errorMsg = "登录失败，请稍后重试";
                    }
                    else
                    {
                        content = string.Format("用户{1}(ID:{0})注册成功。", userInfo.UserID, dto.mobile);
                        loginType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册;
                        actionType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add;
                    }
                }
                if (!string.IsNullOrEmpty(errorMsg)) return resp;
                if (userInfo == null) return resp;
                XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(loginType, actionType, content, "", userInfo.UserID);
                resp.CookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userInfo.UserID, true);
            }
            return resp;
        }
        /// <summary>
        /// 添加手机号登录用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private UserInfo InsertUserMobile(ReqMobileLoginDto dto, string channel)
        {
            var promotionId = new LePromotionChannelDictBO().GetChanneIdByChanneCode(channel);
            var dt = DateTime.Now;
            var newUserInfo = new UserInfo()
            {
                UserName = dto.mobile,
                Mobile = dto.mobile,
                Pwd = string.Empty,
                Type = (int)UserTypeEnum.个人,
                Source = GetWxSource(dto.from),
                IsAuthMTZ = false,
                AuthAEUserID = 0,
                IsAuthAE = false,
                SysUserID = 0,
                EmployeeNumber = string.Empty,
                Status = 0,
                CreateTime = dt,
                LastUpdateTime = dt,
                CreateUserID = 0,
                LastUpdateUserID = 0,
                Category = (int)UserInfoCategoryEnum.媒体主,
                Email = string.Empty,
                LockState = 0,
                LockType = 0,
                SleepStatus = 0,
                RegisterType = (int)RegisterTypeEnum.手机号,
                PromotionChannelID = promotionId,
                CleanStatus = 0,
                HeadimgURL = string.Empty,
                ChannelUserID = string.Empty,
                RegisterIp = RequestHelper.GetIpAddress("127.0.0.1")
            };
            return new UserInfoBO().Insert(newUserInfo);
        }
        private int GetWxSource(int from)
        {
            if (from == (int)RegisterFromEnum.Android)
            {
                return (int)ReqMessageTypeEnum.Android;
            }
            else if (from == (int)RegisterFromEnum.Ios)
            {
                return (int)ReqMessageTypeEnum.Ios;
            }
            return 0;
        }
    }
}
