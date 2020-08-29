using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class UserSkillDataRight
    {
        public UserSkillDataRight()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            _sgid = Constant.INT_INVALID_VALUE;
            _skillpriority = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private int? _userid;
        private int? _sgid;
        private int? _skillpriority;
        private DateTime? _createtime;
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
        public int? UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 技能组ID
        /// </summary>
        public int? SGID
        {
            set { _sgid = value; }
            get { return _sgid; }
        }
        /// <summary>
        /// 技能优先级: 1为高;2为中;3为低
        /// </summary>
        public int? SkillPriority
        {
            set { _skillpriority = value; }
            get { return _skillpriority; }
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
