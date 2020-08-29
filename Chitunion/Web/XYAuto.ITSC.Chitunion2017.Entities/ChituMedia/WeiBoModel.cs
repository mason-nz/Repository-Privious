using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    //媒体-微博公众号信息 （冗余表）
    public class WeiBoModel
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
        /// 微信号
        /// </summary>		
        private string _number;
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
        /// <summary>
        /// 微信名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Sex
        /// </summary>		
        private string _sex;
        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
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
        /// 粉丝数
        /// </summary>		
        private int _fanscount;
        public int FansCount
        {
            get { return _fanscount; }
            set { _fanscount = value; }
        }
        ///// <summary>
        ///// 粉丝男比例
        ///// </summary>		
        //private string _fanssex;
        //public string FansSex
        //{
        //    get { return _fanssex; }
        //    set { _fanssex = value; }
        //}
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
        /// 描述、签名
        /// </summary>		
        private string _sign;
        public string Sign
        {
            get { return _sign; }
            set { _sign = value; }
        }
        ///// <summary>
        ///// 创建时间
        ///// </summary>		
        //private DateTime _createtime;
        //public DateTime CreateTime
        //{
        //    get { return _createtime; }
        //    set { _createtime = value; }
        //}
        ///// <summary>
        ///// 创建人
        ///// </summary>		
        //private int _createuserid;
        //public int CreateUserID
        //{
        //    get { return _createuserid; }
        //    set { _createuserid = value; }
        //}
        ///// <summary>
        ///// LastUpdateTime
        ///// </summary>		
        //private DateTime _lastupdatetime;
        //public DateTime LastUpdateTime
        //{
        //    get { return _lastupdatetime; }
        //    set { _lastupdatetime = value; }
        //}
        ///// <summary>
        ///// LastUpdateUserID
        ///// </summary>		
        //private int _lastupdateuserid;
        //public int LastUpdateUserID
        //{
        //    get { return _lastupdateuserid; }
        //    set { _lastupdateuserid = value; }
        //}
        ///// <summary>
        ///// 关联LE_SmartSearch主键
        ///// </summary>		
        //private int _smartsearchid;
        //public int SmartSearchID
        //{
        //    get { return _smartsearchid; }
        //    set { _smartsearchid = value; }
        //}
        ///// <summary>
        ///// 源表LE_Weibo 的ID 
        ///// </summary>		
        //private int _sourceid;
        //public int SourceID
        //{
        //    get { return _sourceid; }
        //    set { _sourceid = value; }
        //}
        /// <summary>
        /// 平均转发数
        /// </summary>		
        private int _forwardavg;
        public int ForwardAvg
        {
            get { return _forwardavg; }
            set { _forwardavg = value; }
        }
        /// <summary>
        /// 平均评论数
        /// </summary>		
        private int _commentavg;
        public int CommentAvg
        {
            get { return _commentavg; }
            set { _commentavg = value; }
        }
        /// <summary>
        /// 平均点赞数
        /// </summary>		
        private int _likeavg;
        public int LikeAvg
        {
            get { return _likeavg; }
            set { _likeavg = value; }
        }
        /// <summary>
        /// 直发参考价
        /// </summary>		
        private decimal _directprice;
        public decimal DirectPrice
        {
            get { return _directprice; }
            set { _directprice = value; }
        }
        /// <summary>
        /// 转发参考价
        /// </summary>		
        private decimal _forwardprice;
        public decimal ForwardPrice
        {
            get { return _forwardprice; }
            set { _forwardprice = value; }
        }
        /// <summary>
        /// 省级ID
        /// </summary>		
        private int _provinceid;
        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        /// 市级ID
        /// </summary>		
        private int _cityid;
        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        ///// <summary>
        ///// 媒体状态
        ///// </summary>		
        //private int _status;
        //public int Status
        //{
        //    get { return _status; }
        //    set { _status = value; }
        //}


        private string _categoryname;
        public string CategoryName
        {
            get { return _categoryname; }
            set { _categoryname = value; }
        }

        /// <summary>
        /// TimestampSign
        /// </summary>		
        private DateTime _timestampsign;
        public DateTime TimestampSign
        {
            get { return _timestampsign; }
            set { _timestampsign = value; }
        }
        private int _authtype;
        public int AuthType
        {
            get { return _authtype; }
            set { _authtype = value; }
        }

        public int IsReserve { get; set; }


        public int TotalScores { get; set; }

        public string Summary { get; set; }


        public decimal IndexScore { get; set; }

        public string TagText { get; set; }


    }
}
