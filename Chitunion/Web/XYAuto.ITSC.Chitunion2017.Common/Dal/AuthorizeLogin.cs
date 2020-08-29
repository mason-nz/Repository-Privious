using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Common.Dal
{
     public class AuthorizeLogin:DataBase
    {
        public readonly static AuthorizeLogin Instance = new AuthorizeLogin();

        public void Insert(Entities.AuthorizeLogin model)
        {
            string sqlstr = @"INSERT dbo.AuthorizeLogin_Token
                                    ( APPID ,
                                      IP ,
                                      TimeStamp ,
                                      MD5Code ,
                                      Status ,
                                      CreateTime ,
                                      ModifyTime
                                    )
                            VALUES  ( @APPID , -- APPID - int
                                      @IP , -- IP - varchar(20)
                                      @TimeStamp , -- TimeStamp - timestamp
                                      @MD5Code , -- MD5Code - varchar(200)
                                      0 , -- Status - int
                                      @CreateTime , -- CreateTime - datetime
                                      @ModifyTime  -- ModifyTime - datetime
                                    )";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@APPID",model.APPID),
                new SqlParameter("@IP",model.IP),
                new SqlParameter("@TimeStamp",model.TimeStamp),
                new SqlParameter("@MD5Code",model.MD5Code),
                new SqlParameter("@CreateTime",model.CreateTime),
                new SqlParameter("@ModifyTime",model.ModifyTime)
            };
            SqlHelper.ExecuteNonQuery(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }

        public bool Verification(int appid,string accessToken)
        {
            string sqlstr = @"SELECT  COUNT(1)
                                FROM    dbo.AuthorizeLogin_Token
                                WHERE   Status=0
                                        AND MD5Code = @MD5Code
                                        AND APPID = @APPID
                                        AND @CurrentTime <= DATEADD(MINUTE, 5, CreateTime)";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@APPID",appid),
                new SqlParameter("@MD5Code",accessToken),
                new SqlParameter("@CurrentTime",DateTime.Now)
            };
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        public bool Verification(string accessToken)
        {
            string sqlstr = @"SELECT  COUNT(1)
                                FROM    dbo.AuthorizeLogin_Token
                                WHERE   Status=0
                                        AND MD5Code = @MD5Code
                                        AND @CurrentTime <= DATEADD(MINUTE, 5, CreateTime)";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@MD5Code",accessToken),
                new SqlParameter("@CurrentTime",DateTime.Now)
            };
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        public void Update(Entities.AuthorizeLogin model)
        {
            string sqlstr = @"UPDATE  dbo.AuthorizeLogin_Token
                                SET     Status = -1 ,
                                        ModifyTime = @CurrentTime
                                WHERE   APPID = @APPID
                                        AND MD5Code = @MD5Code";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@APPID",model.APPID),
                new SqlParameter("@MD5Code",model.MD5Code),
                new SqlParameter("@CurrentTime",DateTime.Now)
            };
            SqlHelper.ExecuteNonQuery(SYSCONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }

        public int p_UserBroker_Insert(Dictionary<string, string> dict,out string username)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Msg",SqlDbType.VarChar,200),
                new SqlParameter("@BrokerUserID",Convert.ToInt32(dict["UserId"])),
                new SqlParameter("@EnterpriseName",dict["EnterpriseName"]),
                new SqlParameter("@Contact",dict["Contact"]),
                new SqlParameter("@Mobile",dict["Mobile"]),
                new SqlParameter("@RETURN_VALUE",SqlDbType.Int),
                new SqlParameter("@businessLicence",dict["businessLicence"])
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[5].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(SYSCONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserBroker_Insert", parameters);
            username = (string)parameters[0].Value;
            return (int)parameters[5].Value;
        }

        public int p_UserBroker_Update(int userID, string mobile, out string msg)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Msg",SqlDbType.VarChar,200),
                new SqlParameter("@BrokerUserID",userID),
                new SqlParameter("@Mobile",mobile),
                new SqlParameter("@RETURN_VALUE",SqlDbType.Int)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[3].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(SYSCONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserBroker_Update", parameters);
            msg = (string)parameters[0].Value;
            return (int)parameters[3].Value;
        }
    }
}
