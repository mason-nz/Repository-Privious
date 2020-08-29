using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class MoveArticleReqDTO
    {
        public int ArticleID { get; set; }
        public int OptType { get; set; }
    }

    public class BatchDeleteArticleReqDTO {
        public int GroupID { get; set; }
        public List<int> ArticleIDs { get; set; }
    }
}
