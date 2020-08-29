using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public partial class PhoneNumDataDict
    {
        public PhoneNumDataDict()
        { }
        #region Model
        private int _recid;
        private string _phoneprefix;
        private string _districtnum;
        private string _areaname;
        private int? _areaid;
        private int? _areapid;
        private int? _arealevel;
        private int? _status = 0;
        private DateTime? _createtime = DateTime.Now;
        private int? _proviceid;
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
        public string PhonePrefix
        {
            set { _phoneprefix = value; }
            get { return _phoneprefix; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DistrictNum
        {
            set { _districtnum = value; }
            get { return _districtnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AreaName
        {
            set { _areaname = value; }
            get { return _areaname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AreaID
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AreaPid
        {
            set { _areapid = value; }
            get { return _areapid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AreaLevel
        {
            set { _arealevel = value; }
            get { return _arealevel; }
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
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProviceID
        {
            set { _proviceid = value; }
            get { return _proviceid; }
        }
        #endregion Model

    }
}
