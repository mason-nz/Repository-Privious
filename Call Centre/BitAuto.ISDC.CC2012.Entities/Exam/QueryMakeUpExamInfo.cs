using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryMakeUpExamInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:20 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryMakeUpExamInfo
	{
		public QueryMakeUpExamInfo()
		{
		 _meiid = Constant.INT_INVALID_VALUE;
		 _eiid = Constant.INT_INVALID_VALUE;
		 _makeupepid = Constant.INT_INVALID_VALUE;
		 _makeupexamstarttime = Constant.DATE_INVALID_VALUE;
		 _makeupexamendtime = Constant.DATE_INVALID_VALUE;
		 _joinnum = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
        private int _meiid;
		private long _eiid;
		private int? _makeupepid;
		private DateTime? _makeupexamstarttime;
		private DateTime? _makeupexamendtime;
		private int? _joinnum;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
        public int MEIID
		{
			set{ _meiid=value;}
			get{return _meiid;}
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
		public int? MakeUpEPID
		{
			set{ _makeupepid=value;}
			get{return _makeupepid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? MakeUpExamStartTime
		{
			set{ _makeupexamstarttime=value;}
			get{return _makeupexamstarttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? MakeupExamEndTime
		{
			set{ _makeupexamendtime=value;}
			get{return _makeupexamendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? JoinNum
		{
			set{ _joinnum=value;}
			get{return _joinnum;}
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

