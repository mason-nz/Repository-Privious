using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
    public class QueryBussinessLineTotal
    {
        private int? _sourcetype;
        private DateTime? begintime;
        private DateTime? endtime;
        //统计方式，1，日，2，周，3.月,4.小时
        private int? _selecttype;
        public QueryBussinessLineTotal()
        {
            _sourcetype = Constant.INT_INVALID_VALUE;
            begintime = Constant.DATE_INVALID_VALUE;
            endtime = Constant.DATE_INVALID_VALUE;
            _selecttype = Constant.INT_INVALID_VALUE;
        }
        public int? SourceType
        {
            set { _sourcetype = value; }
            get { return _sourcetype; }
        }
        public int? SelectType
        {
            set { _selecttype = value; }
            get { return _selecttype; }
        }

        public DateTime? BeginTime
        {
            set { begintime = value; }
            get { return begintime; }
        }
        public DateTime? EndTime
        {
            set { endtime = value; }
            get { return endtime; }
        }
    }
}
