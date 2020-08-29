using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoringManage
{
    public class QS_ResultData
    {
        public string _qs_rid;
        public string QS_RID
        {
            set { _qs_rid = HttpUtility.UrlDecode(value); }
            get { return _qs_rid; }
        }
        public string _callid;
        public string CallID
        {
            set { _callid = HttpUtility.UrlDecode(value); }
            get { return _callid; }
        }
        public string _csid;
        public string CSID
        {
            set { _csid = HttpUtility.UrlDecode(value); }
            get { return _csid; }
        }
        public string _qs_rtid;
        public string QS_RTID
        {
            set { _qs_rtid = HttpUtility.UrlDecode(value); }
            get { return _qs_rtid; }
        }

        public string _deadnum;
        /// <summary>
        /// 致命项数
        /// </summary>
        public string DeadNum
        {
            set { _deadnum = HttpUtility.UrlDecode(value); }
            get { return _deadnum; }
        }
        public string _nodeadnum;
        /// <summary>
        /// 非致命项数
        /// </summary>
        public string NoDeadNum
        {
            set { _nodeadnum = HttpUtility.UrlDecode(value); }
            get { return _nodeadnum; }
        }

        public string _scoretype;
        public string ScoreType
        {
            set { _scoretype = HttpUtility.UrlDecode(value); }
            get { return _scoretype; }
        }
        public string _qualityappraisal;
        public string QualityAppraisal
        {
            set { _qualityappraisal = HttpUtility.UrlDecode(value); }
            get { return _qualityappraisal; }
        }
        public QS_ResultDetailData[] QS_ResultDetailList;
    }
    public class QS_ResultDetailData
    {
        public string _qs_rdid;
        public string QS_RDID
        {
            get { return _qs_rdid; }
            set { _qs_rdid = HttpUtility.UrlDecode(value); }
        }

        public string _scoretype;
        public string ScoreType
        {
            get { return _scoretype; }
            set { _scoretype = HttpUtility.UrlDecode(value); }
        }

        public string _qs_rtid;
        public string QS_RTID
        {
            get { return _qs_rtid; }
            set { _qs_rtid = HttpUtility.UrlDecode(value); }
        }

        public string _qs_rid;
        public string QS_RID
        {
            get { return _qs_rid; }
            set { _qs_rid = HttpUtility.UrlDecode(value); }
        }

        public string _qs_cid;
        public string QS_CID
        {
            get { return _qs_cid; }
            set { _qs_cid = HttpUtility.UrlDecode(value); }
        }

        public string _qs_iid;
        public string QS_IID
        {
            get { return _qs_iid; }
            set { _qs_iid = HttpUtility.UrlDecode(value); }
        }

        public string _qs_sid;
        public string QS_SID
        {
            get { return _qs_sid; }
            set { _qs_sid = HttpUtility.UrlDecode(value); }
        }

        public string _qs_mid;
        public string QS_MID
        {
            get { return _qs_mid; }
            set { _qs_mid = HttpUtility.UrlDecode(value); }
        }
        public string _qs_mid_end;
        public string QS_MID_End
        {
            get { return _qs_mid_end; }
            set { _qs_mid_end = HttpUtility.UrlDecode(value); }
        }
        public string _type;
        public string Type
        {
            get { return _type; }
            set { _type = HttpUtility.UrlDecode(value); }
        }
        public string _scoredeadid;
        public string ScoreDeadID
        {
            get { return _scoredeadid; }
            set { _scoredeadid = HttpUtility.UrlDecode(value); }
        }
        public string _scoredeadid_end;
        public string ScoreDeadID_End
        {
            get { return _scoredeadid_end; }
            set { _scoredeadid_end = HttpUtility.UrlDecode(value); }
        }
        public string _remark;
        public string Remark
        {
            set { _remark = HttpUtility.UrlDecode(value); }
            get { return _remark; }
        }
    }
}