using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.Extensions;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.App.PublicService;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Response.Withdrawals;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Provider
{
    /// <summary>
    /// 注释：WithdrawalsProvider
    /// 作者：lix
    /// 日期：2018/5/23 18:50:43
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppWithdrawalsProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly PsWithdrawalsService _publicWithdrawalsService;


        public AppWithdrawalsProvider(ConfigEntity configEntity, ReqWithdrawalsDto reqWithdrawalsDto)
        {
            _configEntity = configEntity;
            _publicWithdrawalsService = GetPsWithdrawalsService(reqWithdrawalsDto);
        }

        public PsWithdrawalsService GetPsWithdrawalsService(ReqWithdrawalsDto reqWithdrawals)
        {
            return new PsWithdrawalsService(new PsReqWithdrawalsDto()
            {
                WithdrawalsPrice = reqWithdrawals.WithdrawalsPrice,
                ApplySource = reqWithdrawals.ApplySource,
                Ip = _configEntity.Ip,
                UserId = _configEntity.UserId,
                Mobile = reqWithdrawals.Mobile
            });
        }

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

            return _publicWithdrawalsService.PriceCalc();

        }

        #endregion

        #region 提现申请 直接调用公共服务

        /// <summary>
        /// 提现申请
        /// </summary>
        /// <returns></returns>
        public ReturnValue Withdrawals()
        {
            return _publicWithdrawalsService.Withdrawals();
        }
        
        /// <summary>
        /// 校验点击提现按钮
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyWithdrawalsClick()
        {
            return _publicWithdrawalsService.VerifyWithdrawalsClick();
        }

        #endregion

    }
}
