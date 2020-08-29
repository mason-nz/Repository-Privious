using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //微信分享明细表
    public partial class LeShareDetail : DataBase
    {


        public static readonly LeShareDetail Instance = new LeShareDetail();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeShareDetail entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_ShareDetail(");
            strSql.Append("Type,ShareURL,OrderCoding,CategoryID,ShareResult,IP,Status,CreateUserID,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@Type,@ShareURL,@OrderCoding,@CategoryID,@ShareResult,@IP,@Status,@CreateUserID,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Type",entity.Type),
                        new SqlParameter("@ShareURL",entity.ShareURL),
                        new SqlParameter("@OrderCoding",entity.OrderCoding),
                        new SqlParameter("@CategoryID",entity.CategoryId),
                        new SqlParameter("@ShareResult",entity.ShareResult),
                        new SqlParameter("@IP",entity.IP),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateUserID",entity.CreateUserId),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public bool IsExist(int userId, LeShareDetailTypeEnum type)
        {
            var sql = $@"
				  SELECT COUNT(*) FROM dbo.LE_ShareDetail
				  WHERE Type = {(int)type} AND CreateUserID = {userId}
                ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public bool IsExistWithdrawas(int userId)
        {
            var sql = $@"
				  SELECT COUNT(*) FROM dbo.LE_ShareDetail
				  WHERE Type = {(int)LeShareDetailTypeEnum.提现分享} AND CreateUserID = {userId} AND DATEDIFF(DAY,CreateTime,'{DateTime.Now}') = 0
                ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj != null && Convert.ToInt32(obj) > 0;
        }
    }
}

