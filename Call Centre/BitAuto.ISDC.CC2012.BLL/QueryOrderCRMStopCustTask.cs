using System;
using BitAuto.YanFa.HRManagement2011.Entities.Constants;

namespace BitAuto.YanFa.HRManagement2011.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryOrderCRMStopCustTask 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-07-01 12:18:32 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryOrderCRMStopCustTask
	{
		public QueryOrderCRMStopCustTask()
		{
		 _taskid = Constant.INT_INVALID_VALUE;
		 _taskstatus = Constant.INT_INVALID_VALUE;
		 _relationid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _assignuserid = Constant.INT_INVALID_VALUE;
		 _assigntime = Constant.DATE_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _submittime = Constant.DATE_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _taskid;
		private int? _taskstatus;
		private long _relationid;
		private int? _bgid;
		private int? _assignuserid;
		private DateTime? _assigntime;
		private int? _status;
		private DateTime? _submittime;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public long TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TaskStatus
		{
			set{ _taskstatus=value;}
			get{return _taskstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long RelationID
		{
			set{ _relationid=value;}
			get{return _relationid;}
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
		public int? AssignUserID
		{
			set{ _assignuserid=value;}
			get{return _assignuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AssignTime
		{
			set{ _assigntime=value;}
			get{return _assigntime;}
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
		public DateTime? SubmitTime
		{
			set{ _submittime=value;}
			get{return _submittime;}
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
		#endregion Model

	}
}

