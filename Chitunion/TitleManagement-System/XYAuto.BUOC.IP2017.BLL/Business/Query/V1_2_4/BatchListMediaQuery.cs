using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4;
using XYAuto.BUOC.IP2017.Entities.Query;

namespace XYAuto.BUOC.IP2017.BLL.Business.Query.V1_2_4
{
    public class BatchListMediaQuery : DataListQueryClient<ReqBatchListMediaDto, ResBatchListMediaDto>
    {
        protected override DataListQuery<ResBatchListMediaDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.Append($@"
                            SELECT  BM.BatchMediaID ,
                                    BM.MediaName Name ,
                                    BM.HeadImg ,
                                    BM.MediaNumber Number ,
                                    BM.MediaType ,
                                    BM.IsSelfDo ,
                                    BM.SubmitTime CreateTime ,
                                    BM.AuditTime ,
                                    BM.Status ,
                                    VUI.SysName AuditUser
                            FROM    dbo.BatchMedia BM
                                    JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.CreateUserID
                            WHERE   BM.CreateUserID = {RequestQuery.CurrentUserID} 
                                    AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.媒体} ");

            if (RequestQuery.MediaType != -2)
                sbSql.Append($@" AND BM.MediaType = {RequestQuery.MediaType} ");

            if (RequestQuery.SelfDoBusiness != -2)
                sbSql.Append($@" AND BM.IsSelfDo = {RequestQuery.SelfDoBusiness} ");

            if (!(string.IsNullOrEmpty(RequestQuery.Name)))
                sbSql.Append($@" AND (BM.MediaNumber LIKE '%{RequestQuery.Name}%' OR BM.MediaName LIKE '%{RequestQuery.Name}%')");

            if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND BM.SubmitTime >= '{0}'", RequestQuery.StartDate.Date);

            if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
            {
                sbSql.AppendFormat(@" AND BM.SubmitTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));
            }
            if (RequestQuery.Status != -2)
            {
                if (RequestQuery.Status == (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.审核中)
                {
                    sbSql.AppendFormat($@" AND BM.Status IN ({(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.审核中},{(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审}) ");
                }
                else
                    sbSql.AppendFormat($@" AND BM.Status = {RequestQuery.Status} ");
            }
            sbSql.AppendLine(@") T");
            var query = new DataListQuery<ResBatchListMediaDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " CreateTime DESC",
                PageSize = RequestQuery.PageSize,
                PageIndex = RequestQuery.PageIndex
            };
            return query;
        }
    }
}
