using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaCommonlyClass
    {
        public int RecID { get; set; }
        public int MediaID { get; set; }

        public int MediaType { get; set; }
        public int CategoryID { get; set; }

        public int SortNumber { get; set; }
        public List<int> CategoryIDs { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }

        public string CategoryName { get; set; }
    }
}