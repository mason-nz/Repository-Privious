using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    public class TemplateInfo
    {
        public string _ttcode;
        public string ttcode
        {
            get { return _ttcode; }
            set { _ttcode = HttpUtility.UrlDecode(value).Trim(); }
        }


        public string _templateName;
        public string templateName
        {
            get { return _templateName; }
            set { _templateName = HttpUtility.UrlDecode(value).Trim(); }
        }


        public string _BGID;
        public string BGID
        {
            get { return _BGID; }
            set { _BGID = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _CID;
        public string CID
        {
            get { return _CID; }
            set { _CID = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _templateDesc;
        public string templateDesc
        {
            get { return _templateDesc; }
            set { _templateDesc = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _isshowbtn;
        public string IsShowBtn
        {
            get { return _isshowbtn; }
            set { _isshowbtn = HttpUtility.UrlDecode(value).Trim(); }
        }
        public string _isshowworkorderbtn;
        public string IsShowWorkOrderBtn
        {
            get { return _isshowworkorderbtn; }
            set { _isshowworkorderbtn = HttpUtility.UrlDecode(value).Trim(); }
        }
        public string _isshowsendmsg;
        public string IsShowSendMsgBtn
        {
            get { return _isshowsendmsg; }
            set { _isshowsendmsg = HttpUtility.UrlDecode(value).Trim(); }
        }
        public string _isshowqichetong;
        public string IsShowQiCheTong
        {
            get { return _isshowqichetong; }
            set { _isshowqichetong = HttpUtility.UrlDecode(value).Trim(); }
        }
        public string _isshowsubmitorder;
        public string IsShowSubmitOrder
        {
            get { return _isshowsubmitorder; }
            set { _isshowsubmitorder = HttpUtility.UrlDecode(value).Trim(); }
        }
        public FieldInfoData[] _fieldListInfo;
        public FieldInfoData[] fieldListInfo
        {
            get { return _fieldListInfo; }
            set { _fieldListInfo = value; }
        }
    }

    public class FieldInfoData
    {



        public string _RecID;
        public string RecID
        {
            get { return _RecID; }
            set { _RecID = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFCode;
        public string TFCode
        {
            get { return _TFCode; }
            set { _TFCode = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFDesName;
        public string TFDesName
        {
            get { return _TFDesName; }
            set { _TFDesName = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFName;
        public string TFName
        {
            get { return _TFName; }
            set { _TFName = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TTCode;
        public string TTCode
        {
            get { return _TTCode; }
            set { _TTCode = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TTypeID;
        public string TTypeID
        {
            get { return _TTypeID; }
            set { _TTypeID = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFLen;
        public string TFLen
        {
            get { return _TFLen; }
            set { _TFLen = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFDes;
        public string TFDes
        {
            get { return _TFDes; }
            set { _TFDes = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFInportIsNull;
        public string TFInportIsNull
        {
            get { return _TFInportIsNull; }
            set { _TFInportIsNull = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFIsNull;
        public string TFIsNull
        {
            get { return _TFIsNull; }
            set { _TFIsNull = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFSortIndex;
        public string TFSortIndex
        {
            get { return _TFSortIndex; }
            set { _TFSortIndex = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFCssName;
        public string TFCssName
        {
            get { return _TFCssName; }
            set { _TFCssName = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFIsExportShow;
        public string TFIsExportShow
        {
            get { return _TFIsExportShow; }
            set { _TFIsExportShow = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFIsListShow;
        public string TFIsListShow
        {
            get { return _TFIsListShow; }
            set { _TFIsListShow = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _TFShowCode;
        public string TFShowCode
        {
            get { return _TFShowCode; }
            set { _TFShowCode = HttpUtility.UrlDecode(value).Trim(); }
        }
        public string _TFGenSubField;
        public string TFGenSubField
        {
            get { return _TFGenSubField; }
            set { _TFGenSubField = HttpUtility.UrlDecode(value).Trim(); }
        }
        public string _TFValue;
        public string TFValue
        {
            get { return _TFValue; }
            set { _TFValue = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _Status;
        public string Status
        {
            get { return _Status; }
            set { _Status = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _CreateTime;
        public string CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _CreateUserID;
        public string CreateUserID
        {
            get { return _CreateUserID; }
            set { _CreateUserID = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _RowNumber;
        public string RowNumber
        {
            get { return _RowNumber; }
            set { _RowNumber = HttpUtility.UrlDecode(value).Trim(); }
        }

        public string _index;
        public string index
        {
            get { return _index; }
            set { _index = HttpUtility.UrlDecode(value).Trim(); }
        }


    }
}