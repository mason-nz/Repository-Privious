using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryKnowledgeLib 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:10 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryKnowledgeLib
	{
		public QueryKnowledgeLib()
		{
         _klfaqid = Constant.INT_INVALID_VALUE;
		 _klid = Constant.INT_INVALID_VALUE;
		 _klnum = Constant.STRING_INVALID_VALUE;
		 _title = Constant.STRING_INVALID_VALUE;
		 _kcid = Constant.INT_INVALID_VALUE;
		 _content = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastmodifytime = Constant.DATE_INVALID_VALUE;
		 _lastmodifyuserid = Constant.INT_INVALID_VALUE;
		 _ishistory = Constant.INT_INVALID_VALUE;
		 _rejectreason = Constant.STRING_INVALID_VALUE;
		 _uploadfilecount = Constant.INT_INVALID_VALUE;
		 _faqcount = Constant.INT_INVALID_VALUE;
		 _questioncount = Constant.INT_INVALID_VALUE;

         _statuss = Constant.STRING_INVALID_VALUE;  //状态（多个，串）

         _begintime = Constant.STRING_INVALID_VALUE;
         _endtime = Constant.STRING_INVALID_VALUE;
         _property = Constant.STRING_INVALID_VALUE;//属性：1 有附件 2 有FAQ 3 有试题
         _category = Constant.STRING_INVALID_VALUE;//题型

         _keywords = Constant.STRING_INVALID_VALUE;//关键字
         _unread = Constant.STRING_INVALID_VALUE;//未读 
         _userid = Constant.INT_INVALID_VALUE;//阅读标记表中已读人ID

         _kcids = Constant.STRING_INVALID_VALUE;
		}
		#region Model
        private long _klfaqid;
		private long _klid;
		private string _klnum;
		private string _title;
		private int? _kcid;
		private string _content;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastmodifytime;
		private int? _lastmodifyuserid;
		private int? _ishistory;
		private string _rejectreason;
		private int? _uploadfilecount;
		private int? _faqcount;
        private int? _questioncount;
        private string _begintime;
        private string _endtime;
        private string _mbegintime;
        private string _mendtime;
        private string _property;
        private string _category;
        private string _keywords;
        private string _unread;
        private int _userid;
        private string _kcids;
        private string _statuss;

        public long KLFAQID
        {
            set { _klfaqid = value; }
            get { return _klfaqid; }
        }
        /// <summary>
		/// 
		/// </summary>
		public long KLID
		{
			set{ _klid=value;}
			get{return _klid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KLNum
		{
			set{ _klnum=value;}
			get{return _klnum;}
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
		public int? KCID
		{
			set{ _kcid=value;}
			get{return _kcid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
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
		public DateTime? LastModifyTime
		{
			set{ _lastmodifytime=value;}
			get{return _lastmodifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastModifyUserID
		{
			set{ _lastmodifyuserid=value;}
			get{return _lastmodifyuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsHistory
		{
			set{ _ishistory=value;}
			get{return _ishistory;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RejectReason
		{
			set{ _rejectreason=value;}
			get{return _rejectreason;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UploadFileCount
		{
			set{ _uploadfilecount=value;}
			get{return _uploadfilecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FAQCount
		{
			set{ _faqcount=value;}
			get{return _faqcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QuestionCount
		{
			set{ _questioncount=value;}
			get{return _questioncount;}
        }
        /// <summary>
        /// 
        /// </summary>
        public string MBeginTime
        {
            set { _mbegintime = value; }
            get { return _mbegintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MEndTime
        {
            set { _mendtime = value; }
            get { return _mendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Property
        {
            set { _property = value; }
            get { return _property; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Category
        {
            set { _category = value; }
            get { return _category; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Keywords
        {
            set { _keywords = value; }
            get { return _keywords; }
        }

        public string UnRead
        {
            set { _unread = value; }
            get { return _unread; }
        }

        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
            
        public string KCIDS
        {
            set { _kcids = value; }
            get { return _kcids; }
        }
        
        public string StatusS
        {
            set { _statuss = value; }
            get { return _statuss; }
        }
		#endregion Model

	}
}

