using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryTTable 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryTTable
	{
		public QueryTTable()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _ttcode = Constant.STRING_INVALID_VALUE;
		 _ttdesname = Constant.STRING_INVALID_VALUE;
		 _ttname = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _ttisdata = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _ttcode;
		private string _ttdesname;
		private string _ttname;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
        private int? _ttisdata;
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
		public string TTCode
		{
			set{ _ttcode=value;}
			get{return _ttcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TTDesName
		{
			set{ _ttdesname=value;}
			get{return _ttdesname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TTName
		{
			set{ _ttname=value;}
			get{return _ttname;}
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
		public int? TTIsData
		{
            set { _ttisdata = value; }
            get { return _ttisdata; }
		}
		#endregion Model

	}
}

