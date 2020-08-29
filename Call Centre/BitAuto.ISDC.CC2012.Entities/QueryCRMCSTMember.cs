using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;
namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryCRMCSTMember
    {
        public string CSTMemberCode
        {
            set;
            get;
        }
        public string CSTMemberName
        {
            set;
            get;
        }
        public string ProvinceID
        {
            set;
            get;
        }
        public string CityID
        {
            set;
            get;
        }
        public string CountyID
        {
            set;
            get;
        }
        public string TFCustPids
        {
            set;
            get;
        }
        public string MemberTypeIDs
        {
            set;
            get;
        }
        public string LoggingAmountStarts
        {
            set;
            get;
        }
        public string LoggingAmountEnds
        {
            set;
            get;
        }
        public string RemainAmountStarts
        {
            set;
            get;
        }
        public string RemainAmountEnds
        {
            set;
            get;
        }

        public string AvailabilityTimeStarts
        {
            set;
            get;
        }
        public string AvailabilityTimeEnds
        {
            set;
            get;
        }

        public string UserdAmountStarts
        {
            set;
            get;
        }
        public string UserdAmountEnds
        {
            set;
            get;
        }
        public string MemberSyncStatus
        {
            set;
            get;
        }
        //                //所属交易市场
        //                TFCustPids:encodeURIComponent(tfCustPids),
        //                //会员类型
        //                MemberTypeIDs: escapeStr(memberTypeIDs),
        //                //累计充值
        //                LoggingAmountStarts:encodeURIComponent(loggingAmountStart),
        //                LoggingAmountEnds:encodeURIComponent(loggingAmountEnd),

        //                //车商币余额
        //                RemainAmountStarts:encodeURIComponent(RemainAmountStart),
        //                RemainAmountEnds:encodeURIComponent(RemainAmountEnd),

        //                //车商币有效期
        //                AvailabilityTimeStarts:encodeURIComponent(AvailabilityTimeStart),
        //                AvailabilityTimeEnds:encodeURIComponent(AvailabilityTimeEnd),

        //                //累计消费车商币
        //                UserdAmountStarts:encodeURIComponent(UserdAmountStart),
        //                UserdAmountEnds:encodeURIComponent(UserdAmountEnd),
    }
}
