using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class ADDetailInfo
    {
        public ADDetailInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _SubOrderID = Constant.STRING_INVALID_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            _PubID = Constant.INT_INVALID_VALUE;
            _ADLaunchIDs = Constant.STRING_INVALID_VALUE;
            _ADLaunchStr = Constant.STRING_INVALID_VALUE;       
            _OriginalPrice = 0;
            _AdjustPrice = 0;
            _AdjustDiscount = 0;
            _PurchaseDiscount = 0;
            _SaleDiscount = 0;
            _ADLaunchDays = 0;
            _LastUpdateTime = Constant.DATE_INVALID_VALUE;
            _LastUpdateUserID = Constant.INT_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;
            _PubDetailID = Constant.INT_INVALID_VALUE;
        }
        public bool EnableOriginPrice { get; set; } = false;
        public int ChannelID { get; set; } = -2;
        public decimal CostReferencePrice { get; set; } = 0;
        public decimal CostPrice { get; set; } = 0;
        public decimal FinalCostPrice { get; set; } = 0;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public int Status { get; set; } = -2;
        public int CityExtendID { get; set; } = -2;
        private int _PubDetailID;

        public int PubDetailID
        {
            get { return _PubDetailID; }
            set { _PubDetailID = value; }
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

        private string _ADLaunchIDs;

        public string ADLaunchIDs
        {
            get { return _ADLaunchIDs; }
            set { _ADLaunchIDs = value; }
        }

        private string _ADLaunchStr;

        public string ADLaunchStr
        {
            get { return _ADLaunchStr; }
            set { _ADLaunchStr = value; }
        }

        private decimal _OriginalPrice;

        public decimal OriginalPrice
        {
            get { return _OriginalPrice; }
            set { _OriginalPrice = value; }
        }

        private decimal _AdjustPrice;

        public decimal AdjustPrice
        {
            get { return _AdjustPrice; }
            set { _AdjustPrice = value; }
        }

        private decimal _AdjustDiscount;

        public decimal AdjustDiscount
        {
            get { return _AdjustDiscount; }
            set { _AdjustDiscount = value; }
        }

        private decimal _PurchaseDiscount;

        public decimal PurchaseDiscount
        {
            get { return _PurchaseDiscount; }
            set { _PurchaseDiscount = value; }
        }

        private decimal _SaleDiscount;

        public decimal SaleDiscount
        {
            get { return _SaleDiscount; }
            set { _SaleDiscount = value; }
        }

        private int _ADLaunchDays;

        public int ADLaunchDays
        {
            get { return _ADLaunchDays; }
            set { _ADLaunchDays = value; }
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
        
        
        private int _RecID;

        public int RecID
        {
            get { return _RecID; }
            set { _RecID = value; }
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
        public int HasHoliday { get; set; } = 0;
        public int Holidays { get; set; } = 0;
        public decimal SalePrice_Holiday { get; set; } = 0;
        public int SaleArea { get; set; } = -2;
        public int expired { get; set; } = 0;
        public int PublishStatus { get; set; } = -2;
    }
}
