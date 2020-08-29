using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    /// <summary>
    /// zlb 2017-7-1
    /// URL导入文章类
    /// </summary>
    public class ImportGroupArticle
    {
        public string code { get; set; }
        public string message { get; set; }
        public SingleArticleImport data { get; set; }
    }
    /// <summary>
    /// zlb 2017-7-1
    /// URL导入文章类
    /// </summary>
    public class SingleArticleImport
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 来源微信名称
        /// </summary>
        public string fromWxName { get; set; }
        /// <summary>
        /// 来源微信号
        /// </summary>
        public string fromWxNumber { get; set; }

    }


}
