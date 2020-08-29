using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query
{
    public class SearchTitleResponse
    {
        public int MediaId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
    }

    public class PublishStatisticsCount
    {
        public int AppendAuditCount { get; set; }//待审核数量
        public int RejectNotPassCount { get; set; }//驳回数量
        public int AuditPassCount { get; set; }//审核通过
    }
}