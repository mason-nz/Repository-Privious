using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类ProjectTask_CSTLinkMan 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class ProjectTask_CSTLinkMan
	{
		public ProjectTask_CSTLinkMan()
		{
		 _recid = Constant.INT_INVALID_VALUE;
         _ptid = Constant.STRING_INVALID_VALUE;
		 _cstmemberid = Constant.INT_INVALID_VALUE;
		 _originalcstlinkmanid = Constant.INT_INVALID_VALUE;
		 _cstrecid = Constant.STRING_INVALID_VALUE;
		 _name = Constant.STRING_INVALID_VALUE;
		 _department = Constant.STRING_INVALID_VALUE;
		 _position = Constant.STRING_INVALID_VALUE;
		 _mobile = Constant.STRING_INVALID_VALUE;
		 _email = Constant.STRING_INVALID_VALUE;
		 _status = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private int _recid;
		private string _ptid;
		private int? _cstmemberid;
		private int? _originalcstlinkmanid;
		private string _cstrecid;
		private string _name;
		private string _department;
		private string _position;
		private string _mobile;
		private string _email;
		private int? _status;
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
		public string PTID
		{
			set{ _ptid=value;}
			get{return _ptid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CSTMemberID
		{
			set{ _cstmemberid=value;}
			get{return _cstmemberid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OriginalCSTLinkManID
		{
			set{ _originalcstlinkmanid=value;}
			get{return _originalcstlinkmanid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CSTRecID
		{
			set{ _cstrecid=value;}
			get{return _cstrecid;}
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
		public string Department
		{
			set{ _department=value;}
			get{return _department;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Position
		{
			set{ _position=value;}
			get{return _position;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
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
		#endregion Model

	}
}

