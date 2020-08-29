using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryKLFavorites
    {
        public QueryKLFavorites()
        {
            _id = Constant.INT_INVALID_VALUE;
            _userId = Constant.INT_INVALID_VALUE;
            _kLRefId = Constant.INT_INVALID_VALUE;
            _type = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
        }
        #region Model
        private int _id;
        private int _userId;
        private int _kLRefId;
        private int _type;
        private DateTime _createtime;

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userId = value; }
            get { return _userId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int KLRefId
        {
            set { _kLRefId = value; }
            get { return _kLRefId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        #endregion Model
    }
}
