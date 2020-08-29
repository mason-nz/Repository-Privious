/********************************************************
*创建人：lixiong
*创建时间：2017/9/9 14:51:32
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.AutoResolver;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics
{
    public class DetailedStatisticsQuery

           : PublishInfoQueryClient<RequestDistributeQueryDto, RespDetailedStatisticsDto>
    {
        public DetailedStatisticsQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDetailedStatisticsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                SELECT  DD.DistributeId ,
                        DD.MaterielId ,
                        DD.Date
                FROM    dbo.Materiel_DistributeDetailed AS DD WITH ( NOLOCK )
                        --INNER JOIN dbo.Materiel_DetailedStatistics AS DS WITH ( NOLOCK ) ON DS.DistributeId = DD.DistributeId
                WHERE   DD.MaterielId = {0}
                ", RequetQuery.MaterielId);

            if (RequetQuery.DistributeType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@"AND DD.Source = {0}", RequetQuery.DistributeType);
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDate))
            {
                sbSql.AppendFormat(@" AND DD.Date BETWEEN '{0}' AND '{1}'", RequetQuery.StartDate, RequetQuery.EndDate);
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDetailedStatisticsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " Date ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        /// <summary>
        /// 现在只能是先取出Materiel_DistributeDetailed 的DistributeId 列表
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override BaseResponseEntity<RespDetailedStatisticsDto> GetResult(List<RespDetailedStatisticsDto> resultList,
            QueryPageBase<RespDetailedStatisticsDto> query)
        {
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            //var distributeIds = resultList.Select(s => s.DistributeId).ToList();
            //var statisticsList = Dal.Distribute.MaterielDetailedStatistics.Instance.GetList(0, distributeIds);
            var statisticsList = Dal.Distribute.MaterielDetailedStatistics.Instance.GetList(0, query.StrSql);

            //赤兔平级返回
            if (RequetQuery.DistributeType == (int)DistributeTypeEnum.QuanWangYu)
            {
                resultList.ForEach(item =>
                {
                    item.Item = new List<StatisticsDto>();
                    var list = statisticsList.Where(s => s.DistributeId == item.DistributeId).OrderBy(s => s.ConentType).ToList();
                    item.Item.AddRange(AutoMapper.Mapper.Map<List<MaterielDetailedStatistics>,
                        List<StatisticsDto>>(list));
                });
                return base.GetResult(resultList, query);
            }
            //经纪人
            var statisticsHeadBodyList = statisticsList.Where(s => s.ConentType == (int)MaterielConentTypeEnum.Head
                                                || s.ConentType == (int)MaterielConentTypeEnum.Body);
            var statisticsFootList = statisticsList.Where(s => s.ConentType == (int)MaterielConentTypeEnum.Foot);

            resultList.ForEach(item =>
            {
                item.Item = new List<StatisticsDto>();

                //先把头，腰的数据加载进来
                item.Item.AddRange(AutoMapper.Mapper.Map<List<MaterielDetailedStatistics>,
                    List<StatisticsDto>>(statisticsHeadBodyList.Where(s => s.DistributeId == item.DistributeId).OrderBy(s => s.ConentType).ToList()));
                //最后循环获取
                DetailedStatisticsRsolver.GetFootStatistics(statisticsFootList, item.Item);
            });

            return base.GetResult(resultList, query);
        }
    }
}