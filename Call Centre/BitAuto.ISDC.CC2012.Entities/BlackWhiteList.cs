using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class BlackWhiteList
    {
        public BlackWhiteList()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _type = Constant.INT_INVALID_VALUE;
            _phonenum = Constant.STRING_INVALID_VALUE;
            _effectivedate = Constant.DATE_INVALID_VALUE;
            _expirydate = Constant.DATE_INVALID_VALUE;
            _calltype = Constant.INT_INVALID_VALUE;
            _cdids = Constant.INT_INVALID_VALUE;
            _reason = Constant.STRING_INVALID_VALUE;
            _synchrodatastatus = Constant.INT_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createdate = Constant.DATE_INVALID_VALUE;
            _updateuserid = Constant.INT_INVALID_VALUE;
            _updatedate = Constant.DATE_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _callid = null;
            _calloutndtype = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
        }

        private int _recid;
        private int _type;
        private string _phonenum;
        private DateTime _effectivedate;
        private DateTime _expirydate;
        private int _calltype;
        private int _cdids;
        private string _reason;
        private int _synchrodatastatus;
        private int _createuserid;
        private DateTime _createdate;
        private int _updateuserid;
        private DateTime _updatedate;
        private int _status;
        private long? _callid;
        private int _calloutndtype;
        private int _bgid;

        public int BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }

        public int RecId
        {
            get { return _recid; }
            set { _recid = value; }
        }
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        public string PhoneNum
        {
            set { _phonenum = value; }
            get { return _phonenum; }
        }
        public DateTime EffectiveDate
        {
            set { _effectivedate = value; }
            get { return _effectivedate; }
        }
        public DateTime ExpiryDate
        {
            set { _expirydate = value; }
            get { return _expirydate; }
        }
        public int CallType
        {
            set { _calltype = value; }
            get { return _calltype; }
        }
        public int CDIDS
        {
            set { _cdids = value; }
            get { return _cdids; }
        }
        public string Reason
        {
            set { _reason = value; }
            get { return _reason; }
        }
        public int SynchrodataStatus
        {
            set { _synchrodatastatus = value; }
            get { return _synchrodatastatus; }
        }
        public int CreateUserId
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        public int UpdateUserId
        {
            set { _updateuserid = value; }
            get { return _updateuserid; }
        }
        public DateTime UpdateDate
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        public long? CallID
        {
            get { return _callid; }
            set { _callid = value; }
        }
        public int CallOutNDType
        {
            get { return _calloutndtype; }
            set { _calloutndtype = value; }
        }
    }
}
