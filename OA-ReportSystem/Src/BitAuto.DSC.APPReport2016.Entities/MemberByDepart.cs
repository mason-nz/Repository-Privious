
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 会员按部门统计
	/// </summary>
	[Serializable]
	public partial class MemberByDepart
	{
		public MemberByDepart()
		{}
		#region Model
		private int _yearmonth;
		private int _itemid;
		private string _departid;
		private int _total;
		private int _count;
		private decimal? _monthbasis;
		private DateTime _createtime;
		/// <summary>
		/// 月份
		/// </summary>
		public int YearMonth
		{
			set{ _yearmonth=value;}
			get{return _yearmonth;}
		}
		/// <summary>
		/// 统计项目ID
		/// </summary>
		public int ItemId
		{
			set{ _itemid=value;}
			get{return _itemid;}
		}
		/// <summary>
		/// 部门ID
		/// </summary>
		public string DepartId
		{
			set{ _departid=value;}
			get{return _departid;}
		}
		/// <summary>
		/// 总数
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// 合作数
		/// </summary>
		public int Count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 同比
		/// </summary>
		public decimal? MonthBasis
		{
			set{ _monthbasis=value;}
			get{return _monthbasis;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

