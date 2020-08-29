using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO
{
    public class ResSummaryKeyWordDTO
    {
        public int code { get; set; } = -2;
        public ResSummaryKeyWorddataDTO data { get; set; }
        public string msg { get; set; } = string.Empty;
        public int sub_code { get; set; } = -2;
    }
    public class ResSummaryKeyWorddataDTO
    {
        public string KeyWord { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

    }
}
