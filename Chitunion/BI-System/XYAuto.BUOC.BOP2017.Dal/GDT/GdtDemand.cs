﻿/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 10:50:34
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Entities.GDT;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    public class GdtDemand : DataBase
    {
        #region Instance

        public static readonly GdtDemand Instance = new GdtDemand();

        #endregion Instance

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.GDT.GdtDemand entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_Demand(");
            strSql.Append("DemandBillNo,Name,Status,BrandSerialJson,ProvinceCityJson,DistributorJson,PromotionPolicy,TotalBudget,ClueNumber,BeginDate,EndDate,CreateUserId,UpdateUserId,CreateTime,UpdateTime");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@Name,@Status,@BrandSerialJson,@ProvinceCityJson,@DistributorJson,@PromotionPolicy,@TotalBudget,@ClueNumber,@BeginDate,@EndDate,@CreateUserId,@UpdateUserId,getdate(),getdate()");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@BrandSerialJson",entity.BrandSerialJson),
                        new SqlParameter("@ProvinceCityJson",entity.ProvinceCityJson),
                        new SqlParameter("@DistributorJson",entity.DistributorJson),
                        new SqlParameter("@PromotionPolicy",entity.PromotionPolicy),
                        new SqlParameter("@TotalBudget",entity.TotalBudget),
                        new SqlParameter("@ClueNumber",entity.ClueNumber),
                        new SqlParameter("@BeginDate",entity.BeginDate),
                        new SqlParameter("@EndDate",entity.EndDate),
                        new SqlParameter("@AuditStatus",(int)entity.AuditStatus),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@UpdateUserId",entity.UpdateUserId),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@UpdateTime",entity.UpdateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int UpdateByDemandBillNo(Entities.GDT.GdtDemand entity)
        {
            var sql = @"
                    UPDATE [dbo].[GDT_Demand]
                       SET [Name] = @Name
                          ,[BrandSerialJson] = @BrandSerialJson
                          ,[ProvinceCityJson] = @ProvinceCityJson
                          ,[DistributorJson] = @DistributorJson
                          ,[PromotionPolicy] = @PromotionPolicy
                          ,[TotalBudget] =@TotalBudget
                          ,[ClueNumber] = @ClueNumber
                          ,[BeginDate] = @BeginDate
                          ,[EndDate] = @EndDate
                          ,[AuditStatus] = @AuditStatus
                          ,[UpdateUserId] = @UpdateUserId
                          ,[UpdateTime] = getdate()
                     WHERE DemandBillNo = @DemandBillNo";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@BrandSerialJson",entity.BrandSerialJson),
                        new SqlParameter("@ProvinceCityJson",entity.ProvinceCityJson),
                        new SqlParameter("@DistributorJson",entity.DistributorJson),
                        new SqlParameter("@PromotionPolicy",entity.PromotionPolicy),
                        new SqlParameter("@TotalBudget",entity.TotalBudget),
                        new SqlParameter("@ClueNumber",entity.ClueNumber),
                        new SqlParameter("@BeginDate",entity.BeginDate),
                        new SqlParameter("@EndDate",entity.EndDate),
                        new SqlParameter("@AuditStatus",(int)entity.AuditStatus),
                        new SqlParameter("@UpdateUserId",entity.UpdateUserId)
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int UpdateAuditStatus(int demandBillNo, DemandAuditStatusEnum auditStatus, int updateUserId, DateTime updateTime)
        {
            var sql = @"
                    UPDATE [dbo].[GDT_Demand]
                       SET [AuditStatus] = @AuditStatus
                           ,[UpdateUserId] = @UpdateUserId
                           ,[UpdateTime] = @UpdateTime
                     WHERE DemandBillNo = @DemandBillNo";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",demandBillNo),
                        new SqlParameter("@AuditStatus",(int)auditStatus),
                        new SqlParameter("@UpdateTime",updateTime),
                        new SqlParameter("@UpdateUserId",updateUserId)
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public List<Entities.GDT.GdtDemand> GetList()
        {
            var sql = @"
                        SELECT * FROM DBO.GDT_Demand
                        WHERE Status= 0";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.GDT.GdtDemand>(data.Tables[0]);
        }

        public Entities.GDT.GdtDemand GetInfoByDemandBillNo(int demandBillNo)
        {
            var sql = @"SELECT TOP 1 [DemandId]
                                      ,[DemandBillNo]
                                      ,[Name]
                                      ,[Status]
                                      ,[BrandSerialJson]
                                      ,[ProvinceCityJson]
                                      ,[DistributorJson]
                                      ,[PromotionPolicy]
                                      ,[TotalBudget]
                                      ,[ClueNumber]
                                      ,[BeginDate]
                                      ,[EndDate]
	                                  ,[AuditStatus]
                                      ,[CreateUserId]
                                      ,[UpdateUserId]
                                      ,[CreateTime]
                                      ,[UpdateTime]
                                  FROM [dbo].[GDT_Demand] WITH(NOLOCK)
                                  WHERE DemandBillNo = @DemandBillNo AND Status = 0";
            var paras = new List<SqlParameter>() { new SqlParameter("@DemandBillNo", demandBillNo) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.GDT.GdtDemand>(data.Tables[0]);
        }

        /// <summary>
        /// 找到广告组id 对应的需求，该需求所对应的广告组id，一对多
        /// </summary>
        /// <returns></returns>
        public List<Entities.GDT.GdtDemand> GetListByDemandRelation(int adgroupId)
        {
            var sql =
                @"

                SELECT  DR.DemandBillNo ,
                        DR.AdgroupId ,
                        DD.CreateUserId
                FROM    dbo.GDT_DemandRelation AS DR WITH ( NOLOCK )
                        INNER JOIN dbo.GDT_Demand AS DD WITH ( NOLOCK ) ON DD.DemandBillNo = DR.DemandBillNo
                WHERE   DD.Status = 0
                        AND DD.DemandBillNo = ( SELECT TOP 1
                                                        DemandBillNo
                                                FROM    dbo.GDT_DemandRelation WITH ( NOLOCK )
                                                WHERE   AdgroupId = 1
                                              )
                ";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.GDT.GdtDemand>(data.Tables[0]);
        }

        /// <summary>
        /// 需求达到开始时间，自动设置为【投放中】状态，并通知智慧云
        /// </summary>
        /// <returns></returns>
        public List<Entities.GDT.GdtDemandStatusNotes> GetDemandStatusNoteses()
        {
            string storedProcedure = "p_DemandStatusNotes";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure);

            return DataTableToList<GdtDemandStatusNotes>(data.Tables[0]);
        }
    }
}