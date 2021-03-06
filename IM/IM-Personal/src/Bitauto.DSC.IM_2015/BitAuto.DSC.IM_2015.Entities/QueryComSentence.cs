using System;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryComSentence 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryComSentence
	{
		public QueryComSentence()
		{
		 _csid = Constant.INT_INVALID_VALUE;
		 _ltid = Constant.INT_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _sortnum = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;
		 _modifytime = Constant.DATE_INVALID_VALUE;
		 _modifyuserid = Constant.INT_INVALID_VALUE;
         _ltname = Constant.STRING_INVALID_VALUE;
         _csname = Constant.STRING_INVALID_VALUE;

         _dataright = Constant.STRING_INVALID_VALUE;
		}
		#region Model
		private int _csid;
		private int? _ltid;
		private string _name;
		private int? _status;
		private int? _sortnum;
		private DateTime? _createtime;
		private int? _createuserid;
		private DateTime? _modifytime;
		private int? _modifyuserid;
        private string _ltname;
        private string _csname;

        private string _dataright;

        public string DataRight
        {
            set { _dataright = value; }
            get { return _dataright; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int CSID
		{
			set{ _csid=value;}
			get{return _csid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LTID
		{
			set{ _ltid=value;}
			get{return _ltid;}
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
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SortNum
		{
			set{ _sortnum=value;}
			get{return _sortnum;}
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
        public string LTName
        {
            set { _ltname = value; }
            get { return _ltname; }
        }
        public string CSName
        {
            set { _csname = value; }
            get { return _csname; }
        }
		#endregion Model

	}
}

