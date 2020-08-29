using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryExamOnlineDetail 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryExamOnlineDetail
	{
		public QueryExamOnlineDetail()
		{
		 _eoldid = Constant.INT_INVALID_VALUE;
		 _eolid = Constant.INT_INVALID_VALUE;
		 _epid = Constant.INT_INVALID_VALUE;
		 _bqid = Constant.INT_INVALID_VALUE;
		 _klqid = Constant.INT_INVALID_VALUE;
		 _score = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _creaetuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _eoldid;
		private int _eolid;
		private int _epid;
		private long _bqid;
		private long _klqid;
		private int _score;
		private DateTime? _createtime;
		private int? _creaetuserid;
		/// <summary>
		/// 
		/// </summary>
		public long EOLDID
		{
			set{ _eoldid=value;}
			get{return _eoldid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int EOLID
		{
			set{ _eolid=value;}
			get{return _eolid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int EPID
		{
			set{ _epid=value;}
			get{return _epid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long BQID
		{
			set{ _bqid=value;}
			get{return _bqid;}
		}
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
		public int Score
		{
			set{ _score=value;}
			get{return _score;}
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

