using System;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类AgentInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:57 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class AgentInfo
	{
		public AgentInfo()
		{
		 _agentid = Constant.STRING_INVALID_VALUE;
		 _agentstatus = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _priority = Constant.INT_INVALID_VALUE;
		 _maxdialogcount = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _domainaccount = Constant.STRING_EMPTY_VALUE;
         _username = Constant.STRING_INVALID_VALUE;
		}
		#region Model
		private string _agentid;
		private int? _agentstatus;
		private int? _status;
		private int? _priority;
		private int? _maxdialogcount;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _domainaccount;
        private string _username;
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Priority
		{
			set{ _priority=value;}
			get{return _priority;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MaxDialogCount
		{
			set{ _maxdialogcount=value;}
			get{return _maxdialogcount;}
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
        /// 域帐号
        /// </summary>
        public string DomainAccount
        {
            set { _domainaccount = value; }
            get { return _domainaccount; }
        }

        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
		#endregion Model

	}
}

