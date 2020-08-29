using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal
{
    public class DataListQuery : DataBase
    {
        #region Instance

        public static readonly DataListQuery Instance = new DataListQuery();

        #endregion Instance

        /// <summary>
        /// 刊例查询列表 公共方法，Auth：李雄
        /// 因为5种媒体查询条件相似，只是返回数据格式稍微不同，而都是调用公共的分页方法，只需要传入不同的sql+where即可
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(DataListQuery<T> query)
        {
            const string storedProcedure = "p_Page";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",query.StrSql),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order",query.OrderBy)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            return query.DataList = DataTableToList<T>(data.Tables[0]);
        }

        /// <summary>
        /// Auth：李雄
        /// 公共查询分页存储过程（2012以上版本可用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryListBySql2014Offset_Fetch<T>(DataListQuery<T> query)
        {
            const string storedProcedure = "p_Page_OFFSET_FETCH";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",query.StrSql),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order",query.OrderBy)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            return query.DataList = DataTableToList<T>(data.Tables[0]);
        }
    }
}
