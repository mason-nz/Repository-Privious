using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class ADOrderInfo
    {
        public ADOrderInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_EMPTY_VALUE;
            _OrderName = Constant.STRING_EMPTY_VALUE;
            _BeginTime = Constant.DATE_INVALID_VALUE;
            _EndTime = Constant.DATE_INVALID_VALUE;
            _Note = Constant.STRING_EMPTY_VALUE;
            _UploadFileURL = Constant.STRING_EMPTY_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            //_TotalAmount = Constant.INT_INVALID_VALUE;
            _TotalAmount = 0;
            _Status = Constant.INT_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;
            _CustomerID = Constant.INT_INVALID_VALUE;
            _CreatorName = Constant.STRING_EMPTY_VALUE;
            _CustomerName = Constant.STRING_EMPTY_VALUE;
            _CreatorUserName = Constant.STRING_EMPTY_VALUE;
            _CustomerUserName = Constant.STRING_EMPTY_VALUE;
        }
        public string MasterName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string SerialName { get; set; } = string.Empty;
        public string CrmNum { get; set; } = string.Empty;
        public string FinalReportURL { get; set; } = string.Empty;
        public int OrderType { get; set; } = -2;
        public decimal CostTotal { get; set; } = 0;
        public decimal BudgetTotal { get; set; } = 0;
        public string OrderRemark { get; set; } = string.Empty;
        public string MasterBrand { get; set; } = string.Empty;
        public string CarBrand { get; set; } = string.Empty;
        public string CarSerial { get; set; } = string.Empty;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public bool JKEntrance { get; set; } = false;
        public string MarketingPolices { get; set; } = string.Empty;
        public string MarketingUrl { get; set; } = string.Empty;
        public int MasterID { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public string CRMCustomerID { get; set; } = string.Empty;
        public string CustomerText { get; set; } = string.Empty;
        private int _CustomerID;

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }               

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

        private string _OrderName;

        public string OrderName
        {
            get { return _OrderName; }
            set { _OrderName = value; }
        }

        private DateTime _BeginTime;

        public DateTime BeginTime
        {
            get { return _BeginTime; }
            set { _BeginTime = value; }
        }

        private DateTime _EndTime;

        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        private string _Note;

        public string Note
        {
            get { return _Note; }
            set { _Note = value; }
        }

        private string _UploadFileURL;

        public string UploadFileURL
        {
            get { return _UploadFileURL; }
            set { _UploadFileURL = value; }
        }

        private int _MediaType;

        public int MediaType
        {
            get { return _MediaType; }
            set { _MediaType = value; }
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

        private string _CreatorName;

        public string CreatorName
        {
            get { return _CreatorName; }
            set { _CreatorName = value; }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }

        private string _CreatorUserName;

        public string CreatorUserName
        {
            get { return _CreatorUserName; }
            set { _CreatorUserName = value; }
        }

        private string _CustomerUserName;

        public string CustomerUserName
        {
            get { return _CustomerUserName; }
            set { _CustomerUserName = value; }
        }
        
    }
}
