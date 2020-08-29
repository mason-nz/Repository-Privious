using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryBlackWhiteListOperLog
    {
        public QueryBlackWhiteListOperLog()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _bwrecid = Constant.INT_INVALID_VALUE;
            _bwtype = Constant.INT_INVALID_VALUE;
            _phonenum = Constant.STRING_INVALID_VALUE;
            _callid = null;
            _opertype = Constant.INT_INVALID_VALUE;
            _operuserid = Constant.INT_INVALID_VALUE;
            _opertime = Constant.DATE_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;

        }
        private int _recid;
        private int _bwrecid;
        private int _bwtype;
        private string _phonenum;
        private long? _callid;
        private int _opertype;
        private int _operuserid;
        private DateTime _opertime;
        private string _custid;
        public string CustID
        {
            get { return _custid; }
            set { _custid = value; }
        }
        public int RecId
        {
            get { return _recid; }
            set { _recid = value; }
        }
        public int BWRecID
        {
            set { _bwrecid = value; }
            get { return _bwrecid; }
        }
        public int BWType
        {
            set { _bwtype = value; }
            get { return _bwtype; }
        }
        public string PhoneNum
        {
            set { _phonenum = value; }
            get { return _phonenum; }
        }
        public long? CallID
        {
            get { return _callid; }
            set { _callid = value; }
        }
        public int OperType
        {
            get { return _opertype; }
            set { _opertype = value; }
        }
        public int OperUserID
        {
            get { return _operuserid; }
            set { _operuserid = value; }
        }
        public DateTime OperTime
        {
            set { _opertime = value; }
            get { return _opertime; }
        }
    }
}
