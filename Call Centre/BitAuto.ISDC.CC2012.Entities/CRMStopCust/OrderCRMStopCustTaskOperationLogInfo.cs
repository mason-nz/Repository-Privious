using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：OrderCRMStopCustTaskOperationLogInfo  
    /// <summary>
    /// 实体类：OrderCRMStopCustTaskOperationLogInfo  
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-08-16
    /// </summary>
    [DBTableAttribute("OrderCRMStopCustTaskOperationLog")]
    public class OrderCRMStopCustTaskOperationLogInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public OrderCRMStopCustTaskOperationLogInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public OrderCRMStopCustTaskOperationLogInfo(long _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public OrderCRMStopCustTaskOperationLogInfo(DataRow dr)
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

        #region OperationStatus [操作类型]
        /// <summary>
        /// 操作类型
        /// </summary>
        [DBField("OperationStatus", SqlDbType.Int, 4)]
        public int? OperationStatus { get { return _operationstatus; } set { if (_operationstatus != value) { _operationstatus = value; IsModify_OperationStatus = true; } } }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int OperationStatus_Value { get { return _operationstatus.HasValue ? _operationstatus.Value : 0; } }
        private int? _operationstatus = null;

        [IsModify("OperationStatus")]
        public bool IsModify_OperationStatus { get; set; }
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

        #region Remark [备注]
        /// <summary>
        /// 备注
        /// </summary>
        [DBField("Remark", SqlDbType.NVarChar, 1000)]
        public string Remark { get { return _remark; } set { if (_remark != value) { _remark = value; IsModify_Remark = true; } } }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark_Value { get { return _remark != null ? _remark : ""; } }
        private string _remark = null;

        [IsModify("Remark")]
        public bool IsModify_Remark { get; set; }
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
            IsModify_TaskID = b;
            IsModify_OperationStatus = b;
            IsModify_TaskStatus = b;
            IsModify_Remark = b;
            IsModify_CreateTime = b;
            IsModify_CreateUserID = b;
        }

        #endregion

    }
}
