using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1
{
    public class PbQueryProxy
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestPublishQueryDto _requestPublishQueryDto;
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyQueryDic;//lazy加载

        //public PbQueryProxy(ConfigEntity configEntity)
        //{
        //    _configEntity = configEntity;
        //}

        public PbQueryProxy(ConfigEntity configEntity, RequestPublishQueryDto requestPublishQueryDto)
        {
            _configEntity = configEntity;
            _requestPublishQueryDto = requestPublishQueryDto;
            _lazyQueryDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(InitializationByQuery);
        }

        private Dictionary<int, Func<string, dynamic>> InitializationByQuery()
        {
            return new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => QueryWeiXin()}
            };
        }

        public dynamic GetQuery()
        {
            return _lazyQueryDic.Value.ContainsKey(_requestPublishQueryDto.BusinessType)
                  ? _lazyQueryDic.Value[_requestPublishQueryDto.BusinessType].Invoke(string.Empty)
                  : default(dynamic);
        }

        /// <summary>
        /// 刊例列表：媒体主查看全部已通过的（不用审核）。AE查看自己的，区分状态查询
        /// 运营和超级管理员：看到的是全部的信息
        /// </summary>
        /// <returns></returns>
        private dynamic QueryWeiXin()
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                return new PbWeiXinAuditPassQuery(_configEntity).GetQueryList(_requestPublishQueryDto);
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate
                || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                //运营和超级管理员 能看见所有的审核操作记录（现在是精确到角色，暂没有到个人）
                return new PbWeiXinQueryByYunYing(_configEntity).GetQueryList(_requestPublishQueryDto);
            }
            else
            {
                if (_requestPublishQueryDto.IsPassed)//== (int)PublishBasicStatusEnum.已通过
                {
                    return new PbWeiXinAuditPassQuery(_configEntity).GetQueryList(_requestPublishQueryDto);
                }
                else
                {
                    return new PbWeiXinNotPassQuery(_configEntity).GetQueryList(_requestPublishQueryDto);
                }
            }
        }

        /// <summary>
        /// 获取刊例待审核、驳回的数据统计总数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PublishStatisticsCount GetPublishStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
                _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                //看到的是全部的信息,不用带CreateUserId参数
                query.CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            }
            return Dal.Publish.PublishInfoQuery.Instance.GetPublishStatisticsCount(query);
        }
    }
}