using System;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类AgentStatusDetail 。(属性说明自动提取数据库字段的描述信息)
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
    public class AgentStatusDetail
    {
        public AgentStatusDetail()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _userid = Constant.STRING_INVALID_VALUE;
            _status = (int)AgentStatus.Leaveline;
            _starttime = Constant.DATE_INVALID_VALUE;
            _endtime = Constant.DATE_INVALID_VALUE;
            _timelong = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private long _recid;
        private string _userid;
        private int _status;
        private DateTime _starttime;
        private DateTime? _endtime;
        private int _timelong;
        /// <summary>
        /// 
        /// </summary>
        public long RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        #endregion Model

    }
}

