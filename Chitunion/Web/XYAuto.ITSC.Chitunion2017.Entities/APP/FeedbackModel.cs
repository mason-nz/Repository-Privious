using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.APP
{
    public class FeedbackModel
    {
        public int UserID { get; set; }

        public string OpinionText { get; set; }

        public string CreateTime { get; set; }

        public string ReplyText { get; set; }

        public string ReplyTime { get; set; }
    }
}
