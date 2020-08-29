using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类DealerInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:17 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class DealerInfo
	{
		public DealerInfo()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _membercode = Constant.STRING_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _cityscope = Constant.INT_INVALID_VALUE;
		 _membertype = Constant.INT_INVALID_VALUE;
		 _cartype = Constant.INT_INVALID_VALUE;
		 _memberstatus = Constant.INT_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _custid;
		private string _membercode;
		private string _name;
		private int? _cityscope;
		private int? _membertype;
		private int? _cartype;
		private int? _memberstatus;
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
		public string MemberCode
		{
			set{ _membercode=value;}
			get{return _membercode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CityScope
		{
			set{ _cityscope=value;}
			get{return _cityscope;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MemberType
		{
			set{ _membertype=value;}
			get{return _membertype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarType
		{
			set{ _cartype=value;}
			get{return _cartype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MemberStatus
		{
			set{ _memberstatus=value;}
			get{return _memberstatus;}
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

