using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class BusinessGroup
    {
        public BusinessGroup()
        {
            _bgid = Constant.INT_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _cdid = Constant.INT_INVALID_VALUE;
            _regionid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            BusinessType = null;
        }

        private int _bgid;
        private string _name;
        private int _status;
        private int _cdid;
        private int _regionid;
        private DateTime _createtime;

        public int BGID
        {
            set { _bgid = value; }
            get { return _bgid; }
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }

        public int CDID
        {
            set { _cdid = value; }
            get { return _cdid; }
        }

        public int RegionID
        {
            set { _regionid = value; }
            get { return _regionid; }
        }

        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public int? BusinessType { get; set; }

        public string LineIDs { get; set; }
    }
}
