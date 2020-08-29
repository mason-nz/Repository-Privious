using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.Entities
{
    [Serializable]
    public class CEntity
    {
        public string Tel
        { set; get; }
        public string Name
        { set; get; }
        public string Sex
        { set; get; }
        public string CustID
        { set; get; }
        public string ProvinceID
        { set; get; }
        public string CityID
        { set; get; }
    }
}
