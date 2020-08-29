using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CarSerial 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-12-11 03:57:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class CarSerial
	{
		public CarSerial()
		{
		 _csid = Constant.INT_INVALID_VALUE;
		 _masterbrandid = Constant.INT_INVALID_VALUE;
         _brandid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _oldcbid = Constant.STRING_INVALID_VALUE;
		 _cslevel = Constant.STRING_INVALID_VALUE;
		 _multipricerange = Constant.STRING_INVALID_VALUE;
		 _cssalestate = Constant.STRING_INVALID_VALUE;
		 _allspell = Constant.STRING_INVALID_VALUE;
		 _csmultichar = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _csid;
		private int _masterbrandid;
        private int _brandid;
		private string _name;
		private string _oldcbid;
		private string _cslevel;
		private string _multipricerange;
		private string _cssalestate;
		private string _allspell;
		private string _csmultichar;
		private DateTime? _createtime;
		private DateTime? _modifytime;
		/// <summary>
		/// 
		/// </summary>
		public int CSID
		{
			set{ _csid=value;}
			get{return _csid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MasterBrandID
		{
			set{ _masterbrandid=value;}
			get{return _masterbrandid;}
		}
        public int BrandID
		{
            set { _brandid = value; }
            get { return _brandid; }
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
		public string OldCbID
		{
			set{ _oldcbid=value;}
			get{return _oldcbid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CsLevel
		{
			set{ _cslevel=value;}
			get{return _cslevel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MultiPriceRange
		{
			set{ _multipricerange=value;}
			get{return _multipricerange;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CsSaleState
		{
			set{ _cssalestate=value;}
			get{return _cssalestate;}
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
		public string CsMultiChar
		{
			set{ _csmultichar=value;}
			get{return _csmultichar;}
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

