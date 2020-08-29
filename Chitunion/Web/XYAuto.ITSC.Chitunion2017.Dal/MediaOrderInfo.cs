using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class MediaOrderInfo : DataBase
    {
        public static readonly MediaOrderInfo Instance = new  MediaOrderInfo();
        #region 新增媒体项目中间表
        public int Insert(Entities.MediaOrderInfo model)
        {
            string sqlstr = @"INSERT dbo.MediaOrderInfo
                                    ( OrderID ,
                                      MediaType ,
                                      Note ,
                                      UploadFileURL ,
                                      CreateTime ,
                                      CreateUserID
                                    )
                            VALUES  ( @OrderID , -- OrderID - varchar(20)
                                      @MediaType , -- MediaType - int
                                      @Note , -- Note - varchar(1000)
                                      @UploadFileURL , -- UploadFileURL - varchar(200)
                                      GETDATE() , -- CreateTime - datetime
                                      @CreateUserID  -- CreateUserID - int
                                    );
                            SELECT @@IDENTITY;";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",model.OrderID),
                new SqlParameter("@MediaType",model.MediaType),
                new SqlParameter("@Note",model.Note),
                new SqlParameter("@UploadFileURL",model.UploadFileURL),
                new SqlParameter("@CreateUserID",model.CreateUserID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            if (ds.Tables[0]?.Rows.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            return -2;
        }
        #endregion
        #region 根据项目号删除
        public void DeleteByOrderID(string orderid)
        {
            string sqlstr = @"DELETE FROM dbo.ADOrderInfo WHERE OrderID=@OrderID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderid)
            };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS,CommandType.Text,sqlstr,parameters);
        }
        #endregion
    }
}
