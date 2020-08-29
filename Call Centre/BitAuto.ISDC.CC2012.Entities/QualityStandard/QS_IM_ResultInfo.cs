using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：QS_IM_ResultInfo
    /// <summary>
    /// 实体类：QS_IM_ResultInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2015-04-16
    /// </summary>
    [DBTableAttribute("QS_IM_Result")]
    public class QS_IM_ResultInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public QS_IM_ResultInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public QS_IM_ResultInfo(int _qs_rid)
        {
            this._qs_rid = _qs_rid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public QS_IM_ResultInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _qs_rid = (!dr.Table.Columns.Contains("QS_RID") || dr["QS_RID"] == null || dr["QS_RID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["QS_RID"]);
            _csid = (!dr.Table.Columns.Contains("CSID") || dr["CSID"] == null || dr["CSID"] == DBNull.Value) ? null : (long?)Convert.ToInt64(dr["CSID"]);
            _qs_rtid = (!dr.Table.Columns.Contains("QS_RTID") || dr["QS_RTID"] == null || dr["QS_RTID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["QS_RTID"]);
            _seatid = (!dr.Table.Columns.Contains("SeatID") || dr["SeatID"] == null || dr["SeatID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["SeatID"]);
            _scoretype = (!dr.Table.Columns.Contains("ScoreType") || dr["ScoreType"] == null || dr["ScoreType"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ScoreType"]);
            _score = (!dr.Table.Columns.Contains("Score") || dr["Score"] == null || dr["Score"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Score"]);
            _isqualified = (!dr.Table.Columns.Contains("IsQualified") || dr["IsQualified"] == null || dr["IsQualified"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["IsQualified"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _stateresult = (!dr.Table.Columns.Contains("StateResult") || dr["StateResult"] == null || dr["StateResult"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["StateResult"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
            _modifytime = (!dr.Table.Columns.Contains("ModifyTime") || dr["ModifyTime"] == null || dr["ModifyTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["ModifyTime"]);
            _modifyuserid = (!dr.Table.Columns.Contains("ModifyUserID") || dr["ModifyUserID"] == null || dr["ModifyUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ModifyUserID"]);
            _qualityappraisal = (!dr.Table.Columns.Contains("QualityAppraisal") || dr["QualityAppraisal"] == null || dr["QualityAppraisal"] == DBNull.Value) ? null : (string)Convert.ToString(dr["QualityAppraisal"]);
            #endregion

        }

        #endregion

        #region 属性
        #region 字段 QS_RID (自动生成) (不是自增列)
        private int? _qs_rid = null;
        [DBFieldAttribute("QS_RID", SqlDbType.Int, 4, true, false)]
        public int? QS_RID { get { return _qs_rid; } set { if (_qs_rid != value) { _qs_rid = value; isModify_QS_RID = true; } } }
        public int ValueOrDefault_QS_RID { get { return _qs_rid.HasValue ? _qs_rid.Value : 0; } }
        private bool isModify_QS_RID = false;
        [IsModifyAttribute("QS_RID")]
        public bool IsModify_QS_RID { get { return isModify_QS_RID; } }
        #endregion

        #region 字段 CSID (自动生成)
        private long? _csid = null;
        [DBFieldAttribute("CSID", SqlDbType.BigInt, 8)]
        public long? CSID { get { return _csid; } set { if (_csid != value) { _csid = value; isModify_CSID = true; } } }
        public long ValueOrDefault_CSID { get { return _csid.HasValue ? _csid.Value : 0; } }
        private bool isModify_CSID = false;
        [IsModifyAttribute("CSID")]
        public bool IsModify_CSID { get { return isModify_CSID; } }
        #endregion

        #region 字段 QS_RTID (自动生成)
        private int? _qs_rtid = null;
        [DBFieldAttribute("QS_RTID", SqlDbType.Int, 4)]
        public int? QS_RTID { get { return _qs_rtid; } set { if (_qs_rtid != value) { _qs_rtid = value; isModify_QS_RTID = true; } } }
        public int ValueOrDefault_QS_RTID { get { return _qs_rtid.HasValue ? _qs_rtid.Value : 0; } }
        private bool isModify_QS_RTID = false;
        [IsModifyAttribute("QS_RTID")]
        public bool IsModify_QS_RTID { get { return isModify_QS_RTID; } }
        #endregion

        #region 字段 SeatID (自动生成)
        private string _seatid = null;
        [DBFieldAttribute("SeatID", SqlDbType.VarChar, 50)]
        public string SeatID { get { return _seatid; } set { if (_seatid != value) { _seatid = value; isModify_SeatID = true; } } }
        public string ValueOrDefault_SeatID { get { return _seatid != null ? _seatid : ""; } }
        private bool isModify_SeatID = false;
        [IsModifyAttribute("SeatID")]
        public bool IsModify_SeatID { get { return isModify_SeatID; } }
        #endregion

        #region 字段 ScoreType (自动生成)
        private int? _scoretype = null;
        [DBFieldAttribute("ScoreType", SqlDbType.Int, 4)]
        public int? ScoreType { get { return _scoretype; } set { if (_scoretype != value) { _scoretype = value; isModify_ScoreType = true; } } }
        public int ValueOrDefault_ScoreType { get { return _scoretype.HasValue ? _scoretype.Value : 0; } }
        private bool isModify_ScoreType = false;
        [IsModifyAttribute("ScoreType")]
        public bool IsModify_ScoreType { get { return isModify_ScoreType; } }
        #endregion

        #region 字段 Score (自动生成)
        private decimal? _score = null;
        [DBFieldAttribute("Score", SqlDbType.Decimal)]
        public decimal? Score { get { return _score; } set { if (_score != value) { _score = value; isModify_Score = true; } } }
        public decimal ValueOrDefault_Score { get { return _score.HasValue ? _score.Value : 0; } }
        private bool isModify_Score = false;
        [IsModifyAttribute("Score")]
        public bool IsModify_Score { get { return isModify_Score; } }
        #endregion

        #region 字段 IsQualified (自动生成)
        private int? _isqualified = null;
        [DBFieldAttribute("IsQualified", SqlDbType.Int, 4)]
        public int? IsQualified { get { return _isqualified; } set { if (_isqualified != value) { _isqualified = value; isModify_IsQualified = true; } } }
        public int ValueOrDefault_IsQualified { get { return _isqualified.HasValue ? _isqualified.Value : 0; } }
        private bool isModify_IsQualified = false;
        [IsModifyAttribute("IsQualified")]
        public bool IsModify_IsQualified { get { return isModify_IsQualified; } }
        #endregion

        #region 字段 Status (自动生成)
        private int? _status = null;
        [DBFieldAttribute("Status", SqlDbType.Int, 4)]
        public int? Status { get { return _status; } set { if (_status != value) { _status = value; isModify_Status = true; } } }
        public int ValueOrDefault_Status { get { return _status.HasValue ? _status.Value : 0; } }
        private bool isModify_Status = false;
        [IsModifyAttribute("Status")]
        public bool IsModify_Status { get { return isModify_Status; } }
        #endregion

        #region 字段 StateResult (自动生成)
        private int? _stateresult = null;
        [DBFieldAttribute("StateResult", SqlDbType.Int, 4)]
        public int? StateResult { get { return _stateresult; } set { if (_stateresult != value) { _stateresult = value; isModify_StateResult = true; } } }
        public int ValueOrDefault_StateResult { get { return _stateresult.HasValue ? _stateresult.Value : 0; } }
        private bool isModify_StateResult = false;
        [IsModifyAttribute("StateResult")]
        public bool IsModify_StateResult { get { return isModify_StateResult; } }
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

        #region 字段 CreateUserID (自动生成)
        private int? _createuserid = null;
        [DBFieldAttribute("CreateUserID", SqlDbType.Int, 4)]
        public int? CreateUserID { get { return _createuserid; } set { if (_createuserid != value) { _createuserid = value; isModify_CreateUserID = true; } } }
        public int ValueOrDefault_CreateUserID { get { return _createuserid.HasValue ? _createuserid.Value : 0; } }
        private bool isModify_CreateUserID = false;
        [IsModifyAttribute("CreateUserID")]
        public bool IsModify_CreateUserID { get { return isModify_CreateUserID; } }
        #endregion

        #region 字段 ModifyTime (自动生成)
        private DateTime? _modifytime = null;
        [DBFieldAttribute("ModifyTime", SqlDbType.DateTime, 8)]
        public DateTime? ModifyTime { get { return _modifytime; } set { if (_modifytime != value) { _modifytime = value; isModify_ModifyTime = true; } } }
        public DateTime ValueOrDefault_ModifyTime { get { return _modifytime.HasValue ? _modifytime.Value : new DateTime(); } }
        private bool isModify_ModifyTime = false;
        [IsModifyAttribute("ModifyTime")]
        public bool IsModify_ModifyTime { get { return isModify_ModifyTime; } }
        #endregion

        #region 字段 ModifyUserID (自动生成)
        private int? _modifyuserid = null;
        [DBFieldAttribute("ModifyUserID", SqlDbType.Int, 4)]
        public int? ModifyUserID { get { return _modifyuserid; } set { if (_modifyuserid != value) { _modifyuserid = value; isModify_ModifyUserID = true; } } }
        public int ValueOrDefault_ModifyUserID { get { return _modifyuserid.HasValue ? _modifyuserid.Value : 0; } }
        private bool isModify_ModifyUserID = false;
        [IsModifyAttribute("ModifyUserID")]
        public bool IsModify_ModifyUserID { get { return isModify_ModifyUserID; } }
        #endregion

        #region 字段 QualityAppraisal (自动生成)
        private string _qualityappraisal = null;
        [DBFieldAttribute("QualityAppraisal", SqlDbType.VarChar, 2000)]
        public string QualityAppraisal { get { return _qualityappraisal; } set { if (_qualityappraisal != value) { _qualityappraisal = value; isModify_QualityAppraisal = true; } } }
        public string ValueOrDefault_QualityAppraisal { get { return _qualityappraisal != null ? _qualityappraisal : ""; } }
        private bool isModify_QualityAppraisal = false;
        [IsModifyAttribute("QualityAppraisal")]
        public bool IsModify_QualityAppraisal { get { return isModify_QualityAppraisal; } }
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
            isModify_QS_RID = b;
            isModify_CSID = b;
            isModify_QS_RTID = b;
            isModify_SeatID = b;
            isModify_ScoreType = b;
            isModify_Score = b;
            isModify_IsQualified = b;
            isModify_Status = b;
            isModify_StateResult = b;
            isModify_CreateTime = b;
            isModify_CreateUserID = b;
            isModify_ModifyTime = b;
            isModify_ModifyUserID = b;
            isModify_QualityAppraisal = b;
        }

        #endregion

    }
}
