using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Task;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Media
{
    /// <summary>
    /// auth:lixiong
    /// desc:个人中心-已绑定-微信列表
    /// </summary>
    public class BindingsWeiXinQuery
           : PublishInfoQueryClient<ReqMediaBindingsDto, RespWeiXinBindingsDto>
    {
        public BindingsWeiXinQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespWeiXinBindingsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    
                SELECT  WX.RecID AS MediaId,
                        WX.AppID ,
                        WX.NickName,
                        WX.WxNumber ,
                        WX.HeadImg ,
                        WX.QrCodeUrl ,
                        WX.FansCount ,
                        PricesInfo = ( SELECT   STUFF(( SELECT  '|'
                                                                + ISNULL(CAST(PD.ADPosition1 AS VARCHAR(100)), '') + ','
                                                                + ISNULL(CAST(PD.ADPosition2 AS VARCHAR(100)), '') + ','
                                                                + ISNULL(CAST(PD.Price AS VARCHAR(100)), '')
                                                        FROM    dbo.LE_PublishDetailInfo AS PD WITH ( NOLOCK )
                                                                LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = PD.ADPosition1
                                                                LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON DC2.DictId = PD.ADPosition2
                                                        WHERE   PD.MediaType = 14001
                                                                AND PD.MediaID = WX.RecID
                                                      FOR XML PATH('')
                                                      ), 1, 1, '')
                                     )
                FROM    dbo.LE_Weixin AS WX WITH ( NOLOCK )
                WHERE   WX.CreateUserID = {0}
                        ", RequetQuery.UserId);

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespWeiXinBindingsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MediaId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespWeiXinBindingsDto> GetResult(List<RespWeiXinBindingsDto> resultList,
            QueryPageBase<RespWeiXinBindingsDto> query)
        {
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            resultList.ForEach(SetPricesInfos);

            return base.GetResult(resultList, query);

        }

        public static void SetPricesInfos(RespWeiXinBindingsDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.PricesInfo))
            {
                return;
            }
            var list = GetArrayValue(dto.PricesInfo);

            var adPosition1List = list.Where(s => s.AdPosition1 == (int)ADPosition1Enum.单图文).ToList();

            dto.First = new PriceWeiXinInfo()
            {
                Forward = adPosition1List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.转发).Select(s => s.Price).FirstOrDefault(),
                OriginalPublish = adPosition1List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.原创十发布).Select(s => s.Price).FirstOrDefault(),
            };

            var adPosition2List = list.Where(s => s.AdPosition1 == (int)ADPosition1Enum.多图文头条).ToList();

            dto.Second = new PriceWeiXinInfo()
            {
                Forward = adPosition2List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.转发).Select(s => s.Price).FirstOrDefault(),
                OriginalPublish = adPosition2List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.原创十发布).Select(s => s.Price).FirstOrDefault(),
            };

            var adPosition3List = list.Where(s => s.AdPosition1 == (int)ADPosition1Enum.多图文第二条).ToList();

            dto.Third = new PriceWeiXinInfo()
            {
                Forward = adPosition3List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.转发).Select(s => s.Price).FirstOrDefault(),
                OriginalPublish = adPosition3List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.原创十发布).Select(s => s.Price).FirstOrDefault(),
            };
            var adPosition4List = list.Where(s => s.AdPosition1 == (int)ADPosition1Enum.多图文3一N条).ToList();

            dto.Fourth = new PriceWeiXinInfo()
            {
                Forward = adPosition4List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.转发).Select(s => s.Price).FirstOrDefault(),
                OriginalPublish = adPosition4List.Where(s => s.AdPosition2 == (int)ADPosition4Enum.原创十发布).Select(s => s.Price).FirstOrDefault(),
            };
        }



        public static List<PricesInfoDto> GetArrayValue(string prices)
        {
            /*
             6001,8001,213.0000|6001,8002,23.0000
            */
            var respList = new List<PricesInfoDto>();

            if (string.IsNullOrWhiteSpace(prices))
            {
                return respList;
            }
            var spPriceInfo = prices.Split('|');

            foreach (var sp in spPriceInfo)
            {
                if (string.IsNullOrWhiteSpace(sp)) continue;

                var spl = sp.Split(',');
                respList.Add(new PricesInfoDto()
                {
                    AdPosition1 = VerifyOperateBase.GetArrayContent(spl, 0).ToInt(),
                    AdPosition2 = VerifyOperateBase.GetArrayContent(spl, 1).ToInt(),
                    Price = Convert.ToDecimal(VerifyOperateBase.GetArrayContent(spl, 2))
                });
            }


            return respList;
        }
    }

    public class PricesInfoDto
    {
        public int AdPosition1 { get; set; }
        public int AdPosition2 { get; set; }
        public decimal Price { get; set; }
    }
}
