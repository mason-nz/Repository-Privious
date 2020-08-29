using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    //智慧云推送过来的广告主用户与广告运营角色绑定关系，告运营角色->（一对多）广告主
    public partial class GdtRoleUser : DataBase
    {
        public static readonly GdtRoleUser Instance = new GdtRoleUser();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(XYAuto.ITSC.Chitunion2017.Entities.GDT.GdtRoleUser entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_RoleUser(");
            strSql.Append("UserId,AuthToUserId,CreateUserId,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@UserId,@AuthToUserId,@CreateUserId,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserId",entity.UserId),
                        new SqlParameter("@AuthToUserId",entity.AuthToUserId),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }
    }
}