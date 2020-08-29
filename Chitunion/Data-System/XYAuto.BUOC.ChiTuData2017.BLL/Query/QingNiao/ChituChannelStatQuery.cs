/********************************************************
*创建人：lixiong
*创建时间：2017/9/14 16:18:42
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.QingNiao
{
    public class ChituChannelStatQuery
          : PublishInfoQueryClient<RequestChituChannelDto, RespChituChannelDto>
    {
        public ChituChannelStatQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespChituChannelDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                        SELECT  CCS.* ,
                                MC.ChannelID ,
                                MC.MaterielID ,
                                MC.PromotionUrlCode ,
                                MC.CreateUserID ,
                                MD.DistributeId ,
                                MC.ChannelType as DistributeDetailType
                        FROM    dbo.chitu_channel_stat AS CCS WITH ( NOLOCK )
                                INNER JOIN Chitunion_OP2017.dbo.MaterielChannel AS MC WITH ( NOLOCK ) ON MC.PromotionUrlCode = CCS.channel
                                INNER JOIN dbo.Materiel_DistributeDetailed AS MD WITH(NOLOCK) ON MD.MaterielId = MC.MaterielID AND MD.Date = '{0}'
                        WHERE   CCS.dt = '{0}'
                ", RequetQuery.Date);

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespChituChannelDto>()
            {
                StrSql = sbSql.ToString(),
                //OrderBy = " dt ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}