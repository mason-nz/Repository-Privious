using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QueryADDetailInfo
    {
        public QueryADDetailInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _SubOrderID = Constant.STRING_INVALID_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            _PubID = Constant.INT_INVALID_VALUE;                        
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
    }
}
