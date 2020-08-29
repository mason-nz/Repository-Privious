using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Interaction
{
    //媒体-互动参数信息表-直播，PC、APP没有互动参数设置
    public class InteractionBroadcast
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
        /// AudienceCount
        /// </summary>		
        private int _audiencecount;
        public int AudienceCount
        {
            get { return _audiencecount; }
            set { _audiencecount = value; }
        }
        /// <summary>
        /// MaximumAudience
        /// </summary>		
        private int _maximumaudience;
        public int MaximumAudience
        {
            get { return _maximumaudience; }
            set { _maximumaudience = value; }
        }
        /// <summary>
        /// AverageAudience
        /// </summary>		
        private int _averageaudience;
        public int AverageAudience
        {
            get { return _averageaudience; }
            set { _averageaudience = value; }
        }
        /// <summary>
        /// CumulateReward
        /// </summary>		
        private int _cumulatereward;
        public int CumulateReward
        {
            get { return _cumulatereward; }
            set { _cumulatereward = value; }
        }
        /// <summary>
        /// CumulateIncome
        /// </summary>		
        private int _cumulateincome;
        public int CumulateIncome
        {
            get { return _cumulateincome; }
            set { _cumulateincome = value; }
        }
        /// <summary>
        /// CumulatePoints
        /// </summary>		
        private int _cumulatepoints;
        public int CumulatePoints
        {
            get { return _cumulatepoints; }
            set { _cumulatepoints = value; }
        }
        /// <summary>
        /// CumulateSendCount
        /// </summary>		
        private int _cumulatesendcount;
        public int CumulateSendCount
        {
            get { return _cumulatesendcount; }
            set { _cumulatesendcount = value; }
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

    }

}
