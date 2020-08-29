
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 平台覆盖用户
	/// </summary>
	[Serializable]
	public partial class Traffic
	{
		public Traffic()
		{}
		#region Model
		private DateTime _date;
		private int _siteid;
		private long _uv;
		private decimal? _weekbasis;
		private decimal? _daybasis;
		private DateTime _createtime;
		private long _pv;
		/// <summary>
		/// 日期
		/// </summary>
		public DateTime Date
		{
			set{ _date=value;}
			get{return _date;}
		}
		/// <summary>
		/// 平台ID
		/// </summary>
		public int SiteId
		{
			set{ _siteid=value;}
			get{return _siteid;}
		}
		/// <summary>
		/// 覆盖用户数
		/// </summary>
		public long UV
		{
			set{ _uv=value;}
			get{return _uv;}
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
		/// 环比上一天
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
		/// <summary>
		/// 页面浏览量
		/// </summary>
		public long PV
		{
			set{ _pv=value;}
			get{return _pv;}
		}
		#endregion Model

	}
}

