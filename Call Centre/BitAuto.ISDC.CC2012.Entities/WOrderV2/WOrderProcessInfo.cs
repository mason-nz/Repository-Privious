using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace BitAuto.ISDC.CC2012.Entities
{
    /// ʵ���ࣺWOrderProcessInfo [�����б�]-[����V2]-��������� 
    /// <summary>
    /// ʵ���ࣺWOrderProcessInfo [�����б�]-[����V2]-��������� 
    /// �Զ����ɣ�Copyright ?  2014 qiangfei Powered By [ADO�Զ����ɹ���] Version  1.2014.10.25��
    /// 2016-07-20
    /// </summary>
    [DBTableAttribute("WOrderProcess")]
    public class WOrderProcessInfo
    {
        #region ���췽��
        /// ���췽�� (�Զ�����)
        /// <summary>
        /// ���췽�� (�Զ�����)
        /// </summary>
        public WOrderProcessInfo()
        {
        }


        /// ���췽�����������������У�(�Զ�����)
        /// <summary>
        /// ���췽�����������������У�(�Զ�����)
        /// </summary>
        public WOrderProcessInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// ���췽������������ת���� (�Զ�����)
        /// <summary>
        /// ���췽������������ת���� (�Զ�����)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderProcessInfo(DataRow dr)
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

        #region ProcessType [�������ͣ� ���=1��ת��=2������=3���ط�=4]
        /// <summary>
        /// �������ͣ� ���=1��ת��=2������=3���ط�=4
        /// </summary>
        [DBField("ProcessType", SqlDbType.Int, 4)]
        public int? ProcessType { get { return _processtype; } set { if (_processtype != value) { _processtype = value; IsModify_ProcessType = true; } } }
        /// <summary>
        /// �������ͣ� ���=1��ת��=2������=3���ط�=4
        /// </summary>
        public int ProcessType_Value { get { return _processtype.HasValue ? _processtype.Value : 0; } }
        private int? _processtype = null;

        [IsModify("ProcessType")]
        public bool IsModify_ProcessType { get; set; }
        #endregion

        #region WorkOrderStatus [��ʱ�л���Ĺ���״̬�������=1��������=2��������=3���Ѵ���=4�������=5���ѹر�=6]
        /// <summary>
        /// ��ʱ�л���Ĺ���״̬�������=1��������=2��������=3���Ѵ���=4�������=5���ѹر�=6
        /// </summary>
        [DBField("WorkOrderStatus", SqlDbType.Int, 4)]
        public int? WorkOrderStatus { get { return _workorderstatus; } set { if (_workorderstatus != value) { _workorderstatus = value; IsModify_WorkOrderStatus = true; } } }
        /// <summary>
        /// ��ʱ�л���Ĺ���״̬�������=1��������=2��������=3���Ѵ���=4�������=5���ѹر�=6
        /// </summary>
        public int WorkOrderStatus_Value { get { return _workorderstatus.HasValue ? _workorderstatus.Value : 0; } }
        private int? _workorderstatus = null;

        [IsModify("WorkOrderStatus")]
        public bool IsModify_WorkOrderStatus { get; set; }
        #endregion

        #region IsReturnVisit [�Ƿ���Ҫ�طã�1=�� 0=��-1=��Чֵ]
        /// <summary>
        /// �Ƿ���Ҫ�طã�1=�� 0=��-1=��Чֵ
        /// </summary>
        [DBField("IsReturnVisit", SqlDbType.Int, 4)]
        public int? IsReturnVisit { get { return _isreturnvisit; } set { if (_isreturnvisit != value) { _isreturnvisit = value; IsModify_IsReturnVisit = true; } } }
        /// <summary>
        /// �Ƿ���Ҫ�طã�1=�� 0=��-1=��Чֵ
        /// </summary>
        public int IsReturnVisit_Value { get { return _isreturnvisit.HasValue ? _isreturnvisit.Value : 0; } }
        private int? _isreturnvisit = null;

        [IsModify("IsReturnVisit")]
        public bool IsModify_IsReturnVisit { get; set; }
        #endregion

        #region ProcessContent [�������ݣ�����Ҫ�طõ����ɣ����طõĽ����]
        /// <summary>
        /// �������ݣ�����Ҫ�طõ����ɣ����طõĽ����
        /// </summary>
        [DBField("ProcessContent", SqlDbType.NVarChar, 3000)]
        public string ProcessContent { get { return _processcontent; } set { if (_processcontent != value) { _processcontent = value; IsModify_ProcessContent = true; } } }
        /// <summary>
        /// �������ݣ�����Ҫ�طõ����ɣ����طõĽ����
        /// </summary>
        public string ProcessContent_Value { get { return _processcontent != null ? _processcontent : ""; } }
        private string _processcontent = null;

        [IsModify("ProcessContent")]
        public bool IsModify_ProcessContent { get; set; }
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

        #region CreateUserNum [������Ա�����]
        /// <summary>
        /// ������Ա�����
        /// </summary>
        [DBField("CreateUserNum", SqlDbType.VarChar, 20)]
        public string CreateUserNum { get { return _createusernum; } set { if (_createusernum != value) { _createusernum = value; IsModify_CreateUserNum = true; } } }
        /// <summary>
        /// ������Ա�����
        /// </summary>
        public string CreateUserNum_Value { get { return _createusernum != null ? _createusernum : ""; } }
        private string _createusernum = null;

        [IsModify("CreateUserNum")]
        public bool IsModify_CreateUserNum { get; set; }
        #endregion

        #region CreateUserName [������Ա����ʵ����]
        /// <summary>
        /// ������Ա����ʵ����
        /// </summary>
        [DBField("CreateUserName", SqlDbType.VarChar, 50)]
        public string CreateUserName { get { return _createusername; } set { if (_createusername != value) { _createusername = value; IsModify_CreateUserName = true; } } }
        /// <summary>
        /// ������Ա����ʵ����
        /// </summary>
        public string CreateUserName_Value { get { return _createusername != null ? _createusername : ""; } }
        private string _createusername = null;

        [IsModify("CreateUserName")]
        public bool IsModify_CreateUserName { get; set; }
        #endregion

        #region CreateUserDeptName [�����˵�ʱ���ڶ�����������]
        /// <summary>
        /// �����˵�ʱ���ڶ�����������
        /// </summary>
        [DBField("CreateUserDeptName", SqlDbType.VarChar, 50)]
        public string CreateUserDeptName { get { return _createuserdeptname; } set { if (_createuserdeptname != value) { _createuserdeptname = value; IsModify_CreateUserDeptName = true; } } }
        /// <summary>
        /// �����˵�ʱ���ڶ�����������
        /// </summary>
        public string CreateUserDeptName_Value { get { return _createuserdeptname != null ? _createuserdeptname : ""; } }
        private string _createuserdeptname = null;

        [IsModify("CreateUserDeptName")]
        public bool IsModify_CreateUserDeptName { get; set; }
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

        #region  ���������ݿ��ֶ�
        /// <summary>
        /// ��Ŵ����¼״̬ ����
        /// </summary>
        public string StatusStr { get; set; }
        #endregion
    }
}
