
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 人员情况
	/// </summary>
	[Serializable]
	public partial class Employee
	{
		public Employee()
		{}
		#region Model
		private int _yearmonth;
		private int _total;
		private decimal? _monthbasis;
		private int _entry;
		private int _dimission;
		private int _male;
		private int _female;
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
		/// 总数
		/// </summary>
		public int Total
		{
			set{ _total=value;}
			get{return _total;}
		}
		/// <summary>
		/// 环比增长率
		/// </summary>
		public decimal? MonthBasis
		{
			set{ _monthbasis=value;}
			get{return _monthbasis;}
		}
		/// <summary>
		/// 入职
		/// </summary>
		public int Entry
		{
			set{ _entry=value;}
			get{return _entry;}
		}
		/// <summary>
		/// 离职
		/// </summary>
		public int Dimission
		{
			set{ _dimission=value;}
			get{return _dimission;}
		}
		/// <summary>
		/// 男
		/// </summary>
		public int Male
		{
			set{ _male=value;}
			get{return _male;}
		}
		/// <summary>
		/// 女性
		/// </summary>
		public int Female
		{
			set{ _female=value;}
			get{return _female;}
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

