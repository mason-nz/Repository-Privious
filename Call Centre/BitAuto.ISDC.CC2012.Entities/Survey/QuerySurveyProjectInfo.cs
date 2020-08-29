using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QuerySurveyProjectInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:18 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QuerySurveyProjectInfo
	{
		public QuerySurveyProjectInfo()
		{
		 _spiid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
         //_bgidstr = Constant.STRING_INVALID_VALUE;
		 _scid = Constant.INT_INVALID_VALUE;
         _statusstr = Constant.STRING_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _description = Constant.STRING_INVALID_VALUE;
		 _businessgroup = Constant.STRING_INVALID_VALUE;
		 _surveystarttime = Constant.DATE_INVALID_VALUE;
		 _surveyendtime = Constant.DATE_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
         _statusstr = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
         _begincreatetime = Constant.DATE_INVALID_VALUE;
         _endcreatetime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _loginuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;

         _creatertype = Constant.INT_INVALID_VALUE;//登陆人类型，如果为1，则会关联人员表
         _userid = Constant.INT_INVALID_VALUE;//登陆者ID
		}
		#region Model
		private int _spiid;
		private int? _bgid;
        //private string _bgidstr;
		private int? _scid;
        private string _scidstr;
		private string _name;
		private string _description;
		private string _businessgroup;
		private DateTime? _surveystarttime;
		private DateTime? _surveyendtime;
		private int? _siid;
		private int? _status;
        private string _statusstr;
		private DateTime? _createtime;
        private DateTime? _begincreatetime;
        private DateTime? _endcreatetime;
		private int? _createuserid;
        private int? _loginuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
        private int _creatertype;
        private int _userid;
		/// <summary>
		/// 
		/// </summary>
		public int SPIID
		{
			set{ _spiid=value;}
			get{return _spiid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string BGIDStr
        //{
        //    set { _bgidstr = value; }
        //    get { return _bgidstr; }
        //}
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
        public string SCIDStr
        {
            set { _scidstr = value; }
            get { return _scidstr; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BusinessGroup
		{
			set{ _businessgroup=value;}
			get{return _businessgroup;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SurveyStartTime
		{
			set{ _surveystarttime=value;}
			get{return _surveystarttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SurveyEndTime
		{
			set{ _surveyendtime=value;}
			get{return _surveyendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SIID
		{
			set{ _siid=value;}
			get{return _siid;}
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
        public string StatusStr
        {
            set { _statusstr = value; }
            get { return _statusstr; }
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
        public DateTime? BeginCreateTime
        {
            set { _begincreatetime = value; }
            get { return _begincreatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndCreateTime
        {
            set { _endcreatetime = value; }
            get { return _endcreatetime; }
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
        public int? LoginUserID
        {
            set { _loginuserid = value; }
            get { return _loginuserid; }
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
        public int CreaterType
        {
            set { _creatertype = value; }
            get { return _creatertype; }
        }
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
		#endregion Model

	}
}

