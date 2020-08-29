using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryProjectTask_DMSMember_Brand 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryProjectTask_DMSMember_Brand
	{
		public QueryProjectTask_DMSMember_Brand()
		{
		 _memberid = Constant.INT_INVALID_VALUE;
		 _brandid = Constant.INT_INVALID_VALUE;
		 _serialid = Constant.STRING_INVALID_VALUE;
		 _type = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _memberid;
		private int _brandid;
		private string _serialid;
		private string _type;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int MemberID
		{
			set{ _memberid=value;}
			get{return _memberid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BrandID
		{
			set{ _brandid=value;}
			get{return _brandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SerialID
		{
			set{ _serialid=value;}
			get{return _serialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

