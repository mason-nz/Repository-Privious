using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class HolidaysInfo:DataBase
    {
        public static readonly HolidaysInfo Instance = new HolidaysInfo();

        public DataTable GetHolidaysInfo()
        {
            string sql = string.Format(@"SELECT  CurYear ,
                CONVERT(VARCHAR(10), StartDate,120 ) AS StartDate,
                CONVERT(VARCHAR(10), EndDate,120 ) AS EndDate,
                Name
        FROM    dbo.HolidaysInfo
        WHERE   Status = 0
                AND CurYear >= CONVERT(INT, YEAR(GETDATE()))
        ORDER BY CurYear ,
                StartDate;");

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
    }
}
