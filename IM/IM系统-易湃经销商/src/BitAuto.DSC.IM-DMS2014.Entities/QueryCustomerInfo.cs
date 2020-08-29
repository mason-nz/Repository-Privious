using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryCustomerInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:01 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryCustomerInfo
	{
		public QueryCustomerInfo()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _membercode = Constant.STRING_INVALID_VALUE;
		 _lastuserid = Constant.INT_INVALID_VALUE;
		 _lastmessagetime = Constant.DATE_INVALID_VALUE;
		 _lastbegintime = Constant.DATE_INVALID_VALUE;
		 _distribution = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastupdatetime = Constant.DATE_INVALID_VALUE;
		 _lastupdateuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _membercode;
		private int? _lastuserid;
		private DateTime? _lastmessagetime;
		private DateTime? _lastbegintime;
		private int? _distribution;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastupdatetime;
		private int? _lastupdateuserid;
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
		public string MemberCode
		{
			set{ _membercode=value;}
			get{return _membercode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastUserID
		{
			set{ _lastuserid=value;}
			get{return _lastuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastMessageTime
		{
			set{ _lastmessagetime=value;}
			get{return _lastmessagetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastBeginTime
		{
			set{ _lastbegintime=value;}
			get{return _lastbegintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Distribution
		{
			set{ _distribution=value;}
			get{return _distribution;}
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

