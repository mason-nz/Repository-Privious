using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;
using System.Data;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类FreProblem 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:03 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    /// 实体类：FreProblemInfo
    /// <summary>
    /// 实体类：FreProblemInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-10-29
    /// </summary>
    [DBTableAttribute("FreProblem")]
    public class FreProblem
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public FreProblem()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public FreProblem(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public FreProblem(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _title = (!dr.Table.Columns.Contains("Title") || dr["Title"] == null || dr["Title"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Title"]);
            _url = (!dr.Table.Columns.Contains("Url") || dr["Url"] == null || dr["Url"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Url"]);
            _sortnum = (!dr.Table.Columns.Contains("SortNum") || dr["SortNum"] == null || dr["SortNum"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["SortNum"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
            _lastupdatetime = (!dr.Table.Columns.Contains("LastUpdateTime") || dr["LastUpdateTime"] == null || dr["LastUpdateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastUpdateTime"]);
            _lastupdateuserid = (!dr.Table.Columns.Contains("LastUpdateUserID") || dr["LastUpdateUserID"] == null || dr["LastUpdateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["LastUpdateUserID"]);
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

        #region 字段 Title (自动生成)
        private string _title = null;
        [DBFieldAttribute("Title", SqlDbType.VarChar, 100)]
        public string Title { get { return _title; } set { if (_title != value) { _title = value; isModify_Title = true; } } }
        public string ValueOrDefault_Title { get { return _title != null ? _title : ""; } }
        private bool isModify_Title = false;
        [IsModifyAttribute("Title")]
        public bool IsModify_Title { get { return isModify_Title; } }
        #endregion

        #region 字段 Url (自动生成)
        private string _url = null;
        [DBFieldAttribute("Url", SqlDbType.VarChar, 500)]
        public string Url { get { return _url; } set { if (_url != value) { _url = value; isModify_Url = true; } } }
        public string ValueOrDefault_Url { get { return _url != null ? _url : ""; } }
        private bool isModify_Url = false;
        [IsModifyAttribute("Url")]
        public bool IsModify_Url { get { return isModify_Url; } }
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

        #region 字段 Status (自动生成)
        private int? _status = null;
        [DBFieldAttribute("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; isModify_Status = true; } } }
        public int ValueOrDefault_Status { get { return _status.HasValue ? _status.Value : 0; } }
        private bool isModify_Status = false;
        [IsModifyAttribute("Status")]
        public bool IsModify_Status { get { return isModify_Status; } }
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

        #region 字段 CreateUserID (自动生成)
        private int? _createuserid = null;
        [DBFieldAttribute("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; isModify_CreateUserID = true; } } }
        public int ValueOrDefault_CreateUserID { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private bool isModify_CreateUserID = false;
        [IsModifyAttribute("CreateUserID")]
        public bool IsModify_CreateUserID { get { return isModify_CreateUserID; } }
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
            isModify_Title = b;
            isModify_Url = b;
            isModify_SortNum = b;
            isModify_Status = b;
            isModify_CreateTime = b;
            isModify_CreateUserID = b;
            isModify_LastUpdateTime = b;
            isModify_LastUpdateUserID = b;
        }

        #endregion

    }
}

