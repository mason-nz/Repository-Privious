using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Funnel
{
    public class FunnelWaistDetailInfo
    {
     
     
        /// <summary>
        /// 渠道名称
        /// </summary>		
        private string _channelname = string.Empty;
        public string ChannelName
        {
            get { return _channelname; }
            set { _channelname = value; }
        }
  
        /// <summary>
        /// 文章类别名称
        /// </summary>		
        private string _category = string.Empty;
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        /// <summary>
        /// 抓取文章数量
        /// </summary>		
        private int _spidercount;
        public int SpiderCount
        {
            get { return _spidercount; }
            set { _spidercount = value; }
        }
        /// <summary>
        /// 机洗保留文章数量
        /// </summary>		
        private int _autocleancount;
        public int AutoCleanCount
        {
            get { return _autocleancount; }
            set { _autocleancount = value; }
        }
        /// <summary>
        /// 匹配车型文章数量
        /// </summary>		
        private int _matchedcount;
        public int MatchedCount
        {
            get { return _matchedcount; }
            set { _matchedcount = value; }
        }
        /// <summary>
        /// 人工清洗文章保留数量
        /// </summary>		
        private int _artificialcount;
        public int ArtificialCount
        {
            get { return _artificialcount; }
            set { _artificialcount = value; }
        }
        /// <summary>
        /// 封装使用文章数量
        /// </summary>		
        private int _encapsulatecount;
        public int EncapsulateCount
        {
            get { return _encapsulatecount; }
            set { _encapsulatecount = value; }
        }
       

    }
}
