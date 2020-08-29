using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类SurveyOption 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:18 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class SurveyOption
	{
		public SurveyOption()
		{
		 _soid = Constant.INT_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _sqid = Constant.INT_INVALID_VALUE;
		 _optionname = Constant.STRING_INVALID_VALUE;
		 _isblank = Constant.INT_INVALID_VALUE;
		 _score = Constant.INT_INVALID_VALUE;
		 _ordernum = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _soid;
		private int? _siid;
		private int? _sqid;
		private string _optionname;
		private int? _isblank;
		private int? _score;
		private int? _ordernum;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
		/// <summary>
		/// 
		/// </summary>
		public int SOID
		{
			set{ _soid=value;}
			get{return _soid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SIID
		{
			set{ _siid=value;}
			get{return _siid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SQID
		{
			set{ _sqid=value;}
			get{return _sqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OptionName
		{
			set{ _optionname=value;}
			get{return _optionname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsBlank
		{
			set{ _isblank=value;}
			get{return _isblank;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Score
		{
			set{ _score=value;}
			get{return _score;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrderNum
		{
			set{ _ordernum=value;}
			get{return _ordernum;}
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
		/// 
		/// </summary>
		public DateTime? ModifyTime
		{
			set{ _modifytime=value;}
			get{return _modifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ModifyUserID
		{
			set{ _modifyuserid=value;}
			get{return _modifyuserid;}
		}

		#endregion Model

        /// <summary>
        /// 操作标记  -1 删除  0 编辑   1 添加
        /// </summary>
        public int actionFlog { get; set; }

        /// <summary>
        /// 跳题链接ID
        /// </summary>
        public int linkid { get; set; }
	}
}

