using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaFansArea : DataBase
    {
        #region Instance

        public static readonly MediaFansArea Instance = new MediaFansArea();

        #endregion Instance

        public int Insert(Entities.Media.MediaFansArea entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_FansArea(");
            strSql.Append("MediaID,MediaType,ProvinceID,UserScale,CreateUserID,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@MediaID,@MediaType,@ProvinceID,@UserScale,@CreateUserID,getdate()");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserScale",entity.UserScale),
                         new SqlParameter("@MediaType",entity.MediaType),
                          new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CreateUserID",entity.CreateUserID)
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int InsertByBulk(Entities.Media.MediaFansArea entity, List<string[]> fansAreas)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" DELETE FROM DBO.Media_FansArea WHERE MediaID = {0} AND MediaType = {1} ", entity.MediaID, entity.MediaType);

            if (fansAreas.Count > 0)
            {
                strSql.AppendLine(" insert into Media_FansArea(");
                strSql.Append(" MediaID,MediaType,ProvinceID,UserScale,CreateUserID,CreateTime )");
                strSql.Append(" values ");
                foreach (var item in fansAreas)
                {
                    strSql.Append(" (");
                    strSql.AppendFormat(" {0},{1},{2},{3},{4},getdate()", entity.MediaID, entity.MediaType, item[0]
                        , item[1], entity.CreateUserID);
                    strSql.Append(" ),");
                }
            }
            var parameters = new SqlParameter[] { };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString().Trim().TrimEnd(','), parameters);
        }

        /// <summary>
        /// 插入Fans_Weixin表，粉丝分布区域比例数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fansAreas"></param>
        /// <returns></returns>
        public int InsertAuthFansWeixinByBulk(Entities.Media.MediaFansArea entity, List<string[]> fansAreas)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" DELETE FROM DBO.Fans_Weixin WHERE WxID = {0} AND StatisticType = {1} ", entity.MediaID, (int)StatisticTypeEnum.FansAreaMapper);
            strSql.AppendLine(" insert into Fans_Weixin(");
            strSql.Append(@" WxID ,
                              StatisticType,
                              StatisticKey,
                              StatisticText,
                              StatisticValue)");
            strSql.Append(" values ");
            foreach (var item in fansAreas)
            {
                strSql.Append(" (");
                strSql.AppendFormat(" {0},{1},{2},'{3}','{4}'", entity.MediaID, (int)StatisticTypeEnum.FansAreaMapper, item[0],
                    string.Empty, item[1]);
                strSql.Append(" ),");
            }

            var parameters = new SqlParameter[] { };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString().Trim().TrimEnd(','), parameters);
        }

        /// <summary>
        /// 插入Fans_Weixin表，粉丝数比例数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fansMalePer">男粉丝数比例</param>
        /// <param name="fansFemalePer">女粉丝数比例</param>
        /// <returns></returns>
        public int InsertAuthFansWeixinByFansSex(Entities.Media.MediaFansArea entity, decimal fansMalePer, decimal fansFemalePer)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(" DELETE FROM DBO.Fans_Weixin WHERE WxID = {0} AND StatisticType = {1} ", entity.MediaID, (int)StatisticTypeEnum.FansSexProportion);
            strSql.AppendLine(" insert into Fans_Weixin(");
            strSql.Append(@" WxID ,
                              StatisticType,
                              StatisticKey,
                              StatisticText,
                              StatisticValue)");
            strSql.Append(" values ");
            strSql.AppendFormat(" ({0},{1},{2},'{3}','{4}')", entity.MediaID, (int)StatisticTypeEnum.FansSexProportion, (int)SexEnum.男, string.Empty, fansMalePer);
            strSql.AppendFormat(" ,({0},{1},{2},'{3}','{4}')", entity.MediaID, (int)StatisticTypeEnum.FansSexProportion, (int)SexEnum.女, string.Empty, fansFemalePer);
            var parameters = new SqlParameter[] { };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString().Trim().TrimEnd(','), parameters);
        }

        public List<Entities.Media.MediaFansArea> GetList(MediaQuery<Entities.Media.MediaFansArea> query)
        {
            var sql = @"SELECT  MF.* ,
                                AI.AreaName AS ProvinceName
                        FROM    dbo.Media_FansArea AS MF WITH ( NOLOCK )
                                LEFT JOIN dbo.AreaInfo AS AI WITH ( NOLOCK ) ON MF.ProvinceID = AI.AreaID
                        WHERE   1= 1 ";
            var paras = new List<SqlParameter>();

            if (query.MediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MF.MediaId = @MediaId";
                paras.Add(new SqlParameter("@MediaId", query.MediaId));
            }

            if (query.MediaType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MF.MediaType = @MediaType";
                paras.Add(new SqlParameter("@MediaType", query.MediaType));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                sql += " ORDER BY " + query.OrderBy;
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Media.MediaFansArea>(data.Tables[0]);
        }

        public List<Entities.Media.MediaFansArea> GetListByWeiXinAuth(MediaQuery<Entities.Media.MediaFansArea> query)
        {
            var sql = @"SELECT  FW.RecID ,
                                FW.WxID AS WxID ,
                                FW.StatisticType AS MediaType ,
                                FW.StatisticKey AS ProvinceID ,
                                FW.StatisticValue AS UserScale ,
                                AI.AreaName AS ProvinceName
                        FROM    dbo.Fans_Weixin AS FW WITH ( NOLOCK )
                                LEFT JOIN dbo.AreaInfo AS AI WITH ( NOLOCK ) ON FW.StatisticKey = AI.AreaID
                        WHERE   1 = 1";
            var paras = new List<SqlParameter>();

            if (query.WxId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND FW.WxID = @WxID";
                paras.Add(new SqlParameter("@WxID", query.WxId));
            }

            if ((int)query.StatisticType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND FW.StatisticType = @StatisticType";
                paras.Add(new SqlParameter("@StatisticType", (int)query.StatisticType));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                sql += " ORDER BY " + query.OrderBy;
            }
            else
            {
                sql += " ORDER BY FW.StatisticValue DESC ";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Media.MediaFansArea>(data.Tables[0]);
        }
    }
}