using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class KLRaiseQuestions
    {
        public KLRaiseQuestions()
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
		/// 
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
		#endregion Model
    }
}
