using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类TField 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-03-20 03:24:42 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class TField
	{
		public TField()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _tfcode = Constant.STRING_INVALID_VALUE;
		 _tfdesname = Constant.STRING_INVALID_VALUE;
		 _tfname = Constant.STRING_INVALID_VALUE;
		 _ttcode = Constant.STRING_INVALID_VALUE;
		 _ttypeid = Constant.INT_INVALID_VALUE;
		 _tflen = Constant.INT_INVALID_VALUE;
		 _tfdes = Constant.STRING_INVALID_VALUE;
		 _tfinportisnull = Constant.INT_INVALID_VALUE;
		 _tfisnull = Constant.INT_INVALID_VALUE;
		 _tfsortindex = Constant.INT_INVALID_VALUE;
		 _tfcssname = Constant.STRING_INVALID_VALUE;
		 _tfisexportshow = Constant.INT_INVALID_VALUE;
		 _tfislistshow = Constant.INT_INVALID_VALUE;
		 _tfshowcode = Constant.STRING_INVALID_VALUE;
		 _tfvalue = Constant.STRING_INVALID_VALUE;
		 _tfgensubfield = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _tfcode;
		private string _tfdesname;
		private string _tfname;
		private string _ttcode;
		private int? _ttypeid;
		private int? _tflen;
		private string _tfdes;
		private int? _tfinportisnull;
		private int? _tfisnull;
		private int? _tfsortindex;
		private string _tfcssname;
		private int? _tfisexportshow;
		private int? _tfislistshow;
		private string _tfshowcode;
		private string _tfvalue;
		private string _tfgensubfield;
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
		public string TFCode
		{
			set{ _tfcode=value;}
			get{return _tfcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFDesName
		{
			set{ _tfdesname=value;}
			get{return _tfdesname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFName
		{
			set{ _tfname=value;}
			get{return _tfname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TTCode
		{
			set{ _ttcode=value;}
			get{return _ttcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TTypeID
		{
			set{ _ttypeid=value;}
			get{return _ttypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TFLen
		{
			set{ _tflen=value;}
			get{return _tflen;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFDes
		{
			set{ _tfdes=value;}
			get{return _tfdes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TFInportIsNull
		{
			set{ _tfinportisnull=value;}
			get{return _tfinportisnull;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TFIsNull
		{
			set{ _tfisnull=value;}
			get{return _tfisnull;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TFSortIndex
		{
			set{ _tfsortindex=value;}
			get{return _tfsortindex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFCssName
		{
			set{ _tfcssname=value;}
			get{return _tfcssname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TFIsExportShow
		{
			set{ _tfisexportshow=value;}
			get{return _tfisexportshow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TFIsListShow
		{
			set{ _tfislistshow=value;}
			get{return _tfislistshow;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFShowCode
		{
			set{ _tfshowcode=value;}
			get{return _tfshowcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFValue
		{
			set{ _tfvalue=value;}
			get{return _tfvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TFGenSubField
		{
			set{ _tfgensubfield=value;}
			get{return _tfgensubfield;}
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

