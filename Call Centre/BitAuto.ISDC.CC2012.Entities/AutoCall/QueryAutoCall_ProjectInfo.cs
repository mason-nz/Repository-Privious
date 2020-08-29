using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryAutoCall_ProjectInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2015-09-14 09:57:58 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryAutoCall_ProjectInfo
	{
		public QueryAutoCall_ProjectInfo()
		{
		 _projectid = Constant.INT_INVALID_VALUE;
		 _skillid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _acstatus = Constant.INT_INVALID_VALUE;
		 _callnum = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
		 _totaltasknum = Constant.INT_INVALID_VALUE;
		 _appenddatatime = Constant.DATE_INVALID_VALUE;
		//_timestamp没有默认值

		}
		#region Model
		private long _projectid;
		private int _skillid;
		private int? _status;
		private int? _acstatus;
		private string _callnum;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
		private int? _totaltasknum;
		private DateTime? _appenddatatime;
		//private timestamp _timestamp;
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
		public int SkillID
		{
			set{ _skillid=value;}
			get{return _skillid;}
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
		public int? ACStatus
		{
			set{ _acstatus=value;}
			get{return _acstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CallNum
		{
			set{ _callnum=value;}
			get{return _callnum;}
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
		public DateTime? ModifyTime
		{
			set{ _modifytime=value;}
			get{return _modifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ModifyUserID
		{
			set{ _modifyuserid=value;}
			get{return _modifyuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TotalTaskNum
		{
			set{ _totaltasknum=value;}
			get{return _totaltasknum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AppendDataTime
		{
			set{ _appenddatatime=value;}
			get{return _appenddatatime;}
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

