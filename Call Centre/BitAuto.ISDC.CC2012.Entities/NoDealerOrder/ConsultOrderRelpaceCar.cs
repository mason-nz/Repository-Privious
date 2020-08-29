using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ConsultOrderRelpaceCar 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ConsultOrderRelpaceCar
	{
		public ConsultOrderRelpaceCar()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _wantbrandid = Constant.INT_INVALID_VALUE;
		 _wantserialid = Constant.INT_INVALID_VALUE;
		 _wantnameid = Constant.INT_INVALID_VALUE;
		 _wantcarcolor = Constant.STRING_INVALID_VALUE;
		 _wantdealername = Constant.STRING_INVALID_VALUE;
		 _wantdealercode = Constant.STRING_INVALID_VALUE;
		 _callrecord = Constant.STRING_INVALID_VALUE;
		 _oldbrandid = Constant.INT_INVALID_VALUE;
		 _oldserialid = Constant.INT_INVALID_VALUE;
		 _oldnameid = Constant.INT_INVALID_VALUE;
		 _oldcarcolor = Constant.STRING_INVALID_VALUE;
		 _registerdateyear = Constant.STRING_INVALID_VALUE;
		 _registerdatemonth = Constant.STRING_INVALID_VALUE;
		 _registerprovinceid = Constant.INT_INVALID_VALUE;
		 _registercityid = Constant.INT_INVALID_VALUE;
         _registercountyid = Constant.INT_INVALID_VALUE;
		 _mileage = Constant.INT_INVALID_VALUE;
		 _presellprice = Constant.DECIMAL_INVALID_VALUE;
		 _orderremark = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _custid;
		private int? _wantbrandid;
		private int? _wantserialid;
		private int? _wantnameid;
		private string _wantcarcolor;
		private string _wantdealername;
		private string _wantdealercode;
		private string _callrecord;
		private int? _oldbrandid;
		private int? _oldserialid;
		private int? _oldnameid;
		private string _oldcarcolor;
		private string _registerdateyear;
		private string _registerdatemonth;
		private int? _registerprovinceid;
		private int? _registercityid;
        private int? _registercountyid;
		private decimal? _mileage;
		private decimal? _presellprice;
		private string _orderremark;
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
		public int? WantBrandId
		{
			set{ _wantbrandid=value;}
			get{return _wantbrandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WantSerialId
		{
			set{ _wantserialid=value;}
			get{return _wantserialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WantNameID
		{
			set{ _wantnameid=value;}
			get{return _wantnameid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WantCarColor
		{
			set{ _wantcarcolor=value;}
			get{return _wantcarcolor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WantDealerName
		{
			set{ _wantdealername=value;}
			get{return _wantdealername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WantDealerCode
		{
			set{ _wantdealercode=value;}
			get{return _wantdealercode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CallRecord
		{
			set{ _callrecord=value;}
			get{return _callrecord;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OldBrandId
		{
			set{ _oldbrandid=value;}
			get{return _oldbrandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OldSerialId
		{
			set{ _oldserialid=value;}
			get{return _oldserialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OldNameID
		{
			set{ _oldnameid=value;}
			get{return _oldnameid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OldCarColor
		{
			set{ _oldcarcolor=value;}
			get{return _oldcarcolor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RegisterDateYear
		{
			set{ _registerdateyear=value;}
			get{return _registerdateyear;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RegisterDateMonth
		{
			set{ _registerdatemonth=value;}
			get{return _registerdatemonth;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RegisterProvinceID
		{
			set{ _registerprovinceid=value;}
			get{return _registerprovinceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RegisterCityID
		{
			set{ _registercityid=value;}
			get{return _registercityid;}
		}
        public int? RegisterCountyID
		{
            set { _registercountyid = value; }
            get { return _registercountyid; }
		}
     
		/// <summary>
		/// 
		/// </summary>
        public decimal? Mileage
		{
			set{ _mileage=value;}
			get{return _mileage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? PresellPrice
		{
			set{ _presellprice=value;}
			get{return _presellprice;}
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

