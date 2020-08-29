using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.WxEditor
{
    /// <summary>
    /// 2017-06-22  zlb 
    /// 历史记录图文
    /// </summary>
    public class HistoryArticleGroup
    {
        /// <summary>
        /// 图文组批次主键ID
        /// </summary>
        public int WxGRID { get; set; }
        /// <summary>
        ///封面地址 
        /// </summary>
        public string CoverPicUrl { get; set; }
        /// <summary>
        /// 标题集合，如果是单图文则只有一个
        /// </summary>
        public List<string> TitleList = new List<string>();
        /// <summary>
        /// 图文同步微信状态集合
        /// </summary>
        public List<ArticleSatusInfo> AstatusList = new List<ArticleSatusInfo>();
        /// <summary>
        /// 完成时间
        /// </summary>
        public string ComplateTime { get; set; }
    }
    /// <summary>
    /// 图文同步微信状态信息
    /// </summary>
    public class ArticleSatusInfo
    {
        /// <summary>
        /// 微信名称
        /// </summary>
        public string WxName { get; set; }
        /// <summary>
        /// 微信头像地址
        /// </summary>
        public string HeadImg { get; set; }
        /// <summary>
        /// 同步状态
        /// </summary>
        public ArticleSyncStatus WxStatus { get; set; }
    }
}
