using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.UserManage
{
    public class LE_WeiXinUser : DataBase
    {
        public readonly static LE_WeiXinUser Instance = new LE_WeiXinUser();

        public Entities.UserManage.LE_WeiXinUser GetModel(string openId)
        {
            var sbSql = new StringBuilder();

            sbSql.Append($@"
                            SELECT  WX.*
                            FROM    dbo.LE_WeiXinUser WX
                            WHERE   WX.openid = '{openId}'; ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToEntity<Entities.UserManage.LE_WeiXinUser>(ds.Tables[0]);
        }

        public int Insert(Entities.UserManage.LE_WeiXinUser model, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            INSERT dbo.LE_WeiXinUser
                                    ( subscribe ,
                                      openid ,
                                      nickname ,
                                      sex ,
                                      city ,
                                      country ,
                                      province ,
                                      language ,
                                      headimgurl ,
                                      subscribe_time ,
                                      unionid ,
                                      remark ,
                                      groupid ,
                                      tagid_list ,
                                      UserID ,
                                      CreateTime ,
                                      LastUpdateTime ,
                                      AuthorizeTime ,
                                      QRcode ,
                                      Inviter ,
                                      InvitationQR ,
                                      Status ,
                                      Source ,
                                      AdvertiserUserId
                                    )
                            VALUES  ( {model.subscribe} , -- subscribe - int
                                      N'{model.openid}' , -- openid - nvarchar(200)
                                      N'{model.nickname}' , -- nickname - nvarchar(200)
                                      {model.sex} , -- sex - int
                                      N'{model.city}' , -- city - nvarchar(20)
                                      N'{model.country}' , -- country - nvarchar(20)
                                      N'{model.province}' , -- province - nvarchar(20)
                                      N'{model.language}' , -- language - nvarchar(20)
                                      N'{model.headimgurl}' , -- headimgurl - nvarchar(500)
                                      '{model.subscribe_time}' , -- subscribe_time - datetime
                                      N'{model.unionid}' , -- unionid - nvarchar(200)
                                      N'{model.remark}' , -- remark - nvarchar(500)
                                      {model.groupid} , -- groupid - int
                                      N'{model.tagid_list}' , -- tagid_list - nvarchar(1000)
                                      {model.UserID} , -- UserID - int
                                      '{model.CreateTime}' , -- CreateTime - datetime
                                      '{model.LastUpdateTime}',
                                      '{model.AuthorizeTime}' , -- AuthorizeTime - datetime
                                      N'{model.QRcode}' , -- QRcode - nvarchar(500)
                                      N'{model.Inviter}' , -- Inviter - nvarchar(200)
                                      N'{model.InvitationQR}' , -- InvitationQR - nvarchar(500)
                                      {model.Status} , -- Status - int
                                      {model.Source} , -- Source - int
                                      {model.AdvertiserUserId}  -- AdvertiserUserId - int
                                    )");

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount;
        }

        public int UpdateForAndroid(Entities.UserManage.LE_WeiXinUser model)
        {
            var sql = $@"
                        UPDATE  dbo.LE_WeiXinUser
                        SET     country = @country ,
                                nickname = @nickname,
                                city = @city,
                                province = @province ,
                                language = @language,
                                headimgurl = @headimgurl ,
                                sex = @sex,
                                Status = 1,
                                LastUpdateTime = @LastUpdateTime
                        WHERE   openid = @openid;
                    ";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@country",model.country),
                        new SqlParameter("@nickname",model.nickname),
                        new SqlParameter("@city",model.city),
                        new SqlParameter("@province",model.province),
                        new SqlParameter("@language",model.language),
                        new SqlParameter("@headimgurl",model.headimgurl),
                        new SqlParameter("@sex",model.sex),
                        new SqlParameter("@LastUpdateTime",DateTime.Now),
                        new SqlParameter("@openid",model.openid),
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
    }
}
