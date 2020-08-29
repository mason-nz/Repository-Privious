using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.DataImport.DAL
{
    public class Util : XYAuto.ITSC.Chitunion2017.Dal.DataBase
    {
        public static readonly Util Instance = new Util();

        public int p_VerifyADTemplateByName(string name, int baseMediaID, bool isDelete = false)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Name",name),
                new SqlParameter("@RETURN_VALUE",SqlDbType.Int),
                new SqlParameter("@baseMediaID",baseMediaID),
                new SqlParameter("@isDelete",isDelete==true?1:0)
            };
            parameters[1].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_VerifyADTemplateByName", parameters);
            return (int)parameters[1].Value;
        }
        /// <summary>
        /// 根据模板ID，广告样式名称获取广告样式ID
        /// </summary>
        /// <param name="tempID">模板ID</param>
        /// <param name="name">样式名称</param>
        /// <returns>广告样式ID</returns>
        public int QueryADStyleRecid(int tempID,string name)
        {
            string sqlstr = @"SELECT RecID FROM dbo.App_AdTemplateStyle WHERE AdTemplateID=@AdTemplateID AND AdStyle=@AdStyle";            
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@AdStyle",name),
                new SqlParameter("@AdTemplateID",tempID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            if(ds.Tables[0].Rows.Count>0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            return -2;
        }

        public int QuerySaleAreaGroupID(int tempID, string name)
        {
            string sqlstr = @"SELECT GroupID FROM dbo.SaleAreaInfo WHERE TemplateID=@TemplateID AND GroupName=@GroupName";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TemplateID",tempID),
                new SqlParameter("@GroupName",name)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            if (ds.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            return -2;
        }

        public int QueryMediaIDAE(string name)
        {
            string sqlstr = @"SELECT TOP 1
                                    MediaID
                            FROM    dbo.Media_PCAPP
                                    JOIN dbo.UserInfo UI ON UI.UserID = dbo.Media_PCAPP.CreateUserID
                                    JOIN dbo.UserRole UR ON UI.UserID = UR.UserID
                            WHERE   dbo.Media_PCAPP.Status=0 AND UR.RoleID = 'SYS001RL00005' AND Name=@Name";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Name",name)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            if (ds.Tables[0].Rows.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            return -2;
        }
    }
}
