using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class AppModel
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
        /// app名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 头像的URL地址
        /// </summary>		
        private string _headiconurl;
        public string HeadIconURL
        {
            get { return _headiconurl; }
            set { _headiconurl = value; }
        }
        /// <summary>
        /// 省、直辖市ID
        /// </summary>		
        private int _provinceid;
        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        /// 城市ID
        /// </summary>		
        private int _cityid;
        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        /// 日活量
        /// </summary>		
        private int _dailylive;
        public int DailyLive
        {
            get { return _dailylive; }
            set { _dailylive = value; }
        }
        /// <summary>
        /// 媒体介绍
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 数据状态
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
        /// SourceID
        /// </summary>		
        private int _sourceid;
        public int SourceID
        {
            get { return _sourceid; }
            set { _sourceid = value; }
        }
        /// <summary>
        /// 行业分类枚举ID
        /// </summary>		
        private int _categoryid;
        public int CategoryID
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }
        /// <summary>
        /// （0：不可监测 1：可检测）
        /// </summary>		
        private int _ismonitor;
        public int IsMonitor
        {
            get { return _ismonitor; }
            set { _ismonitor = value; }
        }
        /// <summary>
        /// （0：不可定位 1：可定位）
        /// </summary>		
        private int _islocate;
        public int IsLocate
        {
            get { return _islocate; }
            set { _islocate = value; }
        }
        /// <summary>
        /// TotalUser
        /// </summary>		
        private long _totaluser;
        public long TotalUser
        {
            get { return _totaluser; }
            set { _totaluser = value; }
        }
        /// <summary>
        /// 关联LE_SmartSearch主键
        /// </summary>		
        private int _smartsearchid;
        public int SmartSearchID
        {
            get { return _smartsearchid; }
            set { _smartsearchid = value; }
        }

        /// <summary>
        /// 行业分类名称
        /// </summary>		
        private string _categoryname;
        public string CategoryName
        {
            get { return _categoryname; }
            set { _categoryname = value; }
        }
    }


    public class LEAPPTemp
    {
        /// <summary>
        /// Name
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// HeadIconURL
        /// </summary>		
        private string _headiconurl;
        public string HeadIconURL
        {
            get { return _headiconurl; }
            set { _headiconurl = value; }
        }
        /// <summary>
        /// ProvinceID
        /// </summary>		
        private int _provinceid;
        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        /// CityID
        /// </summary>		
        private int _cityid;
        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        /// DailyLive
        /// </summary>		
        private int _dailylive;
        public int DailyLive
        {
            get { return _dailylive; }
            set { _dailylive = value; }
        }
        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// Status
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// CategoryName
        /// </summary>		
        private string _categoryname;
        public string CategoryName
        {
            get { return _categoryname; }
            set { _categoryname = value; }
        }
        /// <summary>
        /// IsMonitor
        /// </summary>		
        private bool _ismonitor;
        public bool IsMonitor
        {
            get { return _ismonitor; }
            set { _ismonitor = value; }
        }
        /// <summary>
        /// IsLocate
        /// </summary>		
        private bool _islocate;
        public bool IsLocate
        {
            get { return _islocate; }
            set { _islocate = value; }
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
        /// TotalUser
        /// </summary>		
        private long _totaluser;
        public long TotalUser
        {
            get { return _totaluser; }
            set { _totaluser = value; }
        }
        /// <summary>
        /// CreateUserID
        /// </summary>		
        private int _createuserid;
        public int CreateUserID
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        public string TagText { get; set; }
    }
}
