using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// ʵ���ࣺWOrderDataInfo [�����б�]-[����V2]-�������ݹ����� 
    /// <summary>
    /// ʵ���ࣺWOrderDataInfo [�����б�]-[����V2]-�������ݹ����� 
    /// �Զ����ɣ�Copyright ?  2014 qiangfei Powered By [ADO�Զ����ɹ���] Version  1.2014.10.25��
    /// 2016-07-19
    /// </summary>
    [DBTableAttribute("WOrderData")]
    public class WOrderDataInfo
    {
        #region ���췽��
        /// ���췽�� (�Զ�����)
        /// <summary>
        /// ���췽�� (�Զ�����)
        /// </summary>
        public WOrderDataInfo()
        {
        }


        /// ���췽�����������������У�(�Զ�����)
        /// <summary>
        /// ���췽�����������������У�(�Զ�����)
        /// </summary>
        public WOrderDataInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// ���췽������������ת���� (�Զ�����)
        /// <summary>
        /// ���췽������������ת���� (�Զ�����)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderDataInfo(DataRow dr)
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

        #region ReceiverID [�ظ�ID���ǹ���������������ʱ��ֵ=-1]
        /// <summary>
        /// �ظ�ID���ǹ���������������ʱ��ֵ=-1
        /// </summary>
        [DBField("ReceiverID", SqlDbType.Int, 4)]
        public int? ReceiverID { get { return _receiverid; } set { if (_receiverid != value) { _receiverid = value; IsModify_ReceiverID = true; } } }
        /// <summary>
        /// �ظ�ID���ǹ���������������ʱ��ֵ=-1
        /// </summary>
        public int ReceiverID_Value { get { return _receiverid.HasValue ? _receiverid.Value : 0; } }
        private int? _receiverid = null;

        [IsModify("ReceiverID")]
        public bool IsModify_ReceiverID { get; set; }
        #endregion

        #region DataType [��������1=���� 2=�Ի�]
        /// <summary>
        /// ��������1=���� 2=�Ի�
        /// </summary>
        [DBField("DataType", SqlDbType.Int, 4)]
        public int? DataType { get { return _datatype; } set { if (_datatype != value) { _datatype = value; IsModify_DataType = true; } } }
        /// <summary>
        /// ��������1=���� 2=�Ի�
        /// </summary>
        public int DataType_Value { get { return _datatype.HasValue ? _datatype.Value : 0; } }
        private int? _datatype = null;

        [IsModify("DataType")]
        public bool IsModify_DataType { get; set; }
        #endregion

        #region DataID [����ID/�Ի�ID ������ж���ֻ��¼���һ����]
        /// <summary>
        /// ����ID/�Ի�ID ������ж���ֻ��¼���һ����
        /// </summary>
        [DBField("DataID", SqlDbType.BigInt, 8)]
        public long? DataID { get { return _dataid; } set { if (_dataid != value) { _dataid = value; IsModify_DataID = true; } } }
        /// <summary>
        /// ����ID/�Ի�ID ������ж���ֻ��¼���һ����
        /// </summary>
        public long DataID_Value { get { return _dataid.HasValue ? _dataid.Value : 0; } }
        private long? _dataid = null;

        [IsModify("DataID")]
        public bool IsModify_DataID { get; set; }
        #endregion

        #region StartTime [��ͨʱ��/��ʼʱ��]
        /// <summary>
        /// ��ͨʱ��/��ʼʱ��
        /// </summary>
        [DBField("StartTime", SqlDbType.DateTime, 8)]
        public DateTime? StartTime { get { return _starttime; } set { if (_starttime != value) { _starttime = value; IsModify_StartTime = true; } } }
        /// <summary>
        /// ��ͨʱ��/��ʼʱ��
        /// </summary>
        public DateTime StartTime_Value { get { return _starttime.HasValue ? _starttime.Value : new DateTime(); } }
        private DateTime? _starttime = null;

        [IsModify("StartTime")]
        public bool IsModify_StartTime { get; set; }
        #endregion

        #region EndTime [�Ҷ�ʱ��/����ʱ��]
        /// <summary>
        /// �Ҷ�ʱ��/����ʱ��
        /// </summary>
        [DBField("EndTime", SqlDbType.DateTime, 8)]
        public DateTime? EndTime { get { return _endtime; } set { if (_endtime != value) { _endtime = value; IsModify_EndTime = true; } } }
        /// <summary>
        /// �Ҷ�ʱ��/����ʱ��
        /// </summary>
        public DateTime EndTime_Value { get { return _endtime.HasValue ? _endtime.Value : new DateTime(); } }
        private DateTime? _endtime = null;

        [IsModify("EndTime")]
        public bool IsModify_EndTime { get; set; }
        #endregion

        #region TallTime [ͨ��ʱ��/�Ի�ʱ��]
        /// <summary>
        /// ͨ��ʱ��/�Ի�ʱ��
        /// </summary>
        [DBField("TallTime", SqlDbType.Int, 4)]
        public int? TallTime { get { return _talltime; } set { if (_talltime != value) { _talltime = value; IsModify_TallTime = true; } } }
        /// <summary>
        /// ͨ��ʱ��/�Ի�ʱ��
        /// </summary>
        public int TallTime_Value { get { return _talltime.HasValue ? _talltime.Value : 0; } }
        private int? _talltime = null;

        [IsModify("TallTime")]
        public bool IsModify_TallTime { get; set; }
        #endregion

        #region AudioURL [¼����ַ]
        /// <summary>
        /// ¼����ַ
        /// </summary>
        [DBField("AudioURL", SqlDbType.VarChar, 500)]
        public string AudioURL { get { return _audiourl; } set { if (_audiourl != value) { _audiourl = value; IsModify_AudioURL = true; } } }
        /// <summary>
        /// ¼����ַ
        /// </summary>
        public string AudioURL_Value { get { return _audiourl != null ? _audiourl : ""; } }
        private string _audiourl = null;

        [IsModify("AudioURL")]
        public bool IsModify_AudioURL { get; set; }
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
            IsModify_DataType = b;
            IsModify_DataID = b;
            IsModify_StartTime = b;
            IsModify_EndTime = b;
            IsModify_TallTime = b;
            IsModify_AudioURL = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
        }

        #endregion

    }
}
