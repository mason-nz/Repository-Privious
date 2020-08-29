using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Interaction
{
    //媒体-互动参数信息表-微信，PC、APP没有互动参数设置
    public class InteractionWeixin
    {

        /// <summary>
        /// RecID
        /// </summary>		
        private int _recid;
        public int RecID
        {
            get { return _recid; }
            set { _recid = value; }
        }
        /// <summary>
        /// 媒体分类
        /// </summary>		
        private int _meidatype;
        public int MeidaType
        {
            get { return _meidatype; }
            set { _meidatype = value; }
        }
        /// <summary>
        /// MediaID
        /// </summary>		
        private int _mediaid;
        public int MediaID
        {
            get { return _mediaid; }
            set { _mediaid = value; }
        }
        /// <summary>
        /// ReferReadCount
        /// </summary>		
        private int _referreadcount;
        public int ReferReadCount
        {
            get { return _referreadcount; }
            set { _referreadcount = value; }
        }
        /// <summary>
        /// AveragePointCount
        /// </summary>		
        private int _averagepointcount;
        public int AveragePointCount
        {
            get { return _averagepointcount; }
            set { _averagepointcount = value; }
        }
        /// <summary>
        /// MoreReadCount
        /// </summary>		
        private int _morereadcount;
        public int MoreReadCount
        {
            get { return _morereadcount; }
            set { _morereadcount = value; }
        }
        /// <summary>
        /// OrigArticleCount
        /// </summary>		
        private int _origarticlecount;
        public int OrigArticleCount
        {
            get { return _origarticlecount; }
            set { _origarticlecount = value; }
        }
        /// <summary>
        /// UpdateCount
        /// </summary>		
        private int _updatecount;
        public int UpdateCount
        {
            get { return _updatecount; }
            set { _updatecount = value; }
        }
        /// <summary>
        /// MaxinumReading
        /// </summary>		
        private int _maxinumreading;
        public int MaxinumReading
        {
            get { return _maxinumreading; }
            set { _maxinumreading = value; }
        }
        /// <summary>
        /// ScreenShotURL
        /// </summary>		
        private string _screenshoturl;
        public string ScreenShotURL
        {
            get { return _screenshoturl; }
            set { _screenshoturl = value; }
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
        /// CreateUserID
        /// </summary>		
        private int _createuserid;
        public int CreateUserID
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        /// <summary>
        /// LastUpdateTime
        /// </summary>		
        private DateTime _lastupdatetime;
        public DateTime LastUpdateTime
        {
            get { return _lastupdatetime; }
            set { _lastupdatetime = value; }
        }
        /// <summary>
        /// LastUpdateUserID
        /// </summary>		
        private int _lastupdateuserid;
        public int LastUpdateUserID
        {
            get { return _lastupdateuserid; }
            set { _lastupdateuserid = value; }
        }

        public int WxID { get; set; }

    }

}
