using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Distribute
{
    //物料分发-渠道明细表（多个渠道的数据统计）
    public partial class MaterielChannelDetailed : DataBase
    {
        public static readonly MaterielChannelDetailed Instance = new MaterielChannelDetailed();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Distribute.MaterielChannelDetailed entity, DateTime createTime)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat($@"
IF (NOT EXISTS(SELECT 1 FROM DBO.Materiel_ChannelDetailed WITH(NOLOCK)
		WHERE DistributeId = {entity.DistributeId} AND Source = {entity.Source}
                AND Date = '{entity.Date.ToString("yyyy-MM-dd")}' ))
BEGIN
        INSERT INTO DBO.Materiel_ChannelDetailed
                            ( MaterielId ,
                              ChannelId ,
                              DistributeId ,
                              ChannelName ,
                              ChirldChannelName ,
                              Date ,
                              PV ,
                              UV ,
                              OnLineAvgTime ,
                              BrowsePageAvg ,
                              JumpProportion ,
                              InquiryNumber ,
                              SessionNumber ,
                              TelConnectNumber ,
                              Source ,
                              DistributeDetailType ,
                              Status ,
                              CreateTime
                            ) VALUES
");
            strSql.AppendFormat($@"({entity.MaterielId},{entity.ChannelId},{entity.DistributeId},'{entity.ChannelName}','{entity.ChirldChannelName}',");
            strSql.AppendFormat($@"'{entity.Date}',{entity.PV},{entity.UV},{entity.OnLineAvgTime},{entity.BrowsePageAvg},{entity.JumpProportion},{entity.InquiryNumber},");
            strSql.AppendFormat($@"{entity.SessionNumber},{entity.TelConnectNumber},{entity.Source},{entity.DistributeDetailType},{entity.Status},GETDATE())");
            strSql.AppendLine();
            strSql.AppendFormat($@" UPDATE dbo.Materiel_DistributeDetailed SET DistributeDetailType  = {entity.DistributeDetailType}
                                    WHERE DistributeId = {entity.DistributeId}");
            strSql.AppendLine();
            strSql.AppendFormat(@"END");

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString());
        }

        public int Insert(List<Entities.Distribute.MaterielChannelDetailed> list)
        {
            if (!list.Any())
            {
                return 1;
            }
            var sbSql = new StringBuilder();

            sbSql.AppendFormat(@"
                    INSERT INTO DBO.Materiel_ChannelDetailed
                            ( MaterielId ,
                              ChannelId ,
                              DistributeId ,
                              ChannelName ,
                              ChirldChannelName ,
                              Date ,
                              PV ,
                              UV ,
                              OnLineAvgTime ,
                              BrowsePageAvg ,
                              JumpProportion ,
                              InquiryNumber ,
                              SessionNumber ,
                              TelConnectNumber ,
                              Source ,
                              Status ,
                              CreateTime
                            ) VALUES");

            list.ForEach(item =>
            {
                sbSql.AppendFormat($@"({item.MaterielId},{item.ChannelId},{item.DistributeId},'{item.ChannelName}','{item.ChirldChannelName}',");
                sbSql.AppendFormat($@"'{item.Date}',{item.PV},{item.UV},{item.OnLineAvgTime},{item.BrowsePageAvg},{item.JumpProportion},{item.InquiryNumber},");
                sbSql.AppendFormat($@"{item.SessionNumber},{item.TelConnectNumber},{item.Source},{item.Status},getdate()),");
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString().Trim(','));
        }

        public List<Entities.Distribute.MaterielChannelDetailed> GetList(int distributeId, List<int> distributeIds, int materielId)
        {
            var sql = @"
                        SELECT  CD.*
                        FROM    dbo.Materiel_ChannelDetailed AS CD WITH ( NOLOCK )
                        WHERE   CD.Status = 0
                        ";
            var parameters = new List<SqlParameter>();
            if (distributeId > 0)
            {
                sql += $" AND CD.DistributeId = @DistributeId";
                parameters.Add(new SqlParameter("@DistributeId", distributeId));
            }

            if (distributeIds.Any())
            {
                sql += $" AND CD.DistributeId IN ({string.Join(",", distributeIds)})";
            }
            if (materielId > 0)
            {
                sql += $" AND CD.MaterielId = @MaterielId";
                parameters.Add(new SqlParameter("@MaterielId", materielId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Distribute.MaterielChannelDetailed>(data.Tables[0]);
        }

        public List<Entities.Distribute.MaterielChannelDetailed> GetChannleList(DistributeQuery<Entities.Distribute.MaterielChannelDetailed> query)
        {
            var sql = @"
                        SELECT  DD.Date,
		                        CD.*
                        FROM    dbo.Materiel_DistributeDetailed AS DD WITH ( NOLOCK )
                        LEFT JOIN DBO.Materiel_ChannelDetailed AS CD WITH(NOLOCK) ON CD.DistributeId = DD.DistributeId
                        WHERE  DD.Status = 0 --AND CD.Id >0
                        ";
            var parameters = new List<SqlParameter>();
            if (query.MaterielId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.MaterielId = @MaterielId";
                parameters.Add(new SqlParameter("@MaterielId", query.MaterielId));
            }

            if (!string.IsNullOrWhiteSpace(query.StartDate))
            {
                sql += $" AND DD.Date >= @StartDate";
                parameters.Add(new SqlParameter("@StartDate", query.StartDate));
            }
            if (!string.IsNullOrWhiteSpace(query.EndDate))
            {
                sql += $" AND DD.Date <= @EndDate";
                parameters.Add(new SqlParameter("@EndDate", query.EndDate));
            }
            if (query.Source != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.Source <= @Source";
                parameters.Add(new SqlParameter("@Source", query.Source));
            }
            sql += "  ORDER BY DD.Date ASC";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Distribute.MaterielChannelDetailed>(data.Tables[0]);
        }
    }
}