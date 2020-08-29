/********************************************************
*创建人：hantao
*创建时间：2017/10/11
*说明：广点通同步用户信息
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    public class GDTDeriveUserInfo:DataBase
    {
        #region Instance
        public static readonly GDTDeriveUserInfo Instance = new GDTDeriveUserInfo();
        #endregion

        public int Insert(Entities.GDT.GDTDeriveUserInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("INSERT INTO [dbo].[GDT_DeriveUserInfo](");
            strSql.Append("[ID],[Name],[Phone],[Brand],[Province],[City],[Dealer],[CarType],[CarModel],[OnLikeType],[DeviceNumber],[Time],[VisitIP],[SourceUrl],[CreateTime]");
            strSql.Append(") VALUES (");
            strSql.Append("@ID,@Name,@Phone,@Brand,@Province,@City,@Dealer,@CarType,@CarModel,@OnLikeType,@DeviceNumber,@Time,@VisitIP,@SourceUrl,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";SELECT SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@ID",entity.ID),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@Phone",entity.Phone),
                        new SqlParameter("@Brand",entity.Brand),
                        new SqlParameter("@Province",entity.Province),
                        new SqlParameter("@City",entity.City),
                        new SqlParameter("@Dealer",entity.Dealer),
                        new SqlParameter("@CarType",entity.CarType),
                        new SqlParameter("@CarModel",entity.CarModel),
                        new SqlParameter("@OnLikeType",entity.OnLikeType),
                        new SqlParameter("@DeviceNumber",entity.DeviceNumber),
                        new SqlParameter("@Time",entity.Time),
                        new SqlParameter("@VisitIP",entity.VisitIP),
                        new SqlParameter("@SourceUrl",entity.SourceUrl),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                       
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int SelectMaxId()
        {
            string sql = "SELECT ISNULL(MAX(ID),0) AS ID FROM [GDT_DeriveUserInfo]";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }


        public void UpdaetDeriveUserInfoByKeyId(int deriveUserInfoId,int status)
        {
            string sql = $@"UPDATE [dbo].[GDT_DeriveUserInfo] SET [Status] = {status} WHERE [DeriveUserInfoId] = {deriveUserInfoId}";
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public void UpdaetDeriveUserInfoByClueId(int clueId, int status)
        {
            string sql = $@"UPDATE [dbo].[GDT_DeriveUserInfo] SET [Status] = {status} WHERE ID = {clueId}";
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public DataSet GetClueIds(int status,DateTime time)
        {
            string sql = $@"SELECT [ID] FROM [dbo].[GDT_DeriveUserInfo] WHERE Status = {status} AND CreateTime>'{time}'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);

        }
    }
}
