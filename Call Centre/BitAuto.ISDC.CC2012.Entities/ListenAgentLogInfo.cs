using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：ListenAgentLogInfo
    /// <summary>
    /// 实体类：ListenAgentLogInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-11-12
    /// </summary>
    [DBTableAttribute("ListenAgentLog")]
    public class ListenAgentLogInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public ListenAgentLogInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public ListenAgentLogInfo(int _recid)
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public ListenAgentLogInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _recid = (!dr.Table.Columns.Contains("RecID") || dr["RecID"] == null || dr["RecID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["RecID"]);
            _listenuserid = (!dr.Table.Columns.Contains("ListenUserID") || dr["ListenUserID"] == null || dr["ListenUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ListenUserID"]);
            _listenusername = (!dr.Table.Columns.Contains("ListenUserName") || dr["ListenUserName"] == null || dr["ListenUserName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ListenUserName"]);
            _listennum = (!dr.Table.Columns.Contains("ListenNum") || dr["ListenNum"] == null || dr["ListenNum"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ListenNum"]);
            _listenextensionnum = (!dr.Table.Columns.Contains("ListenExtensionNum") || dr["ListenExtensionNum"] == null || dr["ListenExtensionNum"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ListenExtensionNum"]);
            _listenoper = (!dr.Table.Columns.Contains("ListenOper") || dr["ListenOper"] == null || dr["ListenOper"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ListenOper"]);
            _agentuserid = (!dr.Table.Columns.Contains("AgentUserID") || dr["AgentUserID"] == null || dr["AgentUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["AgentUserID"]);
            _agentusername = (!dr.Table.Columns.Contains("AgentUserName") || dr["AgentUserName"] == null || dr["AgentUserName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["AgentUserName"]);
            _agentnum = (!dr.Table.Columns.Contains("AgentNum") || dr["AgentNum"] == null || dr["AgentNum"] == DBNull.Value) ? null : (string)Convert.ToString(dr["AgentNum"]);
            _agentextensionnum = (!dr.Table.Columns.Contains("AgentExtensionNum") || dr["AgentExtensionNum"] == null || dr["AgentExtensionNum"] == DBNull.Value) ? null : (string)Convert.ToString(dr["AgentExtensionNum"]);
            _agentstatus = (!dr.Table.Columns.Contains("AgentStatus") || dr["AgentStatus"] == null || dr["AgentStatus"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["AgentStatus"]);
            _remark = (!dr.Table.Columns.Contains("Remark") || dr["Remark"] == null || dr["Remark"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Remark"]);
            _vendor = (!dr.Table.Columns.Contains("Vendor") || dr["Vendor"] == null || dr["Vendor"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Vendor"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            #endregion

        }

        #endregion

        #region 属性
        #region 字段 RecID (自动生成)
        private int? _recid = null;
        [DBFieldAttribute("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; isModify_RecID = true; } } }
        public int ValueOrDefault_RecID { get { return _recid.HasValue ? _recid.Value : 0; } }
        private bool isModify_RecID = false;
        [IsModifyAttribute("RecID")]
        public bool IsModify_RecID { get { return isModify_RecID; } }
        #endregion

        #region 字段 ListenUserID (自动生成)
        private int? _listenuserid = null;
        [DBFieldAttribute("ListenUserID", SqlDbType.Int, 4)]
        public int? ListenUserID { get { return _listenuserid; } set { if (_listenuserid != value) { _listenuserid = value; isModify_ListenUserID = true; } } }
        public int ValueOrDefault_ListenUserID { get { return _listenuserid.HasValue ? _listenuserid.Value : 0; } }
        private bool isModify_ListenUserID = false;
        [IsModifyAttribute("ListenUserID")]
        public bool IsModify_ListenUserID { get { return isModify_ListenUserID; } }
        #endregion

        #region 字段 ListenUserName (自动生成)
        private string _listenusername = null;
        [DBFieldAttribute("ListenUserName", SqlDbType.VarChar, 50)]
        public string ListenUserName { get { return _listenusername; } set { if (_listenusername != value) { _listenusername = value; isModify_ListenUserName = true; } } }
        public string ValueOrDefault_ListenUserName { get { return _listenusername != null ? _listenusername : ""; } }
        private bool isModify_ListenUserName = false;
        [IsModifyAttribute("ListenUserName")]
        public bool IsModify_ListenUserName { get { return isModify_ListenUserName; } }
        #endregion

        #region 字段 ListenNum (自动生成)
        private string _listennum = null;
        [DBFieldAttribute("ListenNum", SqlDbType.VarChar, 10)]
        public string ListenNum { get { return _listennum; } set { if (_listennum != value) { _listennum = value; isModify_ListenNum = true; } } }
        public string ValueOrDefault_ListenNum { get { return _listennum != null ? _listennum : ""; } }
        private bool isModify_ListenNum = false;
        [IsModifyAttribute("ListenNum")]
        public bool IsModify_ListenNum { get { return isModify_ListenNum; } }
        #endregion

        #region 字段 ListenExtensionNum (自动生成)
        private string _listenextensionnum = null;
        [DBFieldAttribute("ListenExtensionNum", SqlDbType.VarChar, 10)]
        public string ListenExtensionNum { get { return _listenextensionnum; } set { if (_listenextensionnum != value) { _listenextensionnum = value; isModify_ListenExtensionNum = true; } } }
        public string ValueOrDefault_ListenExtensionNum { get { return _listenextensionnum != null ? _listenextensionnum : ""; } }
        private bool isModify_ListenExtensionNum = false;
        [IsModifyAttribute("ListenExtensionNum")]
        public bool IsModify_ListenExtensionNum { get { return isModify_ListenExtensionNum; } }
        #endregion

        #region 字段 ListenOper (自动生成)
        private int? _listenoper = null;
        [DBFieldAttribute("ListenOper", SqlDbType.Int, 4)]
        public int? ListenOper { get { return _listenoper; } set { if (_listenoper != value) { _listenoper = value; isModify_ListenOper = true; } } }
        public int ValueOrDefault_ListenOper { get { return _listenoper.HasValue ? _listenoper.Value : 0; } }
        private bool isModify_ListenOper = false;
        [IsModifyAttribute("ListenOper")]
        public bool IsModify_ListenOper { get { return isModify_ListenOper; } }
        #endregion

        #region 字段 AgentUserID (自动生成)
        private int? _agentuserid = null;
        [DBFieldAttribute("AgentUserID", SqlDbType.Int, 4)]
        public int? AgentUserID { get { return _agentuserid; } set { if (_agentuserid != value) { _agentuserid = value; isModify_AgentUserID = true; } } }
        public int ValueOrDefault_AgentUserID { get { return _agentuserid.HasValue ? _agentuserid.Value : 0; } }
        private bool isModify_AgentUserID = false;
        [IsModifyAttribute("AgentUserID")]
        public bool IsModify_AgentUserID { get { return isModify_AgentUserID; } }
        #endregion

        #region 字段 AgentUserName (自动生成)
        private string _agentusername = null;
        [DBFieldAttribute("AgentUserName", SqlDbType.VarChar, 50)]
        public string AgentUserName { get { return _agentusername; } set { if (_agentusername != value) { _agentusername = value; isModify_AgentUserName = true; } } }
        public string ValueOrDefault_AgentUserName { get { return _agentusername != null ? _agentusername : ""; } }
        private bool isModify_AgentUserName = false;
        [IsModifyAttribute("AgentUserName")]
        public bool IsModify_AgentUserName { get { return isModify_AgentUserName; } }
        #endregion

        #region 字段 AgentNum (自动生成)
        private string _agentnum = null;
        [DBFieldAttribute("AgentNum", SqlDbType.VarChar, 10)]
        public string AgentNum { get { return _agentnum; } set { if (_agentnum != value) { _agentnum = value; isModify_AgentNum = true; } } }
        public string ValueOrDefault_AgentNum { get { return _agentnum != null ? _agentnum : ""; } }
        private bool isModify_AgentNum = false;
        [IsModifyAttribute("AgentNum")]
        public bool IsModify_AgentNum { get { return isModify_AgentNum; } }
        #endregion

        #region 字段 AgentExtensionNum (自动生成)
        private string _agentextensionnum = null;
        [DBFieldAttribute("AgentExtensionNum", SqlDbType.VarChar, 10)]
        public string AgentExtensionNum { get { return _agentextensionnum; } set { if (_agentextensionnum != value) { _agentextensionnum = value; isModify_AgentExtensionNum = true; } } }
        public string ValueOrDefault_AgentExtensionNum { get { return _agentextensionnum != null ? _agentextensionnum : ""; } }
        private bool isModify_AgentExtensionNum = false;
        [IsModifyAttribute("AgentExtensionNum")]
        public bool IsModify_AgentExtensionNum { get { return isModify_AgentExtensionNum; } }
        #endregion

        #region 字段 AgentStatus (自动生成)
        private int? _agentstatus = null;
        [DBFieldAttribute("AgentStatus", SqlDbType.Int, 4)]
        public int? AgentStatus { get { return _agentstatus; } set { if (_agentstatus != value) { _agentstatus = value; isModify_AgentStatus = true; } } }
        public int ValueOrDefault_AgentStatus { get { return _agentstatus.HasValue ? _agentstatus.Value : 0; } }
        private bool isModify_AgentStatus = false;
        [IsModifyAttribute("AgentStatus")]
        public bool IsModify_AgentStatus { get { return isModify_AgentStatus; } }
        #endregion

        #region 字段 Remark (自动生成)
        private string _remark = null;
        [DBFieldAttribute("Remark", SqlDbType.VarChar, 500)]
        public string Remark { get { return _remark; } set { if (_remark != value) { _remark = value; isModify_Remark = true; } } }
        public string ValueOrDefault_Remark { get { return _remark != null ? _remark : ""; } }
        private bool isModify_Remark = false;
        [IsModifyAttribute("Remark")]
        public bool IsModify_Remark { get { return isModify_Remark; } }
        #endregion

        #region 字段 Vendor (自动生成)
        private int? _vendor = null;
        [DBFieldAttribute("Vendor", SqlDbType.Int, 4)]
        public int? Vendor { get { return _vendor; } set { if (_vendor != value) { _vendor = value; isModify_Vendor = true; } } }
        public int ValueOrDefault_Vendor { get { return _vendor.HasValue ? _vendor.Value : 0; } }
        private bool isModify_Vendor = false;
        [IsModifyAttribute("Vendor")]
        public bool IsModify_Vendor { get { return isModify_Vendor; } }
        #endregion

        #region 字段 CreateUserID (自动生成)
        private int? _createuserid = null;
        [DBFieldAttribute("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; isModify_CreateUserID = true; } } }
        public int ValueOrDefault_CreateUserID { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private bool isModify_CreateUserID = false;
        [IsModifyAttribute("CreateUserID")]
        public bool IsModify_CreateUserID { get { return isModify_CreateUserID; } }
        #endregion

        #region 字段 CreateTime (自动生成)
        private DateTime? _createtime = null;
        [DBFieldAttribute("CreateTime", SqlDbType.DateTime, 8)]
        public DateTime? CreateTime { get { return _createtime; } set { if (_createtime != value) { _createtime = value; isModify_CreateTime = true; } } }
        public DateTime ValueOrDefault_CreateTime { get { return _createtime.HasValue ? _createtime.Value : new DateTime(); } }
        private bool isModify_CreateTime = false;
        [IsModifyAttribute("CreateTime")]
        public bool IsModify_CreateTime { get { return isModify_CreateTime; } }
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
            isModify_RecID = b;
            isModify_ListenUserID = b;
            isModify_ListenUserName = b;
            isModify_ListenNum = b;
            isModify_ListenExtensionNum = b;
            isModify_ListenOper = b;
            isModify_AgentUserID = b;
            isModify_AgentUserName = b;
            isModify_AgentNum = b;
            isModify_AgentExtensionNum = b;
            isModify_AgentStatus = b;
            isModify_Remark = b;
            isModify_Vendor = b;
            isModify_CreateUserID = b;
            isModify_CreateTime = b;
        }

        #endregion

    }
}
