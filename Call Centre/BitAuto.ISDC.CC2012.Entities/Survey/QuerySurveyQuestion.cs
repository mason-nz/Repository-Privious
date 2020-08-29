using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QuerySurveyQuestion 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:19 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QuerySurveyQuestion
	{
		public QuerySurveyQuestion()
		{
		 _sqid = Constant.INT_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _ask = Constant.STRING_INVALID_VALUE;
		 _askcategory = Constant.INT_INVALID_VALUE;
         _askcategorystr = Constant.STRING_INVALID_VALUE;
		 _showcolumnnum = Constant.INT_INVALID_VALUE;
		 _maxtextlen = Constant.INT_INVALID_VALUE;
		 _mintextlen = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _ordernum = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
         _ismustanswer = Constant.INT_INVALID_VALUE;
         _isstatbyscore = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private int _sqid;
		private int _siid;
		private string _ask;
		private int? _askcategory;
        private string _askcategorystr;
		private int? _showcolumnnum;
		private int? _maxtextlen;
		private int? _mintextlen;
		private int? _status;
		private int? _ordernum;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
        private int? _ismustanswer;
        private int? _isstatbyscore;
		/// <summary>
		/// 
		/// </summary>
		public int SQID
		{
			set{ _sqid=value;}
			get{return _sqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SIID
		{
			set{ _siid=value;}
			get{return _siid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ask
		{
			set{ _ask=value;}
			get{return _ask;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AskCategory
		{
			set{ _askcategory=value;}
			get{return _askcategory;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string AskCategoryStr
        {
            set { _askcategorystr = value; }
            get { return _askcategorystr; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int? ShowColumnNum
		{
			set{ _showcolumnnum=value;}
			get{return _showcolumnnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MaxTextLen
		{
			set{ _maxtextlen=value;}
			get{return _maxtextlen;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MinTextLen
		{
			set{ _mintextlen=value;}
			get{return _mintextlen;}
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
		public int? OrderNum
		{
			set{ _ordernum=value;}
			get{return _ordernum;}
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
        /// <summary>
        /// 是否为必答题（1-是，0-否）
        /// </summary>
        public int? IsMustAnswer
        {
            set { _ismustanswer = value; }
            get { return _ismustanswer; }
        }
        /// <summary>
        /// 是否按评分统计（1-是,0-否）
        /// </summary>
        public int? IsStatByScore
        {
            set { _isstatbyscore = value; }
            get { return _isstatbyscore; }
        }
		#endregion Model

	}
}

