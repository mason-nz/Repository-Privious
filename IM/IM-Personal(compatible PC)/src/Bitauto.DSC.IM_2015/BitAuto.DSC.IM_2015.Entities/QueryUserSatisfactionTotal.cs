using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
    public class QueryUserSatisfactionTotal
    {
        private int? _bgid;
        private int? _userid;
        private string _agentnum;
        private DateTime? begintime;
        private DateTime? endtime;
        //统计方式，1，日，2，周，3.月
        private int? _selecttype;
        public QueryUserSatisfactionTotal()
        {
            _bgid = Constant.INT_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            _agentnum = Constant.STRING_INVALID_VALUE;
            begintime = Constant.DATE_INVALID_VALUE;
            endtime = Constant.DATE_INVALID_VALUE;
            _selecttype = Constant.INT_INVALID_VALUE;
        }
        public int? SelectType
        {
            set { _selecttype = value; }
            get { return _selecttype; }
        }
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }
        public int? UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        public string AgentNum
        {
            set { _agentnum = value; }
            get { return _agentnum; }
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
