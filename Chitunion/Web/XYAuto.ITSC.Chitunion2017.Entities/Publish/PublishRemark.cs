using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Publish
{
    public class PublishRemark
    {
        public int RecID { get; set; }
        public int RelationID { get; set; }
        public int RemarkID { get; set; }
        public string OtherContent { get; set; }
        public int EnumType { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }

        public string RemarkName { get; set; }
    }
}
