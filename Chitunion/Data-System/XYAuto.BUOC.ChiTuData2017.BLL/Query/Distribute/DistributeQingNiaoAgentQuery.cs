/********************************************************
*创建人：lixiong
*创建时间：2017/10/17 10:49:22
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
using XYAuto.BUOC.ChiTuData2017.Entities.Query;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute
{
    public class DistributeQingNiaoAgentQuery
          : PublishInfoQueryClient<QingNiaoAgentQueryDto, Entities.Distribute.MaterielDistributeQingNiaoAgent>
    {
        public DistributeQingNiaoAgentQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<MaterielDistributeQingNiaoAgent> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    SELECT  MaterielId ,
                            MIN(DistributeDate) AS DistributeDate ,
                            DQN.Status
                    FROM    dbo.Materiel_DistributeQingNiaoAgent AS DQN WITH ( NOLOCK )
                    GROUP BY DQN.MaterielId ,
                            DQN.Status
                    HAVING  DQN.Status = 0
                            AND AND MIN(DistributeDate) BETWEEN '{0}' AND '{1}'
                ", DateTime.Now.AddDays(-RequetQuery.DistributeQueryDateOffset),
                DateTime.Now.AddDays(-1));

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<MaterielDistributeQingNiaoAgent>()
            {
                StrSql = sbSql.ToString(),
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}