using System;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;

namespace BitAuto.DSC.IM_2015.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类EPVisitLog 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:02 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    /// 实体类：EPVisitLogInfo
    /// <summary>
    /// 实体类：EPVisitLogInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-10-29
    /// </summary>
    [DBTableAttribute("EPVisitLog")]
    public class EPVisitLog
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public EPVisitLog()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public EPVisitLog(string _visitid)
        {
            this._visitid = _visitid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public EPVisitLog(DataRow dr)
            : this()
        {
            #region 表字段转换
            _visitid = (!dr.Table.Columns.Contains("VisitID") || dr["VisitID"] == null || dr["VisitID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["VisitID"]);
            _loginid = (!dr.Table.Columns.Contains("LoginID") || dr["LoginID"] == null || dr["LoginID"] == DBNull.Value) ? null : (long?)Convert.ToInt64(dr["LoginID"]);
            _membercode = (!dr.Table.Columns.Contains("MemberCode") || dr["MemberCode"] == null || dr["MemberCode"] == DBNull.Value) ? null : (string)Convert.ToString(dr["MemberCode"]);
            _visitrefer = (!dr.Table.Columns.Contains("VisitRefer") || dr["VisitRefer"] == null || dr["VisitRefer"] == DBNull.Value) ? null : (string)Convert.ToString(dr["VisitRefer"]);
            _userrefertitle = (!dr.Table.Columns.Contains("UserReferTitle") || dr["UserReferTitle"] == null || dr["UserReferTitle"] == DBNull.Value) ? null : (string)Convert.ToString(dr["UserReferTitle"]);
            _userreferurl = (!dr.Table.Columns.Contains("UserReferURL") || dr["UserReferURL"] == null || dr["UserReferURL"] == DBNull.Value) ? null : (string)Convert.ToString(dr["UserReferURL"]);
            _contractname = (!dr.Table.Columns.Contains("ContractName") || dr["ContractName"] == null || dr["ContractName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ContractName"]);
            _contractjob = (!dr.Table.Columns.Contains("ContractJob") || dr["ContractJob"] == null || dr["ContractJob"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ContractJob"]);
            _contractphone = (!dr.Table.Columns.Contains("ContractPhone") || dr["ContractPhone"] == null || dr["ContractPhone"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ContractPhone"]);
            _contractemail = (!dr.Table.Columns.Contains("ContractEmail") || dr["ContractEmail"] == null || dr["ContractEmail"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ContractEmail"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            #endregion

        }

        #endregion

        #region 属性
        #region 字段 VisitID (自动生成)
        private string _visitid = null;
        [DBFieldAttribute("VisitID", SqlDbType.VarChar, 50, true)]
        public string VisitID { get { return _visitid; } set { if (_visitid != value) { _visitid = value; isModify_VisitID = true; } } }
        public string ValueOrDefault_VisitID { get { return _visitid != null ? _visitid : ""; } }
        private bool isModify_VisitID = false;
        [IsModifyAttribute("VisitID")]
        public bool IsModify_VisitID { get { return isModify_VisitID; } }
        #endregion

        #region 字段 LoginID (自动生成)
        private long? _loginid = null;
        [DBFieldAttribute("LoginID", SqlDbType.BigInt, 8)]
        public long? LoginID { get { return _loginid; } set { if (_loginid != value) { _loginid = value; isModify_LoginID = true; } } }
        public long ValueOrDefault_LoginID { get { return _loginid.HasValue ? _loginid.Value : 0; } }
        private bool isModify_LoginID = false;
        [IsModifyAttribute("LoginID")]
        public bool IsModify_LoginID { get { return isModify_LoginID; } }
        #endregion

        #region 字段 MemberCode (自动生成)
        private string _membercode = null;
        [DBFieldAttribute("MemberCode", SqlDbType.VarChar, 50)]
        public string MemberCode { get { return _membercode; } set { if (_membercode != value) { _membercode = value; isModify_MemberCode = true; } } }
        public string ValueOrDefault_MemberCode { get { return _membercode != null ? _membercode : ""; } }
        private bool isModify_MemberCode = false;
        [IsModifyAttribute("MemberCode")]
        public bool IsModify_MemberCode { get { return isModify_MemberCode; } }
        #endregion

        #region 字段 VisitRefer (自动生成)
        private string _visitrefer = null;
        [DBFieldAttribute("VisitRefer", SqlDbType.VarChar, 50)]
        public string VisitRefer { get { return _visitrefer; } set { if (_visitrefer != value) { _visitrefer = value; isModify_VisitRefer = true; } } }
        public string ValueOrDefault_VisitRefer { get { return _visitrefer != null ? _visitrefer : ""; } }
        private bool isModify_VisitRefer = false;
        [IsModifyAttribute("VisitRefer")]
        public bool IsModify_VisitRefer { get { return isModify_VisitRefer; } }
        #endregion

        #region 字段 UserReferTitle (自动生成)
        private string _userrefertitle = null;
        [DBFieldAttribute("UserReferTitle", SqlDbType.VarChar, 256)]
        public string UserReferTitle { get { return _userrefertitle; } set { if (_userrefertitle != value) { _userrefertitle = value; isModify_UserReferTitle = true; } } }
        public string ValueOrDefault_UserReferTitle { get { return _userrefertitle != null ? _userrefertitle : ""; } }
        private bool isModify_UserReferTitle = false;
        [IsModifyAttribute("UserReferTitle")]
        public bool IsModify_UserReferTitle { get { return isModify_UserReferTitle; } }
        #endregion

        #region 字段 UserReferURL (自动生成)
        private string _userreferurl = null;
        [DBFieldAttribute("UserReferURL", SqlDbType.VarChar, 100)]
        public string UserReferURL { get { return _userreferurl; } set { if (_userreferurl != value) { _userreferurl = value; isModify_UserReferURL = true; } } }
        public string ValueOrDefault_UserReferURL { get { return _userreferurl != null ? _userreferurl : ""; } }
        private bool isModify_UserReferURL = false;
        [IsModifyAttribute("UserReferURL")]
        public bool IsModify_UserReferURL { get { return isModify_UserReferURL; } }
        #endregion

        #region 字段 ContractName (自动生成)
        private string _contractname = null;
        [DBFieldAttribute("ContractName", SqlDbType.VarChar, 50)]
        public string ContractName { get { return _contractname; } set { if (_contractname != value) { _contractname = value; isModify_ContractName = true; } } }
        public string ValueOrDefault_ContractName { get { return _contractname != null ? _contractname : ""; } }
        private bool isModify_ContractName = false;
        [IsModifyAttribute("ContractName")]
        public bool IsModify_ContractName { get { return isModify_ContractName; } }
        #endregion

        #region 字段 ContractJob (自动生成)
        private string _contractjob = null;
        [DBFieldAttribute("ContractJob", SqlDbType.VarChar, 100)]
        public string ContractJob { get { return _contractjob; } set { if (_contractjob != value) { _contractjob = value; isModify_ContractJob = true; } } }
        public string ValueOrDefault_ContractJob { get { return _contractjob != null ? _contractjob : ""; } }
        private bool isModify_ContractJob = false;
        [IsModifyAttribute("ContractJob")]
        public bool IsModify_ContractJob { get { return isModify_ContractJob; } }
        #endregion

        #region 字段 ContractPhone (自动生成)
        private string _contractphone = null;
        [DBFieldAttribute("ContractPhone", SqlDbType.VarChar, 28)]
        public string ContractPhone { get { return _contractphone; } set { if (_contractphone != value) { _contractphone = value; isModify_ContractPhone = true; } } }
        public string ValueOrDefault_ContractPhone { get { return _contractphone != null ? _contractphone : ""; } }
        private bool isModify_ContractPhone = false;
        [IsModifyAttribute("ContractPhone")]
        public bool IsModify_ContractPhone { get { return isModify_ContractPhone; } }
        #endregion

        #region 字段 ContractEmail (自动生成)
        private string _contractemail = null;
        [DBFieldAttribute("ContractEmail", SqlDbType.VarChar, 50)]
        public string ContractEmail { get { return _contractemail; } set { if (_contractemail != value) { _contractemail = value; isModify_ContractEmail = true; } } }
        public string ValueOrDefault_ContractEmail { get { return _contractemail != null ? _contractemail : ""; } }
        private bool isModify_ContractEmail = false;
        [IsModifyAttribute("ContractEmail")]
        public bool IsModify_ContractEmail { get { return isModify_ContractEmail; } }
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
            isModify_VisitID = b;
            isModify_LoginID = b;
            isModify_MemberCode = b;
            isModify_VisitRefer = b;
            isModify_UserReferTitle = b;
            isModify_UserReferURL = b;
            isModify_ContractName = b;
            isModify_ContractJob = b;
            isModify_ContractPhone = b;
            isModify_ContractEmail = b;
            isModify_CreateTime = b;
        }

        #endregion

    }
}

