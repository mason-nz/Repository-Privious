using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.Extensions;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Response.Withdrawals;
using XYAuto.ChiTu2018.Service.ThirdApi;
using XYAuto.ChiTu2018.Service.ThirdApi.Dto.Request;

namespace XYAuto.ChiTu2018.Service.LE.Provider
{
    /// <summary>
    /// 注释：WxWithdrawalsProvider 微信提现相关 http 请求公共服务
    /// 作者：lix
    /// 日期：2018/6/1 16:43:10
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WxWithdrawalsProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqWithdrawalsDto _reqWithdrawalsDto;

        public WxWithdrawalsProvider(ConfigEntity configEntity, ReqWithdrawalsDto reqWithdrawalsDto)
        {
            _configEntity = configEntity;
            _reqWithdrawalsDto = reqWithdrawalsDto;
        }


        #region 计算个税金额 http 请求公共服务

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
            
            var retValue = new WithdrawalsClient().PostWithdrawalsPriceCalc(new ReqPostWithdrawalsClickDto
            {
                UserId = _configEntity.UserId
            });

            return retValue.ReturnObject as RespWithdrawalsPriceClc;
        }

        #endregion

        #region 提现申请 http 请求公共服务

        /// <summary>
        /// 提现申请
        /// </summary>
        /// <returns></returns>
        public ReturnValue Withdrawals()
        {
            var retValue = WithdrawalsVerify(_reqWithdrawalsDto);
            if (retValue.HasError)
                return retValue;
            retValue = new WithdrawalsClient().PostWithdrawals(new ReqPostWithdrawlsDto()
            {
                ApplySource = (int)WithdrawalsApplySourceEnum.WeiXin,
                Ip = _reqWithdrawalsDto.Ip,
                Mobile = _reqWithdrawalsDto.Mobile,
                UserId = _configEntity.UserId,
                WithdrawalsPrice = _reqWithdrawalsDto.WithdrawalsPrice
            });
            if (retValue.HasError)
            {
                return retValue;
            }
            return retValue;
        }

        /// <summary>
        /// 提现操作-校验相关信息
        /// </summary>
        /// <returns></returns>
        private ReturnValue WithdrawalsVerify(ReqWithdrawalsDto requestDto)
        {
            var retValue = VerifyOfNecessaryParameters<ReqWithdrawalsDto>(requestDto);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        #endregion

        /// <summary>
        /// 校验点击提现按钮
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyWithdrawalsClick()
        {
            var retValue = new WithdrawalsClient().PostVerifyWithdrawalsClick(new ReqPostWithdrawalsClickDto
            {
                UserId = _configEntity.UserId,
                Ip = _configEntity.Ip
            });

            return retValue;
        }
    }
}
