using System;
using System.Collections.Generic;
using System.Text;


namespace BitAuto.ISDC.CC2012.Entities.CallReport
{
    public class QueryInComingCallDetails
    {
         public string StartTime { get; set; }
         public string EndTime { get; set; }
         public string AgentID { get; set; }
         //public string AgentNum { get; set; }
         public string BusinessType { get; set; }
         public string QueryArea { get; set; }
         public string QueryType { get; set; }
         public int LoginUserId { get; set; }

    }
}
