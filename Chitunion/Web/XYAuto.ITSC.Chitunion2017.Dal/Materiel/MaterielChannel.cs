using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Materiel
{
    public class MaterielChannel:DataBase
    {
        public static readonly MaterielChannel Instance = new MaterielChannel();

        public bool Save(int materielID, List<Entities.Materiel.MaterielChannel> channelList)
        {
            List<int> oldChannelIDs = this.GetExistsChannelIDs(materielID);
            List<int> editChannelIDs = channelList.Where(c => c.ChannelID > 0).Select(c => c.ChannelID).Distinct().ToList();
            List<int> delChannelIDs = oldChannelIDs.Except(editChannelIDs).ToList();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                #region 增改删
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in channelList)
                        {
                            if (item.ChannelID.Equals(0))
                            {//新增
                                this.Add(item, trans);
                            }
                            else
                            {//编辑
                                this.Update(item, trans);
                            }
                        }
                        if (delChannelIDs != null && delChannelIDs.Count > 0)
                        {//删除
                            delChannelIDs.ForEach(id => { this.Delete(id, trans); });
                        }
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                #endregion
            }
        }

        public int Add(Entities.Materiel.MaterielChannel model, SqlTransaction trans = null)
        {
            string sql = @"INSERT INTO dbo.MaterielChannel( MaterielID ,MediaTypeName ,ChannelType ,MediaNumber ,MediaName ,PayType ,PayMode ,UnitCost ,PromotionUrl ,PromotionUrlCode ,CreateUserID ,CreateTime ,LastUpdateTime)
                                    VALUES( @MaterielID ,@MediaTypeName ,@ChannelType ,@MediaNumber ,@MediaName ,@PayType ,@PayMode ,@UnitCost ,@PromotionUrl ,@PromotionUrlCode ,@CreateUserID ,@CreateTime ,@LastUpdateTime);
                                    SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaterielID", model.MaterielID),
                new SqlParameter("@MediaTypeName", model.MediaTypeName),
                new SqlParameter("@ChannelType", model.ChannelType),
                new SqlParameter("@MediaNumber", model.MediaNumber),
                new SqlParameter("@MediaName", model.MediaName),
                new SqlParameter("@PayType", model.PayType),
                new SqlParameter("@PayMode", model.PayMode),
                new SqlParameter("@UnitCost", model.UnitCost),
                new SqlParameter("@PromotionUrl", model.PromotionUrl),
                new SqlParameter("@PromotionUrlCode", model.PromotionUrlCode),
                new SqlParameter("@CreateUserID", model.CreateUserID),
                new SqlParameter("@CreateTime", model.CreateTime),
                new SqlParameter("@LastUpdateTime", model.LastUpdateTime),
            };
            int channelID = 0;
            if(trans == null)
                channelID = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            else
                channelID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
            return channelID;
        }

        public bool Update(Entities.Materiel.MaterielChannel model, SqlTransaction trans = null)
        {
            string sql = @"UPDATE dbo.MaterielChannel SET ChannelType = @ChannelType, PayType = @PayType, PayMode = @PayMode, UnitCost = @UnitCost, LastUpdateTime = @LastUpdateTime  
                                    WHERE ChannelID = @ChannelID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ChannelID", model.ChannelID),
                new SqlParameter("@ChannelType", model.ChannelType),
                new SqlParameter("@PayType", model.PayType),
                new SqlParameter("@PayMode", model.PayMode),
                new SqlParameter("@UnitCost", model.UnitCost),
                new SqlParameter("@LastUpdateTime", model.LastUpdateTime),
            };
            int rowcount = 0;
            if (trans == null)
                rowcount =SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
            return rowcount > 0;
        }

        public bool Delete(int channelID, SqlTransaction trans = null)
        {
            string sql = "DELETE FROM dbo.MaterielChannel WHERE ChannelID = " + channelID + ";";
            sql += "DELETE FROM dbo.MaterielChannelData WHERE ChannelID = " + channelID + ";";
            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
            return rowcount > 0;
        }

        public List<int> GetExistsChannelIDs(int materielID)
        {
            List<int> list = new List<int>();
            string sql = "SELECT ChannelID FROM dbo.MaterielChannel WHERE MaterielID = " + materielID;
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add((int)dr["ChannelID"]);
                }
            }
            return list;
        }
    }
}
