/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 16:19:52
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
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute
{
    public class DistributeDetailedChannelQuery
         : PublishInfoQueryClient<RequestDistributeQueryDto, RespDistributeChannelDto>
    {
        public DistributeDetailedChannelQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

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
                    FROM    dbo.Materiel_DistributeDetailed AS DD WITH ( NOLOCK )
                    WHERE   DD.Status = 0 AND DD.MaterielId = {0}
                    ", RequetQuery.MaterielId);

            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDate))
            {
                sbSql.AppendFormat(@" AND DD.Date BETWEEN '{0}' AND '{1}'", RequetQuery.StartDate, RequetQuery.EndDate);
            }
            if (RequetQuery.DistributeType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND DD.Source = {0}", RequetQuery.DistributeType);
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
            if (!RequetQuery.IsGetResult)
                return base.GetResult(resultList, query);
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            resultList.ForEach(item =>
            {
                item.OnLineAvgTimeFormt = DistributeProfile.GetOnLineAvgTimeFormt(item.OnLineAvgTime);
                item.BrowsePageAvg = DistributeProfile.GetBrowsePageAvg(item.PV, item.UV, item.BrowsePageAvg);
            });

            return base.GetResult(resultList, query);
        }
    }
}