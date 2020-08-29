
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 品牌广告
	/// </summary>
	[Serializable]
	public partial class BrandAd
	{
		public BrandAd()
		{}
		#region Model
		private int _yearmonth;
		private int _count;
		private decimal _amount;
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
		/// 合作数
		/// </summary>
		public int Count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 收入
		/// </summary>
		public decimal Amount
		{
			set{ _amount=value;}
			get{return _amount;}
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

