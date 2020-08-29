using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.MediaBase
{
    public class MediaCategory
    {
        public int MediaType { get; set; }
        public int WxID { get; set; }
        public int CategoryID { get; set; }
        public int SortNumber { get; set; }
    }
    public class DicInfo
    {
        public int DictId { get; set; }
        public string DictName { get; set; }
    }
}
