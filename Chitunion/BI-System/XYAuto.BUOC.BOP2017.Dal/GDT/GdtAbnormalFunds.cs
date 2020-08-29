using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //资金异常表
    public partial class GdtAbnormalFunds : DataBase
    {
        public static readonly GdtAbnormalFunds Instance = new GdtAbnormalFunds();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtAbnormalFunds entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_AbnormalFunds(");
            strSql.Append("RechargeNumber,DemandBillNo,AccountId,TradeType,RechargeAmount,SpendAmount,TransferBackAmount,CreateTime,CreateUserId");
            strSql.Append(") values (");
            strSql.Append("@RechargeNumber,@DemandBillNo,@AccountId,@TradeType,@RechargeAmount,@SpendAmount,@TransferBackAmount,@CreateTime,@CreateUserId");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@RechargeNumber",entity.RechargeNumber),
                        new SqlParameter("@DemandBillNo",entity.DemandBillNo),
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@TradeType",entity.TradeType),
                        new SqlParameter("@RechargeAmount",entity.RechargeAmount),
                        new SqlParameter("@SpendAmount",entity.SpendAmount),
                        new SqlParameter("@TransferBackAmount",entity.TransferBackAmount),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }
    }
}