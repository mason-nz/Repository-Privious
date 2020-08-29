using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth
{
    public class Article
    {
        ///RecID
        ///</summary>
        public int RecID { get; set; }
     
        ///<summary>
        ///MsgID
        ///</summary>
        public string MsgID { get; set; }

        ///<summary>
        ///WxID
        ///</summary>
        public int WxID { get; set; }

        ///<summary>
        ///AppID
        ///</summary>
        public string AppID { get; set; }
      
        ///<summary>
        ///PubDate
        ///</summary>
        public DateTime PubDate { get; set; }

        ///<summary>
        ///ArticleType
        ///</summary>
        public int ArticleType { get; set; }

        ///<summary>
        ///IntReadUserCount
        ///</summary>
        public int IntReadUserCount { get; set; }

        ///<summary>
        ///IntReadCount
        ///</summary>
        public int IntReadCount { get; set; }

        ///<summary>
        ///OriReadUserCount
        ///</summary>
        public int OriReadUserCount { get; set; }

        ///<summary>
        ///OriReadCount
        ///</summary>
        public int OriReadCount { get; set; }

        ///<summary>
        ///ShareUserCount
        ///</summary>
        public int ShareUserCount { get; set; }

        ///<summary>
        ///ShareCount
        ///</summary>
        public int ShareCount { get; set; }

        ///<summary>
        ///CreateTime
        ///</summary>
        public DateTime CreateTime { get; set; }
    }
}

