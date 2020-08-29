using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：YTGActivityTaskLogInfo
    /// <summary>
    /// 实体类：YTGActivityTaskLogInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-12-19
    /// </summary>
    [DBTableAttribute("YTGActivityTaskLog")]
    public class YTGActivityTaskLogInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public YTGActivityTaskLogInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public YTGActivityTaskLogInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public YTGActivityTaskLogInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _taskid = (!dr.Table.Columns.Contains("TaskID") || dr["TaskID"] == null || dr["TaskID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["TaskID"]);
            _operationstatus = (!dr.Table.Columns.Contains("OperationStatus") || dr["OperationStatus"] == null || dr["OperationStatus"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["OperationStatus"]);
            _taskstatus = (!dr.Table.Columns.Contains("TaskStatus") || dr["TaskStatus"] == null || dr["TaskStatus"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["TaskStatus"]);
            _remark = (!dr.Table.Columns.Contains("Remark") || dr["Remark"] == null || dr["Remark"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Remark"]);
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

        #region 字段 TaskID (自动生成)
        private string _taskid = null;
        [DBFieldAttribute("TaskID", SqlDbType.VarChar, 50)]
        public string TaskID { get { return _taskid; } set { if (_taskid != value) { _taskid = value; isModify_TaskID = true; } } }
        public string ValueOrDefault_TaskID { get { return _taskid != null ? _taskid : ""; } }
        private bool isModify_TaskID = false;
        [IsModifyAttribute("TaskID")]
        public bool IsModify_TaskID { get { return isModify_TaskID; } }
        #endregion

        #region 字段 OperationStatus (自动生成)
        private int? _operationstatus = null;
        [DBFieldAttribute("OperationStatus", SqlDbType.Int, 4)]
        public int? OperationStatus { get { return _operationstatus; } set { if (_operationstatus != value) { _operationstatus = value; isModify_OperationStatus = true; } } }
        public int ValueOrDefault_OperationStatus { get { return _operationstatus.HasValue ? _operationstatus.Value : 0; } }
        private bool isModify_OperationStatus = false;
        [IsModifyAttribute("OperationStatus")]
        public bool IsModify_OperationStatus { get { return isModify_OperationStatus; } }
        #endregion

        #region 字段 TaskStatus (自动生成)
        private int? _taskstatus = null;
        [DBFieldAttribute("TaskStatus", SqlDbType.Int, 4)]
        public int? TaskStatus { get { return _taskstatus; } set { if (_taskstatus != value) { _taskstatus = value; isModify_TaskStatus = true; } } }
        public int ValueOrDefault_TaskStatus { get { return _taskstatus.HasValue ? _taskstatus.Value : 0; } }
        private bool isModify_TaskStatus = false;
        [IsModifyAttribute("TaskStatus")]
        public bool IsModify_TaskStatus { get { return isModify_TaskStatus; } }
        #endregion

        #region 字段 Remark (自动生成)
        private string _remark = null;
        [DBFieldAttribute("Remark", SqlDbType.NVarChar, 4000)]
        public string Remark { get { return _remark; } set { if (_remark != value) { _remark = value; isModify_Remark = true; } } }
        public string ValueOrDefault_Remark { get { return _remark != null ? _remark : ""; } }
        private bool isModify_Remark = false;
        [IsModifyAttribute("Remark")]
        public bool IsModify_Remark { get { return isModify_Remark; } }
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
            isModify_TaskID = b;
            isModify_OperationStatus = b;
            isModify_TaskStatus = b;
            isModify_Remark = b;
            isModify_CreateTime = b;
            isModify_CreateUserID = b;
        }

        #endregion

    }
}
