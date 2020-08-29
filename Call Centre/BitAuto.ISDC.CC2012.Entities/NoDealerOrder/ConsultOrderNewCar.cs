using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ConsultOrderNewCar 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ConsultOrderNewCar
	{
		public ConsultOrderNewCar()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _carbrandid = Constant.INT_INVALID_VALUE;
		 _carserialid = Constant.INT_INVALID_VALUE;
		 _carnameid = Constant.INT_INVALID_VALUE;
		 _carcolor = Constant.STRING_INVALID_VALUE;
		 _dealername = Constant.STRING_INVALID_VALUE;
		 _dealercode = Constant.STRING_INVALID_VALUE;
		 _orderremark = Constant.STRING_INVALID_VALUE;
		 _callrecord = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _custid;
		private int? _carbrandid;
		private int? _carserialid;
		private int? _carnameid;
		private string _carcolor;
		private string _dealername;
		private string _dealercode;
		private string _orderremark;
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
		public int? CarNameID
		{
			set{ _carnameid=value;}
			get{return _carnameid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarColor
		{
			set{ _carcolor=value;}
			get{return _carcolor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DealerName
		{
			set{ _dealername=value;}
			get{return _dealername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DealerCode
		{
			set{ _dealercode=value;}
			get{return _dealercode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderRemark
		{
			set{ _orderremark=value;}
			get{return _orderremark;}
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

