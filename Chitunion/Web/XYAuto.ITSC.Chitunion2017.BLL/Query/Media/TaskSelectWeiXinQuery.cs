using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Media
{
   public class TaskSelectWeiXinQuery
        : PublishInfoQueryClient<ReqMediaBindingsDto, RespTaskSelectWeiXinDto>
    {
       public TaskSelectWeiXinQuery(ConfigEntity configEntity) : base(configEntity)
       {
       }

       protected override PublishQuery<RespTaskSelectWeiXinDto> GetQueryParams()
       {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                    
                SELECT  WX.RecID ,
                        WX.AppID ,
                        WX.WxNumber ,
                        WX.HeadImg ,
                        WX.QrCodeUrl ,
                        WX.FansCount 
                FROM    dbo.LE_Weixin AS WX WITH ( NOLOCK )
                WHERE   WX.CreateUserID = {0}
                        ", RequetQuery.UserId);

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespTaskSelectWeiXinDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " RecID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}
