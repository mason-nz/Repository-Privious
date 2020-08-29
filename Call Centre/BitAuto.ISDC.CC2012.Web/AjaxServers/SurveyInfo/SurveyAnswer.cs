using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo
{
    public class SurveyAnswerRoot
    {
        private SurveyAnswer[] _dataroot;

        public SurveyAnswer[] DataRoot
        {
            get { return _dataroot; }
            set { _dataroot = value; }
        }

    }

    public class SurveyAnswer
    {
        private string _sqid;

        public string SQID
        {
            get { return _sqid; }
            set { _sqid = HttpUtility.UrlDecode(value); }
        }
        private string _smrtid;

        public string SMRTID
        {
            get { return _smrtid; }
            set { _smrtid = HttpUtility.UrlDecode(value); }
        }
        private string _smctid;

        public string SMCTID
        {
            get { return _smctid; }
            set { _smctid = HttpUtility.UrlDecode(value); }
        }
        private string _soid;

        public string SOID
        {
            get { return _soid; }
            set { _soid = HttpUtility.UrlDecode(value); }
        }
        private string _answercontent;

        public string AnswerContent
        {
            get { return _answercontent; }
            set { _answercontent = HttpUtility.UrlDecode(value); }
        }
    }
}