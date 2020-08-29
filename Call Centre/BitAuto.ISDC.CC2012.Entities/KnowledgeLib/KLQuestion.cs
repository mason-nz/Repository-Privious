using System;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Collections.Generic;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类KLQuestion 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class KLQuestion
	{
		public KLQuestion()
		{
		 _klqid = Constant.INT_INVALID_VALUE;
         _klid = Constant.INT_INVALID_VALUE;
         _kcid = Constant.INT_INVALID_VALUE;
		 _askcategory = Constant.INT_INVALID_VALUE;
		 _ask = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private long _klqid;
		private long _klid;
	    private int _kcid;
		private int _askcategory;
		private string _ask;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
        private List<Entities.KLAnswerOption> _optionlist;
        private List<int> _answeroptionindexlist;
		/// <summary>
		/// 
		/// </summary>
		public long KLQID
		{
			set{ _klqid=value;}
			get{return _klqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long KLID
		{
			set{ _klid=value;}
			get{return _klid;}
		}
        public int KCID
        {
            set { _kcid = value; }
            get { return _kcid; }
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
		public string Ask
		{
			set{ _ask=value;}
			get{return _ask;}
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

        /// <summary>
        /// 试题选项
        /// </summary>
        public List<Entities.KLAnswerOption> OptionList
        {
            set { _optionlist = value; }
            get { return _optionlist; }
        }

        /// <summary>
        /// 试题的正确答案
        /// </summary>
        public List<int> AnswerOptionIndexList
        {
            set { _answeroptionindexlist = value; }
            get { return _answeroptionindexlist; }
        }
		#endregion Model

	}
}

