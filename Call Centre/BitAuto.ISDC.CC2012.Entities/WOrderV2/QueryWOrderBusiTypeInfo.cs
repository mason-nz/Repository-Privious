using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryWOrderBusiTypeInfo
    {
        public string RecID { get; set; }

        /// <summary>
        /// 不等于RecID
        /// </summary>
        public string NoRecID { get; set; }

        public string BusiTypeName { get; set; }

        public string Status { get; set; }
    }
}
