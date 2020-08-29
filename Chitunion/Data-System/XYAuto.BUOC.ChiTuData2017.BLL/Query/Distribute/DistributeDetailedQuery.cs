/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 15:54:30
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute
{
    public class DistributeDetailedQuery
        : PublishInfoQueryClient<RequestDistributeQueryDto, RespDistributeChannelDto>
    {
        public DistributeDetailedQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        /// <summary>
        /// 物料分发，日结数据汇总,有渠道的是赤兔类型，渠道单独提供接口获取
        /// </summary>
        /// <returns></returns>
        protected override QueryPageBase<RespDistributeChannelDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    SELECT  DD.DistributeId ,
                            DD.MaterielId ,
                            DD.Date ,
                            DD.PV ,
                            DD.UV ,
                            DD.OnLineAvgTime,
                            DD.BrowsePageAvg ,
                            DD.JumpProportion ,
                            DD.InquiryNumber ,
                            DD.SessionNumber ,
                            DD.TelConnectNumber ,
                            DD.Source ,
                            DD.Status ,
                            DD.CreateTime ,
                            DD.CreateUserId
                            ,ForwardNumber
                    FROM    dbo.Materiel_DistributeDetailed AS DD WITH ( NOLOCK )
                    LEFT JOIN (SELECT 
 B.Date,SUM(CASE  WHEN ForwardNumber <=-1 THEN 0 WHEN ForwardNumber IS NULL THEN 0 ELSE ForwardNumber END)AS ForwardNumber 
  FROM [Chitunion_DataSystem2017].[dbo].[Materiel_DetailedStatistics] A WITH ( NOLOCK )
  INNER JOIN [Materiel_DistributeDetailed] B WITH ( NOLOCK ) ON A.DistributeId = B.DistributeId
  WHERE  B.MaterielId={0}
  GROUP BY B.Date
                    ) DS ON DS.Date = DD.Date
                    WHERE   DD.Status = 0
                    ", RequetQuery.MaterielId);


            //对于前台页面必须传 MaterielId
            if (RequetQuery.MaterielId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND DD.MaterielId = {0}", RequetQuery.MaterielId);
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDate))
            {
                sbSql.AppendFormat(@" AND DD.Date BETWEEN '{0}' AND '{1}'", RequetQuery.StartDate, RequetQuery.EndDate);
            }
            if (RequetQuery.DistributeType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@"AND DD.Source = {0}", RequetQuery.DistributeType);
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDistributeChannelDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " Date ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespDistributeChannelDto> GetResult(List<RespDistributeChannelDto> resultList,
            QueryPageBase<RespDistributeChannelDto> query)
        {
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            resultList.ForEach(s =>
            {
                s.OnLineAvgTimeFormt = "一";// DistributeProfile.GetOnLineAvgTimeFormt(s.OnLineAvgTime);
                //s.BrowsePageAvg = DistributeProfile.GetBrowsePageAvg(s.PV, s.UV, s.BrowsePageAvg);
            });
            return base.GetResult(resultList, query);
        }
    }
}