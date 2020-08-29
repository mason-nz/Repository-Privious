using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage
{
    /// <summary>
    /// Auth:李雄
    /// 前台/后台刊例查询接口代理类
    /// </summary>
    public class PublishQueryProxy
    {
        private readonly RequestPublishQueryDto _requestPublishBgQuery;
        private readonly RequestFrontPublishQueryDto _requestFrontPublishQuery;
        private readonly ConfigEntity _configEntity = new ConfigEntity();
        private Dictionary<int, Func<string, dynamic>> _dictionary;//前台页面查询接口泛型集合
        private Dictionary<int, Func<string, dynamic>> _dicBgtionary;//后台页面查询接口泛型集合

        public PublishQueryProxy(RequestFrontPublishQueryDto requestFrontPublishQuery)
        {
            _requestFrontPublishQuery = requestFrontPublishQuery;
            InitQuery();
        }

        public PublishQueryProxy(RequestPublishQueryDto requestPublishQuery)
        {
            _requestPublishBgQuery = requestPublishQuery;
            InitBgQuery();
        }

        /// <summary>
        /// 初始化后台查询泛型集合字典
        /// </summary>
        private void InitBgQuery()
        {
            _dicBgtionary = new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s => new PbWeiXinQuery(_configEntity).GetQueryList(_requestPublishBgQuery)},
                {(int) MediaType.WeiBo, s => new PbWeiBoQuery(_configEntity).GetQueryList(_requestPublishBgQuery)},
                {(int) MediaType.Video, s => new PbVideoQuery(_configEntity).GetQueryList(_requestPublishBgQuery)},
                {(int) MediaType.Broadcast, s => new PbBroadcastQuery(_configEntity).GetQueryList(_requestPublishBgQuery)},
                {(int) MediaType.APP, s => PbAppQueryFilter() }
            };
        }

        /// <summary>
        /// 初始化前台查询泛型集合字典
        /// </summary>
        private void InitQuery()
        {
            _dictionary = new Dictionary<int, Func<string, dynamic>>()
            {
                { (int)MediaType.WeiXin, s=> new FpWeiXinQuery(_configEntity).GetQueryList(_requestFrontPublishQuery)},
                {
                    (int)MediaType.WeiBo, s=> new FpWeiBoQuery(_configEntity).GetQueryList(_requestFrontPublishQuery)
                },
                {(int)MediaType.APP, s=> new FpAppQuery(_configEntity).GetQueryList(_requestFrontPublishQuery) },
                {(int)MediaType.Video, s=> new FpVideoQuery(_configEntity).GetQueryList(_requestFrontPublishQuery) },
                {(int)MediaType.Broadcast, s=> new FpBroadcastQuery(_configEntity).GetQueryList(_requestFrontPublishQuery) }
            };
        }

        /// <summary>
        /// 刊例查询（前台、后台）
        /// </summary>
        /// <returns></returns>
        public dynamic GetQuery()
        {
            if (_dictionary != null)
                return _dictionary.ContainsKey(_requestFrontPublishQuery.BusinessType)
                    ? _dictionary[_requestFrontPublishQuery.BusinessType].Invoke(string.Empty)
                    : default(dynamic);
            return _dicBgtionary.ContainsKey(_requestPublishBgQuery.BusinessType)
                ? _dicBgtionary[_requestPublishBgQuery.BusinessType].Invoke(string.Empty)
                : default(dynamic);
        }

        /// <summary>
        /// app后台页面涉及到运营角色区别比较大,所以分开执行
        /// </summary>
        /// <returns></returns>
        private dynamic PbAppQueryFilter()
        {
            var roleEnum = RoleInfoMapping.GetUserRole();
            if (roleEnum == RoleEnum.YunYingOperate || roleEnum == RoleEnum.SupperAdmin)
            {
                return new PbAppQueryByYunYing(_configEntity).GetQueryList(_requestPublishBgQuery);
            }
            return new PbAppQuery(_configEntity).GetQueryList(_requestPublishBgQuery);
        }
    }
}