using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：YTGActivityTaskInfo
    /// <summary>
    /// 实体类：YTGActivityTaskInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2014-12-19
    /// </summary>
    [DBTableAttribute("YTGActivityTask")]
    public class YTGActivityTaskInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public YTGActivityTaskInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public YTGActivityTaskInfo(string _taskid)
        {
            this._taskid = _taskid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public YTGActivityTaskInfo(DataRow dr)
            : this()
        {
            #region 表字段转换
            _taskid = (!dr.Table.Columns.Contains("TaskID") || dr["TaskID"] == null || dr["TaskID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["TaskID"]);
            _projectid = (!dr.Table.Columns.Contains("ProjectID") || dr["ProjectID"] == null || dr["ProjectID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ProjectID"]);
            _activityid = (!dr.Table.Columns.Contains("ActivityID") || dr["ActivityID"] == null || dr["ActivityID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["ActivityID"]);
            _signid = (!dr.Table.Columns.Contains("SignID") || dr["SignID"] == null || dr["SignID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["SignID"]);
            _username = (!dr.Table.Columns.Contains("UserName") || dr["UserName"] == null || dr["UserName"] == DBNull.Value) ? null : (string)Convert.ToString(dr["UserName"]);
            _tel = (!dr.Table.Columns.Contains("Tel") || dr["Tel"] == null || dr["Tel"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Tel"]);
            _sex = (!dr.Table.Columns.Contains("Sex") || dr["Sex"] == null || dr["Sex"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Sex"]);
            _provinceid = (!dr.Table.Columns.Contains("ProvinceID") || dr["ProvinceID"] == null || dr["ProvinceID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["ProvinceID"]);
            _cityid = (!dr.Table.Columns.Contains("CityID") || dr["CityID"] == null || dr["CityID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CityID"]);
            _dealerid = (!dr.Table.Columns.Contains("DealerID") || dr["DealerID"] == null || dr["DealerID"] == DBNull.Value) ? null : (string)Convert.ToString(dr["DealerID"]);
            _ordercreatetime = (!dr.Table.Columns.Contains("OrderCreateTime") || dr["OrderCreateTime"] == null || dr["OrderCreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["OrderCreateTime"]);
            _ordercarmasterid = (!dr.Table.Columns.Contains("OrderCarMasterID") || dr["OrderCarMasterID"] == null || dr["OrderCarMasterID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["OrderCarMasterID"]);
            _ordercarserialid = (!dr.Table.Columns.Contains("OrderCarSerialID") || dr["OrderCarSerialID"] == null || dr["OrderCarSerialID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["OrderCarSerialID"]);
            _ordercarid = (!dr.Table.Columns.Contains("OrderCarID") || dr["OrderCarID"] == null || dr["OrderCarID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["OrderCarID"]);
            _dcarmasterid = (!dr.Table.Columns.Contains("DCarMasterID") || dr["DCarMasterID"] == null || dr["DCarMasterID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["DCarMasterID"]);
            _dcarserialid = (!dr.Table.Columns.Contains("DCarSerialID") || dr["DCarSerialID"] == null || dr["DCarSerialID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["DCarSerialID"]);
            _dcarid = (!dr.Table.Columns.Contains("DCarID") || dr["DCarID"] == null || dr["DCarID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["DCarID"]);
            _pbuycartime = (!dr.Table.Columns.Contains("PBuyCarTime") || dr["PBuyCarTime"] == null || dr["PBuyCarTime"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["PBuyCarTime"]);
            _testdriveprovinceid = (!dr.Table.Columns.Contains("TestDriveProvinceID") || dr["TestDriveProvinceID"] == null || dr["TestDriveProvinceID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["TestDriveProvinceID"]);
            _testdrivecityid = (!dr.Table.Columns.Contains("TestDriveCityID") || dr["TestDriveCityID"] == null || dr["TestDriveCityID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["TestDriveCityID"]);
            _isjt = (!dr.Table.Columns.Contains("IsJT") || dr["IsJT"] == null || dr["IsJT"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["IsJT"]);
            _issuccess = (!dr.Table.Columns.Contains("IsSuccess") || dr["IsSuccess"] == null || dr["IsSuccess"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["IsSuccess"]);
            _failreason = (!dr.Table.Columns.Contains("FailReason") || dr["FailReason"] == null || dr["FailReason"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["FailReason"]);
            _remark = (!dr.Table.Columns.Contains("Remark") || dr["Remark"] == null || dr["Remark"] == DBNull.Value) ? null : (string)Convert.ToString(dr["Remark"]);
            _assignuserid = (!dr.Table.Columns.Contains("AssignUserID") || dr["AssignUserID"] == null || dr["AssignUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["AssignUserID"]);
            _status = (!dr.Table.Columns.Contains("Status") || dr["Status"] == null || dr["Status"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["Status"]);
            _createtime = (!dr.Table.Columns.Contains("CreateTime") || dr["CreateTime"] == null || dr["CreateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["CreateTime"]);
            _createuserid = (!dr.Table.Columns.Contains("CreateUserID") || dr["CreateUserID"] == null || dr["CreateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["CreateUserID"]);
            _lastupdatetime = (!dr.Table.Columns.Contains("LastUpdateTime") || dr["LastUpdateTime"] == null || dr["LastUpdateTime"] == DBNull.Value) ? null : (DateTime?)Convert.ToDateTime(dr["LastUpdateTime"]);
            _lastupdateuserid = (!dr.Table.Columns.Contains("LastUpdateUserID") || dr["LastUpdateUserID"] == null || dr["LastUpdateUserID"] == DBNull.Value) ? null : (int?)Convert.ToInt32(dr["LastUpdateUserID"]);
            #endregion

        }

        #endregion

        #region 属性
        #region 字段 TaskID (自动生成)
        private string _taskid = null;
        [DBFieldAttribute("TaskID", SqlDbType.VarChar, 50, true)]
        public string TaskID { get { return _taskid; } set { if (_taskid != value) { _taskid = value; isModify_TaskID = true; } } }
        public string ValueOrDefault_TaskID { get { return _taskid != null ? _taskid : ""; } }
        private bool isModify_TaskID = false;
        [IsModifyAttribute("TaskID")]
        public bool IsModify_TaskID { get { return isModify_TaskID; } }
        #endregion

        #region 字段 ProjectID (自动生成)
        private long? _projectid = null;
        [DBFieldAttribute("ProjectID", SqlDbType.BigInt)]
        public long? ProjectID { get { return _projectid; } set { if (_projectid != value) { _projectid = value; isModify_ProjectID = true; } } }
        public long ValueOrDefault_ProjectID { get { return _projectid.HasValue ? _projectid.Value : 0; } }
        private bool isModify_ProjectID = false;
        [IsModifyAttribute("ProjectID")]
        public bool IsModify_ProjectID { get { return isModify_ProjectID; } }
        #endregion

        #region 字段 ActivityID (自动生成)
        private string _activityid = null;
        [DBFieldAttribute("ActivityID", SqlDbType.VarChar, 50)]
        public string ActivityID { get { return _activityid; } set { if (_activityid != value) { _activityid = value; isModify_ActivityID = true; } } }
        public string ValueOrDefault_ActivityID { get { return _activityid != null ? _activityid : ""; } }
        private bool isModify_ActivityID = false;
        [IsModifyAttribute("ActivityID")]
        public bool IsModify_ActivityID { get { return isModify_ActivityID; } }
        #endregion

        #region 字段 SignID (自动生成)
        private string _signid = null;
        [DBFieldAttribute("SignID", SqlDbType.VarChar, 50)]
        public string SignID { get { return _signid; } set { if (_signid != value) { _signid = value; isModify_SignID = true; } } }
        public string ValueOrDefault_SignID { get { return _signid != null ? _signid : ""; } }
        private bool isModify_SignID = false;
        [IsModifyAttribute("SignID")]
        public bool IsModify_SignID { get { return isModify_SignID; } }
        #endregion

        #region 字段 UserName (自动生成)
        private string _username = null;
        [DBFieldAttribute("UserName", SqlDbType.VarChar, 50)]
        public string UserName { get { return _username; } set { if (_username != value) { _username = value; isModify_UserName = true; } } }
        public string ValueOrDefault_UserName { get { return _username != null ? _username : ""; } }
        private bool isModify_UserName = false;
        [IsModifyAttribute("UserName")]
        public bool IsModify_UserName { get { return isModify_UserName; } }
        #endregion

        #region 字段 Tel (自动生成)
        private string _tel = null;
        [DBFieldAttribute("Tel", SqlDbType.VarChar, 20)]
        public string Tel { get { return _tel; } set { if (_tel != value) { _tel = value; isModify_Tel = true; } } }
        public string ValueOrDefault_Tel { get { return _tel != null ? _tel : ""; } }
        private bool isModify_Tel = false;
        [IsModifyAttribute("Tel")]
        public bool IsModify_Tel { get { return isModify_Tel; } }
        #endregion

        #region 字段 Sex (自动生成)
        private int? _sex = null;
        [DBFieldAttribute("Sex", SqlDbType.Int, 4)]
        public int? Sex { get { return _sex; } set { if (_sex != value) { _sex = value; isModify_Sex = true; } } }
        public int ValueOrDefault_Sex { get { return _sex.HasValue ? _sex.Value : 0; } }
        private bool isModify_Sex = false;
        [IsModifyAttribute("Sex")]
        public bool IsModify_Sex { get { return isModify_Sex; } }
        #endregion

        #region 字段 ProvinceID (自动生成)
        private int? _provinceid = null;
        [DBFieldAttribute("ProvinceID", SqlDbType.Int, 4)]
        public int? ProvinceID { get { return _provinceid; } set { if (_provinceid != value) { _provinceid = value; isModify_ProvinceID = true; } } }
        public int ValueOrDefault_ProvinceID { get { return _provinceid.HasValue ? _provinceid.Value : 0; } }
        private bool isModify_ProvinceID = false;
        [IsModifyAttribute("ProvinceID")]
        public bool IsModify_ProvinceID { get { return isModify_ProvinceID; } }
        #endregion

        #region 字段 CityID (自动生成)
        private int? _cityid = null;
        [DBFieldAttribute("CityID", SqlDbType.Int, 4)]
        public int? CityID { get { return _cityid; } set { if (_cityid != value) { _cityid = value; isModify_CityID = true; } } }
        public int ValueOrDefault_CityID { get { return _cityid.HasValue ? _cityid.Value : 0; } }
        private bool isModify_CityID = false;
        [IsModifyAttribute("CityID")]
        public bool IsModify_CityID { get { return isModify_CityID; } }
        #endregion

        #region 字段 DealerID (自动生成)
        private string _dealerid = null;
        [DBFieldAttribute("DealerID", SqlDbType.VarChar, 50)]
        public string DealerID { get { return _dealerid; } set { if (_dealerid != value) { _dealerid = value; isModify_DealerID = true; } } }
        public string ValueOrDefault_DealerID { get { return _dealerid != null ? _dealerid : ""; } }
        private bool isModify_DealerID = false;
        [IsModifyAttribute("DealerID")]
        public bool IsModify_DealerID { get { return isModify_DealerID; } }
        #endregion

        #region 字段 OrderCreateTime (自动生成)
        private DateTime? _ordercreatetime = null;
        [DBFieldAttribute("OrderCreateTime", SqlDbType.DateTime, 8)]
        public DateTime? OrderCreateTime { get { return _ordercreatetime; } set { if (_ordercreatetime != value) { _ordercreatetime = value; isModify_OrderCreateTime = true; } } }
        public DateTime ValueOrDefault_OrderCreateTime { get { return _ordercreatetime.HasValue ? _ordercreatetime.Value : new DateTime(); } }
        private bool isModify_OrderCreateTime = false;
        [IsModifyAttribute("OrderCreateTime")]
        public bool IsModify_OrderCreateTime { get { return isModify_OrderCreateTime; } }
        #endregion

        #region 字段 OrderCarMasterID (自动生成)
        private int? _ordercarmasterid = null;
        [DBFieldAttribute("OrderCarMasterID", SqlDbType.Int, 4)]
        public int? OrderCarMasterID { get { return _ordercarmasterid; } set { if (_ordercarmasterid != value) { _ordercarmasterid = value; isModify_OrderCarMasterID = true; } } }
        public int ValueOrDefault_OrderCarMasterID { get { return _ordercarmasterid.HasValue ? _ordercarmasterid.Value : 0; } }
        private bool isModify_OrderCarMasterID = false;
        [IsModifyAttribute("OrderCarMasterID")]
        public bool IsModify_OrderCarMasterID { get { return isModify_OrderCarMasterID; } }
        #endregion

        #region 字段 OrderCarSerialID (自动生成)
        private int? _ordercarserialid = null;
        [DBFieldAttribute("OrderCarSerialID", SqlDbType.Int, 4)]
        public int? OrderCarSerialID { get { return _ordercarserialid; } set { if (_ordercarserialid != value) { _ordercarserialid = value; isModify_OrderCarSerialID = true; } } }
        public int ValueOrDefault_OrderCarSerialID { get { return _ordercarserialid.HasValue ? _ordercarserialid.Value : 0; } }
        private bool isModify_OrderCarSerialID = false;
        [IsModifyAttribute("OrderCarSerialID")]
        public bool IsModify_OrderCarSerialID { get { return isModify_OrderCarSerialID; } }
        #endregion

        #region 字段 OrderCarID (自动生成)
        private int? _ordercarid = null;
        [DBFieldAttribute("OrderCarID", SqlDbType.Int, 4)]
        public int? OrderCarID { get { return _ordercarid; } set { if (_ordercarid != value) { _ordercarid = value; isModify_OrderCarID = true; } } }
        public int ValueOrDefault_OrderCarID { get { return _ordercarid.HasValue ? _ordercarid.Value : 0; } }
        private bool isModify_OrderCarID = false;
        [IsModifyAttribute("OrderCarID")]
        public bool IsModify_OrderCarID { get { return isModify_OrderCarID; } }
        #endregion

        #region 字段 DCarMasterID (自动生成)
        private int? _dcarmasterid = null;
        [DBFieldAttribute("DCarMasterID", SqlDbType.Int, 4)]
        public int? DCarMasterID { get { return _dcarmasterid; } set { if (_dcarmasterid != value) { _dcarmasterid = value; isModify_DCarMasterID = true; } } }
        public int ValueOrDefault_DCarMasterID { get { return _dcarmasterid.HasValue ? _dcarmasterid.Value : 0; } }
        private bool isModify_DCarMasterID = false;
        [IsModifyAttribute("DCarMasterID")]
        public bool IsModify_DCarMasterID { get { return isModify_DCarMasterID; } }
        #endregion

        #region 字段 DCarSerialID (自动生成)
        private int? _dcarserialid = null;
        [DBFieldAttribute("DCarSerialID", SqlDbType.Int, 4)]
        public int? DCarSerialID { get { return _dcarserialid; } set { if (_dcarserialid != value) { _dcarserialid = value; isModify_DCarSerialID = true; } } }
        public int ValueOrDefault_DCarSerialID { get { return _dcarserialid.HasValue ? _dcarserialid.Value : 0; } }
        private bool isModify_DCarSerialID = false;
        [IsModifyAttribute("DCarSerialID")]
        public bool IsModify_DCarSerialID { get { return isModify_DCarSerialID; } }
        #endregion

        #region 字段 DCarID (自动生成)
        private int? _dcarid = null;
        [DBFieldAttribute("DCarID", SqlDbType.Int, 4)]
        public int? DCarID { get { return _dcarid; } set { if (_dcarid != value) { _dcarid = value; isModify_DCarID = true; } } }
        public int ValueOrDefault_DCarID { get { return _dcarid.HasValue ? _dcarid.Value : 0; } }
        private bool isModify_DCarID = false;
        [IsModifyAttribute("DCarID")]
        public bool IsModify_DCarID { get { return isModify_DCarID; } }
        #endregion

        #region 字段 PBuyCarTime (自动生成)
        private int? _pbuycartime = null;
        [DBFieldAttribute("PBuyCarTime", SqlDbType.Int, 4)]
        public int? PBuyCarTime { get { return _pbuycartime; } set { if (_pbuycartime != value) { _pbuycartime = value; isModify_PBuyCarTime = true; } } }
        public int ValueOrDefault_PBuyCarTime { get { return _pbuycartime.HasValue ? _pbuycartime.Value : 0; } }
        private bool isModify_PBuyCarTime = false;
        [IsModifyAttribute("PBuyCarTime")]
        public bool IsModify_PBuyCarTime { get { return isModify_PBuyCarTime; } }
        #endregion

        #region 字段 TestDriveProvinceID (自动生成)
        private int? _testdriveprovinceid = null;
        [DBFieldAttribute("TestDriveProvinceID", SqlDbType.Int, 4)]
        public int? TestDriveProvinceID { get { return _testdriveprovinceid; } set { if (_testdriveprovinceid != value) { _testdriveprovinceid = value; isModify_TestDriveProvinceID = true; } } }
        public int ValueOrDefault_TestDriveProvinceID { get { return _testdriveprovinceid.HasValue ? _testdriveprovinceid.Value : 0; } }
        private bool isModify_TestDriveProvinceID = false;
        [IsModifyAttribute("TestDriveProvinceID")]
        public bool IsModify_TestDriveProvinceID { get { return isModify_TestDriveProvinceID; } }
        #endregion

        #region 字段 TestDriveCityID (自动生成)
        private int? _testdrivecityid = null;
        [DBFieldAttribute("TestDriveCityID", SqlDbType.Int, 4)]
        public int? TestDriveCityID { get { return _testdrivecityid; } set { if (_testdrivecityid != value) { _testdrivecityid = value; isModify_TestDriveCityID = true; } } }
        public int ValueOrDefault_TestDriveCityID { get { return _testdrivecityid.HasValue ? _testdrivecityid.Value : 0; } }
        private bool isModify_TestDriveCityID = false;
        [IsModifyAttribute("TestDriveCityID")]
        public bool IsModify_TestDriveCityID { get { return isModify_TestDriveCityID; } }
        #endregion

        #region 字段 IsJT (自动生成)
        private int? _isjt = null;
        [DBFieldAttribute("IsJT", SqlDbType.Int, 4)]
        public int? IsJT { get { return _isjt; } set { if (_isjt != value) { _isjt = value; isModify_IsJT = true; } } }
        public int ValueOrDefault_IsJT { get { return _isjt.HasValue ? _isjt.Value : 0; } }
        private bool isModify_IsJT = false;
        [IsModifyAttribute("IsJT")]
        public bool IsModify_IsJT { get { return isModify_IsJT; } }
        #endregion

        #region 字段 IsSuccess (自动生成)
        private int? _issuccess = null;
        [DBFieldAttribute("IsSuccess", SqlDbType.Int, 4)]
        public int? IsSuccess { get { return _issuccess; } set { if (_issuccess != value) { _issuccess = value; isModify_IsSuccess = true; } } }
        public int ValueOrDefault_IsSuccess { get { return _issuccess.HasValue ? _issuccess.Value : 0; } }
        private bool isModify_IsSuccess = false;
        [IsModifyAttribute("IsSuccess")]
        public bool IsModify_IsSuccess { get { return isModify_IsSuccess; } }
        #endregion

        #region 字段 FailReason (自动生成)
        private int? _failreason = null;
        [DBFieldAttribute("FailReason", SqlDbType.Int, 4)]
        public int? FailReason { get { return _failreason; } set { if (_failreason != value) { _failreason = value; isModify_FailReason = true; } } }
        public int ValueOrDefault_FailReason { get { return _failreason.HasValue ? _failreason.Value : 0; } }
        private bool isModify_FailReason = false;
        [IsModifyAttribute("FailReason")]
        public bool IsModify_FailReason { get { return isModify_FailReason; } }
        #endregion

        #region 字段 Remark (自动生成)
        private string _remark = null;
        [DBFieldAttribute("Remark", SqlDbType.NVarChar, 400)]
        public string Remark { get { return _remark; } set { if (_remark != value) { _remark = value; isModify_Remark = true; } } }
        public string ValueOrDefault_Remark { get { return _remark != null ? _remark : ""; } }
        private bool isModify_Remark = false;
        [IsModifyAttribute("Remark")]
        public bool IsModify_Remark { get { return isModify_Remark; } }
        #endregion

        #region 字段 AssignUserID (自动生成)
        private int? _assignuserid = null;
        [DBFieldAttribute("AssignUserID", SqlDbType.Int, 4)]
        public int? AssignUserID { get { return _assignuserid; } set { if (_assignuserid != value) { _assignuserid = value; isModify_AssignUserID = true; } } }
        public int ValueOrDefault_AssignUserID { get { return _assignuserid.HasValue ? _assignuserid.Value : 0; } }
        private bool isModify_AssignUserID = false;
        [IsModifyAttribute("AssignUserID")]
        public bool IsModify_AssignUserID { get { return isModify_AssignUserID; } }
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

        #region 字段 LastUpdateTime (自动生成)
        private DateTime? _lastupdatetime = null;
        [DBFieldAttribute("LastUpdateTime", SqlDbType.DateTime, 8)]
        public DateTime? LastUpdateTime { get { return _lastupdatetime; } set { if (_lastupdatetime != value) { _lastupdatetime = value; isModify_LastUpdateTime = true; } } }
        public DateTime ValueOrDefault_LastUpdateTime { get { return _lastupdatetime.HasValue ? _lastupdatetime.Value : new DateTime(); } }
        private bool isModify_LastUpdateTime = false;
        [IsModifyAttribute("LastUpdateTime")]
        public bool IsModify_LastUpdateTime { get { return isModify_LastUpdateTime; } }
        #endregion

        #region 字段 LastUpdateUserID (自动生成)
        private int? _lastupdateuserid = null;
        [DBFieldAttribute("LastUpdateUserID", SqlDbType.Int, 4)]
        public int? LastUpdateUserID { get { return _lastupdateuserid; } set { if (_lastupdateuserid != value) { _lastupdateuserid = value; isModify_LastUpdateUserID = true; } } }
        public int ValueOrDefault_LastUpdateUserID { get { return _lastupdateuserid.HasValue ? _lastupdateuserid.Value : 0; } }
        private bool isModify_LastUpdateUserID = false;
        [IsModifyAttribute("LastUpdateUserID")]
        public bool IsModify_LastUpdateUserID { get { return isModify_LastUpdateUserID; } }
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
            isModify_TaskID = b;
            isModify_ProjectID = b;
            isModify_ActivityID = b;
            isModify_SignID = b;
            isModify_UserName = b;
            isModify_Tel = b;
            isModify_Sex = b;
            isModify_ProvinceID = b;
            isModify_CityID = b;
            isModify_DealerID = b;
            isModify_OrderCreateTime = b;
            isModify_OrderCarMasterID = b;
            isModify_OrderCarSerialID = b;
            isModify_OrderCarID = b;
            isModify_DCarMasterID = b;
            isModify_DCarSerialID = b;
            isModify_DCarID = b;
            isModify_PBuyCarTime = b;
            isModify_TestDriveProvinceID = b;
            isModify_TestDriveCityID = b;
            isModify_IsJT = b;
            isModify_IsSuccess = b;
            isModify_FailReason = b;
            isModify_Remark = b;
            isModify_AssignUserID = b;
            isModify_Status = b;
            isModify_CreateTime = b;
            isModify_CreateUserID = b;
            isModify_LastUpdateTime = b;
            isModify_LastUpdateUserID = b;
        }

        #endregion

    }
}
