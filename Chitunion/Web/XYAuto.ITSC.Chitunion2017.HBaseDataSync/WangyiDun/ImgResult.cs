using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.WangyiDun
{
    public class ImgResult
    {
        public int code { get; set; }
        public string msg { get; set; } = string.Empty;
        public List<ImgResultDetail> result { get; set; }
    }

    public class ImgResultDetail
    {
        public string taskId { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public List<ImgLabel> labels { get; set; }
    }

    public class ImgLabel
    {
        public int label { get; set; }
        public int level { get; set; }
        public decimal rate { get; set; }
    }
}
