using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// ʵ���ࣺWOrderToAndCCInfo [�����б�]-[����V2]-���������˺ͳ����˱� 
    /// <summary>
    /// ʵ���ࣺWOrderToAndCCInfo [�����б�]-[����V2]-���������˺ͳ����˱� 
    /// �Զ����ɣ�Copyright ?  2014 qiangfei Powered By [ADO�Զ����ɹ���] Version  1.2014.10.25��
    /// 2016-07-20
    /// </summary>
    [DBTableAttribute("WOrderToAndCC")]
    public class WOrderToAndCCInfo
    {
        #region ���췽��
        /// ���췽�� (�Զ�����)
        /// <summary>
        /// ���췽�� (�Զ�����)
        /// </summary>
        public WOrderToAndCCInfo()
        {
        }


        /// ���췽�����������������У�(�Զ�����)
        /// <summary>
        /// ���췽�����������������У�(�Զ�����)
        /// </summary>
        public WOrderToAndCCInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// ���췽������������ת���� (�Զ�����)
        /// <summary>
        /// ���췽������������ת���� (�Զ�����)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderToAndCCInfo(DataRow dr)
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

        #region ReceiverID [�����Ļظ�ID��������Ϊ�գ�]
        /// <summary>
        /// �����Ļظ�ID��������Ϊ�գ�
        /// </summary>
        [DBField("ReceiverID", SqlDbType.Int, 4)]
        public int? ReceiverID { get { return _receiverid; } set { if (_receiverid != value) { _receiverid = value; IsModify_ReceiverID = true; } } }
        /// <summary>
        /// �����Ļظ�ID��������Ϊ�գ�
        /// </summary>
        public int ReceiverID_Value { get { return _receiverid.HasValue ? _receiverid.Value : 0; } }
        private int? _receiverid = null;

        [IsModify("ReceiverID")]
        public bool IsModify_ReceiverID { get; set; }
        #endregion

        #region PersonType [��Ա���ͣ�������=1��������=2]
        /// <summary>
        /// ��Ա���ͣ�������=1��������=2
        /// </summary>
        [DBField("PersonType", SqlDbType.Int, 4)]
        public int? PersonType { get { return _persontype; } set { if (_persontype != value) { _persontype = value; IsModify_PersonType = true; } } }
        /// <summary>
        /// ��Ա���ͣ�������=1��������=2
        /// </summary>
        public int PersonType_Value { get { return _persontype.HasValue ? _persontype.Value : 0; } }
        private int? _persontype = null;

        [IsModify("PersonType")]
        public bool IsModify_PersonType { get; set; }
        #endregion

        #region UserNum [�����˻����˵�Ա�����]
        /// <summary>
        /// �����˻����˵�Ա�����
        /// </summary>
        [DBField("UserNum", SqlDbType.VarChar, 20)]
        public string UserNum { get { return _usernum; } set { if (_usernum != value) { _usernum = value; IsModify_UserNum = true; } } }
        /// <summary>
        /// �����˻����˵�Ա�����
        /// </summary>
        public string UserNum_Value { get { return _usernum != null ? _usernum : ""; } }
        private string _usernum = null;

        [IsModify("UserNum")]
        public bool IsModify_UserNum { get; set; }
        #endregion

        #region UserID [��ԱID]
        /// <summary>
        /// ��ԱID
        /// </summary>
        [DBField("UserID", SqlDbType.Int, 4)]
        public int? UserID { get { return _userid; } set { if (_userid != value) { _userid = value; IsModify_UserID = true; } } }
        /// <summary>
        /// ��ԱID
        /// </summary>
        public int UserID_Value { get { return _userid.HasValue ? _userid.Value : 0; } }
        private int? _userid = null;

        [IsModify("UserID")]
        public bool IsModify_UserID { get; set; }
        #endregion

        #region UserName [�����˻����˵���ʵ����]
        /// <summary>
        /// �����˻����˵���ʵ����
        /// </summary>
        [DBField("UserName", SqlDbType.VarChar, 50)]
        public string UserName { get { return _username; } set { if (_username != value) { _username = value; IsModify_UserName = true; } } }
        /// <summary>
        /// �����˻����˵���ʵ����
        /// </summary>
        public string UserName_Value { get { return _username != null ? _username : ""; } }
        private string _username = null;

        [IsModify("UserName")]
        public bool IsModify_UserName { get; set; }
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
