using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// ʵ���ࣺWOrderInfoInfo [�����б�]-[����V2]-�������� 
    /// <summary>
    /// ʵ���ࣺWOrderInfoInfo [�����б�]-[����V2]-�������� 
    /// �Զ����ɣ�Copyright ?  2014 qiangfei Powered By [ADO�Զ����ɹ���] Version  1.2014.10.25��
    /// 2016-07-22
    /// </summary>
    [DBTableAttribute("WOrderInfo")]
    public class WOrderInfoInfo
    {
        #region ���췽��
        /// ���췽�� (�Զ�����)
        /// <summary>
        /// ���췽�� (�Զ�����)
        /// </summary>
        public WOrderInfoInfo()
        {
        }


        /// ���췽�����������������У�(�Զ�����)
        /// <summary>
        /// ���췽�����������������У�(�Զ�����)
        /// </summary>
        public WOrderInfoInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// ���췽������������ת���� (�Զ�����)
        /// <summary>
        /// ���췽������������ת���� (�Զ�����)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderInfoInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region ����
        #region RecID [��������]
        /// <summary>
        /// ��������
        /// </summary>
        [DBField("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        /// <summary>
        /// ��������
        /// </summary>
        public int RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private int? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region OrderID [����ID]
        /// <summary>
        /// ����ID
        /// </summary>
        [DBField("OrderID", SqlDbType.VarChar, 20)]
        public string OrderID { get { return _orderid; } set { if (_orderid != value) { _orderid = value; IsModify_OrderID = true; } } }
        /// <summary>
        /// ����ID
        /// </summary>
        public string OrderID_Value { get { return _orderid != null ? _orderid : ""; } }
        private string _orderid = null;

        [IsModify("OrderID")]
        public bool IsModify_OrderID { get; set; }
        #endregion

        #region CallSource [ͨ����Դ��0=�ޣ�1=���룬2=������3= IM]
        /// <summary>
        /// ͨ����Դ��0=�ޣ�1=���룬2=������3= IM
        /// </summary>
        [DBField("CallSource", SqlDbType.Int, 4)]
        public int? CallSource { get { return _callsource; } set { if (_callsource != value) { _callsource = value; IsModify_CallSource = true; } } }
        /// <summary>
        /// ͨ����Դ��0=�ޣ�1=���룬2=������3= IM
        /// </summary>
        public int CallSource_Value { get { return _callsource.HasValue ? _callsource.Value : 0; } }
        private int? _callsource = null;

        [IsModify("CallSource")]
        public bool IsModify_CallSource { get; set; }
        #endregion

        #region ModuleSource [������Դ��1=�ͻ��أ�2=������3=�طã�4=δ�����磬5=IM�����̣�6=IM����]
        /// <summary>
        /// ������Դ��1=�ͻ��أ�2=������3=�طã�4=δ�����磬5=IM�����̣�6=IM����
        /// </summary>
        [DBField("ModuleSource", SqlDbType.Int, 4)]
        public int? ModuleSource { get { return _modulesource; } set { if (_modulesource != value) { _modulesource = value; IsModify_ModuleSource = true; } } }
        /// <summary>
        /// ������Դ��1=�ͻ��أ�2=������3=�طã�4=δ�����磬5=IM�����̣�6=IM����
        /// </summary>
        public int ModuleSource_Value { get { return _modulesource.HasValue ? _modulesource.Value : 0; } }
        private int? _modulesource = null;

        [IsModify("ModuleSource")]
        public bool IsModify_ModuleSource { get; set; }
        #endregion

        #region DataSource [ҵ������Դ]
        /// <summary>
        /// ҵ������Դ
        /// </summary>
        [DBField("DataSource", SqlDbType.Int, 4)]
        public int? DataSource { get { return _datasource; } set { if (_datasource != value) { _datasource = value; IsModify_DataSource = true; } } }
        /// <summary>
        /// ҵ������Դ
        /// </summary>
        public int DataSource_Value { get { return _datasource.HasValue ? _datasource.Value : 0; } }
        private int? _datasource = null;

        [IsModify("DataSource")]
        public bool IsModify_DataSource { get; set; }
        #endregion

        #region WorkOrderStatus [����״̬�������=1��������=2��������=3���Ѵ���=4�������=5���ѹر�=6]
        /// <summary>
        /// ����״̬�������=1��������=2��������=3���Ѵ���=4�������=5���ѹر�=6
        /// </summary>
        [DBField("WorkOrderStatus", SqlDbType.Int, 4)]
        public int? WorkOrderStatus { get { return _workorderstatus; } set { if (_workorderstatus != value) { _workorderstatus = value; IsModify_WorkOrderStatus = true; } } }
        /// <summary>
        /// ����״̬�������=1��������=2��������=3���Ѵ���=4�������=5���ѹر�=6
        /// </summary>
        public int WorkOrderStatus_Value { get { return _workorderstatus.HasValue ? _workorderstatus.Value : 0; } }
        private int? _workorderstatus = null;

        [IsModify("WorkOrderStatus")]
        public bool IsModify_WorkOrderStatus { get; set; }
        #endregion

        #region CategoryID [��������ID�����⴦��=1����ѯ=2��Ͷ��=3���ط�=4������=5������=6������=7]
        /// <summary>
        /// ��������ID�����⴦��=1����ѯ=2��Ͷ��=3���ط�=4������=5������=6������=7
        /// </summary>
        [DBField("CategoryID", SqlDbType.Int, 4)]
        public int? CategoryID { get { return _categoryid; } set { if (_categoryid != value) { _categoryid = value; IsModify_CategoryID = true; } } }
        /// <summary>
        /// ��������ID�����⴦��=1����ѯ=2��Ͷ��=3���ط�=4������=5������=6������=7
        /// </summary>
        public int CategoryID_Value { get { return _categoryid.HasValue ? _categoryid.Value : 0; } }
        private int? _categoryid = null;

        [IsModify("CategoryID")]
        public bool IsModify_CategoryID { get; set; }
        #endregion

        #region ComplaintLevel [Ͷ�߼���A��=1��B��=2������=3]
        /// <summary>
        /// Ͷ�߼���A��=1��B��=2������=3
        /// </summary>
        [DBField("ComplaintLevel", SqlDbType.Int, 4)]
        public int? ComplaintLevel { get { return _complaintlevel; } set { if (_complaintlevel != value) { _complaintlevel = value; IsModify_ComplaintLevel = true; } } }
        /// <summary>
        /// Ͷ�߼���A��=1��B��=2������=3
        /// </summary>
        public int ComplaintLevel_Value { get { return _complaintlevel.HasValue ? _complaintlevel.Value : 0; } }
        private int? _complaintlevel = null;

        [IsModify("ComplaintLevel")]
        public bool IsModify_ComplaintLevel { get; set; }
        #endregion

        #region BusinessType [ҵ������]
        /// <summary>
        /// ҵ������
        /// </summary>
        [DBField("BusinessType", SqlDbType.Int, 4)]
        public int? BusinessType { get { return _businesstype; } set { if (_businesstype != value) { _businesstype = value; IsModify_BusinessType = true; } } }
        /// <summary>
        /// ҵ������
        /// </summary>
        public int BusinessType_Value { get { return _businesstype.HasValue ? _businesstype.Value : 0; } }
        private int? _businesstype = null;

        [IsModify("BusinessType")]
        public bool IsModify_BusinessType { get; set; }
        #endregion

        #region BusinessTag [ҵ���ǩ]
        /// <summary>
        /// ҵ���ǩ
        /// </summary>
        [DBField("BusinessTag", SqlDbType.Int, 4)]
        public int? BusinessTag { get { return _businesstag; } set { if (_businesstag != value) { _businesstag = value; IsModify_BusinessTag = true; } } }
        /// <summary>
        /// ҵ���ǩ
        /// </summary>
        public int BusinessTag_Value { get { return _businesstag.HasValue ? _businesstag.Value : 0; } }
        private int? _businesstag = null;

        [IsModify("BusinessTag")]
        public bool IsModify_BusinessTag { get; set; }
        #endregion

        #region IsSyncCRM [�Ƿ���Ҫͬ�������ʼ�¼��1=�� 0=��]
        /// <summary>
        /// �Ƿ���Ҫͬ�������ʼ�¼��1=�� 0=��
        /// </summary>
        [DBField("IsSyncCRM", SqlDbType.Int, 4)]
        public int? IsSyncCRM { get { return _issynccrm; } set { if (_issynccrm != value) { _issynccrm = value; IsModify_IsSyncCRM = true; } } }
        /// <summary>
        /// �Ƿ���Ҫͬ�������ʼ�¼��1=�� 0=��
        /// </summary>
        public int IsSyncCRM_Value { get { return _issynccrm.HasValue ? _issynccrm.Value : 0; } }
        private int? _issynccrm = null;

        [IsModify("IsSyncCRM")]
        public bool IsModify_IsSyncCRM { get; set; }
        #endregion

        #region VisitType [���ʷ���]
        /// <summary>
        /// ���ʷ���
        /// </summary>
        [DBField("VisitType", SqlDbType.Int, 4)]
        public int? VisitType { get { return _visittype; } set { if (_visittype != value) { _visittype = value; IsModify_VisitType = true; } } }
        /// <summary>
        /// ���ʷ���
        /// </summary>
        public int VisitType_Value { get { return _visittype.HasValue ? _visittype.Value : 0; } }
        private int? _visittype = null;

        [IsModify("VisitType")]
        public bool IsModify_VisitType { get; set; }
        #endregion

        #region CBID [�����û�ID]
        /// <summary>
        /// �����û�ID
        /// </summary>
        [DBField("CBID", SqlDbType.VarChar, 20)]
        public string CBID { get { return _cbid; } set { if (_cbid != value) { _cbid = value; IsModify_CBID = true; } } }
        /// <summary>
        /// �����û�ID
        /// </summary>
        public string CBID_Value { get { return _cbid != null ? _cbid : ""; } }
        private string _cbid = null;

        [IsModify("CBID")]
        public bool IsModify_CBID { get; set; }
        #endregion

        #region Phone [�󶨵ĵ绰����]
        /// <summary>
        /// �󶨵ĵ绰����
        /// </summary>
        [DBField("Phone", SqlDbType.VarChar, 20)]
        public string Phone { get { return _phone; } set { if (_phone != value) { _phone = value; IsModify_Phone = true; } } }
        /// <summary>
        /// �󶨵ĵ绰����
        /// </summary>
        public string Phone_Value { get { return _phone != null ? _phone : ""; } }
        private string _phone = null;

        [IsModify("Phone")]
        public bool IsModify_Phone { get; set; }
        #endregion

        #region CRMCustID [�ͻ��طõ�CRMCustID]
        /// <summary>
        /// �ͻ��طõ�CRMCustID
        /// </summary>
        [DBField("CRMCustID", SqlDbType.VarChar, 20)]
        public string CRMCustID { get { return _crmcustid; } set { if (_crmcustid != value) { _crmcustid = value; IsModify_CRMCustID = true; } } }
        /// <summary>
        /// �ͻ��طõ�CRMCustID
        /// </summary>
        public string CRMCustID_Value { get { return _crmcustid != null ? _crmcustid : ""; } }
        private string _crmcustid = null;

        [IsModify("CRMCustID")]
        public bool IsModify_CRMCustID { get; set; }
        #endregion

        #region Content [������¼]
        /// <summary>
        /// ������¼
        /// </summary>
        [DBField("Content", SqlDbType.NVarChar, 3000)]
        public string Content { get { return _content; } set { if (_content != value) { _content = value; IsModify_Content = true; } } }
        /// <summary>
        /// ������¼
        /// </summary>
        public string Content_Value { get { return _content != null ? _content : ""; } }
        private string _content = null;

        [IsModify("Content")]
        public bool IsModify_Content { get; set; }
        #endregion

        #region ContactName [��ϵ������]
        /// <summary>
        /// ��ϵ������
        /// </summary>
        [DBField("ContactName", SqlDbType.VarChar, 50)]
        public string ContactName { get { return _contactname; } set { if (_contactname != value) { _contactname = value; IsModify_ContactName = true; } } }
        /// <summary>
        /// ��ϵ������
        /// </summary>
        public string ContactName_Value { get { return _contactname != null ? _contactname : ""; } }
        private string _contactname = null;

        [IsModify("ContactName")]
        public bool IsModify_ContactName { get; set; }
        #endregion

        #region ContactTel [��ϵ�˵绰]
        /// <summary>
        /// ��ϵ�˵绰
        /// </summary>
        [DBField("ContactTel", SqlDbType.VarChar, 50)]
        public string ContactTel { get { return _contacttel; } set { if (_contacttel != value) { _contacttel = value; IsModify_ContactTel = true; } } }
        /// <summary>
        /// ��ϵ�˵绰
        /// </summary>
        public string ContactTel_Value { get { return _contacttel != null ? _contacttel : ""; } }
        private string _contacttel = null;

        [IsModify("ContactTel")]
        public bool IsModify_ContactTel { get; set; }
        #endregion

        #region LastReceiverID [���һ�δ���ID]
        /// <summary>
        /// ���һ�δ���ID
        /// </summary>
        [DBField("LastReceiverID", SqlDbType.Int, 4)]
        public int? LastReceiverID { get { return _lastreceiverid; } set { if (_lastreceiverid != value) { _lastreceiverid = value; IsModify_LastReceiverID = true; } } }
        /// <summary>
        /// ���һ�δ���ID
        /// </summary>
        public int LastReceiverID_Value { get { return _lastreceiverid.HasValue ? _lastreceiverid.Value : 0; } }
        private int? _lastreceiverid = null;

        [IsModify("LastReceiverID")]
        public bool IsModify_LastReceiverID { get; set; }
        #endregion

        #region BGID [�����˵�ʱ���ڷ���ID]
        /// <summary>
        /// �����˵�ʱ���ڷ���ID
        /// </summary>
        [DBField("BGID", SqlDbType.Int, 4)]
        public int? BGID { get { return _bgid; } set { if (_bgid != value) { _bgid = value; IsModify_BGID = true; } } }
        /// <summary>
        /// �����˵�ʱ���ڷ���ID
        /// </summary>
        public int BGID_Value { get { return _bgid.HasValue ? _bgid.Value : 0; } }
        private int? _bgid = null;

        [IsModify("BGID")]
        public bool IsModify_BGID { get; set; }
        #endregion

        #region Status [״̬]
        /// <summary>
        /// ״̬
        /// </summary>
        [DBField("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; IsModify_Status = true; } } }
        /// <summary>
        /// ״̬
        /// </summary>
        public int Status_Value { get { return _status.HasValue ? _status.Value : 0; } }
        private int? _status = null;

        [IsModify("Status")]
        public bool IsModify_Status { get; set; }
        #endregion

        #region CreateUserID [������ID]
        /// <summary>
        /// ������ID
        /// </summary>
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        /// <summary>
        /// ������ID
        /// </summary>
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion

        #region CreateTime [����ʱ��]
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [DBField("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; IsModify_CreateTime = true; } } }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime_Value { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private DateTime? _createtime = null;

        [IsModify("CreateTime")]
        public bool IsModify_CreateTime { get; set; }
        #endregion

        #region LastUpdateUserID [��������ID]
        /// <summary>
        /// ��������ID
        /// </summary>
        [DBField("LastUpdateUserID", SqlDbType.Int, 4)]
        public int? LastUpdateUserID { get { return _lastupdateuserid; } set { if (_lastupdateuserid != value) { _lastupdateuserid = value; IsModify_LastUpdateUserID = true; } } }
        /// <summary>
        /// ��������ID
        /// </summary>
        public int LastUpdateUserID_Value { get { return _lastupdateuserid.HasValue ? _lastupdateuserid.Value : 0; } }
        private int? _lastupdateuserid = null;

        [IsModify("LastUpdateUserID")]
        public bool IsModify_LastUpdateUserID { get; set; }
        #endregion

        #region LastUpdateTime [������ʱ��]
        /// <summary>
        /// ������ʱ��
        /// </summary>
        [DBField("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; IsModify_LastUpdateTime = true; } } }
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public DateTime LastUpdateTime_Value { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private DateTime? _lastupdatetime = null;

        [IsModify("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get; set; }
        #endregion

        #endregion

        #region ����
        /// �����Ƿ���������ֶ� (�Զ�����)
        /// <summary>
        /// �����Ƿ���������ֶ� (�Զ�����)
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
