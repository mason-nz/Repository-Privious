using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class SubADInfo
    {
        public SubADInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _subOrderID = Constant.STRING_INVALID_VALUE;            
            _MediaType = Constant.INT_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            //_TotalAmount = Constant.INT_INVALID_VALUE;
            _TotalAmount = 0;
            _Status = Constant.INT_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;
            _LastUpdateTime = Constant.DATE_INVALID_VALUE;
            _LastUpdateUserID = Constant.INT_INVALID_VALUE;
        }
        public int ChannelID { get; set; } = -2;
        private int _RecID;

        public int RecID
        {
            get { return _RecID; }
            set { _RecID = value; }
        }

        private string _OrderID;

        public string OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
        }

        private string _subOrderID;

        public string SubOrderID
        {
            get { return _subOrderID; }
            set { _subOrderID = value; }
        }
        
      
        private int _MediaType;

        public int MediaType
        {
            get { return _MediaType; }
            set { _MediaType = value; }
        }

        private int _MediaID;

        public int MediaID
        {
            get { return _MediaID; }
            set { _MediaID = value; }
        }
        
        private decimal _TotalAmount;

        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set { _TotalAmount = value; }
        }

        private int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
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

        private DateTime _LastUpdateTime;

        public DateTime LastUpdateTime
        {
            get { return _LastUpdateTime; }
            set { _LastUpdateTime = value; }
        }

        private int _LastUpdateUserID;

        public int LastUpdateUserID
        {
            get { return _LastUpdateUserID; }
            set { _LastUpdateUserID = value; }
        }
        
    }
}
