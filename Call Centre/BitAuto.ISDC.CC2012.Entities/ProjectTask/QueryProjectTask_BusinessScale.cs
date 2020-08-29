using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryProjectTask_BusinessScale
    {
        public QueryProjectTask_BusinessScale()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _ptid = Constant.STRING_INVALID_VALUE;
            _originalrecid = Constant.INT_INVALID_VALUE;
            _monthstock = Constant.INT_INVALID_VALUE;
            _monthsales = Constant.INT_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _modifyuserid = Constant.INT_INVALID_VALUE;
            _modifytime = Constant.DATE_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;

        }
        #region Model
        private int _recid;
        private string _ptid;
        private int? _originalrecid;
        private int? _monthstock;
        private int? _monthsales;
        private int? _createuserid;
        private DateTime? _createtime;
        private int? _modifyuserid;
        private DateTime? _modifytime;
        private int? _status;
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
        public string PTID
        {
            set { _ptid = value; }
            get { return _ptid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OriginalRecID
        {
            set { _originalrecid = value; }
            get { return _originalrecid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? MonthStock
        {
            set { _monthstock = value; }
            get { return _monthstock; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? MonthSales
        {
            set { _monthsales = value; }
            get { return _monthsales; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
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
        public int? ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model
    }
}
