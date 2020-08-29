using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CustBasicInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:12 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class CustBasicInfo
	{
		public CustBasicInfo()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _custname = Constant.STRING_INVALID_VALUE;
		 _sex = Constant.INT_INVALID_VALUE;
		 _custcategoryid = Constant.INT_INVALID_VALUE;
		 _provinceid = Constant.INT_INVALID_VALUE;
		 _cityid = Constant.INT_INVALID_VALUE;
		 _countyid = Constant.INT_INVALID_VALUE;
         _areaid = Constant.STRING_INVALID_VALUE;
		 _address = Constant.STRING_INVALID_VALUE;
		 _datasource = Constant.INT_INVALID_VALUE;
		 _calltime = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private string _custid;
		private string _custname;
		private int? _sex;
		private int? _custcategoryid;
		private int? _provinceid;
		private int? _cityid;
		private int? _countyid;
		private string _areaid;
		private string _address;
		private int? _datasource;
		private int? _calltime;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
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
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
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
		public int? Sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CustCategoryID
		{
			set{ _custcategoryid=value;}
			get{return _custcategoryid;}
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
		public int? CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CountyID
		{
			set{ _countyid=value;}
			get{return _countyid;}
		}
		/// <summary>
        /// 大区ID，新增和更新时，自动根据 省 城市 区县 重新计算大区信息
        /// 强斐
        /// 2014年12月17日
		/// </summary>
		public string AreaID
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DataSource
		{
			set{ _datasource=value;}
			get{return _datasource;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CallTime
		{
			set{ _calltime=value;}
			get{return _calltime;}
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
		public DateTime? ModifyTime
		{
			set{ _modifytime=value;}
			get{return _modifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ModifyUserID
		{
			set{ _modifyuserid=value;}
			get{return _modifyuserid;}
		}
		#endregion Model

	}
}

