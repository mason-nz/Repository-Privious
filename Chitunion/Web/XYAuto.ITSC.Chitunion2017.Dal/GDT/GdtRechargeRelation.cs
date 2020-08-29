using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    //接收智慧云回传的充值单，及时去拉取账户信息，然后传给智慧云接口
    public partial class GdtRechargeRelation : DataBase
    {
        public static readonly GdtRechargeRelation Instance = new GdtRechargeRelation();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(XYAuto.ITSC.Chitunion2017.Entities.GDT.GdtRechargeRelation entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_RechargeRelation(");
            strSql.Append("DemandBillNo,RechargeNumber,Amount,CreateUserId,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@DemandBillNo,@RechargeNumber,@Amount,@CreateUserId,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@RechargeNumber",entity.RechargeNumber),
                        new SqlParameter("@Amount",entity.Amount),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public Entities.GDT.GdtRechargeRelation VerifyOfRechargeNumber(int demandBillNo, string rechargeNumber)
        {
            var sql = @"SELECT RR.* FROM DBO.GDT_RechargeRelation AS RR WITH(NOLOCK)
                                    WHERE RR.DemandBillNo = @DemandBillNo AND RR.RechargeNumber = @RechargeNumber";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@DemandBillNo",demandBillNo),
                        new SqlParameter("@RechargeNumber",rechargeNumber)
                        };
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Entities.GDT.GdtRechargeRelation>(obj.Tables[0]);
        }
    }
}