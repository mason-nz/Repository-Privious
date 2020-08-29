using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_CSTMember 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_CSTMember
	{
		public ProjectTask_CSTMember()
		{
		 _id = Constant.INT_INVALID_VALUE;
		 _ptid = Constant.STRING_INVALID_VALUE;
		 _originalcstrecid = Constant.STRING_INVALID_VALUE;
		 _vendorcode = Constant.STRING_INVALID_VALUE;
		 _fullname = Constant.STRING_INVALID_VALUE;
		 _shortname = Constant.STRING_INVALID_VALUE;
		 _vendorclass = Constant.INT_INVALID_VALUE;
		 _superid = Constant.INT_INVALID_VALUE;
		 _provinceid = Constant.STRING_INVALID_VALUE;
		 _cityid = Constant.STRING_INVALID_VALUE;
		 _countyid = Constant.STRING_INVALID_VALUE;
		 _address = Constant.STRING_INVALID_VALUE;
		 _postcode = Constant.STRING_INVALID_VALUE;
		 _trafficinfo = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _id;
		private string _ptid;
		private string _originalcstrecid;
		private string _vendorcode;
		private string _fullname;
		private string _shortname;
		private int? _vendorclass;
		private int? _superid;
		private string _provinceid;
		private string _cityid;
		private string _countyid;
		private string _address;
		private string _postcode;
		private string _trafficinfo;
		private DateTime? _createtime;
		private int? _createuserid;
		private int? _status;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PTID
		{
			set{ _ptid=value;}
			get{return _ptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OriginalCSTRecID
		{
			set{ _originalcstrecid=value;}
			get{return _originalcstrecid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string VendorCode
		{
			set{ _vendorcode=value;}
			get{return _vendorcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FullName
		{
			set{ _fullname=value;}
			get{return _fullname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ShortName
		{
			set{ _shortname=value;}
			get{return _shortname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? VendorClass
		{
			set{ _vendorclass=value;}
			get{return _vendorclass;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SuperId
		{
			set{ _superid=value;}
			get{return _superid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProvinceID
		{
			set{ _provinceid=value;}
			get{return _provinceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CountyID
		{
			set{ _countyid=value;}
			get{return _countyid;}
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
		public string PostCode
		{
			set{ _postcode=value;}
			get{return _postcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TrafficInfo
		{
			set{ _trafficinfo=value;}
			get{return _trafficinfo;}
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

