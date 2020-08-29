using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryCustomerVoiceMsg
    {
        public string ANI { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string selBusinessType { get; set; }
        public string Agent { get; set; }
        public string PRBeginTime { get; set; }
        public string PREndTime { get; set; }
        public string prStatus { get; set; }
        public string HasSkill { get; set; }
        public int SourceType { get; set; }
        public string Hasaudio { get; set; }
        public string IsExclusive { get; set; }//是否专属客服， 数据库无此字段
    }
}
