
using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
	/// <summary>
	/// 字典表
	/// </summary>
	[Serializable]
	public partial class DictInfo
	{
		public DictInfo()
		{}
		#region Model
		private int _dictid;
		private int _dicttype;
		private string _dictname;
		private int _status;
		private int _ordernum;
		private DateTime _createtime;
		/// <summary>
		/// 字典ID
		/// </summary>
		public int DictId
		{
			set{ _dictid=value;}
			get{return _dictid;}
		}
		/// <summary>
		/// 字典类型
		/// </summary>
		public int DictType
		{
			set{ _dicttype=value;}
			get{return _dicttype;}
		}
		/// <summary>
		/// 字典名称
		/// </summary>
		public string DictName
		{
			set{ _dictname=value;}
			get{return _dictname;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int OrderNum
		{
			set{ _ordernum=value;}
			get{return _ordernum;}
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

