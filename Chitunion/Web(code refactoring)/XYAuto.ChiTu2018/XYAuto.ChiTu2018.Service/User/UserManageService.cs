using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Entities.User.Dto;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Entities.Extend.User;
using XYAuto.ChiTu2018.Entities.ThirdModel;
using XYAuto.ChiTu2018.Infrastructure.MobileOperate;
using XYAuto.ChiTu2018.Service.ThirdService;
using XYAuto.ChiTu2018.Service.User.Dto;
using XYAuto.ChiTu2018.Service.Wechat;
using XYAuto.CTUtils.Html;
using XYAuto.CTUtils.Sys;
using XYAuto.ITSC.Chitunion2017.Common;
using UserInfo = XYAuto.ChiTu2018.Entities.Chitunion2017.User.UserInfo;

namespace XYAuto.ChiTu2018.Service.User
{
    /// <summary>
    /// 注释：UserManage
    /// 作者：zhanglb
    /// 日期：2018/5/15 11:15:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class UserManageService
    {
        private UserManageService() { }
        private static readonly Lazy<UserManageService> Linstance = new Lazy<UserManageService>(() => new UserManageService());

        public static UserManageService Instance => Linstance.Value;
        /// <summary>
        /// 获取手机号状态（0：不存在 1：该手机号码已注册，请更换手机号 2：该手机号已在pc端注册，是否做信息合并，-1：其他错误）
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="errorMsg">输出错误消息</param>
        /// <returns></returns>
        public Dictionary<string, object> GetMobileStatus(string mobile, out string errorMsg)
        {
            errorMsg = "";
            var dic = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(mobile))
            {
                dic.Add("MobileStatus", -1);
                errorMsg = "请填写手机号！";
                return dic;
            }
            if (!XYAuto.CTUtils.Sys.VerifyHelper.IsHandset(mobile.Trim()))
            {
                dic.Add("MobileStatus", -1);
                errorMsg = "请输入正确的手机号！";
                return dic;
            }
            var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            var userInfoBo = new UserInfoBO();
            var userInfo = userInfoBo.GetUserInfoByMobile(mobile.Trim(), (int)UserInfoCategoryEnum.媒体主);
            if (userInfo == null)
            {
                dic.Add("MobileStatus", 0);
            }
            else
            {
                if (!string.IsNullOrEmpty(userInfoBo.GetMobileByUserId(userId)))
                {
                    dic.Add("MobileStatus", 1);
                }
                else
                {
                    dic.Add("MobileStatus", new LEWeiXinUserBO().GetModel(t => t.UserID == userInfo.UserID) != null ? 1 : 2);
                }
            }
            return dic;
        }

