using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class CC_MagazineReturn
    {
        public CC_MagazineReturn()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _contactid = Constant.INT_INVALID_VALUE;
            _dmsmemberid = Constant.STRING_INVALID_VALUE;
            _title = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _cmdid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private string _custid;
        private int? _contactid;
        private string _dmsmemberid;
        private string _title;
        private int? _status;
        private int? _cmdid;
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
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
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
        public string DMSMemberID
        {
            set { _dmsmemberid = value; }
            get { return _dmsmemberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
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
        public int? CMDID
        {
            set { _cmdid = value; }
            get { return _cmdid; }
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
