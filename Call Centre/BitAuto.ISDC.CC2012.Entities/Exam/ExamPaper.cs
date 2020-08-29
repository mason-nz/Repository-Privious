using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ExamPaper 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:17 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ExamPaper
	{
		public ExamPaper()
		{
		 _epid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
         _ecid = Constant.INT_INVALID_VALUE;
         _bgid = Constant.INT_INVALID_VALUE;
		 _examdesc = Constant.STRING_INVALID_VALUE;
		 _totalscore = Constant.INT_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _creaetuserid = Constant.INT_INVALID_VALUE;
		 _lastmodifytime = Constant.DATE_INVALID_VALUE;
		 _lastmodifyuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model

	    private int _bgid;
		private long _epid;
		private string _name;
		private int _ecid;
		private string _examdesc;
		private int _totalscore;
		private int _status;
		private DateTime? _createtime;
		private int? _creaetuserid;
		private DateTime? _lastmodifytime;
		private int? _lastmodifyuserid;
		/// <summary>
		/// 
		/// </summary>
		public long EPID
		{
			set{ _epid=value;}
			get{return _epid;}
		}

	    public int BGID
	    {
            set { _bgid = value; }
            get { return _bgid; } 
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
		public string ExamDesc
		{
			set{ _examdesc=value;}
			get{return _examdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TotalScore
		{
			set{ _totalscore=value;}
			get{return _totalscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Status
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
		#endregion Model

	}
}

