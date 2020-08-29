using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryKLOptionLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryKLOptionLog
	{
		public QueryKLOptionLog()
		{
		 _kloptid = Constant.INT_INVALID_VALUE;
		 _klid = Constant.INT_INVALID_VALUE;
		 _optstatus = Constant.INT_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _kloptid;
		private long _klid;
		private int? _optstatus;
		private string _remark;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public long KLOptID
		{
			set{ _kloptid=value;}
			get{return _kloptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long KLID
		{
			set{ _klid=value;}
			get{return _klid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OptStatus
		{
			set{ _optstatus=value;}
			get{return _optstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
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

