using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryWorkOrderRevert 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-08-23 10:24:21 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryWorkOrderRevert
	{
		public QueryWorkOrderRevert()
		{
		 _worid = Constant.INT_INVALID_VALUE;
		 _orderid = Constant.STRING_INVALID_VALUE;
		 _revertcontent = Constant.STRING_INVALID_VALUE;
		 _categoryname = Constant.STRING_INVALID_VALUE;
		 _datasource = Constant.STRING_INVALID_VALUE;
		 _custname = Constant.STRING_INVALID_VALUE;
		 _crmcustid = Constant.STRING_INVALID_VALUE;
		 _provincename = Constant.STRING_INVALID_VALUE;
		 _cityname = Constant.STRING_INVALID_VALUE;
		 _countyname = Constant.STRING_INVALID_VALUE;
		 _contact = Constant.STRING_INVALID_VALUE;
		 _contacttel = Constant.STRING_INVALID_VALUE;
		 _prioritylevelname = Constant.STRING_INVALID_VALUE;
		 _lastprocessdate = Constant.STRING_INVALID_VALUE;
		 _iscomplainttype = Constant.STRING_INVALID_VALUE;
		 _title = Constant.STRING_INVALID_VALUE;
		 _tagname = Constant.STRING_INVALID_VALUE;
		 _workorderstatus = Constant.STRING_INVALID_VALUE;
		 _receiverid = Constant.STRING_INVALID_VALUE;
		 _receivername = Constant.STRING_INVALID_VALUE;
		 _receiverdepartname = Constant.STRING_INVALID_VALUE;
		 _issales = Constant.STRING_INVALID_VALUE;
		 _attentioncarbrandname = Constant.STRING_INVALID_VALUE;
		 _attentioncarserialname = Constant.STRING_INVALID_VALUE;
		 _attentioncartypename = Constant.STRING_INVALID_VALUE;
		 _selectdealerid = Constant.STRING_INVALID_VALUE;
		 _selectdealername = Constant.STRING_INVALID_VALUE;
		 _nominateactivity = Constant.STRING_INVALID_VALUE;
		 _salecarbrandname = Constant.STRING_INVALID_VALUE;
		 _salecarserialname = Constant.STRING_INVALID_VALUE;
		 _salecartypename = Constant.STRING_INVALID_VALUE;
		 _isreturnvisit = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _callid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _worid;
		private string _orderid;
		private string _revertcontent;
		private string _categoryname;
		private string _datasource;
		private string _custname;
		private string _crmcustid;
		private string _provincename;
		private string _cityname;
		private string _countyname;
		private string _contact;
		private string _contacttel;
		private string _prioritylevelname;
		private string _lastprocessdate;
		private string _iscomplainttype;
		private string _title;
		private string _tagname;
		private string _workorderstatus;
		private string _receiverid;
		private string _receivername;
		private string _receiverdepartname;
		private string _issales;
		private string _attentioncarbrandname;
		private string _attentioncarserialname;
		private string _attentioncartypename;
		private string _selectdealerid;
		private string _selectdealername;
		private string _nominateactivity;
		private string _salecarbrandname;
		private string _salecarserialname;
		private string _salecartypename;
		private string _isreturnvisit;
		private DateTime? _createtime;
		private int? _createuserid;
		private Int64? _callid;
		/// <summary>
		/// 
		/// </summary>
		public long WORID
		{
			set{ _worid=value;}
			get{return _worid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RevertContent
		{
			set{ _revertcontent=value;}
			get{return _revertcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CategoryName
		{
			set{ _categoryname=value;}
			get{return _categoryname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DataSource
		{
			set{ _datasource=value;}
			get{return _datasource;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CustName
		{
			set{ _custname=value;}
			get{return _custname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CRMCustID
		{
			set{ _crmcustid=value;}
			get{return _crmcustid;}
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
		public string CityName
		{
			set{ _cityname=value;}
			get{return _cityname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CountyName
		{
			set{ _countyname=value;}
			get{return _countyname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Contact
		{
			set{ _contact=value;}
			get{return _contact;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ContactTel
		{
			set{ _contacttel=value;}
			get{return _contacttel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PriorityLevelName
		{
			set{ _prioritylevelname=value;}
			get{return _prioritylevelname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LastProcessDate
		{
			set{ _lastprocessdate=value;}
			get{return _lastprocessdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IsComplaintType
		{
			set{ _iscomplainttype=value;}
			get{return _iscomplainttype;}
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
		public string TagName
		{
			set{ _tagname=value;}
			get{return _tagname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkOrderStatus
		{
			set{ _workorderstatus=value;}
			get{return _workorderstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReceiverID
		{
			set{ _receiverid=value;}
			get{return _receiverid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReceiverName
		{
			set{ _receivername=value;}
			get{return _receivername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReceiverDepartName
		{
			set{ _receiverdepartname=value;}
			get{return _receiverdepartname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IsSales
		{
			set{ _issales=value;}
			get{return _issales;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttentionCarBrandName
		{
			set{ _attentioncarbrandname=value;}
			get{return _attentioncarbrandname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttentionCarSerialName
		{
			set{ _attentioncarserialname=value;}
			get{return _attentioncarserialname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttentionCarTypeName
		{
			set{ _attentioncartypename=value;}
			get{return _attentioncartypename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SelectDealerID
		{
			set{ _selectdealerid=value;}
			get{return _selectdealerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SelectDealerName
		{
			set{ _selectdealername=value;}
			get{return _selectdealername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NominateActivity
		{
			set{ _nominateactivity=value;}
			get{return _nominateactivity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SaleCarBrandName
		{
			set{ _salecarbrandname=value;}
			get{return _salecarbrandname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SaleCarSerialName
		{
			set{ _salecarserialname=value;}
			get{return _salecarserialname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SaleCarTypeName
		{
			set{ _salecartypename=value;}
			get{return _salecartypename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IsReturnVisit
		{
			set{ _isreturnvisit=value;}
			get{return _isreturnvisit;}
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
		public Int64? CallID
		{
			set{ _callid=value;}
			get{return _callid;}
		}
		#endregion Model

	}
}

