using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类Conversations 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:01 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class Conversations
	{
		public Conversations()
		{
		 _csid = Constant.INT_INVALID_VALUE;
		 _userid = Constant.INT_INVALID_VALUE;
		 _username = Constant.STRING_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _visitid = Constant.STRING_INVALID_VALUE;
		 _agentstarttime = Constant.DATE_INVALID_VALUE;
		 _lastclienttime = Constant.DATE_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _orderid = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _endtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _csid;
		private int? _userid;
		private string _username;
		private int? _bgid;
		private string _visitid;
		private DateTime? _agentstarttime;
		private DateTime? _lastclienttime;
		private int? _status;
		private string _orderid;
		private DateTime? _createtime;
		private DateTime? _endtime;
		/// <summary>
		/// 
		/// </summary>
		public int CSID
		{
			set{ _csid=value;}
			get{return _csid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
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
		public string VisitID
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AgentStartTime
		{
			set{ _agentstarttime=value;}
			get{return _agentstarttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastClientTime
		{
			set{ _lastclienttime=value;}
			get{return _lastclienttime;}
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
		public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
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
		public DateTime? EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		#endregion Model

	}
}

