/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 15:37:20
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.QueryPage
{
    public class PublicPageQuery : DataBase
    {
        #region Instance

        public static readonly PublicPageQuery Instance = new PublicPageQuery();

        #endregion Instance

        /// <summary>
        /// 刊例查询列表 公共方法，Auth：李雄
        /// 因为5种媒体查询条件相似，只是返回数据格式稍微不同，而都是调用公共的分页方法，只需要传入不同的sql+where即可
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(QueryPageBase<T> query)
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

        public List<T> QueryList<T>(QueryPageBase<T> query, string excel)
        {

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, query.StrSql);
            return query.DataList = DataTableToList<T>(data.Tables[0]);
        }

        /// <summary>
        /// Auth：李雄
        /// 公共查询分页存储过程（2012以上版本可用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryListBySql2014Offset_Fetch<T>(QueryPageBase<T> query)
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