using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public class ProjectInfo
    {
        string _ProjectID = "";
        public string ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = HttpUtility.UrlDecode(value); }
        }

        string _txtProjectName = "";
        public string txtProjectName
        {
            get { return _txtProjectName.Replace(" ","").Replace("　","") ; }
            set { _txtProjectName = HttpUtility.UrlDecode(value); }
        }

        string _sltUserGroup = "";
        public string sltUserGroup
        {
            get { return _sltUserGroup; }
            set { _sltUserGroup = HttpUtility.UrlDecode(value); }
        }

        string _sltSurveyCategory = "";
        public string sltSurveyCategory
        {
            get { return _sltSurveyCategory; }
            set { _sltSurveyCategory = HttpUtility.UrlDecode(value); }
        }

        string _txtDescription = "";
        public string txtDescription
        {
            get { return _txtDescription; }
            set { _txtDescription = HttpUtility.UrlDecode(value); }
        }

        string _Source = "";
        public string Source
        {
            get { return _Source; }
            set { _Source = HttpUtility.UrlDecode(value); }
        }

        string _hidExportSelectIDs = "";
        public string hidExportSelectIDs
        {
            get { return _hidExportSelectIDs; }
            set { _hidExportSelectIDs = HttpUtility.UrlDecode(value); }
        }

        string _hidexportaddids = "";
        public string hidExportAddIDs
        {
            get { return _hidexportaddids; }
            set { _hidexportaddids = HttpUtility.UrlDecode(value); }
        }

        string _TTCode = "";
        public string TTCode
        {
            get { return _TTCode; }
            set { _TTCode = HttpUtility.UrlDecode(value); }
        }

        public int IsBlacklistCheck { get; set; }

        public int BlackListCheckType { get; set; }

        public SurveryInfo[] _surveyList;
        public SurveryInfo[] SurveyList
        {
            get { return _surveyList; }
            set { _surveyList = value; }
        }
        string _hidcrmaddids = "";
        public string hidCrmAddIDs
        {
            get { return _hidcrmaddids; }
            set { _hidcrmaddids = HttpUtility.UrlDecode(value); }
        }
    }

    public class SurveryInfo
    {
        string _ProjectID = "";
        public string ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = HttpUtility.UrlDecode(value); }
        }

        string _hdnSIID = "";
        public string hdnSIID
        {
            get { return _hdnSIID; }
            set { _hdnSIID = HttpUtility.UrlDecode(value); }
        }

        string _txtSurveyName = "";
        public string txtSurveyName
        {
            get { return _txtSurveyName; }
            set { _txtSurveyName = HttpUtility.UrlDecode(value); }
        }

        string _beginTime = "";
        public string beginTime
        {
            get { return _beginTime; }
            set { _beginTime = HttpUtility.UrlDecode(value); }
        }

        string _endTime = "";
        public string endTime
        {
            get { return _endTime; }
            set { _endTime = HttpUtility.UrlDecode(value); }
        }



    }

}