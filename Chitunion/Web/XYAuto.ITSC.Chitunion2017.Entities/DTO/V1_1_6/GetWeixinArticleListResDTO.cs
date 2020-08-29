using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class GetWeixinArticleListResDTO
    {
        public List<ArticleItem> List { get; set; }
        public int TotalCount { get; set; }
    }
}
