using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ʵ����QueryQS_Result ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryQS_Result
	{
		public QueryQS_Result()
		{
		 _qs_rid = Constant.INT_INVALID_VALUE;
		 _callrecordid = Constant.INT_INVALID_VALUE;
		 _qs_rtid = Constant.INT_INVALID_VALUE;
		 _seatid = Constant.STRING_INVALID_VALUE;
		 _scoretype = Constant.INT_INVALID_VALUE;
		 _score = Constant.INT_INVALID_VALUE;
		 _isqualified = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _stateresult = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
		 _qualityappraisal = Constant.STRING_INVALID_VALUE;
         _begintime = Constant.DATE_INVALID_VALUE;
         _endtime = Constant.DATE_INVALID_VALUE;
         _agentuserid = Constant.INT_INVALID_VALUE;
         _callbegintime = Constant.STRING_INVALID_VALUE;
         _callendtime = Constant.STRING_INVALID_VALUE;
		}
		#region Model
		private int _qs_rid;
		private Int64 _callrecordid;
		private int _qs_rtid;
		private string _seatid;
		private int? _scoretype;
		private int? _score;
		private int? _isqualified;
		private int? _status;
		private int? _stateresult;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
		private string _qualityappraisal;
        private DateTime _begintime;
        private DateTime _endtime;
        private int _agentuserid;
        private string _callbegintime;
        private string _callendtime;
		/// <summary>
		/// 
		/// </summary>
		public int QS_RID
		{
			set{ _qs_rid=value;}
			get{return _qs_rid;}
		}
		/// <summary>
		/// 
		/// </summary>
        public Int64 CallReCordID
		{
			set{ _callrecordid=value;}
			get{return _callrecordid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_RTID
		{
			set{ _qs_rtid=value;}
			get{return _qs_rtid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SeatID
		{
			set{ _seatid=value;}
			get{return _seatid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ScoreType
		{
			set{ _scoretype=value;}
			get{return _scoretype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsQualified
		{
			set{ _isqualified=value;}
			get{return _isqualified;}
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
		public int? StateResult
		{
			set{ _stateresult=value;}
			get{return _stateresult;}
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
		public DateTime? ModifyTime
		{
			set{ _modifytime=value;}
			get{return _modifytime;}
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
		public string QualityAppraisal
		{
			set{ _qualityappraisal=value;}
			get{return _qualityappraisal;}
		}
        /// <summary>
        /// 
        /// </summary>
        public DateTime BeginTime
        {
            set{_begintime = value;}
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        public string CallBeginTime
        {
            set { _callbegintime = value; }
            get { return _callbegintime; }
        }
        public string CallEndTime
        {
            set { _callendtime = value; }
            get { return _callendtime; }
        }
		#endregion Model

	}
}

