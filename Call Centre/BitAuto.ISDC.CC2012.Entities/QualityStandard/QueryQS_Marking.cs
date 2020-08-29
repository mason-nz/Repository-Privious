using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryQS_Marking 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryQS_Marking
	{
		public QueryQS_Marking()
		{
		 _qs_rtid = Constant.INT_INVALID_VALUE;
		 _qs_cid = Constant.INT_INVALID_VALUE;
		 _qs_mid = Constant.INT_INVALID_VALUE;
		 _qs_iid = Constant.INT_INVALID_VALUE;
		 _qs_sid = Constant.INT_INVALID_VALUE;
		 _scoretype = Constant.INT_INVALID_VALUE;
		 _markingitemname = Constant.STRING_INVALID_VALUE;
		 _score = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _sort = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _qs_rtid;
		private int _qs_cid;
		private int _qs_mid;
		private int _qs_iid;
		private int _qs_sid;
		private int? _scoretype;
		private string _markingitemname;
		private int? _score;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private int? _sort;
		/// <summary>
		/// 
		/// </summary>
		public int QS_RTID
		{
			set{ _qs_rtid=value;}
			get{return _qs_rtid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_CID
		{
			set{ _qs_cid=value;}
			get{return _qs_cid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_MID
		{
			set{ _qs_mid=value;}
			get{return _qs_mid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_IID
		{
			set{ _qs_iid=value;}
			get{return _qs_iid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_SID
		{
			set{ _qs_sid=value;}
			get{return _qs_sid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ScoreType
		{
			set{ _scoretype=value;}
			get{return _scoretype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MarkingItemName
		{
			set{ _markingitemname=value;}
			get{return _markingitemname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Score
		{
			set{ _score=value;}
			get{return _score;}
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
		public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}
		#endregion Model

	}
}

