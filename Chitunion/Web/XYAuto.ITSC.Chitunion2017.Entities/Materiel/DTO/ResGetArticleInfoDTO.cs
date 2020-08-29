using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel.DTO
{
    public class ResGetArticleInfoDTO
    {
        public int ArticleId { get; set; } = -2;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
    }
}
