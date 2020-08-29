using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.StockBroker
{
    public class StockBroker:DataBase
    {
        public readonly static StockBroker Instance = new StockBroker();
        public int isStockBrokerUser(string username)
        {
            string sqlstr = @"SELECT TOP 1 UserBroker.UserID FROM dbo.UserBroker 
                                INNER JOIN	dbo.UserInfo ON UserInfo.UserID = UserBroker.UserID
                                WHERE dbo.UserInfo.UserName=@UserName";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserName",username)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);

            int ret = -2;
            if (ds.Tables[0].Rows.Count>0 && int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret))
                return ret;

            return -2;
        }

        public int p_UserBrokerLogin_Update(Entities.StockBroker.LoginDto dto, out string msg)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Msg",SqlDbType.VarChar,200),
                new SqlParameter("@BrokerUserID",dto.dealerID),
                new SqlParameter("@EnterpriseName",dto.dealerName),
                new SqlParameter("@Contact",dto.contacts),
                new SqlParameter("@Mobile",dto.contactNumber),
                new SqlParameter("@RETURN_VALUE",SqlDbType.Int),
                new SqlParameter("@businessLicence",dto.businessLicence)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[5].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserBrokerLogin_Update", parameters);
            msg = (string)parameters[0].Value;
            return (int)parameters[5].Value;
        }

        public int DeleteByUserID(int userid)
        {
            string sqlstr = "DELETE FROM UserBroker WHERE UserID=@UserID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@UserID",userid)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }
    }
}
