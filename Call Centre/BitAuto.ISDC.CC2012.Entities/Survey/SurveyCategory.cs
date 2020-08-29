using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类SurveyCategory 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:17 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class SurveyCategory
	{
		public SurveyCategory()
		{
		 _scid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _level = Constant.INT_INVALID_VALUE;
		 _pid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _typeid = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private int _scid;
		private string _name;
		private int? _bgid;
		private int? _level;
		private int? _pid;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
        private int? _typeid;

		/// <summary>
		/// 
		/// </summary>
		public int SCID
		{
			set{ _scid=value;}
			get{return _scid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Level
		{
			set{ _level=value;}
			get{return _level;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Pid
		{
			set{ _pid=value;}
			get{return _pid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
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
        /// 1 问卷类型   2 项目类型
        /// </summary>
        public int? TypeId
		{
            set { _typeid = value; }
            get { return _typeid; }
		}
		#endregion Model

	}
}

