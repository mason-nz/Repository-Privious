using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.User;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    public class UserProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqUserDto _reqUserDto;

        public UserProvider(ConfigEntity configEntity, ReqUserDto reqUserDto)
        {
            _configEntity = configEntity;
            _reqUserDto = reqUserDto;
        }

        #region 绑定合并用户相关

        /// <summary>
        /// 用户合并操作(用户首次绑定手机号)
        /// </summary>
        /// <returns></returns>
        public ReturnValue ModifyAttestation()
        {
            #region 校验流程

            //todo:
            /*
            1.先判断输入的手机号是否已经存在（排除当前userId），返回用户信息mUser
            2.判断 mUser 是否存在
                1.存在
                    1.判断 mUser 是否有微信用户关联
                        1.有微信关联     return：该手机号码已注册，请更换手机号
                        2.没有关联       return：该手机号已经注册，是否做信息合并【修改手机号】【合并信息】
                2.不存在
                    1.属于新用户，直接修改手机号，和所在城市信息

            【修改手机号】：点击此按钮，前端引导用户更换手机号
            【合并信息】：点击此按钮，直接进行合并。成功之后，继续补充所在城市信息
            */

            #endregion

            return BindUser(_configEntity.CreateUserId);
        }

        /// <summary>
        /// 首次绑定
        /// </summary>
        /// <returns></returns>
        private ReturnValue BindUser(int currentUserId)
        {
            //todo:校验基本信息-:验证手机验证码
            var retValue = VerifyModifyAttestation();
            if (retValue.HasError)
                return retValue;
            //todo:校验是否合并用户信息(是否具备合并用户的条件)
            retValue = VerifyMobileBind();
            if (!retValue.HasError)
            {
                return CreateFailMessage(retValue, "50005", "当前信息不满足合并的要求，请直接修改信息");
            }
            if (retValue.ErrorCode != ((int)BindUserTips.该手机号已经注册是否做信息合并).ToString())
            {
                return CreateFailMessage(retValue, "50006", "当前信息不满足合并的要求，请直接修改信息");
            }
            //todo:满足合并用户要求
            var oldUserId = Convert.ToInt32(retValue.ReturnObject);
            var cleanResult = Dal.UserManage.UserManage.Instance.CleanUserInfoPc(oldUserId, currentUserId, _reqUserDto.ProvinceId, _reqUserDto.CityId, _reqUserDto.Address);
            if (!cleanResult)
            {
                Loger.Log4Net.Error($" BindUser CleanUserInfoPc 错误。参数：oldUserId={oldUserId},newUserId={currentUserId},ProvinceId={_reqUserDto.ProvinceId}" +
                                    $",CityId={ _reqUserDto.CityId},Address={_reqUserDto.Address}");
                return CreateFailMessage(retValue, "50007", "操作失败");
            }
            //todo:发送消息到消息队列
            PushCleanUserMessage(oldUserId, currentUserId);
            //todo:模拟用户登录



            return CreateSuccessMessage(retValue, "0", "success");
        }

        /// <summary>
        /// 发送消息到消息队列
        /// </summary>
        /// <param name="oldUserId"></param>
        /// <param name="newUserId"></param>
        private void PushCleanUserMessage(int oldUserId, int newUserId)
        {
            string queueName = "";
            LeadsTask.Instance.CreateQueue(false, out queueName);
            MessageQueue MQ = new MessageQueue(queueName);
            MQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(UpdateUserId) });
            UpdateUserId updateUserId = new UpdateUserId();
            updateUserId.Source = 1;
            updateUserId.OldUserId = oldUserId;
            updateUserId.NewUserId = newUserId;
            MQ.Send(updateUserId);
        }

        #endregion

        #region 直接修改用户信息（不满足合并，属于新用户）

        /// <summary>
        /// 更改
        /// </summary>
        /// <returns></returns>
        public ReturnValue UpdateUser()
        {
            //todo:校验基本信息-:验证手机验证码
            var retValue = VerifyModifyAttestation();
            if (retValue.HasError)
                return retValue;
            //todo:校验用户信息，满足什么条件（1.合并 2.新用户更改信息）
            retValue = VerifyMobileBind();
            if (retValue.HasError)
            {
                return CreateFailMessage(retValue, "50015", "当前信息不满足直接修改操作");
            }
            //todo:直接修改信息
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(_configEntity.CreateUserId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "50016", "用户信息不存在");
            }
            if (!string.IsNullOrWhiteSpace(userInfo.Mobile.Trim()))
            {
                return CreateFailMessage(retValue, "50017", $"当前用户已存在手机号：【{userInfo.Mobile}】");
            }
            var excuteCount = Dal.UserDetailInfo.Instance.FirstUpdateUserInfo(userInfo.UserID, _reqUserDto.Mobile, _reqUserDto.ProvinceId,
                  _reqUserDto.CityId, _reqUserDto.Address);
            if (excuteCount <= 0)
            {
                return CreateFailMessage(retValue, "50018", "用户信息补充失败");
            }


            ClearPassport(userInfo.UserID);

            return retValue;
        }

        /// <summary>
        /// todo:模拟用户登录
        /// </summary>
        private void ClearPassport(int userId)
        {
            Loger.Log4Net.Info($"ClearPassport userId:{userId}");
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
            var cookie = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            Loger.Log4Net.Info($"ClearPassport userId:{userId},Passport cookie:{cookie}");
        }

        /// <summary>
        /// 校验基本信息-:验证手机验证码
        /// </summary>
        /// <returns></returns>
        private ReturnValue VerifyModifyAttestation()
        {
            var retValue = VerifyOfNecessaryParameters<ReqUserDto>(_reqUserDto);
            if (retValue.HasError)
            {
                return retValue;
            }
            if (!BLL.Util.IsHandset(_reqUserDto.Mobile))
            {
                return CreateFailMessage(retValue, "1001", "请输入正确的手机号");
            }

            //retValue = VerifyMsgCode(retValue);
            //if (retValue.HasError)
            //{
            //    return retValue;
            //}
            return retValue;
        }

        private ReturnValue VerifyMsgCode(ReturnValue retValue)
        {
            //todo:验证手机验证码
            var code = BLL.Util.GetMobileCheckCodeByCache(_reqUserDto.Mobile);
            if (string.IsNullOrWhiteSpace(code))
                return CreateFailMessage(retValue, "50001", "校验验证码异常");
            if (!_reqUserDto.MobileCode.Equals(code))
            {
                return CreateFailMessage(retValue, "50002", "短信验证码错误");
            }
            return retValue;
        }

        #endregion

        #region 校验手机号，是否需要合并用户信息

        /// <summary>
        /// 校验是否合并用户信息
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyMobileBind()
        {
            //todo:输入手机号验证码通过之后才进行此验证
            var retValue = new ReturnValue();
            var category = _configEntity.LoginUser.Category;
            UserCategoryEnum userCategoryEnum;
            if (!Enum.TryParse(category.ToString(), out userCategoryEnum))
            {
                return CreateFailMessage(retValue, "6000", "用户分类角色错误");
            }
            //用当前手机号查询是否已经存在（排除当前userId）
            int mUserId = Dal.UserManage.UserInfo.Instance.GetUserIDByMobile(_reqUserDto.Mobile.Trim(), category, _configEntity.CreateUserId);

            if (mUserId > 0)
            {
                //1.存在-->1.判断 mUser 是否有微信用户关联
                var weixinUser = Dal.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(mUserId, userCategoryEnum);
                if (weixinUser != null)
                {
                    //有微信关联     return：该手机号码已注册，请更换手机号
                    return CreateFailMessage(retValue, ((int)BindUserTips.该手机号码已注册请更换手机号).ToString(), "该手机号码已注册，请更换手机号", mUserId);
                }
                else
                {
                    return CreateFailMessage(retValue, ((int)BindUserTips.该手机号已经注册是否做信息合并).ToString(), "该手机号已经注册，是否做信息合并", mUserId);
                }
            }
            return retValue;
        }

        #endregion

        #region 校验用户是否存在密码

        /// <summary>
        /// 校验用户是否存在密码
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyUserPassword(ReturnValue retValue)
        {
            retValue = retValue ?? new ReturnValue();
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(_configEntity.CreateUserId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "50010", "用户信息不存在");
            }
            if (!string.IsNullOrWhiteSpace(userInfo.Pwd))
            {
                return CreateFailMessage(retValue, "50011", "修改密码");
            }
            return CreateSuccessMessage(retValue, "0", "补充密码");
        }

        #endregion

        #region 创建密码相关

        /// <summary>
        /// 首次添加密码
        /// </summary>
        /// <returns></returns>
        public ReturnValue AddPassword(ReqUserPasswordDto reqUserPasswordDto)
        {
            var retValue = VerifyAddPassword(reqUserPasswordDto);
            //todo:校验密码的规则
            retValue = VerifyUserPassword(retValue);
            if (retValue.HasError)
                return retValue;
            //todo:入库密码（修改密码）
            //todo:md5
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(_configEntity.CreateUserId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "50010", "用户信息不存在");
            }
            var pwd = SetPwd(reqUserPasswordDto.Password, userInfo.Category);
            var excuteCount = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.UpdateUserPwd(_configEntity.CreateUserId, pwd);
            if (excuteCount <= 0)
            {
                return CreateFailMessage(retValue, "50010", "创建密码失败");
            }

            return retValue;
        }

        /// <summary>
        /// md5 加密
        /// </summary>
        /// <param name="requestPwd"></param>
        /// <param name="requestCategory"></param>
        /// <returns></returns>
        private string SetPwd(string requestPwd, int requestCategory)
        {
            string loginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
            return XYAuto.Utils.Security.DESEncryptor.MD5Hash(requestPwd + requestCategory + loginPwdKey, System.Text.Encoding.UTF8);
        }

        private ReturnValue VerifyAddPassword(ReqUserPasswordDto reqUserPasswordDto)
        {
            var retValue = VerifyOfNecessaryParameters<ReqUserPasswordDto>(reqUserPasswordDto);
            if (retValue.HasError)
            {
                return retValue;
            }

            if (!reqUserPasswordDto.Password.Equals(reqUserPasswordDto.PasswordAgain))
            {
                return CreateFailMessage(retValue, "50020", "两次输入的密码不一致");
            }
            return retValue;
        }

        #endregion

    }

    public enum BindUserTips
    {
        该手机号码已注册请更换手机号 = 60001,
        该手机号已经注册是否做信息合并 = 60002,
    }

}
