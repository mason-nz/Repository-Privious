using System;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryUserSatisfaction 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:05 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryUserSatisfaction
	{
		public QueryUserSatisfaction()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _csid = Constant.INT_INVALID_VALUE;
		 _persatisfaction = Constant.INT_INVALID_VALUE;
		 _prosatisfaction = Constant.INT_INVALID_VALUE;
		 _contents = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int _csid;
		private int _persatisfaction;
		private int _prosatisfaction;
		private string _contents;
		private DateTime _createtime;
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
		public int CSID
		{
			set{ _csid=value;}
			get{return _csid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PerSatisfaction
		{
			set{ _persatisfaction=value;}
			get{return _persatisfaction;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ProSatisfaction
		{
			set{ _prosatisfaction=value;}
			get{return _prosatisfaction;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Contents
		{
			set{ _contents=value;}
			get{return _contents;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

