using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类ConsultNewCar 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class ConsultNewCar
    {
        public ConsultNewCar()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _carbrandid = Constant.INT_INVALID_VALUE;
            _carserialid = Constant.INT_INVALID_VALUE;
            _carname = Constant.STRING_INVALID_VALUE;
            _dealername = Constant.STRING_INVALID_VALUE;
            _buycarbudget = Constant.STRING_INVALID_VALUE;
            _activity = Constant.STRING_INVALID_VALUE;
            _buycartime = Constant.INT_INVALID_VALUE;
            _buyordisplace = Constant.INT_INVALID_VALUE;
            _callrecord = Constant.STRING_INVALID_VALUE;
            _accepttel = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _dealercode = Constant.STRING_INVALID_VALUE;
            _carid = Constant.INT_INVALID_VALUE;
        }
        #region Model
        private string _dealercode;

        private int _recid;
        private string _custid;
        private int? _carbrandid;
        private int? _carserialid;
        private string _carname;
        private string _dealername;
        private string _buycarbudget;
        private string _activity;
        private int? _buycartime;
        private int? _buyordisplace;
        private string _callrecord;
        private int? _accepttel;
        private DateTime? _createtime;
        private int? _createuserid;
        private int _carid;

        /// <summary>
        /// 车款Id
        /// </summary>
        public int CarID
        {
            set { _carid = value; }
            get { return _carid; }
        }

        public string DealerCode
        {
            set { _dealercode = value; }
            get { return _dealercode; }
        }


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
        public int? CarBrandId
        {
            set { _carbrandid = value; }
            get { return _carbrandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CarSerialId
        {
            set { _carserialid = value; }
            get { return _carserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarName
        {
            set { _carname = value; }
            get { return _carname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DealerName
        {
            set { _dealername = value; }
            get { return _dealername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyCarBudget
        {
            set { _buycarbudget = value; }
            get { return _buycarbudget; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Activity
        {
            set { _activity = value; }
            get { return _activity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BuyCarTime
        {
            set { _buycartime = value; }
            get { return _buycartime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BuyOrDisplace
        {
            set { _buyordisplace = value; }
            get { return _buyordisplace; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CallRecord
        {
            set { _callrecord = value; }
            get { return _callrecord; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AcceptTel
        {
            set { _accepttel = value; }
            get { return _accepttel; }
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

