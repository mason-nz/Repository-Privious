using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryConsultSolveUserMapping 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryConsultSolveUserMapping
	{
		public QueryConsultSolveUserMapping()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _templateid = Constant.INT_INVALID_VALUE;
		 _solveusereid = Constant.INT_INVALID_VALUE;
		 _solveuserid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int? _templateid;
		private int? _solveusereid;
		private int? _solveuserid;
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
		public int? TemplateID
		{
			set{ _templateid=value;}
			get{return _templateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SolveUserEID
		{
			set{ _solveusereid=value;}
			get{return _solveusereid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SolveUserID
		{
			set{ _solveuserid=value;}
			get{return _solveuserid;}
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

