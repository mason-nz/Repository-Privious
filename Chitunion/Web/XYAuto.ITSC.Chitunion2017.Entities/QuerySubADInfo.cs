using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QuerySubADInfo
    {
        public QuerySubADInfo()
        {
            _OrderID = Constant.STRING_INVALID_VALUE;
            _subOrderID = Constant.STRING_INVALID_VALUE;            
            _MediaType = Constant.INT_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            _Status = Constant.INT_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;        

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

        
        private int _Status;

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
       
        private int _CreateUserID;

        public int CreateUserID
        {
            get { return _CreateUserID; }
            set { _CreateUserID = value; }
        }
        
    }
}