        /// <summary>
        /// 根据手机号或用Id查询用户关联信息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="errorMsg">输出错误消息</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public object GetUserRelatedInfoByMobile(string mobile, out string errorMsg, int userId = 0)
        {
            errorMsg = string.Empty;


            var ret = new RespUserBasicInfoDto();
            UserInfo userInfo = null;
            if (userId > 0)
            {
                userInfo = new UserInfoBO().GetInfo(userId);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(mobile))
                {
                    errorMsg = "请填写手机号";
                    return null;
                }
                userInfo = new UserInfoBO().GetUserInfoByMobile(mobile.Trim(), (int)UserInfoCategoryEnum.媒体主);
            }
            if (userInfo == null) return ret;
            var userDetail = new UserDetailInfoBO().GetUserDetailByUserId(userInfo.UserID);
            var bankAccount = new LeUserBankAccountBO().GetBankAccountByUserId(userInfo.UserID);
            ret.BasicInfo = new RespMobileDto() { UserName = userInfo.UserName, Mobile = userInfo.Mobile };
            if (userDetail != null)
            {
                ret.AuthenticationInfo = new RespUserDetailDto()
                {
                    Type = (int)userInfo.Type,
                    Status = userDetail.Status,
                    TrueName = userDetail.TrueName,
                    IdentityNo = userDetail.IdentityNo,
                    IdCardFrontUrl = userDetail.IDCardFrontURL,
                    IdCardBackUrl = userDetail.IDCardBackURL,
                    BLicenceUrl = userDetail.BLicenceURL,
                    Reason = userDetail.Reason
                };
            }
            if (bankAccount != null)
            {
                ret.BankAccountInfo = new RespBankAccountDto()
                {
                    AccountType = (int)bankAccount.AccountType,
                    AccountName = bankAccount.AccountName
                };
            }
            return ret;
        }
        /// <summary>
        /// 查询个人关联信息
        /// </summary>
        /// <returns></returns>
        public RespUserBasicDto GetUserRelatedInfo()
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            return new UserInfoBO().GetUserRelatedInfo(userId);
        }
        /// <summary>
        /// 查询个人认证信息
        /// </summary>
        /// <returns></returns>
        public RespUserDetailDto GetUserDetail()
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            return new UserDetailInfoBO().GetUserDetail(userId);
        }
        /// <summary>
        /// 验证认证信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string VerifUserDetail(RespUserDetailDto dto, ref int userId)
        {
            if (dto == null)
                return "参数错误";
            if (!Enum.IsDefined(typeof(UserTypeEnum), dto.Type))
                return "参数错误";

            userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (dto.Type == (int)UserTypeEnum.个人)
            {
                if (string.IsNullOrWhiteSpace(dto.TrueName))
                    return "真实姓名不能为空";
                if (string.IsNullOrWhiteSpace(dto.IdentityNo))
                    return "身份证号码不能为空";
                if (!XYAuto.CTUtils.Sys.VerifyHelper.CheckIDCard(dto.IdentityNo.Trim()))
                    return "请输入正确的身份证号码";
                if (new UserDetailInfoBO().IsExistsIdentityNo(userId, (int)UserInfoCategoryEnum.媒体主, dto.IdentityNo.Trim()))
                    return "您输入的身份证号已绑定其他账户";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dto.TrueName))
                    return "公司名称不能为空";
                if (string.IsNullOrWhiteSpace(dto.BLicenceUrl))
                    return "营业执照不能为空";
            }
            return "";
        }
        /// <summary>
        /// 保存认证信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string SaveUserDetail(RespUserDetailDto dto)
        {
            var userId = 0;
            var errorMsg = VerifUserDetail(dto, ref userId);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return errorMsg;
            }
            var userInfoBo = new UserInfoBO();
            var userInfo = userInfoBo.GetInfo(userId);
            if (string.IsNullOrEmpty(userInfo?.Mobile))
            {
                return "请先填写手机号";
            };
            if (userInfo.Type != dto.Type)
            {
                userInfo.Type = dto.Type;
                if (userInfoBo.Update(userInfo) == null)
                {
                    return "保存失败，请重试！";
                }
            }

            bool result;
            var userDetailInfoBo = new UserDetailInfoBO();
            var userDetailInfo = userDetailInfoBo.GetUserDetailByUserId(userId);


            if ((int)UserTypeEnum.企业 == dto.Type)
            {
                //清空个人信息
                dto.IdentityNo = string.Empty;
                dto.IdCardFrontUrl = string.Empty;
            }
            else
            {
                dto.BLicenceUrl = string.Empty;
            }
            if (userDetailInfo != null)
            {
                userDetailInfo.TrueName = dto.TrueName.Trim();
                userDetailInfo.BLicenceURL = dto.BLicenceUrl.Trim();
                userDetailInfo.IdentityNo = dto.IdentityNo.Trim();
                userDetailInfo.IDCardFrontURL = dto.IdCardFrontUrl.Trim();
                userDetailInfo.Status = (int)UserTypeEnum.企业 == dto.Type ? 1 : 2;
                result = userDetailInfoBo.UpdateUserDetail(userDetailInfo) != null;
            }
            else
            {
                userDetailInfo = new UserDetailInfo
                {

                    UserID = userId,
                    TrueName = dto.TrueName.Trim(),
                    BusinessID = -2,
                    ProvinceID = -2,
                    CityID = -2,
                    CounntyID = -2,
                    Contact = string.Empty,
                    Address = string.Empty,
                    BLicenceURL = dto.BLicenceUrl.Trim(),
                    IDCardFrontURL = dto.IdCardFrontUrl.Trim(),
                    IDCardBackURL = string.Empty,
                    CreateTime = DateTime.Now,
                    CreateUserID = -2,
                    LastUpdateTime = null,
                    LastUpdateUserID = -2,
                    OrganizationURL = string.Empty,
                    Status = (int)UserTypeEnum.企业 == dto.Type ? 1 : 2,
                    IdentityNo = dto.IdentityNo.Trim(),
                    Reason = string.Empty,
                    ApplyTime = DateTime.Now,
                    AuditTime = ((int)UserTypeEnum.企业 == dto.Type ? (DateTime?)null : DateTime.Now),
                    AuditUserID = -2,
                    Sex = -2
                };
                result = userDetailInfoBo.AddUserDetail(userDetailInfo) != null;
            }
            return !result ? "保存失败，请重试！" : "";
        }
        /// <summary>
        /// 保存用户手机号
        /// </summary>
        /// <param name="moblie">手机号</param>
        /// <param name="checkCode">验证码</param>
        /// <returns></returns>
        private int SaveMobile(string moblie, int checkCode)
        {
            if (ValidateMobileCode(moblie.Trim(), checkCode) != 0)
            {
                return -13;
            }
            var userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            var userInfoBo = new UserInfoBO();
            var userInfo = userInfoBo.GetUserInfoByMobile(moblie.Trim(), (int)(int)UserInfoCategoryEnum.媒体主);
            if (userInfo != null)
            {
                if (userInfo.UserID == userId)
                {
                    return -14;
                }
                else
                {

                    if (new LEWeiXinUserBO().GetModelByUserId(userInfo.UserID) == null)
                    {
                        try
                        {
                            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"合并用户：老用户{userInfo.UserID };新用户{userId}");
                            if (userInfoBo.CleanUserInfoForM(userInfo.UserID, userId))
                            {
                                string queueName;
                                XYAuto.ChiTu2018.Infrastructure.LeadsTask.LeadsTaskUtils.Instance.CreateQueue(false, out queueName);
                                var mq = new MessageQueue(queueName)
                                {
                                    Formatter = new XmlMessageFormatter(new Type[] { typeof(UpdateUserId) })
                                };
                                var updateUserId = new UpdateUserId
                                {
                                    Source = 1,
                                    OldUserId = userInfo.UserID,
                                    NewUserId = userId
                                };
                                mq.Send(updateUserId);
                                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userInfo.UserID);
                                return 0;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        catch (Exception ex)
                        {
                            XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"用户保存失败：", ex);
                            return -1;
                        }
                    }
                    else
                    {
                        return -15;
                    }
                }
            }
            else
            {
                var u = userInfoBo.GetInfo(userId);
                u.Mobile = moblie.Trim();
                var updateUser = userInfoBo.Update(u);
                if (updateUser == null)
                {
                    return -1;
                }
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
                return 0;
            }
        }
        /// <summary>
        /// 发送手机号验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="sendCodeType">发送类型（208001 绑定 ；208002 修改； 208003 提现）</param>
        /// <returns></returns>
        private int SendMobileCode(string mobile, MobileOperateEnum sendCodeType)
        {
            if (MobileUtils.GetMobileCheckCodeTimesByCache(mobile) > 2)
            {
                return -10;//上次发送过验证码给到这个手机号，还没有过超时时间
            }
            string sendSmsKey = "SendSMS_RegisterContentTemp";
            switch (sendCodeType)
            {
                case MobileOperateEnum.绑定发送验证码:
                    sendSmsKey = "SendSMS_BindingContentTemp";
                    break;
                case MobileOperateEnum.修改发送验证码:
                    sendSmsKey = "SendSMS_ModifyMobileContentTemp";
                    break;
                case MobileOperateEnum.提现发送验证码:
                    sendSmsKey = "SendSMS_WithdrawContentTemp";
                    break;
            }
            string code = MobileUtils.GetMobileCheckCodeByCache(mobile) ?? XYAuto.CTUtils.Sys.RandomHelper.GenerateRandomCode(4, GenerateRandomType.Num);
            string sendSmsContentTemp = CTUtils.Config.ConfigurationUtil.GetAppSettingValue(sendSmsKey);
            string content = string.Format(sendSmsContentTemp, code);
            var smr = SMSUtils.Instance.SendMsgImmediately(mobile, content);
            if (Convert.ToBoolean(smr.Result))
            {
                MobileUtils.AddWebCacheByMobile(mobile, code);
                return 0;
            }
            else
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"给{mobile}手机，发送短信内容：{content}，失败，具体接口返回内容为：{smr.Message}");
                return -11; //发送短信失败
            }
        }
        /// <summary>
        /// 验证手机短信验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="mobileCode">验证码</param>
        /// <returns></returns>
        private int ValidateMobileCode(string mobile, int mobileCode)
        {
            if (MobileUtils.GetMobileCheckCodeByCache(mobile) != mobileCode.ToString())
                return -13;
            return 0;
        }
        /// <summary>
        /// 发送或校验短信验证码||保存手机号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int SmsValidateCode(ReqMobileInfoDto dto)
        {
            if (dto == null || !Enum.IsDefined(typeof(MobileOperateEnum), dto.SmsAction))
            {
                return -16;
            }
            if (string.IsNullOrEmpty(dto.Mobile) || !VerifyHelper.IsHandset(dto.Mobile))
            {
                return -12;
            }
            MobileOperateEnum mobileOperateEnum = (MobileOperateEnum)dto.SmsAction;
            if (mobileOperateEnum == MobileOperateEnum.提现发送验证码 || mobileOperateEnum == MobileOperateEnum.修改发送验证码 || mobileOperateEnum == MobileOperateEnum.绑定发送验证码)
            {
                return SendMobileCode(dto.Mobile, mobileOperateEnum);
            }
            else if (mobileOperateEnum == MobileOperateEnum.修改验证验证码)
            {
                return ValidateMobileCode(dto.Mobile, dto.CheckCode);
            }
            else
            {
                return SaveMobile(dto.Mobile, dto.CheckCode);
            }
        }
        /// <summary>
        /// 网页端手机号登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string MobileLogin(ReqMobileInfoDto dto)
        {
            if (string.IsNullOrEmpty(dto?.Mobile) || !XYAuto.CTUtils.Sys.VerifyHelper.IsHandset(dto.Mobile))
            {
                return "请填写正确的手机号";
            }
            if (ValidateMobileCode(dto.Mobile, dto.CheckCode) != 0)
            {
                return "您输入的手机短信验证码不正确";
            }
            var userInfo = new UserInfoBO().GetUserInfoByMobile(dto.Mobile, (int)UserInfoCategoryEnum.媒体主);
            string content;
            int loginType;
            int actionType;
            if (userInfo != null)
            {
                content = string.Format("用户{1}(ID:{0})登录成功。", userInfo.UserID, dto.Mobile);
                loginType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.登陆;
                actionType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Select;
            }
            else
            {
                userInfo = InsertUserMobile(dto.Mobile);
                if (userInfo == null || userInfo.UserID <= 0)
                    return "登录失败，请稍后重试";
                content = string.Format("用户{1}(ID:{0})注册成功。", userInfo.UserID, dto.Mobile);
                loginType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册;
                actionType = (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add;
            }
            XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(loginType, actionType, content, "", userInfo.UserID);
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userInfo.UserID, true);
            return string.Empty;
        }
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static object GetCookieValue(string keyName)
        {
            var value = "";
            if (HttpContext.Current.Request.Cookies[keyName] == null) return value;
            value = HttpContext.Current.Request.Cookies[keyName].Value;
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"获取cookie的Key：{keyName}值：" + value);
            return value;
        }
        /// <summary>
        /// 插入手机号登录用户
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private UserInfo InsertUserMobile(string mobile)
        {
            var cookieVlaue = GetCookieValue("promotionName").ToString();
            long promotionId = 0;
            if (!string.IsNullOrEmpty(cookieVlaue))
            {
                promotionId = new LePromotionChannelDictBO().GetChanneIdByChanneCode(cookieVlaue);
            }
            var dt = DateTime.Now;
            var newUserInfo = new UserInfo()
            {
                UserName = mobile,
                Mobile = mobile,
                Pwd = string.Empty,
                Type = (int)UserTypeEnum.个人,
                Source = (int)RegisterFromEnum.H5,
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

        /// <summary>
        /// 汽车大全端登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserInfo InsertUserInfoByToken(string token, CarUserInfo carUser = null)
        {
            try
            {
                if (carUser == null)
                {
                    carUser = CarUserService.Instance.GetUserInfoByToken(token);
                }
                if (carUser != null)
                {
                    var userInfoBo = new UserInfoBO();
                    var userInfo = userInfoBo.GetUserInfoByChannelUid(carUser.User_token, (int)UserInfoCategoryEnum.媒体主);
                    if (userInfo != null)
                    {
                        var user = new  CarToChiTuUser();
                        {
                            //ChannelUserId
                            //ChannelUserId = carUser.User_token,
                            //UserId = userInfo.UserID,
                            //UserName = carUser.Nick_name,
                            //HeadimgUrl = carUser.User_avatar

                        };
                        switch (carUser.User_gender)
                        {
                            case 1:
                                user.Sex = 1;
                                break;
                            case 2:
                                user.Sex = 0;
                                break;
                        }
                        userInfoBo.UpdateCarUser(user);
                        var userDetail = new UserDetailInfoBO().GetUserDetailByUserId(userInfo.UserID);
                        if (userDetail == null)
                        {
                            var userDetailInfo = new UserDetailInfo()
                            {
                                UserID = userInfo.UserID,
                                TrueName = string.Empty,
                                BusinessID = -2,
                                ProvinceID = -2,
                                CityID = -2,
                                CounntyID = -2,
                                Contact = string.Empty,
                                Address = string.Empty,
                                BLicenceURL = string.Empty,
                                IDCardFrontURL = string.Empty,
                                IDCardBackURL = string.Empty,
                                CreateTime = DateTime.Now,
                                CreateUserID = -2,
                                LastUpdateTime = null,
                                LastUpdateUserID = -2,
                                OrganizationURL = string.Empty,
                                Status = (int)UserTypeEnum.个人,
                                IdentityNo = string.Empty,
                                Reason = string.Empty,
                                ApplyTime = DateTime.Now,
                                AuditTime = DateTime.Now,
                                AuditUserID = -2,
                                Sex = user.Sex
                            };
                            new UserDetailInfoBO().AddUserDetail(userDetailInfo);
                        }

                        CTUtils.Log.Log4NetHelper.Default().Info($"汽车大全用户{token}已存在，更新成功");
                    }
                    else
                    {
                        userInfo = InsertCarUser(carUser);
                        CTUtils.Log.Log4NetHelper.Default()
                            .Info(userInfo != null ? $"汽车大全用户{token}不存在，新增成功" : $"汽车大全用户{token}不存在，新增失败");
                    }
                    if (userInfo != null)
                    {
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userInfo.UserID, true);
                        return userInfo;
                    }
                    CTUtils.Log.Log4NetHelper.Default().Info($"汽车大全用户登录失败{token}");
                    return null;
                }
                CTUtils.Log.Log4NetHelper.Default().Info($"汽车大全用户{token}未找到");
                return null;
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"汽车大全用户添加失败", ex);
                return null;
            }
        }
        private static UserInfo InsertCarUser(CarUserInfo carUser)
        {
            var cookieVlaue = GetCookieValue("promotionName").ToString();
            long promotionId = 0;
            if (!string.IsNullOrEmpty(cookieVlaue))
            {
                promotionId = new LePromotionChannelDictBO().GetChanneIdByChanneCode(cookieVlaue);
            }
            var dt = DateTime.Now;
            var newUserInfo = new UserInfo()
            {
                UserName = carUser.Nick_name,
                Mobile = string.Empty,
                Pwd = string.Empty,
                Type = (int)UserTypeEnum.个人,
                Source = (int)RegisterFromEnum.H5,
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
                HeadimgURL = carUser.User_avatar,
                ChannelUserID = carUser.User_token,
                RegisterIp = RequestHelper.GetIpAddress("127.0.0.1")
            };
            return new UserInfoBO().Insert(newUserInfo);
        }
        /// <summary>
        /// 查询汽车大全端登录信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByToken(string token)
        {
            CTUtils.Log.Log4NetHelper.Default().Info("查询汽车大全用户信息" + token);
            var carUser = CarUserService.Instance.GetUserInfoByToken(token);
            var chituUser = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
            if (carUser != null)
            {
                var isSuccess = false;
                var userid = -1;
                var mobile = "";
                var userName = "";
                if (chituUser != null && chituUser.UserID > 0)
                {
                    userid = chituUser.UserID;
                    mobile = chituUser.Mobile;
                    userName = chituUser.UserName;
                }
                else
                {
                    var u = InsertUserInfoByToken(token, carUser);
                    if (u != null && u.UserID > 0)
                    {
                        userid = u.UserID;
                        mobile = u.Mobile;
                        userName = u.UserName;
                        isSuccess = true;
                    }
                }
                if (!isSuccess) return null;
                if (chituUser != null)
                {
                    mobile = chituUser.Mobile;
                }
                var lu = new UserInfo
                {
                    UserID = userid,
                    UserName = userName,
                    Mobile = mobile,
                    Type = (int)(int)UserTypeEnum.个人,
                    Category = (int)UserCategoryEnum.媒体主,
                    Source = (int)RegisterFromEnum.H5,
                    RegisterType = (int)RegisterTypeEnum.汽车大全,
                    HeadimgURL = carUser.User_avatar
                };
                return lu;
            }
            return null;
        }
    }
    /// <summary>
    /// 合并用户类
    /// </summary>
    public class UpdateUserId
    {
        public int Source { get; set; }
        public int OldUserId { get; set; }
        public int NewUserId { get; set; }
    }

}
