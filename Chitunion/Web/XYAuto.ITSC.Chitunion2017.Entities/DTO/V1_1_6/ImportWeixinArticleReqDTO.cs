using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class ImportWeixinArticleReqDTO
    {
        public int ImportType { get; set; }
        public string ImportUrl { get; set; }
        public List<int> ImportWxIDs { get; set; }
    }
}
