using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Media
{
    /// <summary>
    /// auth:lixiong
    /// desc:个人中心-已绑定-微博列表
    /// </summary>
    public class BindingsWeiBoQuery
            : PublishInfoQueryClient<ReqMediaBindingsDto, RespWeiBoBindingsDto>
    {
        public BindingsWeiBoQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespWeiBoBindingsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    
                
                SELECT  WB.RecID AS MediaId,
                        WB.Number ,
                        WB.Name ,
                        WB.Sex ,
                        WB.HeadIconURL AS HeadImg,
                        WB.FansCount ,
                        WB.CategoryID ,
                        DC.DictName AS CategoryName ,
                        PricesInfo = ( SELECT   STUFF(( SELECT  '|'
                                                                + ISNULL(CAST(PD.ADPosition1 AS VARCHAR(100)), '') + ','
                                                                + ISNULL(CAST(PD.ADPosition2 AS VARCHAR(100)), '') + ','
                                                                + ISNULL(CAST(PD.Price AS VARCHAR(100)), '')
                                                        FROM    dbo.LE_PublishDetailInfo AS PD WITH ( NOLOCK )
                                                                LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = PD.ADPosition1
                                                                LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON DC2.DictId = PD.ADPosition2
                                                        WHERE   PD.MediaType = 14003
                                                                AND PD.MediaID = WB.RecID
                                                      FOR
                                                        XML PATH('')
                                                      ), 1, 1, '')
                                     )
                FROM    dbo.LE_Weibo AS WB WITH ( NOLOCK )
                        LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = WB.CategoryID
                WHERE   WB.CreateUserID = {0}
                        ", RequetQuery.UserId);

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespWeiBoBindingsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MediaId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespWeiBoBindingsDto> GetResult(List<RespWeiBoBindingsDto> resultList,
            QueryPageBase<RespWeiBoBindingsDto> query)
        {

            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            resultList.ForEach(SetPricesInfos);

            return base.GetResult(resultList, query);

        }

        public void SetPricesInfos(RespWeiBoBindingsDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.PricesInfo))
            {
                return;
            }
            var list = BindingsWeiXinQuery.GetArrayValue(dto.PricesInfo);

            var adPosition1List = list.Where(s => s.AdPosition1 == (int)ADPosition4Enum.转发).ToList();

            dto.Forward = adPosition1List.Select(s => s.Price).FirstOrDefault();

            var adPosition2List = list.Where(s => s.AdPosition1 == (int)ADPosition4Enum.直发).ToList();

            dto.Direct = adPosition2List.Select(s => s.Price).FirstOrDefault();

        }

    }
}
