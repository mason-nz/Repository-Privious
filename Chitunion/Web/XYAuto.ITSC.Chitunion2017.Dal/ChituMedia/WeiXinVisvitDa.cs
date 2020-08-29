using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class WeiXinVisvitDa : DataBase
    {
        #region 单例
        private WeiXinVisvitDa() { }

        static WeiXinVisvitDa instance = null;
        static readonly object padlock = new object();

        public static WeiXinVisvitDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WeiXinVisvitDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        /// <summary>
        /// 添加用户有效访问日志信息
        /// </summary>
        /// <param name="WXModel"></param>
        /// <returns></returns>
        public int AddWeiXinVisvitInfo(WeiXinVisvitModel WXModel)
        {
            #region (注释)逻辑修改前 每个用户、渠道、每天只存在一条数据
            //string InsertSQL = @"
            //                     DECLARE @ChannelID BIGINT= 0;

            //                     SELECT @ChannelID = DictID
            //                     FROM   dbo.LE_PromotionChannel_Dict
            //                     WHERE  ChannelCode = @ChannelCode;
            //                     IF @ChannelID > 0
            //                        BEGIN
            //                            IF NOT EXISTS ( SELECT  *
            //                                            FROM    dbo.LE_WeiXinVisvit_Log
            //                                            WHERE   UserID = @UserID
            //                                                    AND ChannelID = @ChannelID
            //                                                    AND 0 = DATEDIFF(DAY, LastUpdateTime,
            //                                                                     GETDATE())
            //                                                    AND Type = @Type )
            //                                INSERT  INTO dbo.LE_WeiXinVisvit_Log
            //                                        ( UserID ,
            //                                          ChannelID ,
            //                                          Url ,
            //                                          LastUpdateTime ,
            //                                          Type ,
            //                                          ChannelCode

            //                                        )
            //                                VALUES  ( @UserID ,
            //                                          @ChannelID ,
            //                                          @Url ,
            //                                          GETDATE() ,
            //                                          @Type ,
            //                                          @ChannelCode

            //                                        );

            //                            ELSE
            //                                 UPDATE dbo.LE_WeiXinVisvit_Log
            //                                 SET    LastUpdateTime = GETDATE(),
            //                                        Url = @Url
            //                                 WHERE  UserID = @UserID
            //                                        AND ChannelID = @ChannelID
            //                                        AND 0 = DATEDIFF(DAY, LastUpdateTime, GETDATE())
            //                                        AND Type = @Type; 
            //                        END;";
            #endregion

            Loger.Log4Net.Info($"渠道推广码参数：{WXModel.ChannelCode}");

            string InsertSQL = @"
                                 DECLARE @ChannelID BIGINT= 0;

                                 SELECT @ChannelID = DictID
                                 FROM   dbo.LE_PromotionChannel_Dict
                                 WHERE  ChannelCode = @ChannelCode;
                                 IF @ChannelID > 0
                                    BEGIN
                                            INSERT  INTO dbo.LE_WeiXinVisvit_Log
                                                    ( UserID ,
                                                      ChannelID ,
                                                      Url ,
                                                      LastUpdateTime ,
                                                      Type ,
                                                      ChannelCode ,
                                                      UserAgent
                                                    )
                                            VALUES  ( @UserID ,
                                                      @ChannelID ,
                                                      @Url ,
                                                      GETDATE() ,
                                                      @Type ,
                                                      @ChannelCode ,
                                                      @UserAgent
                                                    );
                                    END;";



            var sqlParams = new SqlParameter[] {
                            new SqlParameter("@UserID", WXModel.UserID),
                            new SqlParameter("@ChannelCode", WXModel.ChannelCode),
                            new SqlParameter("@Url", WXModel.Url),
                            new SqlParameter("@Type", WXModel.Type),
                            new SqlParameter("@UserAgent", WXModel.UserAgent)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, InsertSQL, sqlParams);
        }

        public List<Entities.LE_PromotionChannel_Dict.LE_PromotionChannel_Dict> GetDictList()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"SELECT DictID,ChannelName,ChannelCode FROM dbo.LE_PromotionChannel_Dict;");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToList<Entities.LE_PromotionChannel_Dict.LE_PromotionChannel_Dict>(ds.Tables[0]);
        }
        /// <summary>
        /// 根据渠道编码获取渠道ID
        /// </summary>
        /// <param name="ChannelCode"></param>
        /// <returns></returns>
        public Int64 GetByChanneID(string ChannelCode)
        {
            string selectSQL = @"SELECT DictID FROM dbo.LE_PromotionChannel_Dict WHERE ChannelCode=@ChannelCode";

            var sqlParams = new SqlParameter[] {
                            new SqlParameter("@ChannelCode", ChannelCode)
            };
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, selectSQL, sqlParams);
            return obj == null ? 0 : Convert.ToInt64(obj.ToString() == "" ? 0 : obj);

        }
    }
}
