using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryExamPerson 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:19 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryExamPerson
	{
		public QueryExamPerson()
		{
		 _epid = Constant.INT_INVALID_VALUE;
		 _eiid = Constant.INT_INVALID_VALUE;
		 _meiid = Constant.INT_INVALID_VALUE;
		 _exampersonid = Constant.INT_INVALID_VALUE;
		 _examtype = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _epid;
		private long _eiid;
		private int? _meiid;
		private int _exampersonid;
		private int _examtype;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public long EPID
		{
			set{ _epid=value;}
			get{return _epid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long EIID
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
		public int ExamPerSonID
		{
			set{ _exampersonid=value;}
			get{return _exampersonid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ExamType
		{
			set{ _examtype=value;}
			get{return _examtype;}
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

