using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类CRMCustForNextVisit 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-04-17 10:45:54 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class CRMCustForNextVisit
    {
        public CRMCustForNextVisit()
        {
            _crmcustid = Constant.STRING_INVALID_VALUE;
            _userId = Constant.INT_INVALID_VALUE;
            _nextvisitdate = Constant.STRING_INVALID_VALUE;
            _lastupdatetime = Constant.DATE_INVALID_VALUE;
            _lastupdateuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private string _crmcustid;
        private int _userId;
        private string _nextvisitdate;
        private DateTime? _lastupdatetime;
        private int? _lastupdateuserid;
        private DateTime? _createtime;
        private int? _createuserid;
        /// <summary>
        /// 
        /// </summary>
        public string CrmCustID
        {
            set { _crmcustid = value; }
            get { return _crmcustid; }
        }
        /// <summary>
        /// 客服
        /// </summary>
        public int UserID
        {
            set { _userId = value; }
            get { return _userId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NextVisitDate
        {
            set { _nextvisitdate = value; }
            get { return _nextvisitdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastUpdateUserID
        {
            set { _lastupdateuserid = value; }
            get { return _lastupdateuserid; }
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
        #endregion Model

    }
}

