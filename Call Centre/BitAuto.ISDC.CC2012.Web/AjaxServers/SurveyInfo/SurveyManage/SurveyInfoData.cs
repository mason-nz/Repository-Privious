using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage
{
    public class SurveyInfoData
    {
        public string _siid;
        public string siid
        {
            get { return _siid; }
            set { _siid = HttpUtility.UrlDecode(value); }
        }
        public string _name;
        public string name
        {
            get { return _name; }
            set { _name = HttpUtility.UrlDecode(value); }
        }

        public string _bgid;
        public string bgid
        {
            get { return _bgid; }
            set { _bgid = HttpUtility.UrlDecode(value); }
        }
        public string _scid;
        public string scid
        {
            get { return _scid; }
            set { _scid = HttpUtility.UrlDecode(value); }
        }
        public string _desc;
        public string desc
        {
            get { return _desc; }
            set { _desc = HttpUtility.UrlDecode(value); }
        }

        public string _ismustanswer;
        public string IsMustAnswer
        {
            get { return _ismustanswer; }
            set { _ismustanswer = HttpUtility.UrlDecode(value); }
        }

        public string _isstatbyscore;
        public string IsStatByScore
        {
            get { return _isstatbyscore; }
            set { _isstatbyscore = HttpUtility.UrlDecode(value); }
        }


        public SurveyQuestionInfoData[] _questList;
        public SurveyQuestionInfoData[] questList
        {
            get { return _questList; }
            set { _questList = value; }
        }

    }

    public class SurveyQuestionInfoData
    {
        public string _mintextlen;
        public string mintextlen
        {
            get { return _mintextlen; }
            set { _mintextlen = HttpUtility.UrlDecode(value); }
        }

        public string _maxtextlen;
        public string maxtextlen
        {
            get { return _maxtextlen; }
            set { _maxtextlen = HttpUtility.UrlDecode(value); }
        }

        public string _showcolumnnum;
        public string showcolumnnum
        {
            get { return _showcolumnnum; }
            set { _showcolumnnum = HttpUtility.UrlDecode(value); }
        }

        public string _askcategory;
        public string askcategory
        {
            get { return _askcategory; }
            set { _askcategory = HttpUtility.UrlDecode(value); }
        }

        public string _ask;
        public string ask
        {
            get { return _ask; }
            set { _ask = HttpUtility.UrlDecode(value); }
        }

        public string _siid;
        public string siid
        {
            get { return _siid; }
            set { _siid = HttpUtility.UrlDecode(value); }
        }

        public string _sqid;
        public string sqid
        {
            get { return _sqid; }
            set { _sqid = HttpUtility.UrlDecode(value); }
        }

        public string _ordernum;
        public string ordernum
        {
            get { return _ordernum; }
            set { _ordernum = HttpUtility.UrlDecode(value); }
        }

        public string _ismustanswer;
        public string IsMustAnswer
        {
            get { return _ismustanswer; }
            set { _ismustanswer = HttpUtility.UrlDecode(value); }
        }
        public string _isstatbyscore;
        public string IsStatByScore
        {
            get { return _isstatbyscore; }
            set { _isstatbyscore = HttpUtility.UrlDecode(value); }
        }

        public string _questionlinkid;

        /// <summary>
        /// 链接ID
        /// </summary>
        public string QuestionLinkId 
        {
            get { return _questionlinkid; }
            set { _questionlinkid = HttpUtility.UrlDecode(value); }
        }

        //option
        //matrix

        public SurveyOptionInfoData[] option;
        public SurveyMatiexInfoData[] matrix;
    }

    public class SurveyOptionInfoData
    {
        public string _ordernum;
        public string ordernum
        {
            get { return _ordernum; }
            set { _ordernum = HttpUtility.UrlDecode(value); }
        }

        public string _score;
        public string score
        {
            get { return _score; }
            set { _score = HttpUtility.UrlDecode(value); }
        }

        public string _isblank;
        public string isblank
        {
            get { return _isblank; }
            set { _isblank = HttpUtility.UrlDecode(value); }
        }

        public string _optionname;
        public string optionname
        {
            get { return _optionname; }
            set { _optionname = HttpUtility.UrlDecode(value); }
        }

        public string _sqid;
        public string sqid
        {
            get { return _sqid; }
            set { _sqid = HttpUtility.UrlDecode(value); }
        }

        public string _siid;
        public string siid
        {
            get { return _siid; }
            set { _siid = HttpUtility.UrlDecode(value); }
        }

        public string _soid;
        public string soid
        {
            get { return _soid; }
            set { _soid = HttpUtility.UrlDecode(value); }
        }

        /// <summary>
        /// 跳题用的链接ID
        /// </summary>
        public string _linkid;
        public string linkid
        {
            get { return _linkid; }
            set { _linkid = HttpUtility.UrlDecode(value); }
        }
    }

    public class SurveyMatiexInfoData
    {
        public string _smtid;
        public string smtid
        {
            get { return _smtid; }
            set { _smtid = HttpUtility.UrlDecode(value); }
        }

        public string _siid;
        public string siid
        {
            get { return _siid; }
            set { _siid = HttpUtility.UrlDecode(value); }
        }

        public string _sqid;
        public string sqid
        {
            get { return _sqid; }
            set { _sqid = HttpUtility.UrlDecode(value); }
        }

        public string _type;
        public string type
        {
            get { return _type; }
            set { _type = HttpUtility.UrlDecode(value); }
        }

        public string _titlename;
        public string titlename
        {
            get { return _titlename; }
            set { _titlename = HttpUtility.UrlDecode(value); }
        }

    }

}