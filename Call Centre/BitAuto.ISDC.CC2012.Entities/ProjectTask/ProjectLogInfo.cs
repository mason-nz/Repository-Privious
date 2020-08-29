using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：ProjectLogInfo
    /// <summary>
    /// 实体类：ProjectLogInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-09-22
    /// </summary>
    [DBTableAttribute("ProjectLog")]
    public class ProjectLogInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public ProjectLogInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public ProjectLogInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public ProjectLogInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _projectid = (!dr.Table.Columns.Contains("ProjectID") || dr["ProjectID"] == null || dr["ProjectID"] == DBNull.Value) ? null : (long?)Convert.ToInt64(dr["ProjectID"]);
            _opername = (!dr.Table.Columns.Contains("OperName") || dr["OperName"] == null || dr["OperName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["OperName"]);
            _remark = (!dr.Table.Columns.Contains("Remark") || dr["Remark"] == null || dr["Remark"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Remark"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
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

        #region 字段 ProjectID (自动生成)
        private long? _projectid = null;
        [DBFieldAttribute("ProjectID", SqlDbType.BigInt, 8)]
        public long? ProjectID { get { return _projectid; } set { if (_projectid != value) { _projectid = value; isModify_ProjectID = true; } } }
        public long ValueOrDefault_ProjectID { get { return _projectid.HasValue ? _projectid.Value : 0; } }
        private bool isModify_ProjectID = false;
        [IsModifyAttribute("ProjectID")]
        public bool IsModify_ProjectID { get { return isModify_ProjectID; } }
        #endregion

        #region 字段 OperName (自动生成)
        private string _opername = null;
        [DBFieldAttribute("OperName", SqlDbType.VarChar, 50)]
        public string OperName { get { return _opername; } set { if (_opername != value) { _opername = value; isModify_OperName = true; } } }
        public string ValueOrDefault_OperName { get { return _opername != null ? _opername : ""; } }
        private bool isModify_OperName = false;
        [IsModifyAttribute("OperName")]
        public bool IsModify_OperName { get { return isModify_OperName; } }
        #endregion

        #region 字段 Remark (自动生成)
        private string _remark = null;
        [DBFieldAttribute("Remark", SqlDbType.VarChar, 300)]
        public string Remark { get { return _remark; } set { if (_remark != value) { _remark = value; isModify_Remark = true; } } }
        public string ValueOrDefault_Remark { get { return _remark != null ? _remark : ""; } }
        private bool isModify_Remark = false;
        [IsModifyAttribute("Remark")]
        public bool IsModify_Remark { get { return isModify_Remark; } }
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
            isModify_ProjectID = b;
            isModify_OperName = b;
            isModify_Remark = b;
            isModify_CreateUserID = b;
            isModify_CreateTime = b;
        }

        #endregion

    }
}
