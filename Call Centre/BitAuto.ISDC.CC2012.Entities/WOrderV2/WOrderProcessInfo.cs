using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：WOrderProcessInfo [任务列表]-[工单V2]-工单处理表 
    /// <summary>
    /// 实体类：WOrderProcessInfo [任务列表]-[工单V2]-工单处理表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-20
    /// </summary>
    [DBTableAttribute("WOrderProcess")]
    public class WOrderProcessInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public WOrderProcessInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public WOrderProcessInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderProcessInfo(DataRow dr)
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
        [DBField("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        /// <summary>
        /// 自增主键
        /// </summary>
        public int RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private int? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region OrderID [工单ID]
        /// <summary>
        /// 工单ID
        /// </summary>
        [DBField("OrderID", SqlDbType.VarChar, 20)]
        public string OrderID { get { return _orderid; } set { if (_orderid != value) { _orderid = value; IsModify_OrderID = true; } } }
        /// <summary>
        /// 工单ID
        /// </summary>
        public string OrderID_Value { get { return _orderid != null ? _orderid : ""; } }
        private string _orderid = null;

        [IsModify("OrderID")]
        public bool IsModify_OrderID { get; set; }
        #endregion

        #region ProcessType [处理类型： 添加=1，转出=2，处理=3，回访=4]
        /// <summary>
        /// 处理类型： 添加=1，转出=2，处理=3，回访=4
        /// </summary>
        [DBField("ProcessType", SqlDbType.Int, 4)]
        public int? ProcessType { get { return _processtype; } set { if (_processtype != value) { _processtype = value; IsModify_ProcessType = true; } } }
        /// <summary>
        /// 处理类型： 添加=1，转出=2，处理=3，回访=4
        /// </summary>
        public int ProcessType_Value { get { return _processtype.HasValue ? _processtype.Value : 0; } }
        private int? _processtype = null;

        [IsModify("ProcessType")]
        public bool IsModify_ProcessType { get; set; }
        #endregion

        #region WorkOrderStatus [当时切换后的工单状态；待审核=1，待处理=2，处理中=3，已处理=4，已完成=5，已关闭=6]
        /// <summary>
        /// 当时切换后的工单状态；待审核=1，待处理=2，处理中=3，已处理=4，已完成=5，已关闭=6
        /// </summary>
        [DBField("WorkOrderStatus", SqlDbType.Int, 4)]
        public int? WorkOrderStatus { get { return _workorderstatus; } set { if (_workorderstatus != value) { _workorderstatus = value; IsModify_WorkOrderStatus = true; } } }
        /// <summary>
        /// 当时切换后的工单状态；待审核=1，待处理=2，处理中=3，已处理=4，已完成=5，已关闭=6
        /// </summary>
        public int WorkOrderStatus_Value { get { return _workorderstatus.HasValue ? _workorderstatus.Value : 0; } }
        private int? _workorderstatus = null;

        [IsModify("WorkOrderStatus")]
        public bool IsModify_WorkOrderStatus { get; set; }
        #endregion

        #region IsReturnVisit [是否需要回访；1=是 0=否，-1=无效值]
        /// <summary>
        /// 是否需要回访；1=是 0=否，-1=无效值
        /// </summary>
        [DBField("IsReturnVisit", SqlDbType.Int, 4)]
        public int? IsReturnVisit { get { return _isreturnvisit; } set { if (_isreturnvisit != value) { _isreturnvisit = value; IsModify_IsReturnVisit = true; } } }
        /// <summary>
        /// 是否需要回访；1=是 0=否，-1=无效值
        /// </summary>
        public int IsReturnVisit_Value { get { return _isreturnvisit.HasValue ? _isreturnvisit.Value : 0; } }
        private int? _isreturnvisit = null;

        [IsModify("IsReturnVisit")]
        public bool IsModify_IsReturnVisit { get; set; }
        #endregion

        #region ProcessContent [处理内容（不需要回访的理由）（回访的结果）]
        /// <summary>
        /// 处理内容（不需要回访的理由）（回访的结果）
        /// </summary>
        [DBField("ProcessContent", SqlDbType.NVarChar, 3000)]
        public string ProcessContent { get { return _processcontent; } set { if (_processcontent != value) { _processcontent = value; IsModify_ProcessContent = true; } } }
        /// <summary>
        /// 处理内容（不需要回访的理由）（回访的结果）
        /// </summary>
        public string ProcessContent_Value { get { return _processcontent != null ? _processcontent : ""; } }
        private string _processcontent = null;

        [IsModify("ProcessContent")]
        public bool IsModify_ProcessContent { get; set; }
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

        #region CreateUserID [创建人ID]
        /// <summary>
        /// 创建人ID
        /// </summary>
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion

        #region CreateUserNum [创建人员工编号]
        /// <summary>
        /// 创建人员工编号
        /// </summary>
        [DBField("CreateUserNum", SqlDbType.VarChar, 20)]
        public string CreateUserNum { get { return _createusernum; } set { if (_createusernum != value) { _createusernum = value; IsModify_CreateUserNum = true; } } }
        /// <summary>
        /// 创建人员工编号
        /// </summary>
        public string CreateUserNum_Value { get { return _createusernum != null ? _createusernum : ""; } }
        private string _createusernum = null;

        [IsModify("CreateUserNum")]
        public bool IsModify_CreateUserNum { get; set; }
        #endregion

        #region CreateUserName [创建人员工真实姓名]
        /// <summary>
        /// 创建人员工真实姓名
        /// </summary>
        [DBField("CreateUserName", SqlDbType.VarChar, 50)]
        public string CreateUserName { get { return _createusername; } set { if (_createusername != value) { _createusername = value; IsModify_CreateUserName = true; } } }
        /// <summary>
        /// 创建人员工真实姓名
        /// </summary>
        public string CreateUserName_Value { get { return _createusername != null ? _createusername : ""; } }
        private string _createusername = null;

        [IsModify("CreateUserName")]
        public bool IsModify_CreateUserName { get; set; }
        #endregion

        #region CreateUserDeptName [创建人当时所在二级部门名称]
        /// <summary>
        /// 创建人当时所在二级部门名称
        /// </summary>
        [DBField("CreateUserDeptName", SqlDbType.VarChar, 50)]
        public string CreateUserDeptName { get { return _createuserdeptname; } set { if (_createuserdeptname != value) { _createuserdeptname = value; IsModify_CreateUserDeptName = true; } } }
        /// <summary>
        /// 创建人当时所在二级部门名称
        /// </summary>
        public string CreateUserDeptName_Value { get { return _createuserdeptname != null ? _createuserdeptname : ""; } }
        private string _createuserdeptname = null;

        [IsModify("CreateUserDeptName")]
        public bool IsModify_CreateUserDeptName { get; set; }
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
            IsModify_OrderID = b;
            IsModify_ProcessType = b;
            IsModify_WorkOrderStatus = b;
            IsModify_IsReturnVisit = b;
            IsModify_ProcessContent = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateUserNum = b;
            IsModify_CreateUserName = b;
            IsModify_CreateUserDeptName = b;
            IsModify_CreateTime = b;
        }

        #endregion

        #region  其他非数据库字段
        /// <summary>
        /// 存放处理记录状态 汉字
        /// </summary>
        public string StatusStr { get; set; }
        #endregion
    }
}
