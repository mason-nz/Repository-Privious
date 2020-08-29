using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CustomerCallInPressKeyInfo
    /// <summary>
    /// 实体类：CustomerCallInPressKeyInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-07-23
    /// </summary>
    [DBTableAttribute("CustomerCallInPressKey")]
    public class CustomerCallInPressKeyInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CustomerCallInPressKeyInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CustomerCallInPressKeyInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CustomerCallInPressKeyInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _vendercallid = (!dr.Table.Columns.Contains("VenderCallID") || dr["VenderCallID"] == null || dr["VenderCallID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["VenderCallID"]);
            _vender = (!dr.Table.Columns.Contains("Vender") || dr["Vender"] == null || dr["Vender"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Vender"]);
            _sortnum = (!dr.Table.Columns.Contains("SortNum") || dr["SortNum"] == null || dr["SortNum"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["SortNum"]);
            _presskey = (!dr.Table.Columns.Contains("PressKey") || dr["PressKey"] == null || dr["PressKey"] == DBNull.Value) ? null : (string)Convert.ToString(dr["PressKey"]);
            _presskeyname = (!dr.Table.Columns.Contains("PressKeyName") || dr["PressKeyName"] == null || dr["PressKeyName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["PressKeyName"]);
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

        #region 字段 SortNum (自动生成)
        private int? _sortnum = null;
        [DBFieldAttribute("SortNum", SqlDbType.Int, 4)]
        public int? SortNum { get { return _sortnum; } set { if (_sortnum != value) { _sortnum = value; isModify_SortNum = true; } } }
        public int ValueOrDefault_SortNum { get { return _sortnum.HasValue ? _sortnum.Value : 0; } }
        private bool isModify_SortNum = false;
        [IsModifyAttribute("SortNum")]
        public bool IsModify_SortNum { get { return isModify_SortNum; } }
        #endregion

        #region 字段 PressKey (自动生成)
        private string _presskey = null;
        [DBFieldAttribute("PressKey", SqlDbType.Char, 1)]
        public string PressKey { get { return _presskey; } set { if (_presskey != value) { _presskey = value; isModify_PressKey = true; } } }
        public string ValueOrDefault_PressKey { get { return _presskey != null ? _presskey : ""; } }
        private bool isModify_PressKey = false;
        [IsModifyAttribute("PressKey")]
        public bool IsModify_PressKey { get { return isModify_PressKey; } }
        #endregion

        #region 字段 PressKeyName (自动生成)
        private string _presskeyname = null;
        [DBFieldAttribute("PressKeyName", SqlDbType.VarChar, 50)]
        public string PressKeyName { get { return _presskeyname; } set { if (_presskeyname != value) { _presskeyname = value; isModify_PressKeyName = true; } } }
        public string ValueOrDefault_PressKeyName { get { return _presskeyname != null ? _presskeyname : ""; } }
        private bool isModify_PressKeyName = false;
        [IsModifyAttribute("PressKeyName")]
        public bool IsModify_PressKeyName { get { return isModify_PressKeyName; } }
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
            isModify_SortNum = b;
            isModify_PressKey = b;
            isModify_PressKeyName = b;
            isModify_CallID = b;
            isModify_CreateTime = b;
        }

        #endregion

    }
}
