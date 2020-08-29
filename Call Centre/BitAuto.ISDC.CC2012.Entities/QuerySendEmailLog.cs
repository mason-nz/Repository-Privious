using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ʵ����QuerySendEmailLog ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:19 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QuerySendEmailLog
	{
		public QuerySendEmailLog()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _templateid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _mailto = Constant.STRING_INVALID_VALUE;
		 _sendtime = Constant.DATE_INVALID_VALUE;
		 _sendcontent = Constant.STRING_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int? _templateid;
		private string _custid;
		private string _mailto;
		private DateTime? _sendtime;
		private string _sendcontent;
		private int? _createuserid;
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
		public int? TemplateID
		{
			set{ _templateid=value;}
			get{return _templateid;}
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
		public string MailTo
		{
			set{ _mailto=value;}
			get{return _mailto;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SendTime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SendContent
		{
			set{ _sendcontent=value;}
			get{return _sendcontent;}
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
