using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.WangyiDun
{
    public class TextResult
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public Result result { get; set; }
    }

    public class Result
    {
        public string taskId { get; set; } = string.Empty;
        public int action { get; set; }
        public List<Label> labels { get; set; }
    }

    public class Label
    {
        public int label { get; set; }
        public int level { get; set; }
        public Details details { get; set; }
    }

    public class Details
    {
        public string[] hint { get; set; }
    }
}
