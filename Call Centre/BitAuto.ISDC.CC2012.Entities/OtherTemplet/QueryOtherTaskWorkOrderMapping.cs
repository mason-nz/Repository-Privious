using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryOtherTaskWorkOrderMapping 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-09-05 03:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryOtherTaskWorkOrderMapping
	{
		public QueryOtherTaskWorkOrderMapping()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _ptid = Constant.STRING_INVALID_VALUE;
		 _orderid = Constant.STRING_INVALID_VALUE;
		 _pcrmcustid = Constant.STRING_INVALID_VALUE;
		 _ocrmcustid = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _ptid;
		private string _orderid;
		private string _pcrmcustid;
		private string _ocrmcustid;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public int RecID
		{
			set{ _recid=value;}
			get{return _recid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PTID
		{
			set{ _ptid=value;}
			get{return _ptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PCRMCustID
		{
			set{ _pcrmcustid=value;}
			get{return _pcrmcustid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OCRMCustID
		{
			set{ _ocrmcustid=value;}
			get{return _ocrmcustid;}
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

