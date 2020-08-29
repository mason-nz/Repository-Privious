using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：YTGActivityInfo
    /// <summary>
    /// 实体类：YTGActivityInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-12-19
    /// </summary>
    [DBTableAttribute("YTGActivity")]
    public class YTGActivityInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public YTGActivityInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public YTGActivityInfo(string _activityid)
        {
            this._activityid = _activityid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public YTGActivityInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _activityid = (!dr.Table.Columns.Contains("ActivityID") || dr["ActivityID"] == null || dr["ActivityID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ActivityID"]);
            _activityname = (!dr.Table.Columns.Contains("ActivityName") || dr["ActivityName"] == null || dr["ActivityName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ActivityName"]);
            _signbegintime = (!dr.Table.Columns.Contains("SignBeginTime") || dr["SignBeginTime"] == null || dr["SignBeginTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["SignBeginTime"]);
            _signendtime = (!dr.Table.Columns.Contains("SignEndTime") || dr["SignEndTime"] == null || dr["SignEndTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["SignEndTime"]);
            _activebegintime = (!dr.Table.Columns.Contains("ActiveBeginTime") || dr["ActiveBeginTime"] == null || dr["ActiveBeginTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["ActiveBeginTime"]);
            _activeendtime = (!dr.Table.Columns.Contains("ActiveEndTime") || dr["ActiveEndTime"] == null || dr["ActiveEndTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["ActiveEndTime"]);
            _address = (!dr.Table.Columns.Contains("Address") || dr["Address"] == null || dr["Address"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Address"]);
            _carserials = (!dr.Table.Columns.Contains("CarSerials") || dr["CarSerials"] == null || dr["CarSerials"] == DBNull.Value) ? null : (string)Convert.ToString(dr["CarSerials"]);
            _content = (!dr.Table.Columns.Contains("Content") || dr["Content"] == null || dr["Content"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Content"]);
            _url = (!dr.Table.Columns.Contains("Url") || dr["Url"] == null || dr["Url"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Url"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _lastupdatetime = (!dr.Table.Columns.Contains("LastUpdateTime") || dr["LastUpdateTime"] == null || dr["LastUpdateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastUpdateTime"]);
            #endregion

        }

        #endregion

        #region 属性
        #region 字段 ActivityID (自动生成)
        private string _activityid = null;
        [DBFieldAttribute("ActivityID", SqlDbType.VarChar, 50, true)]
        public string ActivityID { get { return _activityid; } set { if (_activityid != value) { _activityid = value; isModify_ActivityID = true; } } }
        public string ValueOrDefault_ActivityID { get { return _activityid != null ? _activityid : ""; } }
        private bool isModify_ActivityID = false;
        [IsModifyAttribute("ActivityID")]
        public bool IsModify_ActivityID { get { return isModify_ActivityID; } }
        #endregion

        #region 字段 ActivityName (自动生成)
        private string _activityname = null;
        [DBFieldAttribute("ActivityName", SqlDbType.VarChar, 100)]
        public string ActivityName { get { return _activityname; } set { if (_activityname != value) { _activityname = value; isModify_ActivityName = true; } } }
        public string ValueOrDefault_ActivityName { get { return _activityname != null ? _activityname : ""; } }
        private bool isModify_ActivityName = false;
        [IsModifyAttribute("ActivityName")]
        public bool IsModify_ActivityName { get { return isModify_ActivityName; } }
        #endregion

        #region 字段 SignBeginTime (自动生成)
        private DateTime? _signbegintime = null;
        [DBFieldAttribute("SignBeginTime", SqlDbType.DateTime, 8)]
        public DateTime? SignBeginTime { get { return _signbegintime; } set { if (_signbegintime != value) { _signbegintime = value; isModify_SignBeginTime = true; } } }
        public DateTime ValueOrDefault_SignBeginTime { get { return _signbegintime.HasValue ? _signbegintime.Value : new DateTime(); } }
        private bool isModify_SignBeginTime = false;
        [IsModifyAttribute("SignBeginTime")]
        public bool IsModify_SignBeginTime { get { return isModify_SignBeginTime; } }
        #endregion

        #region 字段 SignEndTime (自动生成)
        private DateTime? _signendtime = null;
        [DBFieldAttribute("SignEndTime", SqlDbType.DateTime, 8)]
        public DateTime? SignEndTime { get { return _signendtime; } set { if (_signendtime != value) { _signendtime = value; isModify_SignEndTime = true; } } }
        public DateTime ValueOrDefault_SignEndTime { get { return _signendtime.HasValue ? _signendtime.Value : new DateTime(); } }
        private bool isModify_SignEndTime = false;
        [IsModifyAttribute("SignEndTime")]
        public bool IsModify_SignEndTime { get { return isModify_SignEndTime; } }
        #endregion

        #region 字段 ActiveBeginTime (自动生成)
        private DateTime? _activebegintime = null;
        [DBFieldAttribute("ActiveBeginTime", SqlDbType.DateTime, 8)]
        public DateTime? ActiveBeginTime { get { return _activebegintime; } set { if (_activebegintime != value) { _activebegintime = value; isModify_ActiveBeginTime = true; } } }
        public DateTime ValueOrDefault_ActiveBeginTime { get { return _activebegintime.HasValue ? _activebegintime.Value : new DateTime(); } }
        private bool isModify_ActiveBeginTime = false;
        [IsModifyAttribute("ActiveBeginTime")]
        public bool IsModify_ActiveBeginTime { get { return isModify_ActiveBeginTime; } }
        #endregion

        #region 字段 ActiveEndTime (自动生成)
        private DateTime? _activeendtime = null;
        [DBFieldAttribute("ActiveEndTime", SqlDbType.DateTime, 8)]
        public DateTime? ActiveEndTime { get { return _activeendtime; } set { if (_activeendtime != value) { _activeendtime = value; isModify_ActiveEndTime = true; } } }
        public DateTime ValueOrDefault_ActiveEndTime { get { return _activeendtime.HasValue ? _activeendtime.Value : new DateTime(); } }
        private bool isModify_ActiveEndTime = false;
        [IsModifyAttribute("ActiveEndTime")]
        public bool IsModify_ActiveEndTime { get { return isModify_ActiveEndTime; } }
        #endregion

        #region 字段 Address (自动生成)
        private string _address = null;
        [DBFieldAttribute("Address", SqlDbType.VarChar, 500)]
        public string Address { get { return _address; } set { if (_address != value) { _address = value; isModify_Address = true; } } }
        public string ValueOrDefault_Address { get { return _address != null ? _address : ""; } }
        private bool isModify_Address = false;
        [IsModifyAttribute("Address")]
        public bool IsModify_Address { get { return isModify_Address; } }
        #endregion

        #region 字段 CarSerials (自动生成)
        private string _carserials = null;
        [DBFieldAttribute("CarSerials", SqlDbType.VarChar, 100)]
        public string CarSerials { get { return _carserials; } set { if (_carserials != value) { _carserials = value; isModify_CarSerials = true; } } }
        public string ValueOrDefault_CarSerials { get { return _carserials != null ? _carserials : ""; } }
        private bool isModify_CarSerials = false;
        [IsModifyAttribute("CarSerials")]
        public bool IsModify_CarSerials { get { return isModify_CarSerials; } }
        #endregion

        #region 字段 Content (自动生成)
        private string _content = null;
        [DBFieldAttribute("Content", SqlDbType.VarChar, 1000)]
        public string Content { get { return _content; } set { if (_content != value) { _content = value; isModify_Content = true; } } }
        public string ValueOrDefault_Content { get { return _content != null ? _content : ""; } }
        private bool isModify_Content = false;
        [IsModifyAttribute("Content")]
        public bool IsModify_Content { get { return isModify_Content; } }
        #endregion

        #region 字段 Url (自动生成)
        private string _url = null;
        [DBFieldAttribute("Url", SqlDbType.VarChar, 100)]
        public string Url { get { return _url; } set { if (_url != value) { _url = value; isModify_Url = true; } } }
        public string ValueOrDefault_Url { get { return _url != null ? _url : ""; } }
        private bool isModify_Url = false;
        [IsModifyAttribute("Url")]
        public bool IsModify_Url { get { return isModify_Url; } }
        #endregion

        #region 字段 Status (自动生成)
        private int? _status = null;
        [DBFieldAttribute("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; isModify_Status = true; } } }
        public int ValueOrDefault_Status { get { return _status.HasValue ? _status.Value : 0; } }
        private bool isModify_Status = false;
        [IsModifyAttribute("Status")]
        public bool IsModify_Status { get { return isModify_Status; } set { isModify_Status = value; } }
        #endregion

        #region 字段 CreateTime (自动生成)
        private DateTime? _createtime = null;
        [DBFieldAttribute("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; isModify_CreateTime = true; } } }
        public DateTime ValueOrDefault_CreateTime { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private bool isModify_CreateTime = false;
        [IsModifyAttribute("CreateTime")]
        public bool IsModify_CreateTime { get { return isModify_CreateTime; } set { isModify_CreateTime = value; } }
        #endregion

        #region 字段 LastUpdateTime (自动生成)
        private DateTime? _lastupdatetime = null;
        [DBFieldAttribute("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; isModify_LastUpdateTime = true; } } }
        public DateTime ValueOrDefault_LastUpdateTime { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private bool isModify_LastUpdateTime = false;
        [IsModifyAttribute("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get { return isModify_LastUpdateTime; } }
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
            isModify_ActivityID = b;
            isModify_ActivityName = b;
            isModify_SignBeginTime = b;
            isModify_SignEndTime = b;
            isModify_ActiveBeginTime = b;
            isModify_ActiveEndTime = b;
            isModify_Address = b;
            isModify_CarSerials = b;
            isModify_Content = b;
            isModify_Url = b;
            isModify_Status = b;
            isModify_CreateTime = b;
            isModify_LastUpdateTime = b;
        }

        #endregion

        public override string ToString()
        {
            return CommonFunction.DefineObjectToString(this);
        }
    }
}
