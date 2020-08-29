using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_2_1
{
    public class ArticleQuery : PublishInfoQueryClient<Media.Dto.RequestDto.V1_2_1.RequestArticleQueryDto, Media.Dto.ResponseDto.V1_2_1.ResponseArticleListDto>
    {
        public ArticleQuery(Dto.ConfigEntity configEntity) : base(configEntity)
        {

        }
        protected override PublishQuery<Media.Dto.ResponseDto.V1_2_1.ResponseArticleListDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                            SELECT  RecID ArticleId ,
                                    Title ,
                                    Resource ,
                                    CopyrightState ,
                                    PublishTime ,
                                    ReadNum ,
                                    LikeNum ,
                                    ComNum ,
                                    Category
                            FROM    BaseData2017.dbo.ArticleInfo
                            WHERE   XyAttr = 2
                                    --AND RecID NOT IN ( @ArticleIds )
                                    --AND RecID IN ( SELECT   ArticleID
                                    --               FROM     NLP2017.dbo.TR_ArticleCarMapping
                                    --               WHERE    CSID = 2913 )
                            ");

            if (!string.IsNullOrWhiteSpace(RequetQuery.ArticleIds))
                sbSql.AppendFormat(@" AND RecID NOT IN ( {0} )", RequetQuery.ArticleIds);

            if (RequetQuery.CarSerialId != -2)
                sbSql.AppendFormat(@" AND RecID IN ( SELECT   ArticleID
                                                     FROM     NLP2017.dbo.TR_ArticleCarMapping
                                                     WHERE    CSID = {0} )", RequetQuery.CarSerialId);

            if (RequetQuery.Resource != -2)
                sbSql.AppendFormat(@" AND Resource = {0}", RequetQuery.Resource);

            if (RequetQuery.CopyrightState != -2)
                sbSql.AppendFormat(@" AND CopyrightState = {0}", RequetQuery.CopyrightState);

            if (RequetQuery.StartDate != Entities.Constants.Constant.DATE_INVALID_VALUE)
                sbSql.AppendFormat(@" AND PublishTime >= '{0}'", RequetQuery.StartDate.Date);

            if (RequetQuery.EndDate != Entities.Constants.Constant.DATE_INVALID_VALUE)
            {
                //sbSql.AppendFormat(@" AND PublishTime < '{0}'", DateTime.Parse(RequetQuery.EndDate).AddDays(1).ToString("yyyy-MM-dd"));
                sbSql.AppendFormat(@" AND PublishTime < '{0}'", RequetQuery.EndDate.Date.AddDays(1));
            }

            sbSql.AppendLine(@") T");
            var query = new PublishQuery<Media.Dto.ResponseDto.V1_2_1.ResponseArticleListDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = GetOrderBy(RequetQuery.OrderBy),
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        private string GetOrderBy(int orderBy)
        {
            var orderByStr = " PublishTime DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," PublishTime DESC "},
                {1002," PublishTime ASC "},
                {2001," ReadNum DESC "},
                {2002," ReadNum ASC "},
                {3001," LikeNum DESC "},
                {3002," LikeNum ASC "},
                {4001," ComNum DESC "},
                {4002," ComNum ASC "}
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == orderBy);
            return value.Value ?? orderByStr;
        }
    }
}
