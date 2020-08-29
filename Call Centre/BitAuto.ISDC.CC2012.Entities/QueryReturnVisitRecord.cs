using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryReturnVisitRecord
    {
           //url += '&CustName=' + escape(txtSearchCustName);
        //       url += '&CustID=' + escape(txtCustID);
        //       url += '&Province=' + escape(ddlSearchProvince);
        //       url += '&City=' + escape(ddlSearchCity);
        //       url += '&County=' + escape(ddlSearchCounty);
        //       url += '&CustType=' + escape(selCustType);
        //       url += '&StartTime=' + escape(txtStartTime);
        //       url += '&EndTime=' + escape(txtEndTime);
        //       url += '&TypeID=' + escape(TypeID);
        //       url += '&VisitType=' + escape(selVisitType);
        //       url += '&VisitUserid=' + escape(SelectUserid);
        //       url += '&ProjectName=' + escapeStr(txtSearchProjectName);
        #region Model
        private string _custid;
        private string _custname;
        private string _provinceid;
        private string _cityid;
        private string _CountyID;
        private string _custtype;
        private string _typeid;
        private string _visittype;
        public string VisitType
        {
            get{return _visittype;}
            set{_visittype=value;}
        }
        private string _visituserid;
        public string VisitUserid
        {
            get{return _visituserid;}
            set{_visituserid=value;}
        }

        private DateTime _starttime;
        private DateTime _endime;
        //add masj 2012-08-21 集采项目名
        private string _projectname;

        /// <summary>
        /// 
        /// </summary>
        public string CustID
        {
            set { _custid = value; }
            get { return _custid; }
        }
        /// <summary>
        /// 
        /// </summary>

        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }
      


        public string ProvinceID
        {
            set { _provinceid = value; }
            get { return _provinceid; }
        }
        public string CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        public string CountyID
        {
            set { _CountyID = value; }
            get { return _CountyID; }
        }

        /// <summary>
        /// 
        /// </summary>
        
       
        //add by qizq  客户类型，经营范围2012-5-31
        public string CustType
        {
            set { _custtype = value; }
            get { return _custtype; }
        }
        /// <summary>
        /// 1—新车，2—二手车，4—易卡
        /// </summary>
        public string TypeID
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndTime
        {
            set { _endime = value; }
            get { return _endime; }
        }
       
        /// <summary>
        /// add masj 2012-08-21 集采项目名
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        #endregion Model

        public QueryReturnVisitRecord()
        {
           
            _custid = Constant.STRING_INVALID_VALUE;


            _custname = Constant.STRING_INVALID_VALUE;


            _provinceid = Constant.STRING_INVALID_VALUE;
            _cityid = Constant.STRING_INVALID_VALUE;
            _CountyID = Constant.STRING_INVALID_VALUE;

            //add by qizq  客户类型，经营范围2012-5-31
            _typeid = Constant.STRING_INVALID_VALUE;
            _custtype = Constant.STRING_INVALID_VALUE;
            _visituserid = Constant.STRING_INVALID_VALUE;
            _starttime = Constant.DATE_INVALID_VALUE;
            _endime = Constant.DATE_INVALID_VALUE;
            _visittype = Constant.STRING_INVALID_VALUE;
            _projectname = Constant.STRING_INVALID_VALUE;
            _reqeustccprojectname = Constant.STRING_INVALID_VALUE;
        }
        private string _reqeustccprojectname;
        public string ReqeustCCProjectName
        {
            set { _reqeustccprojectname = value; }
            get { return _reqeustccprojectname; }
        }
        private string _bgidstr;
        public string BGIDStr
        {
            set { _bgidstr = value; }
            get { return _bgidstr; }
        }

    }
}
