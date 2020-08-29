using System;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类UserActionLog 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:04 Created
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
		 _loginfo = Constant.STRING_INVALID_VALUE;
		 _ip = Constant.STRING_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _truename = Constant.STRING_INVALID_VALUE;
         _operusertype = Constant.INT_INVALID_VALUE;
         _logintype = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private int _recid;
		private string _loginfo;
		private string _ip;
		private DateTime? _createtime;
		private int? _createuserid;
		private string _truename;

        private int? _operusertype;
        private int? _logintype;

        public int? LogInType
        {
            set { _logintype = value; }
            get { return _logintype; }
        }

        public int? OperUserType
        {
            set { _operusertype = value; }
            get { return _operusertype; }
        }

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
		public string LogInfo
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
		/// 
		/// </summary>
		public int? CreateUserID
		{
			set{ _createuserid=value;}
			get{return _createuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TrueName
		{
			set{ _truename=value;}
			get{return _truename;}
		}
		#endregion Model

	}
}

