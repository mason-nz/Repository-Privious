using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Funnel
{
    public class FunnelHeadDetailInfo
    {
  
        /// <summary>
        /// 场景名称
        /// </summary>		
        private string _scenename = string.Empty;
        public string SceneName
        {
            get { return _scenename; }
            set { _scenename = value; }
        }

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
        /// 头部文章账号类型名称
        /// </summary>		
        private string _aascoretypename = string.Empty;
        public string AAScoreTypeName
        {
            get { return _aascoretypename; }
            set { _aascoretypename = value; }
        }
        /// <summary>
        /// 抓取文章量
        /// </summary>		
        private int _spiderarticlecount;
        public int SpiderArticleCount
        {
            get { return _spiderarticlecount; }
            set { _spiderarticlecount = value; }
        }
        /// <summary>
        /// 抓取账号量
        /// </summary>		
        private int _spideraccountcount;
        public int SpiderAccountCount
        {
            get { return _spideraccountcount; }
            set { _spideraccountcount = value; }
        }
        /// <summary>
        /// 机洗文章数量
        /// </summary>		
        private int _autoarticlecount;
        public int AutoArticleCount
        {
            get { return _autoarticlecount; }
            set { _autoarticlecount = value; }
        }
        /// <summary>
        /// 机洗账号数量
        /// </summary>		
        private int _autoaccountcount;
        public int AutoAccountCount
        {
            get { return _autoaccountcount; }
            set { _autoaccountcount = value; }
        }
        /// <summary>
        /// 初筛保留文章
        /// </summary>		
        private int _primaryarticlecount;
        public int PrimaryArticleCount
        {
            get { return _primaryarticlecount; }
            set { _primaryarticlecount = value; }
        }
        /// <summary>
        /// 初筛保留账号
        /// </summary>		
        private int _primaryaccountcount;
        public int PrimaryAccountCount
        {
            get { return _primaryaccountcount; }
            set { _primaryaccountcount = value; }
        }
        /// <summary>
        /// 清洗保留文章
        /// </summary>		
        private int _artificialarticlecount;
        public int ArtificialArticleCount
        {
            get { return _artificialarticlecount; }
            set { _artificialarticlecount = value; }
        }
        /// <summary>
        /// 清洗保留账号
        /// </summary>		
        private int _artificialaccountcount;
        public int ArtificialAccountCount
        {
            get { return _artificialaccountcount; }
            set { _artificialaccountcount = value; }
        }
        /// <summary>
        /// 封装使用文章
        /// </summary>		
        private int _encapsulatearticlecount;
        public int EncapsulateArticleCount
        {
            get { return _encapsulatearticlecount; }
            set { _encapsulatearticlecount = value; }
        }
        /// <summary>
        /// 封装使用账号
        /// </summary>		
        private int _encapsulateaccountcount;
        public int EncapsulateAccountCount
        {
            get { return _encapsulateaccountcount; }
            set { _encapsulateaccountcount = value; }
        }
      
    }
}
