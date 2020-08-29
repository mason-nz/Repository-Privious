using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage
{
    public class ScoreTableInfo
    {
        private string _rtid;
        public string RTID
        {
            get { return _rtid; }
            set { _rtid = HttpUtility.UrlDecode(value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = HttpUtility.UrlDecode(value); }
        }

        private string _scoretype;
        public string ScoreType
        {
            get { return _scoretype; }
            set { _scoretype = HttpUtility.UrlDecode(value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = HttpUtility.UrlDecode(value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = HttpUtility.UrlDecode(value); }
        }

        private string _appraisal;
        public string Appraisal
        {
            get { return _appraisal; }
            set { _appraisal = HttpUtility.UrlDecode(value); }
        }

        private string _deaditemnum;
        public string DeadItemNum
        {
            get { return _deaditemnum; }
            set { _deaditemnum = HttpUtility.UrlDecode(value); }
        }

        private string _nodeaditemnum;
        public string NoDeadItemNum
        {
            get { return _nodeaditemnum; }
            set { _nodeaditemnum = HttpUtility.UrlDecode(value); }
        }

        private Catage[] _catage;
        public Catage[] Catage
        {
            get { return _catage; }
            set { _catage = value; }
        }

        private Dead[] _dead;
        public Dead[] Dead
        {
            get { return _dead; }
            set { _dead = value; }
        }

        private string _regionid;
        public string RegionID
        {
            get { return _regionid; }
            set { _regionid = HttpUtility.UrlDecode(value); }
        }

    }

    public class Catage
    {
        private string _cid;
        public string CID
        {
            get { return _cid; }
            set { _cid = HttpUtility.UrlDecode(value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = HttpUtility.UrlDecode(value); }
        }

        private string _score;
        public string Score
        {
            get { return _score; }
            set { _score = HttpUtility.UrlDecode(value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = HttpUtility.UrlDecode(value); }
        }

        private Item[] _item;
        public Item[] Item
        {
            get { return _item; }
            set { _item = value; }
        }


    }

    public class Item
    {
        private string _iid;
        public string IID
        {
            get { return _iid; }
            set { _iid = HttpUtility.UrlDecode(value); }
        }

        private string _itemname;
        public string ItemName
        {
            get { return _itemname; }
            set { _itemname = HttpUtility.UrlDecode(value); }
        }

        private string _score;
        public string Score
        {
            get { return _score; }
            set { _score = HttpUtility.UrlDecode(value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = HttpUtility.UrlDecode(value); }
        }

        private string _cid;
        public string CID
        {
            get { return _cid; }
            set { _cid = HttpUtility.UrlDecode(value); }
        }

        private Standard[] _standard;
        public Standard[] Standard
        {
            get { return _standard; }
            set { _standard = value; }
        }

    }

    public class Standard
    {
        private string _sid;
        public string SID
        {
            get { return _sid; }
            set { _sid = HttpUtility.UrlDecode(value); }
        }

        private string _iid;
        public string IID
        {
            get { return _iid; }
            set { _iid = HttpUtility.UrlDecode(value); }
        }

        private string _cid;
        public string CID
        {
            get { return _cid; }
            set { _cid = HttpUtility.UrlDecode(value); }
        }

        private string _sname;
        public string SName
        {
            get { return _sname; }
            set { _sname = HttpUtility.UrlDecode(value); }
        }
        private string _sexplanation;
        public string SExplanation
        {
            get { return _sexplanation; }
            set { _sexplanation = HttpUtility.UrlDecode(value); }
        }
        private string _skilllevel;
        public string SkillLevel
        {
            get { return _skilllevel; }
            set { _skilllevel = HttpUtility.UrlDecode(value); }
        }
        private string _score;
        public string Score
        {
            get { return _score; }
            set { _score = HttpUtility.UrlDecode(value); }
        }

        private string _isisdead;
        public string IsIsDead
        {
            get { return _isisdead; }
            set { _isisdead = HttpUtility.UrlDecode(value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = HttpUtility.UrlDecode(value); }
        }

        private Marking[] _marking;
        public Marking[] Marking
        {
            get { return _marking; }
            set { _marking = value; }
        }

    }

    public class Marking
    {
        private string _sid;
        public string SID
        {
            get { return _sid; }
            set { _sid = HttpUtility.UrlDecode(value); }
        }

        private string _iid;
        public string IID
        {
            get { return _iid; }
            set { _iid = HttpUtility.UrlDecode(value); }
        }

        private string _cid;
        public string CID
        {
            get { return _cid; }
            set { _cid = HttpUtility.UrlDecode(value); }
        }

        private string _mid;
        public string MID
        {
            get { return _mid; }
            set { _mid = HttpUtility.UrlDecode(value); }
        }

        private string _mname;
        public string MName
        {
            get { return _mname; }
            set { _mname = HttpUtility.UrlDecode(value); }
        }

        private string _score;
        public string Score
        {
            get { return _score; }
            set { _score = HttpUtility.UrlDecode(value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = HttpUtility.UrlDecode(value); }
        }

    }

    public class Dead
    {
        private string _did;
        public string DID
        {
            get { return _did; }
            set { _did = HttpUtility.UrlDecode(value); }
        }

        private string _dname;
        public string DName
        {
            get { return _dname; }
            set { _dname = HttpUtility.UrlDecode(value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = HttpUtility.UrlDecode(value); }
        }
    }
}