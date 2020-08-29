using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类CallEventActionLog 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-01-28 05:23:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class CallEventActionLog
    {
        public CallEventActionLog()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _eventname = Constant.STRING_INVALID_VALUE;
            _sessionid = Constant.STRING_INVALID_VALUE;
            _loginfo = Constant.STRING_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private string _eventname;
        private string _sessionid;
        private string _loginfo;
        private int _userid;
        private DateTime? _createtime;
        /// <summary>
        /// 
        /// </summary>
        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EventName
        {
            set { _eventname = value; }
            get { return _eventname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SessionID
        {
            set { _sessionid = value; }
            get { return _sessionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Loginfo
        {
            set { _loginfo = value; }
            get { return _loginfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        #endregion Model

    }
}

