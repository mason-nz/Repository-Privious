using System;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类LeadsTask 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-05-19 11:30:49 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    [Serializable]
    public class LeadsTask
    {
        public LeadsTask()
        {
            _taskid = Constant.STRING_INVALID_VALUE;
            _projectid = Constant.INT_INVALID_VALUE;
            _demandid = Constant.STRING_INVALID_VALUE;
            //_relationid没有默认值
            _username = Constant.STRING_INVALID_VALUE;
            _tel = Constant.STRING_INVALID_VALUE;
            _sex = Constant.INT_INVALID_VALUE;
            _provinceid = Constant.INT_INVALID_VALUE;
            _cityid = Constant.INT_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _ordercarmasterid = Constant.INT_INVALID_VALUE;
            _ordercarmaster = Constant.STRING_INVALID_VALUE;
            _ordercarserialid = Constant.INT_INVALID_VALUE;
            _ordercarserial = Constant.STRING_INVALID_VALUE;
            _ordercarid = Constant.INT_INVALID_VALUE;
            _ordercar = Constant.STRING_INVALID_VALUE;
            _dealerid = Constant.STRING_INVALID_VALUE;
            _dealername = Constant.STRING_INVALID_VALUE;
            _ordercreatetime = Constant.DATE_INVALID_VALUE;
            _dcarmasterid = Constant.INT_INVALID_VALUE;
            _dcarmaster = Constant.STRING_INVALID_VALUE;
            _dcarserialid = Constant.INT_INVALID_VALUE;
            _dcarserial = Constant.STRING_INVALID_VALUE;
            _dcarid = Constant.INT_INVALID_VALUE;
            _dcarname = Constant.STRING_INVALID_VALUE;
            _issuccess = Constant.INT_INVALID_VALUE;
            _failreason = Constant.INT_INVALID_VALUE;
            _remark = Constant.STRING_INVALID_VALUE;
            _assignuserid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _lastupdatetime = Constant.DATE_INVALID_VALUE;
            _lastupdateuserid = Constant.INT_INVALID_VALUE;
            _lastdealtime = Constant.DATE_INVALID_VALUE;
            _demandversion = Constant.INT_INVALID_VALUE;
            _isjt = Constant.INT_INVALID_VALUE;
            _pbuycartime = Constant.INT_INVALID_VALUE;
            _thinkcar = Constant.STRING_INVALID_VALUE;
            _ordernum = Constant.INT_INVALID_VALUE;
            _TargetProvinceID = Constant.INT_INVALID_VALUE;
            _TargetCityID = Constant.INT_INVALID_VALUE;
 
            _isboughtcar = Constant.INT_INVALID_VALUE;
            _boughtcarmasterid = Constant.INT_INVALID_VALUE;
            _boughtcarmaster = Constant.STRING_INVALID_VALUE;
            _boughtcarserialid = Constant.INT_INVALID_VALUE;
            _boughtcarserial = Constant.STRING_INVALID_VALUE; 
            _boughtcaryearmonth = Constant.STRING_INVALID_VALUE;
            _boughtcardealerid = Constant.STRING_INVALID_VALUE;
            _boughtcardealername = Constant.STRING_INVALID_VALUE;
            _hasbuycarplan = Constant.INT_INVALID_VALUE;
            _intentioncarmasterid = Constant.INT_INVALID_VALUE;
            _intentioncarmaster = Constant.STRING_INVALID_VALUE;
            _intentioncarserialid = Constant.INT_INVALID_VALUE;
            _intentioncarserial = Constant.STRING_INVALID_VALUE;

            _isattention = Constant.INT_INVALID_VALUE; ;
            _iscontacteddealer = Constant.INT_INVALID_VALUE; ;
            _issatisfiedservice = Constant.INT_INVALID_VALUE; ;
            _contactedwhichdealer = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private string _taskid;
        private int? _TargetProvinceID;
        private int? _TargetCityID;
        private int _projectid;
        private string _demandid;
        private Guid _relationid;
        private string _username;
        private string _tel;
        private int? _sex;
        private int? _provinceid;
        private int? _cityid;
        private int? _status;
        private int? _ordercarmasterid;
        private string _ordercarmaster;
        private int? _ordercarserialid;
        private string _ordercarserial;
        private int? _ordercarid;
        private string _ordercar;
        private string _dealerid;
        private string _dealername;
        private DateTime? _ordercreatetime;
        private int? _dcarmasterid;
        private string _dcarmaster;
        private int? _dcarserialid;
        private string _dcarserial;
        private int? _dcarid;
        private string _dcarname;
        private int? _issuccess;
        private int? _failreason;
        private string _remark;
        private int? _assignuserid;
        private DateTime? _createtime;
        private int? _createuserid;
        private DateTime? _lastupdatetime;
        private int? _lastupdateuserid;
        private DateTime? _lastdealtime;
        private int? _demandversion;

        private int? _isjt;
        private int? _pbuycartime;
        private string _thinkcar;
        private int? _ordernum;


        private int? _isboughtcar;
        private int? _boughtcarmasterid;
        private string _boughtcarmaster;
        private int? _boughtcarserialid;
        private string _boughtcarserial;
        private string _boughtcaryearmonth;
        private string _boughtcardealerid;
        private string _boughtcardealername;
        private int? _hasbuycarplan;
        private int? _intentioncarmasterid;
        private string _intentioncarmaster;
        private int? _intentioncarserialid;
        private string _intentioncarserial;

        private int? _notEstablishReason;
        private int? _notSuccessReason;

        private int? _isattention;
        private int? _iscontacteddealer;
        private int? _issatisfiedservice;
        private string _contactedwhichdealer;
         
        public int? TargetProvinceID
        {
            set { _TargetProvinceID = value; }
            get { return _TargetProvinceID; }
        }
        public int? TargetCityID
        {
            set { _TargetCityID = value; }
            get { return _TargetCityID; }
        }
        public int? OrderNum
        {
            set { _ordernum = value; }
            get { return _ordernum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsJT
        {
            set { _isjt = value; }
            get { return _isjt; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? PBuyCarTime
        {
            set { _pbuycartime = value; }
            get { return _pbuycartime; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ThinkCar
        {
            set { _thinkcar = value; }
            get { return _thinkcar; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TaskID
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ProjectID
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DemandID
        {
            set { _demandid = value; }
            get { return _demandid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid RelationID
        {
            set { _relationid = value; }
            get { return _relationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProvinceID
        {
            set { _provinceid = value; }
            get { return _provinceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
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
        public int? OrderCarMasterID
        {
            set { _ordercarmasterid = value; }
            get { return _ordercarmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCarMaster
        {
            set { _ordercarmaster = value; }
            get { return _ordercarmaster; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderCarSerialID
        {
            set { _ordercarserialid = value; }
            get { return _ordercarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCarSerial
        {
            set { _ordercarserial = value; }
            get { return _ordercarserial; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderCarID
        {
            set { _ordercarid = value; }
            get { return _ordercarid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrderCar
        {
            set { _ordercar = value; }
            get { return _ordercar; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DealerID
        {
            set { _dealerid = value; }
            get { return _dealerid; }
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
        public DateTime? OrderCreateTime
        {
            set { _ordercreatetime = value; }
            get { return _ordercreatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DCarMasterID
        {
            set { _dcarmasterid = value; }
            get { return _dcarmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DCarMaster
        {
            set { _dcarmaster = value; }
            get { return _dcarmaster; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DCarSerialID
        {
            set { _dcarserialid = value; }
            get { return _dcarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DCarSerial
        {
            set { _dcarserial = value; }
            get { return _dcarserial; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DCarID
        {
            set { _dcarid = value; }
            get { return _dcarid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DCarName
        {
            set { _dcarname = value; }
            get { return _dcarname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsSuccess
        {
            set { _issuccess = value; }
            get { return _issuccess; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FailReason
        {
            set { _failreason = value; }
            get { return _failreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AssignUserID
        {
            set { _assignuserid = value; }
            get { return _assignuserid; }
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
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LastUpdateUserID
        {
            set { _lastupdateuserid = value; }
            get { return _lastupdateuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastDealTime
        {
            set { _lastdealtime = value; }
            get { return _lastdealtime; }
        }

        public int? DemandVersion
        {
            set { _demandversion = value; }
            get { return _demandversion; }
        }
        //*********************************
        /// <summary>
        /// 
        /// </summary>
        public int? IsBoughtCar
        {
            set { _isboughtcar = value; }
            get { return _isboughtcar; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BoughtCarMasterID
        {
            set { _boughtcarmasterid = value; }
            get { return _boughtcarmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoughtCarMaster
        {
            set { _boughtcarmaster = value; }
            get { return _boughtcarmaster; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BoughtCarSerialID
        {
            set { _boughtcarserialid = value; }
            get { return _boughtcarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoughtCarSerial
        {
            set { _boughtcarserial = value; }
            get { return _boughtcarserial; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoughtCarYearMonth
        {
            set { _boughtcaryearmonth = value; }
            get { return _boughtcaryearmonth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoughtCarDealerID
        {
            set { _boughtcardealerid = value; }
            get { return _boughtcardealerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BoughtCarDealerName
        {
            set { _boughtcardealername = value; }
            get { return _boughtcardealername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HasBuyCarPlan
        {
            set { _hasbuycarplan = value; }
            get { return _hasbuycarplan; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IntentionCarMasterID
        {
            set { _intentioncarmasterid = value; }
            get { return _intentioncarmasterid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntentionCarMaster
        {
            set { _intentioncarmaster = value; }
            get { return _intentioncarmaster; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IntentionCarSerialID
        {
            set { _intentioncarserialid = value; }
            get { return _intentioncarserialid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IntentionCarSerial
        {
            set { _intentioncarserial = value; }
            get { return _intentioncarserial; }
        }

        public int? NotEstablishReason
        {
            set { _notEstablishReason = value; }
            get { return _notEstablishReason; }
        }

        public int? NotSuccessReason
        {
            set { _notSuccessReason = value; }
            get { return _notSuccessReason; }
        }
        /// <summary>
        /// 是否关注该品牌
        /// </summary>
        public int? IsAttention
        {
            set { _isattention = value; }
            get { return _isattention; }
        }
        /// <summary>
        /// 是否有经销商联系
        /// </summary>
        public int? IsContactedDealer
        {
            set { _iscontacteddealer = value; }
            get { return _iscontacteddealer; }
        }
        /// <summary>
        /// 经销商服务是否满意
        /// </summary>
        public int? IsSatisfiedService
        {
            set { _issatisfiedservice = value; }
            get { return _issatisfiedservice; }
        }
        /// <summary>
        /// 哪家经销商联系
        /// </summary>
        public string ContactedWhichDealer
        {
            set { _contactedwhichdealer = value; }
            get { return _contactedwhichdealer; }
        } 
         
        #endregion Model

    }
}

