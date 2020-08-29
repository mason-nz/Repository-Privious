using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryTaskCurrentSolveUser 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:21 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryTaskCurrentSolveUser
	{
		public QueryTaskCurrentSolveUser()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _taskid = Constant.STRING_INVALID_VALUE;
		 _currentsolveusereid = Constant.INT_INVALID_VALUE;
		 _currentsolveuserid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuseradname = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _taskid;
		private int? _currentsolveusereid;
		private int? _currentsolveuserid;
		private int? _status;
		private DateTime? _createtime;
		private string _createuseradname;
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
		public string TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CurrentSolveUserEID
		{
			set{ _currentsolveusereid=value;}
			get{return _currentsolveusereid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CurrentSolveUserID
		{
			set{ _currentsolveuserid=value;}
			get{return _currentsolveuserid;}
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
		public string CreateUserAdName
		{
			set{ _createuseradname=value;}
			get{return _createuseradname;}
		}
		#endregion Model

	}
}

