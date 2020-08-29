using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类SMSSendHistory 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-12-23 06:16:58 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class SMSSendHistory
	{
		public SMSSendHistory()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _templateid = Constant.INT_INVALID_VALUE;
		 _phone = Constant.STRING_INVALID_VALUE;
		 _content = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _custid = Constant.STRING_INVALID_VALUE;
		 _crmcustid = Constant.STRING_INVALID_VALUE;
		 _tasktype = Constant.INT_INVALID_VALUE;
		 _taskid = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int? _bgid;
		private int? _templateid;
		private string _phone;
		private string _content;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _custid;
		private string _crmcustid;
		private int? _tasktype;
		private string _taskid;
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
		public int? BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TemplateID
		{
			set{ _templateid=value;}
			get{return _templateid;}
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
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
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
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CRMCustID
		{
			set{ _crmcustid=value;}
			get{return _crmcustid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TaskType
		{
			set{ _tasktype=value;}
			get{return _tasktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		#endregion Model

	}
}

