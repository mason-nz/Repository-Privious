using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：WOrderBusiTypeInfo [任务列表]-[工单V2]-工单业务类型表 
    /// <summary>
    /// 实体类：WOrderBusiTypeInfo [任务列表]-[工单V2]-工单业务类型表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-19
    /// </summary>
    [DBTableAttribute("WOrderBusiType")]
    public class WOrderBusiTypeInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public WOrderBusiTypeInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public WOrderBusiTypeInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderBusiTypeInfo(DataRow dr)
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

        #region BusiTypeName [业务类型名称]
        /// <summary>
        /// 业务类型名称
        /// </summary>
        [DBField("BusiTypeName", SqlDbType.VarChar, 20)]
        public string BusiTypeName { get { return _busitypename; } set { if (_busitypename != value) { _busitypename = value; IsModify_BusiTypeName = true; } } }
        /// <summary>
        /// 业务类型名称
        /// </summary>
        public string BusiTypeName_Value { get { return _busitypename != null ? _busitypename : ""; } }
        private string _busitypename = null;

        [IsModify("BusiTypeName")]
        public bool IsModify_BusiTypeName { get; set; }
        #endregion

        #region SortNum [顺序]
        /// <summary>
        /// 顺序
        /// </summary>
        [DBField("SortNum", SqlDbType.Int, 4)]
        public int? SortNum { get { return _sortnum; } set { if (_sortnum != value) { _sortnum = value; IsModify_SortNum = true; } } }
        /// <summary>
        /// 顺序
        /// </summary>
        public int SortNum_Value { get { return _sortnum.HasValue ? _sortnum.Value : 0; } }
        private int? _sortnum = null;

        [IsModify("SortNum")]
        public bool IsModify_SortNum { get; set; }
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

        #region LastUpdateUserID [最后更新人ID]
        /// <summary>
        /// 最后更新人ID
        /// </summary>
        [DBField("LastUpdateUserID", SqlDbType.Int, 4)]
        public int? LastUpdateUserID { get { return _lastupdateuserid; } set { if (_lastupdateuserid != value) { _lastupdateuserid = value; IsModify_LastUpdateUserID = true; } } }
        /// <summary>
        /// 最后更新人ID
        /// </summary>
        public int LastUpdateUserID_Value { get { return _lastupdateuserid.HasValue ? _lastupdateuserid.Value : 0; } }
        private int? _lastupdateuserid = null;

        [IsModify("LastUpdateUserID")]
        public bool IsModify_LastUpdateUserID { get; set; }
        #endregion

        #region LastUpdateTime [最后更新时间]
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DBField("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; IsModify_LastUpdateTime = true; } } }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime_Value { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private DateTime? _lastupdatetime = null;

        [IsModify("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get; set; }
        #endregion

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
            IsModify_BusiTypeName = b;
            IsModify_SortNum = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
            IsModify_LastUpdateUserID = b;
            IsModify_LastUpdateTime = b;
        }

        #endregion

    }
}
