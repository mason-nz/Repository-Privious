using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryKLAnswerOption 。(属性说明自动提取数据库字段的描述信息)
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
	public class QueryKLAnswerOption
	{
		public QueryKLAnswerOption()
		{
		 _klaoid = Constant.INT_INVALID_VALUE;
		 _klqid = Constant.INT_INVALID_VALUE;
		 _answer = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _klaoid;
		private long _klqid;
		private string _answer;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
		/// <summary>
		/// 
		/// </summary>
		public long KLAOID
		{
			set{ _klaoid=value;}
			get{return _klaoid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long KLQID
		{
			set{ _klqid=value;}
			get{return _klqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer
		{
			set{ _answer=value;}
			get{return _answer;}
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
		public DateTime? ModifyTime
		{
			set{ _modifytime=value;}
			get{return _modifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ModifyUserID
		{
			set{ _modifyuserid=value;}
			get{return _modifyuserid;}
		}
		#endregion Model

	}
}

