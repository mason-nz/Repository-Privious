using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类ProjectTask_ReturnVisit 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-07 03:04:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class ProjectTask_ReturnVisit
    {
        public ProjectTask_ReturnVisit()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _crmcustid = Constant.STRING_INVALID_VALUE;
            _title = Constant.STRING_INVALID_VALUE;
            _rvtype = Constant.INT_INVALID_VALUE;
            _contactinfouserid = Constant.INT_INVALID_VALUE;
            _remark = Constant.STRING_INVALID_VALUE;
            _contactinfotitle = Constant.STRING_INVALID_VALUE;
            _begintime = Constant.STRING_INVALID_VALUE;
            _endtime = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createuserdepart = Constant.STRING_INVALID_VALUE;
            _userclass = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _lastupdatetime = Constant.DATE_INVALID_VALUE;
            _rvresult = Constant.STRING_INVALID_VALUE;
            _rvquestionstatus = Constant.INT_INVALID_VALUE;
            _rvquestionremark = Constant.STRING_INVALID_VALUE;
            _memberid = Constant.STRING_INVALID_VALUE;
            _visittype = Constant.STRING_INVALID_VALUE;
            _typeid = Constant.STRING_INVALID_VALUE;
        }

        private string _typeid;
        public string TypeID
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        #region Model
        private long _recid;
        private string _crmcustid;
        private string _title;
        private int? _rvtype;
        private int? _contactinfouserid;
        private string _remark;
        private string _contactinfotitle;
        private string _begintime;
        private string _endtime;
        private DateTime? _createtime;
        private int? _createuserid;
        private string _createuserdepart;
        private int? _userclass;
        private int? _status;
        private DateTime? _lastupdatetime;
        private string _rvresult;
        private int? _rvquestionstatus;
        private string _rvquestionremark;
        private string _memberid;
        private string _visittype;
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
        public string CRMCustID
        {
            set { _crmcustid = value; }
            get { return _crmcustid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RVType
        {
            set { _rvtype = value; }
            get { return _rvtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactInfoUserID
        {
            set { _contactinfouserid = value; }
            get { return _contactinfouserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactInfotitle
        {
            set { _contactinfotitle = value; }
            get { return _contactinfotitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Begintime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Endtime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createtime
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
        /// 
        /// </summary>
        public string CreateuserDepart
        {
            set { _createuserdepart = value; }
            get { return _createuserdepart; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserClass
        {
            set { _userclass = value; }
            get { return _userclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
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
        public string RVresult
        {
            set { _rvresult = value; }
            get { return _rvresult; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RVQuestionStatus
        {
            set { _rvquestionstatus = value; }
            get { return _rvquestionstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RVQuestionRemark
        {
            set { _rvquestionremark = value; }
            get { return _rvquestionremark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MemberId
        {
            set { _memberid = value; }
            get { return _memberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VisitType
        {
            set { _visittype = value; }
            get { return _visittype; }
        }
        #endregion Model

    }
}

