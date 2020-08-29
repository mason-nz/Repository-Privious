using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    [Serializable]
    public class QueryPublishAuditInfo
    {
        public QueryPublishAuditInfo()
        {                 
            _MediaType = Constant.INT_INVALID_VALUE;
            _PublishID = Constant.INT_INVALID_VALUE;
            _OptType = Constant.INT_INVALID_VALUE;

            _begincreatetime = Constant.STRING_INVALID_VALUE;
            _endcreatetime = Constant.STRING_INVALID_VALUE;
        }
        private string _begincreatetime;
        private string _endcreatetime;
        public string BeginCreateTime
        {
            set { _begincreatetime = value; }
            get { return _begincreatetime; }
        }

        public string EndCreateTime
        {
            set { _endcreatetime = value; }
            get { return _endcreatetime; }
        }
        
        private int _MediaType;

        public int MediaType
        {
            get { return _MediaType; }
            set { _MediaType = value; }
        }

        private int _PublishID;

        public int PublishID
        {
            get { return _PublishID; }
            set { _PublishID = value; }
        }

        private int _OptType;

        public int OptType
        {
            get { return _OptType; }
            set { _OptType = value; }
        }
       
    }
}
