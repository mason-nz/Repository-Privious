using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 实体类IVRSatisfaction 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <author>
    /// lihf
    /// </author>
    /// <history>
    /// 2013-07-16 11:08:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class IVRSatisfaction
    {
        public IVRSatisfaction()
        {
            _callid = Constant.INT_INVALID_VALUE;
            _CallRecordID = Constant.INT_INVALID_VALUE;
            _Score = Constant.INT_INVALID_VALUE;
            _CreateTime = Constant.DATE_INVALID_VALUE;
        }

        private Int64? _callid;
        public Int64? CallID
        {
            set { _callid = value; }
            get { return _callid; }
        }

        private Int64 _CallRecordID;
        public Int64 CallRecordID
        {
            set{_CallRecordID=value;}
            get { return _CallRecordID; }
        }

        private int _Score;
        public int Score
        {
            set { _Score = value; }
            get { return _Score; }
        }

        private DateTime _CreateTime;
        public DateTime CreateTime
        {
            set { _CreateTime = value; }
            get { return _CreateTime; }
        }
    }
}
