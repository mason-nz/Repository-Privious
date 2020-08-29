using System;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryChatMessageLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:58 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryChatMessageLog
	{
		public QueryChatMessageLog()
		{
		 _messageid = Constant.INT_INVALID_VALUE;
		 _allocid = Constant.INT_INVALID_VALUE;
		 _sender = Constant.STRING_INVALID_VALUE;
		 _receiver = Constant.STRING_INVALID_VALUE;
		 _content = Constant.STRING_INVALID_VALUE;
		 _type = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
         _begintime = Constant.STRING_EMPTY_VALUE;
         _endtime = Constant.STRING_EMPTY_VALUE;
		}
		#region Model
		private long _messageid;
		private long _allocid;
		private string _sender;
		private string _receiver;
		private string _content;
		private int? _type;
		private int? _status;
		private DateTime? _createtime;
        private string _begintime;
        private string _endtime;
		/// <summary>
		/// 
		/// </summary>
		public long MessageID
		{
			set{ _messageid=value;}
			get{return _messageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long AllocID
		{
			set{ _allocid=value;}
			get{return _allocid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sender
		{
			set{ _sender=value;}
			get{return _sender;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Receiver
		{
			set{ _receiver=value;}
			get{return _receiver;}
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
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
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
        public string BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
		#endregion Model

	}
}

