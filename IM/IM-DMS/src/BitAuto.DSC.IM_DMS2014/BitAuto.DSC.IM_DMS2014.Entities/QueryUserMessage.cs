using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类QueryUserMessage 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:04 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class QueryUserMessage
    {
        public QueryUserMessage()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _visitid = Constant.STRING_INVALID_VALUE;
            _typeid = Constant.INT_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _username = Constant.STRING_INVALID_VALUE;
            _email = Constant.STRING_INVALID_VALUE;
            _phone = Constant.STRING_INVALID_VALUE;
            _orderid = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _remarks = Constant.STRING_INVALID_VALUE;
            _remarkstime = Constant.DATE_INVALID_VALUE;
            _remarkuserid = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _lastmodifytime = Constant.DATE_INVALID_VALUE;
            _lastmodifyuserid = Constant.INT_INVALID_VALUE;

            _membername = Constant.STRING_INVALID_VALUE;
            _districtid = Constant.STRING_INVALID_VALUE;
            _citygroupid = Constant.STRING_INVALID_VALUE;
            _querystarttime = Constant.STRING_INVALID_VALUE;
            _queryendtime = Constant.STRING_INVALID_VALUE;
            _agentid = Constant.INT_INVALID_VALUE;
            _lastmodifyusername = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private string _visitid;
        private int _typeid;
        private string _content;
        private string _username;
        private string _email;
        private string _phone;
        private string _orderid;
        private DateTime? _createtime;
        private string _remarks;
        private DateTime? _remarkstime;
        private int? _remarkuserid;
        private int _status;
        private DateTime? _lastmodifytime;
        private int? _lastmodifyuserid;

        private string _membername;
        private string _districtid;
        private string _citygroupid;
        private string _querystarttime;
        private string _queryendtime;
        private int? _agentid;
        private string _lastmodifyusername;

        public string LastModifyUserName
        {
            set { _lastmodifyusername = value; }
            get { return _lastmodifyusername; }
        }
        public int? AgentID
        {
            set { _agentid = value; }
            get { return _agentid; }
        }
        public string QueryStarttime
        {
            set { _querystarttime = value; }
            get { return _querystarttime; }
        }
        public string QueryEndTime
        {
            set { _queryendtime = value; }
            get { return _queryendtime; }
        }
        public string MemberName
        {
            set { _membername = value; }
            get { return _membername; }
        }
        public string DistrictID
        {
            set { _districtid = value; }
            get { return _districtid; }
        }
        public string CityGroupID
        {
            set { _citygroupid = value; }
            get { return _citygroupid; }
        }
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
        public string VisitID
        {
            set { _visitid = value; }
            get { return _visitid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TypeID
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RemarksTime
        {
            set { _remarkstime = value; }
            get { return _remarkstime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RemarkUserID
        {
            set { _remarkuserid = value; }
            get { return _remarkuserid; }
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
        public DateTime? LastModifyTime
        {
            set { _lastmodifytime = value; }
            get { return _lastmodifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastModifyUserID
        {
            set { _lastmodifyuserid = value; }
            get { return _lastmodifyuserid; }
        }
        #endregion Model

    }
}

