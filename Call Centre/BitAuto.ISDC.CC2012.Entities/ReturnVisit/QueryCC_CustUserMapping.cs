using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;
namespace BitAuto.ISDC.CC2012.Entities
{
    public class QueryCC_CustUserMapping
    {
        #region Model
        private int _id;
        private string _custid;
        private string _custids;
        private int _userid;
        private string _createtime;
        private string _custname;
        private string _UserName;
        private string _AgentNum;
        private string _provinceid;
        private string _cityid;
        private string _CountyID;
        private string _Brandids;
        //add by qizq  客户类型，经营范围2012-5-31
        private string _typeid;
        private string _cartype;
        //add lxw 2012.6.8 最近访问时间的起始时间、结束时间 是否联系
        private DateTime _starttime;
        private DateTime _endime;
        private int _contact;
        //add masj 2012-08-21 集采项目名
        private string _projectname;
        //add lihf 2014-03-26 执行中淘车通排期
        private string _radioTaoche;
        //add bif 2014-04-17 无坐席标识：-2表示不限制
        private int _noResponser;

        private string _tagid;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
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
        public string CustIDs
        {
            set { _custids = value; }
            get { return _custids; }
        }
        public string CustName
        {
            set { _custname = value; }
            get { return _custname; }
        }
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }
        public string AgentNum
        {
            set { _AgentNum = value; }
            get { return _AgentNum; }
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
        public string Brandids
        {
            set { _Brandids = value; }
            get { return _Brandids; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        //add by qizq  客户类型，经营范围2012-5-31
        public string TypeID
        {
            set { _typeid = value; }
            get { return _typeid; }
        }
        /// <summary>
        /// 客户经营范围0—未知，1—新车，2—二手车，3—新车和二手车
        /// </summary>
        public string CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
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
        public int Contact
        {
            set { _contact = value; }
            get { return _contact; }
        }
        /// <summary>
        /// add masj 2012-08-21 集采项目名
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        /// <summary>
        /// add lihf 2014-03-25 执行中淘车通排期
        /// </summary>
        public string radioTaoche
        {
            set { _radioTaoche = value; }
            get { return _radioTaoche; }
        }
        public int NoResponser
        {
            set { _noResponser = value; }
            get { return _noResponser; }
        }
        #endregion Model

        public QueryCC_CustUserMapping()
        {
            _id = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _custids = Constant.STRING_INVALID_VALUE;
            _userid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.STRING_INVALID_VALUE;

            _custname = Constant.STRING_INVALID_VALUE;
            _UserName = Constant.STRING_INVALID_VALUE;
            _noResponser = Constant.INT_INVALID_VALUE;
            _AgentNum = Constant.STRING_INVALID_VALUE;

            _provinceid = Constant.STRING_INVALID_VALUE;
            _cityid = Constant.STRING_INVALID_VALUE;
            _CountyID = Constant.STRING_INVALID_VALUE;
            _Brandids = Constant.STRING_INVALID_VALUE;
            //add by qizq  客户类型，经营范围2012-5-31
            _typeid = Constant.STRING_INVALID_VALUE;
            _cartype = Constant.STRING_INVALID_VALUE;

            _starttime = Constant.DATE_INVALID_VALUE;
            _endime = Constant.DATE_INVALID_VALUE;
            _contact = Constant.INT_INVALID_VALUE;
            _projectname = Constant.STRING_INVALID_VALUE;
            _bgidstr = Constant.STRING_INVALID_VALUE;
            _reqeustccprojectname = Constant.STRING_INVALID_VALUE;
            _radioTaoche = Constant.STRING_INVALID_VALUE;

            _tagid = Constant.STRING_INVALID_VALUE;
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

        public string TagID
        {
            set { _tagid = value; }
            get { return _tagid; }
        }
    }
}
