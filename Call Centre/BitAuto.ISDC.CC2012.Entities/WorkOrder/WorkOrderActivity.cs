using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类WorkOrderActivity 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-01-13 04:17:23 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class WorkOrderActivity
    {
        public WorkOrderActivity()
        {
            _orderid = Constant.STRING_INVALID_VALUE;
            //_activityguid没有默认值
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private string _orderid;
        private Guid _activityguid;
        private DateTime? _createtime;
        private int? _createuserid;
        /// <summary>
        /// 
        /// </summary>
        public string OrderID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid ActivityGUID
        {
            set { _activityguid = value; }
            get { return _activityguid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        #endregion Model

    }
}

