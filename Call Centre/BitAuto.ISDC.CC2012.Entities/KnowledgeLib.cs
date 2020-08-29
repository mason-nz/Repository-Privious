using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类KnowledgeLib 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:10 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class KnowledgeLib
	{
		public KnowledgeLib()
		{
		 _klid = Constant.INT_INVALID_VALUE;
		 _klnum = Constant.STRING_INVALID_VALUE;
		 _title = Constant.STRING_INVALID_VALUE;
		 _kcid = Constant.INT_INVALID_VALUE;
		 _content = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastmodifytime = Constant.DATE_INVALID_VALUE;
		 _lastmodifyuserid = Constant.INT_INVALID_VALUE;
		 _ishistory = Constant.INT_INVALID_VALUE;
		 _rejectreason = Constant.STRING_INVALID_VALUE;
		 _uploadfilecount = Constant.INT_INVALID_VALUE;
		 _faqcount = Constant.INT_INVALID_VALUE;
		 _questioncount = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _klid;
		private string _klnum;
		private string _title;
		private int? _kcid;
		private string _content;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastmodifytime;
		private int? _lastmodifyuserid;
		private int? _ishistory;
		private string _rejectreason;
		private int? _uploadfilecount;
		private int? _faqcount;
		private int? _questioncount;
		/// <summary>
		/// 
		/// </summary>
		public long KLID
		{
			set{ _klid=value;}
			get{return _klid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KLNum
		{
			set{ _klnum=value;}
			get{return _klnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? KCID
		{
			set{ _kcid=value;}
			get{return _kcid;}
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
		public DateTime? LastModifyTime
		{
			set{ _lastmodifytime=value;}
			get{return _lastmodifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastModifyUserID
		{
			set{ _lastmodifyuserid=value;}
			get{return _lastmodifyuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsHistory
		{
			set{ _ishistory=value;}
			get{return _ishistory;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RejectReason
		{
			set{ _rejectreason=value;}
			get{return _rejectreason;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UploadFileCount
		{
			set{ _uploadfilecount=value;}
			get{return _uploadfilecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FAQCount
		{
			set{ _faqcount=value;}
			get{return _faqcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QuestionCount
		{
			set{ _questioncount=value;}
			get{return _questioncount;}
		}
		#endregion Model

	}
}

