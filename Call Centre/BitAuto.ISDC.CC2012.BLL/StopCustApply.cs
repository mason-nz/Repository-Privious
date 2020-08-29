using System;
using BitAuto.YanFa.HRManagement2011.Entities.Constants;

namespace BitAuto.YanFa.HRManagement2011.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类StopCustApply 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-07-01 12:18:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class StopCustApply
	{
		public StopCustApply()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _crmstopcustapplyid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _applyerid = Constant.INT_INVALID_VALUE;
		 _areaname = Constant.STRING_INVALID_VALUE;
		 _areaid = Constant.INT_INVALID_VALUE;
		 _applytime = Constant.DATE_INVALID_VALUE;
		 _audittime = Constant.DATE_INVALID_VALUE;
		 _stoptime = Constant.DATE_INVALID_VALUE;
		 _stopstatus = Constant.INT_INVALID_VALUE;
		 _rejectreason = Constant.STRING_INVALID_VALUE;
		 _auditopinion = Constant.STRING_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int? _crmstopcustapplyid;
		private string _custid;
		private int? _applyerid;
		private string _areaname;
		private int? _areaid;
		private DateTime? _applytime;
		private DateTime? _audittime;
		private DateTime? _stoptime;
		private int? _stopstatus;
		private string _rejectreason;
		private string _auditopinion;
		private string _remark;
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
		public int? CRMStopCustApplyID
		{
			set{ _crmstopcustapplyid=value;}
			get{return _crmstopcustapplyid;}
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
		public int? ApplyerID
		{
			set{ _applyerid=value;}
			get{return _applyerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AreaName
		{
			set{ _areaname=value;}
			get{return _areaname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AreaID
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ApplyTime
		{
			set{ _applytime=value;}
			get{return _applytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? AuditTime
		{
			set{ _audittime=value;}
			get{return _audittime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? StopTime
		{
			set{ _stoptime=value;}
			get{return _stoptime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? StopStatus
		{
			set{ _stopstatus=value;}
			get{return _stopstatus;}
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
		public string AuditOpinion
		{
			set{ _auditopinion=value;}
			get{return _auditopinion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

