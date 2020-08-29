using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryGroupOrderTask 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryGroupOrderTask
	{
		public QueryGroupOrderTask()
		{
		 _taskid = Constant.INT_INVALID_VALUE;
		 _taskstatus = Constant.INT_INVALID_VALUE;
		 _orderid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _scid = Constant.INT_INVALID_VALUE;
		 _assignuserid = Constant.INT_INVALID_VALUE;
		 _assigntime = Constant.DATE_INVALID_VALUE;
		 _submittime = Constant.DATE_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastupdatetime = Constant.DATE_INVALID_VALUE;
		 _lastupdateuserid = Constant.INT_INVALID_VALUE;


         _CustName = Constant.STRING_INVALID_VALUE;
         _ProvinceID = Constant.INT_INVALID_VALUE;
         _CityID = Constant.INT_INVALID_VALUE;
         _DealPerson = Constant.STRING_INVALID_VALUE;
         _Dealer = Constant.STRING_INVALID_VALUE;
         _IsReturnVisit = Constant.INT_INVALID_VALUE;
         _createtimebegin = Constant.DATE_INVALID_VALUE;
         _createtimeend = Constant.DATE_INVALID_VALUE;
         _submittimebegin = Constant.DATE_INVALID_VALUE;
         _submittimeend = Constant.DATE_INVALID_VALUE;
         _CarMasterID = Constant.INT_INVALID_VALUE;
         _CarSerialID = Constant.INT_INVALID_VALUE;
         _CarID = Constant.INT_INVALID_VALUE;
         _FailReason = Constant.INT_INVALID_VALUE;
         _loginid = Constant.INT_INVALID_VALUE;
         _ordercode = Constant.STRING_INVALID_VALUE;
         _CustomerTel = Constant.STRING_INVALID_VALUE;
         _telcount = Constant.STRING_INVALID_VALUE;
		}
		#region Model
		private long _taskid;
		private int? _taskstatus;
		private int? _orderid;
		private int? _bgid;
		private int? _scid;
		private int? _assignuserid;
		private DateTime? _assigntime;
		private DateTime? _submittime;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastupdatetime;
		private int? _lastupdateuserid;
        private int _loginid;
        private string _telcount;

        private string _CustomerTel;
        public string CustomerTel
        {
            set { _CustomerTel = value; }
            get { return _CustomerTel; }
        }

        private string _ordercode;
        public string OrderCode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 登录UserID
        /// </summary>
        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }

        private string _CustName;
        public string CustName
        {
            set { _CustName = value; }
            get { return _CustName; }
        }
        private int? _ProvinceID;
        public int? ProvinceID
        {
            set { _ProvinceID = value; }
            get { return _ProvinceID; }
        }
        private int? _CityID;
        public int? CityID
        {
            set { _CityID = value; }
            get { return _CityID; }
        }
        private string _DealPerson;
        public string DealPerson
        {
            set { _DealPerson = value; }
            get { return _DealPerson; }
        }
        private string _Dealer;
        public string Dealer
        {
            set { _Dealer = value; }
            get { return _Dealer; }
        }
        private int? _IsReturnVisit;
        public int? IsReturnVisit
        {
            set { _IsReturnVisit = value; }
            get { return _IsReturnVisit; }
        }
        private DateTime? _createtimebegin;
        public DateTime? CreatetimeBegin
        {
            set { _createtimebegin = value; }
            get { return _createtimebegin; }
        }
        private DateTime? _createtimeend;
        public DateTime? CreatetimeEnd
        {
            set { _createtimeend = value; }
            get { return _createtimeend; }
        }
        private DateTime? _submittimebegin;
        public DateTime? SubmitTimeBegin
        {
            set { _submittimebegin = value; }
            get { return _submittimebegin; }
        }
        private DateTime? _submittimeend;
        public DateTime? SubmitTimeEnd
        {
            set { _submittimeend = value; }
            get { return _submittimeend; }
        }
        private int? _CarMasterID;
        public int? CarMasterID
        {
            set { _CarMasterID = value; }
            get { return _CarMasterID; }
        }
        private int? _CarSerialID;
        public int? CarSerialID
        {
            set { _CarSerialID = value; }
            get { return _CarSerialID; }
        }
        private int? _CarID;
        public int? CarID
        {
            set { _CarID = value; }
            get { return _CarID; }
        }
        private int? _FailReason;
        public int? FailReason
        {
            set { _FailReason = value; }
            get { return _FailReason; }
        }

		/// <summary>
		/// 
		/// </summary>
		public long TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
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
		public int? OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SCID
		{
			set{ _scid=value;}
			get{return _scid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AssignUserID
		{
			set{ _assignuserid=value;}
			get{return _assignuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AssignTime
		{
			set{ _assigntime=value;}
			get{return _assigntime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SubmitTime
		{
			set{ _submittime=value;}
			get{return _submittime;}
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
		public DateTime? LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastUpdateUserID
		{
			set{ _lastupdateuserid=value;}
			get{return _lastupdateuserid;}
		}
        /// <summary>
        /// 客户电话在订单中出现次数(-1,1,n)
        /// </summary>
        public string TelCount
        {
            set { _telcount = value; }
            get { return _telcount; }
        }
		#endregion Model

	}
}

