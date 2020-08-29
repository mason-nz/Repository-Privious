using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ExamBigQuestion 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:15 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ExamBigQuestion
	{
		public ExamBigQuestion()
		{
		 _bqid = Constant.INT_INVALID_VALUE;
		 _epid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _bqdesc = Constant.STRING_INVALID_VALUE;
		 _askcategory = Constant.INT_INVALID_VALUE;
		 _eachquestionscore = Constant.INT_INVALID_VALUE;
		 _questioncount = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
         _No = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private long _bqid;
		private long _epid;
		private string _name;
		private string _bqdesc;
		private int _askcategory;
		private int _eachquestionscore;
		private int _questioncount;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
        private int _No;
		/// <summary>
		/// 
		/// </summary>
		public long BQID
		{
			set{ _bqid=value;}
			get{return _bqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long EPID
		{
			set{ _epid=value;}
			get{return _epid;}
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
		public string BQDesc
		{
			set{ _bqdesc=value;}
			get{return _bqdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AskCategory
		{
			set{ _askcategory=value;}
			get{return _askcategory;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int EachQuestionScore
		{
			set{ _eachquestionscore=value;}
			get{return _eachquestionscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QuestionCount
		{
			set{ _questioncount=value;}
			get{return _questioncount;}
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
        /// 编号
        /// </summary>
        public int NO
        {
            set { _No = value; }
            get { return _No; }
        }
	}
}

