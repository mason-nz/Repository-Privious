using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryFreProblem 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:03 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryFreProblem
	{
		public QueryFreProblem()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _title = Constant.STRING_INVALID_VALUE;
		 _url = Constant.STRING_INVALID_VALUE;
		 _sortnum = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _lastupdatetime = Constant.DATE_INVALID_VALUE;
		 _lastupdateuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _title;
		private string _url;
		private int? _sortnum;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _lastupdatetime;
		private int? _lastupdateuserid;
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
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SortNum
		{
			set{ _sortnum=value;}
			get{return _sortnum;}
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
		public DateTime? LastUpdateTime
		{
			set{ _lastupdatetime=value;}
			get{return _lastupdatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastUpdateUserID
		{
			set{ _lastupdateuserid=value;}
			get{return _lastupdateuserid;}
		}
		#endregion Model

	}
}

