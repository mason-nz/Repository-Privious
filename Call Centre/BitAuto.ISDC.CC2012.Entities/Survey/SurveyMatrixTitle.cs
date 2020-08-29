using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类SurveyMatrixTitle 。(属性说明自动提取数据库字段的描述信息)
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
	public class SurveyMatrixTitle
	{
		public SurveyMatrixTitle()
		{
		 _smtid = Constant.INT_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _sqid = Constant.INT_INVALID_VALUE;
		 _titlename = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _type = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _smtid;
		private int? _siid;
		private int? _sqid;
		private string _titlename;
		private int? _status;
		private int? _type;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public int SMTID
		{
			set{ _smtid=value;}
			get{return _smtid;}
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
		public string TitleName
		{
			set{ _titlename=value;}
			get{return _titlename;}
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
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
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

        /// <summary>
        /// 操作标记  -1 删除  0 编辑   1 添加
        /// </summary>
        public int actionFlog { get; set; }
	}
}

