using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.MediaLabelResult
{
    public class MediaLabelResult : DataBase
    {
        public static readonly MediaLabelResult Instance = new MediaLabelResult();
        public DataTable GetListByMedia(int MediaType, string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  VUI.SysName UserName ,
                                    MLR.CreateTime ,
                                    MLR.MediaNumber
                            FROM    dbo.MediaLabelResult MLR WITH ( NOLOCK )
                                    LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI WITH ( NOLOCK ) ON VUI.UserID = MLR.CreateUserID
                            WHERE   MLR.Status = 0
                                    AND MLR.MediaType = {MediaType} ");
            switch (MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    if (!string.IsNullOrEmpty(NumberOrName))
                        sbSql.Append($"AND MLR.MediaNumber='{NumberOrName}'");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.APP:
                case (int)Entities.ENUM.ENUM.EnumMediaType.视频:
                case (int)Entities.ENUM.ENUM.EnumMediaType.直播:
                case (int)Entities.ENUM.ENUM.EnumMediaType.新浪微博:
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    if (!string.IsNullOrEmpty(NumberOrName))
                        sbSql.Append($"AND MLR.MediaName='{NumberOrName}'");
                    break;
                default:
                    break;
            }
            sbSql.Append($" ORDER BY MLR.CreateTime DESC ");
            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public int GetCountByMedia(int MediaType, string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  COUNT(1)
                            FROM    dbo.BatchMedia BM
                            WHERE   BM.MediaType = {MediaType}
                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} ");
            switch (MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    if (!string.IsNullOrEmpty(NumberOrName))
                        sbSql.Append($"AND BM.MediaNumber='{NumberOrName}'");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.APP:
                case (int)Entities.ENUM.ENUM.EnumMediaType.视频:
                case (int)Entities.ENUM.ENUM.EnumMediaType.直播:
                case (int)Entities.ENUM.ENUM.EnumMediaType.新浪微博:
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    if (!string.IsNullOrEmpty(NumberOrName))
                        sbSql.Append($"AND BM.MediaName='{NumberOrName}'");
                    break;
                default:
                    break;
            }

            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return Convert.ToInt32(data);
        }
        public DataTable GetListByCar(int brandID, int serialID)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT TOP 50
                                    VUI.SysName UserName ,
                                    MLR.CreateTime
                            FROM    dbo.MediaLabelResult MLR
                                    JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = MLR.CreateUserID
                            WHERE   MLR.Status = 0
                                    AND MLR.BrandID = {brandID}");

            //if (serialID != -2)
            sbSql.Append($"AND MLR.SerialID = {serialID}");

            sbSql.Append($" ORDER BY MLR.CreateTime DESC ");
            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public int GetCountByCar(int brandID, int serialID)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  COUNT(1)
                            FROM    dbo.BatchMedia BM
                            WHERE   BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                                    AND BM.BrandID = {brandID} ");
            sbSql.Append($" AND BM.SerialID = {serialID}");

            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return Convert.ToInt32(data);
        }
    }
}
