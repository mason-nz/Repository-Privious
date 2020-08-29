using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ExamInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ExamInfo
	{
		public ExamInfo()
		{
		 _eiid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _ecid = Constant.INT_INVALID_VALUE;
		 _description = Constant.STRING_INVALID_VALUE;
		 _businessgroup = Constant.STRING_INVALID_VALUE;
		 _epid = Constant.INT_INVALID_VALUE;
		 _examstarttime = Constant.DATE_INVALID_VALUE;
		 _examendtime = Constant.DATE_INVALID_VALUE;
		 _joinnum = Constant.INT_INVALID_VALUE;
		 _ismakeup = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _creaetuserid = Constant.INT_INVALID_VALUE;
		 _lastmodifytime = Constant.DATE_INVALID_VALUE;
		 _lastmodifyuserid = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
         _bgid = Constant.INT_INVALID_VALUE;
		}
		#region Model
		private long _eiid;
		private string _name;
		private int _ecid;
		private string _description;
		private string _businessgroup;
		private int _epid;
		private DateTime _examstarttime;
		private DateTime _examendtime;
		private int? _joinnum;
		private int _ismakeup;
		private DateTime? _createtime;
		private int? _creaetuserid;
		private DateTime? _lastmodifytime;
		private int? _lastmodifyuserid;
		private int? _status;
        private int? _bgid;
		/// <summary>
		/// 
		/// </summary>
		public long EIID
		{
			set{ _eiid=value;}
			get{return _eiid;}
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
		public int ECID
		{
			set{ _ecid=value;}
			get{return _ecid;}
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
		public int EPID
		{
			set{ _epid=value;}
			get{return _epid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ExamStartTime
		{
			set{ _examstarttime=value;}
			get{return _examstarttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ExamEndTime
		{
			set{ _examendtime=value;}
			get{return _examendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? JoinNum
		{
			set{ _joinnum=value;}
			get{return _joinnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsMakeUp
		{
			set{ _ismakeup=value;}
			get{return _ismakeup;}
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
		public int? CreaetUserID
		{
			set{ _creaetuserid=value;}
			get{return _creaetuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastModifyTime
		{
			set{ _lastmodifytime=value;}
			get{return _lastmodifytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LastModifyUserID
		{
			set{ _lastmodifyuserid=value;}
			get{return _lastmodifyuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}

        public int? BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }
		#endregion Model

	}
}

