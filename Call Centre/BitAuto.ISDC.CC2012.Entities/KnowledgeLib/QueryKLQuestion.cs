using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryKLQuestion 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryKLQuestion
	{
		public QueryKLQuestion()
		{
		 _klqid = Constant.INT_INVALID_VALUE;
		 _klid = Constant.INT_INVALID_VALUE;
        _kcid = Constant.INT_INVALID_VALUE;
		 _askcategory = Constant.INT_INVALID_VALUE;
		 _ask = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;

         _begintime = Constant.STRING_INVALID_VALUE;
         _endtime = Constant.STRING_INVALID_VALUE;
         _statuss = Constant.STRING_INVALID_VALUE;  //知识点状态（多个，串）
         _questionstatuss = Constant.STRING_INVALID_VALUE;  //试题状态（多个，串）
		}
		#region Model
		private long _klqid;
		private long _klid;
	    private int _kcid;
        private int _askcategory;
        private string _askcategorys;
		private string _ask;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
        private int? _modifyuserid;
        private string _begintime;
        private string _endtime;
        private string _mbegintime;
        private string _mendtime;
        private string _statuss;
        private string _questionstatuss;
		/// <summary>
		/// 
		/// </summary>
		public long KLQID
		{
			set{ _klqid=value;}
			get{return _klqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long KLID
		{
			set{ _klid=value;}
			get{return _klid;}
		}
	 	public int KCID
		{
			set{ _kcid=value;}
			get{return _kcid;}
		}
       
		public int AskCategory
		{
			set{ _askcategory=value;}
			get{return _askcategory;}
		}
        public string AskCategorys
        {
            set { _askcategorys = value; }
            get { return _askcategorys; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string Ask
		{
			set{ _ask=value;}
			get{return _ask;}
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
        public string StatusS
        {
            set { _statuss = value; }
            get { return _statuss; }
        }
        public string QuestionStatusS
        {
            set { _questionstatuss = value; }
            get { return _questionstatuss; }
        }
		#endregion Model

	}
}

