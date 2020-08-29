using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities.BusinessReport
{
    public class QueryProjectReport
    {
        public QueryProjectReport()
		{
            _projectid = Constant.INT_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            agentnum = Constant.STRING_INVALID_VALUE;
            begintime = Constant.DATE_INVALID_VALUE;
            endtime = Constant.DATE_INVALID_VALUE;
            _businesstype = Constant.INT_INVALID_VALUE;
		}
        private int? _businesstype;
        public int? BusinessType
        {
            set
            {
                _businesstype = value;
            }
            get
            {
                return _businesstype;
            }
        }
        private DateTime begintime;
        public DateTime BeginTime
        {
            set
            {
                begintime = value;
            }
            get
            {
                return begintime;
            }
        }
        private DateTime endtime;
        public DateTime EndTime
        {
            set
            {
                endtime = value;
            }
            get
            {
                return endtime;
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
        private int? _projectid;
        public int? ProjectID
        {
            set
            {
                _projectid = value;
            }
            get
            {
                return _projectid;
            }
        }
    }
}
