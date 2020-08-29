using System;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryUserMessage 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:59 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryUserMessage
	{
		public QueryUserMessage()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _userid = Constant.STRING_INVALID_VALUE;
		 _content = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private string _userid;
		private string _content;
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
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
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

