using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：CommonAttachmentInfo [公共表]-[附件]-公共附件表 
    /// <summary>
    /// 实体类：CommonAttachmentInfo [公共表]-[附件]-公共附件表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-19
    /// </summary>
    [DBTableAttribute("CommonAttachment")]
    public class CommonAttachmentInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public CommonAttachmentInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public CommonAttachmentInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public CommonAttachmentInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region 属性
        #region RecID [自增主键]
        /// <summary>
        /// 自增主键
        /// </summary>
        [DBField("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        /// <summary>
        /// 自增主键
        /// </summary>
        public int RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private int? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region BTypeID [业务类型ID 枚举： ProjectTypePath]
        /// <summary>
        /// 业务类型ID 枚举： ProjectTypePath
        /// </summary>
        [DBField("BTypeID", SqlDbType.Int, 4)]
        public int? BTypeID { get { return _btypeid; } set { if (_btypeid != value) { _btypeid = value; IsModify_BTypeID = true; } } }
        /// <summary>
        /// 业务类型ID 枚举： ProjectTypePath
        /// </summary>
        public int BTypeID_Value { get { return _btypeid.HasValue ? _btypeid.Value : 0; } }
        private int? _btypeid = null;

        [IsModify("BTypeID")]
        public bool IsModify_BTypeID { get; set; }
        #endregion

        #region RelatedID [关联数据ID]
        /// <summary>
        /// 关联数据ID
        /// </summary>
        [DBField("RelatedID", SqlDbType.VarChar, 50)]
        public string RelatedID { get { return _relatedid; } set { if (_relatedid != value) { _relatedid = value; IsModify_RelatedID = true; } } }
        /// <summary>
        /// 关联数据ID
        /// </summary>
        public string RelatedID_Value { get { return _relatedid != null ? _relatedid : ""; } }
        private string _relatedid = null;

        [IsModify("RelatedID")]
        public bool IsModify_RelatedID { get; set; }
        #endregion

        #region FileName [原始名称]
        /// <summary>
        /// 原始名称
        /// </summary>
        [DBField("FileName", SqlDbType.VarChar, 50)]
        public string FileName { get { return _filename; } set { if (_filename != value) { _filename = value; IsModify_FileName = true; } } }
        /// <summary>
        /// 原始名称
        /// </summary>
        public string FileName_Value { get { return _filename != null ? _filename : ""; } }
        private string _filename = null;

        [IsModify("FileName")]
        public bool IsModify_FileName { get; set; }
        #endregion

        #region FileType [文件类型]
        /// <summary>
        /// 文件类型
        /// </summary>
        [DBField("FileType", SqlDbType.VarChar, 20)]
        public string FileType { get { return _filetype; } set { if (_filetype != value) { _filetype = value; IsModify_FileType = true; } } }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType_Value { get { return _filetype != null ? _filetype : ""; } }
        private string _filetype = null;

        [IsModify("FileType")]
        public bool IsModify_FileType { get; set; }
        #endregion

        #region FileSize [文件大小（KB）]
        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        [DBField("FileSize", SqlDbType.Int, 4)]
        public int? FileSize { get { return _filesize; } set { if (_filesize != value) { _filesize = value; IsModify_FileSize = true; } } }
        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        public int FileSize_Value { get { return _filesize.HasValue ? _filesize.Value : 0; } }
        private int? _filesize = null;

        [IsModify("FileSize")]
        public bool IsModify_FileSize { get; set; }
        #endregion

        #region FilePath [服务器全路径（包含实际文件名称）]
        /// <summary>
        /// 服务器全路径（包含实际文件名称）
        /// </summary>
        [DBField("FilePath", SqlDbType.VarChar, 50)]
        public string FilePath { get { return _filepath; } set { if (_filepath != value) { _filepath = value; IsModify_FilePath = true; } } }
        /// <summary>
        /// 服务器全路径（包含实际文件名称）
        /// </summary>
        public string FilePath_Value { get { return _filepath != null ? _filepath : ""; } }
        private string _filepath = null;

        [IsModify("FilePath")]
        public bool IsModify_FilePath { get; set; }
        #endregion

        #region Status [状态]
        /// <summary>
        /// 状态
        /// </summary>
        [DBField("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; IsModify_Status = true; } } }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status_Value { get { return _status.HasValue ? _status.Value : 0; } }
        private int? _status = null;

        [IsModify("Status")]
        public bool IsModify_Status { get; set; }
        #endregion

        #region CreateUserID [创建人ID]
        /// <summary>
        /// 创建人ID
        /// </summary>
        [DBField("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; IsModify_CreateUserID = true; } } }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserID_Value { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private int? _createuserid = null;

        [IsModify("CreateUserID")]
        public bool IsModify_CreateUserID { get; set; }
        #endregion

        #region CreateTime [创建时间]
        /// <summary>
        /// 创建时间
        /// </summary>
        [DBField("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; IsModify_CreateTime = true; } } }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime_Value { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private DateTime? _createtime = null;

        [IsModify("CreateTime")]
        public bool IsModify_CreateTime { get; set; }
        #endregion

        #endregion

        #region 其他非数据库字段
        /// 图片类型对应的缩略图
        /// <summary>
        /// 图片类型对应的缩略图
        /// </summary>
        public string SmallFilePath { get; set; }
        #endregion

        #region 方法
        /// 设置是否更新所有字段 (自动生成)
        /// <summary>
        /// 设置是否更新所有字段 (自动生成)
        /// </summary>
        /// <param name="b"></param>
        public void SetModify(bool b)
        {
            IsModify_RecID = b;
            IsModify_BTypeID = b;
            IsModify_RelatedID = b;
            IsModify_FileName = b;
            IsModify_FileType = b;
            IsModify_FileSize = b;
            IsModify_FilePath = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
        }

        #endregion

    }
}
