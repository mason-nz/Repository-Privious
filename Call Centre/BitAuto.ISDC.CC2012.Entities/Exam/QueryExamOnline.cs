using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryExamOnline 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryExamOnline
	{
		public QueryExamOnline()
		{
		 _eolid = Constant.INT_INVALID_VALUE;
		 _eiid = Constant.INT_INVALID_VALUE;
		 _meiid = Constant.INT_INVALID_VALUE;
		 _exampersonid = Constant.INT_INVALID_VALUE;
		 _examstarttime = Constant.DATE_INVALID_VALUE;
		 _examendtime = Constant.DATE_INVALID_VALUE;
		 _sumscore = Constant.INT_INVALID_VALUE;
		 _ismakeup = Constant.INT_INVALID_VALUE;
		 _ismarking = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _creaetuserid = Constant.INT_INVALID_VALUE;
		 _lastmodifytime = Constant.DATE_INVALID_VALUE;
		 _lastmodifyuserid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
         _islack = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _eolid;
		private int _eiid;
		private int? _meiid;
		private int _exampersonid;
		private DateTime _examstarttime;
		private DateTime? _examendtime;
		private int? _sumscore;
		private int _ismakeup;
		private int _ismarking;
		private DateTime? _createtime;
		private int? _creaetuserid;
		private DateTime? _lastmodifytime;
		private int? _lastmodifyuserid;
		private int? _status;
        private int? _islack;
		/// <summary>
		/// 
		/// </summary>
		public long EOLID
		{
			set{ _eolid=value;}
			get{return _eolid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int EIID
		{
			set{ _eiid=value;}
			get{return _eiid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MEIID
		{
			set{ _meiid=value;}
			get{return _meiid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ExamPersonID
		{
			set{ _exampersonid=value;}
			get{return _exampersonid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ExamStartTime
		{
			set{ _examstarttime=value;}
			get{return _examstarttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ExamEndTime
		{
			set{ _examendtime=value;}
			get{return _examendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SumScore
		{
			set{ _sumscore=value;}
			get{return _sumscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsMakeUp
		{
			set{ _ismakeup=value;}
			get{return _ismakeup;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsMarking
		{
			set{ _ismarking=value;}
			get{return _ismarking;}
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
		public int? CreaetUserID
		{
			set{ _creaetuserid=value;}
			get{return _creaetuserid;}
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsLack
		{
            set { _islack = value; }
            get { return _islack; }
		}
		#endregion Model

	}
}

