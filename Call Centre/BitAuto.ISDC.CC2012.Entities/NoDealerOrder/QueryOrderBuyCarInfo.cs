using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryOrderBuyCarInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryOrderBuyCarInfo
	{
		public QueryOrderBuyCarInfo()
		{
		 _taskid = Constant.INT_INVALID_VALUE;
		 _type = Constant.INT_INVALID_VALUE;
		 _age = Constant.INT_INVALID_VALUE;
		 _idcard = Constant.STRING_INVALID_VALUE;
		 _vocation = Constant.INT_INVALID_VALUE;
		 _marriage = Constant.INT_INVALID_VALUE;
		 _income = Constant.INT_INVALID_VALUE;
		 _carbrandid = Constant.INT_INVALID_VALUE;
		 _carserialid = Constant.INT_INVALID_VALUE;
		 _carname = Constant.STRING_INVALID_VALUE;
		 _isattestation = Constant.INT_INVALID_VALUE;
		 _driveage = Constant.INT_INVALID_VALUE;
		 _username = Constant.STRING_INVALID_VALUE;
		 _carno = Constant.STRING_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastmodifytime = Constant.DATE_INVALID_VALUE;
		 _lastmodifyuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _taskid;
		private int? _type;
		private int? _age;
		private string _idcard;
		private int? _vocation;
		private int? _marriage;
		private int? _income;
		private int? _carbrandid;
		private int? _carserialid;
		private string _carname;
		private int? _isattestation;
		private int? _driveage;
		private string _username;
		private string _carno;
		private string _remark;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastmodifytime;
		private int? _lastmodifyuserid;
		/// <summary>
		/// 
		/// </summary>
		public long TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Age
		{
			set{ _age=value;}
			get{return _age;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IDCard
		{
			set{ _idcard=value;}
			get{return _idcard;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Vocation
		{
			set{ _vocation=value;}
			get{return _vocation;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Marriage
		{
			set{ _marriage=value;}
			get{return _marriage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Income
		{
			set{ _income=value;}
			get{return _income;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarBrandId
		{
			set{ _carbrandid=value;}
			get{return _carbrandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarSerialId
		{
			set{ _carserialid=value;}
			get{return _carserialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarName
		{
			set{ _carname=value;}
			get{return _carname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsAttestation
		{
			set{ _isattestation=value;}
			get{return _isattestation;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DriveAge
		{
			set{ _driveage=value;}
			get{return _driveage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarNo
		{
			set{ _carno=value;}
			get{return _carno;}
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
		#endregion Model

	}
}

