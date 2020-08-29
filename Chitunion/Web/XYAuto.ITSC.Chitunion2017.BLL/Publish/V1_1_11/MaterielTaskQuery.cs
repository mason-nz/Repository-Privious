/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 11:12:22
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_11
{
    public class MaterielTaskQuery
        : PublishInfoQueryClient<RequestTaskSchedulerDto, TaskSchedulerDto>
    {
        public MaterielTaskQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<TaskSchedulerDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                        SELECT  a.RecID ,
                                a.ArticleID ,
                                g.GroupID ,
                                g.CarBrandID ,
                                g.CSID AS SerialId ,
                                a.XyAttr ,
                                ai.Url ,
                                REPLACE(ai.Title, CHAR(10), '') AS Title ,
                                ai.HeadImg ,
                                ai.ReadNum ,
                                ai.LikeNum ,
                                REPLACE(ai.Abstract, CHAR(10), '') AS Abstract ,
                                ai.CopyrightState ,
                                ai.CarSerial ,
                                ai.Resource ,
                                ai.Category ,
                                ai.PublishTime ,
                                ai.CreateTime ,
                                CS.ShowName AS SerialName ,
                                CB.Name AS BrandName ,
                                DC.DictId AS TaskStatus ,
                                DC.DictName AS TaskStatusName,
                                UI.SysName AS UserName
                        FROM    dbo.TaskScheduler_User AS TSU WITH ( NOLOCK )
                                INNER JOIN NLP2017.dbo.TR_GroupInfo AS g ON g.GroupID = TSU.GroupID
                                INNER JOIN NLP2017.dbo.TR_ArticleInfo AS a ON g.GroupID = a.GroupID
                                INNER JOIN BaseData2017.dbo.ArticleInfo AS ai ON ai.RecID = a.ArticleID
                                LEFT JOIN BaseData2017.dbo.CarSerial AS CS WITH ( NOLOCK ) ON CS.SerialID = g.CSID
                                LEFT JOIN BaseData2017.dbo.CarBrand AS CB WITH ( NOLOCK ) ON CB.BrandID = g.CarBrandID
                                LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = TSU.TaskStatus
                                LEFT JOIN DBO.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = TSU.UserId
                        WHERE   TSU.UserId = {0}
                        AND	    g.TaskID IN
								(SELECT  TaskID
									FROM    NLP2017.dbo.SubscribeTaskInfo
									WHERE   TypeID = 1
											AND CONVERT(VARCHAR(10), BeginTime, 120)
											BETWEEN '{1}' AND '{2}')
                                AND ai.XyAttr = 1
                                #BaseSqlWhere#

                        ", ConfigEntity.CreateUserId, RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());

            var baseSqlWhere = new StringBuilder();

            baseSqlWhere.AppendFormat(@"
                        AND CONVERT(VARCHAR(10), g.CreateTime, 120)
                                BETWEEN '{0}' AND '{1}'
                                    ", RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                baseSqlWhere.AppendFormat(@" AND ai.Resource = {0}", RequetQuery.ChannelId);
            }
            if (RequetQuery.CarSerialId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                baseSqlWhere.AppendFormat(@" AND g.CSID = {0}", RequetQuery.CarSerialId);
            }
            //当前清洗员角色，不能查询别的用户
            //if (RequetQuery.UserId != Entities.Constants.Constant.INT_INVALID_VALUE)
            //{
            //    baseSqlWhere.AppendFormat(@" AND TSU.UserId = {0}", RequetQuery.UserId);
            //}
            if (!string.IsNullOrWhiteSpace(RequetQuery.Category))
            {
                baseSqlWhere.AppendFormat(@" AND ai.Category = '{0}'", RequetQuery.Category.ToSqlFilter());
            }

            if (RequetQuery.TaskStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                baseSqlWhere.AppendFormat(@" AND TSU.TaskStatus = {0}", RequetQuery.TaskStatus);
            }

            sbSql = sbSql.Replace("#BaseSqlWhere#", baseSqlWhere.ToString());

            sbSql.AppendLine(@") T");
            var query = new PublishQuery<TaskSchedulerDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " GroupID , SerialId ,XyAttr,RecID ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}