using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    //智慧云需求关联广告表，我们把维度设置到最细
    public partial class GdtDemandRelation : DataBase
    {
        public static readonly GdtDemandRelation Instance = new GdtDemandRelation();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(XYAuto.ITSC.Chitunion2017.Entities.GDT.GdtDemandRelation entity)
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
    }
}