using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CustPhoneVisitBusinessInfo
    /// <summary>
    /// 实体类：CustPhoneVisitBusinessInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-04-29
    /// </summary>
    [DBTableAttribute("CustPhoneVisitBusiness")]
    public class CustPhoneVisitBusinessInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CustPhoneVisitBusinessInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CustPhoneVisitBusinessInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CustPhoneVisitBusinessInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region 属性
        #region 字段 RecID (自动生成)
        [DBField("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        public int RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private int? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region 字段 PhoneNum (自动生成)
        [DBField("PhoneNum", SqlDbType.VarChar, 20)]
        public string PhoneNum { get { return _phonenum; } set { if (_phonenum != value) { _phonenum = value; IsModify_PhoneNum = true; } } }
        public string PhoneNum_Value { get { return _phonenum != null ? _phonenum : ""; } }
        private string _phonenum = null;

        [IsModify("PhoneNum")]
        public bool IsModify_PhoneNum { get; set; }
        #endregion

        #region 字段 TaskID (自动生成)
        [DBField("TaskID", SqlDbType.VarChar, 50)]
        public string TaskID { get { return _taskid; } set { if (_taskid != value) { _taskid = value; IsModify_TaskID = true; } } }
        public string TaskID_Value { get { return _taskid != null ? _taskid : ""; } }
        private string _taskid = null;

        [IsModify("TaskID")]
        public bool IsModify_TaskID { get; set; }
        #endregion

        #region 字段 BusinessType (自动生成)
        /// <summary>
        /// 任务类型：-1 不存在 0 其他非CC系统 ，1：工单  3：客户核实  4：其他任务  5：YJK 6：CJK  7:易团购 
        /// </summary>
        [DBField("BusinessType", SqlDbType.Int, 4)]
        public int? BusinessType { get { return _businesstype; } set { if (_businesstype != value) { _businesstype = value; IsModify_BusinessType = true; } } }
        public int BusinessType_Value { get { return _businesstype.HasValue ? _businesstype.Value : 0; } }
        private int? _businesstype = null;

        [IsModify("BusinessType")]
        public bool IsModify_BusinessType { get; set; }
        #endregion

        #region 字段 TaskSource (自动生成)
        [DBField("TaskSource", SqlDbType.Int, 4)]
        public int? TaskSource { get { return _tasksource; } set { if (_tasksource != value) { _tasksource = value; IsModify_TaskSource = true; } } }
        public int TaskSource_Value { get { return _tasksource.HasValue ? _tasksource.Value : 0; } }
        private int? _tasksource = null;

        [IsModify("TaskSource")]
        public bool IsModify_TaskSource { get; set; }
        #endregion

        #region 字段 CallID (自动生成)
        [DBField("CallID", SqlDbType.BigInt, 8)]
        public long? CallID { get { return _callid; } set { if (_callid != value) { _callid = value; IsModify_CallID = true; } } }
        public long CallID_Value { get { return _callid.HasValue ? _callid.Value : 0; } }
        private long? _callid = null;

        [IsModify("CallID")]
        public bool IsModify_CallID { get; set; }
        #endregion

        #region 字段 CreateTime (自动生成)
        [DBField("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; IsModify_CreateTime = true; } } }
        public DateTime CreateTime_Value { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private DateTime? _createtime = null;

        [IsModify("CreateTime")]
        public bool IsModify_CreateTime { get; set; }
        #endregion

        #region 字段 CreateUserID (自动生成)
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion

        #region 字段 ModifyTime (自动生成)
        [DBField("ModifyTime", SqlDbType.DateTime, 8)]
        public DateTime? ModifyTime { get { return _modifytime; } set { if (_modifytime != value) { _modifytime = value; IsModify_ModifyTime = true; } } }
        public DateTime ModifyTime_Value { get { return _modifytime.HasValue ? _modifytime.Value : new DateTime(); } }
        private DateTime? _modifytime = null;

        [IsModify("ModifyTime")]
        public bool IsModify_ModifyTime { get; set; }
        #endregion

        #region 字段 ModifyUserID (自动生成)
        [DBField("ModifyUserID", SqlDbType.Int, 4)]
        public int? ModifyUserID { get { return _modifyuserid; } set { if (_modifyuserid != value) { _modifyuserid = value; IsModify_ModifyUserID = true; } } }
        public int ModifyUserID_Value { get { return _modifyuserid.HasValue ? _modifyuserid.Value : 0; } }
        private int? _modifyuserid = null;

        [IsModify("ModifyUserID")]
        public bool IsModify_ModifyUserID { get; set; }
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
            IsModify_PhoneNum = b;
            IsModify_TaskID = b;
            IsModify_BusinessType = b;
            IsModify_TaskSource = b;
            IsModify_CallID = b;
            IsModify_CreateTime = b;
            IsModify_CreateUserID = b;
            IsModify_ModifyTime = b;
            IsModify_ModifyUserID = b;
        }

        #endregion

    }
}
