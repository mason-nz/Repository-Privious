using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.APP;
using XYAuto.Utils;
using XYAuto.Utils.Config;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.APP
{
    public class FeedbackDa: DataBase
    {
        #region 单例
        private FeedbackDa() { }

        static FeedbackDa instance = null;
        static readonly object padlock = new object();

        public static FeedbackDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new FeedbackDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 添加反馈信息
        public int AddFeedbackInfo(FeedbackModel query)
        {
            string SQL = @" 
                         INSERT INTO dbo.LE_Feedback
                                ( UserID ,
                                  OpinionText ,
                                  CreateTime
                                )
                         VALUES ( @UserID ,
                                  @OpinionText ,
                                  GETDATE()
                                )";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",Common.UserInfo.GetLoginUser().UserID),
                new SqlParameter("@OpinionText",query.OpinionText)
            };

            return (int)SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
        }

        public int GetShareCount(int UserID)
        {
            string SQL = "SELECT COUNT(0) FROM dbo.LE_ShareDetail WHERE Type=202004 AND CreateUserID=@UserID";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };
            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

        }

        public int GetTaskNewID()
        {
            string SQL = "SELECT TOP 1 * FROM LE_TaskInfo WHERE 0>=DATEDIFF(DAY,EndTime,GETDATE())  ORDER BY NEWID()";
            //var sqlParams = new SqlParameter[]{
            //    new SqlParameter("@UserID",UserID)
            //};
            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, null);

        }
        /// <summary>
        /// 根据任务ID获取文章阅读数
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public int GetReadNum(int TaskID)
        {
            var randomNumber = ConfigurationUtil.GetAppSettingValue("GetRandomNumber", false) ?? "21,155.22";
            string SQL = "SELECT dbo.[f_GenArticleReadNum](CreateTime, @Jishu,@Xishu),* FROM dbo.LE_TaskInfo WHERE RecID=@TaskID";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@Jishu",randomNumber.Split(',')[0]),
                new SqlParameter("@Xishu",randomNumber.Split(',')[1]),
                new SqlParameter("@TaskID",TaskID)
            };
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams));

        }
        #endregion

    }
}
