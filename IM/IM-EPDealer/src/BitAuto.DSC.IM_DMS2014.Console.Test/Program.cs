using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.IM_DMS2014.Console.Test
{
    class Program
    {
        static string conn = System.Configuration.ConfigurationManager.AppSettings["ConnectionStrings_IMDMS2014"];
        static void Main(string[] args)
        {
            while (true) {
                TestDB();
                System.Threading.Thread.Sleep(10000);
            }
        }

        private static void TestDB(){
            try
            {
                Loger.Log4Net.Info("IM-经销商【DB-测试】：开始");
                System.Console.WriteLine("IM-经销商【DB-测试】：开始" + DateTime.Now);
                string sql = string.Format("SELECT top 1 recid FROM CityGroupAgent");
                DataSet ds = new DataSet();
                ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(conn, System.Data.CommandType.Text, sql);
                string s = string.Empty;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    s = string.Format("Count:{0},RecID:{1}", ds.Tables[0].Rows.Count, ds.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    s = "Not Data";
                }
                System.Console.WriteLine(s);
                Loger.Log4Net.Info(s);
                Loger.Log4Net.Info("IM-经销商【DB-测试】：结束");
                System.Console.WriteLine("IM-经销商【DB-测试】：结束" + DateTime.Now);
            }
            catch (Exception e)
            {
                Loger.Log4Net.Error(e);
            }

            
        }
    }
}
