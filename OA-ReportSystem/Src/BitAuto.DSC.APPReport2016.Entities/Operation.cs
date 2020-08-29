
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 运营日报
	/// </summary>
	[Serializable]
	public partial class Operation
	{
		public Operation()
		{}
		#region Model
		private DateTime _date;
		private int _itemid;
		private long _count;
		private decimal? _weekbasis;
		private decimal? _daybasis;
		private DateTime _createtime;
		/// <summary>
		/// 日期
		/// </summary>
		public DateTime Date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 运营统计项目ID
		/// </summary>
		public int ItemId
		{
			set{ _itemid=value;}
			get{return _itemid;}
		}
		/// <summary>
		/// 总数
		/// </summary>
		public long Count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 同比上周
		/// </summary>
		public decimal? WeekBasis
		{
			set{ _weekbasis=value;}
			get{return _weekbasis;}
		}
		/// <summary>
		/// 环比前一天
		/// </summary>
		public decimal? DayBasis
		{
			set{ _daybasis=value;}
			get{return _daybasis;}
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

