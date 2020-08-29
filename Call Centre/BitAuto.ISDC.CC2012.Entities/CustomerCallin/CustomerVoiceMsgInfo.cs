using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CustomerVoiceMsgInfo
    /// <summary>
    /// 实体类：CustomerVoiceMsgInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-07-29
    /// </summary>
    [DBTableAttribute("CustomerVoiceMsg")]
    public class CustomerVoiceMsgInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CustomerVoiceMsgInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CustomerVoiceMsgInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CustomerVoiceMsgInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _vender = (!dr.Table.Columns.Contains("Vender") || dr["Vender"] == null || dr["Vender"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Vender"]);
            _callno = (!dr.Table.Columns.Contains("CallNO") || dr["CallNO"] == null || dr["CallNO"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CallNO"]);
            _calledno = (!dr.Table.Columns.Contains("CalledNo") || dr["CalledNo"] == null || dr["CalledNo"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CalledNo"]);
            _starttime = (!dr.Table.Columns.Contains("StartTime") || dr["StartTime"] == null || dr["StartTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["StartTime"]);
            _endtime = (!dr.Table.Columns.Contains("EndTime") || dr["EndTime"] == null || dr["EndTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["EndTime"]);
            _filefullname = (!dr.Table.Columns.Contains("FileFullName") || dr["FileFullName"] == null || dr["FileFullName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["FileFullName"]);
            _sgid = (!dr.Table.Columns.Contains("SGID") || dr["SGID"] == null || dr["SGID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["SGID"]);
            _sessionid = (!dr.Table.Columns.Contains("SessionID") || dr["SessionID"] == null || dr["SessionID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["SessionID"]);
            _sourcetype = (!dr.Table.Columns.Contains("SourceType") || dr["SourceType"] == null || dr["SourceType"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["SourceType"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _processuserid = (!dr.Table.Columns.Contains("ProcessUserID") || dr["ProcessUserID"] == null || dr["ProcessUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ProcessUserID"]);
            _processtime = (!dr.Table.Columns.Contains("ProcessTime") || dr["ProcessTime"] == null || dr["ProcessTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["ProcessTime"]);
            _iscallback = (!dr.Table.Columns.Contains("IsCallBack") || dr["IsCallBack"] == null || dr["IsCallBack"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["IsCallBack"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _lastupdatetime = (!dr.Table.Columns.Contains("LastUpdateTime") || dr["LastUpdateTime"] == null || dr["LastUpdateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastUpdateTime"]);
            _lastupdateuserid = (!dr.Table.Columns.Contains("LastUpdateUserID") || dr["LastUpdateUserID"] == null || dr["LastUpdateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["LastUpdateUserID"]);
            _orderid = (!dr.Table.Columns.Contains("OrderID") || dr["OrderID"] == null || dr["OrderID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["OrderID"]);
            _custid = (!dr.Table.Columns.Contains("CustID") || dr["CustID"] == null || dr["CustID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CustID"]);
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

        #region 字段 Vender (自动生成)
        private int? _vender = null;
        [DBFieldAttribute("Vender", SqlDbType.Int, 4)]
        public int? Vender { get { return _vender; } set { if (_vender != value) { _vender = value; isModify_Vender = true; } } }
        public int ValueOrDefault_Vender { get { return _vender.HasValue ? _vender.Value : 0; } }
        private bool isModify_Vender = false;
        [IsModifyAttribute("Vender")]
        public bool IsModify_Vender { get { return isModify_Vender; } }
        #endregion

        #region 字段 CallNO (自动生成)
        private string _callno = null;
        [DBFieldAttribute("CallNO", SqlDbType.VarChar, 50)]
        public string CallNO { get { return _callno; } set { if (_callno != value) { _callno = value; isModify_CallNO = true; } } }
        public string ValueOrDefault_CallNO { get { return _callno != null ? _callno : ""; } }
        private bool isModify_CallNO = false;
        [IsModifyAttribute("CallNO")]
        public bool IsModify_CallNO { get { return isModify_CallNO; } }
        #endregion

        #region 字段 CalledNo (自动生成)
        private string _calledno = null;
        [DBFieldAttribute("CalledNo", SqlDbType.VarChar, 50)]
        public string CalledNo { get { return _calledno; } set { if (_calledno != value) { _calledno = value; isModify_CalledNo = true; } } }
        public string ValueOrDefault_CalledNo { get { return _calledno != null ? _calledno : ""; } }
        private bool isModify_CalledNo = false;
        [IsModifyAttribute("CalledNo")]
        public bool IsModify_CalledNo { get { return isModify_CalledNo; } }
        #endregion

        #region 字段 StartTime (自动生成)
        private DateTime? _starttime = null;
        [DBFieldAttribute("StartTime", SqlDbType.DateTime, 8)]
        public DateTime? StartTime { get { return _starttime; } set { if (_starttime != value) { _starttime = value; isModify_StartTime = true; } } }
        public DateTime ValueOrDefault_StartTime { get { return _starttime.HasValue ? _starttime.Value : new DateTime(); } }
        private bool isModify_StartTime = false;
        [IsModifyAttribute("StartTime")]
        public bool IsModify_StartTime { get { return isModify_StartTime; } }
        #endregion

        #region 字段 EndTime (自动生成)
        private DateTime? _endtime = null;
        [DBFieldAttribute("EndTime", SqlDbType.DateTime, 8)]
        public DateTime? EndTime { get { return _endtime; } set { if (_endtime != value) { _endtime = value; isModify_EndTime = true; } } }
        public DateTime ValueOrDefault_EndTime { get { return _endtime.HasValue ? _endtime.Value : new DateTime(); } }
        private bool isModify_EndTime = false;
        [IsModifyAttribute("EndTime")]
        public bool IsModify_EndTime { get { return isModify_EndTime; } }
        #endregion

        #region 字段 FileFullName (自动生成)
        private string _filefullname = null;
        [DBFieldAttribute("FileFullName", SqlDbType.VarChar, 500)]
        public string FileFullName { get { return _filefullname; } set { if (_filefullname != value) { _filefullname = value; isModify_FileFullName = true; } } }
        public string ValueOrDefault_FileFullName { get { return _filefullname != null ? _filefullname : ""; } }
        private bool isModify_FileFullName = false;
        [IsModifyAttribute("FileFullName")]
        public bool IsModify_FileFullName { get { return isModify_FileFullName; } }
        #endregion

        #region 字段 SGID (自动生成)
        private int? _sgid = null;
        [DBFieldAttribute("SGID", SqlDbType.Int, 4)]
        public int? SGID { get { return _sgid; } set { if (_sgid != value) { _sgid = value; isModify_SGID = true; } } }
        public int ValueOrDefault_SGID { get { return _sgid.HasValue ? _sgid.Value : 0; } }
        private bool isModify_SGID = false;
        [IsModifyAttribute("SGID")]
        public bool IsModify_SGID { get { return isModify_SGID; } }
        #endregion

        #region 字段 SessionID (自动生成)
        private string _sessionid = null;
        [DBFieldAttribute("SessionID", SqlDbType.VarChar, 50)]
        public string SessionID { get { return _sessionid; } set { if (_sessionid != value) { _sessionid = value; isModify_SessionID = true; } } }
        public string ValueOrDefault_SessionID { get { return _sessionid != null ? _sessionid : ""; } }
        private bool isModify_SessionID = false;
        [IsModifyAttribute("SessionID")]
        public bool IsModify_SessionID { get { return isModify_SessionID; } }
        #endregion

        #region 字段 SourceType (自动生成)
        private int? _sourcetype = null;
        [DBFieldAttribute("SourceType", SqlDbType.Int, 4)]
        public int? SourceType { get { return _sourcetype; } set { if (_sourcetype != value) { _sourcetype = value; isModify_SourceType = true; } } }
        public int ValueOrDefault_SourceType { get { return _sourcetype.HasValue ? _sourcetype.Value : 0; } }
        private bool isModify_SourceType = false;
        [IsModifyAttribute("SourceType")]
        public bool IsModify_SourceType { get { return isModify_SourceType; } }
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

        #region 字段 ProcessUserID (自动生成)
        private int? _processuserid = null;
        [DBFieldAttribute("ProcessUserID", SqlDbType.Int, 4)]
        public int? ProcessUserID { get { return _processuserid; } set { if (_processuserid != value) { _processuserid = value; isModify_ProcessUserID = true; } } }
        public int ValueOrDefault_ProcessUserID { get { return _processuserid.HasValue ? _processuserid.Value : 0; } }
        private bool isModify_ProcessUserID = false;
        [IsModifyAttribute("ProcessUserID")]
        public bool IsModify_ProcessUserID { get { return isModify_ProcessUserID; } }
        #endregion

        #region 字段 ProcessTime (自动生成)
        private DateTime? _processtime = null;
        [DBFieldAttribute("ProcessTime", SqlDbType.DateTime, 8)]
        public DateTime? ProcessTime { get { return _processtime; } set { if (_processtime != value) { _processtime = value; isModify_ProcessTime = true; } } }
        public DateTime ValueOrDefault_ProcessTime { get { return _processtime.HasValue ? _processtime.Value : new DateTime(); } }
        private bool isModify_ProcessTime = false;
        [IsModifyAttribute("ProcessTime")]
        public bool IsModify_ProcessTime { get { return isModify_ProcessTime; } }
        #endregion

        #region 字段 IsCallBack (自动生成)
        private int? _iscallback = null;
        [DBFieldAttribute("IsCallBack", SqlDbType.Int, 4)]
        public int? IsCallBack { get { return _iscallback; } set { if (_iscallback != value) { _iscallback = value; isModify_IsCallBack = true; } } }
        public int ValueOrDefault_IsCallBack { get { return _iscallback.HasValue ? _iscallback.Value : 0; } }
        private bool isModify_IsCallBack = false;
        [IsModifyAttribute("IsCallBack")]
        public bool IsModify_IsCallBack { get { return isModify_IsCallBack; } }
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

        #region 字段 LastUpdateTime (自动生成)
        private DateTime? _lastupdatetime = null;
        [DBFieldAttribute("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; isModify_LastUpdateTime = true; } } }
        public DateTime ValueOrDefault_LastUpdateTime { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private bool isModify_LastUpdateTime = false;
        [IsModifyAttribute("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get { return isModify_LastUpdateTime; } }
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

        #region 字段 OrderID (自动生成)
        private string _orderid = null;
        [DBFieldAttribute("OrderID", SqlDbType.VarChar, 50)]
        public string OrderID { get { return _orderid; } set { if (_orderid != value) { _orderid = value; isModify_OrderID = true; } } }
        public string ValueOrDefault_OrderID { get { return _orderid != null ? _orderid : ""; } }
        private bool isModify_OrderID = false;
        [IsModifyAttribute("OrderID")]
        public bool IsModify_OrderID { get { return isModify_OrderID; } }
        #endregion

        #region 字段 CustID (自动生成)
        private string _custid = null;
        [DBFieldAttribute("CustID", SqlDbType.VarChar, 20)]
        public string CustID { get { return _custid; } set { if (_custid != value) { _custid = value; isModify_CustID = true; } } }
        public string ValueOrDefault_CustID { get { return _custid != null ? _custid : ""; } }
        private bool isModify_CustID = false;
        [IsModifyAttribute("CustID")]
        public bool IsModify_CustID { get { return isModify_CustID; } }
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
            isModify_Vender = b;
            isModify_CallNO = b;
            isModify_CalledNo = b;
            isModify_StartTime = b;
            isModify_EndTime = b;
            isModify_FileFullName = b;
            isModify_SGID = b;
            isModify_SessionID = b;
            isModify_SourceType = b;
            isModify_Status = b;
            isModify_ProcessUserID = b;
            isModify_ProcessTime = b;
            isModify_IsCallBack = b;
            isModify_CreateTime = b;
            isModify_LastUpdateTime = b;
            isModify_LastUpdateUserID = b;
            isModify_OrderID = b;
            isModify_CustID = b;
        }

        #endregion

    }
}
