using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：WOrderInfoInfo [任务列表]-[工单V2]-工单主表 
    /// <summary>
    /// 实体类：WOrderInfoInfo [任务列表]-[工单V2]-工单主表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-22
    /// </summary>
    [DBTableAttribute("WOrderInfo")]
    public class WOrderInfoInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public WOrderInfoInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public WOrderInfoInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderInfoInfo(DataRow dr)
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

        #region CallSource [通话来源；0=无，1=呼入，2=呼出，3= IM]
        /// <summary>
        /// 通话来源；0=无，1=呼入，2=呼出，3= IM
        /// </summary>
        [DBField("CallSource", SqlDbType.Int, 4)]
        public int? CallSource { get { return _callsource; } set { if (_callsource != value) { _callsource = value; IsModify_CallSource = true; } } }
        /// <summary>
        /// 通话来源；0=无，1=呼入，2=呼出，3= IM
        /// </summary>
        public int CallSource_Value { get { return _callsource.HasValue ? _callsource.Value : 0; } }
        private int? _callsource = null;

        [IsModify("CallSource")]
        public bool IsModify_CallSource { get; set; }
        #endregion

        #region ModuleSource [功能来源；1=客户池，2=工单，3=回访，4=未接来电，5=IM经销商，6=IM个人]
        /// <summary>
        /// 功能来源；1=客户池，2=工单，3=回访，4=未接来电，5=IM经销商，6=IM个人
        /// </summary>
        [DBField("ModuleSource", SqlDbType.Int, 4)]
        public int? ModuleSource { get { return _modulesource; } set { if (_modulesource != value) { _modulesource = value; IsModify_ModuleSource = true; } } }
        /// <summary>
        /// 功能来源；1=客户池，2=工单，3=回访，4=未接来电，5=IM经销商，6=IM个人
        /// </summary>
        public int ModuleSource_Value { get { return _modulesource.HasValue ? _modulesource.Value : 0; } }
        private int? _modulesource = null;

        [IsModify("ModuleSource")]
        public bool IsModify_ModuleSource { get; set; }
        #endregion

        #region DataSource [业务线来源]
        /// <summary>
        /// 业务线来源
        /// </summary>
        [DBField("DataSource", SqlDbType.Int, 4)]
        public int? DataSource { get { return _datasource; } set { if (_datasource != value) { _datasource = value; IsModify_DataSource = true; } } }
        /// <summary>
        /// 业务线来源
        /// </summary>
        public int DataSource_Value { get { return _datasource.HasValue ? _datasource.Value : 0; } }
        private int? _datasource = null;

        [IsModify("DataSource")]
        public bool IsModify_DataSource { get; set; }
        #endregion

        #region WorkOrderStatus [工单状态；待审核=1，待处理=2，处理中=3，已处理=4，已完成=5，已关闭=6]
        /// <summary>
        /// 工单状态；待审核=1，待处理=2，处理中=3，已处理=4，已完成=5，已关闭=6
        /// </summary>
        [DBField("WorkOrderStatus", SqlDbType.Int, 4)]
        public int? WorkOrderStatus { get { return _workorderstatus; } set { if (_workorderstatus != value) { _workorderstatus = value; IsModify_WorkOrderStatus = true; } } }
        /// <summary>
        /// 工单状态；待审核=1，待处理=2，处理中=3，已处理=4，已完成=5，已关闭=6
        /// </summary>
        public int WorkOrderStatus_Value { get { return _workorderstatus.HasValue ? _workorderstatus.Value : 0; } }
        private int? _workorderstatus = null;

        [IsModify("WorkOrderStatus")]
        public bool IsModify_WorkOrderStatus { get; set; }
        #endregion

        #region CategoryID [工单分类ID；问题处理=1，咨询=2，投诉=3，回访=4，合作=5，建议=6，其他=7]
        /// <summary>
        /// 工单分类ID；问题处理=1，咨询=2，投诉=3，回访=4，合作=5，建议=6，其他=7
        /// </summary>
        [DBField("CategoryID", SqlDbType.Int, 4)]
        public int? CategoryID { get { return _categoryid; } set { if (_categoryid != value) { _categoryid = value; IsModify_CategoryID = true; } } }
        /// <summary>
        /// 工单分类ID；问题处理=1，咨询=2，投诉=3，回访=4，合作=5，建议=6，其他=7
        /// </summary>
        public int CategoryID_Value { get { return _categoryid.HasValue ? _categoryid.Value : 0; } }
        private int? _categoryid = null;

        [IsModify("CategoryID")]
        public bool IsModify_CategoryID { get; set; }
        #endregion

        #region ComplaintLevel [投诉级别；A级=1，B级=2，常规=3]
        /// <summary>
        /// 投诉级别；A级=1，B级=2，常规=3
        /// </summary>
        [DBField("ComplaintLevel", SqlDbType.Int, 4)]
        public int? ComplaintLevel { get { return _complaintlevel; } set { if (_complaintlevel != value) { _complaintlevel = value; IsModify_ComplaintLevel = true; } } }
        /// <summary>
        /// 投诉级别；A级=1，B级=2，常规=3
        /// </summary>
        public int ComplaintLevel_Value { get { return _complaintlevel.HasValue ? _complaintlevel.Value : 0; } }
        private int? _complaintlevel = null;

        [IsModify("ComplaintLevel")]
        public bool IsModify_ComplaintLevel { get; set; }
        #endregion

        #region BusinessType [业务类型]
        /// <summary>
        /// 业务类型
        /// </summary>
        [DBField("BusinessType", SqlDbType.Int, 4)]
        public int? BusinessType { get { return _businesstype; } set { if (_businesstype != value) { _businesstype = value; IsModify_BusinessType = true; } } }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType_Value { get { return _businesstype.HasValue ? _businesstype.Value : 0; } }
        private int? _businesstype = null;

        [IsModify("BusinessType")]
        public bool IsModify_BusinessType { get; set; }
        #endregion

        #region BusinessTag [业务标签]
        /// <summary>
        /// 业务标签
        /// </summary>
        [DBField("BusinessTag", SqlDbType.Int, 4)]
        public int? BusinessTag { get { return _businesstag; } set { if (_businesstag != value) { _businesstag = value; IsModify_BusinessTag = true; } } }
        /// <summary>
        /// 业务标签
        /// </summary>
        public int BusinessTag_Value { get { return _businesstag.HasValue ? _businesstag.Value : 0; } }
        private int? _businesstag = null;

        [IsModify("BusinessTag")]
        public bool IsModify_BusinessTag { get; set; }
        #endregion

        #region IsSyncCRM [是否需要同步至访问记录；1=是 0=否]
        /// <summary>
        /// 是否需要同步至访问记录；1=是 0=否
        /// </summary>
        [DBField("IsSyncCRM", SqlDbType.Int, 4)]
        public int? IsSyncCRM { get { return _issynccrm; } set { if (_issynccrm != value) { _issynccrm = value; IsModify_IsSyncCRM = true; } } }
        /// <summary>
        /// 是否需要同步至访问记录；1=是 0=否
        /// </summary>
        public int IsSyncCRM_Value { get { return _issynccrm.HasValue ? _issynccrm.Value : 0; } }
        private int? _issynccrm = null;

        [IsModify("IsSyncCRM")]
        public bool IsModify_IsSyncCRM { get; set; }
        #endregion

        #region VisitType [访问分类]
        /// <summary>
        /// 访问分类
        /// </summary>
        [DBField("VisitType", SqlDbType.Int, 4)]
        public int? VisitType { get { return _visittype; } set { if (_visittype != value) { _visittype = value; IsModify_VisitType = true; } } }
        /// <summary>
        /// 访问分类
        /// </summary>
        public int VisitType_Value { get { return _visittype.HasValue ? _visittype.Value : 0; } }
        private int? _visittype = null;

        [IsModify("VisitType")]
        public bool IsModify_VisitType { get; set; }
        #endregion

        #region CBID [个人用户ID]
        /// <summary>
        /// 个人用户ID
        /// </summary>
        [DBField("CBID", SqlDbType.VarChar, 20)]
        public string CBID { get { return _cbid; } set { if (_cbid != value) { _cbid = value; IsModify_CBID = true; } } }
        /// <summary>
        /// 个人用户ID
        /// </summary>
        public string CBID_Value { get { return _cbid != null ? _cbid : ""; } }
        private string _cbid = null;

        [IsModify("CBID")]
        public bool IsModify_CBID { get; set; }
        #endregion

        #region Phone [绑定的电话号码]
        /// <summary>
        /// 绑定的电话号码
        /// </summary>
        [DBField("Phone", SqlDbType.VarChar, 20)]
        public string Phone { get { return _phone; } set { if (_phone != value) { _phone = value; IsModify_Phone = true; } } }
        /// <summary>
        /// 绑定的电话号码
        /// </summary>
        public string Phone_Value { get { return _phone != null ? _phone : ""; } }
        private string _phone = null;

        [IsModify("Phone")]
        public bool IsModify_Phone { get; set; }
        #endregion

        #region CRMCustID [客户回访的CRMCustID]
        /// <summary>
        /// 客户回访的CRMCustID
        /// </summary>
        [DBField("CRMCustID", SqlDbType.VarChar, 20)]
        public string CRMCustID { get { return _crmcustid; } set { if (_crmcustid != value) { _crmcustid = value; IsModify_CRMCustID = true; } } }
        /// <summary>
        /// 客户回访的CRMCustID
        /// </summary>
        public string CRMCustID_Value { get { return _crmcustid != null ? _crmcustid : ""; } }
        private string _crmcustid = null;

        [IsModify("CRMCustID")]
        public bool IsModify_CRMCustID { get; set; }
        #endregion

        #region Content [工单记录]
        /// <summary>
        /// 工单记录
        /// </summary>
        [DBField("Content", SqlDbType.NVarChar, 3000)]
        public string Content { get { return _content; } set { if (_content != value) { _content = value; IsModify_Content = true; } } }
        /// <summary>
        /// 工单记录
        /// </summary>
        public string Content_Value { get { return _content != null ? _content : ""; } }
        private string _content = null;

        [IsModify("Content")]
        public bool IsModify_Content { get; set; }
        #endregion

        #region ContactName [联系人姓名]
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [DBField("ContactName", SqlDbType.VarChar, 50)]
        public string ContactName { get { return _contactname; } set { if (_contactname != value) { _contactname = value; IsModify_ContactName = true; } } }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName_Value { get { return _contactname != null ? _contactname : ""; } }
        private string _contactname = null;

        [IsModify("ContactName")]
        public bool IsModify_ContactName { get; set; }
        #endregion

        #region ContactTel [联系人电话]
        /// <summary>
        /// 联系人电话
        /// </summary>
        [DBField("ContactTel", SqlDbType.VarChar, 50)]
        public string ContactTel { get { return _contacttel; } set { if (_contacttel != value) { _contacttel = value; IsModify_ContactTel = true; } } }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactTel_Value { get { return _contacttel != null ? _contacttel : ""; } }
        private string _contacttel = null;

        [IsModify("ContactTel")]
        public bool IsModify_ContactTel { get; set; }
        #endregion

        #region LastReceiverID [最后一次处理ID]
        /// <summary>
        /// 最后一次处理ID
        /// </summary>
        [DBField("LastReceiverID", SqlDbType.Int, 4)]
        public int? LastReceiverID { get { return _lastreceiverid; } set { if (_lastreceiverid != value) { _lastreceiverid = value; IsModify_LastReceiverID = true; } } }
        /// <summary>
        /// 最后一次处理ID
        /// </summary>
        public int LastReceiverID_Value { get { return _lastreceiverid.HasValue ? _lastreceiverid.Value : 0; } }
        private int? _lastreceiverid = null;

        [IsModify("LastReceiverID")]
        public bool IsModify_LastReceiverID { get; set; }
        #endregion

        #region BGID [创建人当时所在分组ID]
        /// <summary>
        /// 创建人当时所在分组ID
        /// </summary>
        [DBField("BGID", SqlDbType.Int, 4)]
        public int? BGID { get { return _bgid; } set { if (_bgid != value) { _bgid = value; IsModify_BGID = true; } } }
        /// <summary>
        /// 创建人当时所在分组ID
        /// </summary>
        public int BGID_Value { get { return _bgid.HasValue ? _bgid.Value : 0; } }
        private int? _bgid = null;

        [IsModify("BGID")]
        public bool IsModify_BGID { get; set; }
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

        #region LastUpdateUserID [最后更新人ID]
        /// <summary>
        /// 最后更新人ID
        /// </summary>
        [DBField("LastUpdateUserID", SqlDbType.Int, 4)]
        public int? LastUpdateUserID { get { return _lastupdateuserid; } set { if (_lastupdateuserid != value) { _lastupdateuserid = value; IsModify_LastUpdateUserID = true; } } }
        /// <summary>
        /// 最后更新人ID
        /// </summary>
        public int LastUpdateUserID_Value { get { return _lastupdateuserid.HasValue ? _lastupdateuserid.Value : 0; } }
        private int? _lastupdateuserid = null;

        [IsModify("LastUpdateUserID")]
        public bool IsModify_LastUpdateUserID { get; set; }
        #endregion

        #region LastUpdateTime [最后更新时间]
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DBField("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; IsModify_LastUpdateTime = true; } } }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime_Value { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private DateTime? _lastupdatetime = null;

        [IsModify("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get; set; }
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
            IsModify_CallSource = b;
            IsModify_ModuleSource = b;
            IsModify_DataSource = b;
            IsModify_WorkOrderStatus = b;
            IsModify_CategoryID = b;
            IsModify_ComplaintLevel = b;
            IsModify_BusinessType = b;
            IsModify_BusinessTag = b;
            IsModify_IsSyncCRM = b;
            IsModify_VisitType = b;
            IsModify_CBID = b;
            IsModify_Phone = b;
            IsModify_CRMCustID = b;
            IsModify_Content = b;
            IsModify_ContactName = b;
            IsModify_ContactTel = b;
            IsModify_LastReceiverID = b;
            IsModify_BGID = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
            IsModify_LastUpdateUserID = b;
            IsModify_LastUpdateTime = b;
        }

        #endregion

    }
}
