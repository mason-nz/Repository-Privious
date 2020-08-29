using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CustomerCallInInfo
    /// <summary>
    /// 实体类：CustomerCallInInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-07-23
    /// </summary>
    [DBTableAttribute("CustomerCallIn")]
    public class CustomerCallInInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CustomerCallInInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CustomerCallInInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CustomerCallInInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _vendercallid = (!dr.Table.Columns.Contains("VenderCallID") || dr["VenderCallID"] == null || dr["VenderCallID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["VenderCallID"]);
            _vender = (!dr.Table.Columns.Contains("Vender") || dr["Vender"] == null || dr["Vender"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Vender"]);
            _callno = (!dr.Table.Columns.Contains("CallNO") || dr["CallNO"] == null || dr["CallNO"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CallNO"]);
            _calledno = (!dr.Table.Columns.Contains("CalledNo") || dr["CalledNo"] == null || dr["CalledNo"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CalledNo"]);
            _starttime = (!dr.Table.Columns.Contains("StartTime") || dr["StartTime"] == null || dr["StartTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["StartTime"]);
            _endtime = (!dr.Table.Columns.Contains("EndTime") || dr["EndTime"] == null || dr["EndTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["EndTime"]);
            _ivrkeys = (!dr.Table.Columns.Contains("IvrKeys") || dr["IvrKeys"] == null || dr["IvrKeys"] == DBNull.Value) ? null : (string)Convert.ToString(dr["IvrKeys"]);
            _callid = (!dr.Table.Columns.Contains("CallID") || dr["CallID"] == null || dr["CallID"] == DBNull.Value) ? null : (long?)Convert.ToInt64(dr["CallID"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
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

        #region 字段 VenderCallID (自动生成)
        private string _vendercallid = null;
        [DBFieldAttribute("VenderCallID", SqlDbType.VarChar, 50)]
        public string VenderCallID { get { return _vendercallid; } set { if (_vendercallid != value) { _vendercallid = value; isModify_VenderCallID = true; } } }
        public string ValueOrDefault_VenderCallID { get { return _vendercallid != null ? _vendercallid : ""; } }
        private bool isModify_VenderCallID = false;
        [IsModifyAttribute("VenderCallID")]
        public bool IsModify_VenderCallID { get { return isModify_VenderCallID; } }
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

        #region 字段 IvrKeys (自动生成)
        private string _ivrkeys = null;
        [DBFieldAttribute("IvrKeys", SqlDbType.VarChar, 50)]
        public string IvrKeys { get { return _ivrkeys; } set { if (_ivrkeys != value) { _ivrkeys = value; isModify_IvrKeys = true; } } }
        public string ValueOrDefault_IvrKeys { get { return _ivrkeys != null ? _ivrkeys : ""; } }
        private bool isModify_IvrKeys = false;
        [IsModifyAttribute("IvrKeys")]
        public bool IsModify_IvrKeys { get { return isModify_IvrKeys; } }
        #endregion

        #region 字段 CallID (自动生成)
        private long? _callid = null;
        [DBFieldAttribute("CallID", SqlDbType.BigInt, 8)]
        public long? CallID { get { return _callid; } set { if (_callid != value) { _callid = value; isModify_CallID = true; } } }
        public long ValueOrDefault_CallID { get { return _callid.HasValue ? _callid.Value : 0; } }
        private bool isModify_CallID = false;
        [IsModifyAttribute("CallID")]
        public bool IsModify_CallID { get { return isModify_CallID; } }
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
            isModify_VenderCallID = b;
            isModify_Vender = b;
            isModify_CallNO = b;
            isModify_CalledNo = b;
            isModify_StartTime = b;
            isModify_EndTime = b;
            isModify_IvrKeys = b;
            isModify_CallID = b;
            isModify_CreateTime = b;
        }

        #endregion

    }
}
