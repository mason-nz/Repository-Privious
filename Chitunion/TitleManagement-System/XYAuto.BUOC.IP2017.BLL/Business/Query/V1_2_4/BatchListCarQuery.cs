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
    public class BatchListCarQuery : DataListQueryClient<ReqInputListCarDto, ResInputListCarDto>
    {
        protected override DataListQuery<ResInputListCarDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            if (RequestQuery.BrandID == -2 && RequestQuery.MasterId==-2)
                sbSql.AppendFormat($@"
                                    SELECT  BM.BatchMediaID ,
                                            CB.MasterId ,
                                            BM.BrandID ,
                                            BM.SerialID ,
                                            CMB.Name MasterName ,
                                            CB.Name BrandName ,
                                            CS.ShowName SerialName ,
                                            BM.SubmitTime CreateTime ,
                                            BM.AuditTime ,
                                            VUI.SysName AuditUser
                                    FROM    dbo.BatchMedia BM
                                            JOIN BaseData2017.dbo.CarBrand CB ON CB.BrandID = BM.BrandID
                                            JOIN BaseData2017.dbo.CarMasterBrand CMB ON CMB.MasterID = CB.MasterId
                                            LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.AuditUserID
                                            LEFT JOIN BaseData2017.dbo.CarSerial CS ON CS.SerialID = BM.SerialID
                                    WHERE   BM.CreateUserID = {RequestQuery.CurrentUserID}
                                            AND BM.TaskType > {(int)Entities.ENUM.ENUM.EnumTaskType.媒体}
                            ");
            else if (RequestQuery.BrandID == -2)
                sbSql.AppendFormat($@"
                                    SELECT  BM.BatchMediaID ,
                                            CB.MasterId ,
                                            BM.BrandID ,                                            
                                            CMB.Name MasterName ,
                                            CB.Name BrandName ,
                                            BM.SubmitTime CreateTime ,
                                            BM.AuditTime ,
                                            VUI.SysName AuditUser
                                    FROM    dbo.BatchMedia BM
                                            JOIN BaseData2017.dbo.CarBrand CB ON CB.BrandID = BM.BrandID
                                            JOIN BaseData2017.dbo.CarMasterBrand CMB ON CMB.MasterID = CB.MasterId
                                            LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.AuditUserID
                                    WHERE   BM.CreateUserID = {RequestQuery.CurrentUserID}
                                            AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                                            AND CB.MasterId = {RequestQuery.MasterId}
                            ");
            else
                sbSql.AppendFormat($@"
                                    SELECT  BM.BatchMediaID ,
                                            CB.MasterId ,
                                            BM.BrandID ,
                                            BM.SerialID ,
                                            CMB.Name MasterName ,
                                            CB.Name BrandName ,
                                            CS.ShowName SerialName ,
                                            BM.SubmitTime CreateTime ,
                                            BM.AuditTime ,
                                            VUI.SysName AuditUser
                                    FROM    dbo.BatchMedia BM
                                            JOIN BaseData2017.dbo.CarBrand CB ON CB.BrandID = BM.BrandID
                                            LEFT JOIN BaseData2017.dbo.CarSerial CS ON CS.SerialID = BM.SerialID
                                            JOIN BaseData2017.dbo.CarMasterBrand CMB ON CMB.MasterID = CB.MasterId
                                            LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.AuditUserID
                                    WHERE   BM.CreateUserID = {RequestQuery.CurrentUserID} 
                                            AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                                            AND CB.MasterId = {RequestQuery.MasterId}
                                            AND CB.BrandID = {RequestQuery.BrandID}
                            ");

            if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND BM.SubmitTime >= '{0}'", RequestQuery.StartDate.Date);

            if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND BM.SubmitTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

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
            return new DataListQuery<ResInputListCarDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " CreateTime DESC",
                PageSize = RequestQuery.PageSize,
                PageIndex = RequestQuery.PageIndex
            };
        }
    }
}
