using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类TType 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class TType
	{
		public TType()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _typedesname = Constant.STRING_INVALID_VALUE;
		 _typename = Constant.STRING_INVALID_VALUE;
		 _typedes = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _typedesname;
		private string _typename;
		private string _typedes;
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
		public string TypeDesName
		{
			set{ _typedesname=value;}
			get{return _typedesname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TypeName
		{
			set{ _typename=value;}
			get{return _typename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TypeDes
		{
			set{ _typedes=value;}
			get{return _typedes;}
		}
		#endregion Model

	}
}

