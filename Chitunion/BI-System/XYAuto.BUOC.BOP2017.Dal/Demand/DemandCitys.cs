using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Dto.Demand;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.Demand
{
    //需求省份城市关联表
    public partial class DemandCitys : DataBase
    {
        public static readonly DemandCitys Instance = new DemandCitys();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Demand.DemandCitys entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Demand_Citys(");
            strSql.Append("DemandBillNo,ProvinceId,ProvinceName,CityId,CityName,Status,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@ProvinceId,@ProvinceName,@CityId,@CityName,@Status,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@ProvinceId",entity.ProvinceId),
                        new SqlParameter("@ProvinceName",entity.ProvinceName),
                        new SqlParameter("@CityId",entity.CityId),
                        new SqlParameter("@CityName",entity.CityName),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int Insert(List<DemandCitysDto> list, int demandBillNo)
        {
            var sbSql = new StringBuilder();
            if (list == null || !list.Any())
                return 0;

            sbSql.Append($@"delete from dbo.Demand_Citys where DemandBillNo={demandBillNo}");

            sbSql.Append("insert into Demand_Citys(");
            sbSql.Append("DemandBillNo,ProvinceId,ProvinceName,CityId,CityName,Status,CreateTime");
            sbSql.Append(") values ");
            list.ForEach(entity =>
            {
                if (entity.City == null || !entity.City.Any())
                {
                    sbSql.Append($"({demandBillNo},{entity.ProvinceId},'{entity.ProvinceName}',-2,'',0,getdate()),");
                }
                else
                {
                    entity.City.ForEach(s =>
                    {
                        sbSql.Append($"({demandBillNo},{entity.ProvinceId},'{entity.ProvinceName}',{s.CityId},'{s.CityName}',0,getdate()),");
                    });
                }
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString().Trim(','));
        }

        public Tuple<List<Entities.Demand.DemandCitys>, List<Entities.Demand.DemandCarSeriel>> GetCarAndCityInfo(int demandBillNo)
        {
            var sql = @"
                        SELECT DC.* FROM dbo.Demand_Citys AS DC WITH(NOLOCK)
                        WHERE DC.DemandBillNo = @DemandBillNo1

                        SELECT CS.* FROM dbo.Demand_CarSeriel AS CS WITH(NOLOCK)
                        WHERE CS.DemandBillNo = @DemandBillNo2
                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"DemandBillNo1",demandBillNo),
                new SqlParameter(@"DemandBillNo2",demandBillNo)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());

            return new Tuple<List<Entities.Demand.DemandCitys>, List<Entities.Demand.DemandCarSeriel>>(
                DataTableToList<Entities.Demand.DemandCitys>(data.Tables[0]),
                DataTableToList<Entities.Demand.DemandCarSeriel>(data.Tables[1])
                );
        }
    }
}