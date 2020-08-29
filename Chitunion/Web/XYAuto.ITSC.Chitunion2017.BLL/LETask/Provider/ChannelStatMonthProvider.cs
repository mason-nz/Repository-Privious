using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider
{
    /// <summary>
    /// auth:lixiong
    /// desc:渠道月结数据汇总提供
    /// </summary>
    public class ChannelStatMonthProvider : VerifyOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly ReqChannelStatMonthPayDto _reqChannelStatMonthPayDto;

        public ChannelStatMonthProvider(ConfigEntity configEntity, ReqChannelStatMonthPayDto reqChannelStatMonthPayDto)
        {
            _configEntity = configEntity;
            _reqChannelStatMonthPayDto = reqChannelStatMonthPayDto;
        }

        public ReturnValue Pay()
        {
            var retValue = VerifyPay();
            if (retValue.HasError)
            {
                return retValue;
            }
            var insertInfo = GetEntity();
            var excuteId = Dal.LETask.LeChannelStatMonthRelation.Instance.Insert(insertInfo);

            if (excuteId <= 0)
            {
                Loger.Log4Net.Error($"渠道月结数据汇总-支付操作失败：{JsonConvert.SerializeObject(insertInfo)}");
                return CreateFailMessage(retValue, "5002", "操作失败");
            }
            return retValue;
        }

        private Entities.LETask.LeChannelStatMonthRelation GetEntity()
        {
            return new Entities.LETask.LeChannelStatMonthRelation()
            {
                StatisticsId = _reqChannelStatMonthPayDto.StatisticsId,
                CreateTime = DateTime.Now,
                PayStatus = _reqChannelStatMonthPayDto.PayStatus,
                PayTime = DateTime.Now,
                Reason = string.Empty,
                Status = 0,
                CreateUserId = _configEntity.CreateUserId
            };
        }

        private ReturnValue VerifyPay()
        {
            var retValue = VerifyOfNecessaryParameters(_reqChannelStatMonthPayDto);
            if (retValue.HasError)
                return retValue;
            var info = Dal.LETask.LeChannelStatMonthRelation.Instance.GetInfoByStatId(_reqChannelStatMonthPayDto.StatisticsId);

            if (info != null)
            {
                return CreateFailMessage(retValue, "5001", "已经操作过，不能重复操作");
            }
            return retValue;
        }

        /// <summary>
        /// 获取渠道月结数据-汇总年月下拉数据
        /// </summary>
        /// <returns></returns>
        public List<ChannelStatMonth> GetChannelStatMonths()
        {
            return Dal.LETask.LeChannelStatMonthRelation.Instance.GetChannelStatMonths();
        }
    }
}
