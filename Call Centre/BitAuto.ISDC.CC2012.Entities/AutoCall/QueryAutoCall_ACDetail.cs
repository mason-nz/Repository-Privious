using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryAutoCall_ACDetail 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2015-09-14 09:57:57 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryAutoCall_ACDetail
	{
		public QueryAutoCall_ACDetail()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _acstatus = Constant.INT_INVALID_VALUE;
		 _actid = Constant.INT_INVALID_VALUE;
		 _projectid = Constant.INT_INVALID_VALUE;
		 _businessrecid = Constant.INT_INVALID_VALUE;
		 _returntime = Constant.DATE_INVALID_VALUE;
		 _acresult = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		//_timestamp没有默认值

		}
		#region Model
		private int _recid;
		private int? _acstatus;
		private int _actid;
		private long _projectid;
		private int? _businessrecid;
		private DateTime? _returntime;
		private int? _acresult;
		private DateTime? _createtime;
		//private timestamp _timestamp;
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
		public int? ACStatus
		{
			set{ _acstatus=value;}
			get{return _acstatus;}
		}
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
		public long ProjectID
		{
			set{ _projectid=value;}
			get{return _projectid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BusinessRecID
		{
			set{ _businessrecid=value;}
			get{return _businessrecid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ReturnTime
		{
			set{ _returntime=value;}
			get{return _returntime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ACResult
		{
			set{ _acresult=value;}
			get{return _acresult;}
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
		#endregion Model

	}
}

