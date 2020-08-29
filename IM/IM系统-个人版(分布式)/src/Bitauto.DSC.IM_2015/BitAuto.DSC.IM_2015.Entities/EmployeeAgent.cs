using System;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类EmployeeAgent 。(属性说明自动提取数据库字段的描述信息)
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
    [Serializable]
    public class EmployeeAgent
    {
        public EmployeeAgent()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            _maxdialoguen = Constant.INT_INVALID_VALUE;
            _maxqueuen = Constant.INT_INVALID_VALUE;
            _lastlogintime = Constant.DATE_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private int _userid;
        private int? _maxdialoguen;
        private int? _maxqueuen;
        private DateTime? _lastlogintime;
        private DateTime _createtime;
        private int? _createuserid;

        /// <summary>
        /// 
        /// </summary>
        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxDialogueN
        {
            set { _maxdialoguen = value; }
            get { return _maxdialoguen; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxQueueN
        {
            set { _maxqueuen = value; }
            get { return _maxqueuen; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
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


        #region 扩展
        /// <summary>
        /// 员工编号
        /// </summary>
        public string AgentNum { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所属分组ID
        /// </summary>
        public int BGID { get; set; }
        /// <summary>
        /// 业务线ID列表
        /// </summary>
        public string BusinessLineIDs { get; set; }
        /// <summary>
        /// 管辖分组ID列表
        /// </summary>
        public string ManageBGIDs { get; set; }

        /// <summary>
        /// 所在分组名称
        /// </summary>
        public string BGName { get; set; }


        #endregion

    }
}

