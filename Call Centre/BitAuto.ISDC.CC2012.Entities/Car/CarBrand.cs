using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CarBrand 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-21 05:15:21 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class CarBrand
	{
		public CarBrand()
		{
		 _brandid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _country = Constant.STRING_INVALID_VALUE;
		 _newcountry = Constant.STRING_INVALID_VALUE;
		 _allspell = Constant.STRING_INVALID_VALUE;
		 _spell = Constant.STRING_INVALID_VALUE;
		 _brandseoname = Constant.STRING_INVALID_VALUE;
		 _masterbrandid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _brandid;
		private string _name;
		private string _country;
		private string _newcountry;
		private string _allspell;
		private string _spell;
		private string _brandseoname;
		private int? _masterbrandid;
		private DateTime? _createtime;
		private DateTime? _modifytime;
		/// <summary>
		/// 
		/// </summary>
		public int BrandID
		{
			set{ _brandid=value;}
			get{return _brandid;}
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
		public string Country
		{
			set{ _country=value;}
			get{return _country;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NewCountry
		{
			set{ _newcountry=value;}
			get{return _newcountry;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AllSpell
		{
			set{ _allspell=value;}
			get{return _allspell;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Spell
		{
			set{ _spell=value;}
			get{return _spell;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BrandSEOName
		{
			set{ _brandseoname=value;}
			get{return _brandseoname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MasterBrandID
		{
			set{ _masterbrandid=value;}
			get{return _masterbrandid;}
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
		public DateTime? ModifyTime
		{
			set{ _modifytime=value;}
			get{return _modifytime;}
		}
		#endregion Model

	}
}

