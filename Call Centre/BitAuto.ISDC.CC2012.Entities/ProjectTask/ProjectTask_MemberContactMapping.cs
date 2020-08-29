using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class ProjectTask_MemberContactMapping
    {
        public ProjectTask_MemberContactMapping()
        {
            _recid = Constants.Constant.INT_INVALID_VALUE;
            _memberid = Guid.NewGuid();
            _contactid = Constants.Constant.INT_INVALID_VALUE;
            _ismain = Constants.Constant.INT_INVALID_VALUE;
            _createtime = Constants.Constant.DATE_INVALID_VALUE;
        }
        #region Model
        private int _recid;
        private Guid _memberid;
        private int? _contactid;
        private int? _ismain;
        private DateTime? _createtime;
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
        public Guid MemberID
        {
            set { _memberid = value; }
            get { return _memberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContactID
        {
            set { _contactid = value; }
            get { return _contactid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsMain
        {
            set { _ismain = value; }
            get { return _ismain; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        #endregion Model

    }
}
