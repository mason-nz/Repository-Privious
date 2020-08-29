using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryEPVisitLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:02 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryEPVisitLog
	{
		public QueryEPVisitLog()
		{
		 _visitid = Constant.STRING_INVALID_VALUE;
		 _loginid = Constant.INT_INVALID_VALUE;
		 _loginname = Constant.STRING_INVALID_VALUE;
		 _membercode = Constant.STRING_INVALID_VALUE;
		 _visitrefer = Constant.STRING_INVALID_VALUE;
		 _userrefertitle = Constant.STRING_INVALID_VALUE;
		 _userreferurl = Constant.STRING_INVALID_VALUE;
		 _contractname = Constant.STRING_INVALID_VALUE;
		 _contractjob = Constant.STRING_INVALID_VALUE;
		 _contractphone = Constant.STRING_INVALID_VALUE;
		 _contractemail = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private string _visitid;
		private long _loginid;
		private string _loginname;
		private string _membercode;
		private string _visitrefer;
		private string _userrefertitle;
		private string _userreferurl;
		private string _contractname;
		private string _contractjob;
		private string _contractphone;
		private string _contractemail;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public string VisitID
		{
			set{ _visitid=value;}
			get{return _visitid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long LoginID
		{
			set{ _loginid=value;}
			get{return _loginid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoginName
		{
			set{ _loginname=value;}
			get{return _loginname;}
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
		public string VisitRefer
		{
			set{ _visitrefer=value;}
			get{return _visitrefer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserReferTitle
		{
			set{ _userrefertitle=value;}
			get{return _userrefertitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserReferURL
		{
			set{ _userreferurl=value;}
			get{return _userreferurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ContractName
		{
			set{ _contractname=value;}
			get{return _contractname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ContractJob
		{
			set{ _contractjob=value;}
			get{return _contractjob;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ContractPhone
		{
			set{ _contractphone=value;}
			get{return _contractphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ContractEmail
		{
			set{ _contractemail=value;}
			get{return _contractemail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

