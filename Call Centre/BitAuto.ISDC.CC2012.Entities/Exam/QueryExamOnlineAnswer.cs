using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryExamOnlineAnswer 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryExamOnlineAnswer
	{
		public QueryExamOnlineAnswer()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _eoldid = Constant.INT_INVALID_VALUE;
		 _examanswer = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _creaetuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private int _eoldid;
		private string _examanswer;
		private DateTime? _createtime;
		private int? _creaetuserid;
		/// <summary>
		/// 
		/// </summary>
		public long RecID
		{
			set{ _recid=value;}
			get{return _recid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int EOLDID
		{
			set{ _eoldid=value;}
			get{return _eoldid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ExamAnswer
		{
			set{ _examanswer=value;}
			get{return _examanswer;}
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
		#endregion Model

	}
}

