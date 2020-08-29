using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class ProjectTask_AuditContrastInfo
    {
        public ProjectTask_AuditContrastInfo()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _ptid = Constant.STRING_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _dmsmemberid = Constant.STRING_INVALID_VALUE;
            _contrastinfoinside = Constant.STRING_INVALID_VALUE;
            _contrastinfo = Constant.STRING_INVALID_VALUE;
            _exportstatus = Constant.INT_INVALID_VALUE;
            _disposestatus = Constant.INT_INVALID_VALUE;
            _contrasttype = Constant.INT_INVALID_VALUE;
            _remark = Constant.STRING_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _disposetime = null;

        }
        #region Model
        private int _recid;
        private string _ptid;
        private string _custid;
        private string _dmsmemberid;
        private string _contrastinfoinside;
        private string _contrastinfo;
        private int? _exportstatus;
        private int? _disposestatus;
        private int? _contrasttype;
        private string _remark;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _disposetime;
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
        public string PTID
        {
            set { _ptid = value; }
            get { return _ptid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DMSMemberID
        {
            set { _dmsmemberid = value; }
            get { return _dmsmemberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContrastInfoInside
        {
            set { _contrastinfoinside = value; }
            get { return _contrastinfoinside; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContrastInfo
        {
            set { _contrastinfo = value; }
            get { return _contrastinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ExportStatus
        {
            set { _exportstatus = value; }
            get { return _exportstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DisposeStatus
        {
            set { _disposestatus = value; }
            get { return _disposestatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContrastType
        {
            set { _contrasttype = value; }
            get { return _contrasttype; }
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
        /// 
        /// </summary>
        public DateTime? DisposeTime
        {
            set { _disposetime = value; }
            get { return _disposetime; }
        }
        #endregion Model
    }
}
