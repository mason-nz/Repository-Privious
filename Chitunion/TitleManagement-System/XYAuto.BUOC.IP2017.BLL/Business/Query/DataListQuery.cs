using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.Query
{
    public class DataListQuery
    {
        #region Instance

        public static readonly DataListQuery Instance = new DataListQuery();

        #endregion Instance

        #region Contructor

        protected DataListQuery()
        { }

        #endregion Contructor

        /// <summary>
        /// 因为5种媒体查询条件相似，只是返回数据格式稍微不同，而都是调用公共的分页方法，只需要传入不同的sql+where即可
        /// </summary>
        /// <typeparam name="T">返回类型泛型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public List<T> QueryList<T>(Entities.Query.DataListQuery<T> query)
        {
            if (query.PageSize > Util.PageSize)
                query.PageSize = Util.PageSize;
            return Dal.DataListQuery.Instance.QueryList<T>(query);
        }

        /// <summary>
        /// 公共查询分页存储过程（2012以上版本可用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> QueryListBySql2014Offset_Fetch<T>(Entities.Query.DataListQuery<T> query)
        {
            return Dal.DataListQuery.Instance.QueryListBySql2014Offset_Fetch<T>(query);
        }
    }
}
