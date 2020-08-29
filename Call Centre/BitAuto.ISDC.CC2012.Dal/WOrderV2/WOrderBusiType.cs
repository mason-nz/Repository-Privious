using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WOrderBusiType : DataBase
    {
        public static WOrderBusiType Instance = new WOrderBusiType();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetAllData(Entities.QueryWOrderBusiTypeInfo query)
        {
            string sql = "SELECT [RecID] ,[BusiTypeName] ,[SortNum] ,[Status] FROM WOrderBusiType WHERE status<>'-1'";
            if (!string.IsNullOrEmpty(query.RecID))
            {
                sql += " and RecID=" + SqlFilter(query.RecID);
            }
            if (!string.IsNullOrEmpty(query.NoRecID))
            {
                sql += " and RecID<>" + SqlFilter(query.NoRecID);
            }
            if (!string.IsNullOrEmpty(query.BusiTypeName))
            {
                sql += " and BusiTypeName='" + SqlFilter(query.BusiTypeName) + "'";
            }
            if (!string.IsNullOrEmpty(query.Status))
            {
                sql += " and status in (" + SqlFilter(query.Status) + ")";
            }
            sql += " ORDER BY SortNum";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 最大排序号
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum()
        {
            string sql = "SELECT max(SortNum) FROM WOrderBusiType ";
            object max = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            try
            {
                int maxSort = Int32.Parse(max.ToString());
                return maxSort;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// 修改 事物
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool UpdateTran(List<Entities.WOrderBusiTypeInfo> list)
        {
            string connectionstrings = CONNECTIONSTRINGS;
            SqlConnection conn = new SqlConnection(connectionstrings);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction("businesstype");
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    CommonDal.Instance.UpdateComAdoInfo<Entities.WOrderBusiTypeInfo>(list[i], tran);
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }

                throw ex;

            }
            finally
            {
                conn.Close();
            }
            return false;


        }

    }
}
