using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //收入明细
    public partial class LeAccountBalance : DataBase
    {


        public static readonly LeAccountBalance Instance = new LeAccountBalance();


        public Tuple<List<Entities.LETask.LeAccountBalance>, Entities.LETask.TotalAccountBalance>
            GetAccountBalances(int orderId, int userId)
        {

            var sql = $@"
                     
                    SELECT  AB.RecID ,
                            AB.CPCCount ,
                            AB.CPLCount ,
                            AB.CPCShowCount ,
                            AB.CPLShowCount ,
                            AB.PVCount ,
                            AB.OrderID ,
                            AB.Status ,
                            AB.CreateTime ,
                            AB.StatisticsTime ,
                            AB.CreateUserID ,
                            AB.CPCTotalPrice ,
                            AB.CPLTotalPrice ,
                            AB.TotalMoney
                    FROM    dbo.LE_AccountBalance AS AB WITH ( NOLOCK )
                    WHERE   AB.OrderID = {orderId} AND AB.CreateUserID = {userId}
             
                    SELECT  SUM(AB.CPCTotalPrice) AS TotalCPCTotalPrice ,
                            SUM(AB.CPLTotalPrice) AS TotalCPLTotalPrice ,
                            SUM(AB.TotalMoney) AS TotalMoney,
                            SUM(AB.CPCShowCount) AS TotalCPCCount ,
							SUM(AB.CPLShowCount) AS TotalCPLCount
                    FROM    dbo.LE_AccountBalance AS AB WITH ( NOLOCK )
                    WHERE   AB.OrderID = {orderId} AND AB.CreateUserID = {userId}
                ";

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<List<Entities.LETask.LeAccountBalance>, Entities.LETask.TotalAccountBalance>(
                DataTableToList<Entities.LETask.LeAccountBalance>(obj.Tables[0]),
                DataTableToEntity<Entities.LETask.TotalAccountBalance>(obj.Tables[1]));
        }


        public Entities.DTO.LeTask.RespUserStatDto GetUserStatInfo(int userId)
        {
            var sql = string.Format(@"

                    CREATE TABLE #Temp_Stat
                    (
                      OrderCount INT ,
                      WeiXinCount INT ,
                      EarningsPrice DECIMAL(18, 2)
                    );
				
                DECLARE @OrderCount INT ,
                        @WeiXinCount INT ,
                        @EarningsPrice DECIMAL(18, 2);

                SELECT  @OrderCount = COUNT(*)
                FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )
                WHERE   AO.UserID = {0};

                SELECT  @WeiXinCount = COUNT(*)
                FROM    dbo.LE_Weixin AS WX WITH ( NOLOCK )
                WHERE   WX.CreateUserID = {0}
                        AND WX.Status = 0;
                SELECT  @EarningsPrice = WS.AccumulatedIncome
                FROM    dbo.LE_WithdrawalsStatistics AS WS WITH ( NOLOCK )
                WHERE   WS.UserID = {0};

                INSERT  #Temp_Stat
                        ( OrderCount ,
                          WeiXinCount ,
                          EarningsPrice
				        )
                VALUES  ( @OrderCount , -- OrderCount - int
                          @WeiXinCount , -- WeiXinCount - int
                          @EarningsPrice  -- EarningsPrice - decimal
				        );
                SELECT  * FROM    #Temp_Stat;
                    ", userId);

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.DTO.LeTask.RespUserStatDto>(obj.Tables[0]);
        }
    }
}

