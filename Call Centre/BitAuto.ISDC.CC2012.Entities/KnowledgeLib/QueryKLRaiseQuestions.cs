using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryKLRaiseQuestions
    {
        public QueryKLRaiseQuestions()
		{
            _id = Constant.INT_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createdate = Constant.DATE_INVALID_VALUE;
            _title = Constant.STRING_INVALID_VALUE;
            _content = Constant.STRING_INVALID_VALUE;
            _klcid = Constant.INT_INVALID_VALUE;
            _klrefid = Constant.INT_INVALID_VALUE;
            _type = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _answeruser = Constant.INT_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _lastmodifydate = Constant.DATE_INVALID_VALUE;
            _lastmodifyby = Constant.INT_INVALID_VALUE;

            _createusername = Constant.STRING_INVALID_VALUE;
            _createbegintime = Constant.DATE_INVALID_VALUE;
            _createendtime = Constant.DATE_INVALID_VALUE;
            _answerbegintime = Constant.DATE_INVALID_VALUE;
            _answerendtime = Constant.DATE_INVALID_VALUE;
            _querystatuses = Constant.STRING_INVALID_VALUE;
            _querybgids = Constant.STRING_INVALID_VALUE;
            _regionid = Constant.INT_INVALID_VALUE;
		}

		#region Model
		private int _id;
		private int _createuserid;
		private DateTime _createdate;
		private string _title;
		private string _content;
		private int _klcid;
		private int _klrefid;
		private int _type;
		private int _status;
		private int _answeruser;
		private int _bgid;
		private DateTime _lastmodifydate;
		private int _lastmodifyby;

        private string _createusername;
        private DateTime _createbegintime;
        private DateTime _createendtime;
        private DateTime _answerbegintime;
        private DateTime _answerendtime;
        private string _querystatuses;
        private string _querybgids;
        private int _regionid;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CreateUserId
		{
			set{ _createuserid=value;}
			get{return _createuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CONTENT
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 知识库分类（1级或2级）
		/// </summary>
		public int KLCId
		{
			set{ _klcid=value;}
			get{return _klcid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int KLRefId
		{
			set{ _klrefid=value;}
			get{return _klrefid;}
		}
		/// <summary>
		/// 0:知识点； 1：FAQ
		/// </summary>
		public int Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 0:未解答； 1：已解答
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AnswerUser
		{
			set{ _answeruser=value;}
			get{return _answeruser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LastModifyDate
		{
			set{ _lastmodifydate=value;}
			get{return _lastmodifydate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LastModifyBy
		{
			set{ _lastmodifyby=value;}
			get{return _lastmodifyby;}
		}

        public string CreateUserName
		{
            set { _createusername = value; }
            get { return _createusername; }
		}
        public DateTime CreateBeginTime
		{
            set { _createbegintime = value; }
            get { return _createbegintime; }
		}
        public DateTime CreateEndTime
		{
            set { _createendtime = value; }
            get { return _createendtime; }
		}
        public DateTime AnswerBeginTime
		{
            set { _answerbegintime = value; }
            get { return _answerbegintime; }
		}
        public DateTime AnswerEndTime
		{
            set { _answerendtime = value; }
            get { return _answerendtime; }
		}
        public string QueryStatuses
        {
            get { return _querystatuses; }
            set { _querystatuses = value; }
        }
        public string QueryBGIDs
        {
            get { return _querybgids; }
            set { _querybgids = value; }
        }
        /// <summary>
        /// 当前登录者所在区域（1-北京，2-西安）
        /// </summary>
        public int RegionID
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
		#endregion Model
    }
}
