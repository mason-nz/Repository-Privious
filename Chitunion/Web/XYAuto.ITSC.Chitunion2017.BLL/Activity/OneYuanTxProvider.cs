using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.WeChat;

namespace XYAuto.ITSC.Chitunion2017.BLL.Activity
{
    /// <summary>
    /// 注释：OneYuanTxProvider
    /// 作者：lix
    /// 日期：2018/6/12 16:28:26
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class OneYuanTxProvider : VerifyOperateBase
    {
        private readonly int ActivityKey = 103009;
        private readonly ConfigEntity _configEntity;
        private readonly ReqWithdrawalsDto _reqWithdrawalsDto;
        public OneYuanTxProvider(ConfigEntity configEntity, ReqWithdrawalsDto reqWithdrawalsDto)
        {
            _configEntity = configEntity;
            _reqWithdrawalsDto = reqWithdrawalsDto;
        }

        public OneYuanActivityEntity ActivityConfig
        {
            get
            {
                return ActivityConfigManage.GetActivityConfig();
            }
        }

        private VerifyUserModel GetVerifyUserModel()
        {
            return new VerifyUserModel
            {
                UserID = _configEntity.CreateUserId,
                BeginTime = ActivityConfig.BeginTime,
                EndTime = ActivityConfig.EndTime,
                Source = ActivityKey
            };
        }


        /// <summary>
        /// 是否满足活动
        /// </summary>
        /// <returns></returns>
        public bool IsMatchActivity()
        {
            var vModel = GetVerifyUserModel();

            var isNewUser = ActivityVerifyBll.Instance.VerifyUser(vModel);
            var incomeDetail = ActivityVerifyBll.Instance.VerifyIncomeDetail(vModel);
            var verifyWithdrawals = ActivityVerifyBll.Instance.VerifyWithdrawals(vModel);
            if (isNewUser && !incomeDetail
                && verifyWithdrawals)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        public ReturnValue PostWithdrawas()
        {
            var retValue = new ReturnValue();

            if ((ActivityConfig.EndTime - DateTime.Now).TotalDays < 0)
            {
                return CreateFailMessage(retValue, "1000", "活动已经过期");
            }
            if (_reqWithdrawalsDto.WithdrawalsPrice < ActivityConfig.Price)
            {
                return CreateFailMessage(retValue, "1001", "提现金额必须大于1元");
            }

            if (!IsMatchActivity())
            {
                return CreateFailMessage(retValue, "1002", "不满足1元提现活动规则");
            }

            retValue = WithdrawalsVerify(retValue);

            if (retValue.HasError)
            {
                return retValue;
            }
            _reqWithdrawalsDto.ApplySource = WithdrawalsApplySourceEnum.ActivityOneYuanTiXian;
            return new WithdrawalsProvider(_configEntity, _reqWithdrawalsDto).PostWithdrawals(retValue);
        }


        /// <summary>
        /// 提现操作-校验相关信息
        /// </summary>
        /// <returns></returns>
        private ReturnValue WithdrawalsVerify(ReturnValue retValue)
        {
            //添加黑名单验证逻辑,begin================(add=masj,Date=2018-04-17)
            //string ip = TaskProvider.GetIP();
            string ip = ITSC.Chitunion2017.BLL.Util.GetIP($"用户{ _configEntity.CreateUserId}提现申请");
            BLL.LETask.Provider.TaskProvider pro = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto());
            retValue = pro.VerifyAntiCheating(retValue, ip, _configEntity.CreateUserId);
            if (retValue.HasError)
            {
                Loger.Log4Net.Info($"提现申请VerifyAntiCheating:{JsonConvert.SerializeObject(retValue)},userId:{_configEntity.CreateUserId},ip;{ip}");
                return CreateFailMessage(retValue, "1010", "操作异常，错误代码：1010");
            }
            //添加黑名单验证逻辑,end==================

            var resultCode = Dal.LETask.LeWithdrawalsStatistics.Instance.VerifyWithdrawals(_configEntity.CreateUserId,
                 string.Empty, _reqWithdrawalsDto.WithdrawalsPrice);

            if (resultCode != null && resultCode.ResultCode > 0)
            {
                return CreateFailMessage(retValue, resultCode.ResultCode.ToString(),
                    WithdrawalsProvider.GetVerifyMessage(resultCode.ResultCode));
            }

            return retValue;
        }

        /// <summary>
        /// 校验点击提现按钮
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyWithdrawalsClick()
        {
            //获取可提现金额
            var dto = new OrderProvider().GetIncomeInfo(_configEntity.CreateUserId);

            _reqWithdrawalsDto.WithdrawalsPrice = dto.CanWithdrawalsMoney;

            var retValue = new ReturnValue();
            retValue = WithdrawalsVerify(retValue);
            if (retValue.HasError)
            {
                return retValue;
            }
            return retValue;
        }
    }
}
