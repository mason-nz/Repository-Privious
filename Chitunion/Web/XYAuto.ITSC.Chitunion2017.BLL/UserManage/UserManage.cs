using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto.LoginForWeChat;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.PayInfo;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.UserManage;
using XYAuto.ITSC.Chitunion2017.WebService.ITSupport;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage
{
    public class UserManage : VerifyOperateBase
    {
        public readonly static UserManage Instance = new UserManage();
        public object QueryUserBasicInfo(ReqQueryUserBasicInfoDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            var tp = Dal.UserManage.UserManage.Instance.QueryUserBasicInfo(req.UserID);

            var userInfo = BLL.Util.DataTableToEntity<ResBasicinfoDto>(tp.Item1);
            var userDetail = Util.DataTableToEntity<ResAuthenticationinfoDto>(tp.Item2);
            var userBank = Util.DataTableToEntity<ResBankaccountinfoDto>(tp.Item3);

            //获取微信信息
            var weixinUser = Dal.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(req.UserID, (UserCategoryEnum)userInfo.Category);
            userInfo.NickName = weixinUser != null ? weixinUser.nickname : string.Empty;
            var ret = new ResQueryUserBasicInfoDto()
            {
                BasicInfo = new ResBasicinfoDto(),
                AuthenticationInfo = new ResAuthenticationinfoDto(),
                BankAccountInfo = new ResBankaccountinfoDto()
            };

            ret.BasicInfo = userInfo;
            ret.AuthenticationInfo = userDetail;
            ret.BankAccountInfo = userBank;
            return ret;
        }
        public Dictionary<string, object> QueryUserBasicInfo_M(ReqQueryUserBasicInfoDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            var tp = Dal.UserManage.UserManage.Instance.QueryUserBasicInfo(req.UserID);

            var userInfo = BLL.Util.DataTableToEntity<ResBasicinfoDto>(tp.Item1);
            var userDetail = Util.DataTableToEntity<ResAuthenticationinfoDto>(tp.Item2);
            var userBank = Util.DataTableToEntity<ResBankaccountinfoDto>(tp.Item3);
            Dictionary<string, object> dicAll = new Dictionary<string, object>();

            #region 基本信息
            Dictionary<string, object> dicBasic = new Dictionary<string, object>();
            dicBasic.Add("UserName", "");
            dicBasic.Add("Mobile", "");
            if (userInfo != null)
            {
                dicBasic["UserName"] = userInfo.UserName;
                dicBasic["Mobile"] = userInfo.Mobile;
            }
            dicAll.Add("BasicInfo", dicBasic);
            #endregion

            #region 认证信息
            Dictionary<string, object> dicAuthentication = new Dictionary<string, object>();
            dicAuthentication.Add("Type", "");
            dicAuthentication.Add("Status", "");
            dicAuthentication.Add("TrueName", "");
            dicAuthentication.Add("IdentityNo", "");
            dicAuthentication.Add("IDCardFrontURL", "");
            dicAuthentication.Add("IDCardBackURL", "");
            dicAuthentication.Add("BLicenceURL", "");
            dicAuthentication.Add("Reason", "");
            if (userDetail != null)
            {
                dicAuthentication["Type"] = userDetail.Type;
                dicAuthentication["Status"] = userDetail.Status;
                dicAuthentication["TrueName"] = userDetail.TrueName;
                dicAuthentication["IdentityNo"] = userDetail.IdentityNo;
                dicAuthentication["IDCardFrontURL"] = userDetail.IDCardFrontURL;
                dicAuthentication["IDCardBackURL"] = userDetail.IDCardBackURL;
                dicAuthentication["BLicenceURL"] = userDetail.BLicenceURL;
                dicAuthentication["Reason"] = userDetail.Reason;
            }
            dicAll.Add("AuthenticationInfo", dicAuthentication);
            #endregion

            #region 账号信息
            Dictionary<string, object> dicBankAccount = new Dictionary<string, object>();
            dicBankAccount.Add("AccountName", "");
            dicBankAccount.Add("AccountType", "");
            if (userBank != null)
            {
                dicBankAccount["AccountName"] = userBank.AccountName;
                dicBankAccount["AccountType"] = userBank.AccountType;
            }
            dicAll.Add("BankAccountInfo", dicBankAccount);
            #endregion
            return dicAll;
        }
        public object EditUserBasicInfo(ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserBasicInfo.ReqDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            var isOK = Dal.UserManage.UserManage.Instance.EditUserBasicInfo(req.UserID, req.UserName, req.ProvinceID, req.CityID, req.Address, ref errorMsg);
            if (string.IsNullOrEmpty(errorMsg) && isOK)
                return "操作成功";
            else
                return "操作失败";
        }

        public object EditUserAuthenticationInfo(ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserAuthenticationInfo.ReqDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            var retValue = new WithdrawalsProvider(new ConfigEntity { CreateUserId = req.UserID }, new ReqWithdrawalsDto()).VerifyUserMobile();
            if (retValue.HasError)
            {
                return retValue.Message;
            }

            var isOK = Dal.UserManage.UserManage.Instance.EditUserAuthenticationInfo(req.UserID, req.Type, req.TrueName, req.BLicenceURL, req.IdentityNo, req.IDCardFrontURL, (int)Entities.UserManage.UserDetailStatus.待审核, ref errorMsg);
            if (string.IsNullOrEmpty(errorMsg) && isOK)
                return "操作成功";
            else
                return "操作失败";
        }

        public object EditUserPasswordInfo(ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserPasswordInfo.ReqDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            var userInfoAll = Dal.UserManage.UserInfo.Instance.GetModel(req.UserID);
            userInfoAll.Pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(req.NewPassword + userInfoAll.Category.ToString() + Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey"), System.Text.Encoding.UTF8);
            var isOK = Dal.UserManage.UserInfo.Instance.Update(userInfoAll);
            if (string.IsNullOrEmpty(errorMsg) && isOK)
                return "操作成功";
            else
                return "操作失败";
        }

        public object EditUserMobileInfo(ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserMobileInfo.ReqDto req, ref string errorMsg)
        {
            errorMsg = string.Empty;
            if (!req.CheckSelfModel(out errorMsg))
                return null;

            var userInfoAll = Dal.UserManage.UserInfo.Instance.GetModel(req.UserID);
            userInfoAll.Mobile = req.Mobile;
            var isOK = Dal.UserManage.UserInfo.Instance.Update(userInfoAll);
            if (string.IsNullOrEmpty(errorMsg) && isOK)
            {
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(req.UserID);
                return "操作成功";
            }
            else
                return "操作失败";
        }
        public Entities.UserManage.UserInfoAll GetModel(int userID)
        {
            return Dal.UserManage.UserInfo.Instance.GetModel(userID);
        }

        #region V2.3微信版
        public Dictionary<string, object> GetMobileStatus(string Mobile, out string ErrorMessage)
        {
            ErrorMessage = "";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(Mobile))
            {
                dic.Add("MobileStatus", -1);
                ErrorMessage = "请填写手机号！";
                return dic;
            }
            if (!BLL.Util.IsHandset(Mobile.Trim()))
            {
                dic.Add("MobileStatus", -1);
                ErrorMessage = "请输入正确的手机号！";
                return dic;
            }
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            int mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(Mobile.Trim(), (int)UserCategoryEnum.媒体主, userId);
            if (mUserId <= 0)
            {
                dic.Add("MobileStatus", 0);
            }
            else
            {
                string mobile = "";
                if (Dal.UserManage.UserInfo.Instance.IsAddedMobile(userId, ref mobile))
                {
                    dic.Add("MobileStatus", 1);
                }
                else
                {
                    //分支合并后，需要自行修改逻辑
                    //if (Dal.WechatUser.Instance.IsWxUser(mUserId))
                    string channelUserId = Dal.UserManage.UserInfo.Instance.GechannelUserId(userId, (int)UserCategoryEnum.媒体主);
                    if (Dal.WechatUser.Instance.IsWxUser(mUserId) || !string.IsNullOrEmpty(channelUserId))
                    {
                        dic.Add("MobileStatus", 1);
                    }
                    else
                    {
                        dic.Add("MobileStatus", 2);
                    }
                }
            }
            return dic;
        }
        /// <summary>
        /// 根据手机号查询用户信息
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public object QueryUserInfo(string Mobile, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(Mobile))
            {

                return null;
            }
            var tp = Dal.UserManage.UserManage.Instance.QueryUserInfo(Mobile.Trim(), (int)UserCategoryEnum.媒体主);

            var userInfo = BLL.Util.DataTableToEntity<ResBasicinfoDto>(tp.Item1);
            var userDetail = Util.DataTableToEntity<ResAuthenticationinfoDto>(tp.Item2);
            var userBank = Util.DataTableToEntity<ResBankaccountinfoDto>(tp.Item3);

            var ret = new ResQueryUserBasicInfoDto()
            {
                BasicInfo = new ResBasicinfoDto(),
                AuthenticationInfo = new ResAuthenticationinfoDto(),
                BankAccountInfo = new ResBankaccountinfoDto()
            };

            ret.BasicInfo = userInfo;
            ret.AuthenticationInfo = userDetail;
            ret.BankAccountInfo = userBank;
            return ret;
        }
        /// <summary>
        /// 验证支付宝账号是否可用
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        public string VerifBankAccount(BankAccountReqDto DTO)
        {
            if (DTO == null)
                return "参数错误";

            if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), DTO.AccountType))
                return "参数错误";

            if (string.IsNullOrWhiteSpace(DTO.OldAccountName))
                return "请输入旧支付宝账号！";

            if (string.IsNullOrWhiteSpace(DTO.NewAccountName))
                return "请输入新支付宝账号！";

            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (!Dal.UserManage.UserManage.Instance.IsSelfAccount(DTO.AccountType, DTO.OldAccountName.Trim(), userId))
            {
                return "旧支付宝账号输入有误！";
            };
            if (DTO.OldAccountName.Trim() == DTO.NewAccountName.Trim())
            {
                return "请输入新的支付宝账号";
            }
            if (!BLL.Util.IsHandset(DTO.NewAccountName.Trim()) && !BLL.Util.IsEmail(DTO.NewAccountName.Trim()))
            {
                return "支付宝账号格式不正确";
            }
            if (Dal.UserManage.UserManage.Instance.IsSelfAccount(DTO.AccountType, DTO.NewAccountName.Trim()))
            {
                return "您输入的支付宝账号已绑定其他账户";
            }
            return "";
        }
        /// <summary>
        /// 获取审核状态和微信昵称
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetExamineStatus()
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            DataSet ds = Dal.UserManage.UserManage.Instance.GetExamineStatus(userId);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Status", 0);
            dic.Add("NickName", "");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                dic["Status"] = ds.Tables[0].Rows[0]["Status"];
            }
            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
            {
                dic["NickName"] = ds.Tables[1].Rows[0]["nickname"];
            }
            return dic;
        }
        /// <summary>
        /// 修改支付宝和手机号
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        public string ModifyMobileAccount(ModifyMobileReqDto DTO)
        {
            if (DTO == null)
                return "参数错误";

            if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), DTO.AccountType))
                return "参数错误";

            if (string.IsNullOrWhiteSpace(DTO.AccountName))
                return "请输入支付宝账号！";

            if (string.IsNullOrWhiteSpace(DTO.Mobile))
                return "请填写手机号！";

            if (!BLL.Util.IsHandset(DTO.Mobile.Trim()))
                return "手机号格式不正确！";

            if (!BLL.Util.IsHandset(DTO.AccountName.Trim()) && !BLL.Util.IsEmail(DTO.AccountName.Trim()))
                return "支付宝账号格式不正确!";

            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            int mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(DTO.Mobile.Trim(), (int)UserCategoryEnum.媒体主, userId);
            if (mUserId > 0)
                return "该手机号码已注册，请更换手机号";

            if (Dal.UserManage.UserManage.Instance.IsOtherAccount(DTO.AccountType, DTO.AccountName.Trim(), new List<int> { userId }))
                return "该支付宝号已注册，请更换支付宝号";

            Entities.LETask.LeUserBankAccount entity = new Entities.LETask.LeUserBankAccount() { UserID = userId, AccountType = DTO.AccountType, AccountName = DTO.AccountName.Trim() };

            int resultAcount = Dal.LETask.LeUserBankAccount.Instance.Update(entity);

            int resultMobile = Dal.UserManage.UserInfo.Instance.UpdateMobile(DTO.Mobile.Trim(), userId);
            if (resultAcount <= 0 && resultMobile > 0)
            {
                return "支付宝账号修改失败！";
            }
            else if (resultAcount <= 0 && resultMobile <= 0)
            {
                return "修改失败，请重试！";
            }
            else if (resultAcount > 0 && resultMobile <= 0)
            {
                return "手机号修改失败！";
            }
            if (resultMobile > 0)
            {
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            }
            return "";
        }
        public string ModifyAttestation(ModifyAttestationReqDto DTO)
        {
            string strJson = Json.JsonSerializerBySingleData(DTO);
            Loger.Log4Net.Info("保存用户参数：" + strJson);
            if (DTO == null)
                return "参数错误";

            if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), DTO.AccountType))
                return "参数错误";

            if (string.IsNullOrWhiteSpace(DTO.AccountName))
                return "请填写支付宝账号！";
            if (!BLL.Util.IsHandset(DTO.AccountName.Trim()) && !BLL.Util.IsEmail(DTO.AccountName.Trim()))
                return "支付宝账号格式不正确!";
            if (string.IsNullOrWhiteSpace(DTO.Mobile))
                return "请填写手机号！";
            if (!BLL.Util.IsHandset(DTO.Mobile.Trim()))
                return "手机号格式不正确！";
            if (DTO.Type <= 0)
                return "请选择类型！";

            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (userId <= 0)
            {
                BLL.Loger.Log4Net.Info("请先登录,输出用户Id:" + userId);
                return "请先登录";
            }

            if (DTO.Type == (int)UserTypeEnum.个人)
            {
                if (string.IsNullOrWhiteSpace(DTO.TrueName))
                    return "请填写真实姓名！";
                if (string.IsNullOrWhiteSpace(DTO.IDCardFrontURL))
                    return "请上传身份证图片！";
                if (string.IsNullOrWhiteSpace(DTO.IdentityNo))
                    return "请输入身份证号码！";
                if (!DTO.IDCardFrontURL.Contains("http://"))
                    DTO.IDCardFrontURL = WeChat.ImageOperate.Instance.GetImgUrlByServerId(DTO.IDCardFrontURL);
                if (DTO.IDCardFrontURL == null)
                {
                    return "身份证图片保存失败，请重试！";
                }

            }
            else
            {
                if (string.IsNullOrWhiteSpace(DTO.TrueName))
                    return "请填写公司名称！";
                if (string.IsNullOrWhiteSpace(DTO.BLicenceURL))
                    return "请上传营业执照图片！";
                if (!DTO.BLicenceURL.Contains("http://"))
                    DTO.BLicenceURL = WeChat.ImageOperate.Instance.GetImgUrlByServerId(DTO.BLicenceURL);
                if (DTO.BLicenceURL == null)
                {
                    return "营业执照图片保存失败，请重试！";
                }
            }
            int mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(DTO.Mobile.Trim(), (int)UserCategoryEnum.媒体主, userId);
            List<int> listUserId = new List<int>() { userId };
            if (mUserId > 0)
            {
                // 判断是否已添加过认证
                if (Dal.UserManage.UserDetailInfo.Instance.IsExistsByUserID(userId))
                {
                    return "该手机号已注册，请更换手机号";
                }
                else
                {
                    if (Dal.WechatUser.Instance.IsWxUser(mUserId))
                        return "该手机号已注册，请更换手机号";
                }
            }
            if (mUserId > 0)
                listUserId.Add(mUserId);
            if (DTO.Type == (int)UserTypeEnum.个人)
            {
                if (Dal.UserManage.UserDetailInfo.Instance.IsExistsIdentityNo(listUserId, (int)UserCategoryEnum.媒体主, DTO.IdentityNo.Trim()))
                    return "您输入的身份证号已绑定其他账户";
            }
            if (Dal.UserManage.UserManage.Instance.IsOtherAccount(DTO.AccountType, DTO.AccountName.Trim(), listUserId))
                return "您输入的支付宝账号已绑定其他账户";
            if (mUserId > 0)
            {
                int DetailStatus = Dal.UserManage.UserManage.Instance.GetUserDetailStatus(mUserId);
                if (Dal.UserManage.UserManage.Instance.CleanUserInfo(DTO.Type, mUserId, userId, DTO.TrueName.Trim(), DTO.BLicenceURL.Trim(), DTO.IdentityNo.Trim(), DTO.IDCardFrontURL.Trim(), DetailStatus, DTO.AccountType, DTO.AccountName.Trim()))
                {
                    string QueueName = "";
                    LeadsTask.Instance.CreateQueue(false, out QueueName);
                    MessageQueue MQ = new MessageQueue(QueueName);

                    MQ.Formatter = new XmlMessageFormatter(new System.Type[] { typeof(UpdateUserId) });
                    UpdateUserId updateUserId = new UpdateUserId();
                    updateUserId.Source = 1;
                    updateUserId.OldUserId = mUserId;
                    updateUserId.NewUserId = userId;
                    MQ.Send(updateUserId);
                    userId = mUserId;
                }
                else
                {
                    return "保存失败，请稍后重试！";
                }
            }
            else
            {
                string errorMsg = "";
                Dal.UserManage.UserManage.Instance.EditUserAuthenticationInfo(userId, DTO.Type, DTO.TrueName.Trim(), DTO.BLicenceURL.Trim(), DTO.IdentityNo.Trim(), DTO.IDCardFrontURL.Trim(), (int)Entities.UserManage.UserDetailStatus.待审核, ref errorMsg, DTO.AccountType, 1, DTO.AccountName.Trim(), DTO.Mobile.Trim());
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return "保存失败，请稍后重试！";
                }
            }
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            return "";
        }
        #endregion

        #region V2.5微信版
        /// <summary>
        /// 查询个人信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> QueryUserInfo()
        {
            Dictionary<string, object> dicAll = null;
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"QueryUserInfo->userId:{userId}");
            DataTable dt = Dal.UserManage.UserManage.Instance.QueryUserInfo(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                dicAll = new Dictionary<string, object>();
                DataRow dr = dt.Rows[0];
                dicAll.Add("NickName", dr["NickName"].ToString());
                dicAll.Add("Mobile", dr["Mobile"].ToString());
                dicAll.Add("AccountName", dr["AccountName"].ToString());
                dicAll.Add("AccountType", string.IsNullOrWhiteSpace(dr["AccountType"].ToString()) ? 0 : Convert.ToInt32(dr["AccountType"]));
                dicAll.Add("Status", string.IsNullOrWhiteSpace(dr["Status"].ToString()) ? 0 : Convert.ToInt32(dr["Status"]));
                dicAll.Add("IsFollow", string.IsNullOrWhiteSpace(dr["IsFollow"].ToString()) ? -1 : Convert.ToInt32(dr["IsFollow"]));
            }
            return dicAll;
        }
        /// <summary>
        /// 验证支付宝账号是否可用
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        private string VerifBankAccount(PayInfoDto DTO, ref int UserId)
        {
            if (DTO == null)
                return "参数错误";

            if (!DTO.IsAdd)
            {
                if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), DTO.OldAccountType))
                    return "参数错误";
                if (string.IsNullOrWhiteSpace(DTO.OldAccountName))
                    return "请输入旧支付宝账号！";
            }
            if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), DTO.AccountType))
                return "参数错误";
            if (string.IsNullOrWhiteSpace(DTO.AccountName))
                return "请输入新支付宝账号！";
            if (!DTO.IsAdd)
            {
                if (DTO.OldAccountName.Trim() == DTO.AccountName.Trim())
                {
                    return "新支付宝账号与老支付宝账号一致";
                }
                if (!Dal.UserManage.UserManage.Instance.IsSelfAccount(DTO.OldAccountType, DTO.OldAccountName.Trim(), UserId))
                {
                    return "旧支付宝账号输入有误！";
                };
            }
            UserId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (UserId <= 0)
            {
                BLL.Loger.Log4Net.Info("请先登录,输出用户Id:" + UserId);
                return "请先登录";
            }
            if (!BLL.Util.IsHandset(DTO.AccountName.Trim()) && !BLL.Util.IsEmail(DTO.AccountName.Trim()))
            {
                return "支付宝账号格式不正确";
            }
            if (Dal.UserManage.UserManage.Instance.IsSelfAccount(DTO.AccountType, DTO.AccountName.Trim()))
            {
                return "您输入的支付宝账号已绑定其他账户";
            }
            return "";
        }
        /// <summary>
        /// 保存支付宝
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        public string SavePayInfo(PayInfoDto Dto)
        {
            int userId = 0;
            string errorMsg = VerifBankAccount(Dto, ref userId);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return errorMsg;
            }
            string mobile = "";
            if (!Dal.UserManage.UserInfo.Instance.IsAddedMobile(userId, ref mobile))
            {
                return "请先填写手机号";
            };
            Entities.LETask.LeUserBankAccount entity = new Entities.LETask.LeUserBankAccount() { UserID = userId, AccountType = Dto.AccountType, AccountName = Dto.AccountName.Trim() };
            int resultAcount = Dal.LETask.LeUserBankAccount.Instance.Update(entity);
            if (resultAcount <= 0)
            {
                return "保存失败，请重试！";
            }
            return "";
        }
        /// <summary>
        /// 查询认证信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> QueryUserDetailInfo()
        {
            Dictionary<string, object> dicAll = null;
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            DataTable dt = Dal.UserManage.UserManage.Instance.QueryUserDetailInfo(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                dicAll = new Dictionary<string, object>();
                DataRow dr = dt.Rows[0];
                dicAll.Add("Type", string.IsNullOrWhiteSpace(dr["Type"].ToString()) ? 0 : Convert.ToInt32(dr["Type"]));
                dicAll.Add("Status", string.IsNullOrWhiteSpace(dr["Status"].ToString()) ? 0 : Convert.ToInt32(dr["Status"]));
                dicAll.Add("TrueName", dr["TrueName"].ToString());
                dicAll.Add("IdentityNo", dr["IdentityNo"].ToString());
                dicAll.Add("IDCardFrontURL", dr["IDCardFrontURL"].ToString());
                dicAll.Add("BLicenceURL", dr["BLicenceURL"].ToString());
                dicAll.Add("Reason", dr["Reason"].ToString());
            }
            return dicAll;
        }
        /// <summary>
        /// 验证认证信息
        /// </summary>
        /// <param name="DTO"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private string VerifUserDetail(ReqUserDetailDto DTO, ref int UserId)
        {
            if (DTO == null)
                return "参数错误";
            if (!Enum.IsDefined(typeof(Entities.UserManage.Type), DTO.Type))
                return "参数错误";

            UserId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (UserId <= 0)
            {
                BLL.Loger.Log4Net.Info("请先登录,输出用户Id:" + UserId);
                return "请先登录";
            }
            if (DTO.Type == (int)Entities.UserManage.Type.个人)
            {
                if (string.IsNullOrWhiteSpace(DTO.TrueName))
                    return "真实姓名不能为空";
                //if (string.IsNullOrWhiteSpace(DTO.IDCardFrontURL))
                //    return "身份证图片不能为空！";
                if (string.IsNullOrWhiteSpace(DTO.IdentityNo))
                    return "身份证号码不能为空";
                if (!BLL.Util.CheckIDCard(DTO.IdentityNo.Trim()))
                    return "请输入正确的身份证号码";
                if (Dal.UserManage.UserDetailInfo.Instance.IsExistsIdentityNo(new List<int> { UserId }, (int)UserCategoryEnum.媒体主, DTO.IdentityNo.Trim()))
                    return "您输入的身份证号已绑定其他账户";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(DTO.TrueName))
                    return "公司名称不能为空";
                if (string.IsNullOrWhiteSpace(DTO.BLicenceURL))
                    return "营业执照不能为空";
            }
            return "";
        }
        /// <summary>
        /// 保存认证信息
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        public string SaveUserDetail(ReqUserDetailDto DTO)
        {
            int userId = 0;
            string errorMsg = VerifUserDetail(DTO, ref userId);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return errorMsg;
            }
            string mobile = "";
            if (!Dal.UserManage.UserInfo.Instance.IsAddedMobile(userId, ref mobile))
            {
                return "请先填写手机号";
            };
            var userInfoAll = Dal.UserManage.UserInfo.Instance.GetModel(userId);
            userInfoAll.Type = (int)DTO.Type;
            userInfoAll.TrueName = DTO.TrueName.Trim();

            if ((int)Entities.UserManage.Type.企业 == DTO.Type)
            {
                userInfoAll.BLicenceURL = DTO.BLicenceURL.Trim();
                //清空个人信息
                userInfoAll.IdentityNo = string.Empty;
                userInfoAll.IDCardFrontURL = string.Empty;
            }
            else if ((int)Entities.UserManage.Type.个人 == DTO.Type)
            {
                userInfoAll.IdentityNo = DTO.IdentityNo.Trim();
                //userInfoAll.IDCardFrontURL = DTO.IDCardFrontURL.Trim();
                //清空企业信息
                userInfoAll.BLicenceURL = string.Empty;
            }
            bool result;
            if (!Dal.UserManage.UserInfo.Instance.UpdateType(DTO.Type, userId))
            {
                return "保存失败，请重试！";
            }
            if (!Dal.UserManage.UserDetailInfo.Instance.IsExistsByUserID(userId))
                result = Dal.UserManage.UserDetailInfo.Instance.Insert(userInfoAll, DTO.Type == 1002 ? 2 : 1) > 0 ? true : false;
            else
                result = Dal.UserManage.UserDetailInfo.Instance.Update(userInfoAll, DTO.Type == 1002 ? 2 : 1);

            if (!result)
            {
                return "保存失败，请重试！";
            }
            return "";
        }
        /// <summary>
        /// 保存手机号
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public string SaveMobile(string Mobile)
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (userId <= 0)

            {
                BLL.Loger.Log4Net.Info("请先登录,输出用户Id:" + userId);
                return "请先登录";
            }
            int mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(Mobile, (int)UserCategoryEnum.媒体主, userId);
            string mobile = "";

            bool isAddMobile = Dal.UserManage.UserInfo.Instance.IsAddedMobile(userId, ref mobile);
            if (Mobile == mobile)
            {
                return "新手机号码与老手机号码一致";
            }
            //是否有其他人注册过该手机号
            if (mUserId > 0)
            {
                //是否添加过手机号

                if (isAddMobile)
                {
                    return "该手机号已注册，请更换手机号";
                }
                else
                {
                    //分支合并后，需要自行修改逻辑
                    //if (Dal.WechatUser.Instance.IsWxUser(mUserId))
                    string channelUserId = Dal.UserManage.UserInfo.Instance.GechannelUserId(userId, (int)UserCategoryEnum.媒体主);
                    if (Dal.WechatUser.Instance.IsWxUser(mUserId) || !string.IsNullOrEmpty(channelUserId))
                        return "该手机号已注册，请更换手机号";
                }
            }
            if (mUserId > 0)
            {
                try
                {
                    BLL.Loger.Log4Net.Info($"合并用户：老用户{mUserId};新用户{userId}");
                    int DetailStatus = Dal.UserManage.UserManage.Instance.GetUserDetailStatus(mUserId);
                    if (Dal.UserManage.UserManage.Instance.CleanUserInfo(mUserId, userId))
                    {
                        string QueueName = "";
                        LeadsTask.Instance.CreateQueue(false, out QueueName);
                        MessageQueue MQ = new MessageQueue(QueueName);

                        MQ.Formatter = new XmlMessageFormatter(new System.Type[] { typeof(UpdateUserId) });
                        UpdateUserId updateUserId = new UpdateUserId();
                        updateUserId.Source = 1;
                        updateUserId.OldUserId = mUserId;
                        updateUserId.NewUserId = userId;
                        MQ.Send(updateUserId);
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(mUserId);
                    }
                    else
                    {
                        return "保存失败，请稍后重试！";
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error($"用户保存失败：", ex);
                    return "保存失败，请稍后重试！";
                }

            }
            else
            {
                int resultMobile = Dal.UserManage.UserInfo.Instance.UpdateMobile(Mobile, userId);
                if (resultMobile <= 0)
                {
                    return "保存失败，请稍后重试！";
                }
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            }
            return "";
        }
        /// <summary>
        /// 判断新老手机号是否一致
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public bool IsAddedMobile(string Mobile)
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            string mobile = "";
            bool IsAddMobile = Dal.UserManage.UserInfo.Instance.IsAddedMobile(userId, ref mobile);
            if (Mobile == mobile)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region V2.8 H5版本
        public int GetUserIdByMobile(string Mobile, int Category)
        {
            return Dal.UserManage.UserInfo.Instance.GetUserIdByMobile(Mobile, Category);
        }
        public int InsertUser(string Mobile, string ip)
        {
            long promotionId = 0;
            Loger.Log4Net.Info("新增用户：" + Mobile);
            if (HttpContext.Current.Request.Cookies["promotionName"] != null)
            {
                string promotionName = HttpContext.Current.Request.Cookies["promotionName"].Value;
                Loger.Log4Net.Info("获取cookie值：" + promotionName);
                if (!string.IsNullOrEmpty(promotionName))
                {
                    promotionId = XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetByChanneID(promotionName);
                }
            }
            else
            {
                Loger.Log4Net.Info("未获取到cookie值");
            }
            //分支合并后，需要自行修改逻辑
            //return Dal.UserManage.UserInfo.Instance.InsertUser(Mobile, Mobile, (int)Entities.UserManage.Type.个人, (int)RegisterFromEnum.H5, (int)UserCategoryEnum.媒体主, (int)RegisterTypeEnum.H5, promotionId);
            return Dal.UserManage.UserInfo.Instance.InsertUser(Mobile, Mobile, (int)Entities.UserManage.Type.个人, (int)RegisterFromEnum.H5, (int)UserCategoryEnum.媒体主, (int)RegisterTypeEnum.手机号, promotionId, ip);
        }

        public string Login(ReqLoginDTO DTO)
        {
            if (DTO == null)
            {
                return "请填写正确的手机号";
            }
            string RequestMobile = DTO.mobile;
            string RequestMobileCheckCode = DTO.mobileCheckCode;
            if (RequestMobile != null && RequestMobile.Length > 0 &&
             RequestMobileCheckCode != null && RequestMobileCheckCode.Length > 0)
            {
                if (!BLL.Util.IsHandset(RequestMobile))
                {
                    return "请填写正确的手机号";
                }
                else if (BLL.Util.GetMobileCheckCodeByCache(RequestMobile) != RequestMobileCheckCode)
                {
                    return "您输入的手机短信验证码不正确";
                }
                int userId = BLL.UserManage.UserManage.Instance.GetUserIdByMobile(RequestMobile, (int)UserCategoryEnum.媒体主);
                if (userId > 0)
                {
                    string content = string.Format("用户{1}(ID:{0})登录成功。", userId, RequestMobile);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.登陆, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, "", userId);
                    XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId, true);
                }
                else
                {
                    userId = BLL.UserManage.UserManage.Instance.InsertUser(RequestMobile, DTO.Ip);
                    if (userId <= 0)
                    {
                        return "登录失败，请稍后重试";
                    }
                    else
                    {
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId, true);
                        string content = string.Format("用户{1}(ID:{0})注册成功。", userId, RequestMobile);
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, "", userId);
                    }
                }
            }
            else
            {
                return "手机号或验证码错误";
            }
            return "";
        }
        public string SendValidateCode(ReqLoginDTO DTO)
        {
            string ret = string.Empty;
            if (DTO == null)
            {
                ret = "-12";
            }
            else
            {
                string RequestMobile = DTO.mobile;
                if (!BLL.Util.IsHandset(RequestMobile))
                { ret = "-12"; }
                else
                {
                    if (BLL.Util.GetMobileCheckCodeTimesByCache(RequestMobile) > 2)
                    {
                        ret = "-10";//上次发送过验证码给到这个手机号，还没有过超时时间
                    }
                    else
                    {
                        string code = BLL.Util.GetMobileCheckCodeByCache(RequestMobile);
                        if (code == null)
                        {
                            BLL.Loger.Log4Net.Info($"根据手机号：{RequestMobile}获取随机码");
                            ValidateCode vc = new ValidateCode();
                            code = vc.GetRandomCode(4, 1);
                            BLL.Loger.Log4Net.Info($"获取随机码成功：{code}");
                        }
                        string sendSMS_ContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_LoginContentTemp");
                        BLL.Loger.Log4Net.Info(sendSMS_ContentTemp);
                        string content = string.Format(sendSMS_ContentTemp, code);
                        BLL.Loger.Log4Net.Info(content);
                        SendMsgResult smr = SMSServiceHelper.Instance.SendMsgImmediately(RequestMobile, content);
                        BLL.Loger.Log4Net.Info("调用SendMsgImmediately成功");
                        if (Convert.ToBoolean(smr.Result))
                        {
                            BLL.Util.AddWebCacheByMobile(RequestMobile, code);
                            ret = "0";//发送短信成功
                        }
                        else
                        {
                            BLL.Loger.Log4Net.Info($"给{RequestMobile}手机，发送短信内容：{content}，失败，具体接口返回内容为：{smr.Message}");
                            ret = "-11";///发送短信失败
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 查询汽车大全端登录信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public LoginUserInfo GetUserInfoByToken(string token)
        {
            Loger.Log4Net.Info("查询汽车大全用户信息" + token);
            WebService.Qichedaquan.UserInfo u = WebService.Qichedaquan.UserInfoHelper.Instance.GetUserInfoByToken(token);
            Loger.Log4Net.Info("UserInfo:" + Json.JsonSerializerBySingleData(u));
            Common.LoginUser chituUser = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
            if (u != null)
            {
                bool isSuccess = false;
                var userid = -1;
                if (chituUser != null && chituUser.UserID > 0 && chituUser.UserName == u.Nick_name)
                {
                    userid = chituUser.UserID;
                    isSuccess = true;
                }
                else
                {
                    userid = InsertUserInfoByToken(token, u);
                    if (userid > 0)
                    {
                        isSuccess = true;
                    }
                }
                if (!isSuccess) return null;
                var mobile = "";
                if (chituUser != null)
                {
                    mobile = chituUser.Mobile;
                }
                LoginUserInfo lu = new LoginUserInfo
                {
                    UserID = userid,
                    UserName = u.Nick_name,
                    Mobile = mobile,
                    TypeID = (int)Entities.UserManage.Type.个人,
                    Category = (int)UserCategoryEnum.媒体主,
                    Source = (int)RegisterFromEnum.H5,
                    RegisterType = (int)RegisterTypeEnum.手机号,
                    HeadImgUrl = u.User_avatar
                };
                return lu;
            }
            return null;
        }

        /// <summary>
        /// 汽车大全端登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int InsertUserInfoByToken(string token, WebService.Qichedaquan.UserInfo u = null)
        {
            try
            {
                if (u == null)
                {
                    u = WebService.Qichedaquan.UserInfoHelper.Instance.GetUserInfoByToken(token);
                }
                if (u != null)
                {
                    int userId = 0;
                    bool isSuccess = false;
                    //if (!string.IsNullOrEmpty(u.Mobile))
                    //{
                    //    userId = Dal.UserManage.UserInfo.Instance.GetUserIdByMobile(u.Mobile, (int)UserCategoryEnum.媒体主);
                    //}
                    if (userId <= 0)
                    {
                        userId = Dal.UserManage.UserInfo.Instance.GetUserIdByChannelUID(u.User_token,
                            (int)UserCategoryEnum.媒体主);
                    }
                    if (userId > 0)
                    {
                        Dal.UserManage.UserInfo.Instance.UpdateH5User(u.User_token, userId, u.Nick_name, u.User_avatar,
                            u.User_gender);
                        if (!Dal.UserManage.UserDetailInfo.Instance.IsExistsByUserID(userId))
                        {
                            Entities.UserManage.UserInfoAll ua = new Entities.UserManage.UserInfoAll();
                            ua.UserID = userId;
                            if (u.User_gender == 1)
                            {
                                ua.Sex = 1;
                            }
                            else if (u.User_gender == 2)
                            {
                                ua.Sex = 0;
                            }
                            Dal.UserManage.UserDetailInfo.Instance.Insert(ua, 0);
                        }
                        isSuccess = true;
                        Loger.Log4Net.Info($"汽车大全用户{token}已存在，更新成功");
                    }
                    else
                    {
                        #region  获取cookie

                        long promotionId = 0;
                        Loger.Log4Net.Info("新增用户：" + u.Nick_name);
                        if (HttpContext.Current != null)
                        {
                            string cookies = "获取cookie：";
                            foreach (var item in HttpContext.Current.Request.Cookies)
                            {
                                cookies += Json.JsonSerializerBySingleData(item) + "--";
                            }
                            if (HttpContext.Current.Request.Cookies["promotionName"] != null)
                            {
                                string promotionName = HttpContext.Current.Request.Cookies["promotionName"].Value;
                                Loger.Log4Net.Info("获取cookie值：" + promotionName);
                                if (!string.IsNullOrEmpty(promotionName))
                                {
                                    promotionId =
                                        XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetByChanneID(
                                            promotionName);
                                }
                            }
                            else
                            {
                                Loger.Log4Net.Info("未获取到cookie值");
                            }
                            Loger.Log4Net.Info(cookies);
                        }
                        else
                        {
                            Loger.Log4Net.Info("cookie为null");
                        }

                        #endregion

                        userId = Dal.UserManage.UserInfo.Instance.InsertH5User(u.Nick_name,
                            (int)Entities.UserManage.Type.个人, (int)RegisterFromEnum.H5, (int)UserCategoryEnum.媒体主,
                            (int)RegisterTypeEnum.手机号, promotionId, u.User_avatar, u.User_token);
                        if (userId > 0)
                        {
                            isSuccess = true;
                            Entities.UserManage.UserInfoAll ua = new Entities.UserManage.UserInfoAll();
                            ua.UserID = userId;
                            if (u.User_gender == 1)
                            {
                                ua.Sex = 1;
                            }
                            else if (u.User_gender == 2)
                            {
                                ua.Sex = 0;
                            }
                            Dal.UserManage.UserDetailInfo.Instance.Insert(ua, 0);
                            Loger.Log4Net.Info($"汽车大全用户{token}不存在，新增成功");
                        }
                        else
                        {
                            Loger.Log4Net.Info($"汽车大全用户{token}不存在，新增失败");
                        }
                    }
                    if (isSuccess)
                    {
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId, true);
                    }
                    return userId;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"汽车大全用户添加失败", ex);
                return -1;
            }
        }

        #endregion

        #region V2.1.0
        public Dictionary<string, object> LoginForAndroid(ReqLoginDTO DTO, out string errorMsg)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("IsNewUser", "false");
            dic.Add("cookiesVal", "");
            dic.Add("HomeUrl", ConfigurationUtil.GetAppSettingValue("Domin") + "/index.html?channel=dqappwdcticon");
            errorMsg = string.Empty;
            if (DTO == null)
            {
                errorMsg = "请填写正确的手机号";
            }
            string RequestMobile = DTO.mobile;
            string RequestMobileCheckCode = DTO.mobileCheckCode;
            if (RequestMobile != null && RequestMobile.Length > 0 &&
             RequestMobileCheckCode != null && RequestMobileCheckCode.Length > 0)
            {
                if (!BLL.Util.IsHandset(RequestMobile))
                {
                    errorMsg = "请填写正确的手机号";
                }
                else if (BLL.Util.GetMobileCheckCodeByCache(RequestMobile) != RequestMobileCheckCode)
                {
                    errorMsg = "您输入的手机短信验证码不正确";
                }
                else
                {
                    string cookiesVal = "";
                    int userId = BLL.UserManage.UserManage.Instance.GetUserIdByMobile(RequestMobile, (int)UserCategoryEnum.媒体主);
                    if (userId > 0)
                    {

                        string content = string.Format("用户{1}(ID:{0})登录成功。", userId, RequestMobile);
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.登陆, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, "", userId);
                        cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId, true);
                    }
                    else
                    {

                        userId = BLL.UserManage.UserManage.Instance.InsertUserForAndroid(RequestMobile, DTO.Ip);
                        if (userId <= 0)
                        {
                            errorMsg = "登录失败，请稍后重试";
                        }
                        else
                        {
                            cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId, true);
                            string content = string.Format("用户{1}(ID:{0})注册成功。", userId, RequestMobile);
                            XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, "", userId);
                        }
                    }
                    dic["cookiesVal"] = cookiesVal;
                    dic["IsNewUser"] = Dal.LETask.AppDevice.Instance.IsExist(userId) ? "false" : "true";

                }
            }
            else
            {
                errorMsg = "手机号或验证码错误";
            }
            return dic;
        }
        public int InsertUserForAndroid(string Mobile, string ip)
        {
            long promotionId = 0;
            Loger.Log4Net.Info("InsertUserForAndroid新增用户：" + Mobile);
            return Dal.UserManage.UserInfo.Instance.InsertUser(Mobile, Mobile, (int)Entities.UserManage.Type.个人, (int)RegisterFromEnum.APP, (int)UserCategoryEnum.媒体主, (int)RegisterTypeEnum.手机号, promotionId, ip);
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetUserInfo()
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetUserInfo->userId:{userId}");
            DataTable dt = Dal.UserManage.UserManage.Instance.GetUserInfo(userId);
            return AddUserToDic(dt);
        }
        /// <summary>
        /// 根据手机号查询用户信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetUserInfoByMobile(string mobile)
        {
            //分支合并后，需要自行修改逻辑
            //DataTable dt = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(mobile, (int)UserCategoryEnum.媒体主);
            DataTable dt = Dal.UserManage.UserManage.Instance.GetUserInfoByMobile(mobile.Trim());
            return AddUserToDic(dt);
        }
        /// <summary>
        /// 个人信息table加入字典
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private Dictionary<string, object> AddUserToDic(DataTable dt)
        {
            Dictionary<string, object> dicAll = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                dicAll = new Dictionary<string, object>();
                DataRow dr = dt.Rows[0];
                dicAll.Add("NickName", dr["nickname"].ToString());
                dicAll.Add("HeadImgUrl", dr["headimgurl"].ToString());
                dicAll.Add("Mobile", dr["Mobile"].ToString());
                dicAll.Add("TrueName", dr["TrueName"].ToString());
                dicAll.Add("Sex", string.IsNullOrWhiteSpace(dr["Sex"].ToString()) ? -1 : Convert.ToInt32(dr["Sex"]));
                dicAll.Add("IdentityNo", dr["IdentityNo"].ToString());
                dicAll.Add("AccountName", dr["AccountName"].ToString());
                dicAll.Add("AccountType", string.IsNullOrWhiteSpace(dr["AccountType"].ToString()) ? 0 : Convert.ToInt32(dr["AccountType"]));
                dicAll.Add("Status", string.IsNullOrWhiteSpace(dr["Status"].ToString()) ? 0 : Convert.ToInt32(dr["Status"]));
            }
            return dicAll;
        }
        /// <summary>
        /// 获取手机号状态（0：不存在 1：该手机号码已注册，请更换手机号 2：该手机号已在pc端注册，是否做信息合并，-1：手机号格式不正确）
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetMobileStatus(string Mobile)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(Mobile))
            {
                dic.Add("MobileStatus", -1);
                return dic;
            }
            if (!BLL.Util.IsHandset(Mobile.Trim()))
            {
                dic.Add("MobileStatus", -1);
                return dic;
            }
            int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            int mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(Mobile.Trim(), (int)UserCategoryEnum.媒体主, userId);
            if (mUserId <= 0)
            {
                dic.Add("MobileStatus", 0);
            }
            else
            {
                string mobile = "";
                if (Dal.UserManage.UserInfo.Instance.IsAddedMobile(userId, ref mobile))
                    dic.Add("MobileStatus", 1);
                else
                    dic.Add("MobileStatus", 2);
            }
            return dic;
        }
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="errormsg"></param>
        public RespLoginForWeChatDto LoginForWeChat(ReqLoginDTO dto, ref string errormsg)
        {
            errormsg = string.Empty;
            var resp = new RespLoginForWeChatDto() { IsNewUser = "false", HomeUrl = ConfigurationUtil.GetAppSettingValue("Domin") + "/index.html?channel=dqappwdcticon" };
            var userId = 0;
            if (dto == null)
            {
                errormsg = "请填写正确信息";
                return resp;
            }
            if (XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(dto.openid) != null)
            {
                Loger.Log4Net.Info("UpdateStatusByOpneId：" + dto.openid);
                ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(0, DateTime.Now, dto.openid);
                var user = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(dto.openid);
                resp.cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
                userId = user.UserID;
            }
            else
            {
                int sex = 0;

                int.TryParse(dto.sex, out sex);
                ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser()
                {
                    unionid = dto.unionid,
                    country = dto.country,
                    openid = dto.openid,
                    nickname = dto.nickname,
                    city = dto.city,
                    province = dto.province,
                    language = dto.language,
                    headimgurl = dto.headimgurl,
                    UserID = 0,
                    sex = sex,
                    Status = 1,
                    LastUpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    AuthorizeTime = DateTime.Now,
                    subscribe_time = DateTime.Now,
                    Source = 3008,
                    RegisterIp = dto.Ip
                };
                bool insertUser = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(wxuser);
                if (insertUser)
                {
                    var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId(dto.openid);
                    resp.cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
                    userId = user.UserID;
                }
                else
                {
                    errormsg = "登录失败，请稍后重试";
                    return resp;
                }
            }
            resp.IsNewUser = Dal.LETask.AppDevice.Instance.IsExist(userId) ? "false" : "true";
            return resp;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string AddUserInfo(ModifyAttestationReqDto dto)
        {
            int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            if (userId <= 0)
            {
                BLL.Loger.Log4Net.Info("请先登录,输出用户Id:" + userId);
                return "请先登录";
            }
            var errorMsg = VerifUserDetail(dto, userId);
            if (errorMsg != "") return errorMsg;

            var mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(dto.Mobile.Trim(), (int)UserCategoryEnum.媒体主, userId);
            List<int> listUserId = new List<int>() { userId };
            if (mUserId > 0 && Dal.WechatUser.Instance.IsWxUser(mUserId))
            {
                return "该手机号已注册，请更换手机号";
            }
            var userInfoAll = new Entities.UserManage.UserInfoAll();
            if (mUserId > 0)
            {
                listUserId.Add(mUserId);
                userInfoAll.UserID = mUserId;
            }
            else
            {
                userInfoAll.UserID = userId;
            }
            if (Dal.UserManage.UserDetailInfo.Instance.IsExistsIdentityNo(listUserId, (int)UserCategoryEnum.媒体主, dto.IdentityNo.Trim()))
                return "您输入的身份证号已绑定其他账户";
            if (Dal.UserManage.UserManage.Instance.IsOtherAccount(dto.AccountType, dto.AccountName.Trim(), listUserId))
                return "该支付宝号已注册，请更换支付宝号";

            userInfoAll.Type = dto.Type;
            userInfoAll.TrueName = dto.TrueName.Trim();
            userInfoAll.IdentityNo = dto.IdentityNo.Trim();
            userInfoAll.Sex = dto.Sex;
            userInfoAll.AccountType = dto.AccountType;
            userInfoAll.AccountName = dto.AccountName;
            userInfoAll.Mobile = dto.Mobile;
            userInfoAll.Status = (int)Entities.UserManage.UserDetailStatus.已认证;

            #region 使用老用户的数据
            //if (mUserId > 0)
            //{
            //    userInfoAll.UserID = mUserId;
            //    var tp = Dal.UserManage.UserManage.Instance.QueryUserBasicInfo(mUserId);
            //    var userDetail = Util.DataTableToEntity<ResAuthenticationinfoDto>(tp.Item2);
            //    var userBank = Util.DataTableToEntity<ResBankaccountinfoDto>(tp.Item3);
            //    if (userDetail != null)
            //    {
            //        if (userDetail.Type > 0)
            //            userInfoAll.Type = userDetail.Type;
            //        if (!string.IsNullOrEmpty(userDetail.TrueName))
            //            userInfoAll.TrueName = userDetail.TrueName;
            //        if (!string.IsNullOrEmpty(userDetail.IdentityNo))
            //            userInfoAll.IdentityNo = userDetail.IdentityNo;
            //        if (userDetail.Sex >= 0)
            //            userInfoAll.Sex = userDetail.Sex;
            //    }
            //    if (userBank != null)
            //    {
            //        userInfoAll.AccountName = userBank.AccountName;
            //        userInfoAll.AccountType = userBank.AccountType;
            //    }
            //}
            #endregion
            errorMsg = Dal.UserManage.UserManage.Instance.AddUserInfo(userInfoAll);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Loger.Log4Net.Error("AddUserInfo->保存用户信息错误：" + errorMsg);
                return "保存失败，请稍后重试！";
            }
            if (mUserId > 0)
            {
                try
                {
                    BLL.Loger.Log4Net.Info($"合并用户：老用户{mUserId};新用户{userId}");
                    if (Dal.UserManage.UserManage.Instance.CleanUserInfo(mUserId, userId))
                    {
                        string QueueName = "";
                        LeadsTask.Instance.CreateQueue(false, out QueueName);
                        MessageQueue MQ = new MessageQueue(QueueName);
                        MQ.Formatter = new XmlMessageFormatter(new System.Type[] { typeof(UpdateUserId) });
                        UpdateUserId updateUserId = new UpdateUserId();
                        updateUserId.Source = 1;
                        updateUserId.OldUserId = mUserId;
                        updateUserId.NewUserId = userId;
                        MQ.Send(updateUserId);
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(mUserId);
                        return "";
                    }
                    else
                    {
                        return "保存失败，请稍后重试！";
                    }
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error($"合并用户保存失败：", ex);
                    return "保存失败，请稍后重试！";
                }
            }
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            return "";
        }

        private static string VerifUserDetail(ModifyAttestationReqDto dto, int userId)
        {
            var strJson = Json.JsonSerializerBySingleData(dto);
            Loger.Log4Net.Info("保存用户参数：" + strJson);
            if (dto == null)
                return "参数错误";
            if (!Enum.IsDefined(typeof(UserBankAccountTypeEnum), dto.AccountType))
                return "参数错误";

            var userInfoAll = Dal.UserManage.UserInfo.Instance.GetModel(userId);
            if (userInfoAll != null)
            {
                if (string.IsNullOrEmpty(userInfoAll.Mobile))
                {
                    if (string.IsNullOrWhiteSpace(dto.Mobile))
                        return "请填写手机号！";
                    if (!BLL.Util.IsHandset(dto.Mobile))
                        return "请填写正确的手机号";
                    if (BLL.Util.GetMobileCheckCodeByCache(dto.Mobile) != dto.CheckCode.ToString())
                        return "您输入的手机短信验证码不正确";
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
            }
            dto.Type = (int)UserTypeEnum.个人;
            if (string.IsNullOrWhiteSpace(dto.TrueName))
                return "请填写姓名！";
            if (dto.Sex != 0 && dto.Sex != 1)
                return "请选择性别";
            if (string.IsNullOrWhiteSpace(dto.IdentityNo))
                return "请输入身份证号码！";
            if (string.IsNullOrWhiteSpace(dto.AccountName))
                return "请填写支付宝账号！";
            if (!BLL.Util.CheckIDCard(dto.IdentityNo.Trim()))
                return "请输入正确的身份证号码";
            if (!BLL.Util.IsHandset(dto.AccountName.Trim()) && !BLL.Util.IsEmail(dto.AccountName.Trim()))
                return "支付宝账号格式不正确!";
            return "";
        }

        #endregion

        #region 绑定微信按钮显示相关

        public ReturnValue GetWxBindConfig(int userId)
        {
            var retValue = new ReturnValue();
            var isShowBind = false;
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(userId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "10052", $"用户不存在：{userId}");
            }
            //todo:用户存在手机号+没有微信关联用户 可以绑定微信
            var wxUser = BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
            if (wxUser == null && !string.IsNullOrWhiteSpace(userInfo.Mobile))
            {
                isShowBind = true;
            }
            retValue.ReturnObject = new { IsOpen = isShowBind };
            return retValue;
        }


        public ReturnValue WxBind(ReqLoginDTO dto, int userId)
        {
            var retValue = new ReturnValue();

            if (dto == null)
            {
                return CreateFailMessage(retValue, "2001", "请输入参数信息");
            }
            if (userId <= 0)
            {
                return CreateFailMessage(retValue, "2001", "未检测到用户id");
            }
            //todo:理论上只处理新的微信用户
            var userInfo = Dal.UserManage.LE_WeiXinUser.Instance.GetModel(dto.openid);
            if (userInfo != null)
            {
                if (userInfo.UserID == userId)
                {
                    return CreateFailMessage(retValue, "2002", "当前微信已存在,不能重复绑定");
                }
                if (userInfo.UserID != userId && userInfo.UserID > 0)
                {
                    return CreateFailMessage(retValue, "2003", "当前微信已被别人绑定");
                }
            }
            //todo:理论上是不存在LE_WeiXinUser 里的UserID为0的情况，所以不需要进行update 当前userId
            int sex = 0;
            int.TryParse(dto.sex, out sex);
            var lewxId = Dal.UserManage.LE_WeiXinUser.Instance.Insert(new Entities.UserManage.LE_WeiXinUser()
            {
                unionid = dto.unionid,
                country = dto.country,
                openid = dto.openid,
                nickname = dto.nickname,
                city = dto.city,
                province = dto.province,
                language = dto.language,
                headimgurl = dto.headimgurl,
                UserID = userId,
                sex = sex,
                Status = 1,
                LastUpdateTime = DateTime.Now,
                CreateTime = DateTime.Now,
                AuthorizeTime = DateTime.Now,
                Source = 3008,
            });
            if (lewxId <= 0)
            {
                return CreateFailMessage(retValue, "2003", "绑定失败，请稍后再试");
            }

            return retValue;
        }


        #endregion

    }
}
