using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryQS_ResultDetail 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryQS_ResultDetail
	{
		public QueryQS_ResultDetail()
		{
		 _qs_rdid = Constant.INT_INVALID_VALUE;
		 _scoretype = Constant.INT_INVALID_VALUE;
		 _qs_rtid = Constant.INT_INVALID_VALUE;
		 _qs_rid = Constant.INT_INVALID_VALUE;
		 _qs_cid = Constant.INT_INVALID_VALUE;
		 _qs_iid = Constant.INT_INVALID_VALUE;
		 _qs_sid = Constant.INT_INVALID_VALUE;
		 _qs_mid = Constant.INT_INVALID_VALUE;
		 _qs_mid_end = Constant.INT_INVALID_VALUE;
		 _type = Constant.INT_INVALID_VALUE;
		 _scoredeadid = Constant.INT_INVALID_VALUE;
		 _scoredeadid_end = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
		 _remark = Constant.STRING_INVALID_VALUE;
         
         _begintime = Constant.STRING_INVALID_VALUE;
         _endtime = Constant.STRING_INVALID_VALUE;
         _callbegintime = Constant.STRING_INVALID_VALUE;
         _callendtime = Constant.STRING_INVALID_VALUE;
		}
		#region Model
		private int _qs_rdid;
		private int _scoretype;
		private int _qs_rtid;
		private int _qs_rid;
		private int? _qs_cid;
		private int? _qs_iid;
		private int? _qs_sid;
		private int? _qs_mid;
		private int? _qs_mid_end;
		private int? _type;
		private int? _scoredeadid;
		private int? _scoredeadid_end;
		private int? _status;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
        private string _remark;
        private string _begintime;
        private string _endtime;
        private string _callbegintime;
        private string _callendtime;
		/// <summary>
		/// 
		/// </summary>
		public int QS_RDID
		{
			set{ _qs_rdid=value;}
			get{return _qs_rdid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ScoreType
		{
			set{ _scoretype=value;}
			get{return _scoretype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_RTID
		{
			set{ _qs_rtid=value;}
			get{return _qs_rtid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QS_RID
		{
			set{ _qs_rid=value;}
			get{return _qs_rid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QS_CID
		{
			set{ _qs_cid=value;}
			get{return _qs_cid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QS_IID
		{
			set{ _qs_iid=value;}
			get{return _qs_iid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QS_SID
		{
			set{ _qs_sid=value;}
			get{return _qs_sid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QS_MID
		{
			set{ _qs_mid=value;}
			get{return _qs_mid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QS_MID_End
		{
			set{ _qs_mid_end=value;}
			get{return _qs_mid_end;}
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
		public int? ScoreDeadID
		{
			set{ _scoredeadid=value;}
			get{return _scoredeadid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ScoreDeadID_End
		{
			set{ _scoredeadid_end=value;}
			get{return _scoredeadid_end;}
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
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		public string BeginTime
		{
			set{ _begintime=value;}
            get { return _begintime; }
		}
		public string EndTime
		{
            set { _endtime = value; }
            get { return _endtime; }
        }
        public string CallBeginTime
        {
            set { _callbegintime = value; }
            get { return _callbegintime; }
        }
        public string CallEndTime
        {
            set { _callendtime = value; }
            get { return _callendtime; }
        }
		#endregion Model

	}
}

