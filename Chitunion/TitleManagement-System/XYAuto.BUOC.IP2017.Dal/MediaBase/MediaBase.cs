using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.MediaBase;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.MediaBase
{
    public class MediaBase : DataBase
    {
        public static readonly MediaBase Instance = new MediaBase();
        private static Encoding _encoding = System.Text.Encoding.GetEncoding("GB2312");
        #region APP媒体操作
        /// <summary>
        /// zlb 2017-11-06
        /// 查询全部app媒体名称集合
        /// </summary>
        /// <returns></returns>
        public DataTable SelectMediaAPP()
        {
            string strSql = $"SELECT Name from Chitunion2017.dbo.Media_BasePCAPP ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql).Tables[0];
        }
        public int InsertMediaApp(MediaApp mediaApp, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Remark", SqlDbType.VarChar,500)

                    };
            int leng = GetLength(mediaApp.Remark);
            if (leng > 500)
            {
                byte[] bytes = _encoding.GetBytes(mediaApp.Remark);
                mediaApp.Remark = _encoding.GetString(bytes, 0, 500);
            }
            parameters[0].Value = mediaApp.Remark;
            string strSql = $@"insert into  dbo.Media_BasePCAPP
        ( Name ,
          HeadIconURL ,
          ProvinceID ,
          CityID ,
          DailyLive ,
          Remark ,
          Status ,
          CreateTime ,
          CreateUserID ,
          LastUpdateTime ,
          LastUpdateUserID
        )
VALUES  ( '{mediaApp.Name}' , -- Name - varchar(200)
          '{mediaApp.HeadIconURL}' , -- HeadIconURL - varchar(200)
          -2 , -- ProvinceID - int
          -2 , -- CityID - int
          {mediaApp.DailyLive} , -- DailyLive - int
          @Remark , -- Remark - varchar(500)
          0 , -- Status - int
          GETDATE() , -- CreateTime - datetime
          {UserID} , -- CreateUserID - int
          GETDATE() , -- LastUpdateTime - datetime
          {UserID}  -- LastUpdateUserID - int
        );SELECT @@IDENTITY";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        #endregion
        #region 微信媒体操作

        public DataTable SelectMediaWechat()
        {
            string strSql = $"SELECT WxNumber from Chitunion2017.dbo.Weixin_OAuth ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql).Tables[0];
        }

        public int InsertMediaWechat(MediaWechat mediaWechat, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Sign", SqlDbType.VarChar,500)

                    };
            parameters[0].Value = mediaWechat.Sign;
            string strSql = $@"INSERT INTO dbo.Weixin_OAuth
        (
          WxNumber ,--微信账号
          NickName ,--名称
          ServiceType ,--服务类型默认-2
		  ProvinceID, --省默认-2
		  CityID, --市默认-2
		  FansCount, --粉丝数
		  IsAreaMedia, --是否区域媒体
		  IsVerify, --是否认证
		  LevelType,  --媒体级别
          Sign
        )
VALUES  ( '{mediaWechat.WxNumber}',
'{mediaWechat.NickName}',
{mediaWechat.ServiceType},
{mediaWechat.ProvinceID},
{mediaWechat.CityID},
{mediaWechat.FansCount},
{mediaWechat.IsAreaMedia},
{mediaWechat.IsVerify},
{mediaWechat.LevelType},
@Sign
        );SELECT @@IDENTITY";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        #endregion
        #region 头条媒体操作
        public DataTable SelectMediaTouTiao()
        {
            string strSql = $"SELECT Url,HeadImg,UserName from Chitunion2017.dbo.Media_TouTiao";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql).Tables[0];
        }
        public int InsertMediaTouTiao(MediaTouTiao TouTiao, int UserID)
        {
            string strSql = $@"
INSERT INTO dbo.Media_TouTiao
        ( Url ,
          UserID ,
          UserName ,
          Abstract ,
          FollowCount ,
          FansCount ,
          HeadImg ,
          CreateTime ,
          LastUpdateTime ,
          UStatus ,
          CreateUserID
        )
VALUES  ( '{TouTiao.Url}' , -- Url - varchar(512)
          '' , -- UserID - varchar(64)
          '{TouTiao.UserName}' , -- UserName - varchar(64)
          '' , -- Abstract - varchar(1024)
          0 , -- FollowCount - int
          0 , -- FansCount - int
          '' , -- HeadImg - varchar(512)
          GETDATE() , -- CreateTime - datetime
          GETDATE() , -- LastUpdateTime - datetime
          0 , -- UStatus - int
          {UserID}  -- CreateUserID - int
        );SELECT @@IDENTITY";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        #endregion

        #region 分类操作
        public DataTable SelectMediaCategory(int CategoryType)
        {
            string strSql = $"select DictId,DictName from DictInfo where dicttype={CategoryType}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql).Tables[0];
        }
        public int InsertMediaCategory(MediaCategory mediaCategory)
        {
            string strSql = $@"
		  INSERT INTO  dbo.MediaCategory
		          ( MediaType ,
		            WxID ,
		            CategoryID ,
		            SortNumber
		          )
		  VALUES  ( {mediaCategory.MediaType} , -- MediaType - int
		            {mediaCategory.WxID} , -- WxID - int
		            {mediaCategory.CategoryID} , -- CategoryID - int
		             {mediaCategory.SortNumber}  -- SortNumber - int
		          )";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql);
        }
        #endregion
        /// <summary>
        /// zlb 2017-11-06
        /// 获取文本字节长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }
    }
}
