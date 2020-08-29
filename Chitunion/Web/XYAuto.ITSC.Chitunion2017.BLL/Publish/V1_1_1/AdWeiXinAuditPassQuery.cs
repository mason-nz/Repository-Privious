/********************************************************
*创建人：lixiong
*创建时间：2017/5/11 16:34:06
*说明：广告审核通过的列表信息(分页数据有包含数据)
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1
{
    public class AdWeiXinAuditPassQuery : PublishInfoQueryClient<RequestAdQueryDto, RespAdWeiXinAuditPassDto>
    {
        public AdWeiXinAuditPassQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespAdWeiXinAuditPassDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                    SELECT  MW.MediaID ,
                            MW.ADName ,
                            WO.RecID AS WxId ,
                            WO.WxNumber AS Number ,
                            WO.NickName AS Name ,
                            WO.HeadImg AS HeadIconURL ,
                            WO.IsAreaMedia,
                            BusinessParams = ISNULL(STUFF(( SELECT  '|'
                                                            + ISNULL(CONVERT(VARCHAR(10), PB.BeginTime, 120),
                                                                     0) + ','
                                                            + ISNULL(CONVERT(VARCHAR(10), PB.EndTime, 120),
                                                                     0) + '@='
                                                            + ( SELECT  RTRIM(CONVERT(VARCHAR(50), ISNULL(MIN(SalePrice),
                                                                                  0))) + ','
                                                                        + RTRIM(CONVERT(VARCHAR(50), ISNULL(MAX(SalePrice),
                                                                                  0)))
                                                                FROM    dbo.Publish_DetailInfo AS PD WITH ( NOLOCK )
                                                                WHERE   PD.MediaType = {0}
                                                                        AND PB.PubID = PD.PubID
                                                              ) ,
                                                            +'@='
                                                            + RTRIM(CONVERT(VARCHAR(10), PB.Wx_Status))
                                                            + '@='
                                                            + RTRIM(CONVERT(VARCHAR(20), PB.PubID))
                                                    FROM    dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                    WHERE   PB.MediaID = MW.MediaID
                                                            AND PB.IsDel = 0
                                                            AND PB.MediaType = {0}
                                                            AND PB.Wx_Status IN ( {1} )--列表查询条件
                                                            #BusinessParamsSqlWhere#
                                                            #ExpirationTime#
                                                  FOR
                                                    XML PATH('')
                                                  ), 1, 1, ''), '')
                    FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )
                            INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID AND WO.Status = 0
                             #BaseSqlWhere#
                        ", (int)MediaType.WeiXin, RequetQuery.Wx_Status.Trim(',').ToSqlFilter());

            var sbExpirationTime = new StringBuilder();
            var sbWhereSql = new StringBuilder();
            var sbBaseSql = new StringBuilder();
            var sbBusinessParamsSqlWhere = new StringBuilder();
            sbSql.AppendLine("  WHERE 1= 1 AND MW.Status = 0 ");

            sbSql.AppendFormat(@"
                                AND MW.MediaID IN (
			                                SELECT DISTINCT PB.MediaID FROM DBO.Publish_BasicInfo AS PB WITH(NOLOCK)
			                                WHERE PB.IsDel = 0
                                                  AND PB.MediaType = {0} --媒体类型
                                                  #sbWhereSql#
                                                  #ExpirationTime#
		                                )
                                ", (int)MediaType.WeiXin);

            //默认加载是已通过的（Wx_Status 字段是组合查询）(int)PublishBasicStatusEnum.已通过
            sbWhereSql.AppendFormat(@" AND PB.Wx_Status IN ({0}) ", RequetQuery.Wx_Status.Trim(',').ToSqlFilter());

            if (ConfigEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
               ConfigEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
            }
            else if (ConfigEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //AE角色 - 到角色,媒体主 - 个人
                sbWhereSql.AppendFormat(@" AND PB.CreateUserID IN (SELECT UserID
                                      FROM   dbo.UserRole
                                      WHERE  RoleID = '{0}' AND Status = 0)", RoleInfoMapping.AE);
            }
            else
            {
                sbWhereSql.AppendFormat(" AND PB.CreateUserID = {0}", RequetQuery.CreateUserId);
            }
            if (RequetQuery.IsAreaMedia != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@"  AND WO.IsAreaMedia = {0}", RequetQuery.IsAreaMedia);
                var areaWhere = string.Empty;
                if (RequetQuery.AreaProvniceId != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    areaWhere += $" AND MAM.ProvinceID = {RequetQuery.AreaProvniceId }";
                }
                if (RequetQuery.AreaCityId != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    areaWhere += $" AND MAM.CityID = {RequetQuery.AreaCityId }";
                }
                if (!string.IsNullOrWhiteSpace(areaWhere))
                {
                    sbSql.AppendFormat(@" AND EXISTS(
											SELECT 1 FROM dbo.Media_Area_Mapping AS MAM WITH ( NOLOCK )
											WHERE MAM.MediaType = {1} AND MAM.MediaID = MW.MediaID
											AND MAM.RelateType = {2}
                                            {0}
                                            )", areaWhere, (int)MediaType.WeiXin, (int)MediaAreaMappingType.AreaMedia);
                }
            }
            //媒体帐号、名称(或所属媒体)
            if (!string.IsNullOrWhiteSpace(RequetQuery.Keyword))
            {
                sbSql.AppendFormat(@"  AND (WO.WxNumber = '{0}' OR WO.NickName = '{0}')", RequetQuery.Keyword.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.AdName))
            {
                sbSql.AppendFormat(@"  AND MW.ADName LIKE '%{0}%'", RequetQuery.AdName.ToSqlFilter());
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDateTime))
            {
                sbWhereSql.AppendFormat(@"  AND PB.CreateTime >= CONVERT(VARCHAR(24),'{0}',120) ", RequetQuery.StartDateTime.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.EndDateTime))
            {
                var date = DateTime.Parse(RequetQuery.EndDateTime.ToSqlFilter()).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                sbWhereSql.AppendFormat(@"  AND PB.CreateTime <= CONVERT(VARCHAR(24),'{0}',120) ", date);
            }

            //过期时间
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbExpirationTime.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }

            sbSql = sbSql.Replace("#sbWhereSql#", sbWhereSql.ToString());
            sbSql = sbSql.Replace("#ExpirationTime#", sbExpirationTime.ToString());
            sbSql = sbSql.Replace("#BusinessParamsSqlWhere#", sbBusinessParamsSqlWhere.ToString());
            sbSql = sbSql.Replace("#BaseSqlWhere#", sbBaseSql.ToString());
            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespAdWeiXinAuditPassDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " WxId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override BaseResponseEntity<RespAdWeiXinAuditPassDto> GetResult(List<RespAdWeiXinAuditPassDto> resultList, QueryPageBase<RespAdWeiXinAuditPassDto> query)
        {
            /*
             item.BusinessParams
              第一种：  2017-04-01,2017-04-08@=9.9000,9.9000@=42001@=19722
              第二种：  2017-05-11,2017-05-12@Price=2.4200,111.0000@Wx_Status=42001@PubID=30642
                    |2017-05-11,2017-05-12@Price=2.4200,111.0000@Wx_Status=42001@PubID=30643
                    |2017-05-11,2017-05-26@Price=2.4200,7.2600@Wx_Status=42001@PubID=30644
                    |2017-05-11,2017-05-26@Price=2.4200,222.0000@Wx_Status=42001@PubID=30645
                    |2017-05-11,2017-05-25@Price=4.8400,11.0000@Wx_Status=42001@PubID=30646
                    |2017-05-11,2017-05-12@Price=4.8400,11.0000@Wx_Status=42001@PubID=30647
                    |2017-05-11,2017-05-12@Price=4.8400,11.0000@Wx_Status=42001@PubID=30648
                    |2017-05-11,2017-05-12@Price=7.2600,12.1000@Wx_Status=42001@PubID=30649

            将@Price= 、@Wx_Status= 、@PubID= 换成@=
             */
            if (resultList.Count == 0)
                return base.GetResult(resultList, query);

            foreach (var item in resultList)
            {
                if (string.IsNullOrWhiteSpace(item.BusinessParams)) continue;
                var split1 = item.BusinessParams.Split('|');
                item.AdItemInfo = new List<AdItemInfo>();
                //2017-04-01,2017-04-08@=9.9000,9.9000@=42001@=19722
                foreach (var sp1 in split1)
                {
                    if (string.IsNullOrWhiteSpace(sp1)) continue;

                    var itemInfo = new AdItemInfo();
                    var sp2 = sp1.Split(new string[] { "@=" }, StringSplitOptions.None);
                    itemInfo.TermOfValidity = sp2[0];
                    itemInfo.ReferencePrice = GetContent(sp2, 1);//sp2.Length > 1 ? sp2[1] : string.Empty;
                    itemInfo.AdStatus = GetContent(sp2, 2); //sp2.Length > 2 ? sp2[2] : string.Empty;
                    itemInfo.PubID = GetContent(sp2, 3); //sp2.Length > 3 ? sp2[3] : string.Empty;
                    item.AdItemInfo.Add(itemInfo);
                }
            }

            return base.GetResult(resultList, query);
        }

        /// <summary>
        /// 获取数组中的元素
        /// </summary>
        /// <param name="arrStrings">数组</param>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        public static string GetContent(string[] arrStrings, int index)
        {
            if (arrStrings.Length > index)
            {
                var value = arrStrings[index];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim(',');
                }
            }
            return string.Empty;
        }
    }
}