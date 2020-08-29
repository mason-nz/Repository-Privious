using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.ArticleInfo
{
    public class ArticleInfo : DataBase
    {
        public readonly static ArticleInfo Instance = new ArticleInfo();        

        public DataTable QueryArticle(int articleID, int mediaType)
        {
            string sql = string.Empty;
            switch (mediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    sql = @"
                            SELECT  AI.Title ,
                                    AI.Digest Abstract ,
                                    AI.RecID ArticleID ,
                                    AI.Content ,
                                    AI.ContentURL Url
                            FROM    dbo.Weixin_ArticleInfo AI
                            WHERE   AI.RecID = @ArticleID;";
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                    sql = @"
                            SELECT  AI.Title ,
                                    AI.Discription Abstract ,
                                    AI.Id ArticleID ,
                                    AI.Content ,
                                    AI.Url Url ,
                                    AI.Category ,
                                    AI.Tags Tag
                            FROM    dbo.TouTiaoArticleInfo AI
                            WHERE   AI.Id = @ArticleID ;";
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                    sql = @"
                            SELECT  AI.Title ,
                                    AI.Abstract ,
                                    AI.RecID ArticleID ,
                                    AI.Content ,
                                    AI.Url Url ,
                                    AI.Category ,
                                    AI.Tag ,
                                    AI.HeadImg
                            FROM    dbo.SouHuArticleInfo AI
                            WHERE   AI.RecID = @ArticleID ";
                    break;
            }            
            var parameters = new SqlParameter[]{
                            new SqlParameter("@ArticleID",articleID)
                        };
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql, parameters);
            return data.Tables[0];
        }        
    }
}
