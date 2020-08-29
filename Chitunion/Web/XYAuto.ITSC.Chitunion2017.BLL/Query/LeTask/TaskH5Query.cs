using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.OpenXmlFormats.Dml;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Task;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Task;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.LeTask
{
    public class TaskH5Query
         : PublishInfoQueryClient<ReqTaskH5Dto, RespTaskH5Dto>
    {
        public TaskH5Query(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespTaskH5Dto> GetQueryParams()
        {

            var randomNumber = ConfigurationUtil.GetAppSettingValue("GetRandomNumber", false) ?? "21,155.22";
            
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    SELECT  T.RecID AS TaskId ,
                            T.MaterialID ,
                            T.TaskName,
                            T.Synopsis,
                            T.MaterialUrl,
                            AA.ArticleID ,
                            AA.HeadImg ,
                            AA.HeadImg2 ,
                            AA.HeadImg3 ,
                            ReadCount = ( SELECT dbo.[f_GenArticleReadNum](AA.CreateTime, {2}) --21,155.22
                                        )
                    FROM    dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                            INNER JOIN Chitunion_OP2017.dbo.MaterielExtend AS ME WITH ( NOLOCK ) ON ME.MaterielID = T.MaterialID
                            INNER JOIN Chitunion_OP2017.dbo.AccountArticle AS AA ON AA.ArticleID = ME.ArticleID
                    WHERE   T.TaskType = {0} AND T.Status = {1}
                        ", (int)LeTaskTypeEnum.ContentDistribute, (int)LeTaskStatusEnum.Ing, randomNumber);

            if (RequetQuery.Category != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND T.CategoryID = {RequetQuery.Category}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespTaskH5Dto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " TaskId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}
