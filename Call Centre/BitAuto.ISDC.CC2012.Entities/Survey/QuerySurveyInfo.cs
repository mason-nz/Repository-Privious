using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ʵ����QuerySurveyInfo ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:17 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QuerySurveyInfo
	{
		public QuerySurveyInfo()
		{
		 _siid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _bgid = Constant.INT_INVALID_VALUE;
		 _scid = Constant.INT_INVALID_VALUE;
		 _description = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _isavailable = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

         _begintime = Constant.STRING_INVALID_VALUE;//����ʱ��Ŀ�ʼʱ��
         _endtime = Constant.STRING_INVALID_VALUE;//����ʱ��Ľ���ʱ��
         _isavailables = Constant.STRING_INVALID_VALUE;//�Ƿ���ô�
         _statuss = Constant.STRING_INVALID_VALUE;//״̬�� 
         _loginid = Constant.INT_INVALID_VALUE;//��½��ID
         _owngroup = Constant.STRING_EMPTY_VALUE;//Ȩ���Ǳ����Ȩ���飻��ʽ�������û���,ҵ����...
         _oneself = Constant.STRING_EMPTY_VALUE;//Ȩ���Ǳ��˵�Ȩ���飻��ʽ�������û���,ҵ����...
              
		}
		#region Model
		private int _siid;
		private string _name;
		private int? _bgid;
		private int? _scid;
		private string _description;
		private int? _status;
		private int? _isavailable;
		private DateTime? _createtime;
		private int? _createuserid;
        private string _begintime;
        private string _endtime;
        private string _isavailables;
        private string _statuss; 
        private int _loginid;
        private string _owngroup;
        private string _oneself;
		/// <summary>
		/// 
		/// </summary>
		public int SIID
		{
			set{ _siid=value;}
			get{return _siid;}
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
		public int? BGID
		{
			set{ _bgid=value;}
			get{return _bgid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SCID
		{
			set{ _scid=value;}
			get{return _scid;}
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsAvailable
		{
			set{ _isavailable=value;}
			get{return _isavailable;}
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
        public string BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        public string EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        public string IsAvailables
        {
            set { _isavailables = value; }
            get { return _isavailables; }
        }
        public string Statuss
        {
            set { _statuss = value; }
            get { return _statuss; }
        } 
        public int LoginID
        {
            set { _loginid = value; }
            get { return _loginid; }
        }
        
        public string OwnGroup
        {
            set { _owngroup = value; }
            get { return _owngroup; }
        }
        public string OneSelf
        {
            set { _oneself = value; }
            get { return _oneself; }
        }
		#endregion Model

	}
}

