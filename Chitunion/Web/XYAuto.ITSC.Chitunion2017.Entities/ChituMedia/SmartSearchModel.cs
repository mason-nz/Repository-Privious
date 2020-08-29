using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class SmartSearchModel
    {
        /// <summary>
		/// 主键
        /// </summary>		
		private int _recid;
        public int RecID
        {
            get { return _recid; }
            set { _recid = value; }
        }
        /// <summary>
        /// 推广名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 推广需求
        /// </summary>		
        private string _demand;
        public string Demand
        {
            get { return _demand; }
            set { _demand = value; }
        }
        /// <summary>
        /// 推广目的
        /// </summary>		
        private int _purposes;
        public int Purposes
        {
            get { return _purposes; }
            set { _purposes = value; }
        }
        /// <summary>
        /// 推广预算
        /// </summary>		
        private decimal _budgetprice;
        public decimal BudgetPrice
        {
            get { return _budgetprice; }
            set { _budgetprice = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>		
        private DateTime _begintime;
        public DateTime BeginTime
        {
            get { return _begintime; }
            set { _begintime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>		
        private DateTime _endtime;
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        /// <summary>
        /// 物料路径
        /// </summary>		
        private string _materialurl;
        public string MaterialUrl
        {
            get { return _materialurl; }
            set { _materialurl = value; }
        }
        /// <summary>
        /// 物料名称
        /// </summary>		
        private string _materialurlname;
        public string MaterialUrlName
        {
            get { return _materialurlname; }
            set { _materialurlname = value; }
        }
        /// <summary>
        /// UserID
        /// </summary>		
        private int _userid;
        public int UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// （待审核、进行中、已完成、驳回、删除）
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// CreateTime
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// RejectCause
        /// </summary>		
        private string _rejectcause;
        public string RejectCause
        {
            get { return _rejectcause; }
            set { _rejectcause = value; }
        }
    }

    public class SmartSearchListModel
    {
        /// <summary>
		/// 主键
        /// </summary>		
		private int _recid;
        public int RecID
        {
            get { return _recid; }
            set { _recid = value; }
        }
        /// <summary>
        /// 推广名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 推广需求
        /// </summary>		
        private string _demand;
        public string Demand
        {
            get { return _demand; }
            set { _demand = value; }
        }
        /// <summary>
        /// 推广目的
        /// </summary>		
        private int _purposes;
        public int Purposes
        {
            get { return _purposes; }
            set { _purposes = value; }
        }
        /// <summary>
        /// 推广预算
        /// </summary>		
        private decimal _budgetprice;
        public decimal BudgetPrice
        {
            get { return _budgetprice; }
            set { _budgetprice = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>		
        private DateTime _begintime;
        public DateTime BeginTime
        {
            get { return _begintime; }
            set { _begintime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>		
        private DateTime _endtime;
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        /// <summary>
        /// 物料路径
        /// </summary>		
        private string _materialurl;
        public string MaterialUrl
        {
            get { return _materialurl; }
            set { _materialurl = value; }
        }
        /// <summary>
        /// RejectCause
        /// </summary>		
        private string _rejectcause;
        public string RejectCause
        {
            get { return _rejectcause; }
            set { _rejectcause = value; }
        }
        /// <summary>
        /// MediaCount
        /// </summary>		
        private int _mediacount;
        public int MediaCount
        {
            get { return _mediacount; }
            set { _mediacount = value; }
        }

        /// <summary>
        /// MediaCount
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// MediaCount
        /// </summary>		
        private string _statusname;
        public string StatusName
        {
            get { return _statusname; }
            set { _statusname = value; }
        }
        /// <summary>
        /// MaterialUrlName
        /// </summary>		
        private string _materialurlname;
        public string MaterialUrlName
        {
            get { return _materialurlname; }
            set { _materialurlname = value; }
        }
        
    }

    public class QueryArgs
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }
    }
}
