using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Materiel
{
    public class MaterielChannelData : DataBase
    {
        public static readonly MaterielChannelData Instance = new MaterielChannelData();

        public bool Add(Entities.Materiel.MaterielChannelData model)
        {
            string sql = @"INSERT INTO dbo.MaterielChannelData( ChannelID ,MaterielID ,DataDate ,ReadCount ,LikeCount ,CommentCount ,CreateUserID ,CreateTime ,LastUpdateTime)
                                    SELECT @ChannelID ,MaterielID ,@DataDate ,@ReadCount ,@LikeCount ,@CommentCount ,@CreateUserID ,@CreateTime ,@LastUpdateTime FROM dbo.MaterielChannel WHERE ChannelID = @ChannelID;
                                    SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@ChannelID", model.ChannelID),
                new SqlParameter("@DataDate", model.DataDate.Date),
                new SqlParameter("@ReadCount", model.ReadCount),
                new SqlParameter("@LikeCount", model.LikeCount),
                new SqlParameter("@CommentCount", model.CommentCount),
                new SqlParameter("@CreateUserID", model.CreateUserID),
                new SqlParameter("@CreateTime", model.CreateTime),
                new SqlParameter("@LastUpdateTime", model.LastUpdateTime)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return Convert.ToInt32(obj) > 0;
        }

        public bool Update(Entities.Materiel.MaterielChannelData model)
        {
            string sql = @"UPDATE dbo.MaterielChannelData SET DataDate = @DataDate, ReadCount = @ReadCount, LikeCount=@LikeCount, CommentCount = @CommentCount, LastUpdateTime = @LastUpdateTime
                                    WHERE RecID = @RecID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@RecID", model.RecID),
                new SqlParameter("@DataDate", model.DataDate.Date),
                new SqlParameter("@ReadCount", model.ReadCount),
                new SqlParameter("@LikeCount", model.LikeCount),
                new SqlParameter("@CommentCount", model.CommentCount),
                new SqlParameter("@LastUpdateTime", model.LastUpdateTime)
            };
            var obj = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return Convert.ToInt32(obj) > 0;
        }

        public bool Delete(int recID)
        {
            string sql = "DELETE FROM dbo.MaterielChannelData WHERE RecID = " + recID;
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            return rowcount > 0;
        }

        public List<Entities.Materiel.MaterielChannelData> GetListByMaterielID(int[] materielIDList)
        {
            //   string sql = @"SELECT  mcd.ChannelID ,RecID ,DataDate ,ReadCount ,LikeCount ,CommentCount ,
            //                           mc.MediaTypeName ,mc.MediaNumber ,mc.MediaName,
            //                           d1.DictName as ChannelTypeName, d2.DictName AS PayTypeName,d3.DictName as PayModeName,mc.UnitCost
            //                           FROM dbo.MaterielChannelData mcd
            //                           INNER JOIN dbo.MaterielChannel mc ON mcd.ChannelID = mc.ChannelID
            //                           LEFT JOIN dbo.DictInfo d1 ON mc.ChannelType = d1.DictId
            //LEFT JOIN dbo.DictInfo d2 ON mc.PayType = d2.DictId
            //LEFT JOIN dbo.DictInfo d3 ON mc.PayMode = d3.DictId
            //                           WHERE mc.MaterielID = @MaterielID
            //                           ORDER BY mc.MediaTypeName ,mc.ChannelID ,mcd.DataDate; ";

            string sql = $@"SELECT  mc.ChannelID ,mc.MediaTypeName ,mc.MediaNumber ,mc.MediaName,
                                     mcd.RecID ,mcd.DataDate ,mcd.ReadCount ,mcd.LikeCount ,mcd.CommentCount ,
                                     d1.DictName as ChannelTypeName, d2.DictName AS PayTypeName,d3.DictName as PayModeName,mc.UnitCost,
                                     me.MaterielID,me.Name AS MaterialName,me.FootContentURL,cs.Name AS CarSerialName,cb.Name AS BrandName
                                     FROM dbo.MaterielChannel mc 
                                     LEFT JOIN dbo.MaterielChannelData mcd  ON mcd.ChannelID = mc.ChannelID
                                     LEFT JOIN dbo.DictInfo d1 ON mc.ChannelType = d1.DictId
                                     LEFT JOIN dbo.DictInfo d2 ON mc.PayType = d2.DictId
                                     LEFT JOIN dbo.DictInfo d3 ON mc.PayMode = d3.DictId
                                    INNER JOIN dbo.MaterielExtend me ON mc.MaterielID = me.MaterielID
		                            LEFT JOIN BaseData2017.dbo.CarSerial cs ON me.SerialID = cs.SerialID
		                            LEFT JOIN BaseData2017.dbo.CarBrand cb ON cs.BrandID = cb.BrandID   
                                     WHERE mc.MaterielID in ({string.Join(",", materielIDList)})
                                     ORDER BY mc.MediaTypeName ,mc.ChannelID ,mcd.DataDate;";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToList<Entities.Materiel.MaterielChannelData>(dt);
        }

        public bool CheckDataDateRepeat(int channelID, DateTime dataDate, int exceptRecID = 0)
        {
            string sql = "SELECT COUNT(1) FROM dbo.MaterielChannelData WHERE ChannelID = @ChannelID AND DataDate = @DataDate";
            if (exceptRecID != 0)
                sql += ("  AND RecID <> " + exceptRecID);
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@ChannelID", channelID),
                new SqlParameter("@DataDate", dataDate)
            };
            var obj = (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj > 0;
        }


    }
}
