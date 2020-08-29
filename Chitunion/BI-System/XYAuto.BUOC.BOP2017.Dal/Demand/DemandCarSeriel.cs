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
    //需求品牌车型关联表
    public partial class DemandCarSeriel : DataBase
    {
        public static readonly DemandCarSeriel Instance = new DemandCarSeriel();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Demand.DemandCarSeriel entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Demand_CarSeriel(");
            strSql.Append("DemandBillNo,SerielId,SerielName,BrandId,BrandName,Status,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@SerielId,@SerielName,@BrandId,@BrandName,@Status,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@SerielId",entity.SerielId),
                        new SqlParameter("@SerielName",entity.SerielName),
                        new SqlParameter("@BrandId",entity.BrandId),
                        new SqlParameter("@BrandName",entity.BrandName),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int Insert(List<DemandCarSerielDto> list, int demandBillNo)
        {
            var sbSql = new StringBuilder();
            if (list == null || !list.Any())
                return 0;

            sbSql.Append($@"delete from dbo.Demand_CarSeriel where DemandBillNo={demandBillNo}");
            sbSql.Append("insert into dbo.Demand_CarSeriel(");
            sbSql.Append("DemandBillNo,SerielId,SerielName,BrandId,BrandName,Status,CreateTime");
            sbSql.Append(") values ");
            list.ForEach(entity =>
            {
                if (entity.CarSerialInfo == null || !entity.CarSerialInfo.Any())
                {
                    sbSql.Append($"({demandBillNo},-2,'',{entity.BrandId},'{entity.BrandName}',0,getdate()),");
                }
                else
                {
                    entity.CarSerialInfo.ForEach(s =>
                    {
                        sbSql.Append($"({demandBillNo},{s.CarSerialId},'{s.CarSerialName}',{entity.BrandId},'{entity.BrandName}',0,getdate()),");
                    });
                }
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString().Trim(','));
        }
    }
}