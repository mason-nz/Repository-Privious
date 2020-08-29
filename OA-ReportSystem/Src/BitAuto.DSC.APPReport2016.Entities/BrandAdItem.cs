
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 品牌广告详情
	/// </summary>
	[Serializable]
	public partial class BrandAdItem
	{
		public BrandAdItem()
		{}
		#region Model
		private int _year;
		private int _brandid;
		private decimal _amount;
		private decimal _percent;
		private decimal? _yearbasis;
		private DateTime _createtime;
		/// <summary>
		/// 年度
		/// </summary>
		public int Year
		{
			set{ _year=value;}
			get{return _year;}
		}
		/// <summary>
		/// 品牌ID
		/// </summary>
		public int BrandId
		{
			set{ _brandid=value;}
			get{return _brandid;}
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
		/// 占比
		/// </summary>
		public decimal Percent
		{
			set{ _percent=value;}
			get{return _percent;}
		}
		/// <summary>
		/// 同比
		/// </summary>
		public decimal? YearBasis
		{
			set{ _yearbasis=value;}
			get{return _yearbasis;}
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

