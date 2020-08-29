using System;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Collections.Generic;

namespace BitAuto.ISDC.CC2012.Entities
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 实体类QueryProjectTask_Cust 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	[Serializable]
	public class QueryProjectTask_Cust
	{
	    #region Model
        private int _userid;
        private string _custname;
        private string _UserName;
        private string _AbbrName;
        private string _provinceid;
        private string _cityid;
        private string _CountyID;
        private string _Brandids;
        private string _BeginSubmitTime;
        private string _EndSubmitTime;
        private string _SubmitTime;
        private string _failurereason;
        //private string _TaskStatus;

        private string _IsCallingOut;
        private string _BusinessNature;
        private string _submitusername;
        private string _cartype;
        private int _taskbatch;
        private int _tasksource;
        //private string _BeginSubmitTime;
        //private string _EndSubmitTime;

        public string IsCallingOut
        {
            set { _IsCallingOut = value; }
            get { return _IsCallingOut; }
        }
        /// <summary>
        /// 提交任务，日期开始时间
        /// </summary>
        public string  BeginSubmitTime
        {
            set { _BeginSubmitTime = value; }
            get { return _BeginSubmitTime; }
        }
        /// <summary>
        /// 提交任务，日期结束时间
        /// </summary>
        public string  EndSubmitTime
        {
            set { _EndSubmitTime = value; }
            get { return _EndSubmitTime; }
        }
        public string BusinessNature
        {
            set { _BusinessNature = value; }
            get { return _BusinessNature; }
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

        /// <summary>
        /// 提交人姓名
        /// </summary>
        public string SubmitUserName
        {
            set { _submitusername = value; }
            get { return _submitusername; }
        }

        public string AbbrName
        {
            set { _AbbrName = value; }
            get { return _AbbrName; }
        }

        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
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

        public string SubmitTime
        {
            set { _SubmitTime = value; }
            get { return _SubmitTime; }
        }

        /// <summary>
        /// 查询失败原因
        /// </summary>
        public string FailureReason
        {
            set { _failurereason = value; }
            get { return _failurereason; }
        }

        /// <summary>
        /// 任务轮次
        /// </summary>
        public int TaskBatch
        {
            set { _taskbatch = value; }
            get { return _taskbatch; }
        }

        /// <summary>
        /// 任务数据来源，1-Excel，2-CRM库
        /// </summary>
        public int TaskSource
        {
            set { _tasksource = value; }
            get { return _tasksource; }
        }
        //public string AuditStatus
        //{
        //    set { _AuditStatus = value; }
        //    get { return _AuditStatus; }
        //}

        //public string TaskStatus
        //{
        //    set { _TaskStatus = value; }
        //    get { return _TaskStatus; }
        //}


        /// <summary>
        /// 经营范围
        /// </summary>
        public string CarType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }
        #endregion Model

        public QueryProjectTask_Cust()
        {

            _userid = Constant.INT_INVALID_VALUE;
            _custname = Constant.STRING_INVALID_VALUE;
            _UserName = Constant.STRING_INVALID_VALUE;
            _AbbrName = Constant.STRING_INVALID_VALUE;
            _provinceid = Constant.STRING_INVALID_VALUE;
            _cityid = Constant.STRING_INVALID_VALUE;
            _CountyID = Constant.STRING_INVALID_VALUE;
            _Brandids = Constant.STRING_INVALID_VALUE;

            _IsCallingOut = Constant.STRING_INVALID_VALUE;
            _BusinessNature = Constant.STRING_INVALID_VALUE;
            //_AuditStatus = Constant.STRING_INVALID_VALUE;
            //_TaskStatus = Constant.STRING_INVALID_VALUE;
            _SubmitTime = Constant.STRING_INVALID_VALUE;
            _BeginSubmitTime = Constant.STRING_INVALID_VALUE;
            _EndSubmitTime = Constant.STRING_INVALID_VALUE;
            ptid = Constants.Constant.STRING_INVALID_VALUE;
            _failurereason = Constant.STRING_INVALID_VALUE;
            _taskbatch = Constants.Constant.INT_INVALID_VALUE;
            _tasksource = Constants.Constant.INT_INVALID_VALUE;
        }

        private string ptid;
        /// <summary>
        /// 
        /// </summary>
        public string PTID { get { return ptid; } set { ptid = value; } } 

        private List< EnumTaskStatus> taskStatus = new List<EnumTaskStatus>();
        /// <summary>
        /// 
        /// </summary>
        public List< EnumTaskStatus> TaskStatus { get { return taskStatus; } set { taskStatus = value; } }

        private List<EnumTaskStatus> toponetaskStatus = new List<EnumTaskStatus>();
        /// <summary>
        /// 查询最新任务状态
        /// </summary>
        public List<EnumTaskStatus> TopOneTaskStatus { get { return toponetaskStatus; } set { toponetaskStatus = value; } }

	}
}

