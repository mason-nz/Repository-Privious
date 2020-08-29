using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类CustHistoryInfo 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:14 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class CustHistoryInfo
	{
		public CustHistoryInfo()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _taskid = Constant.STRING_INVALID_VALUE;
		 _callrecordid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _recordtype = Constant.INT_INVALID_VALUE;
		 _consultid = Constant.INT_INVALID_VALUE;
		 _consultdataid = Constant.INT_INVALID_VALUE;
		 _questionquality = Constant.INT_INVALID_VALUE;
		 _lasttreatmenttime = Constant.DATE_INVALID_VALUE;
		//_iscomplaint没有默认值
		 _processstatus = Constant.INT_INVALID_VALUE;
		//_issendemail没有默认值
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

         _templateid = Constant.INT_INVALID_VALUE;
         _businesstype = Constant.INT_INVALID_VALUE;
		}

        /// <summary>
        /// 查询字段
        /// </summary>
        public static string SelectFieldStr 
        {

            get
            {
                 string fields = " chi.RecID AS RecID,"
				+"chi.TaskID AS TaskID,"
				+"chi.CallRecordID AS CallRecordID,"
				+"chi.CustID AS CustID,"
				+"chi.RecordType AS RecordType,"
				+"chi.ConsultID AS ConsultID,"
				+"chi.ConsultDataID AS ConsultDataID,"
				+"chi.QuestionQuality AS QuestionQuality,"
				+"chi.LastTreatmentTime AS LastTreatmentTime,"
				+"chi.IsComplaint AS IsComplaint,"
				+"chi.ProcessStatus AS ProcessStatus,"
				+"chi.IsSendEmail AS IsSendEmail,"
				+"chi.CreateTime AS CreateTime,"
				+"chi.CreateUserID AS CreateUserID,"
				+"cbi.RecID AS cbiRecID,"
                +"cbi.CustName AS CustName,"
                + "chi.BusinessType AS BusinessType ";
                 return fields;
            }
        }
		#region Model
		private long _recid;
		private string _taskid;
		private long _callrecordid;
		private string _custid;
		private int? _recordtype;
		private int? _consultid;
		private int? _consultdataid;
		private int? _questionquality;
		private DateTime? _lasttreatmenttime;
		private bool _iscomplaint;
		private int? _processstatus;
		private bool _issendemail;
		private DateTime? _createtime;
		private int? _createuserid;
        private int? _templateid; 

        //add by qizq 2013-11-5
        private int? _businesstype;
        public int? BusinessType
        {
            set { _businesstype = value; }
            get { return _businesstype; }
        }
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
		public string TaskID
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long CallRecordID
		{
			set{ _callrecordid=value;}
			get{return _callrecordid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RecordType
		{
			set{ _recordtype=value;}
			get{return _recordtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ConsultID
		{
			set{ _consultid=value;}
			get{return _consultid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ConsultDataID
		{
			set{ _consultdataid=value;}
			get{return _consultdataid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QuestionQuality
		{
			set{ _questionquality=value;}
			get{return _questionquality;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastTreatmentTime
		{
			set{ _lasttreatmenttime=value;}
			get{return _lasttreatmenttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsComplaint
		{
			set{ _iscomplaint=value;}
			get{return _iscomplaint;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ProcessStatus
		{
			set{ _processstatus=value;}
			get{return _processstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsSendEmail
		{
			set{ _issendemail=value;}
			get{return _issendemail;}
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
        public int? TemplateID
        {
            set { _templateid = value; }
            get { return _templateid; }
        }
		#endregion Model

	}
}

