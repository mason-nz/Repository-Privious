using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：ClientLogRequireInfo
    /// <summary>
    /// 实体类：ClientLogRequireInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-12-23
    /// </summary>
    [DBTableAttribute("ClientLogRequire")]
    public class ClientLogRequireInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public ClientLogRequireInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public ClientLogRequireInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public ClientLogRequireInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region 属性
        #region 字段 RecID (自动生成)
        [DBField("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        public int RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private int? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region 字段 AgentID (自动生成)
        [DBField("AgentID", SqlDbType.Int, 4)]
        public int? AgentID { get { return _agentid; } set { if (_agentid != value) { _agentid = value; IsModify_AgentID = true; } } }
        public int AgentID_Value { get { return _agentid.HasValue ? _agentid.Value : 0; } }
        private int? _agentid = null;

        [IsModify("AgentID")]
        public bool IsModify_AgentID { get; set; }
        #endregion

        #region 字段 LogDate (自动生成)
        [DBField("LogDate", SqlDbType.DateTime, 8)]
        public DateTime? LogDate { get { return _logdate; } set { if (_logdate != value) { _logdate = value; IsModify_LogDate = true; } } }
        public DateTime LogDate_Value { get { return _logdate.HasValue ? _logdate.Value : new DateTime(); } }
        private DateTime? _logdate = null;

        [IsModify("LogDate")]
        public bool IsModify_LogDate { get; set; }
        #endregion

        #region 字段 Vendor (自动生成)
        [DBField("Vendor", SqlDbType.Int, 4)]
        public int? Vendor { get { return _vendor; } set { if (_vendor != value) { _vendor = value; IsModify_Vendor = true; } } }
        public int Vendor_Value { get { return _vendor.HasValue ? _vendor.Value : 0; } }
        private int? _vendor = null;

        [IsModify("Vendor")]
        public bool IsModify_Vendor { get; set; }
        #endregion

        #region 字段 RequireID (自动生成)
        [DBField("RequireID", SqlDbType.Int, 4)]
        public int? RequireID { get { return _requireid; } set { if (_requireid != value) { _requireid = value; IsModify_RequireID = true; } } }
        public int RequireID_Value { get { return _requireid.HasValue ? _requireid.Value : 0; } }
        private int? _requireid = null;

        [IsModify("RequireID")]
        public bool IsModify_RequireID { get; set; }
        #endregion

        #region 字段 RequireDateTime (自动生成)
        [DBField("RequireDateTime", SqlDbType.DateTime, 8)]
        public DateTime? RequireDateTime { get { return _requiredatetime; } set { if (_requiredatetime != value) { _requiredatetime = value; IsModify_RequireDateTime = true; } } }
        public DateTime RequireDateTime_Value { get { return _requiredatetime.HasValue ? _requiredatetime.Value : new DateTime(); } }
        private DateTime? _requiredatetime = null;

        [IsModify("RequireDateTime")]
        public bool IsModify_RequireDateTime { get; set; }
        #endregion

        #region 字段 Status (自动生成)
        [DBField("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; IsModify_Status = true; } } }
        public int Status_Value { get { return _status.HasValue ? _status.Value : 0; } }
        private int? _status = null;

        [IsModify("Status")]
        public bool IsModify_Status { get; set; }
        #endregion

        #region 字段 ResponseDateTime (自动生成)
        [DBField("ResponseDateTime", SqlDbType.DateTime, 8)]
        public DateTime? ResponseDateTime { get { return _responsedatetime; } set { if (_responsedatetime != value) { _responsedatetime = value; IsModify_ResponseDateTime = true; } } }
        public DateTime ResponseDateTime_Value { get { return _responsedatetime.HasValue ? _responsedatetime.Value : new DateTime(); } }
        private DateTime? _responsedatetime = null;

        [IsModify("ResponseDateTime")]
        public bool IsModify_ResponseDateTime { get; set; }
        #endregion

        #region 字段 ResponseSuccess (自动生成)
        [DBField("ResponseSuccess", SqlDbType.Int, 4)]
        public int? ResponseSuccess { get { return _responsesuccess; } set { if (_responsesuccess != value) { _responsesuccess = value; IsModify_ResponseSuccess = true; } } }
        public int ResponseSuccess_Value { get { return _responsesuccess.HasValue ? _responsesuccess.Value : 0; } }
        private int? _responsesuccess = null;

        [IsModify("ResponseSuccess")]
        public bool IsModify_ResponseSuccess { get; set; }
        #endregion

        #region 字段 ResponseRemark (自动生成)
        [DBField("ResponseRemark", SqlDbType.VarChar, 500)]
        public string ResponseRemark { get { return _responseremark; } set { if (_responseremark != value) { _responseremark = value; IsModify_ResponseRemark = true; } } }
        public string ResponseRemark_Value { get { return _responseremark != null ? _responseremark : ""; } }
        private string _responseremark = null;

        [IsModify("ResponseRemark")]
        public bool IsModify_ResponseRemark { get; set; }
        #endregion

        #region 字段 FilePath (自动生成)
        [DBField("FilePath", SqlDbType.VarChar, 500)]
        public string FilePath { get { return _filepath; } set { if (_filepath != value) { _filepath = value; IsModify_FilePath = true; } } }
        public string FilePath_Value { get { return _filepath != null ? _filepath : ""; } }
        private string _filepath = null;

        [IsModify("FilePath")]
        public bool IsModify_FilePath { get; set; }
        #endregion

        #region 字段 CreateTime (自动生成)
        [DBField("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; IsModify_CreateTime = true; } } }
        public DateTime CreateTime_Value { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private DateTime? _createtime = null;

        [IsModify("CreateTime")]
        public bool IsModify_CreateTime { get; set; }
        #endregion

        #region 字段 CreateUserID (自动生成)
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion

        #region 字段 LastUpdateTime (自动生成)
        [DBField("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; IsModify_LastUpdateTime = true; } } }
        public DateTime LastUpdateTime_Value { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private DateTime? _lastupdatetime = null;

        [IsModify("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get; set; }
        #endregion

        #region 字段 LastUpdateUserID (自动生成)
        [DBField("LastUpdateUserID", SqlDbType.Int, 4)]
        public int? LastUpdateUserID { get { return _lastupdateuserid; } set { if (_lastupdateuserid != value) { _lastupdateuserid = value; IsModify_LastUpdateUserID = true; } } }
        public int LastUpdateUserID_Value { get { return _lastupdateuserid.HasValue ? _lastupdateuserid.Value : 0; } }
        private int? _lastupdateuserid = null;

        [IsModify("LastUpdateUserID")]
        public bool IsModify_LastUpdateUserID { get; set; }
        #endregion

        #endregion

        #region 方法
        /// 设置是否更新所有字段 (自动生成)
        /// <summary>
        /// 设置是否更新所有字段 (自动生成)
        /// </summary>
        /// <param name="b"></param>
        public void SetModify(bool b)
        {
            IsModify_RecID = b;
            IsModify_AgentID = b;
            IsModify_LogDate = b;
            IsModify_Vendor = b;
            IsModify_RequireID = b;
            IsModify_RequireDateTime = b;
            IsModify_Status = b;
            IsModify_ResponseDateTime = b;
            IsModify_ResponseSuccess = b;
            IsModify_ResponseRemark = b;
            IsModify_FilePath = b;
            IsModify_CreateTime = b;
            IsModify_CreateUserID = b;
            IsModify_LastUpdateTime = b;
            IsModify_LastUpdateUserID = b;
        }

        #endregion

    }
}
