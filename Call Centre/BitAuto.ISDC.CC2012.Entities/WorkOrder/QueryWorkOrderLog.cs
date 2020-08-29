using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryWorkOrderLog
    {
        public QueryWorkOrderLog()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _orderid = Constant.STRING_EMPTY_VALUE;
            _receiverrecid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
        }

        private int _recid;
        private string _orderid;
        private int _receiverrecid;
        private string _logdesc;
        private DateTime? _createtime;
        private int? _createuserid;

        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }

        public string OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }

        public int ReceiverRecID
        {
            set { _receiverrecid = value; }
            get { return _receiverrecid; }
        }
        public string LogDesc
        {
            set { _logdesc = value; }
            get { return _logdesc; }
        }
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }


    }
}
