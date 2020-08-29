using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryAgentState
    {
        public QueryAgentState() { 
         _AgentName = Constants.Constant.STRING_EMPTY_VALUE;
         _UserID = Constants.Constant.INT_INVALID_VALUE;
         _State = Constants.Constant.INT_INVALID_VALUE;
         _AgentAuxState = Constants.Constant.INT_INVALID_VALUE;
         _StartTime = Constants.Constant.DATE_INVALID_VALUE;
         _AGTime = Constants.Constant.INT_INVALID_VALUE;
         _ExtensionNum = Constants.Constant.STRING_EMPTY_VALUE;
         _AgentNum = Constants.Constant.STRING_EMPTY_VALUE;
         _GroupName = Constants.Constant.STRING_EMPTY_VALUE;
         _BGID = Constants.Constant.INT_INVALID_VALUE;
         _loginid = Constants.Constant.INT_INVALID_VALUE;
        }

        #region Model
        private string _AgentName;
        private int? _UserID;
        private int? _State;
        private int? _AgentAuxState;
        private DateTime? _StartTime;
        private int? _AGTime;
        private string _ExtensionNum;
        private string _AgentNum;
        private string _GroupName;
        private int? _BGID;
        private int _loginid;
        /// <summary>
        /// 
        /// </summary>
        public string AgentName
        {
            set { _AgentName = value; }
            get { return _AgentName; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int? UserID
        {
            set { _UserID = value; }
            get { return _UserID; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? State
        {
            set { _State = value; }
            get { return _State; }
        }        
        /// <summary>
        /// 
        /// </summary>
        public int? AgentAuxState
        {
            set { _AgentAuxState = value; }
            get { return _AgentAuxState; }
        }        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _StartTime = value; }
            get { return _StartTime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AGTime
        {
            set { _AGTime = value; }
            get { return _AGTime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExtensionNum
        {
            set { _ExtensionNum = value; }
            get { return _ExtensionNum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AgentNum
        {
            set { _AgentNum = value; }
            get { return _AgentNum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GroupName
        {
            set { _GroupName = value; }
            get { return _GroupName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BGID
        {
            set { _BGID = value; }
            get { return _BGID; }
        }

        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }
        #endregion Model
    }
}
