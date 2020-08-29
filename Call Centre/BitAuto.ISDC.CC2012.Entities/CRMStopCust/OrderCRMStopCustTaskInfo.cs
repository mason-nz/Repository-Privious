using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：OrderCRMStopCustTaskInfo  
    /// <summary>
    /// 实体类：OrderCRMStopCustTaskInfo  
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-08-16
    /// </summary>
    [DBTableAttribute("OrderCRMStopCustTask")]
    public class OrderCRMStopCustTaskInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public OrderCRMStopCustTaskInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public OrderCRMStopCustTaskInfo(long _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public OrderCRMStopCustTaskInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region 属性
        #region RecID [自增主键]
        /// <summary>
        /// 自增主键
        /// </summary>
        [DBField("RecID", SqlDbType.BigInt, 8, true, true)]
        public long? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        /// <summary>
        /// 自增主键
        /// </summary>
        public long RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private long? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region TaskStatus [任务状态]
        /// <summary>
        /// 任务状态
        /// </summary>
        [DBField("TaskStatus", SqlDbType.Int, 4)]
        public int? TaskStatus { get { return _taskstatus; } set { if (_taskstatus != value) { _taskstatus = value; IsModify_TaskStatus = true; } } }
        /// <summary>
        /// 任务状态
        /// </summary>
        public int TaskStatus_Value { get { return _taskstatus.HasValue ? _taskstatus.Value : 0; } }
        private int? _taskstatus = null;

        [IsModify("TaskStatus")]
        public bool IsModify_TaskStatus { get; set; }
        #endregion

        #region RelationID [数据ID]
        /// <summary>
        /// 数据ID
        /// </summary>
        [DBField("RelationID", SqlDbType.BigInt, 8)]
        public long? RelationID { get { return _relationid; } set { if (_relationid != value) { _relationid = value; IsModify_RelationID = true; } } }
        /// <summary>
        /// 数据ID
        /// </summary>
        public long RelationID_Value { get { return _relationid.HasValue ? _relationid.Value : 0; } }
        private long? _relationid = null;

        [IsModify("RelationID")]
        public bool IsModify_RelationID { get; set; }
        #endregion

        #region BGID [分组ID]
        /// <summary>
        /// 分组ID
        /// </summary>
        [DBField("BGID", SqlDbType.Int, 4)]
        public int? BGID { get { return _bgid; } set { if (_bgid != value) { _bgid = value; IsModify_BGID = true; } } }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int BGID_Value { get { return _bgid.HasValue ? _bgid.Value : 0; } }
        private int? _bgid = null;

        [IsModify("BGID")]
        public bool IsModify_BGID { get; set; }
        #endregion

        #region AssignUserID [分配坐席ID]
        /// <summary>
        /// 分配坐席ID
        /// </summary>
        [DBField("AssignUserID", SqlDbType.Int, 4)]
        public int? AssignUserID { get { return _assignuserid; } set { if (_assignuserid != value) { _assignuserid = value; IsModify_AssignUserID = true; } } }
        /// <summary>
        /// 分配坐席ID
        /// </summary>
        public int AssignUserID_Value { get { return _assignuserid.HasValue ? _assignuserid.Value : 0; } }
        private int? _assignuserid = null;

        [IsModify("AssignUserID")]
        public bool IsModify_AssignUserID { get; set; }
        #endregion

        #region AssignTime [分配时间]
        /// <summary>
        /// 分配时间
        /// </summary>
        [DBField("AssignTime", SqlDbType.DateTime, 8)]
        public DateTime? AssignTime { get { return _assigntime; } set { if (_assigntime != value) { _assigntime = value; IsModify_AssignTime = true; } } }
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime AssignTime_Value { get { return _assigntime.HasValue ? _assigntime.Value : new DateTime(); } }
        private DateTime? _assigntime = null;

        [IsModify("AssignTime")]
        public bool IsModify_AssignTime { get; set; }
        #endregion

        #region Status [状态]
        /// <summary>
        /// 状态
        /// </summary>
        [DBField("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; IsModify_Status = true; } } }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status_Value { get { return _status.HasValue ? _status.Value : 0; } }
        private int? _status = null;

        [IsModify("Status")]
        public bool IsModify_Status { get; set; }
        #endregion

        #region SubmitTime [提交时间]
        /// <summary>
        /// 提交时间
        /// </summary>
        [DBField("SubmitTime", SqlDbType.DateTime, 8)]
        public DateTime? SubmitTime { get { return _submittime; } set { if (_submittime != value) { _submittime = value; IsModify_SubmitTime = true; } } }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime_Value { get { return _submittime.HasValue ? _submittime.Value : new DateTime(); } }
        private DateTime? _submittime = null;

        [IsModify("SubmitTime")]
        public bool IsModify_SubmitTime { get; set; }
        #endregion

        #region CreateTime [创建时间]
        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; IsModify_CreateTime = true; } } }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime_Value { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private DateTime? _createtime = null;

        [IsModify("CreateTime")]
        public bool IsModify_CreateTime { get; set; }
        #endregion

        #region CreateUserID [创建人]
        /// <summary>
        /// 创建人
        /// </summary>
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion

        #region TaskID [任务ID]
        /// <summary>
        /// 任务ID
        /// </summary>
        [DBField("TaskID", SqlDbType.VarChar, 20)]
        public string TaskID { get { return _taskid; } set { if (_taskid != value) { _taskid = value; IsModify_TaskID = true; } } }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID_Value { get { return _taskid != null ? _taskid : ""; } }
        private string _taskid = null;

        [IsModify("TaskID")]
        public bool IsModify_TaskID { get; set; }
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
            IsModify_RecID = b;
            IsModify_TaskStatus = b;
            IsModify_RelationID = b;
            IsModify_BGID = b;
            IsModify_AssignUserID = b;
            IsModify_AssignTime = b;
            IsModify_Status = b;
            IsModify_SubmitTime = b;
            IsModify_CreateTime = b;
            IsModify_CreateUserID = b;
            IsModify_TaskID = b;
        }

        #endregion

    }
}
