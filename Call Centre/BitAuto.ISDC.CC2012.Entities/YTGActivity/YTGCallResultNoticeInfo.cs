using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：YTGCallResultNoticeInfo
    /// <summary>
    /// 实体类：YTGCallResultNoticeInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-12-19
    /// </summary>
    [DBTableAttribute("YTGCallResultNotice")]
    public class YTGCallResultNoticeInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public YTGCallResultNoticeInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public YTGCallResultNoticeInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public YTGCallResultNoticeInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _taskid = (!dr.Table.Columns.Contains("TaskID") || dr["TaskID"] == null || dr["TaskID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["TaskID"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _reason = (!dr.Table.Columns.Contains("Reason") || dr["Reason"] == null || dr["Reason"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Reason"]);
            _failcount = (!dr.Table.Columns.Contains("FailCount") || dr["FailCount"] == null || dr["FailCount"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["FailCount"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _updatetime = (!dr.Table.Columns.Contains("UpdateTime") || dr["UpdateTime"] == null || dr["UpdateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["UpdateTime"]);
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

        #region 字段 Status (自动生成)
        private int? _status = null;
        [DBFieldAttribute("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; isModify_Status = true; } } }
        public int ValueOrDefault_Status { get { return _status.HasValue ? _status.Value : 0; } }
        private bool isModify_Status = false;
        [IsModifyAttribute("Status")]
        public bool IsModify_Status { get { return isModify_Status; } }
        #endregion

        #region 字段 Reason (自动生成)
        private string _reason = null;
        [DBFieldAttribute("Reason", SqlDbType.VarChar, 3000)]
        public string Reason { get { return _reason; } set { if (_reason != value) { _reason = value; isModify_Reason = true; } } }
        public string ValueOrDefault_Reason { get { return _reason != null ? _reason : ""; } }
        private bool isModify_Reason = false;
        [IsModifyAttribute("Reason")]
        public bool IsModify_Reason { get { return isModify_Reason; } }
        #endregion

        #region 字段 FailCount (自动生成)
        private int? _failcount = null;
        [DBFieldAttribute("FailCount", SqlDbType.Int, 4)]
        public int? FailCount { get { return _failcount; } set { if (_failcount != value) { _failcount = value; isModify_FailCount = true; } } }
        public int ValueOrDefault_FailCount { get { return _failcount.HasValue ? _failcount.Value : 0; } }
        private bool isModify_FailCount = false;
        [IsModifyAttribute("FailCount")]
        public bool IsModify_FailCount { get { return isModify_FailCount; } }
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

        #region 字段 UpdateTime (自动生成)
        private DateTime? _updatetime = null;
        [DBFieldAttribute("UpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? UpdateTime { get { return _updatetime; } set { if (_updatetime != value) { _updatetime = value; isModify_UpdateTime = true; } } }
        public DateTime ValueOrDefault_UpdateTime { get { return _updatetime.HasValue ? _updatetime.Value : new DateTime(); } }
        private bool isModify_UpdateTime = false;
        [IsModifyAttribute("UpdateTime")]
        public bool IsModify_UpdateTime { get { return isModify_UpdateTime; } }
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
            isModify_Status = b;
            isModify_Reason = b;
            isModify_FailCount = b;
            isModify_CreateTime = b;
            isModify_UpdateTime = b;
        }

        #endregion

    }
}
