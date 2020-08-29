
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 人员统计
	/// </summary>
	[Serializable]
	public partial class EmployeeItem
	{
		public EmployeeItem()
		{}
		#region Model
		private int _yearmonth;
		private int _itemtype;
		private int _itemid;
		private int _count;
		private decimal _percent;
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
		/// 统计分类
		/// </summary>
		public int ItemType
		{
			set{ _itemtype=value;}
			get{return _itemtype;}
		}
		/// <summary>
		/// 统计项目
		/// </summary>
		public int ItemId
		{
			set{ _itemid=value;}
			get{return _itemid;}
		}
		/// <summary>
		/// 数量
		/// </summary>
		public int Count
		{
			set{ _count=value;}
			get{return _count;}
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

