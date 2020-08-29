using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities.BusinessReport
{
    public class QueryReturnVisitReport
    {
        public QueryReturnVisitReport()
        {
            _year = Constant.INT_INVALID_VALUE;
            _month = Constant.INT_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            agentnum = Constant.STRING_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
        }
        private int? _year;
        public int? Year
        {
            set
            {
                _year = value;
            }
            get
            {
                return _year;
            }
        }
        private int? _month;
        public int? Month
        {
            set
            {
                _month = value;
            }
            get
            {
                return _month;
            }
        }
        private string agentnum;
        public string AgentNum
        {
            set
            {
                agentnum = value;
            }
            get
            {
                return agentnum;
            }
        }
        private int? _userid;
        public int? UserID
        {
            set
            {
                _userid = value;
            }
            get
            {
                return _userid;
            }
        }
        private int? _bgid;
        public int? BGID
        {
            set
            {
                _bgid = value;
            }
            get
            {
                return _bgid;
            }
        }
    }
}
