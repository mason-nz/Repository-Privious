using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class ExclusiveMissedCalls
    {
        public string ANI { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }     
        public string AgentID { get; set; }
        public string AgentNum { get; set; }
        public string AgentGroup { get; set; }
    }
    
}
