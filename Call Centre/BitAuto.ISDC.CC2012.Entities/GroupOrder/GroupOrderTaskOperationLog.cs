using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类GroupOrderTaskOperationLog 。(属性说明自动提取数据库字段的描述信息)
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
	public class GroupOrderTaskOperationLog
	{
		public GroupOrderTaskOperationLog()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _taskid = Constant.INT_INVALID_VALUE;
		 _operationstatus = Constant.INT_INVALID_VALUE;
		 _taskstatus = Constant.INT_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _callrecordid = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private long _recid;
		private long _taskid;
		private int? _operationstatus;
		private int? _taskstatus;
		private string _remark;
		private DateTime? _createtime;
		private int? _createuserid;
        private long _callrecordid;

        public long CallRecordID
        {
            set { _callrecordid = value; }
            get { return _callrecordid; }
        }

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
		public long TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OperationStatus
		{
			set{ _operationstatus=value;}
			get{return _operationstatus;}
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
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
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

