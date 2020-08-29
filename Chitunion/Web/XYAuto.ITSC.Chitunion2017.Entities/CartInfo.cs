using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class CartInfo
    {
        public CartInfo()
        {
            _CartID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            _PubID = Constant.INT_INVALID_VALUE;
            _PubDetailID = Constant.INT_INVALID_VALUE;
            _IsSelected = 1;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;

            _ADSchedule = Constant.DATE_INVALID_VALUE;
        }
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
        private DateTime _ADSchedule;

        public DateTime ADSchedule
        {
            get { return _ADSchedule; }
            set { _ADSchedule = value; }
        }

        private int _PubDetailID;

        public int PubDetailID
        {
            get { return _PubDetailID; }
            set { _PubDetailID = value; }
        }
        

        private byte _IsSelected;

        public byte IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }
        

        private int _CartID;

        public int CartID
        {
            get { return _CartID; }
            set { _CartID = value; }
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

        private int _PubID;

        public int PubID
        {
            get { return _PubID; }
            set { _PubID = value; }
        }

       

        private string _OrderID;

        public string OrderID
        {
            get { return _OrderID; }
            set { _OrderID = value; }
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
        public int? ADLaunchDays { get; set; } = -2;
        public int? SaleArea { get; set; } = -2;
    }
}
