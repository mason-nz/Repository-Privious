using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类GroupOrderOrg 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class GroupOrderOrg
	{
		public GroupOrderOrg()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _orderid = Constant.INT_INVALID_VALUE;
		 _ordercode = Constant.INT_INVALID_VALUE;
		 _customername = Constant.STRING_INVALID_VALUE;
		 _customertel = Constant.STRING_INVALID_VALUE;
		 _provinceid = Constant.INT_INVALID_VALUE;
		 _provincename = Constant.STRING_INVALID_VALUE;
		 _cityid = Constant.INT_INVALID_VALUE;
		 _cityname = Constant.STRING_INVALID_VALUE;
		 _ordercreatetime = Constant.DATE_INVALID_VALUE;
		 _price = Constant.DECIMAL_INVALID_VALUE;
		 _carmasterid = Constant.INT_INVALID_VALUE;
		 _carmastername = Constant.STRING_INVALID_VALUE;
		 _carserialid = Constant.INT_INVALID_VALUE;
		 _carserialname = Constant.STRING_INVALID_VALUE;
		 _carid = Constant.INT_INVALID_VALUE;
		 _carname = Constant.STRING_INVALID_VALUE;
		 _dealerid = Constant.INT_INVALID_VALUE;
		 _dealername = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private int? _orderid;
		private int? _ordercode;
		private string _customername;
		private string _customertel;
		private int? _provinceid;
		private string _provincename;
		private int? _cityid;
		private string _cityname;
		private DateTime? _ordercreatetime;
		private decimal? _price;
		private int? _carmasterid;
		private string _carmastername;
		private int? _carserialid;
		private string _carserialname;
		private int? _carid;
		private string _carname;
		private int? _dealerid;
		private string _dealername;
		private DateTime? _createtime;
		private int? _createuserid;
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
		public int? OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrderCode
		{
			set{ _ordercode=value;}
			get{return _ordercode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CustomerName
		{
			set{ _customername=value;}
			get{return _customername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CustomerTel
		{
			set{ _customertel=value;}
			get{return _customertel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ProvinceID
		{
			set{ _provinceid=value;}
			get{return _provinceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProvinceName
		{
			set{ _provincename=value;}
			get{return _provincename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CityName
		{
			set{ _cityname=value;}
			get{return _cityname;}
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
		public decimal? Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarMasterID
		{
			set{ _carmasterid=value;}
			get{return _carmasterid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarMasterName
		{
			set{ _carmastername=value;}
			get{return _carmastername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarSerialID
		{
			set{ _carserialid=value;}
			get{return _carserialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarSerialName
		{
			set{ _carserialname=value;}
			get{return _carserialname;}
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
		public string CarName
		{
			set{ _carname=value;}
			get{return _carname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DealerID
		{
			set{ _dealerid=value;}
			get{return _dealerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DealerName
		{
			set{ _dealername=value;}
			get{return _dealername;}
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

