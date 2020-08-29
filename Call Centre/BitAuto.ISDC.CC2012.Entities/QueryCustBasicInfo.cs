using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryCustBasicInfo
    {
        public string CustName { get; set; }
        public string Sexs { get; set; }
        public string CustTel { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public int CountyID { get; set; }
    }
}

