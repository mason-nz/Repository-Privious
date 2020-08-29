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
    public class InputListCarQuery : DataListQueryClient<ReqInputListCarDto, ResInputListCarDto>
    {
        protected override DataListQuery<ResInputListCarDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            if (RequestQuery.BrandID == -2)
                sbSql.AppendFormat($@"
                                    SELECT  CB.MasterId ,
                                            CB.BrandID ,
                                            CB.Name BrandName ,
                                            CB.CreateTime
                                    FROM    BaseData2017.dbo.CarBrand CB
                                    WHERE   CB.MasterId = {RequestQuery.MasterId}
                            ");
            else
                sbSql.AppendFormat($@"
                                    SELECT  CB.MasterId ,
                                            CB.BrandID ,
                                            CB.Name BrandName ,
                                            CS.ShowName SerialName ,
                                            CS.SerialID ,
                                            CS.CreateTime
                                    FROM    BaseData2017.dbo.CarBrand CB
                                            JOIN BaseData2017.dbo.CarSerial CS ON CB.BrandID = CS.BrandID
                                    WHERE   CB.MasterId = {RequestQuery.MasterId}
                                            AND CB.BrandID = {RequestQuery.BrandID}
                            ");

            if (RequestQuery.BrandID == -2)
            {
                if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                    sbSql.AppendFormat(@" AND CB.CreateTime >= '{0}'", RequestQuery.StartDate.Date);

                if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                    sbSql.AppendFormat(@" AND CB.CreateTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

                #region V2.0.3未录已示标签
                //if (RequestQuery.LabelStatus == 0)//未录标签
                //    sbSql.AppendFormat($@"
                //                        AND NOT EXISTS ( SELECT  1
                //                                            FROM    dbo.BatchMedia BM
                //                                            WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                //                                                    AND BM.BrandID = {RequestQuery.BrandID})");
                //else if (RequestQuery.LabelStatus == 1)//已录标签
                //    sbSql.AppendFormat($@"
                //                        AND EXISTS ( SELECT  1
                //                                            FROM    dbo.BatchMedia BM
                //                                            WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                //                                            AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}     
                //                                                    AND BM.BrandID = {RequestQuery.BrandID})");
                #endregion
                if (RequestQuery.LabelStatus == 0)//未录标签
                {
                    sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT  1
                                                            FROM    dbo.MediaLabelResult MLR
                                                            WHERE   MLR.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                                                                    AND MLR.Status = 0 
                                                                    AND MLR.BrandID = CB.BrandID)");

                    sbSql.AppendFormat($@"
                                            AND NOT EXISTS ( SELECT  1
                                                                FROM    dbo.BatchMedia BM
                                                                WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                                                                        AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                        AND BM.BrandID = {RequestQuery.BrandID})");
                }
                else if (RequestQuery.LabelStatus == 1)//已录标签
                {
                    sbSql.AppendFormat($@"
                                        AND (EXISTS ( SELECT  1
                                                            FROM    dbo.MediaLabelResult MLR
                                                            WHERE   MLR.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                                                                    AND MLR.Status = 0 
                                                                    AND MLR.BrandID = CB.BrandID)");

                    sbSql.AppendFormat($@"
                                            OR EXISTS ( SELECT  1
                                                                FROM    dbo.BatchMedia BM
                                                                WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}     
                                                                        AND BM.BrandID = {RequestQuery.BrandID}))");
                }
            }
            else
            {
                if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                    sbSql.AppendFormat(@" AND CS.CreateTime >= '{0}'", RequestQuery.StartDate.Date);

                if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                    sbSql.AppendFormat(@" AND CS.CreateTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

                #region V2.0.3未录已录标签
                //if (RequestQuery.LabelStatus == 0)//未录标签
                //    sbSql.AppendFormat($@"
                //                        AND NOT EXISTS ( SELECT  1
                //                                            FROM    dbo.BatchMedia BM
                //                                            WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                //                                                    AND BM.BrandID = {RequestQuery.BrandID}
                //                                                    AND BM.SerialID = CS.SerialID )");
                //else if (RequestQuery.LabelStatus == 1)//已录标签
                //    sbSql.AppendFormat($@"
                //                        AND EXISTS ( SELECT  1
                //                                            FROM    dbo.BatchMedia BM
                //                                            WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}     
                //                                                    AND BM.BrandID = {RequestQuery.BrandID}
                //                                                    AND BM.SerialID = CS.SerialID )");
                #endregion
                if (RequestQuery.LabelStatus == 0)//未录标签
                {
                    sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT  1
                                                            FROM    dbo.MediaLabelResult MLR
                                                            WHERE   MLR.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                                                                    AND MLR.Status = 0 
                                                                    AND MLR.BrandID = {RequestQuery.BrandID}
                                                                    AND MLR.SerialID = CS.SerialID )");

                    sbSql.AppendFormat($@"
                                            AND NOT EXISTS ( SELECT  1
                                                                FROM    dbo.BatchMedia BM
                                                                WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                                                                        AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                        AND BM.BrandID = {RequestQuery.BrandID}
                                                                        AND BM.SerialID = CS.SerialID )");
                }
                else if (RequestQuery.LabelStatus == 1)//已录标签
                {
                    sbSql.AppendFormat($@"
                                        AND (EXISTS ( SELECT  1
                                                            FROM    dbo.MediaLabelResult MLR
                                                            WHERE   MLR.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                                                                    AND MLR.Status = 0 
                                                                    AND MLR.BrandID = {RequestQuery.BrandID}
                                                                    AND MLR.SerialID = CS.SerialID )");

                    sbSql.AppendFormat($@"
                                        OR EXISTS ( SELECT  1
                                                            FROM    dbo.BatchMedia BM
                                                            WHERE   BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}     
                                                                    AND BM.BrandID = {RequestQuery.BrandID}
                                                                    AND BM.SerialID = CS.SerialID ))");
                }
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
