using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryProjectTask_CSTMember 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryProjectTask_CSTMember
	{
        public QueryProjectTask_CSTMember()
		{
		 _id = Constant.INT_INVALID_VALUE;
		 _tid = Constant.INT_INVALID_VALUE;
		 _originalcstrecid = Constant.INT_INVALID_VALUE;
		 _vendorcode = Constant.STRING_INVALID_VALUE;
		 _fullname = Constant.STRING_INVALID_VALUE;
		 _shortname = Constant.STRING_INVALID_VALUE;
		 _vendorclass = Constant.INT_INVALID_VALUE;
		 _superid = Constant.INT_INVALID_VALUE;
		 _provinceid = Constant.STRING_INVALID_VALUE;
		 _cityid = Constant.STRING_INVALID_VALUE;
		 _countyid = Constant.STRING_INVALID_VALUE;
		 _address = Constant.STRING_INVALID_VALUE;
		 _postcode = Constant.STRING_INVALID_VALUE;
		 _trafficinfo = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _status = Constant.INT_INVALID_VALUE;
         _custname = Constant.STRING_INVALID_VALUE;
         custID = Constant.STRING_INVALID_VALUE;
         applystarttime = Constant.STRING_INVALID_VALUE;
         applyendtime = Constant.STRING_INVALID_VALUE;
         applyusername = Constant.STRING_INVALID_VALUE;
         memberoptstarttime = Constant.STRING_INVALID_VALUE;
         memberoptendtime = Constant.STRING_INVALID_VALUE;
         cstmembercreateuserid = Constant.INT_INVALID_VALUE;
         cstsyncstatus = Constant.STRING_INVALID_VALUE;
         defaultcstsyncstatus = Constant.STRING_INVALID_VALUE;
         cstmemberapplyuserid = Constant.INT_INVALID_VALUE;
         cststatus = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _id;
		private int? _tid;
		private int? _originalcstrecid;
		private string _vendorcode;
		private string _fullname;
		private string _shortname;
		private int? _vendorclass;
		private int? _superid;
		private string _provinceid;
		private string _cityid;
		private string _countyid;
		private string _address;
		private string _postcode;
		private string _trafficinfo;
		private DateTime? _createtime;
		private int? _createuserid;
		private int? _status;
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
		public int? TID
		{
			set{ _tid=value;}
			get{return _tid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OriginalCSTRecID
		{
			set{ _originalcstrecid=value;}
			get{return _originalcstrecid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string VendorCode
		{
			set{ _vendorcode=value;}
			get{return _vendorcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FullName
		{
			set{ _fullname=value;}
			get{return _fullname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ShortName
		{
			set{ _shortname=value;}
			get{return _shortname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? VendorClass
		{
			set{ _vendorclass=value;}
			get{return _vendorclass;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SuperId
		{
			set{ _superid=value;}
			get{return _superid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProvinceID
		{
			set{ _provinceid=value;}
			get{return _provinceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CountyID
		{
			set{ _countyid=value;}
			get{return _countyid;}
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
		public string PostCode
		{
			set{ _postcode=value;}
			get{return _postcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TrafficInfo
		{
			set{ _trafficinfo=value;}
			get{return _trafficinfo;}
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}

        private string _custname;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName
        {
            get { return _custname; }
            set { _custname = value; }
        }

        private string custID;
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        private string applystarttime;
        /// <summary>
        /// 会员申请开始时间
        /// </summary>
        public string ApplyStartTime
        {
            get { return applystarttime; }
            set { applystarttime = value; }
        }

        private string applyendtime;
        /// <summary>
        /// 会员申请结束时间
        /// </summary>
        public string ApplyEndTime
        {
            get { return applyendtime; }
            set { applyendtime = value; }
        }

        private string applyusername;
        /// <summary>
        /// 会员申请人姓名
        /// </summary>
        public string ApplyUserName
        {
            get { return applyusername; }
            set { applyusername = value; }
        }

        private string memberoptstarttime;
        /// <summary>
        /// 会员审批
        /// </summary>
        public string MemberOptStartTime
        {
            get { return memberoptstarttime; }
            set { memberoptstarttime = value; }
        }

        private string memberoptendtime;
        /// <summary>
        /// 会员申请结束时间
        /// </summary>
        public string MemberOptEndTime
        {
            get { return memberoptendtime; }
            set { memberoptendtime = value; }
        }

        private int cstmembercreateuserid;
        /// <summary>
        /// CRM系统中，CSTMember会员信息创建人ID
        /// </summary>
        public int CSTMemberCreateUserID
        {
            get { return cstmembercreateuserid; }
            set { cstmembercreateuserid = value; }
        }

        private int cstmemberapplyuserid;
        /// <summary>
        /// CRM系统中，CSTMember会员信息申请人ID
        /// </summary>
        public int CSTMemberApplyUserID
        {
            get { return cstmemberapplyuserid; }
            set { cstmemberapplyuserid = value; }
        }


        private string cstsyncstatus;
        /// <summary>
        /// 会员申请状态ID串
        /// </summary>
        public string CSTSyncStatus
        {
            get { return cstsyncstatus; }
            set { cstsyncstatus = value; }
        }

        private string defaultcstsyncstatus;
        /// <summary>
        /// 会员申请状态ID串（默认）
        /// </summary>
        public string DefaultCSTSyncStatus
        {
            get { return defaultcstsyncstatus; }
            set { defaultcstsyncstatus = value; }
        }

        private string cststatus;
        /// <summary>
        /// CRM系统中会员状态
        /// </summary>
        public string CSTStatus
        {
            get { return cststatus; }
            set { cststatus = value; }
        }
		#endregion Model

	}
}

