using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QueryADOrderInfo
    {
        public QueryADOrderInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _OrderName = Constant.STRING_INVALID_VALUE;
            _BeginTime = Constant.DATE_INVALID_VALUE;
            _EndTime = Constant.DATE_INVALID_VALUE;
            _Note = Constant.STRING_INVALID_VALUE;
            _UploadFileURL = Constant.STRING_INVALID_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            _TotalAmount = Constant.INT_INVALID_VALUE;
            _Status = Constant.INT_INVALID_VALUE;
            _CreaetUserID = Constant.INT_INVALID_VALUE;

            _begincreatetime = Constant.STRING_INVALID_VALUE;
            _endcreatetime = Constant.STRING_INVALID_VALUE;
         }
        private string _begincreatetime;
        private string _endcreatetime;
        public string BeginCreateTime
        {
            set { _begincreatetime = value; }
            get { return _begincreatetime; }
        }

        public string EndCreateTime
        {
            set { _endcreatetime = value; }
            get { return _endcreatetime; }
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

        private int _CreaetUserID;

        public int CreaetUserID
        {
            get { return _CreaetUserID; }
            set { _CreaetUserID = value; }
        } 
    }
}
