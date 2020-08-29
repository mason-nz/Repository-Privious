using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryBuyCarInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:07 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryBuyCarInfo
	{
		public QueryBuyCarInfo()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _type = Constant.INT_INVALID_VALUE;
		 _age = Constant.INT_INVALID_VALUE;
		 _idcard = Constant.STRING_INVALID_VALUE;
		 _vocation = Constant.INT_INVALID_VALUE;
		 _marriage = Constant.STRING_INVALID_VALUE;
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

		}
		#region Model
		private int _recid;
		private string _custid;
		private int? _type;
		private int? _age;
		private string _idcard;
		private int? _vocation;
		private string _marriage;
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
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
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
		public string Marriage
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
		#endregion Model

	}
}

