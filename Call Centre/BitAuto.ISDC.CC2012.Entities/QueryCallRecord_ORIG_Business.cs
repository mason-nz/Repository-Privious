using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryCallRecord_ORIG_Business 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-05-27 10:46:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryCallRecord_ORIG_Business
	{
		public QueryCallRecord_ORIG_Business()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _callid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _scid = Constant.INT_INVALID_VALUE;
		 _businessid = Constant.STRING_INVALID_VALUE;
		 _businessdetailurl = Constant.STRING_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private Int64 _callid;
		private int? _bgid;
		private int? _scid;
		private string _businessid;
		private string _businessdetailurl;
		private int? _createuserid;
		private DateTime? _createtime;
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
		public Int64 CallID
		{
			set{ _callid=value;}
			get{return _callid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
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
		public string BusinessID
		{
			set{ _businessid=value;}
			get{return _businessid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BusinessDetailURL
		{
			set{ _businessdetailurl=value;}
			get{return _businessdetailurl;}
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
		public DateTime? CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

