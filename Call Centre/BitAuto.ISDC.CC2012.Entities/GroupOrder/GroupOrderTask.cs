using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类GroupOrderTask 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class GroupOrderTask
	{
		public GroupOrderTask()
		{
		 _taskid = Constant.INT_INVALID_VALUE;
		 _taskstatus = Constant.INT_INVALID_VALUE;
		 _orderid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _scid = Constant.INT_INVALID_VALUE;
		 _assignuserid = Constant.INT_INVALID_VALUE;
		 _assigntime = Constant.DATE_INVALID_VALUE;
		 _submittime = Constant.DATE_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastupdatetime = Constant.DATE_INVALID_VALUE;
		 _lastupdateuserid = Constant.INT_INVALID_VALUE;         

		}
		#region Model
		private long _taskid;
		private int? _taskstatus;
		private int? _orderid;
		private int? _bgid;
		private int? _scid;
		private int? _assignuserid;
		private DateTime? _assigntime;
		private DateTime? _submittime;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastupdatetime;
		private int? _lastupdateuserid;
        
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
		public int? OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
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
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastUpdateUserID
		{
			set{ _lastupdateuserid=value;}
			get{return _lastupdateuserid;}
		}
		#endregion Model

	}
}

