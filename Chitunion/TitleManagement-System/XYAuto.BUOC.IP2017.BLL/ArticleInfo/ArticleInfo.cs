using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.ArticleInfo
{
    public class ArticleInfo
    {
        public readonly static ArticleInfo Instance = new ArticleInfo();
        public Entities.ArticleInfo.ArticleInfo QueryArticle(int articleID, int mediaType)
        {
            return Util.DataTableToEntity<Entities.ArticleInfo.ArticleInfo>(Dal.ArticleInfo.ArticleInfo.Instance.QueryArticle(articleID, mediaType));
        }
    }
}
