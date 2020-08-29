using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;
using System.Data;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CityGroupAgent 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
    /// 实体类：CityGroupAgentInfo
    /// <summary>
    /// 实体类：CityGroupAgentInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-10-29
    /// </summary>
    [DBTableAttribute("CityGroupAgent")]
    public class CityGroupAgent
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CityGroupAgent()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CityGroupAgent(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CityGroupAgent(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _citygroupid = (!dr.Table.Columns.Contains("CityGroupID") || dr["CityGroupID"] == null || dr["CityGroupID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CityGroupID"]);
            _userid = (!dr.Table.Columns.Contains("UserID") || dr["UserID"] == null || dr["UserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["UserID"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
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

        #region 字段 CityGroupID (自动生成)
        private string _citygroupid = null;
        [DBFieldAttribute("CityGroupID", SqlDbType.VarChar, 50)]
        public string CityGroupID { get { return _citygroupid; } set { if (_citygroupid != value) { _citygroupid = value; isModify_CityGroupID = true; } } }
        public string ValueOrDefault_CityGroupID { get { return _citygroupid != null ? _citygroupid : ""; } }
        private bool isModify_CityGroupID = false;
        [IsModifyAttribute("CityGroupID")]
        public bool IsModify_CityGroupID { get { return isModify_CityGroupID; } }
        #endregion

        #region 字段 UserID (自动生成)
        private int? _userid = null;
        [DBFieldAttribute("UserID", SqlDbType.Int, 4)]
        public int? UserID { get { return _userid; } set { if (_userid != value) { _userid = value; isModify_UserID = true; } } }
        public int ValueOrDefault_UserID { get { return _userid.HasValue ? _userid.Value : 0; } }
        private bool isModify_UserID = false;
        [IsModifyAttribute("UserID")]
        public bool IsModify_UserID { get { return isModify_UserID; } }
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
            isModify_CityGroupID = b;
            isModify_UserID = b;
            isModify_Status = b;
            isModify_CreateTime = b;
            isModify_CreateUserID = b;
        }

        #endregion

    }
}

