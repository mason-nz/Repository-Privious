using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
   
    [Serializable]
    public class QueryAgentStateDetail
    {
        public QueryAgentStateDetail()
        {
            _loginID = Constants.Constant.STRING_EMPTY_VALUE;
            _AgentId = Constants.Constant.STRING_EMPTY_VALUE;
            //_UserID = Constants.Constant.STRING_EMPTY_VALUE;
            _State = Constants.Constant.STRING_EMPTY_VALUE;
            _AgentAuxState = Constants.Constant.STRING_EMPTY_VALUE;
            _StartTime = Constants.Constant.STRING_EMPTY_VALUE;
            //_AGTime = Constants.Constant.INT_INVALID_VALUE;
            //_ExtensionNum = Constants.Constant.STRING_EMPTY_VALUE;
            _AgentNum = Constants.Constant.STRING_EMPTY_VALUE;
            //_GroupName = Constants.Constant.STRING_EMPTY_VALUE;
            //_BGID = Constants.Constant.INT_INVALID_VALUE;
            //_loginid = Constants.Constant.INT_INVALID_VALUE;
        }

        #region Model
        private string _loginID;
        private string _AgentId;
        //private  string _UserID;
        private  string _State;
        private  string _AgentAuxState;
        private string _StartTime;
        //private  string  _AGTime;
        //private string _ExtensionNum;
        private string _AgentNum;
        //private string _GroupName;
        //private  string  _BGID;
        //private  string _loginid;
        /// <summary>
        /// 
        /// </summary>
        public string AgentID
        {
            set { _AgentId = value; }
            get { return _AgentId; }
        }

        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }

        }

        /// <summary>
        /// 
        /// </summary>
        //public  string  UserID
        //{
        //    set { _UserID = value; }
        //    get { return _UserID; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public  string  State
        {
            set { _State = value; }
            get { return _State; }
        }
        /// <summary>
        /// 
        /// </summary>
        public  string  AgentAuxState
        {
            set { _AgentAuxState = value; }
            get { return _AgentAuxState; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StartTime
        {
            set { _StartTime = value; }
            get { return _StartTime; }
        }
        
        public string AgentNum
        {
            set { _AgentNum = value; }
            get { return _AgentNum; }
        }
       
        #endregion Model

        //public string _AgentId { get; set; }
    }
}
