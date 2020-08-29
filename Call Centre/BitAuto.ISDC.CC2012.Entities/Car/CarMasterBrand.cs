using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CarMasterBrand 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-12-11 03:57:10 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class CarMasterBrand
	{
		public CarMasterBrand()
		{
		 _masterbrandid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _ename = Constant.STRING_INVALID_VALUE;
		 _country = Constant.STRING_INVALID_VALUE;
		 _allspell = Constant.STRING_INVALID_VALUE;
		 _spell = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _masterbrandid;
		private string _name;
		private string _ename;
		private string _country;
		private string _allspell;
		private string _spell;
		private DateTime? _createtime;
		private DateTime? _modifytime;
		/// <summary>
		/// 
		/// </summary>
		public int MasterBrandID
		{
			set{ _masterbrandid=value;}
			get{return _masterbrandid;}
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
		public string EName
		{
			set{ _ename=value;}
			get{return _ename;}
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

