/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 12:51:08
*说明：广告查询列表（后台）代理类
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1
{
    public class AdQueryProxy : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestAdQueryDto _requestAdQueryDto;
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyBackQueryDic;//lazy加载

        public AdQueryProxy(ConfigEntity configEntity, RequestAdQueryDto requestAdQueryDto)
        {
            _configEntity = configEntity;
            _requestAdQueryDto = requestAdQueryDto;
            _lazyBackQueryDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(InitByBackQuery);
        }

        private Dictionary<int, Func<string, dynamic>> InitByBackQuery()
        {
            return new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => QueryAdWeiXin()},
                { (int)MediaType.Template , s=> new AdTemplateYunYingQuery(_configEntity).GetQueryList(_requestAdQueryDto)}
            };
        }

        public dynamic GetQuery()
        {
            return _lazyBackQueryDic.Value.ContainsKey(_requestAdQueryDto.BusinessType)
                  ? _lazyBackQueryDic.Value[_requestAdQueryDto.BusinessType].Invoke(string.Empty)
                  : CreateFailMessage(new ReturnValue(), "60001", "请输入合法的BusinessType");
        }

        /// <summary>
        /// 媒体主：添加直接通过，没有待审核
        /// AE：看到的待审核，驳回，通过都是自己的
        /// 运营和超级管理员 --> 待审核：是看到的全部数据	通过和驳回：是看到的自己的操作记录
        ///
        /// 返回数据格式解析：
        /// 媒体主：通过 ---> AdWeiXinAuditPassQuery（返回的数据格式与待审核、驳回数据不一样）
        ///
        /// AE
        ///     1.通过         ---> AdWeiXinAuditPassQuery
        ///     2.待审核、驳回  ---> AdWeiXinNotPassQuery
        ///
        /// 运营、管理员（不区分数据返回）
        ///     1.通过（审核是自己的记录）
        ///     2.驳回（审核是自己的记录）
        ///     3.待审核（全部的数据与当前人无关）
        /// 故：运营、管理员    ---> AdWeiXinNotPassQuery（里面有区分）
        /// </summary>
        /// <returns></returns>
        private dynamic QueryAdWeiXin()
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                // _requestAdQueryDto.Wx_Status = ((int)PublishBasicStatusEnum.已通过 + "," + (int)PublishBasicStatusEnum.上架);
                return new AdWeiXinAuditPassQuery(_configEntity).GetQueryList(_requestAdQueryDto);
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate
              || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                if (_requestAdQueryDto.IsAuditView)
                {
                    //是运营角色下的审核页面
                    if (_requestAdQueryDto.Wx_Status.ToInt(0) == (int)PublishBasicStatusEnum.待审核)
                    {
                        //全部数据（AdWeiXinNotPassQuery里面有区分是查询提交人，还是审核人）
                        _requestAdQueryDto.CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
                    }
                    //审核操作人是当前人
                    if (_requestAdQueryDto.IsPassed)
                    {
                        _requestAdQueryDto.Wx_Status = ((int)PublishBasicStatusEnum.上架).ToString();
                    }
                    return new AdWeiXinNotPassQuery(_configEntity).GetQueryList(_requestAdQueryDto);
                }
                //_requestAdQueryDto.Wx_Status = ((int)PublishBasicStatusEnum.已通过 + "," + (int)PublishBasicStatusEnum.上架);
                return new AdWeiXinQueryByYunYing(_configEntity).GetQueryList(_requestAdQueryDto);
            }
            else
            {
                if (_requestAdQueryDto.IsPassed)
                {
                    return new AdWeiXinAuditPassQuery(_configEntity).GetQueryList(_requestAdQueryDto);
                }
                //AE：看到的待审核，驳回，通过都是自己的
                return new AdWeiXinNotPassQuery(_configEntity).GetQueryList(_requestAdQueryDto);
            }
        }

        public dynamic QueryAdWeiXinByYunYing()
        {
            return new AdWeiXinQueryByYunYing(_configEntity).GetQueryList(_requestAdQueryDto);
        }

        public PublishStatisticsCount GetStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            var providerDic = new Dictionary<int, Func<string, dynamic>>()
            {
                { (int)MediaType.WeiXin , pro => GetPublishStatisticsCount(query)},
                { (int)MediaType.Template , pro => GetAdTemplateStatisticsCount(query)},
                { (int)MediaType.APP, pro => GetMediaAppStatisticsCount(query)}
            };

            return providerDic.ContainsKey(query.BusinessType)
            ? providerDic[query.BusinessType].Invoke(string.Empty)
            : CreateFailMessage(new ReturnValue(), "60002", "请输入合法的BusinessType");
        }

        /// <summary>
        /// 获取刊例待审核、驳回的数据统计总数
        ///
        /// 运营+管理员：
        /// --通过或者驳回，看到的是自己的审核操作记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private PublishStatisticsCount GetPublishStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
                _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                return Dal.Publish.PublishInfoQuery.Instance.GetAdStatisticsCountByYunYing(query);
            }
            return Dal.Publish.PublishInfoQuery.Instance.GetPublishStatisticsCount(query);
        }

        /// <summary>
        /// 广告模板列表审核数据统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private PublishStatisticsCount GetAdTemplateStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
                _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                //只有运营才会访问
                return Dal.AdTemplate.AppAdTemplate.Instance.GetAdStatisticsCountByYunYing(query);
            }
            return new PublishStatisticsCount();
        }

        /// <summary>
        /// 媒体app列表数据统计
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private PublishStatisticsCount GetMediaAppStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            return Dal.Media.MediaPCAPP.Instance.GetStatisticsCount(query);
        }
    }
}