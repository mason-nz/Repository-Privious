using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类SurveyProjectInfo 。(属性说明自动提取数据库字段的描述信息)
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
	public class SurveyProjectInfo
	{
		public SurveyProjectInfo()
		{
		 _spiid = Constant.INT_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _scid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _description = Constant.STRING_INVALID_VALUE;
		 _businessgroup = Constant.STRING_INVALID_VALUE;
		 _surveystarttime = Constant.DATE_INVALID_VALUE;
		 _surveyendtime = Constant.DATE_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _spiid;
		private int? _bgid;
		private int? _scid;
		private string _name;
		private string _description;
		private string _businessgroup;
		private DateTime? _surveystarttime;
		private DateTime? _surveyendtime;
		private int? _siid;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
		/// <summary>
		/// 
		/// </summary>
		public int SPIID
		{
			set{ _spiid=value;}
			get{return _spiid;}
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
		public int? SCID
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
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BusinessGroup
		{
			set{ _businessgroup=value;}
			get{return _businessgroup;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SurveyStartTime
		{
			set{ _surveystarttime=value;}
			get{return _surveystarttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? SurveyEndTime
		{
			set{ _surveyendtime=value;}
			get{return _surveyendtime;}
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

	}
}

