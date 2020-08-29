using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类GO_FailureReason 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-11-04 09:34:13 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class GO_FailureReason
	{
		public GO_FailureReason()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _reasonname = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _reasonname;
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
		public string ReasonName
		{
			set{ _reasonname=value;}
			get{return _reasonname;}
		}
		#endregion Model

	}
}

