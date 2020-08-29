using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QueryCartScheduleInfo
    {
        public QueryCartScheduleInfo()
        {
            _CartID = Constants.Constant.INT_INVALID_VALUE;
            _BeginTime = Constants.Constant.DATE_INVALID_VALUE;
            _CreateTime = Constants.Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constants.Constant.INT_INVALID_VALUE;
        }

        private int _RecID;

        public int RecID
        {
            get { return _RecID; }
            set { _RecID = value; }
        }
        private int _CartID;

        public int CartID
        {
            get { return _CartID; }
            set { _CartID = value; }
        }

        private DateTime _BeginTime;

        public DateTime BeginTime
        {
            get { return _BeginTime; }
            set { _BeginTime = value; }
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
