using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类SurveyInfo 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class SurveyInfo
    {
        public SurveyInfo()
        {
            _siid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _bgid = Constant.INT_INVALID_VALUE;
            _scid = Constant.INT_INVALID_VALUE;
            _description = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _isavailable = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _taskid = Constant.STRING_INVALID_VALUE;
            _num = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private int _siid;
        private string _name;
        private int? _bgid;
        private int? _scid;
        private string _description;
        private int? _status;
        private int? _isavailable;
        private DateTime? _createtime;
        private int? _createuserid;
        private string _taskid;
        private int _num;

        public int Num
        {
            set { _num = value; }
            get { return _num; }
        }

        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SIID
        {
            set { _siid = value; }
            get { return _siid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SCID
        {
            set { _scid = value; }
            get { return _scid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsAvailable
        {
            set { _isavailable = value; }
            get { return _isavailable; }
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

