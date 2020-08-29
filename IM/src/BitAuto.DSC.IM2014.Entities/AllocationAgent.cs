using System;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类AllocationAgent 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-03-05 10:05:58 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class AllocationAgent
    {
        public AllocationAgent()
        {
            _allocid = Constant.INT_INVALID_VALUE;
            _agentid = Constant.STRING_INVALID_VALUE;
            _userid = Constant.STRING_INVALID_VALUE;
            _queuestarttime = Constant.DATE_INVALID_VALUE;
            _starttime = Constant.DATE_INVALID_VALUE;
            _agentendtime = Constant.DATE_INVALID_VALUE;
            _userendtime = Constant.DATE_INVALID_VALUE;
            _userreferurl = Constant.STRING_INVALID_VALUE;
            _username = Constant.STRING_INVALID_VALUE;

        }
        #region Model
        private long _allocid;
        private string _agentid;
        private string _userid;
        private DateTime? _queuestarttime;
        private DateTime? _starttime;
        private DateTime? _agentendtime;
        private DateTime? _userendtime;
        private string _userreferurl;
        private string _localip;
        private string _location;
        private string _username;
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        private string _locationid;
        public string LocationID
        {
            set { _locationid = value; }
            get { return _locationid; }
        }
        public string Location
        {
            set { _location = value; }
            get { return _location; }
        }
        public string LocalIP
        {
            set { _localip = value; }
            get { return _localip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long AllocID
        {
            set { _allocid = value; }
            get { return _allocid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AgentID
        {
            set { _agentid = value; }
            get { return _agentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        public DateTime? QueueStartTime
        {
            set { _queuestarttime = value; }
            get { return _queuestarttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AgentEndTime
        {
            set { _agentendtime = value; }
            get { return _agentendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UserEndTime
        {
            set { _userendtime = value; }
            get { return _userendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserReferURL
        {
            set { _userreferurl = value; }
            get { return _userreferurl; }
        }
        #endregion Model

    }
}

