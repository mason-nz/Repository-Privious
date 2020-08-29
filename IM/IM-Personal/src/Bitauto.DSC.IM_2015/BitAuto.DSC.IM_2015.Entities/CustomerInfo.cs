using System;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;
using System.Collections.Generic;

namespace BitAuto.DSC.IM_2015.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类CustomerInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    /// 实体类：CustomerInfoInfo
    /// <summary>
    /// 实体类：CustomerInfoInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-10-29
    /// </summary>
    [DBTableAttribute("CustomerInfo")]
    public class CustomerInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CustomerInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CustomerInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CustomerInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _membercode = (!dr.Table.Columns.Contains("MemberCode") || dr["MemberCode"] == null || dr["MemberCode"] == DBNull.Value) ? null : (string)Convert.ToString(dr["MemberCode"]);
            _lastuserid = (!dr.Table.Columns.Contains("LastUserID") || dr["LastUserID"] == null || dr["LastUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["LastUserID"]);
            _lastmessagetime = (!dr.Table.Columns.Contains("LastMessageTime") || dr["LastMessageTime"] == null || dr["LastMessageTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastMessageTime"]);
            _lastbegintime = (!dr.Table.Columns.Contains("LastBeginTime") || dr["LastBeginTime"] == null || dr["LastBeginTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastBeginTime"]);
            _distribution = (!dr.Table.Columns.Contains("Distribution") || dr["Distribution"] == null || dr["Distribution"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Distribution"]);
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

        #region 字段 MemberCode (自动生成)
        private string _membercode = null;
        [DBFieldAttribute("MemberCode", SqlDbType.VarChar, 50)]
        public string MemberCode { get { return _membercode; } set { if (_membercode != value) { _membercode = value; isModify_MemberCode = true; } } }
        public string ValueOrDefault_MemberCode { get { return _membercode != null ? _membercode : ""; } }
        private bool isModify_MemberCode = false;
        [IsModifyAttribute("MemberCode")]
        public bool IsModify_MemberCode { get { return isModify_MemberCode; } }
        #endregion

        #region 字段 LastUserID (自动生成)
        private int? _lastuserid = null;
        [DBFieldAttribute("LastUserID", SqlDbType.Int, 4)]
        public int? LastUserID { get { return _lastuserid; } set { if (_lastuserid != value) { _lastuserid = value; isModify_LastUserID = true; } } }
        public int ValueOrDefault_LastUserID { get { return _lastuserid.HasValue ? _lastuserid.Value : 0; } }
        private bool isModify_LastUserID = false;
        [IsModifyAttribute("LastUserID")]
        public bool IsModify_LastUserID { get { return isModify_LastUserID; } }
        #endregion

        #region 字段 LastMessageTime (自动生成)
        private DateTime? _lastmessagetime = null;
        [DBFieldAttribute("LastMessageTime", SqlDbType.DateTime, 8)]
        public DateTime? LastMessageTime { get { return _lastmessagetime; } set { if (_lastmessagetime != value) { _lastmessagetime = value; isModify_LastMessageTime = true; } } }
        public DateTime ValueOrDefault_LastMessageTime { get { return _lastmessagetime.HasValue ? _lastmessagetime.Value : new DateTime(); } }
        private bool isModify_LastMessageTime = false;
        [IsModifyAttribute("LastMessageTime")]
        public bool IsModify_LastMessageTime { get { return isModify_LastMessageTime; } }
        #endregion

        #region 字段 LastBeginTime (自动生成)
        private DateTime? _lastbegintime = null;
        [DBFieldAttribute("LastBeginTime", SqlDbType.DateTime, 8)]
        public DateTime? LastBeginTime { get { return _lastbegintime; } set { if (_lastbegintime != value) { _lastbegintime = value; isModify_LastBeginTime = true; } } }
        public DateTime ValueOrDefault_LastBeginTime { get { return _lastbegintime.HasValue ? _lastbegintime.Value : new DateTime(); } }
        private bool isModify_LastBeginTime = false;
        [IsModifyAttribute("LastBeginTime")]
        public bool IsModify_LastBeginTime { get { return isModify_LastBeginTime; } }
        #endregion

        #region 字段 Distribution (自动生成)
        private int? _distribution = null;
        [DBFieldAttribute("Distribution", SqlDbType.Int, 4)]
        public int? Distribution { get { return _distribution; } set { if (_distribution != value) { _distribution = value; isModify_Distribution = true; } } }
        public int ValueOrDefault_Distribution { get { return _distribution.HasValue ? _distribution.Value : 0; } }
        private bool isModify_Distribution = false;
        [IsModifyAttribute("Distribution")]
        public bool IsModify_Distribution { get { return isModify_Distribution; } }
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
            isModify_MemberCode = b;
            isModify_LastUserID = b;
            isModify_LastMessageTime = b;
            isModify_LastBeginTime = b;
            isModify_Distribution = b;
            isModify_Status = b;
            isModify_CreateTime = b;
            isModify_CreateUserID = b;
            isModify_LastUpdateTime = b;
            isModify_LastUpdateUserID = b;
        }

        #endregion

        #region 扩展

        public string Name { get; set; }
        public string Sex { get; set; }
        public string CustID { get; set; }
        public string Tel { get; set; }
        public int CityID { get; set; }
        public int ProvinceID { get; set; }

        #endregion
    }
}

