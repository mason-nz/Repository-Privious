using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class QueryProjectTask_AuditContrastInfo
    {
        public QueryProjectTask_AuditContrastInfo()
        {
            _recid = Constant.INT_INVALID_VALUE;
            _ptid = Constant.INT_INVALID_VALUE;
            _custid = Constant.STRING_INVALID_VALUE;
            _dmsmemberid = Constant.STRING_INVALID_VALUE;
            _contrastinfoinside = Constant.STRING_INVALID_VALUE;
            _contrastinfo = Constant.STRING_INVALID_VALUE;
            _taskbatch = Constant.INT_INVALID_VALUE;
            _contrasttype = Constant.INT_INVALID_VALUE;
            _disposestatus = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _createuserid = Constant.INT_INVALID_VALUE;
            _createstartdate = Constant.STRING_INVALID_VALUE;
            _createenddate = Constant.STRING_INVALID_VALUE;
            _custidormemberid = Constant.STRING_INVALID_VALUE;
            _custnameormembername = Constant.STRING_INVALID_VALUE;
            _usertruename = Constant.STRING_INVALID_VALUE;
            _taskbatch = Constant.INT_INVALID_VALUE;
            _contrasttypeids = Constant.STRING_EMPTY_VALUE;
            _cooperatestatusids = Constant.STRING_EMPTY_VALUE;
        }
        #region Model
        private int _recid;
        private int? _ptid;
        private string _custid;
        private string _dmsmemberid;
        private string _contrastinfoinside;
        private string _contrastinfo;
        private int _exportstatus;
        private int? _contrasttype;
        private int _disposestatus;
        private DateTime? _createtime;
        private int? _createuserid;
        private string _createstartdate;
        private string _createenddate;
        private bool? _isnulldisposestatus = null;
        private string _custidormemberid;
        private string _custnameormembername;
        private string _usertruename;
        private int _taskbatch;
        private string _contrasttypeids;
        private string _cooperatestatusids;
        /// <summary>
        /// 
        /// </summary>
        public int RecID
        {
            set { _recid = value; }
            get { return _recid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PTID
        {
            set { _ptid = value; }
            get { return _ptid; }
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
        public string DMSMemberID
        {
            set { _dmsmemberid = value; }
            get { return _dmsmemberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContrastInfoInside
        {
            set { _contrastinfoinside = value; }
            get { return _contrastinfoinside; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContrastInfo
        {
            set { _contrastinfo = value; }
            get { return _contrastinfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ExportStatus
        {
            set { _exportstatus = value; }
            get { return _exportstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ContrastType
        {
            set { _contrasttype = value; }
            get { return _contrasttype; }
        }

        public int DisposeStatus
        {
            set { _disposestatus = value; }
            get { return _disposestatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }

        /// <summary>
        /// 查询创建开始时间
        /// </summary>
        public string CreateStartDate
        {
            set { _createstartdate = value; }
            get { return _createstartdate; }
        }
        /// <summary>
        /// 查询创建结束时间
        /// </summary>
        public string CreateEndDate
        {
            set { _createenddate = value; }
            get { return _createenddate; }
        }

        /// <summary>
        /// 查询处理状态是否为空NULL
        /// </summary>
        public bool? IsNullDisposeStatus
        {
            set { _isnulldisposestatus = value; }
            get { return _isnulldisposestatus; }
        }
        /// <summary>
        /// 客户或会员编号
        /// </summary>
        public string CustIDORMemberID
        {
            set { _custidormemberid = value; }
            get { return _custidormemberid; }
        }
        /// <summary>
        /// 客户或会员名称
        /// </summary>
        public string CustNameORMemberName
        {
            set { _custnameormembername = value; }
            get { return _custnameormembername; }
        }
        /// <summary>
        /// 坐席名称
        /// </summary>
        public string SeatTrueName
        {
            set { _usertruename = value; }
            get { return _usertruename; }
        }
        private string _cartype = Constant.STRING_EMPTY_VALUE;
        /// <summary>
        /// 经营范围
        /// </summary>
        public string CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }
        /// <summary>
        /// 所属轮次
        /// </summary>
        public int TaskBatch
        {
            set { _taskbatch = value; }
            get { return _taskbatch; }
        }

        /// <summary>
        /// 变更类型ID串
        /// </summary>
        public string ContrastTypeIDs
        {
            set { _contrasttypeids = value; }
            get { return _contrasttypeids; }
        }
        /// <summary>
        /// 会员合作状态ID串
        /// </summary>
        public string CooperateStatusIDs
        {
            set { _cooperatestatusids = value; }
            get { return _cooperatestatusids; }
        }

        //add by qizhiqiang 2012-5-21会员地区
        public string MemberProvinceID
        {
            set;
            get;
        }
        public string MemberCityID
        {
            set;
            get;
        }
        public string MemberCountyID
        {
            set;
            get;
        }

        #endregion Model
    }
}
