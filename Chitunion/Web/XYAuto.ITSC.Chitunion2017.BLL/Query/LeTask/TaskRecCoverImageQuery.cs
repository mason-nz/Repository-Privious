using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Task;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.LeTask
{
    /// <summary>
    /// auth:lixiong
    /// desc:任务领取-贴片广告列表  （内容分发列表）
    /// </summary>
    public class TaskRecCoverImageQuery
            : PublishInfoQueryClient<ReqOrderCoverImageDto, RespReceiveTaskInfoDto>
    {
        public TaskRecCoverImageQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespReceiveTaskInfoDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    SELECT  T.RecID AS TaskId,
                            T.TaskName ,
                            T.BillingRuleName ,
                            T.MaterialUrl ,
                            T.MaterialID ,
                            T.TaskType ,
                            T.RuleCount ,
                            T.TakeCount ,    
                            T.Status ,
                            T.TaskAmount ,
                            T.CPCPrice ,
                            T.CPLPrice ,          
                            T.ImgUrl ,
                            T.Synopsis ,
                            T.CategoryID ,
                            T.CPCLimitPrice ,
                            T.CPLLimitPrice
                    FROM    dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                    WHERE 1 =1 AND T.Status = {0}
                        ", (int)LeTaskStatusEnum.Ing);

            if (RequetQuery.TaskType != LeTaskTypeEnum.None)
            {
                sbSql.AppendFormat($" AND T.TaskType = { (int)RequetQuery.TaskType}");
            }

            if (RequetQuery.Category != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND T.CategoryID = { RequetQuery.Category}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespReceiveTaskInfoDto>()
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
