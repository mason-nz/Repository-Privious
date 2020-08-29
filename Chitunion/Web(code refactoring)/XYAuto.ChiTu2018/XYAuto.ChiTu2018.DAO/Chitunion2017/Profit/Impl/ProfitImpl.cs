/********************************
* 项目名称 ：XYAuto.ChiTu2018.DAO.Chitunion2017.Profit.impl
* 类 名 称 ：ProfitImpl
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 10:24:06
********************************/

using System;
using System.Data.Entity;
using System.Transactions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Profit.Impl
{
    public class ProfitImpl : RepositoryImpl<LE_IncomeDetail>, IProfit
    {
        public int InsertProfit1(int userId, int profitType, string detailDescription, decimal incomPrice,
            DateTime? dtDate, int insertCount)
        {
            var result = false;
            var retMsg = "执行失败";
            var strDate = "";




            return 0;
        }

        public int InsertProfit(int userId, int profitType, string detailDescription, decimal incomPrice, DateTime? dtDate, int insertCount)
        {
            var result = false;
            var retMsg = "执行失败";
            var strDate = "";
            if (dtDate != null)
            {
                strDate = $"  AND CONVERT(VARCHAR(10),IncomeTime,23)='{((DateTime)dtDate).ToString("yyyy-MM-dd")}' ";
            }

            var sql = $@"DECLARE @IncomeCount INT;
                                SELECT  @IncomeCount = COUNT(1)
                                FROM    LE_IncomeDetail
                                WHERE   UserID = {userId}
                                        AND CategoryID = {profitType} {strDate};

                                IF ( @IncomeCount <= {insertCount} )
                                    BEGIN
                                        INSERT  INTO dbo.LE_IncomeDetail
                                                ( IncomeTime ,
                                                  UserID ,
                                                  CategoryID ,
                                                  DetailDescription ,
                                                  IncomePrice ,
                                                  ClickCount
                                                )
                                        VALUES  ( GETDATE() ,
                                                  {userId} ,
                                                  {profitType} ,
                                                  '{detailDescription}' ,
                                                  {incomPrice} , -- IncomePrice - decimal
                                                  0  -- ClickCount - int
                                                );
                                        DECLARE @DetailCount INT;
                                        SELECT  @IncomeCount = COUNT(1)
                                        FROM    LE_IncomeStatisticsCategory
                                        WHERE   UserID = {userId}
                                                AND IncomeCategoryID = {profitType};

                                        IF ( @DetailCount > 0 )
                                            BEGIN
                                                UPDATE  LE_IncomeStatisticsCategory
                                                SET     IncomePrice+= {incomPrice}
                                                WHERE   UserID = {userId}
                                                        AND IncomeCategoryID = {profitType};
                                            END;
                                        ELSE
                                            BEGIN
                                                INSERT  INTO dbo.LE_IncomeStatisticsCategory
                                                        ( IncomeCategoryID ,
                                                          IncomePrice ,
                                                          UserID ,
                                                          CreateTime
                                                        )
                                                VALUES  ( {profitType} , -- IncomeCategoryID - int
                                                          {incomPrice} , -- IncomePrice - decimal
                                                          {userId} , -- UserID - int
                                                          GETDATE()  -- CreateTime - datetime
                                                        );
                                            END;
                                    END ";


            var executeCount1 = context.Database.ExecuteSqlCommand(sql);
            //add LE_WithdrawalsStatistics
            //sql = AddWithdrawalsSql(userId, incomPrice);
            //var executeCount2 = db.Database.ExecuteSqlCommand(sql);

            if (executeCount1 > 0)
            {
                result = true;
            }
            return executeCount1;
        }

    }
}
