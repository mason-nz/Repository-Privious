using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CallRecord_ORIG_Authorizer 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-17 10:17:59 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class CallRecord_ORIG_Authorizer
	{
		public CallRecord_ORIG_Authorizer()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _ip = Constant.STRING_INVALID_VALUE;
		 _authorizecode = Constant.STRING_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _ip;
		private string _authorizecode;
		private string _remark;
		private DateTime? _createtime;
		private int? _status;
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
		public string IP
		{
			set{ _ip=value;}
			get{return _ip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AuthorizeCode
		{
			set{ _authorizecode=value;}
			get{return _authorizecode;}
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

