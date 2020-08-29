/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 11:15:37
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_11
{
    public class MaterielQuery : PublishInfoQueryClient<RequestMaterielQueryDto, RespMaterielListDto>
    {
        public MaterielQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespMaterielListDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                            SELECT  MET.MaterielID,
                                    MET.Name AS MaterielName,
                                    MET.CreateTime ,
                                    MET.ThirdID ,
                                    MET.ArticleFrom ,
                                    DC.DictName AS ArticleFromName ,
                                    MET.ContractNumber ,
		                            CS.SerialID,
		                            CS.ShowName AS SerialName,
		                            CB.Name AS BrandName,
                                    ChannelCount = ( SELECT COUNT(*)
                                                     FROM   dbo.MaterielChannel AS MCL WITH ( NOLOCK )
                                                     WHERE  MCL.MaterielID = MET.MaterielID
                                                   )
                            FROM    dbo.MaterielExtend AS MET WITH ( NOLOCK )
                                    LEFT JOIN BaseData2017.dbo.CarSerial AS CS WITH ( NOLOCK ) ON CS.SerialID = MET.SerialID
		                            LEFT JOIN BaseData2017.DBO.CarBrand AS CB WITH(NOLOCK) ON CB.BrandID = CS.BrandID
                                    LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = MET.ArticleFrom
                            WHERE 1 = 1
                            ");

            if (RequetQuery.CarSerialId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND MET.SerialID = {0}", RequetQuery.CarSerialId);
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.ContractNumber))
            {
                sbSql.AppendFormat(@" AND MET.ContractNumber LIKE '%{0}%'", RequetQuery.ContractNumber.ToSqlFilter());
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.SatrtDate))
            {
                sbSql.AppendFormat(@" AND MET.CreateTime >= '{0}'", DateTime.Parse(RequetQuery.SatrtDate).ToString("yyyy-MM-dd"));
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.EndDate))
            {
                sbSql.AppendFormat(@" AND MET.CreateTime < '{0}'", DateTime.Parse(RequetQuery.EndDate).AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.MaterielName))
            {
                sbSql.AppendFormat(@" AND MET.Name LIKE '%{0}%'", RequetQuery.MaterielName.ToSqlFilter());
            }

            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespMaterielListDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MaterielID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}