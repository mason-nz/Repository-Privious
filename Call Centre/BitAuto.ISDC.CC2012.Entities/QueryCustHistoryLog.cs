using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryCustHistoryLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryCustHistoryLog
	{
		public QueryCustHistoryLog()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _taskid = Constant.STRING_INVALID_VALUE;
		 _solveusereid = Constant.INT_INVALID_VALUE;
		 _solveuserid = Constant.INT_INVALID_VALUE;
		 _solvetime = Constant.DATE_INVALID_VALUE;
		 _comment = Constant.STRING_INVALID_VALUE;
		 _action = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
         _pid = Constant.INT_INVALID_VALUE;
         _tonextsolveusereid = Constant.INT_INVALID_VALUE;
         _tonextsolveuserid = Constant.INT_INVALID_VALUE;
         _allstatus = Constant.STRING_INVALID_VALUE;
		}
		#region Model
		private long _recid;
		private string _taskid;
		private int? _solveusereid;
		private int? _solveuserid;
		private DateTime? _solvetime;
		private string _comment;
		private int? _action;
		private int? _status;
        private long _pid;
        private int? _tonextsolveusereid;
        private int? _tonextsolveuserid;
        private string _allstatus;
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
		public string TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SolveUserEID
		{
			set{ _solveusereid=value;}
			get{return _solveusereid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SolveUserID
		{
			set{ _solveuserid=value;}
			get{return _solveuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SolveTime
		{
			set{ _solvetime=value;}
			get{return _solvetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Comment
		{
			set{ _comment=value;}
			get{return _comment;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Action
		{
			set{ _action=value;}
			get{return _action;}
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
		public long Pid
		{
			set{ _pid=value;}
			get{return _pid;}
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ToNextSolveUserEID
        {
            set { _tonextsolveusereid = value; }
            get { return _tonextsolveusereid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ToNextSolveUserID
        {
            set { _tonextsolveuserid = value; }
            get { return _tonextsolveuserid; }
        }
        public string StatusAll
        {
            set { _allstatus = value; }
            get { return _allstatus; } 
        }
		#endregion Model

	}
}

