using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CallResult_ORIG_TaskInfo
    /// <summary>
    /// 实体类：CallResult_ORIG_TaskInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-10-30
    /// </summary>
    [DBTableAttribute("CallResult_ORIG_Task")]
    public class CallResult_ORIG_TaskInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CallResult_ORIG_TaskInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CallResult_ORIG_TaskInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CallResult_ORIG_TaskInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _businessid = (!dr.Table.Columns.Contains("BusinessID") || dr["BusinessID"] == null || dr["BusinessID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["BusinessID"]);
            _projectid = (!dr.Table.Columns.Contains("ProjectID") || dr["ProjectID"] == null || dr["ProjectID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ProjectID"]);
            _source = (!dr.Table.Columns.Contains("Source") || dr["Source"] == null || dr["Source"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Source"]);
            _isestablish = (!dr.Table.Columns.Contains("IsEstablish") || dr["IsEstablish"] == null || dr["IsEstablish"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["IsEstablish"]);
            _notestablishreason = (!dr.Table.Columns.Contains("NotEstablishReason") || dr["NotEstablishReason"] == null || dr["NotEstablishReason"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["NotEstablishReason"]);
            _issuccess = (!dr.Table.Columns.Contains("IsSuccess") || dr["IsSuccess"] == null || dr["IsSuccess"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["IsSuccess"]);
            _notsuccessreason = (!dr.Table.Columns.Contains("NotSuccessReason") || dr["NotSuccessReason"] == null || dr["NotSuccessReason"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["NotSuccessReason"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _lastupdateuserid = (!dr.Table.Columns.Contains("LastUpdateUserID") || dr["LastUpdateUserID"] == null || dr["LastUpdateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["LastUpdateUserID"]);
            _lastupdatetime = (!dr.Table.Columns.Contains("LastUpdateTime") || dr["LastUpdateTime"] == null || dr["LastUpdateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastUpdateTime"]);
            #endregion

        }

        #endregion

        #region 属性
        #region 字段 RecID (自动生成)
        private int? _recid = null;
        [DBFieldAttribute("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; isModify_RecID = true; } } }
        public int ValueOrDefault_RecID { get { return _recid.HasValue ? _recid.Value : 0; } }
        private bool isModify_RecID = false;
        [IsModifyAttribute("RecID")]
        public bool IsModify_RecID { get { return isModify_RecID; } }
        #endregion

        #region 字段 BusinessID (自动生成)
        private string _businessid = null;
        [DBFieldAttribute("BusinessID", SqlDbType.VarChar, 50)]
        public string BusinessID { get { return _businessid; } set { if (_businessid != value) { _businessid = value; isModify_BusinessID = true; } } }
        public string ValueOrDefault_BusinessID { get { return _businessid != null ? _businessid : ""; } }
        private bool isModify_BusinessID = false;
        [IsModifyAttribute("BusinessID")]
        public bool IsModify_BusinessID { get { return isModify_BusinessID; } }
        #endregion

        #region 字段 ProjectID (自动生成)
        private long? _projectid = null;
        [DBFieldAttribute("ProjectID", SqlDbType.Int, 4)]
        public long? ProjectID { get { return _projectid; } set { if (_projectid != value) { _projectid = value; isModify_ProjectID = true; } } }
        public long ValueOrDefault_ProjectID { get { return _projectid.HasValue ? _projectid.Value : 0; } }
        private bool isModify_ProjectID = false;
        [IsModifyAttribute("ProjectID")]
        public bool IsModify_ProjectID { get { return isModify_ProjectID; } }
        #endregion

        #region 字段 Source (自动生成)
        private int? _source = null;
        [DBFieldAttribute("Source", SqlDbType.Int, 4)]
        public int? Source { get { return _source; } set { if (_source != value) { _source = value; isModify_Source = true; } } }
        public int ValueOrDefault_Source { get { return _source.HasValue ? _source.Value : 0; } }
        private bool isModify_Source = false;
        [IsModifyAttribute("Source")]
        public bool IsModify_Source { get { return isModify_Source; } }
        #endregion

        #region 字段 IsEstablish (自动生成)
        private int? _isestablish = null;
        [DBFieldAttribute("IsEstablish", SqlDbType.Int, 4)]
        public int? IsEstablish { get { return _isestablish; } set { if (_isestablish != value) { _isestablish = value; isModify_IsEstablish = true; } } }
        public int ValueOrDefault_IsEstablish { get { return _isestablish.HasValue ? _isestablish.Value : 0; } }
        private bool isModify_IsEstablish = false;
        [IsModifyAttribute("IsEstablish")]
        public bool IsModify_IsEstablish { get { return isModify_IsEstablish; } }
        #endregion

        #region 字段 NotEstablishReason (自动生成)
        private int? _notestablishreason = null;
        [DBFieldAttribute("NotEstablishReason", SqlDbType.Int, 4)]
        public int? NotEstablishReason { get { return _notestablishreason; } set { if (_notestablishreason != value) { _notestablishreason = value; isModify_NotEstablishReason = true; } } }
        public int ValueOrDefault_NotEstablishReason { get { return _notestablishreason.HasValue ? _notestablishreason.Value : 0; } }
        private bool isModify_NotEstablishReason = false;
        [IsModifyAttribute("NotEstablishReason")]
        public bool IsModify_NotEstablishReason { get { return isModify_NotEstablishReason; } }
        #endregion

        #region 字段 IsSuccess (自动生成)
        private int? _issuccess = null;
        [DBFieldAttribute("IsSuccess", SqlDbType.Int, 4)]
        public int? IsSuccess { get { return _issuccess; } set { if (_issuccess != value) { _issuccess = value; isModify_IsSuccess = true; } } }
        public int ValueOrDefault_IsSuccess { get { return _issuccess.HasValue ? _issuccess.Value : 0; } }
        private bool isModify_IsSuccess = false;
        [IsModifyAttribute("IsSuccess")]
        public bool IsModify_IsSuccess { get { return isModify_IsSuccess; } }
        #endregion

        #region 字段 NotSuccessReason (自动生成)
        private int? _notsuccessreason = null;
        [DBFieldAttribute("NotSuccessReason", SqlDbType.Int, 4)]
        public int? NotSuccessReason { get { return _notsuccessreason; } set { if (_notsuccessreason != value) { _notsuccessreason = value; isModify_NotSuccessReason = true; } } }
        public int ValueOrDefault_NotSuccessReason { get { return _notsuccessreason.HasValue ? _notsuccessreason.Value : 0; } }
        private bool isModify_NotSuccessReason = false;
        [IsModifyAttribute("NotSuccessReason")]
        public bool IsModify_NotSuccessReason { get { return isModify_NotSuccessReason; } }
        #endregion

        #region 字段 Status (自动生成)
        private int? _status = null;
        [DBFieldAttribute("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; isModify_Status = true; } } }
        public int ValueOrDefault_Status { get { return _status.HasValue ? _status.Value : 0; } }
        private bool isModify_Status = false;
        [IsModifyAttribute("Status")]
        public bool IsModify_Status { get { return isModify_Status; } }
        #endregion

        #region 字段 CreateUserID (自动生成)
        private int? _createuserid = null;
        [DBFieldAttribute("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; isModify_CreateUserID = true; } } }
        public int ValueOrDefault_CreateUserID { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private bool isModify_CreateUserID = false;
        [IsModifyAttribute("CreateUserID")]
        public bool IsModify_CreateUserID { get { return isModify_CreateUserID; } }
        #endregion

        #region 字段 CreateTime (自动生成)
        private DateTime? _createtime = null;
        [DBFieldAttribute("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; isModify_CreateTime = true; } } }
        public DateTime ValueOrDefault_CreateTime { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private bool isModify_CreateTime = false;
        [IsModifyAttribute("CreateTime")]
        public bool IsModify_CreateTime { get { return isModify_CreateTime; } }
        #endregion

        #region 字段 LastUpdateUserID (自动生成)
        private int? _lastupdateuserid = null;
        [DBFieldAttribute("LastUpdateUserID", SqlDbType.Int, 4)]
        public int? LastUpdateUserID { get { return _lastupdateuserid; } set { if (_lastupdateuserid != value) { _lastupdateuserid = value; isModify_LastUpdateUserID = true; } } }
        public int ValueOrDefault_LastUpdateUserID { get { return _lastupdateuserid.HasValue ? _lastupdateuserid.Value : 0; } }
        private bool isModify_LastUpdateUserID = false;
        [IsModifyAttribute("LastUpdateUserID")]
        public bool IsModify_LastUpdateUserID { get { return isModify_LastUpdateUserID; } }
        #endregion

        #region 字段 LastUpdateTime (自动生成)
        private DateTime? _lastupdatetime = null;
        [DBFieldAttribute("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; isModify_LastUpdateTime = true; } } }
        public DateTime ValueOrDefault_LastUpdateTime { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private bool isModify_LastUpdateTime = false;
        [IsModifyAttribute("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get { return isModify_LastUpdateTime; } }
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
            isModify_RecID = b;
            isModify_BusinessID = b;
            isModify_ProjectID = b;
            isModify_Source = b;
            isModify_IsEstablish = b;
            isModify_NotEstablishReason = b;
            isModify_IsSuccess = b;
            isModify_NotSuccessReason = b;
            isModify_Status = b;
            isModify_CreateUserID = b;
            isModify_CreateTime = b;
            isModify_LastUpdateUserID = b;
            isModify_LastUpdateTime = b;
        }

        #endregion

    }
}
