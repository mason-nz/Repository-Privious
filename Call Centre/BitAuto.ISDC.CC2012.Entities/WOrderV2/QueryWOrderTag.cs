using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryWOrderTag
    {
        public string RecID { get; set; }

        /// <summary>
        /// 不等于RecID
        /// </summary>
        public string NoRecID { get; set; }

        public string BusiTypeID { get; set; }

        public string TagName { get; set; }

        public string PID { get; set; }


        /// <summary>
        /// 是否根据PID查询
        /// </summary>
        public bool IsPIDSearch { get; set; }

        public string Status { get; set; }
    }
}
