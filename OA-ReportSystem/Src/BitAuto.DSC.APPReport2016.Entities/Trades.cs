
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 交易量
	/// </summary>
	[Serializable]
	public partial class Trades
	{
		public Trades()
		{}
		#region Model
		private DateTime _date;
		private int _lineid;
		private int _count;
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
		/// 业务线
		/// </summary>
		public int LineId
		{
			set{ _lineid=value;}
			get{return _lineid;}
		}
		/// <summary>
		/// 交易量
		/// </summary>
		public int Count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 同步上周
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

