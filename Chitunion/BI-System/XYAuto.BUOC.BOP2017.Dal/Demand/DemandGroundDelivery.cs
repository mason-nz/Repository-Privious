using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Query.Demand;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.Demand
{
    //需求落地页加参管理（关联）
    public partial class DemandGroundDelivery : DataBase
    {
        public static readonly DemandGroundDelivery Instance = new DemandGroundDelivery();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Demand.DemandGroundDelivery entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Demand_GroundDelivery(");
            strSql.Append("DemandBillNo,GroundId,DeliveryType,AdSiteSet,AdCreative,AdName,PromotionUrl,PromotionUrlCode,Status,CreateTime,CreateUserId");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@GroundId,@DeliveryType,@AdSiteSet,@AdCreative,@AdName,@PromotionUrl,@PromotionUrlCode,@Status,@CreateTime,@CreateUserId");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@GroundId",entity.GroundId),
                        new SqlParameter("@DeliveryType",entity.DeliveryType),
                        new SqlParameter("@AdSiteSet",entity.AdSiteSet),
                        new SqlParameter("@AdCreative",entity.AdCreative),
                        new SqlParameter("@AdName",entity.AdName),
                        new SqlParameter("@PromotionUrl",entity.PromotionUrl),
                        new SqlParameter("@PromotionUrlCode",entity.PromotionUrlCode),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public List<Entities.Demand.DemandGroundDeliveryExt> GetGroundDeliveries(int demandBillNo)
        {
            var sql = @"

                        CREATE TABLE #Temp_GroundDicTab(
                        DictId INT,
                        DictName VARCHAR(100)
                        )

                        INSERT INTO #Temp_GroundDicTab
                                ( DictId, DictName )
                        SELECT DictId, DictName FROM dbo.DictInfo WITH(NOLOCK)

                        SELECT  GP.GroundId ,
                                GP.DemandBillNo ,
                                GP.BrandId ,
                                GP.SerielId ,
                                GP.ProvinceId ,
                                GP.CityId ,
                                GP.PromotionUrl ,
                                GD.DeliveryId ,
                                GD.DeliveryType ,
                                D.Name AS DemandName ,
                                D.AuditStatus ,
                                DC4.DictName AS AuditStatusName ,
                                DC1.DictName AS DeliveryTypeName ,
                                GD.AdSiteSet ,
                                DC2.DictName AS AdSiteSetName ,
                                GD.AdCreative ,
                                DC3.DictName AS AdCreativeName ,
                                GD.AdName ,
                                GD.PromotionUrlCode ,
                                AdGroup.AdgroupName ,
                                AdGroup.CampaignName,
		                        AdGroup.AdgroupId ,
                                CarInfo = ( SELECT  STUFF(( SELECT  '|'
                                                                    + CAST(CS.BrandId AS VARCHAR(15))
                                                                    + ',' + CS.BrandName + '@='
                                                                    + CAST(CS.SerielId AS VARCHAR(15))
                                                                    + ',' + CS.SerielName
                                                            FROM    dbo.Demand_CarSeriel AS CS WITH ( NOLOCK )
                                                            WHERE   CS.DemandBillNo = GP.DemandBillNo
                                                          FOR
                                                            XML PATH('')
                                                          ), 1, 1, '')
                                          ) ,
                                CityInfo = ( SELECT STUFF(( SELECT  '|'
                                                                    + CAST(CY.ProvinceId AS VARCHAR(15))
                                                                    + ',' + CY.ProvinceName + '@='
                                                                    + CAST(CY.CityId AS VARCHAR(15))
                                                                    + ',' + CY.CityName
                                                            FROM    dbo.Demand_Citys AS CY WITH ( NOLOCK )
                                                            WHERE   CY.DemandBillNo = GP.DemandBillNo
						                        --AND CY.CityId = GP.CityId
						                        --AND CY.ProvinceId= GP.ProvinceId
                                                          FOR
                                                            XML PATH('')
                                                          ), 1, 1, '')
                                           )
                        FROM    dbo.Demand_GroundPage AS GP WITH ( NOLOCK )
                                INNER JOIN dbo.GDT_Demand AS D WITH ( NOLOCK ) ON D.DemandBillNo = GP.DemandBillNo
                                LEFT JOIN dbo.Demand_GroundDelivery AS GD WITH ( NOLOCK ) ON GD.GroundId = GP.GroundId
                                                                                      AND GP.DemandBillNo = D.DemandBillNo
                                                                                      AND GD.Status = 0
                                LEFT JOIN ( SELECT --TOP 1
                                                    ADGP.AdgroupName ,
                                                    DR.AdgroupId,
                                                    DR.DemandBillNo ,
                                                    DR.DeliveryId ,
                                                    CP.CampaignName
                                            FROM    dbo.GDT_DemandRelation AS DR WITH ( NOLOCK )
                                                    INNER JOIN dbo.GDT_AdGroup AS ADGP WITH ( NOLOCK ) ON ADGP.AdgroupId = DR.AdgroupId
                                                    LEFT JOIN DBO.GDT_Campaign AS CP WITH(NOLOCK) ON CP.CampaignId = ADGP.CampaignId
                                          ) AS AdGroup ON AdGroup.DemandBillNo = GD.DemandBillNo
                                                          AND AdGroup.DeliveryId = GD.DeliveryId
                                LEFT JOIN #Temp_GroundDicTab AS DC1 WITH ( NOLOCK ) ON DC1.DictId = GD.DeliveryType
                                LEFT JOIN #Temp_GroundDicTab AS DC2 WITH ( NOLOCK ) ON DC2.DictId = GD.AdSiteSet
                                LEFT JOIN #Temp_GroundDicTab AS DC3 WITH ( NOLOCK ) ON DC3.DictId = GD.AdCreative
                                LEFT JOIN #Temp_GroundDicTab AS DC4 WITH ( NOLOCK ) ON DC4.DictId = D.AuditStatus
                        WHERE   GP.Status = 0
                        AND GP.DemandBillNo = @DemandBillNo
                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"DemandBillNo",demandBillNo)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Demand.DemandGroundDeliveryExt>(data.Tables[0]);
        }

        public List<Entities.Demand.DemandGroundDelivery> GetList(DemandGroundQuery<Entities.Demand.DemandGroundDelivery> query)
        {
            var sql = @"
                        SELECT  GD.*,D.AuditStatus,D.Name,
		                        D.BeginDate,
		                        D.EndDate
                        FROM    dbo.Demand_GroundDelivery AS GD WITH ( NOLOCK )
                                LEFT JOIN dbo.GDT_Demand AS D WITH ( NOLOCK ) ON D.DemandBillNo = GD.DemandBillNo
                        WHERE   GD.Status = 0
                        ";

            var parameters = new List<SqlParameter>();

            if (query.DemandBillNo != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GD.DemandBillNo = @DemandBillNo";
                parameters.Add(new SqlParameter(@"DemandBillNo", query.DemandBillNo));
            }
            if (query.GroundId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GD.GroundId = @GroundId";
                parameters.Add(new SqlParameter(@"GroundId", query.GroundId));
            }
            if (query.DeliveryId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GD.DeliveryId = @DeliveryId";
                parameters.Add(new SqlParameter(@"DeliveryId", query.DeliveryId));
            }
            if (!string.IsNullOrWhiteSpace(query.AdName))
            {
                sql += " AND GD.AdName = @AdName";
                parameters.Add(new SqlParameter(@"AdName", query.AdName));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Demand.DemandGroundDelivery>(data.Tables[0]);
        }

        public int Delete(int demandBillNo, int deliveryId)
        {
            var sql = @"
                        UPDATE dbo.Demand_GroundDelivery SET Status  = -1
                        WHERE DeliveryId = @DeliveryId
                        --删除加参关联的广告
                        DELETE FROM DBO.GDT_DemandRelation
                        WHERE DemandBillNo = @DemandBillNo AND DeliveryId =@DeliveryId1
                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"DeliveryId",deliveryId),
                new SqlParameter(@"DeliveryId1",deliveryId),
                new SqlParameter(@"DemandBillNo",demandBillNo)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
        }

        public int GetDemandBillNoByPromotionUrlCode(string code)
        {
            var sql = @"
                        SELECT DemandBillNo FROM dbo.Demand_GroundDelivery
                        WHERE PromotionUrlCode = @PromotionUrlCode
                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"PromotionUrlCode",code)
            };
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return obj == null ? 0 : (int)obj;
        }
    }
}