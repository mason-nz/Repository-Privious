using System;
using System.Collections.Generic;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryEmployeeAgent 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-02 10:01:54 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryEmployeeAgent
	{
		public QueryEmployeeAgent()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _userid = Constant.INT_INVALID_VALUE;
		 _agentnum = Constant.STRING_INVALID_VALUE;
         _bgid = Constant.INT_INVALID_VALUE;

		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _agentname = Constant.STRING_INVALID_VALUE;
         _areaid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int? _userid;
		private string _agentnum;
        private int? _bgid;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _agentname;
        private int? _areaid;
        private int? _isexclusive;

		/// <summary>
		/// 
		/// </summary>
		public int RecID
		{
			set{ _recid=value;}
			get{return _recid;}
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
		public string AgentNum
		{
			set{ _agentnum=value;}
			get{return _agentnum;}
		}
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
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
        public string AgentName
        {
            set { _agentname = value; }
            get { return _agentname; }
        }
        public int? RegionID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? IsExclusive
        {
            set { _isexclusive = value; }
            get { return _isexclusive; }
        }
		#endregion Model

	}
}

