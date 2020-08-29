using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：WOrderDataInfo [任务列表]-[工单V2]-工单数据关联表 
    /// <summary>
    /// 实体类：WOrderDataInfo [任务列表]-[工单V2]-工单数据关联表 
    /// 自动生成（Copyright ?  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-07-19
    /// </summary>
    [DBTableAttribute("WOrderData")]
    public class WOrderDataInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public WOrderDataInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public WOrderDataInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public WOrderDataInfo(DataRow dr)
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

        #region OrderID [工单ID]
        /// <summary>
        /// 工单ID
        /// </summary>
        [DBField("OrderID", SqlDbType.VarChar, 20)]
        public string OrderID { get { return _orderid; } set { if (_orderid != value) { _orderid = value; IsModify_OrderID = true; } } }
        /// <summary>
        /// 工单ID
        /// </summary>
        public string OrderID_Value { get { return _orderid != null ? _orderid : ""; } }
        private string _orderid = null;

        [IsModify("OrderID")]
        public bool IsModify_OrderID { get; set; }
        #endregion

        #region ReceiverID [回复ID；是工单主表的相关数据时，值=-1]
        /// <summary>
        /// 回复ID；是工单主表的相关数据时，值=-1
        /// </summary>
        [DBField("ReceiverID", SqlDbType.Int, 4)]
        public int? ReceiverID { get { return _receiverid; } set { if (_receiverid != value) { _receiverid = value; IsModify_ReceiverID = true; } } }
        /// <summary>
        /// 回复ID；是工单主表的相关数据时，值=-1
        /// </summary>
        public int ReceiverID_Value { get { return _receiverid.HasValue ? _receiverid.Value : 0; } }
        private int? _receiverid = null;

        [IsModify("ReceiverID")]
        public bool IsModify_ReceiverID { get; set; }
        #endregion

        #region DataType [数据类型1=话务 2=对话]
        /// <summary>
        /// 数据类型1=话务 2=对话
        /// </summary>
        [DBField("DataType", SqlDbType.Int, 4)]
        public int? DataType { get { return _datatype; } set { if (_datatype != value) { _datatype = value; IsModify_DataType = true; } } }
        /// <summary>
        /// 数据类型1=话务 2=对话
        /// </summary>
        public int DataType_Value { get { return _datatype.HasValue ? _datatype.Value : 0; } }
        private int? _datatype = null;

        [IsModify("DataType")]
        public bool IsModify_DataType { get; set; }
        #endregion

        #region DataID [话务ID/对话ID （如果有多条只记录最后一条）]
        /// <summary>
        /// 话务ID/对话ID （如果有多条只记录最后一条）
        /// </summary>
        [DBField("DataID", SqlDbType.BigInt, 8)]
        public long? DataID { get { return _dataid; } set { if (_dataid != value) { _dataid = value; IsModify_DataID = true; } } }
        /// <summary>
        /// 话务ID/对话ID （如果有多条只记录最后一条）
        /// </summary>
        public long DataID_Value { get { return _dataid.HasValue ? _dataid.Value : 0; } }
        private long? _dataid = null;

        [IsModify("DataID")]
        public bool IsModify_DataID { get; set; }
        #endregion

        #region StartTime [接通时间/开始时间]
        /// <summary>
        /// 接通时间/开始时间
        /// </summary>
        [DBField("StartTime", SqlDbType.DateTime, 8)]
        public DateTime? StartTime { get { return _starttime; } set { if (_starttime != value) { _starttime = value; IsModify_StartTime = true; } } }
        /// <summary>
        /// 接通时间/开始时间
        /// </summary>
        public DateTime StartTime_Value { get { return _starttime.HasValue ? _starttime.Value : new DateTime(); } }
        private DateTime? _starttime = null;

        [IsModify("StartTime")]
        public bool IsModify_StartTime { get; set; }
        #endregion

        #region EndTime [挂断时间/结束时间]
        /// <summary>
        /// 挂断时间/结束时间
        /// </summary>
        [DBField("EndTime", SqlDbType.DateTime, 8)]
        public DateTime? EndTime { get { return _endtime; } set { if (_endtime != value) { _endtime = value; IsModify_EndTime = true; } } }
        /// <summary>
        /// 挂断时间/结束时间
        /// </summary>
        public DateTime EndTime_Value { get { return _endtime.HasValue ? _endtime.Value : new DateTime(); } }
        private DateTime? _endtime = null;

        [IsModify("EndTime")]
        public bool IsModify_EndTime { get; set; }
        #endregion

        #region TallTime [通话时长/对话时长]
        /// <summary>
        /// 通话时长/对话时长
        /// </summary>
        [DBField("TallTime", SqlDbType.Int, 4)]
        public int? TallTime { get { return _talltime; } set { if (_talltime != value) { _talltime = value; IsModify_TallTime = true; } } }
        /// <summary>
        /// 通话时长/对话时长
        /// </summary>
        public int TallTime_Value { get { return _talltime.HasValue ? _talltime.Value : 0; } }
        private int? _talltime = null;

        [IsModify("TallTime")]
        public bool IsModify_TallTime { get; set; }
        #endregion

        #region AudioURL [录音地址]
        /// <summary>
        /// 录音地址
        /// </summary>
        [DBField("AudioURL", SqlDbType.VarChar, 500)]
        public string AudioURL { get { return _audiourl; } set { if (_audiourl != value) { _audiourl = value; IsModify_AudioURL = true; } } }
        /// <summary>
        /// 录音地址
        /// </summary>
        public string AudioURL_Value { get { return _audiourl != null ? _audiourl : ""; } }
        private string _audiourl = null;

        [IsModify("AudioURL")]
        public bool IsModify_AudioURL { get; set; }
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

        #region 方法
        /// 设置是否更新所有字段 (自动生成)
        /// <summary>
        /// 设置是否更新所有字段 (自动生成)
        /// </summary>
        /// <param name="b"></param>
        public void SetModify(bool b)
        {
            IsModify_RecID = b;
            IsModify_OrderID = b;
            IsModify_ReceiverID = b;
            IsModify_DataType = b;
            IsModify_DataID = b;
            IsModify_StartTime = b;
            IsModify_EndTime = b;
            IsModify_TallTime = b;
            IsModify_AudioURL = b;
            IsModify_Status = b;
            IsModify_CreateUserID = b;
            IsModify_CreateTime = b;
        }

        #endregion

    }
}
