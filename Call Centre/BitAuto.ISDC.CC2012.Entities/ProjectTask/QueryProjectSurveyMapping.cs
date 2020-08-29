using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryProjectSurveyMapping 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:28 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryProjectSurveyMapping
	{
		public QueryProjectSurveyMapping()
		{
		 _projectid = Constant.INT_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _begindate = Constant.STRING_INVALID_VALUE;
		 _enddate = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

         _sname = Constant.STRING_INVALID_VALUE;//调查名称
         _pname = Constant.STRING_INVALID_VALUE;//项目名称
         _sbgid = Constant.INT_INVALID_VALUE;   //调查问卷业务组  
         _sscid = Constant.INT_INVALID_VALUE;   //调查分类
         _pbgid = Constant.INT_INVALID_VALUE;   //项目业务组
         _pscid = Constant.INT_INVALID_VALUE;   //项目分类
         _loginid = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private long _projectid;
		private int _siid;
		private string _begindate;
		private string _enddate;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _sname;
        private string _pname;
        private int _sbgid;
        private int _sscid;
        private int _pbgid;
        private int _pscid;
        private int _loginid;
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
		public int SIID
		{
			set{ _siid=value;}
			get{return _siid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BeginDate
		{
			set{ _begindate=value;}
			get{return _begindate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EndDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
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
         
        public string SName
        {
            set { _sname = value; }
            get { return _sname; }
        }
        public string PName
        {
            set { _pname = value; }
            get { return _pname; }
        }
        public int SBGID
        {
            set { _sbgid = value; }
            get { return _sbgid; }
        }
        public int SSCID
        {
            set { _sscid = value; }
            get { return _sscid; }
        }
        public int PBGID
        {
            set { _pbgid = value; }
            get { return _pbgid; }
        }
        public int PSCID
        {
            set { _pscid = value; }
            get { return _pscid; }
        }
        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }
		#endregion Model

	}
}

