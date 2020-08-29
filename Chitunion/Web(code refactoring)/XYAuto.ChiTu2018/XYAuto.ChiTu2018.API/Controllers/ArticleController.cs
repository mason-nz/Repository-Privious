using System.Web.Http;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Service;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 文章测试类
    /// </summary>
    public class ArticleController : ApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:测试
        /// </summary>
        /// <returns></returns>
        [Route("article/getArticlePage")]
        public IHttpActionResult GetArticlePage()
        {
            return Ok(Article.Instance.ListArticlePages(1, 1).Item1);
        }

        /// <summary>
        /// 文章-列表查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("article/query")]
        [HttpGet]
        public IHttpActionResult Query([FromUri]ArticleVO query)
        {
            return Ok(query);
        }

        /// <summary>
        /// 文章-推送
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("article/push")]
        [HttpPost]
        public IHttpActionResult Push([FromBody]ArticleVO query)
        {
            return Ok(query);
        }
    }
}
