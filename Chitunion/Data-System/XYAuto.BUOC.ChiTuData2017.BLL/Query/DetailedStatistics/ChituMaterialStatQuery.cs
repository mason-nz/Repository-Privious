/********************************************************
*创建人：lixiong
*创建时间：2017/10/26 11:32:04
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
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics
{
    public class ChituMaterialStatQuery
           : PublishInfoQueryClient<RequestChituChannelDto, Entities.Distribute.MaterielDistributeDetailed>
    {
        public ChituMaterialStatQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<MaterielDistributeDetailed> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                        SELECT  CAST(CMS.material_id  AS INT)  AS MaterielID,
                                CMS.dt AS Date,
                                CMS.total_pv AS PV,
                                CMS.total_uv AS UV,
                                CMS.avg_dur AS OnLineAvgTime,
                                {1} AS Source,
                                0 AS Status,
                                GETDATE() AS CreateTime,
                                ME.CreateUserID
                        FROM    dbo.chitu_material_stat AS CMS WITH ( NOLOCK )
                                LEFT JOIN Chitunion_OP2017.dbo.MaterielExtend AS ME WITH ( NOLOCK ) ON ME.MaterielID = CMS.material_id
                        WHERE   CMS.dt= '{0}'
", RequetQuery.Date, (int)DistributeTypeEnum.QuanWangYu);

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<Entities.Distribute.MaterielDistributeDetailed>()
            {
                StrSql = sbSql.ToString(),
                //OrderBy = " Date ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}