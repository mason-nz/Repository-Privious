using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_AdditionalStatus 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:28 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_AdditionalStatus
	{
		public ProjectTask_AdditionalStatus()
		{
		 _ptid = Constant.STRING_INVALID_VALUE;
		 _additionalstatus = Constant.STRING_INVALID_VALUE;
		 _description = Constant.STRING_INVALID_VALUE;

		}
		#region Model
		private string _ptid;
		private string _additionalstatus;
		private string _description;
		/// <summary>
		/// 
		/// </summary>
		public string PTID
		{
			set{ _ptid=value;}
			get{return _ptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdditionalStatus
		{
			set{ _additionalstatus=value;}
			get{return _additionalstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		#endregion Model

	}
}

