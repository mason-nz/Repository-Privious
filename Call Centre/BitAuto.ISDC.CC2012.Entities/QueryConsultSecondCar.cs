using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryConsultSecondCar 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryConsultSecondCar
	{
		public QueryConsultSecondCar()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _questiontype = Constant.INT_INVALID_VALUE;
		 _carbrandid = Constant.INT_INVALID_VALUE;
		 _carserialid = Constant.INT_INVALID_VALUE;
		 _carname = Constant.STRING_INVALID_VALUE;
		 _salecarbrandid = Constant.INT_INVALID_VALUE;
		 _salecarserialid = Constant.INT_INVALID_VALUE;
		 _salecarname = Constant.STRING_INVALID_VALUE;
		 _callrecord = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _custid;
		private int? _questiontype;
		private int? _carbrandid;
		private int? _carserialid;
		private string _carname;
		private int? _salecarbrandid;
		private int? _salecarserialid;
		private string _salecarname;
		private string _callrecord;
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
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QuestionType
		{
			set{ _questiontype=value;}
			get{return _questiontype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarBrandId
		{
			set{ _carbrandid=value;}
			get{return _carbrandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CarSerialId
		{
			set{ _carserialid=value;}
			get{return _carserialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarName
		{
			set{ _carname=value;}
			get{return _carname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SaleCarBrandId
		{
			set{ _salecarbrandid=value;}
			get{return _salecarbrandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SaleCarSerialId
		{
			set{ _salecarserialid=value;}
			get{return _salecarserialid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SaleCarName
		{
			set{ _salecarname=value;}
			get{return _salecarname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CallRecord
		{
			set{ _callrecord=value;}
			get{return _callrecord;}
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

