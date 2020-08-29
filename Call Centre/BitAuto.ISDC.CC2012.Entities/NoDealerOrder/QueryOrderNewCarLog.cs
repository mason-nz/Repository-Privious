using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryOrderNewCarLog 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryOrderNewCarLog
	{
		public QueryOrderNewCarLog()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _yporderid = Constant.INT_INVALID_VALUE;
		 _username = Constant.STRING_INVALID_VALUE;
		 _userphone = Constant.STRING_INVALID_VALUE;
		 _usermobile = Constant.STRING_INVALID_VALUE;
		 _usermail = Constant.STRING_INVALID_VALUE;
		 _usergender = Constant.INT_INVALID_VALUE;
		 _locationid = Constant.INT_INVALID_VALUE;
		 _locationname = Constant.STRING_INVALID_VALUE;
		 _useraddress = Constant.STRING_INVALID_VALUE;
		 _ordercreatetime = Constant.DATE_INVALID_VALUE;
		 _orderprice = Constant.DECIMAL_INVALID_VALUE;
		 _orderquantity = Constant.INT_INVALID_VALUE;
		 _orderremark = Constant.STRING_INVALID_VALUE;
		 _carid = Constant.INT_INVALID_VALUE;
		 _carfullname = Constant.STRING_INVALID_VALUE;
		 _carprice = Constant.DECIMAL_INVALID_VALUE;
		 _carcolor = Constant.STRING_INVALID_VALUE;
		 _carpromotions = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private int? _yporderid;
		private string _username;
		private string _userphone;
		private string _usermobile;
		private string _usermail;
		private int? _usergender;
		private int? _locationid;
		private string _locationname;
		private string _useraddress;
		private DateTime? _ordercreatetime;
		private decimal? _orderprice;
		private int? _orderquantity;
		private string _orderremark;
		private int? _carid;
		private string _carfullname;
		private decimal? _carprice;
		private string _carcolor;
		private string _carpromotions;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public long RecID
		{
			set{ _recid=value;}
			get{return _recid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? YPOrderID
		{
			set{ _yporderid=value;}
			get{return _yporderid;}
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
		public string UserPhone
		{
			set{ _userphone=value;}
			get{return _userphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserMobile
		{
			set{ _usermobile=value;}
			get{return _usermobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserMail
		{
			set{ _usermail=value;}
			get{return _usermail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UserGender
		{
			set{ _usergender=value;}
			get{return _usergender;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LocationID
		{
			set{ _locationid=value;}
			get{return _locationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LocationName
		{
			set{ _locationname=value;}
			get{return _locationname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserAddress
		{
			set{ _useraddress=value;}
			get{return _useraddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OrderCreateTime
		{
			set{ _ordercreatetime=value;}
			get{return _ordercreatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? OrderPrice
		{
			set{ _orderprice=value;}
			get{return _orderprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrderQuantity
		{
			set{ _orderquantity=value;}
			get{return _orderquantity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderRemark
		{
			set{ _orderremark=value;}
			get{return _orderremark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarID
		{
			set{ _carid=value;}
			get{return _carid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarFullName
		{
			set{ _carfullname=value;}
			get{return _carfullname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? CarPrice
		{
			set{ _carprice=value;}
			get{return _carprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarColor
		{
			set{ _carcolor=value;}
			get{return _carcolor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarPromotions
		{
			set{ _carpromotions=value;}
			get{return _carpromotions;}
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

