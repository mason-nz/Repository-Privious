using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类UpdateOrderData 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class UpdateOrderData
    {
        public UpdateOrderData()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;
            _yporderid = Constant.INT_INVALID_VALUE;
            _updatetype = Constant.INT_INVALID_VALUE;
            _updateerrormsg = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _isupdate = Constant.INT_INVALID_VALUE;
            _updatedatetime = Constant.DATE_INVALID_VALUE;
            _consulttype = Constant.INT_INVALID_VALUE;
            _consultrecid = Constant.INT_INVALID_VALUE;
            _apitype = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private string _taskid;
        private int? _yporderid;
        private int? _updatetype;
        private string _updateerrormsg;
        private DateTime? _createtime;
        private int? _createuserid;
        private int? _isupdate;
        private DateTime? _updatedatetime;

        private int? _consulttype;
        private int? _consultrecid;
        private int? _apitype;
        private string _custid;

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
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? YPOrderID
        {
            set { _yporderid = value; }
            get { return _yporderid; }
        }
        /// <summary>
        /// 调用接口处理是否成功(1 成功 -1 失败)
        /// </summary>
        public int? UpdateType
        {
            set { _updatetype = value; }
            get { return _updatetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UpdateErrorMsg
        {
            set { _updateerrormsg = value; }
            get { return _updateerrormsg; }
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
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 是否已经处理(1 已处理,-1 未处理)
        /// </summary>
        public int? IsUpdate
        {
            set { _isupdate = value; }
            get { return _isupdate; }
        }
        public DateTime? UpdateDateTime
        {
            set { _updatedatetime = value; }
            get { return _updatedatetime; }
        }

        public int? ConsultType
        {
            set { _consulttype = value; }
            get { return _consulttype; }
        }

        public int? ConsultRecID
        {
            set { _consultrecid = value; }
            get { return _consultrecid; }
        }

        public int? APIType
        {
            set { _apitype = value; }
            get { return _apitype; }
        }

        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        #endregion Model

    }
}

