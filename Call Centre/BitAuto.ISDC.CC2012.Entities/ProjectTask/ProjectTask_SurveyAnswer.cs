using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_SurveyAnswer 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:32 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_SurveyAnswer
	{
		public ProjectTask_SurveyAnswer()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _ptid = Constant.STRING_INVALID_VALUE;
		 _projectid = Constant.INT_INVALID_VALUE;
		 _siid = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
         _returnvisitcrmcustid = Constant.STRING_INVALID_VALUE;
         _status = Constant.INT_INVALID_VALUE;
        }
        private int _status;
        public int Status
        {
            set
            {
                _status = value;
            }
            get
            {
                return _status;
            }
        }
        private string _returnvisitcrmcustid;
        public string ReturnVisitCustID
        {
            set
            {
                _returnvisitcrmcustid = value;
            }
            get
            {
                return _returnvisitcrmcustid;
            }
        }
		#region Model
		private long _recid;
		private string _ptid;
		private long _projectid;
		private int? _siid;
		private DateTime? _createtime;
		private int? _createuserid;
		/// <summary>
		/// 
		/// </summary>
		public long RecID
		{
			set{ _recid=value;}
			get{return _recid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PTID
		{
			set{ _ptid=value;}
			get{return _ptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long ProjectID
		{
			set{ _projectid=value;}
			get{return _projectid;}
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
		#endregion Model

	}
}

