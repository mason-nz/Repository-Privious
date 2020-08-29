using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectSurveyMapping 。(属性说明自动提取数据库字段的描述信息)
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
	public class ProjectSurveyMapping
	{
		public ProjectSurveyMapping()
		{
		 _projectid = Constant.INT_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _begindate = Constant.STRING_INVALID_VALUE;
		 _enddate = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _returnvisitcustid = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private long _projectid;
		private int _siid;
		private string _begindate;
		private string _enddate;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _returnvisitcustid;
        public string ReturnVisitCustID
        {
            set { _returnvisitcustid = value; }
            get { return _returnvisitcustid; }
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
		#endregion Model

	}
}

