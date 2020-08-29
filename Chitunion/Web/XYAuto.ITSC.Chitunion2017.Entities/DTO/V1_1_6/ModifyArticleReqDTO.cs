using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6
{
    public class ModifyArticleReqDTO
    {
        public ModifyArticleReqDTO()
        {
            this.UpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 图文组ID
        /// </summary>
        public int GroupID { get; set; }

        public bool IsCheck { get; set; }

        public List<ModifyArticleItem> ArticleList { get; set; }

        /// <summary>
        /// 获取方式
        /// </summary>
        public ArticleImportTypeEnum ImportType { get; set; }
        public string fromUrl { get; set; }
        /// <summary>
        /// 来源微信ID
        /// </summary>
        public int fromWxID { get; set; }
        /// <summary>
        /// 来源微信名称
        /// </summary>
        public string fromWxName { get; set; }
        /// <summary>
        /// 来源微信号
        /// </summary>
        public string fromWxNumber { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public int UpdateUserID { get; set; }
        /// <summary>
        /// 校验组ID和图文ID
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            if (this.ArticleList == null || this.ArticleList.Count.Equals(0)) {
                sb.AppendLine("请至少添加一篇图文!");
            }
            else if (this.IsCheck)
                {
                foreach (var article in this.ArticleList)
                {
                    if (string.IsNullOrWhiteSpace(article.Title))
                    {
                        sb.AppendLine("图文标题不能为空!");
                        break;
                    }
                    if (string.IsNullOrWhiteSpace(article.CoverPicUrl))
                    {
                        sb.AppendLine("图文封面不能为空!");
                        break;
                    }
                    if (string.IsNullOrWhiteSpace(article.Content))
                    {
                        sb.AppendLine("图文内容不能为空!");
                        break;
                    }
                    //if(article.Content.Length > 20000)
                    //{
                        //sb.AppendLine("图文内容不能超过2w字!");
                       // break;
                    //}
                }
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }

    public class ModifyArticleItem
    {
        /// <summary>
        /// 图文ID
        /// </summary>
        public int ArticleID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Orderby { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        ///封面地址
        /// </summary>
        public string CoverPicUrl { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 原文链接
        /// </summary>
        public string OriginalUrl { get; set; }
    }
}
