using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WeChat
{
    public class LE_WXUserScene : DataBase
    {
        public static readonly LE_WXUserScene Instance = new LE_WXUserScene();

        public bool Insert(Entities.WeChat.LE_WXUserScene model)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                    INSERT dbo.LE_WXUserScene
                            ( UserID ,
                              SceneID ,
                              SceneName ,
                              CreateTime ,
                              Status
                            )
                    VALUES  ( {model.UserID} , -- UserID - int
                              {model.SceneID} , -- SceneID - int
                              N'{model.SceneName}' , -- SceneName - nvarchar(20)
                              GETDATE() , -- CreateTime - datetime
                              {model.Status}  -- Status - int
                            )");
            SqlParameter[] parameters = new SqlParameter[]
            {
                //new SqlParameter("@SceneID", entity.SceneID)
            };
            var obj = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return Convert.ToInt32(obj) > 0;
        }

        public int Delete(Entities.WeChat.LE_WXUserScene model, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                    DELETE FROM dbo.LE_WXUserScene WHERE RecID={model.RecID};");
            SqlParameter[] parameters = new SqlParameter[]
            {
                //new SqlParameter("@SceneID", entity.SceneID)
            };
            int obj = 0;
            if(trans==null)
                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            else
                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString(), parameters);
            return Convert.ToInt32(obj);
        }

        public Entities.WeChat.LE_WXUserScene GetModel(Entities.WeChat.LE_WXUserScene model)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                    SELECT  US.RecID ,
                            US.UserID ,
                            US.SceneID ,
                            US.SceneName ,
                            US.CreateTime ,
                            US.Status
                    FROM    dbo.LE_WXUserScene US
                    WHERE   US.SceneID = {model.SceneID}
                            AND US.UserID = {model.UserID};");
            SqlParameter[] parameters = new SqlParameter[]
            {
                //new SqlParameter("@SceneID", entity.SceneID)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return DataTableToEntity<Entities.WeChat.LE_WXUserScene>(ds.Tables[0]);
        }
    }
}
