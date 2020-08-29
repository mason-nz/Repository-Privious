using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryDealerBrandInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryDealerBrandInfo
	{
		public QueryDealerBrandInfo()
		{
		 _custid = Constant.STRING_INVALID_VALUE;
		 _dealerid = Constant.INT_INVALID_VALUE;
		 _brandid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private string _custid;
		private int _dealerid;
		private int _brandid;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int DealerID
		{
			set{ _dealerid=value;}
			get{return _dealerid;}
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

