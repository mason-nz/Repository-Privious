using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryBlackWhite
    {
        public QueryBlackWhite()
        {
            _type = Constant.INT_INVALID_VALUE;
            _calltype = Constant.INT_INVALID_VALUE;
            _cdids = Constant.INT_INVALID_VALUE;
            _phonenum = Constant.STRING_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

            _querycreatestartdate = Constant.DATE_INVALID_VALUE;
            _querycreateenddate = Constant.DATE_INVALID_VALUE;
            _effectivedate = Constant.DATE_INVALID_VALUE;
            _expirydate = Constant.DATE_INVALID_VALUE;

            _querycalltypes = Constant.STRING_INVALID_VALUE;
            _querycdids = Constant.STRING_INVALID_VALUE;
            _queryloginuserid = Constant.INT_INVALID_VALUE;
         }

        private int _type;
        private int _calltype;
        private int _cdids;
        private string _phonenum;
        private int _createuserid;
        private DateTime _querycreatestartdate;
        private DateTime _querycreateenddate;
        private DateTime _effectivedate;
        private DateTime _expirydate;

        private string _querycalltypes;
        private string _querycdids;
        private int _queryloginuserid;

        public int QueryLoginUserId
        {
            set { _queryloginuserid = value; }
            get { return _queryloginuserid; }
        }

        public string QueryCallTypes
        {
            set { _querycalltypes = value; }
            get { return _querycalltypes; }
        }
        public string QueryCDIDs
        {
            set { _querycdids = value; }
            get { return _querycdids; }
        }

        public int Type
        {
            set { _type = value; }
            get { return _type; }
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
        public string PhoneNum
        {
            set { _phonenum = value; }
            get { return _phonenum; }
        }
        public int CreateUserId
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        public DateTime QueryCreateStartDate
        {
            set { _querycreatestartdate = value; }
            get { return _querycreatestartdate; }
        }
        public DateTime QueryCreateEndDate
        {
            set { _querycreateenddate = value; }
            get { return _querycreateenddate; }
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

         
    }
}
