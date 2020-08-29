using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    public class PublishInfoQuery
    {
        #region Instance

        public static readonly PublishInfoQuery Instance = new PublishInfoQuery();

        #endregion Instance

        #region Contructor

        protected PublishInfoQuery()
        { }

        #endregion Contructor

        /// <summary>
        /// 刊例查询列表 公共方法，Auth：李雄
        /// 因为5种媒体查询条件相似，只是返回数据格式稍微不同，而都是调用公共的分页方法，只需要传入不同的sql+where即可
        /// </summary>
        /// <typeparam name="T">返回类型泛型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public List<T> QueryList<T>(QueryPageBase<T> query)
        {
            if (query.PageSize > Util.PageSize)
                query.PageSize = Util.PageSize;
            return Dal.Publish.PublishInfoQuery.Instance.QueryList<T>(query);
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
            return Dal.Publish.PublishInfoQuery.Instance.QueryListBySql2014Offset_Fetch<T>(query);
        }

        [Obsolete("经测试，p_Page存储过程不支持返回多个table")]
        public Tuple<List<T>, List<PublishItemInfo>> QueryListByTuple<T>(PublishQuery<T> query)
        {
            return Dal.Publish.PublishInfoQuery.Instance.QueryListByTuple<T>(query);
        }

        public List<PublishDetailInfo> QueryPublishItemInfo(PublishQuery<PublishDetailInfo> query)
        {
            return Dal.Publish.PublishInfoQuery.Instance.QueryPublishItemInfo(query);
        }

        public PublishBasicInfo GetPublishBasicInfo(PublishQuery<PublishBasicInfo> query)
        {
            return Dal.Publish.PublishInfoQuery.Instance.GetPublishBasicInfo(query);
        }

        public List<RespPublishAuditInfoDto> GetPublishAuditInfoList(
            PublishAuditInfoQuery<RespPublishAuditInfoDto> query)
        {
            return Dal.Publish.PublishInfoQuery.Instance.GetPublishAuditInfoList(query);
        }
    }
}