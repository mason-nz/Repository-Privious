using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin.EntityUtility;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Kr;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.ITSC.Chitunion2017.WebService.Common;
using XYAuto.Utils.Config;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Exceptions;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:收入管理提现
    /// </summary>
    public class WithdrawalsProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqWithdrawalsAgainDto _reqWithdrawalsAgainDto;
        private readonly ReqWithdrawalsDto _reqWithdrawalsDto;

        public WithdrawalsProvider(ConfigEntity configEntity, ReqWithdrawalsDto reqWithdrawalsDto)
        {
            _configEntity = configEntity;
            _reqWithdrawalsDto = reqWithdrawalsDto;
        }

        public WithdrawalsProvider(ConfigEntity configEntity, ReqWithdrawalsAgainDto reqWithdrawalsAgainDto)
        {
            _configEntity = configEntity;
            _reqWithdrawalsAgainDto = reqWithdrawalsAgainDto;
        }

        #region 提现操作相关


        /// <summary>
        /// 计算提现金额
        /// </summary>
        /// <returns></returns>
        public RespWithdrawalsPriceClc PriceCalc()
        {
            /*
            个税计算公式
                1. 当月累计提现金额 指1号到31号的累计提现金额
                2. 当月累计提现金额小于等于800元时，不计算个税
                3. 当月累计提现金额小于等于4000元的，扣除费用为800元
                    应纳税所得额 = 当月累计提现金额-800
                    应纳税额 =  应纳税所得额 * 20% - 当月累计已纳税金额
                4. 每月累计提现金额大于4000元的，扣除费用为累计提现金额的20%
                    应纳税所得额 = 当月累计提现金额 - 当月累计提现金额*20%
                    应纳税额 = 应纳税所得额 * 适用税率 - 速算扣除数 - 当月累计已纳税金额
                可参考网址  http://www.hnds.gov.cn/taxcalculate/reward.html
                        http://www.taxspirit.com/017.aspx 

            */

            var respPriceClc = new RespWithdrawalsPriceClc();

            //todo
            //1.先找到当前用户这个月申请的总金额（包括体现中，已提现的）

            var startDay = DateTime.Now.FirstDayOfMonth().ToString("yyyy-MM-dd");
            var lastDay = DateTime.Now.LastDayOfMonth().ToString("yyyy-MM-dd");

            var totalMoneyInfo = Dal.LETask.LeWithdrawalsDetail.Instance.GetMonthOfWithdrawalsMoney(_configEntity.CreateUserId, startDay,
                  endDay: lastDay);
            //累计提现金额（数据库已提现的金额+当前这次的金额）
            respPriceClc.IndividualTaxPeice =
                GetIndividualTaxPeice(totalMoneyInfo.WithdrawalsPrice + _reqWithdrawalsDto.WithdrawalsPrice, totalMoneyInfo.IndividualTaxPeice);
            respPriceClc.PracticalPrice = _reqWithdrawalsDto.WithdrawalsPrice - respPriceClc.IndividualTaxPeice;
            respPriceClc.WithdrawalsPrice = _reqWithdrawalsDto.WithdrawalsPrice;


            return respPriceClc;
        }

        /// <summary>
        /// 个税计算规则
        /// </summary>
        /// <param name="totalMoney"></param>
        /// <param name="individualTaxPeice"></param>
        /// <returns></returns>
        public decimal GetIndividualTaxPeice(decimal totalMoney, decimal individualTaxPeice)
        {
            if (totalMoney < 800)
            {
                return 0;
            }
            else if (totalMoney <= 4000)
            {
                return (totalMoney - 800) * 0.2m - individualTaxPeice;// 应纳税额 =  应纳税所得额 * 20% - 当月累计已纳税金额
            }
            else
            {
                var monthMoeny = (totalMoney - totalMoney * 0.2m);

                if (monthMoeny <= 20000)
                {
                    return monthMoeny * 0.2m - individualTaxPeice;
                }
                else if (totalMoney <= 50000)
                {
                    return monthMoeny * 0.3m - 2000 - individualTaxPeice;
                }
                else //if (totalMoney > 50000)
                {
                    return monthMoeny * 0.4m - 7000 - individualTaxPeice;
                }
            }
        }

        /// <summary>
        /// 提现操作-校验相关信息
        /// </summary>
        /// <returns></returns>
        private ReturnValue WithdrawalsVerify()
        {
            var configWithdrawalsPrice = ConfigurationUtil.GetAppSettingValue("ConfigWithdrawalsPrice", true);
            var retValue = VerifyOfNecessaryParameters(_reqWithdrawalsDto);
            if (retValue.HasError)
                return retValue;
            if (_reqWithdrawalsDto.ApplySource == WithdrawalsApplySourceEnum.None)
            {
                return CreateFailMessage(retValue, "3019", "提现申请渠道错误");
            }

            if (configWithdrawalsPrice.ToDecimal(0) <= 0)
            {
                throw new Exception("提现金额配置错误");
            }
            //添加黑名单验证逻辑,begin================(add=masj,Date=2018-04-17)
            //string ip = TaskProvider.GetIP();
            string ip = ITSC.Chitunion2017.BLL.Util.GetIP($"用户{ _configEntity.CreateUserId}提现申请");
            BLL.LETask.Provider.TaskProvider pro = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto());
            retValue = pro.VerifyAntiCheating(retValue, ip, _configEntity.CreateUserId);
            if (retValue.HasError)
            {
                Loger.Log4Net.Info($"提现申请VerifyAntiCheating:{JsonConvert.SerializeObject(retValue)},userId:{_configEntity.CreateUserId},ip;{ip}");
                return CreateFailMessage(retValue, "0", "success");
            }
            //添加黑名单验证逻辑,end==================
            if (_reqWithdrawalsDto.WithdrawalsPrice < configWithdrawalsPrice.ToDecimal())
            {
                return CreateFailMessage(retValue, "3020", $"可提现金额不足{configWithdrawalsPrice}元，无法提现！");
            }
            var resultCode = Dal.LETask.LeWithdrawalsStatistics.Instance.VerifyWithdrawals(_configEntity.CreateUserId,
                 string.Empty, _reqWithdrawalsDto.WithdrawalsPrice);

            if (resultCode != null && resultCode.ResultCode > 0)
            {
                return CreateFailMessage(retValue, resultCode.ResultCode.ToString(),
                    GetVerifyMessage(resultCode.ResultCode));
            }

            return retValue;
        }

        /// <summary>
        /// 提现操作
        /// </summary>
        /// <returns></returns>
        public ReturnValue Withdrawals()
        {
            var retValue = WithdrawalsVerify();
            if (retValue.HasError)
            {
                return retValue;
            }

            //if (!VerifyMsgCode())
            //{
            //    //return CreateFailMessage(retValue, "3005", "短信验证码不正确");
            //}

            return PostWithdrawals(retValue);
        }


        public ReturnValue PostWithdrawals(ReturnValue retValue)
        {
            //todo:个人用户需要计算个税，企业用户不需要
            decimal individualTaxPeice = 0;
            decimal practicalPrice = _reqWithdrawalsDto.WithdrawalsPrice;//企业用户不需要支付个税
            if (_configEntity.UserType == UserTypeEnum.个人)
            {
                var priceClc = PriceCalc();
                individualTaxPeice = priceClc.IndividualTaxPeice;
                practicalPrice = priceClc.PracticalPrice;
            }
            var accountBank = Dal.LETask.LeUserBankAccount.Instance.GetInfo(_configEntity.CreateUserId)
                .FirstOrDefault(s => s.AccountType == (int)UserBankAccountTypeEnum.Zfb);
            if (accountBank == null)
            {
                return CreateFailMessage(retValue, "3001", "暂未查询到提现帐号");
            }
            var withraw = new LeWithdrawalsDetail()
            {
                WithdrawalsPrice = _reqWithdrawalsDto.WithdrawalsPrice, //提现金额
                IndividualTaxPeice = individualTaxPeice, //个税金额
                PracticalPrice = practicalPrice, //实际付款
                PayeeAccount = accountBank.AccountName,
                Status = (int)WithdrawalsStatusEnum.支付中,
                ApplicationDate = DateTime.Now,
                PayeeID = _configEntity.CreateUserId,
                CreateTime = DateTime.Now,
                AuditStatus = (int)WithdrawalsAuditStatusEnum.待审核,
                ApplySource = (int)_reqWithdrawalsDto.ApplySource,
                //SyncPayStatus = (int)WithdrawalsStatusEnum.支付中
            };
            var excuteId = Dal.LETask.LeWithdrawalsDetail.Instance.Insert(withraw);

            if (excuteId <= 0)
            {
                Loger.Log4Net.Info($"提现失败，参数:{JsonConvert.SerializeObject(withraw)}");
                return CreateFailMessage(retValue, "3002", $"提现失败");
            }
            //todo:冻结资金(LE_WithdrawalsStatistics 统计表，提现中 + 当前金额，余额减去当前金额)
            var resultId = Dal.LETask.LeWithdrawalsStatistics.Instance.UpdateWithdrawalsStatistics(_configEntity.CreateUserId, withraw.WithdrawalsPrice);
            if (resultId <= 0)
            {
                Loger.Log4Net.Info($"提现-冻结资金失败，参数:userId:{_configEntity.CreateUserId},money:{withraw.WithdrawalsPrice}");
                return CreateFailMessage(retValue, "3003", $"冻结资金失败");
            }
            retValue.ReturnObject = excuteId;
            return retValue;
        }

        /// <summary>
        /// 再次提交提现申请
        /// </summary>
        /// <returns></returns>
        public ReturnValue WithdrawalsAgain()
        {
            //todo:
            //1.只能修改提现帐号（修改2处，引导用户去修改绑定提现帐号，回来接着修改提现申请的帐号：现在只有支付宝），金额不能修改
            //2.修改提现申请支付失败--> 为支付中
            //3.审核信息添加一条再次提现申请
            var retValue = VerifyWithdrawalsAgain();
            if (retValue.HasError)
                return retValue;
            var info = retValue.ReturnObject as Entities.LETask.LeWithdrawalsDetail;
            if (info == null)
            {
                return CreateFailMessage(retValue, "3019", "数据异常");
            }
            //todo:冻结资金(LE_WithdrawalsStatistics 统计表，提现中 + 当前金额，余额减去当前金额)
            var excuteCount =
                Dal.LETask.LeWithdrawalsDetail.Instance.UpdateApplyAgain(_reqWithdrawalsAgainDto.WithdrawalsId,
                   info.WithdrawalsPrice, _configEntity.CreateUserId);

            if (excuteCount <= 0)
            {
                Loger.Log4Net.Error($"WithdrawalsAgain-申请失败:WithdrawalsId={_reqWithdrawalsAgainDto.WithdrawalsId},userId={ _configEntity.CreateUserId}");
                return CreateFailMessage(retValue, "3020", "申请失败");
            }

            //添加审核日志
            Task.Run(() => { UpdateAuditInfo(_reqWithdrawalsAgainDto.WithdrawalsId, WithdrawalsAuditStatusEnum.待审核, _configEntity.CreateUserId); });

            return retValue;
        }

        public ReturnValue VerifyWithdrawalsAgain()
        {
            var retValue = VerifyOfNecessaryParameters(_reqWithdrawalsAgainDto);
            if (retValue.HasError)
                return retValue;
            var withdrawalsDetail = Dal.LETask.LeWithdrawalsDetail.Instance.GetInfo(_reqWithdrawalsAgainDto.WithdrawalsId);

            if (withdrawalsDetail == null)
            {
                return CreateFailMessage(retValue, "3020", "此提现信息不存在");
            }
            if (withdrawalsDetail.PayeeID != _configEntity.CreateUserId)
            {
                return CreateFailMessage(retValue, "3022", "此提现信息不属于您，没有权限发起提现申请");
            }
            if (withdrawalsDetail.AuditStatus != (int)WithdrawalsAuditStatusEnum.驳回)
            {
                return CreateFailMessage(retValue, "3021", "此提现审核状态不满足再次提现操作要求");
            }
            retValue.ReturnObject = withdrawalsDetail;

            //执行一次申请流程
            var resultCode = Dal.LETask.LeWithdrawalsStatistics.Instance.VerifyWithdrawals(_configEntity.CreateUserId,
               string.Empty, _reqWithdrawalsDto.WithdrawalsPrice);

            if (resultCode != null && resultCode.ResultCode > 0)
            {
                return CreateFailMessage(retValue, resultCode.ResultCode.ToString(),
                    GetVerifyMessage(resultCode.ResultCode));
            }

            return retValue;
        }

        /// <summary>
        /// 校验短信验证码
        /// </summary>
        /// <returns></returns>
        private bool VerifyMsgCode()
        {
            return _reqWithdrawalsDto.MsgCode.Equals(BLL.Util.GetMobileCheckCodeByCache(_reqWithdrawalsDto.Mobile));
        }

        public static string GetVerifyMessage(int key)
        {
            var dicError = new Dictionary<int, string>()
            {
                { 0,"验证通过"},
                { 1011,"请先补充认证信息！"},
                { 1012,"认证信息审核未通过，请修改认证信息！"},
                { 1013,"请到账号管理中完善提现账号！"},
                { 1014,"用户提现手机号与注册手机号不一致！"},
                { 1015,"可提现金额不足，无法提现！"},
                { 1016,"每天只能提现1次，请明天再试！"},
                { 1017,"您有正在支付中的提现申请，请在提现完成后再申请！"},
                { 1018,"还未绑定手机号！"}
            };
            return dicError.Where(s => s.Key == key).Select(s => s.Value).FirstOrDefault() ?? "校验未通过！";
        }


        private static string GetVerifyAuditMessage(int key)
        {
            var dicError = new Dictionary<int, string>()
            {
                { 0,"验证通过"},
                { 1011,"请先补充认证信息！"},
                { 1012,"认证信息审核未通过，请修改认证信息！"},
                { 1013,"请到账号管理中完善提现账号！"},
                //{ 1014,"用户提现手机号与注册手机号不一致"},
                { 1015,"可提现金额不足，无法提现(对账错误)"},
            };
            return dicError.Where(s => s.Key == key).Select(s => s.Value).FirstOrDefault() ?? "校验未通过！";
        }

        #endregion

        #region 详情相关

        /// <summary>
        /// 提现管理-提现详情()
        /// </summary>
        /// <returns></returns>
        public RespWithdrawalsDetailsInfoDto GetWithdrawals(int recId)
        {
            var respDto = new RespWithdrawalsDetailsInfoDto();
            if (recId <= 0)
                return respDto;
            var infoTp = Dal.LETask.LeWithdrawalsDetail.Instance.GetInfoByAuditInfo(recId);
            if (infoTp.Item1 == null)
            {
                return respDto;
            }

            respDto.WithdrawalsInfo = AutoMapper.Mapper.Map<Entities.LETask.LeWithdrawalsDetail, RespWithdrawalsInfoDto>(infoTp.Item1);

            //todo:缺少审核信息
            if (infoTp.Item2 != null)
            {
                respDto.AuditInfo = new RespAuditInfoDto()
                {
                    AuditTime = infoTp.Item2.CreateTime,
                    AuditUser = infoTp.Item2.CreateUserName
                };
            }

            return respDto;
        }

        /// <summary>
        /// 获取审核操作详情
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public RespWithdrawalsAuditDetailDto GetAuditDetails(int recId)
        {
            var respDto = new RespWithdrawalsAuditDetailDto();
            if (recId <= 0)
                return respDto;
            var info = Dal.LETask.LeWithdrawalsDetail.Instance.GetAuditDetail(recId);
            if (info == null)
            {
                return respDto;
            }
            else
            {
                var resp = AutoMapper.Mapper.Map<Entities.LETask.LeWithdrawalsDetail, RespWithdrawalsAuditDetailDto>(info);
                resp.IsInsideEmployee = Dal.LETask.EmployeeInfo.Instance.IsExistByUserID(resp.PayeeID);
                return resp;
            }
        }


        #endregion

        #region 支付宝认证

        /// <summary>
        /// 支付宝认证-首次
        /// </summary>
        /// <param name="reqFillWithdrawalsInfoDto"></param>
        /// <returns></returns>
        public ReturnValue FirstFillWithdrawalsAccount(ReqFillWithdrawalsInfoDto reqFillWithdrawalsInfoDto)
        {
            return FillWithdrawalsAccount(reqFillWithdrawalsInfoDto, true);
        }

        /// <summary>
        /// 支付宝认证-更改信息
        /// </summary>
        /// <param name="reqFillWithdrawalsInfoDto"></param>
        /// <returns></returns>
        public ReturnValue UpdateWithdrawalsAccount(ReqFillWithdrawalsInfoDto reqFillWithdrawalsInfoDto)
        {
            return FillWithdrawalsAccount(reqFillWithdrawalsInfoDto, false);
        }

        /// <summary>
        /// 帐号管理-提现帐号编辑
        /// </summary>
        /// <param name="reqFillWithdrawalsInfoDto"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        private ReturnValue FillWithdrawalsAccount(ReqFillWithdrawalsInfoDto reqFillWithdrawalsInfoDto, bool isFirst)
        {
            var retValue = VerifyOfNecessaryParameters(reqFillWithdrawalsInfoDto);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyUserMobile();
            if (retValue.HasError)
                return retValue;
            if (!reqFillWithdrawalsInfoDto.AccountName.Equals(reqFillWithdrawalsInfoDto.AccountNameAgain))
            {
                return CreateFailMessage(retValue, "3001", "两次输入的帐号不一致");
            }

            if (!isFirst)
            {
                if (string.IsNullOrWhiteSpace(reqFillWithdrawalsInfoDto.OldAccountName))
                {
                    return CreateFailMessage(retValue, "3033", "请输入旧支付宝帐号");
                }
                //查询原支付宝帐号
                var oldAccountList = Dal.LETask.LeUserBankAccount.Instance.GetInfo(reqFillWithdrawalsInfoDto.OldAccountName.ToSqlFilter(),
                    UserBankAccountTypeEnum.Zfb, _configEntity.CreateUserId);
                if (oldAccountList.Count > 1)
                {
                    return CreateFailMessage(retValue, "3003", "旧的帐号异常【存在多个】");
                }
            }

            //查询新的帐号是否存在(全局唯一)
            var accountList = Dal.LETask.LeUserBankAccount.Instance.GetInfo(reqFillWithdrawalsInfoDto.AccountName.ToSqlFilter(),
                UserBankAccountTypeEnum.Zfb, 0);

            if (accountList.Any())
            {
                return CreateFailMessage(retValue, "3002", "支付宝账号已存在，请更换");
            }
            var updateInfo = new LeUserBankAccount()
            {
                UserID = _configEntity.CreateUserId,
                AccountName = reqFillWithdrawalsInfoDto.AccountName,
                AccountType = reqFillWithdrawalsInfoDto.AccountType,
                CreateTime = DateTime.Now,
                Status = 0
            };
            var excuteId = Dal.LETask.LeUserBankAccount.Instance.Update(updateInfo);
            if (excuteId == 0)
            {
                Loger.Log4Net.Error($"FillWithdrawalsAccount 更改失败：{JsonConvert.SerializeObject(updateInfo)}");
                return CreateFailMessage(retValue, "3003", "更改失败");
            }
            return retValue;
        }

        public ReturnValue VerifyUserMobile()
        {
            var retValue = new ReturnValue();
            var userInfo = XYAuto.ITSC.Chitunion2017.Dal.UserDetailInfo.Instance.GetUserInfo(_configEntity.CreateUserId);
            if (userInfo == null)
            {
                return CreateFailMessage(retValue, "3034", "此用户不存在");
            }
            if (string.IsNullOrWhiteSpace(userInfo.Mobile.Trim()))
            {
                return CreateFailMessage(retValue, "3035", "请先完善手机号");
            }
            return retValue;
        }

        #endregion

        #region 提现审核操作相关

        public ReturnValue Audit(ReqWithdrawalsAuditDto reqWithdrawalsAuditDto)
        {
            //todo:
            //1.此操作是运营才有的权限
            //2.调用资金管理系统的接口进行付款；根据返回结果更新状态；
            //3.如果成功，更改提现状态成功
            //4.如果失败，将冻结的资金回滚（1.将LE_WithdrawalsStatistics提现中字段的金额-提现金额 2.将余额+提现金额），更改提现状态失败，记录失败原因
            var retValue = VerifyOfNecessaryParameters(reqWithdrawalsAuditDto);
            if (retValue.HasError)
                return retValue;
            if (reqWithdrawalsAuditDto.AuditStatus == WithdrawalsAuditStatusEnum.驳回)
            {
                return AuditNotPass(reqWithdrawalsAuditDto);
            }
            else if (reqWithdrawalsAuditDto.AuditStatus == WithdrawalsAuditStatusEnum.通过)
            {
                return AuditPass(reqWithdrawalsAuditDto);
            }
            else
            {
                return CreateFailMessage(retValue, "3031", "请作出审核");
            }
        }

        private ReturnValue VerifyAudit(ReqWithdrawalsAuditDto reqWithdrawalsAuditDto,
            out Entities.LETask.LeWithdrawalsDetail withdrawalsDetail, out string identityNo)
        {
            withdrawalsDetail = null;
            identityNo = string.Empty;
            var retValue = VerifyOfNecessaryParameters(reqWithdrawalsAuditDto);
            if (retValue.HasError)
                return retValue;

            withdrawalsDetail = Dal.LETask.LeWithdrawalsDetail.Instance.GetInfo(reqWithdrawalsAuditDto.WithdrawalsId);
            if (withdrawalsDetail == null)
            {
                return CreateFailMessage(retValue, "3010", "此提现信息不存在");
            }
            if (withdrawalsDetail.IsLock)
            {
                return CreateFailMessage(retValue, "3020", "此提现申请单-被【锁定】请联系相关人员解锁");
            }
            if (withdrawalsDetail.Status != (int)WithdrawalsStatusEnum.支付中)
            {
                return CreateFailMessage(retValue, "3011", "此提现信息状态不满足审核操作要求（不是【支付中】状态）");
            }
            if (withdrawalsDetail.SyncPayStatus == (int)WithdrawalsStatusEnum.支付中)
            {
                return CreateFailMessage(retValue, "3012", "此提现审核正在进行中，请稍后");
            }
            //这里的用户不是当前用户是，申请人的信息
            //校验申请时候的条件规则
            var resultCodeTp = Dal.LETask.LeWithdrawalsStatistics.Instance.VerifyWithdrawalsAudit(withdrawalsDetail.PayeeID, withdrawalsDetail.WithdrawalsPrice);
            if (resultCodeTp.Item1 != null && resultCodeTp.Item1.ResultCode > 0)
            {
                return CreateFailMessage(retValue, resultCodeTp.Item1.ResultCode.ToString(),
                    GetVerifyAuditMessage(resultCodeTp.Item1.ResultCode));
            }
            //获取身份证号码
            identityNo = resultCodeTp.Item2.IdentityNo;
            _configEntity.LoginUser = new LoginUser()
            {
                Type = resultCodeTp.Item2.Type,
                UserID = resultCodeTp.Item2.UserID,
                Mobile = resultCodeTp.Item2.Mobile
            };
            return retValue;
        }

        private ReturnValue VerifyAuditNotPass(ReqWithdrawalsAuditDto reqWithdrawalsAuditDto)
        {
            var retValue = VerifyOfNecessaryParameters(reqWithdrawalsAuditDto);
            if (retValue.HasError)
                return retValue;
            var withdrawalsDetail = Dal.LETask.LeWithdrawalsDetail.Instance.GetInfo(reqWithdrawalsAuditDto.WithdrawalsId);
            if (withdrawalsDetail == null)
            {
                return CreateFailMessage(retValue, "3010", "此提现信息不存在");
            }
            if (string.IsNullOrWhiteSpace(reqWithdrawalsAuditDto.RejectMsg))
            {
                return CreateFailMessage(retValue, "3011", "请输入驳回原因");
            }
            if (withdrawalsDetail.AuditStatus != (int)WithdrawalsAuditStatusEnum.待审核)
            {
                return CreateFailMessage(retValue, "3021", "此提现审核状态不满足再次提现操作要求");
            }
            retValue.ReturnObject = withdrawalsDetail;
            return retValue;
        }

        private ReturnValue AuditNotPass(ReqWithdrawalsAuditDto reqWithdrawalsAuditDto)
        {
            //驳回-直接修改申请单状态
            var retValue = VerifyAuditNotPass(reqWithdrawalsAuditDto);
            if (retValue.HasError)
                return retValue;
            var info = retValue.ReturnObject as Entities.LETask.LeWithdrawalsDetail;
            if (info == null)
            {
                return CreateFailMessage(retValue, "3029", "数据转换异常");
            }
            //todo:驳回的情况下，资金应该还原
            var updateId = Dal.LETask.LeWithdrawalsDetail.Instance.UpdateNotPass(reqWithdrawalsAuditDto.WithdrawalsId, "驳回",
                info.WithdrawalsPrice, info.PayeeID);
            if (updateId <= 0)
                return CreateFailMessage(retValue, "3030", "驳回操作失败");

            //添加审核日志
            Task.Run(() =>
            {
                UpdateAuditInfo(reqWithdrawalsAuditDto.WithdrawalsId, WithdrawalsAuditStatusEnum.驳回,
                        _configEntity.CreateUserId, reqWithdrawalsAuditDto.RejectMsg);
            });

            return retValue;
        }

        private ReturnValue AuditPass(ReqWithdrawalsAuditDto reqWithdrawalsAuditDto)
        {
            Entities.LETask.LeWithdrawalsDetail withdrawalsDetail;
            string identityNo;
            var retValue = VerifyAudit(reqWithdrawalsAuditDto, out withdrawalsDetail, out identityNo);
            if (retValue.HasError)
                return retValue;
            _configEntity.IdentityNo = identityNo;
            //审核通过
            var updateId = Dal.LETask.LeWithdrawalsDetail.Instance.UpdateSyncResult(withdrawalsDetail.RecID, WithdrawalsAuditStatusEnum.通过);
            if (updateId <= 0)
                return CreateFailMessage(retValue, "3030", "审核操作失败");
            //添加审核日志
            if (withdrawalsDetail.AuditStatus != (int)WithdrawalsAuditStatusEnum.通过)
                Task.Run(() => { UpdateAuditInfo(reqWithdrawalsAuditDto.WithdrawalsId, WithdrawalsAuditStatusEnum.通过, _configEntity.CreateUserId); });
            //同步接口返回( 添加订单记录，修改同步接口返回结果)
            try
            {
                retValue = new KrFundsProvider(_configEntity).Disbursement(withdrawalsDetail);
            }
            catch (KrPayCreateDisbursementException exception)
            {
                //todo:锁定
                Dal.LETask.LeWithdrawalsDetail.Instance.UpdateLock(withdrawalsDetail.RecID, true);
                Loger.Log4Net.Error($"提现审核-支付同步接口异常错误，:{exception.Message}{System.Environment.NewLine}{JsonConvert.SerializeObject(withdrawalsDetail)}");
                return CreateFailMessage(retValue, "3040", "支付异常-已锁定此申请单");
            }
            if (retValue.HasError)
            {
                //todo:同步接口失败，更新SyncPayStatus为失败，异步支付状态：支付中（不变，默认就是支付中）
                updateId = Dal.LETask.LeWithdrawalsDetail.Instance.UpdateSyncPayStatus(withdrawalsDetail.RecID, WithdrawalsStatusEnum.支付失败, retValue.Message);
                if (updateId <= 0)
                {
                    Loger.Log4Net.Error($"提现审核-支付同步接口返回失败，UpdateSyncPayStatus 修改失败:{withdrawalsDetail.RecID}");
                }
                return retValue;
            }
            else
            {
                //todo:修改同步支付结果：同步支付成功
                updateId = Dal.LETask.LeWithdrawalsDetail.Instance.UpdateSyncPayStatus(withdrawalsDetail.RecID, WithdrawalsStatusEnum.已支付);
                if (updateId <= 0)
                {
                    Loger.Log4Net.Error($"提现审核-支付同步接口支付成功，UpdateSyncPayStatus 修改失败:{withdrawalsDetail.RecID}");
                }
            }

            return retValue;
        }

        public void PushAuditMessage(Entities.LETask.LeWithdrawalsDetail withdrawalsDetail)
        {
            var requestUrl = ConfigurationManager.AppSettings["WXAuditApiUrl"];
            var userInfo = MediaUserDa.Instance.GetUserToken(withdrawalsDetail.PayeeID);
            if (userInfo != null)
            {
                if (!string.IsNullOrWhiteSpace(userInfo.openId))
                {
                    requestUrl += $"/api/WeChatToChiTu/WithdrawalsNotice?openId={userInfo.openId}&dt={DateTime.Now}&price={withdrawalsDetail.WithdrawalsPrice}&txid={withdrawalsDetail.RecID}";
                    var result = new DoPostApiLogClient(requestUrl, string.Empty)
                        .GetPostResult<dynamic>(s => WebService.Common.HttpClient.Get(requestUrl), Loger.Log4Net.Info);
                }
                else
                {
                    Loger.Log4Net.Error($"PushAuditMessage 提现审核通过 消息推送:openId is null .userId:{withdrawalsDetail.PayeeID}");
                }
            }
            else
            {
                Loger.Log4Net.Error($"PushAuditMessage 提现审核通过 消息推送:userInfo is null .userId:{withdrawalsDetail.PayeeID}");
            }
        }


        /// <summary>
        /// 库容异步通知结果为准
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="disbursementNo"></param>
        /// <param name="reason"></param>
        /// <param name="asynResult"></param>
        /// <param name="withdrawalsStatus"></param>
        /// <returns></returns>
        public ReturnValue AuditPayResult(ReturnValue retValue,
            string disbursementNo, string reason, string asynResult, WithdrawalsStatusEnum withdrawalsStatus)
        {
            //todo:调用库容接口
            var disbursementPayInfoList = Dal.LETask.LeDisbursementPay.Instance.GetInfo(disbursementNo);

            if (disbursementPayInfoList.Count > 1)
            {
                return CreateFailMessage(retValue, "3039", $"此提现信息存在问题,查询到多条记录：disbursementNo:{disbursementNo}");
            }
            var disbursementPayInfo = disbursementPayInfoList.FirstOrDefault();
            if (disbursementPayInfo == null)
            {
                return CreateFailMessage(retValue, "3040", "此提现信息不存在-LE_DisbursementPay");
            }
            if (disbursementPayInfo.PayStatus == (int)WithdrawalsStatusEnum.已支付)
            {
                return CreateFailMessage(retValue, "3041", $"异步回调此申请单-付款单{disbursementNo}已支付成功，终止");
            }
            //todo:记录修改资金之前的资金情况
            Loger.ZhyLogger.Info($"AuditPayResult........修改冻结资金之前的资金情况：" +
                                 $"{JsonConvert.SerializeObject(disbursementPayInfo)}");

            //修改冻结资金
            var payTime = DateTime.Now;
            Loger.ZhyLogger.Info($"AuditPayResult........修改冻结资金,PayeeID:{disbursementPayInfo.PayeeID},WithdrawalsPrice={disbursementPayInfo.WithdrawalsPrice}" +
                                 $",WithdrawalsId={disbursementPayInfo.WithdrawalsId},reason={reason},asynResult={asynResult}," +
                                 $",withdrawalsStatus={withdrawalsStatus},payTime={payTime}");

            var excuteCount = Dal.LETask.LeWithdrawalsStatistics.Instance
                 .UpdateWithdrawalsStatisticsSuccess(disbursementPayInfo.PayeeID, disbursementPayInfo.WithdrawalsPrice,
                 disbursementPayInfo.WithdrawalsId, reason, asynResult, withdrawalsStatus, payTime);

            if (excuteCount == 0)
            {
                return CreateFailMessage(retValue, "3012", "支付回调-修改冻结资金失败");
            }

            //推送消息,支付成功发送模版消息
            if (withdrawalsStatus == WithdrawalsStatusEnum.已支付)
            {
                Task.Run(() =>
                {
                    PushAuditMessage(new LeWithdrawalsDetail()
                    {
                        PayeeID = disbursementPayInfo.PayeeID,
                        RecID = disbursementPayInfo.WithdrawalsId,
                        WithdrawalsPrice = disbursementPayInfo.WithdrawalsPrice,
                        PayDate = payTime
                    });
                });
            }
            return retValue;
        }

        /// <summary>
        /// 添加审核记录
        /// </summary>
        /// <param name="withdrawalsId"></param>
        /// <param name="withdrawalsAuditStatus"></param>
        /// <param name="createUserId">审核的时候是审核人，支付失败的时候是自己</param>
        /// <param name="rejectMsg">驳回原因</param>
        public void UpdateAuditInfo(int withdrawalsId, WithdrawalsAuditStatusEnum withdrawalsAuditStatus, int createUserId, string rejectMsg = "")
        {
            var auditInfo = new AuditInfo()
            {
                RejectMsg = rejectMsg,
                AuditStatus = (int)withdrawalsAuditStatus,
                CreateUserId = createUserId,
                RelationId = withdrawalsId,
                RelationType = (int)AuditTypeEnum.提现审核
            };
            var excuteId = Dal.LETask.AuditInfo.Instance.Insert(auditInfo);
            if (excuteId == 0)
            {
                Loger.Log4Net.Error($"UpdateAuditInfo 提现审核插入信息失败：{JsonConvert.SerializeObject(auditInfo)}");
            }
        }


        #endregion

        /// <summary>
        /// 校验点击提现按钮
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyWithdrawalsClick()
        {
            //获取可提现金额
            var dto = new OrderProvider().GetIncomeInfo(_configEntity.CreateUserId);

            _reqWithdrawalsDto.WithdrawalsPrice = dto.CanWithdrawalsMoney;

            var retValue = WithdrawalsVerify();
            if (retValue.HasError)
            {
                return retValue;
            }
            return retValue;
        }

    }
}
