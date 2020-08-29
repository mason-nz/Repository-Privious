using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：WOrderToAndCCInfo [任务列表]-[工单V2]-工单接收人和抄送人表 
    /// <summary>
    /// 实体类：WOrderToAndCCInfo [任务列表]-[工单V2]-工单接收人和抄送人表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-20
    /// </summary>
    [DBTableAttribute("WOrderToAndCC")]
    public class WOrderToAndCCInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public WOrderToAndCCInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public WOrderToAndCCInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderToAndCCInfo(DataRow dr)
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

        #region ReceiverID [关联的回复ID（不允许为空）]
        /// <summary>
        /// 关联的回复ID（不允许为空）
        /// </summary>
        [DBField("ReceiverID", SqlDbType.Int, 4)]
        public int? ReceiverID { get { return _receiverid; } set { if (_receiverid != value) { _receiverid = value; IsModify_ReceiverID = true; } } }
        /// <summary>
        /// 关联的回复ID（不允许为空）
        /// </summary>
        public int ReceiverID_Value { get { return _receiverid.HasValue ? _receiverid.Value : 0; } }
        private int? _receiverid = null;

        [IsModify("ReceiverID")]
        public bool IsModify_ReceiverID { get; set; }
        #endregion

        #region PersonType [人员类型；接收人=1，抄送人=2]
        /// <summary>
        /// 人员类型；接收人=1，抄送人=2
        /// </summary>
        [DBField("PersonType", SqlDbType.Int, 4)]
        public int? PersonType { get { return _persontype; } set { if (_persontype != value) { _persontype = value; IsModify_PersonType = true; } } }
        /// <summary>
        /// 人员类型；接收人=1，抄送人=2
        /// </summary>
        public int PersonType_Value { get { return _persontype.HasValue ? _persontype.Value : 0; } }
        private int? _persontype = null;

        [IsModify("PersonType")]
        public bool IsModify_PersonType { get; set; }
        #endregion

        #region UserNum [接收人或抄送人的员工编号]
        /// <summary>
        /// 接收人或抄送人的员工编号
        /// </summary>
        [DBField("UserNum", SqlDbType.VarChar, 20)]
        public string UserNum { get { return _usernum; } set { if (_usernum != value) { _usernum = value; IsModify_UserNum = true; } } }
        /// <summary>
        /// 接收人或抄送人的员工编号
        /// </summary>
        public string UserNum_Value { get { return _usernum != null ? _usernum : ""; } }
        private string _usernum = null;

        [IsModify("UserNum")]
        public bool IsModify_UserNum { get; set; }
        #endregion

        #region UserID [人员ID]
        /// <summary>
        /// 人员ID
        /// </summary>
        [DBField("UserID", SqlDbType.Int, 4)]
        public int? UserID { get { return _userid; } set { if (_userid != value) { _userid = value; IsModify_UserID = true; } } }
        /// <summary>
        /// 人员ID
        /// </summary>
        public int UserID_Value { get { return _userid.HasValue ? _userid.Value : 0; } }
        private int? _userid = null;

        [IsModify("UserID")]
        public bool IsModify_UserID { get; set; }
        #endregion

        #region UserName [接收人或抄送人的真实姓名]
        /// <summary>
        /// 接收人或抄送人的真实姓名
        /// </summary>
        [DBField("UserName", SqlDbType.VarChar, 50)]
        public string UserName { get { return _username; } set { if (_username != value) { _username = value; IsModify_UserName = true; } } }
        /// <summary>
        /// 接收人或抄送人的真实姓名
        /// </summary>
        public string UserName_Value { get { return _username != null ? _username : ""; } }
        private string _username = null;

        [IsModify("UserName")]
        public bool IsModify_UserName { get; set; }
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
            IsModify_ReceiverID = b;
            IsModify_PersonType = b;
            IsModify_UserNum = b;
            IsModify_UserID = b;
            IsModify_UserName = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
        }

        #endregion

    }
}
