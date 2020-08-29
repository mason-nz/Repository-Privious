using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类KLUploadFile 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:09 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class KLUploadFile
	{
		public KLUploadFile()
		{
		 _recid = Constant.INT_INVALID_VALUE;
		 _klid = Constant.INT_INVALID_VALUE;
		 _filepath = Constant.STRING_INVALID_VALUE;
		 _filename = Constant.STRING_INVALID_VALUE;
		 _extendname = Constant.STRING_INVALID_VALUE;
		 _filesize = Constant.INT_INVALID_VALUE;
		 _clickcount = Constant.INT_INVALID_VALUE;
		 _createtime = Constant.DATE_INVALID_VALUE;
		 _createuserid = Constant.INT_INVALID_VALUE;

		}
		#region Model
		private long _recid;
		private long _klid;
		private string _filepath;
		private string _filename;
		private string _extendname;
		private int? _filesize;
		private int? _clickcount;
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
		public long KLID
		{
			set{ _klid=value;}
			get{return _klid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FilePath
		{
			set{ _filepath=value;}
			get{return _filepath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Filename
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ExtendName
		{
			set{ _extendname=value;}
			get{return _extendname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? FileSize
		{
			set{ _filesize=value;}
			get{return _filesize;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ClickCount
		{
			set{ _clickcount=value;}
			get{return _clickcount;}
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

