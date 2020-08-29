using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类EmployeeAgent 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-02 10:01:54 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryEmployeeAgentExclusive
    {
        public QueryEmployeeAgentExclusive()
        {
           
            _userid = Constant.INT_INVALID_VALUE;
            _agentnum = Constant.STRING_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _isexclusive = Constant.STRING_EMPTY_VALUE;
            _loginid = Constant.INT_INVALID_VALUE;
          
        }

        #region Model
        private string _isexclusive;
        private int? _userid;
        private string _agentnum;
        private int? _bgid;
        private int _loginid;

        /// <summary>
        /// 
        /// </summary>
        public string IsExclusive
        {
            set { _isexclusive = value; }
            get { return _isexclusive; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AgentNum
        {
            set { _agentnum = value; }
            get { return _agentnum; }
        }
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }

        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }

     
        public string TrueName { get; set; }
      
        #endregion Model

    }
}

