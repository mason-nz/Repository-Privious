using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类OtherTaskInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-03-20 03:24:41 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class OtherTaskInfo
	{
		public OtherTaskInfo()
		{
		 _ptid = Constant.STRING_INVALID_VALUE;
		 _projectid = Constant.INT_INVALID_VALUE;
		 _relationtableid = Constant.STRING_INVALID_VALUE;
		 _relationid = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastopttime = Constant.DATE_INVALID_VALUE;
		 _lastoptuserid = Constant.INT_INVALID_VALUE;
		 _taskstatus = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
         _custid = Constant.STRING_INVALID_VALUE;

         _CustNameTemp = Constant.STRING_INVALID_VALUE;
         _SexTemp = Constant.INT_INVALID_VALUE;
         _TelePhoneTemp = Constant.STRING_INVALID_VALUE;
         _DataTypeTemp = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private string _ptid;
		private long _projectid;
		private string _relationtableid;
		private string _relationid;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastopttime;
		private int? _lastoptuserid;
		private int? _taskstatus;
		private int? _status;
        private string _custid;

        private Guid guid;
        public Guid GUID
        {
            set { guid = value; }
            get { return guid; }
        }

        private string _CustNameTemp;
        public string CustNameTemp
        {
            set { _CustNameTemp = value; }
            get { return _CustNameTemp; }
        }
        private int? _SexTemp;
        public int? SexTemp
        {
            set { _SexTemp = value; }
            get { return _SexTemp; }
        }
        private string _TelePhoneTemp;
        public string TelePhoneTemp
        {
            set { _TelePhoneTemp = value; }
            get { return _TelePhoneTemp; }
        }
        private int? _DataTypeTemp;
        public int? DataTypeTemp
        {
            set { _DataTypeTemp = value; }
            get { return _DataTypeTemp; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string PTID
		{
			set{ _ptid=value;}
			get{return _ptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long ProjectID
		{
			set{ _projectid=value;}
			get{return _projectid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RelationTableID
		{
			set{ _relationtableid=value;}
			get{return _relationtableid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RelationID
		{
			set{ _relationid=value;}
			get{return _relationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreateUserID
		{
			set{ _createuserid=value;}
			get{return _createuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastOptTime
		{
			set{ _lastopttime=value;}
			get{return _lastopttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastOptUserID
		{
			set{ _lastoptuserid=value;}
			get{return _lastoptuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TaskStatus
		{
			set{ _taskstatus=value;}
			get{return _taskstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
		#endregion Model

	}
}

