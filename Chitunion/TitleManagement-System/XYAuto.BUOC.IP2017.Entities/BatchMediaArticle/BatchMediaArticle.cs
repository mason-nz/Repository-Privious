using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.BatchMediaArticle
{
    public class BatchMediaArticle
    {
        public int RecID { get; set; } = -2;
        public int BatchMediaID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public int MediaID { get; set; } = -2;
        public int ArticleID { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
    }
}
