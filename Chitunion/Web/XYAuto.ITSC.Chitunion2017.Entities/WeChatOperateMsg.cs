using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class WeChatOperateMsg
    {
        public WeChatOperateMsg()
        {
            _RecID = Constant.INT_INVALID_VALUE;
            _Status = Constant.INT_INVALID_VALUE;
            _MediaType = Constant.INT_INVALID_VALUE;
            _MediaID = Constant.INT_INVALID_VALUE;
            _MediaName = Constant.STRING_EMPTY_VALUE;
            _PublishID = Constant.INT_INVALID_VALUE;
            _PublishName = Constant.STRING_EMPTY_VALUE;
            _SubmitUserName = Constant.STRING_EMPTY_VALUE;
            _SubmitUserID = Constant.INT_INVALID_VALUE;
            _OptType = Constant.INT_INVALID_VALUE;
            _MsgType = Constant.INT_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
            _CreateUserID = Constant.INT_INVALID_VALUE;            
        }
        public int BaseMediaID { get; set; } = -2;
        public string PublishValidDate { get; set; } = string.Empty;
        public int ADTemID { get; set; } = -2;
        public string ADTemName { get; set; } = string.Empty;
        private int _MsgType;

        public int MsgType
        {
            get { return _MsgType; }
            set { _MsgType = value; }
        }

        private int _SubmitUserID;

        public int SubmitUserID
        {
            get { return _SubmitUserID; }
            set { _SubmitUserID = value; }
        }

        private string _SubmitUserName;

        public string SubmitUserName
        {
            get { return _SubmitUserName; }
            set { _SubmitUserName = value; }
        }

        private int _OptType;

        public int OptType
        {
            get { return _OptType; }
            set { _OptType = value; }
        }

        private string _PublishName;

        public string PublishName
        {
            get { return _PublishName; }
            set { _PublishName = value; }
        }

        private int _PublishID;

        public int PublishID
        {
            get { return _PublishID; }
            set { _PublishID = value; }
        }

        private string _MediaName;

        public string MediaName
        {
            get { return _MediaName; }
            set { _MediaName = value; }
        }

        private int _MediaID;

        public int MediaID
        {
            get { return _MediaID; }
            set { _MediaID = value; }
        }

        private int _MediaType;

        public int MediaType
        {
            get { return _MediaType; }
            set { _MediaType = value; }
        }

        private int _RecID;

        public int RecID
        {
            get { return _RecID; }
            set { _RecID = value; }
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
    }
}
