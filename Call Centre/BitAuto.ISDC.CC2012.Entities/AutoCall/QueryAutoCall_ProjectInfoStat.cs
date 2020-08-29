using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryAutoCall_ProjectInfoStat 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2015-09-14 09:57:59 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryAutoCall_ProjectInfoStat
	{
		public QueryAutoCall_ProjectInfoStat()
		{
		 _projectid = Constant.INT_INVALID_VALUE;
		 _actotalnum = Constant.INT_INVALID_VALUE;
		 _ivrconnectnum = Constant.INT_INVALID_VALUE;
		 _disconnectnum = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		//_timestamp没有默认值

		}
		#region Model
		private long _projectid;
		private int? _actotalnum;
		private int? _ivrconnectnum;
		private int? _disconnectnum;
		private DateTime? _createtime;
		private int? _createuserid;
		//private timestamp _timestamp;
		/// <summary>
		/// 
		/// </summary>
		public long ProjectID
		{
			set{ _projectid=value;}
			get{return _projectid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ACTotalNum
		{
			set{ _actotalnum=value;}
			get{return _actotalnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IVRConnectNum
		{
			set{ _ivrconnectnum=value;}
			get{return _ivrconnectnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DisconnectNum
		{
			set{ _disconnectnum=value;}
			get{return _disconnectnum;}
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
        ///// <summary>
        ///// 
        ///// </summary>
        //public timestamp Timestamp
        //{
        //    set{ _timestamp=value;}
        //    get{return _timestamp;}
        //}
		#endregion Model

	}
}

