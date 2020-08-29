using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_Cust_Contact 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_Cust_Contact
	{
		public ProjectTask_Cust_Contact()
		{
		 _id = Constant.INT_INVALID_VALUE;
		 _ptid = Constant.STRING_INVALID_VALUE;
		 _originalcontactid = Constant.INT_INVALID_VALUE;
		 _pid = Constant.INT_INVALID_VALUE;
		 _cname = Constant.STRING_INVALID_VALUE;
		 _ename = Constant.STRING_INVALID_VALUE;
		 _sex = Constant.STRING_INVALID_VALUE;
		 _department = Constant.STRING_INVALID_VALUE;
		 _officetypecode = Constant.INT_INVALID_VALUE;
		 _title = Constant.STRING_INVALID_VALUE;
		 _officetel = Constant.STRING_INVALID_VALUE;
		 _phone = Constant.STRING_INVALID_VALUE;
		 _email = Constant.STRING_INVALID_VALUE;
		 _fax = Constant.STRING_INVALID_VALUE;
		 _remarks = Constant.STRING_INVALID_VALUE;
		 _address = Constant.STRING_INVALID_VALUE;
		 _zipcode = Constant.STRING_INVALID_VALUE;
		 _msn = Constant.STRING_INVALID_VALUE;
		 _birthday = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
         _modifytime = Constant.DATE_INVALID_VALUE;
		 _hobby = Constant.STRING_INVALID_VALUE;
		 _responsible = Constant.STRING_INVALID_VALUE;
         _CreateUserTrueName = Constant.STRING_INVALID_VALUE;
         _CreateTimeFormat = Constant.STRING_INVALID_VALUE;
         _ModifyUserTrueName = Constant.STRING_INVALID_VALUE;
         _ModifyTimeFormat = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _id;
		private string _ptid;
		private int? _originalcontactid;
		private int? _pid;
		private string _cname;
		private string _ename;
		private string _sex;
		private string _department;
		private int? _officetypecode;
		private string _title;
		private string _officetel;
		private string _phone;
		private string _email;
		private string _fax;
		private string _remarks;
		private string _address;
		private string _zipcode;
		private string _msn;
		private string _birthday;
		private int? _status;
		private int? _createuserid;
		private DateTime? _createtime;
		private int? _modifyuserid;
        private DateTime? _modifytime;
		private string _hobby;
		private string _responsible;

        private string _CreateUserTrueName;
        private string _CreateTimeFormat;
        private string _ModifyUserTrueName;
        private string _ModifyTimeFormat;


		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
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
		public int? OriginalContactID
		{
			set{ _originalcontactid=value;}
			get{return _originalcontactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PID
		{
			set{ _pid=value;}
			get{return _pid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CName
		{
			set{ _cname=value;}
			get{return _cname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EName
		{
			set{ _ename=value;}
			get{return _ename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DepartMent
		{
			set{ _department=value;}
			get{return _department;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OfficeTypeCode
		{
			set{ _officetypecode=value;}
			get{return _officetypecode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OfficeTel
		{
			set{ _officetel=value;}
			get{return _officetel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Fax
		{
			set{ _fax=value;}
			get{return _fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ZipCode
		{
			set{ _zipcode=value;}
			get{return _zipcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MSN
		{
			set{ _msn=value;}
			get{return _msn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Birthday
		{
			set{ _birthday=value;}
			get{return _birthday;}
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
		public int? CreateUserID
		{
			set{ _createuserid=value;}
			get{return _createuserid;}
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
		public int? ModifyUserID
		{
			set{ _modifyuserid=value;}
			get{return _modifyuserid;}
		}
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string Hobby
		{
			set{ _hobby=value;}
			get{return _hobby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Responsible
		{
			set{ _responsible=value;}
			get{return _responsible;}
		}

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserTrueName
        {
            set { _CreateUserTrueName = value; }
            get { return _CreateUserTrueName; }
        }/// <summary>
        /// 
        /// </summary>
        public string CreateTimeFormat
        {
            set { _CreateTimeFormat = value; }
            get { return _CreateTimeFormat; }
        }/// <summary>
        /// 
        /// </summary>
        public string ModifyUserTrueName
        {
            set { _ModifyUserTrueName = value; }
            get { return _ModifyUserTrueName; }
        }/// <summary>
        /// 
        /// </summary>
        public string ModifyTimeFormat
        {
            set { _ModifyTimeFormat = value; }
            get { return _ModifyTimeFormat; }
        }

		#endregion Model

	}
}

