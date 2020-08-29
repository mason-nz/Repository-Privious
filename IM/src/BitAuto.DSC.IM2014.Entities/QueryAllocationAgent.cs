using System;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryAllocationAgent 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryAllocationAgent
	{
		public QueryAllocationAgent()
		{
		 _allocid = Constant.INT_INVALID_VALUE;
		 _agentid = Constant.STRING_INVALID_VALUE;
		 _userid = Constant.STRING_INVALID_VALUE;
		 _starttime = Constant.DATE_INVALID_VALUE;
		 _endtime = Constant.DATE_INVALID_VALUE;
		 _userreferurl = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private long _allocid;
		private string _agentid;
		private string _userid;
		private DateTime? _starttime;
		private DateTime? _endtime;
		private string _userreferurl;
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
		public string AgentID
		{
			set{ _agentid=value;}
			get{return _agentid;}
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
		public DateTime? StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserReferURL
		{
			set{ _userreferurl=value;}
			get{return _userreferurl;}
		}
		#endregion Model

	}
}

