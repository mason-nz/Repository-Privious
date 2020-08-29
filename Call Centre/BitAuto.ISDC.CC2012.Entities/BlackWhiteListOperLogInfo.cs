using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 实体类：BlackWhiteListOperLogInfo
    /// <summary>
    /// 实体类：BlackWhiteListOperLogInfo
    /// 自动生成（Copyright ©  2014 qiangfei Powered By [ADO自动生成工具] Version  1.2014.10.25）
    /// 2016-03-16
    /// </summary>
    [DBTableAttribute("BlackWhiteListOperLog")]
    public class BlackWhiteListOperLogInfo
    {
        #region 构造方法
        /// 构造方法 (自动生成)
        /// <summary>
        /// 构造方法 (自动生成)
        /// </summary>
        public BlackWhiteListOperLogInfo()
        {
        }


        /// 构造方法（参数：主键序列）(自动生成)
        /// <summary>
        /// 构造方法（参数：主键序列）(自动生成)
        /// </summary>
        public BlackWhiteListOperLogInfo(int _recid)
            : this()
        {
            this._recid = _recid;
        }


        /// 构造方法（附带数据转换） (自动生成)
        /// <summary>
        /// 构造方法（附带数据转换） (自动生成)
        /// </summary>
        /// <param name="dr"></param>
        public BlackWhiteListOperLogInfo(DataRow dr)
            : this()
        {
            AttributeHelper.SetValues(this, dr);
            SetModify(false);
        }

        #endregion

        #region 属性
        #region 字段 RecID (自动生成)
        /// <summary>
        /// 自增主键
        /// </summary>
        [DBField("RecID", SqlDbType.Int, 4, true, true)]
        public int? RecID { get { return _recid; } set { if (_recid != value) { _recid = value; IsModify_RecID = true; } } }
        public int RecID_Value { get { return _recid.HasValue ? _recid.Value : 0; } }
        private int? _recid = null;

        [IsModify("RecID")]
        public bool IsModify_RecID { get; set; }
        #endregion

        #region 字段 BWRecID (自动生成)
        /// <summary>
        /// 关联主表主键
        /// </summary>
        [DBField("BWRecID", SqlDbType.Int, 4)]
        public int? BWRecID { get { return _bwrecid; } set { if (_bwrecid != value) { _bwrecid = value; IsModify_BWRecID = true; } } }
        public int BWRecID_Value { get { return _bwrecid.HasValue ? _bwrecid.Value : 0; } }
        private int? _bwrecid = null;

        [IsModify("BWRecID")]
        public bool IsModify_BWRecID { get; set; }
        #endregion

        #region 字段 BWType (自动生成)
        /// <summary>
        /// 冗余主表类型
        /// </summary>
        [DBField("BWType", SqlDbType.Int, 4)]
        public int? BWType { get { return _bwtype; } set { if (_bwtype != value) { _bwtype = value; IsModify_BWType = true; } } }
        public int BWType_Value { get { return _bwtype.HasValue ? _bwtype.Value : 0; } }
        private int? _bwtype = null;

        [IsModify("BWType")]
        public bool IsModify_BWType { get; set; }
        #endregion

        #region 字段 PhoneNum (自动生成)
        /// <summary>
        /// 冗余主表电话
        /// </summary>
        [DBField("PhoneNum", SqlDbType.VarChar, 20)]
        public string PhoneNum { get { return _phonenum; } set { if (_phonenum != value) { _phonenum = value; IsModify_PhoneNum = true; } } }
        public string PhoneNum_Value { get { return _phonenum != null ? _phonenum : ""; } }
        private string _phonenum = null;

        [IsModify("PhoneNum")]
        public bool IsModify_PhoneNum { get; set; }
        #endregion

        #region 字段 CallID (自动生成)
        /// <summary>
        /// 话务ID
        /// </summary>
        [DBField("CallID", SqlDbType.BigInt, 8)]
        public long? CallID { get { return _callid; } set { if (_callid != value) { _callid = value; IsModify_CallID = true; } } }
        public long CallID_Value { get { return _callid.HasValue ? _callid.Value : 0; } }
        private long? _callid = null;

        [IsModify("CallID")]
        public bool IsModify_CallID { get; set; }
        #endregion

        #region 字段 OperType (自动生成)
        /// <summary>
        /// 操作类型 1 新增 2修改 3删除
        /// </summary>
        [DBField("OperType", SqlDbType.Int, 4)]
        public int? OperType { get { return _opertype; } set { if (_opertype != value) { _opertype = value; IsModify_OperType = true; } } }
        public int OperType_Value { get { return _opertype.HasValue ? _opertype.Value : 0; } }
        private int? _opertype = null;

        [IsModify("OperType")]
        public bool IsModify_OperType { get; set; }
        #endregion

        #region 字段 OperUserID (自动生成)
        /// <summary>
        /// 操作人
        /// </summary>
        [DBField("OperUserID", SqlDbType.Int, 4)]
        public int? OperUserID { get { return _operuserid; } set { if (_operuserid != value) { _operuserid = value; IsModify_OperUserID = true; } } }
        public int OperUserID_Value { get { return _operuserid.HasValue ? _operuserid.Value : 0; } }
        private int? _operuserid = null;

        [IsModify("OperUserID")]
        public bool IsModify_OperUserID { get; set; }
        #endregion

        #region 字段 OperTime (自动生成)
        /// <summary>
        /// 操作时间
        /// </summary>
        [DBField("OperTime", SqlDbType.DateTime, 8)]
        public DateTime? OperTime { get { return _opertime; } set { if (_opertime != value) { _opertime = value; IsModify_OperTime = true; } } }
        public DateTime OperTime_Value { get { return _opertime.HasValue ? _opertime.Value : new DateTime(); } }
        private DateTime? _opertime = null;

        [IsModify("OperTime")]
        public bool IsModify_OperTime { get; set; }
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
            IsModify_BWRecID = b;
            IsModify_BWType = b;
            IsModify_PhoneNum = b;
            IsModify_CallID = b;
            IsModify_OperType = b;
            IsModify_OperUserID = b;
            IsModify_OperTime = b;
        }

        #endregion

    }
}
