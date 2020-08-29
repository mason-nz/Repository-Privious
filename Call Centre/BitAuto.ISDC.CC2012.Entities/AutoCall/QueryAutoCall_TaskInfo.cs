using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryAutoCall_TaskInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2015-09-14 09:57:59 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryAutoCall_TaskInfo
	{
		public QueryAutoCall_TaskInfo()
		{
		 _actid = Constant.INT_INVALID_VALUE;
		 _businessrecid = Constant.INT_INVALID_VALUE;
		 _businessid = Constant.STRING_INVALID_VALUE;
		 _projectid = Constant.INT_INVALID_VALUE;
		 _phone = Constant.STRING_INVALID_VALUE;
		 _phoneprefix = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		//_timestamp没有默认值
		 _acstatus = Constant.INT_INVALID_VALUE;
		 _servicetaketime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _actid;
		private int _businessrecid;
		private string _businessid;
		private long _projectid;
		private string _phone;
		private string _phoneprefix;
		private int? _status;
		private DateTime? _createtime;
		//private timestamp _timestamp;
		private int? _acstatus;
		private DateTime? _servicetaketime;
		/// <summary>
		/// 
		/// </summary>
		public int ACTID
		{
			set{ _actid=value;}
			get{return _actid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BusinessRecID
		{
			set{ _businessrecid=value;}
			get{return _businessrecid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BusinessID
		{
			set{ _businessid=value;}
			get{return _businessid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long ProjectID
		{
			set{ _projectid=value;}
			get{return _projectid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PhonePrefix
		{
			set{ _phoneprefix=value;}
			get{return _phoneprefix;}
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
        ///// <summary>
        ///// 
        ///// </summary>
        //public timestamp Timestamp
        //{
        //    set{ _timestamp=value;}
        //    get{return _timestamp;}
        //}
		/// <summary>
		/// 
		/// </summary>
		public int? ACStatus
		{
			set{ _acstatus=value;}
			get{return _acstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ServiceTakeTime
		{
			set{ _servicetaketime=value;}
			get{return _servicetaketime;}
		}
		#endregion Model

	}
}

