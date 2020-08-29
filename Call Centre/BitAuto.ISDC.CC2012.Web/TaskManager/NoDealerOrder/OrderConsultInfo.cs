using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder
{

    public class NewCarConsultInfo
    {
        public string _carmasterid;
        public string CarMasterID
        {
            get { return _carmasterid; }
            set { _carmasterid = HttpUtility.UrlDecode(value); }
        }

        public string _carserialid;
        public string CarSerialID
        {
            get { return _carserialid; }
            set { _carserialid = HttpUtility.UrlDecode(value); }
        }

        public string _cartypeid;
        public string CarTypeID
        {
            get { return _cartypeid; }
            set { _cartypeid = HttpUtility.UrlDecode(value); }
        }

        public string _carcolor;
        public string CarColor
        {
            get { return _carcolor; }
            set { _carcolor = HttpUtility.UrlDecode(value); }
        }

        public string _dmsmembercode;
        public string DMSMemberCode
        {
            get { return _dmsmembercode; }
            set { _dmsmembercode = HttpUtility.UrlDecode(value); }
        }

        public string _dmsmembername;
        public string DMSMemberName
        {
            get { return _dmsmembername; }
            set { _dmsmembername = HttpUtility.UrlDecode(value); }
        }

        public string _callrecord;
        public string CallRecord
        {
            get { return _callrecord; }
            set { _callrecord = HttpUtility.UrlDecode(value); }
        }

        public string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = HttpUtility.UrlDecode(value); }
        }
    }

    public class ReplaceCarConsultInfo
    {
        public string _carmasterid;
        public string CarMasterID
        {
            get { return _carmasterid; }
            set { _carmasterid = HttpUtility.UrlDecode(value); }
        }

        public string _carserialid;
        public string CarSerialID
        {
            get { return _carserialid; }
            set { _carserialid = HttpUtility.UrlDecode(value); }
        }

        public string _cartypeid;
        public string CarTypeID
        {
            get { return _cartypeid; }
            set { _cartypeid = HttpUtility.UrlDecode(value); }
        }

        public string _carcolor;
        public string CarColor
        {
            get { return _carcolor; }
            set { _carcolor = HttpUtility.UrlDecode(value); }
        }

        public string _dmsmembercode;
        public string DMSMemberCode
        {
            get { return _dmsmembercode; }
            set { _dmsmembercode = HttpUtility.UrlDecode(value); }
        }

        public string _dmsmembername;
        public string DMSMemberName
        {
            get { return _dmsmembername; }
            set { _dmsmembername = HttpUtility.UrlDecode(value); }
        }

        public string _repcarmasterid;
        public string RepCarMasterID
        {
            get { return _repcarmasterid; }
            set { _repcarmasterid = HttpUtility.UrlDecode(value); }
        }

        public string _repcarserialid;
        public string RepCarSerialID
        {
            get { return _repcarserialid; }
            set { _repcarserialid = HttpUtility.UrlDecode(value); }
        }

        public string _repcartypeid;
        public string RepCarTypeId
        {
            get { return _repcartypeid; }
            set { _repcartypeid = HttpUtility.UrlDecode(value); }
        }

        public string _replacementccarcolor;
        public string ReplacementcCarColor
        {
            get { return _replacementccarcolor; }
            set { _replacementccarcolor = HttpUtility.UrlDecode(value); }
        }

        public string _replacementcarbuyyear;
        public string ReplacementCarBuyYear
        {
            get { return _replacementcarbuyyear; }
            set { _replacementcarbuyyear = HttpUtility.UrlDecode(value); }
        }

        public string _replacementcarbuymonth;
        public string ReplacementCarBuyMonth
        {
            get { return _replacementcarbuymonth; }
            set { _replacementcarbuymonth = HttpUtility.UrlDecode(value); }
        }

        public string _repcarprovinceid;
        public string RepCarProvinceID
        {
            get { return _repcarprovinceid; }
            set { _repcarprovinceid = HttpUtility.UrlDecode(value); }
        }

        public string _repcarcityid;
        public string RepCarCityID
        {
            get { return _repcarcityid; }
            set { _repcarcityid = HttpUtility.UrlDecode(value); }
        }

        public string _repcarcountyid;
        public string RepCarCountyID
        {
            get { return _repcarcountyid; }
            set { _repcarcountyid = HttpUtility.UrlDecode(value); }
        }

        public string _replacementcarusedmiles;
        public string ReplacementCarUsedMiles
        {
            get { return _replacementcarusedmiles; }
            set { _replacementcarusedmiles = HttpUtility.UrlDecode(value); }
        }

        public string _sellprice;
        public string SellPrice
        {
            get { return _sellprice; }
            set { _sellprice = HttpUtility.UrlDecode(value); }
        }

        public string _callrecord;
        public string CallRecord
        {
            get { return _callrecord; }
            set { _callrecord = HttpUtility.UrlDecode(value); }
        }

        public string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = HttpUtility.UrlDecode(value); }
        }
    }
}