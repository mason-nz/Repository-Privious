using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTaskInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:32 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTaskInfo
	{
		public ProjectTaskInfo()
		{
            _ptid = Constant.STRING_INVALID_VALUE;
		 _pdsid = Constant.INT_INVALID_VALUE;
		 _projectid = Constant.INT_INVALID_VALUE;
		 _custname = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _source = Constant.INT_INVALID_VALUE;
		 _relationid = Constant.STRING_INVALID_VALUE;
		 _lastopttime = Constant.DATE_INVALID_VALUE;
		 _lastoptuserid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _taskstatus = Constants.Constant.INT_INVALID_VALUE;
         _crmcustid = Constants.Constant.STRING_INVALID_VALUE; 
		}
		#region Model
		private string _ptid;
		private long _pdsid;
		private long _projectid;
		private string _custname;
		private int? _status;
        private int? _source;
		private string _relationid;
		private DateTime? _lastopttime;
		private int? _lastoptuserid;
		private DateTime? _createtime;
		private int? _createuserid;
        private int _taskstatus;
        private string _crmcustid;
        private string _custtype;
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
		public long PDSID
		{
			set{ _pdsid=value;}
			get{return _pdsid;}
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
		public string CustName
		{
			set{ _custname=value;}
			get{return _custname;}
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
		public int? Source
		{
            set { _source = value; }
            get { return _source; }
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
        public int TaskStatus
        {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }
        public string CrmCustID
        {
            set { _crmcustid = value; }
            get { return _crmcustid; }
        }

        public string CustType
        {
            set { _custtype = value; }
            get { return _custtype; }
        }
		#endregion Model

	}
}

