using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.UserManage
{
    public class LE_UserBankAccount : DataBase
    {
        public readonly static LE_UserBankAccount Instance = new LE_UserBankAccount();
        public bool Update(Entities.UserManage.LE_UserBankAccount model, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                UPDATE  dbo.LE_UserBankAccount
                                SET     AccountName = '{model.AccountName}' ,
                                        AccountType = {model.AccountType}
                                WHERE   UserID = {model.UserID};");

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount > 0;
        }
    }
}
