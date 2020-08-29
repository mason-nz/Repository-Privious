using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QS_ApprovalHistory 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:35 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QS_ApprovalHistory
	{
		public QS_ApprovalHistory()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _qs_rtid = Constant.INT_INVALID_VALUE;
		 _qs_rid = Constant.INT_INVALID_VALUE;
		 _type = Constant.STRING_INVALID_VALUE;
		 _approvaltype = Constant.STRING_INVALID_VALUE;
		 _approvalresult = Constant.INT_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int? _qs_rtid;
		private int? _qs_rid;
		private string _type;
		private string _approvaltype;
		private int? _approvalresult;
		private string _remark;
		private int? _status;
		private DateTime? _createtime;
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
		public int? QS_RTID
		{
			set{ _qs_rtid=value;}
			get{return _qs_rtid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QS_RID
		{
			set{ _qs_rid=value;}
			get{return _qs_rid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ApprovalType
		{
			set{ _approvaltype=value;}
			get{return _approvaltype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ApprovalResult
		{
			set{ _approvalresult=value;}
			get{return _approvalresult;}
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
		#endregion Model

	}
}

