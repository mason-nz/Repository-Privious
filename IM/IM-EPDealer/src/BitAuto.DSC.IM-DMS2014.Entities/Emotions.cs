using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类Emotions 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:02 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class Emotions
	{
		public Emotions()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _etext = Constant.STRING_INVALID_VALUE;
		 _ecategory = Constant.STRING_INVALID_VALUE;
		 _eurl = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _etext;
		private string _ecategory;
		private string _eurl;
		private int? _status;
		private DateTime? _createtime;
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
		public string EText
		{
			set{ _etext=value;}
			get{return _etext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ECategory
		{
			set{ _ecategory=value;}
			get{return _ecategory;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EUrl
		{
			set{ _eurl=value;}
			get{return _eurl;}
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
		#endregion Model

	}
}

