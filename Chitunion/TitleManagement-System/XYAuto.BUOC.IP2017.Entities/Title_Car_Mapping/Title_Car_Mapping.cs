using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Title_Car_Mapping
{
    public class Title_Car_Mapping
    {
        public int RecID { get; set; }
        public int TitleID { get; set; }
        public int CarBrandID { get; set; }
        public int CSID { get; set; }
        public int Status { get; set; } = 0;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; }
        public int Type { get; set; } = -2;
    }
    public class Title_Car_MappingForBatch
    {
        public int TitleID { get; set; }
        public int CarBrandID { get; set; }
        public int CSID { get; set; }
        public int Status { get; set; } = 0;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; }
    }
}
