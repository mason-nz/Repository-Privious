using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类TPage 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:35 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class TPage
	{
		public TPage()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _tpname = Constant.STRING_INVALID_VALUE;
         _bgid = Constant.INT_INVALID_VALUE;
		 _tpcid = Constant.INT_INVALID_VALUE;
		 _tpref = Constant.STRING_INVALID_VALUE;
		 _ttcode = Constant.STRING_INVALID_VALUE;
		 _sttcode = Constant.STRING_INVALID_VALUE;
		 _tpcontent = Constant.STRING_INVALID_VALUE;
         _gentempletpath = Constant.STRING_INVALID_VALUE;
         _remark = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
         _createuserid = Constant.INT_INVALID_VALUE;
         _ttname = Constant.STRING_INVALID_VALUE;
         _isshowbtn = Constant.INT_INVALID_VALUE;
         _isshowworkorderbtn = Constant.INT_INVALID_VALUE;
         _isshowsendmsg = Constant.INT_INVALID_VALUE;
         _isused = Constant.INT_INVALID_VALUE;
         _isshowqichetong = Constant.INT_INVALID_VALUE;
         _isshowsubmitorder = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private int _recid;
		private string _tpname;
        private int? _bgid;
		private int? _tpcid;
		private string _tpref;
		private string _ttcode;
		private string _sttcode;
		private string _tpcontent;
        private string _gentempletpath;
        private string _remark;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _ttname;
        private int _isshowbtn;
        private int _isshowworkorderbtn;
        private int _isshowsendmsg;
        private int _isshowqichetong;
        private int _isshowsubmitorder;
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
		public string TPName
		{
			set{ _tpname=value;}
			get{return _tpname;}
		}

        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }

		/// <summary>
		/// 
		/// </summary>
        public int? SCID
		{
			set{ _tpcid=value;}
			get{return _tpcid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TPRef
		{
			set{ _tpref=value;}
			get{return _tpref;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TTCode
		{
			set{ _ttcode=value;}
			get{return _ttcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string STTCode
		{
			set{ _sttcode=value;}
			get{return _sttcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TPContent
		{
			set{ _tpcontent=value;}
			get{return _tpcontent;}
		}
        public string GenTempletPath
		{
            set { _gentempletpath = value; }
            get { return _gentempletpath; }
		}
        public string Remark
		{
            set { _remark = value; }
            get { return _remark; }
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
		public string TTName
		{
            set { _ttname = value; }
            get { return _ttname; }
		}
        public int IsShowBtn
		{
            set { _isshowbtn = value; }
            get { return _isshowbtn; }
		}
        public int IsShowWorkOrderBtn
        {
            set { _isshowworkorderbtn = value; }
            get { return _isshowworkorderbtn; }
        }
        public int IsShowSendMsgBtn
        {
            set { _isshowsendmsg = value; }
            get { return _isshowsendmsg; }
        }
        public int IsShowQiCheTong
        {
            set { _isshowqichetong = value; }
            get { return _isshowqichetong; }
        }
        public int IsShowSubmitOrder
        {
            set { _isshowsubmitorder = value; }
            get { return _isshowsubmitorder; }
        }
		#endregion Model

        private int _isused;

        public int IsUsed
        {
            get { return _isused; }
            set { _isused = value; }
        }

	}
}

