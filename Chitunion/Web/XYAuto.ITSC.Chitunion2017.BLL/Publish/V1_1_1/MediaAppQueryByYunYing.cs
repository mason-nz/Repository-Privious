/********************************************************
*创建人：lixiong
*创建时间：2017/6/9 16:42:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1
{
    public class MediaAppQueryByYunYing : PublishInfoQueryClient<RequestMediaAppQueryDto, RespMediaAppByYunYingDto>
    {
        public MediaAppQueryByYunYing(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespMediaAppByYunYingDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                        SELECT  MBPC.RecID AS BaseMediaID ,
                                MBPC.Name ,
                                MBPC.HeadIconURL ,
                                MBPC.DailyLive ,
                                MBPC.ProvinceID ,
                                AI1.AreaName AS ProvinceName ,
                                MBPC.CityID ,
                                AI2.AreaName AS CityName ,
                                VUI.SysName AS TrueName ,
                                MBPC.CreateTime,
                                RI.RoleName AS CreateUserRole
                                ,CommonlyClassStr = (SELECT  STUFF(( SELECT  '|' + ( ISNULL(
															( CAST(DIF.DictId AS VARCHAR(15)) + ','
                                                             + DIF.DictName ) +','
															 + CAST(MC.SortNumber AS VARCHAR(15)), '') )
							                    FROM    dbo.MediaCategory AS MC WITH ( NOLOCK )
									                    LEFT JOIN dbo.DictInfo AS DIF WITH ( NOLOCK ) ON MC.CategoryID = DIF.DictId
							                    WHERE   MC.MediaType = {0}
									                    AND MC.WxID = MBPC.RecID
							                    ORDER BY MC.SortNumber DESC
						                      FOR
							                    XML PATH('')
						                      ), 1, 1, '')
                                          )
                                ,HasOnPub=(
								    SELECT  CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END
								    FROM DBO.Publish_BasicInfo AS PB WITH(NOLOCK)
								    WHERE PB.MediaType = {0}
								    AND PB.MediaID = MBPC.RecID
								    AND PB.Wx_Status = {1}
							    )
                        FROM    dbo.Media_BasePCAPP AS MBPC WITH ( NOLOCK )
                                LEFT JOIN dbo.AreaInfo AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MBPC.ProvinceID
                                LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MBPC.CityID
                                LEFT JOIN dbo.v_UserInfo AS VUI WITH ( NOLOCK ) ON VUI.UserID = MBPC.LastUpdateUserID
                                LEFT JOIN dbo.UserRole AS UR WITH ( NOLOCK ) ON UR.UserID = MBPC.LastUpdateUserID
                                LEFT JOIN dbo.RoleInfo AS RI WITH ( NOLOCK ) ON RI.RoleID = UR.RoleID
                        WHERE   MBPC.Status = 0
                        ", (int)MediaType.APP, (int)AppPublishStatus.已上架);

            if (!string.IsNullOrWhiteSpace(RequetQuery.SubmitUserRole))
            {
                sbSql.AppendFormat(" AND UR.RoleID = '{0}'", RequetQuery.SubmitUserRole.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MBPC.Name ='{0}'", RequetQuery.MediaName.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.SubmitUser))
            {
                sbSql.AppendFormat(" AND (VUI.TrueName = '{0}' OR VUI.Mobile = '{0}' OR VUI.UserName = '{0}' OR VUI.SysName='{0}') ", RequetQuery.SubmitUser.ToSqlFilter());
            }

            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespMediaAppByYunYingDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " BaseMediaID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespMediaAppByYunYingDto> GetResult(List<RespMediaAppByYunYingDto> resultList, QueryPageBase<RespMediaAppByYunYingDto> query)
        {
            if (resultList.Count == 0)
                return base.GetResult(resultList, query);

            resultList.ForEach(s =>
            {
                s.CommonlyClass = AppOperate.MapperToCommonlyClass(s.CommonlyClassStr);
            });
            return base.GetResult(resultList, query);
        }
    }
}