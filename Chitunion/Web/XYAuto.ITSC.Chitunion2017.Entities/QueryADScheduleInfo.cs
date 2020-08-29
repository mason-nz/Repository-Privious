using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QueryADScheduleInfo
    {
        public QueryADScheduleInfo()
        {
            _ADSID = Constant.INT_INVALID_VALUE;
            _ADDetailID = Constant.INT_INVALID_VALUE;
            _OrderID = Constant.STRING_INVALID_VALUE;
            _SubOrderID = Constant.STRING_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            _PubID = Constant.INT_INVALID_VALUE;
            _BeginData = Constant.DATE_INVALID_VALUE;
            _EndData = Constant.DATE_INVALID_VALUE;

            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;            
        }

        private int _ADDetailID;

        public int ADDetailID
        {
            get { return _ADDetailID; }
            set { _ADDetailID = value; }
        }
        

        private DateTime _EndData;

        public DateTime EndData
        {
            get { return _EndData; }
            set { _EndData = value; }
        }
                

        private DateTime _BeginData;

        public DateTime BeginData
        {
            get { return _BeginData; }
            set { _BeginData = value; }
        }
        

        private int _ADSID;

        public int ADSID
        {
            get { return _ADSID; }
            set { _ADSID = value; }
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
