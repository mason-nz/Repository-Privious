using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Query.Demand;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.Demand
{
    //需求落地页管理
    public partial class DemandGroundPage : DataBase
    {
        public static readonly DemandGroundPage Instance = new DemandGroundPage();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Demand.DemandGroundPage entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Demand_GroundPage(");
            strSql.Append("DemandBillNo,BrandId,SerielId,ProvinceId,CityId,PromotionUrl,Status,CreateTime,CreateUserId");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@BrandId,@SerielId,@ProvinceId,@CityId,@PromotionUrl,@Status,@CreateTime,@CreateUserId");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@BrandId",entity.BrandId),
                        new SqlParameter("@SerielId",entity.SerielId),
                        new SqlParameter("@ProvinceId",entity.ProvinceId),
                        new SqlParameter("@CityId",entity.CityId),
                        new SqlParameter("@PromotionUrl",entity.PromotionUrl),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public List<Entities.Demand.DemandGroundPage> GetList(DemandGroundQuery<Entities.Demand.DemandGroundPage> query)
        {
            var sql = @"

                        SELECT  GP.GroundId ,
                                GP.DemandBillNo ,
                                GP.PromotionUrl ,
                                GP.SerielId ,
                                GP.BrandId ,
                                GP.ProvinceId ,
                                GP.CityId ,
                                D.DeliveryCount ,
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
                                                          FOR
                                                            XML PATH('')
                                                          ), 1, 1, '')
                                           )
                        FROM    dbo.Demand_GroundPage AS GP WITH ( NOLOCK )
                                LEFT JOIN ( SELECT  COUNT(1) AS DeliveryCount ,
                                                    GD.GroundId
                                            FROM    dbo.Demand_GroundDelivery AS GD WITH ( NOLOCK )
                                            WHERE   GD.Status = 0
                                            GROUP BY GD.GroundId
                                          ) AS D ON D.GroundId = GP.GroundId
                        WHERE   GP.Status = 0 AND GP.DemandBillNo = @DemandBillNo";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"DemandBillNo",query.DemandBillNo)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Demand.DemandGroundPage>(data.Tables[0]);
        }

        public List<Entities.Demand.DemandGroundPage> GetGroundPages(DemandGroundQuery<Entities.Demand.DemandGroundPage> query)
        {
            var sql = @"SELECT GP.*,D.AuditStatus,D.Name
                        FROM  dbo.Demand_GroundPage AS GP WITH ( NOLOCK )
                              INNER JOIN dbo.GDT_Demand AS D WITH ( NOLOCK ) ON D.DemandBillNo = GP.DemandBillNo
                        WHERE GP.Status = 0 ";
            var parameters = new List<SqlParameter>();

            if (query.DemandBillNo != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GP.DemandBillNo = @DemandBillNo";
                parameters.Add(new SqlParameter(@"DemandBillNo", query.DemandBillNo));
            }
            if (query.GroundId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GP.GroundId = @GroundId";
                parameters.Add(new SqlParameter(@"GroundId", query.GroundId));
            }
            if (query.BrandId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GP.BrandId = @BrandId";
                parameters.Add(new SqlParameter(@"BrandId", query.BrandId));
            }
            if (query.SerielId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GP.SerielId = @SerielId";
                parameters.Add(new SqlParameter(@"SerielId", query.SerielId));
            }
            if (query.CityId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GP.CityId = @CityId";
                parameters.Add(new SqlParameter(@"CityId", query.CityId));
            }
            if (query.ProvinceId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND GP.ProvinceId = @ProvinceId";
                parameters.Add(new SqlParameter(@"ProvinceId", query.ProvinceId));
            }
            if (!string.IsNullOrWhiteSpace(query.PromotionUrl))
            {
                sql += " AND GP.PromotionUrl = @PromotionUrl";
                parameters.Add(new SqlParameter(@"PromotionUrl", query.PromotionUrl));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Demand.DemandGroundPage>(data.Tables[0]);
        }

        public int Delete(int groundId, int demandBillNo)
        {
            var sql = @"
                        UPDATE  dbo.Demand_GroundPage
                        SET     Status = -1
                        WHERE   GroundId = @GroundId1
                                AND DemandBillNo = @DemandBillNo1

                        --删除加参关联的广告
                        DELETE FROM DBO.GDT_DemandRelation
                        WHERE DemandBillNo = @DemandBillNo2 AND DeliveryId IN (
	                        SELECT DD.DeliveryId FROM DBO.Demand_GroundDelivery AS DD WITH(NOLOCK)
	                        WHERE DD.GroundId = @GroundId2 AND DD.Status = 0
                        )
                        UPDATE  dbo.Demand_GroundDelivery
                        SET     Status = -1
                        WHERE   GroundId = @GroundId3 AND Status = 0
                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"GroundId1",groundId),
                new SqlParameter(@"GroundId2",groundId),
                new SqlParameter(@"GroundId3",groundId),
                new SqlParameter(@"DemandBillNo1",demandBillNo),
                new SqlParameter(@"DemandBillNo2",demandBillNo)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
        }
    }
}