using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
   public class QueryConsultDFeedback
    {
        public QueryConsultDFeedback()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _custid = Constant.STRING_INVALID_VALUE;
		 _questiontype = Constant.INT_INVALID_VALUE;
		 _callrecord = Constant.STRING_INVALID_VALUE;
	 
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _custid;
		private int? _questiontype;
		private string _callrecord;
	 
		private DateTime? _createtime;
		private int? _createuserid;
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
		public string CustID
		{
			set{ _custid=value;}
			get{return _custid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QuestionType
		{
			set{ _questiontype=value;}
			get{return _questiontype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CallRecord
		{
			set{ _callrecord=value;}
			get{return _callrecord;}
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
