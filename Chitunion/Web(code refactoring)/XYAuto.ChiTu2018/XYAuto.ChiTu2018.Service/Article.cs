using System;
using XYAuto.ChiTu2018.BO;

namespace XYAuto.ChiTu2018.Service
{
    /// <summary>
    /// 注释：具体业务逻辑 将来列表成分微服务，实现自理
    /// 作者：guansl
    /// 日期：2018/4/19 
    /// </summary>
    public class Article
    {
        private Article() { }
        private static readonly Lazy<Article> linstance = new Lazy<Article>(() => { return new Article(); });

        public static Article Instance { get { return linstance.Value; } }


        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>总记录数和页数</returns>
        public Tuple<int, int> ListArticlePages(int? pageIndex, int? pageSize)
        {
            int count;
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                var result = new AccountArticleBO().GetAccountArticleList(pageIndex.Value, pageSize.Value, out count);

                var size = count == 0 ? 0 : count / pageSize.Value;

                return Tuple.Create(count, size);
            }
            else
            {
                return Tuple.Create(0, 0);
            }
        }
    }
}
