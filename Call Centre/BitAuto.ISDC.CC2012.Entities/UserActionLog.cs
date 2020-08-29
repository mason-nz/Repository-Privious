using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{

	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类UserActionLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-04 10:22:38 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class UserActionLog
	{
		public UserActionLog()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _userid = Constant.INT_INVALID_VALUE;
		 _loginfo = Constant.STRING_INVALID_VALUE;
		 _ip = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private int _userid;
		private string _loginfo;
		private string _ip;
		private DateTime? _createtime;
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
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Loginfo
		{
			set{ _loginfo=value;}
			get{return _loginfo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IP
		{
			set{ _ip=value;}
			get{return _ip;}
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
        /// 权限系统USERID
        /// </summary>
        public int UserEID { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string TrueName { get; set; }
		#endregion Model

	}
}

