using System;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类AgentStatusDetail 。(属性说明自动提取数据库字段的描述信息)
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
	public class AgentStatusDetail
	{
		public AgentStatusDetail()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _agentid = Constant.STRING_INVALID_VALUE;
		 _agentstatus = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private string _agentid;
		private int? _agentstatus;
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
		public string AgentID
		{
			set{ _agentid=value;}
			get{return _agentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AgentStatus
		{
			set{ _agentstatus=value;}
			get{return _agentstatus;}
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

