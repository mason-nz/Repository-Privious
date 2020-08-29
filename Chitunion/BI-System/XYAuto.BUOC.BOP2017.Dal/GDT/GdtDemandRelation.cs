using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Entities.Query.Demand;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //智慧云需求关联广告表，我们把维度设置到最细
    public partial class GdtDemandRelation : DataBase
    {
        public static readonly GdtDemandRelation Instance = new GdtDemandRelation();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtDemandRelation entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_DemandRelation(");
            strSql.Append("DemandBillNo,AdgroupId,CreateUserId,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@AdgroupId,@CreateUserId,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@AdgroupId",entity.AdgroupId),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public List<Entities.GDT.GdtDemandRelation> GetAccountId(int demandBillNo)
        {
            var sql = string.Format(@"
                        SELECT DR.*,AR.AccountId
                        FROM dbo.GDT_DemandRelation AS DR WITH(NOLOCK)
                        INNER JOIN DBO.GDT_Demand AS DD WITH(NOLOCK) ON DD.DemandBillNo = DR.DemandBillNo
                        INNER JOIN DBO.GDT_AccountRelation AR WITH(NOLOCK) ON AR.UserId = DD.CreateUserId
                        WHERE DD.Status = 0
                        AND DD.AuditStatus IN ({0})
                    ", $"{(int)DemandAuditStatusEnum.Puting},{(int)DemandAuditStatusEnum.Terminated}");

            if (demandBillNo > 0)
            {
                sql += $" AND DD.DemandBillNo = {demandBillNo}";
            }
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.GDT.GdtDemandRelation>(ds.Tables[0]);
        }

        public List<Entities.GDT.GdtDemandRelation> GetList(DemandGroundQuery<Entities.GDT.GdtDemandRelation> query)
        {
            var sql = @"
                        SELECT  DR.*
                        FROM    dbo.GDT_DemandRelation AS DR WITH ( NOLOCK )
                        WHERE   1 = 1
                        ";
            var parameters = new List<SqlParameter>();
            if (query.DemandBillNo != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND DR.DemandBillNo = @DemandBillNo";
                parameters.Add(new SqlParameter(@"DemandBillNo", query.DemandBillNo));
            }
            if (query.DeliveryId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND DR.DeliveryId = @DeliveryId";
                parameters.Add(new SqlParameter(@"DeliveryId", query.DeliveryId));
            }
            if (query.AdgroupId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND DR.AdgroupId = @AdgroupId";
                parameters.Add(new SqlParameter(@"AdgroupId", query.AdgroupId));
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.GDT.GdtDemandRelation>(data.Tables[0]);
        }

        public int UpdateRelateToAdGroup(Entities.GDT.GdtDemandRelation entity)
        {
            var sql = $@"
                    IF EXISTS(SELECT 1 FROM dbo.GDT_DemandRelation WITH(NOLOCK) WHERE DemandBillNo = {entity.DemandBillNo} AND DeliveryId = {entity.DeliveryId})
                    BEGIN
	                    UPDATE dbo.GDT_DemandRelation SET AdgroupId = {entity.AdgroupId} ,CreateTime = GETDATE() WHERE DemandBillNo = {entity.DemandBillNo} AND DeliveryId = {entity.DeliveryId}
                    END
                    ELSE
                    BEGIN
	                    INSERT INTO dbo.GDT_DemandRelation
	                            ( DemandBillNo ,
	                              AdgroupId ,
	                              DeliveryId ,
	                              CreateTime ,
	                              CreateUserId
	                            )
	                    VALUES  ( {entity.DemandBillNo} , -- DemandBillNo - int
	                              {entity.AdgroupId} , -- AdgroupId - int
	                              {entity.DeliveryId} , -- DeliveryId - int
	                              GETDATE() , -- CreateTime - datetime
	                              {entity.CreateUserId}  -- CreateUserId - int
	                            )
                    END

                    ";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}