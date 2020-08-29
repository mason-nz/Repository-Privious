using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class PublishAuditInfo
    {
        public PublishAuditInfo()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            _PublishID = Constant.INT_INVALID_VALUE;
            _OptType = Constant.INT_INVALID_VALUE;
            _PubStatus = Constant.INT_INVALID_VALUE;
            _RejectMsg = Constant.STRING_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;
        }

        private int _RecID;

        public int RecID
        {
            get { return _RecID; }
            set { _RecID = value; }
        }

        private int _MediaType;

        public int MediaType
        {
            get { return _MediaType; }
            set { _MediaType = value; }
        }

        private int _PublishID;

        public int PublishID
        {
            get { return _PublishID; }
            set { _PublishID = value; }
        }

        private int _OptType;

        public int OptType
        {
            get { return _OptType; }
            set { _OptType = value; }
        }

        private int _PubStatus;

        public int PubStatus
        {
            get { return _PubStatus; }
            set { _PubStatus = value; }
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

        public int MediaID { get; set; }
    }
}