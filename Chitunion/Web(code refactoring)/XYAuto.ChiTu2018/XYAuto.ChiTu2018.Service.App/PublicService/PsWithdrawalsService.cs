using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Infrastructure.Exceptions;
using XYAuto.ChiTu2018.Infrastructure.Extensions;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Response.Withdrawals;

namespace XYAuto.ChiTu2018.Service.App.PublicService
{
    /// <summary>
    /// 注释：PsWithdrawalsService 公共服务-提现相关
    /// 作者：lix
    /// 日期：2018/6/1 15:58:44
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class PsWithdrawalsService : VerifyOperateBase
    {
        private readonly PsReqWithdrawalsDto _reqWithdrawalsDto;
        private readonly LeWithdrawalsDetailBO _leWithdrawalsDetailBo;
        private readonly PsVerifyWithdrawlsService _psVerifyWithdrawlsService;

        public PsWithdrawalsService(PsReqWithdrawalsDto reqWithdrawalsDto)
        {
            _reqWithdrawalsDto = reqWithdrawalsDto;
            _leWithdrawalsDetailBo = new LeWithdrawalsDetailBO();
            _psVerifyWithdrawlsService = new PsVerifyWithdrawlsService();
        }

        #region 配置

        /// <summary>
        /// 配置可申请提现的金额
        /// </summary>
        private decimal ConfigWithdrawalsPrice
        {
            get
            {
                try
                {
                    var configPrice = ConfigurationManager.AppSettings["ConfigWithdrawalsPrice"].ToDecimal(0);
                    if (configPrice <= 0)
                    {
                        throw new Exception("提现金额配置错误");
                    }
                    return configPrice;
                }
                catch (Exception exception)
                {
                    throw new Exception($"AppSettings[ConfigWithdrawalsPrice] 配置文件未找到节点：{exception.Message}{Environment.NewLine}{exception.StackTrace ?? string.Empty}");
                }
            }
        }

        #endregion

        #region 计算个税金额

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

            var totalMoneyInfo = _leWithdrawalsDetailBo.GetMonthOfWithdrawalsMoney(_reqWithdrawalsDto.UserId, Convert.ToDateTime(startDay),
                endDay: Convert.ToDateTime(lastDay));

            //累计提现金额（数据库已提现的金额+当前这次的金额）
            respPriceClc.IndividualTaxPeice =
                GetIndividualTaxPeice(totalMoneyInfo.WithdrawalsPrice.Value + _reqWithdrawalsDto.WithdrawalsPrice, totalMoneyInfo.IndividualTaxPeice.Value);
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
        private decimal GetIndividualTaxPeice(decimal totalMoney, decimal individualTaxPeice)
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

        #endregion

        #region 提现申请

        public ReturnValue Withdrawals()
        {
            var retValue = WithdrawalsVerify();
            if (retValue.HasError)
            {
                return retValue;
            }

            retValue = _psVerifyWithdrawlsService.VerifyUserBank(retValue, _reqWithdrawalsDto.UserId);
            if (retValue.HasError)
            {
                return retValue;
            }
            var userAccountName = retValue.ReturnObject.ToString();
            //todo:个人用户需要计算个税，企业用户不需要
            var tp = GetIndividualTaxPeice();
            if (tp.Item1 == -1 && tp.Item2 == -1)
            {
                return CreateFailMessage(retValue, "3022", $"用户信息不存在");
            }
            decimal practicalPrice = tp.Item1;
            decimal individualTaxPeice = tp.Item2;
            var withrawEntity = new LE_WithdrawalsDetail()
            {
                WithdrawalsPrice = _reqWithdrawalsDto.WithdrawalsPrice, //提现金额
                IndividualTaxPeice = individualTaxPeice, //个税金额
                PracticalPrice = practicalPrice, //实际付款
                PayeeAccount = userAccountName,
                Status = (int)WithdrawalsStatusEnum.支付中,
                ApplicationDate = DateTime.Now,
                PayeeID = _reqWithdrawalsDto.UserId,
                CreateTime = DateTime.Now,
                AuditStatus = (int)WithdrawalsAuditStatusEnum.待审核,
                ApplySource = (int)_reqWithdrawalsDto.ApplySource,
                IsActive = 1,
                OrderID = 0,
                SyncPayStatus = 0
            };

            try
            {
                var execute = _leWithdrawalsDetailBo.PostWithdrawas(entity: withrawEntity);
                if (execute.Item1 < 1 || !execute.Item2)
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"提现失败，参数:{JsonConvert.SerializeObject(withrawEntity)}");
                    return CreateFailMessage(retValue, "3002", $"提现失败:异常信息");
                }
                retValue.ReturnObject = execute.Item1;
            }
            catch (PostWithdrawasException exception)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error($"提现失败，参数:{JsonConvert.SerializeObject(withrawEntity)}" +
                                                                 $"错误信息:{exception.Message}");
                retValue.ReturnObject = 0;
                return CreateFailMessage(retValue, "3003", $"提现失败!服务器异常,请稍后重试");
            }

            return retValue;
        }

        /// <summary>
        /// 判断个人/企业 支付个税(个人用户需要计算个税，企业用户不需要)
        /// </summary>
        /// <returns>item1:实际支付金额  item2:个税金额</returns>
        private Tuple<decimal, decimal> GetIndividualTaxPeice()
        {
            //todo:个人用户需要计算个税，企业用户不需要
            decimal individualTaxPeice = 0;
            decimal practicalPrice = _reqWithdrawalsDto.WithdrawalsPrice;//企业用户不需要支付个税

            var userInfo = new UserInfoBO().GetInfo(_reqWithdrawalsDto.UserId);
            if (userInfo == null)
                return Tuple.Create(-1m, -1m);
            if (userInfo.Type == (int)UserTypeEnum.个人)
            {
                var priceClc = PriceCalc();
                individualTaxPeice = priceClc.IndividualTaxPeice;
                practicalPrice = priceClc.PracticalPrice;
            }
            return Tuple.Create(practicalPrice, individualTaxPeice);
        }

        /// <summary>
        /// 提现操作-校验相关信息
        /// </summary>
        /// <returns></returns>
        private ReturnValue WithdrawalsVerify()
        {
            var retValue = VerifyOfNecessaryParameters(_reqWithdrawalsDto);
            if (retValue.HasError)
                return retValue;
            if (_reqWithdrawalsDto.WithdrawalsPrice < ConfigWithdrawalsPrice)
            {
                return CreateFailMessage(retValue, "3020", $"可提现金额不足{ConfigWithdrawalsPrice}元，无法提现！");
            }
            retValue = _psVerifyWithdrawlsService.VerifyApplySource(retValue, _reqWithdrawalsDto.ApplySource);
            if (retValue.HasError)
            {
                return retValue;
            }
            retValue = _psVerifyWithdrawlsService.VerifyAntiCheating(retValue, _reqWithdrawalsDto.Ip, _reqWithdrawalsDto.UserId);
            if (retValue.HasError)
            {
                return CreateFailMessage(retValue, "0", "success");
            }
            var resultCode = _psVerifyWithdrawlsService.VerifyWithdrawals(_reqWithdrawalsDto.UserId,
                   string.Empty, _reqWithdrawalsDto.WithdrawalsPrice);
            if (resultCode != null && resultCode.ResultCode > 0)
            {
                return CreateFailMessage(retValue, resultCode.ResultCode.ToString(),
                    _psVerifyWithdrawlsService.GetVerifyMessage(resultCode.ResultCode));
            }

            return retValue;
        }

        #endregion

        #region 提现点击校验

        /// <summary>
        /// 校验点击提现按钮
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyWithdrawalsClick()
        {
            //获取可提现金额
            var dto = LeWithdrawalsStatisticsService.Instance.GetIncomeInfo(_reqWithdrawalsDto.UserId);

            _reqWithdrawalsDto.WithdrawalsPrice = dto.CanWithdrawalsMoney;

            var retValue = WithdrawalsVerify();
            if (retValue.HasError)
            {
                return retValue;
            }
            return retValue;
        }

        #endregion
    }
}
