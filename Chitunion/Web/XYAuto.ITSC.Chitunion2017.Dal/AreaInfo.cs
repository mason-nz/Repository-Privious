using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class AreaInfo : DataBase
    {
        public static readonly AreaInfo Instance = new AreaInfo();

        public DataTable GetAreaByPid(int pid)
        {
            string sql = string.Format(@"
SELECT [AreaID]
      ,[PID]
      ,[AreaName]
      ,[AbbrName]
      ,[CreateTime]
      ,[Level]
  FROM  [AreaInfo] Where PID={0} And Status=0 ", pid);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public DataTable GetAreaByID(int id)
        {
            string sql = string.Format(@"
SELECT [AreaID]
      ,[PID]
      ,[AreaName]
      ,[AbbrName]
      ,[CreateTime]
      ,[Level]
  FROM  [AreaInfo] Where AreaID={0} And Status=0 ", id);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}
