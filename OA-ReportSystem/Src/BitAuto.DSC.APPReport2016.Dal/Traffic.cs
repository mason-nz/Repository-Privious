using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils.Data;
using System.Data;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.Dal
{
  public  class Traffic
    {
      public static Traffic Instance = new Traffic();
     
      /// <summary>
      /// 获取最新日期
      /// </summary>
      /// <returns></returns>
      public DateTime GetLatestDate()
        {
            string sql = @"SELECT TOP 1 [Date]  FROM Traffic WHERE SiteId=0 And [Date]<CONVERT(varchar(100), GETDATE(), 23) ORDER BY [Date] DESC";

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

            return CommonFunction.ObjectToDateTime(obj);        
    
        }


      /// <summary>
      /// 获取平台覆盖数据饼图
      /// </summary>
      /// <param name="date"></param>
      /// <returns></returns>
      public DataTable GetDataByDate(DateTime date)
      {
          string sql = @"SELECT t1.UV,t2.DictName,
                        t1.WeekBasis,t1.DayBasis,t1.SiteId 
                        FROM Traffic t1 
                        INNER JOIN DictInfo t2 
                        ON t1.SiteId=t2.DictId AND t2.Status=0
                        WHERE  t1.SiteId!=0 AND [Date]>='" + date.Date.ToString() + "' And  [Date]<'" + date.AddDays(1).Date.ToString() + @"'
                        ORDER BY t2.OrderNum ";

          DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql);         
          if (ds != null && ds.Tables.Count > 0)
          {
              return ds.Tables[0];
          }
          else
          {
              return null;
          }

      }

      /// <summary>
      /// 获取平台覆盖数据片图前的总计数据
      /// </summary>
      /// <param name="date">日期</param>
      /// <returns></returns>
      public DataTable GetWholeSiteByDate(DateTime date)
      {
          string sql = @"SELECT  UV, WeekBasis, DayBasis 
                        FROM Traffic
                        WHERE  SiteId=0 AND [Date]>='" + date.Date.ToString() + "' And  [Date]<'" + date.AddDays(1).Date.ToString() + "'";

          DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql);
          if (ds != null && ds.Tables.Count > 0)
          {
              return ds.Tables[0];
          }
          else
          {
              return null;
          }

      }



      /// <summary>
      /// 获取单个平台线图
      /// </summary>
      /// <param name="siteId"></param>
      /// <param name="date"></param>
      /// <returns></returns>
      public DataTable GetDataTrendBySiteIdAndDate(int siteId, DateTime date)
      {
          string sql = @"SELECT [Date], UV, PV FROM Traffic WHERE SiteId=" + siteId + " AND  [Date]<'" + date.AddDays(1).Date.ToString()+"'";

          DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql);
          if (ds != null && ds.Tables.Count > 0)
          {
              return ds.Tables[0];
          }
          else
          {
              return null;
          }

      }
    }
}
