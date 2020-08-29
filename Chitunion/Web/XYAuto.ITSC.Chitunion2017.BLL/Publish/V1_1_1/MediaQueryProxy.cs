/********************************************************
*创建人：lixiong
*创建时间：2017/6/9 18:20:36
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1
{
    public class MediaQueryProxy : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestMediaAppQueryDto _requestMediaAppQueryDto;
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyBackQueryDic;//lazy加载

        public MediaQueryProxy(ConfigEntity configEntity, RequestMediaAppQueryDto requestMediaAppQueryDto)
        {
            _configEntity = configEntity;
            _requestMediaAppQueryDto = requestMediaAppQueryDto;
            _lazyBackQueryDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(InitByBackQuery);
        }

        private Dictionary<int, Func<string, dynamic>> InitByBackQuery()
        {
            return new Dictionary<int, Func<string, dynamic>>()
            {
                { (int)MediaType.APP , s=> GetAppQuery()}
            };
        }

        public dynamic GetQuery()
        {
            return _lazyBackQueryDic.Value.ContainsKey(_requestMediaAppQueryDto.BusinessType)
                  ? _lazyBackQueryDic.Value[_requestMediaAppQueryDto.BusinessType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "60001", "请输入合法的BusinessType");
        }

        private dynamic GetAppQuery()
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                //媒体主：待审核、驳回、通过
                if (_requestMediaAppQueryDto.IsPassed)
                {
                    _requestMediaAppQueryDto.AuditStatus = ((int)MediaAuditStatusEnum.AlreadyPassed).ToString();
                    return new MediaAppAuditPassQuery(_configEntity).GetQueryList(_requestMediaAppQueryDto);
                }
                else
                {
                    //需要前端传 驳回或者是通过 的状态
                    // _requestMediaAppQueryDto.AuditStatus = ((int)MediaAuditStatusEnum.RejectNotPass + "," + (int)MediaAuditStatusEnum.PendingAudit);
                    return new MediaAppNotPassQuery(_configEntity).GetQueryList(_requestMediaAppQueryDto);
                }
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate
               || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                if (_requestMediaAppQueryDto.IsAuditView)
                {
                    if (_requestMediaAppQueryDto.IsPassed)
                    {
                        _requestMediaAppQueryDto.AuditStatus = ((int)MediaAuditStatusEnum.AlreadyPassed).ToString();
                        return new MediaAppAuditPassQuery(_configEntity).GetQueryList(_requestMediaAppQueryDto);
                    }
                    else
                    {
                        //审核页面（需要前端传 驳回或者是通过 的状态）
                        return new MediaAppNotPassQuery(_configEntity).GetQueryList(_requestMediaAppQueryDto);
                    }
                    //审核页面（都是待审核的）
                    //_requestMediaAppQueryDto.AuditStatus = ((int)MediaAuditStatusEnum.PendingAudit).ToString();
                    // return new MediaAppQueryByYunYingAudit(_configEntity).GetQueryList(_requestMediaAppQueryDto);
                }
                else
                {
                    //基表信息
                    return new MediaAppQueryByYunYing(_configEntity).GetQueryList(_requestMediaAppQueryDto);
                }
            }
            else
            {
                //AE默认通过
                _requestMediaAppQueryDto.AuditStatus = ((int)MediaAuditStatusEnum.AlreadyPassed).ToString();
                return new MediaAppAuditPassQuery(_configEntity).GetQueryList(_requestMediaAppQueryDto);
            }
        }
    }
}