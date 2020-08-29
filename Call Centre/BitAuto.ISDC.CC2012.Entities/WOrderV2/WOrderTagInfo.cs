using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// ʵ���ࣺWOrderTagInfo [�����б�]-[����V2]-����������ǩ�� 
    /// <summary>
    /// ʵ���ࣺWOrderTagInfo [�����б�]-[����V2]-����������ǩ�� 
    /// �Զ����ɣ�Copyright ?  2014 qiangfei Powered By [ADO�Զ����ɹ���] Version  1.2014.10.25��
    /// 2016-07-19
    /// </summary>
    [DBTableAttribute("WOrderTag")]
    public class WOrderTagInfo
    {
        #region ���췽��
        /// ���췽�� (�Զ�����)
        /// <summary>
        /// ���췽�� (�Զ�����)
        /// </summary>
        public WOrderTagInfo()
        {
        }


        /// ���췽�����������������У�(�Զ�����)
        /// <summary>
        /// ���췽�����������������У�(�Զ�����)
        /// </summary>
        public WOrderTagInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// ���췽������������ת���� (�Զ�����)
        /// <summary>
        /// ���췽������������ת���� (�Զ�����)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderTagInfo(DataRow dr)
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

        #region BusiTypeID [����ҵ������ID]
        /// <summary>
        /// ����ҵ������ID
        /// </summary>
        [DBField("BusiTypeID", SqlDbType.Int, 4)]
        public int? BusiTypeID { get { return _busitypeid; } set { if (_busitypeid != value) { _busitypeid = value; IsModify_BusiTypeID = true; } } }
        /// <summary>
        /// ����ҵ������ID
        /// </summary>
        public int BusiTypeID_Value { get { return _busitypeid.HasValue ? _busitypeid.Value : 0; } }
        private int? _busitypeid = null;

        [IsModify("BusiTypeID")]
        public bool IsModify_BusiTypeID { get; set; }
        #endregion

        #region TagName [��ǩ����]
        /// <summary>
        /// ��ǩ����
        /// </summary>
        [DBField("TagName", SqlDbType.VarChar, 20)]
        public string TagName { get { return _tagname; } set { if (_tagname != value) { _tagname = value; IsModify_TagName = true; } } }
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public string TagName_Value { get { return _tagname != null ? _tagname : ""; } }
        private string _tagname = null;

        [IsModify("TagName")]
        public bool IsModify_TagName { get; set; }
        #endregion

        #region PID [������ǩID��-1�����һ����ǩ��]
        /// <summary>
        /// ������ǩID��-1�����һ����ǩ��
        /// </summary>
        [DBField("PID", SqlDbType.Int, 4)]
        public int? PID { get { return _pid; } set { if (_pid != value) { _pid = value; IsModify_PID = true; } } }
        /// <summary>
        /// ������ǩID��-1�����һ����ǩ��
        /// </summary>
        public int PID_Value { get { return _pid.HasValue ? _pid.Value : 0; } }
        private int? _pid = null;

        [IsModify("PID")]
        public bool IsModify_PID { get; set; }
        #endregion

        #region SortNum [˳��]
        /// <summary>
        /// ˳��
        /// </summary>
        [DBField("SortNum", SqlDbType.Int, 4)]
        public int? SortNum { get { return _sortnum; } set { if (_sortnum != value) { _sortnum = value; IsModify_SortNum = true; } } }
        /// <summary>
        /// ˳��
        /// </summary>
        public int SortNum_Value { get { return _sortnum.HasValue ? _sortnum.Value : 0; } }
        private int? _sortnum = null;

        [IsModify("SortNum")]
        public bool IsModify_SortNum { get; set; }
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
            IsModify_BusiTypeID = b;
            IsModify_TagName = b;
            IsModify_PID = b;
            IsModify_SortNum = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
            IsModify_LastUpdateUserID = b;
            IsModify_LastUpdateTime = b;
        }

        #endregion

    }
}
