using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class WorkOrderReceiver
    {
        public WorkOrderReceiver()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _orderid = Constant.STRING_EMPTY_VALUE;
            _receivertext = Constant.STRING_EMPTY_VALUE;
            _receiveruserid = Constant.INT_INVALID_VALUE;
            _receiverdepartname = Constant.STRING_EMPTY_VALUE;
            _callid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _audiourl = Constant.STRING_EMPTY_VALUE;
        }

        private int _recid;
        private string _orderid;
        private int? _receiveruserid;
        private string _receivertext;
        private string _receiverdepartname;
        private long? _callid;
        private DateTime? _createtime;
        private int? _createuserid;
        private string _audiourl;

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
        public string RevertContent
        {
            set { _receivertext = value; }
            get { return _receivertext; }
        }

        public string ReceiverDepartName
        {
            set { _receiverdepartname = value; }
            get { return _receiverdepartname; }
        }


        public int? ReceiverUserID
        {
            set { _receiveruserid = value; }
            get { return _receiveruserid; }
        }

        public long? CallID
        {
            set { _callid = value; }
            get { return _callid; }
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

        public string AudioURL
        {
            set { _audiourl = value; }
            get { return _audiourl; }
        }
    }
}
