using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QueryADOrderOperateInfo
    {
        public QueryADOrderOperateInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _SubOrderID = Constant.STRING_INVALID_VALUE;
            _OptType = Constant.INT_INVALID_VALUE;
            _OrderStatus = Constant.INT_INVALID_VALUE;
            _RejectMsg = Constant.STRING_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;            
        }

        private int _OrderStatus;

        public int OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; }
        }
        

        private string _OrderID;

        public string OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        private string _SubOrderID;

        public string SubOrderID
        {
            get { return _SubOrderID; }
            set { _SubOrderID = value; }
        }

        private int _OptType;

        public int OptType
        {
            get { return _OptType; }
            set { _OptType = value; }
        }
        
        
        private int _RecID;

        public int RecID
        {
            get { return _RecID; }
            set { _RecID = value; }
        }

        
        
        private string _RejectMsg;

        public string RejectMsg
        {
            get { return _RejectMsg; }
            set { _RejectMsg = value; }
        }
        
        
        private DateTime _CreateTime;

        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        private int _CreateUserID;

        public int CreateUserID
        {
            get { return _CreateUserID; }
            set { _CreateUserID = value; }
        }
    }
}
