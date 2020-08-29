using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.EmployeeInfo
{
    public class EmployeeInfo : DataBase
    {
        public static readonly EmployeeInfo Instance = new EmployeeInfo();
        /// <summary>
        /// zlb 2017-07-20 查询Category角色下的用户名称是否存在
        /// </summary>
        /// <param name="UserName">用户名称</param>
        /// <param name="Category">29001 广告主 29002媒体主 29003内部员工</param>
        /// <returns></returns>
        public int GetUserNameCount(string UserName, int Category)
        {
            string sql = string.Format("SELECT COUNT(1)  FROM UserInfo WHERE  UserName='{0}' AND Category={1}", UserName, Category);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// masj 2017-12-15 查询Category角色下的手机号是否存在
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="Category">29001 广告主 29002媒体主 29003内部员工</param>
        /// <returns></returns>
        public int GetMobileCount(string mobile, int Category)
        {
            string sql = string.Format("SELECT COUNT(1)  FROM UserInfo WHERE  Mobile='{0}' AND Category={1}", mobile, Category);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-07-20 添加内部用户信息
        /// </summary>
        /// <param name="EmployeeNumber">员工编号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Mobile">手机号</param>
        /// <param name="Pwd">密码</param>
        /// <param name="Email">邮箱</param>
        /// <param name="SysUserID">内部用户ID</param>
        /// <param name="CreateUserID">创建人</param>
        /// <returns></returns>
        public int InsertUserInfo(string EmployeeNumber, string UserName, string Mobile, string Pwd, string Email, int SysUserID, int CreateUserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@EmployeeNumber", SqlDbType.VarChar,20),
                    new SqlParameter("@UserName",  SqlDbType.VarChar,50),
                    new SqlParameter("@Mobile", SqlDbType.VarChar,20),
                    new SqlParameter("@Pwd", SqlDbType.VarChar,50),
                    new SqlParameter("@Email",SqlDbType.VarChar,50),
                    new SqlParameter("@SysUserID",SqlDbType.Int),
                    new SqlParameter("@CreateUserID",SqlDbType.Int)
                    };
            parameters[0].Value = EmployeeNumber;
            parameters[1].Value = UserName;
            parameters[2].Value = Mobile;
            parameters[3].Value = Pwd;
            parameters[4].Value = Email;
            parameters[5].Value = SysUserID;
            parameters[6].Value = CreateUserID;
            DateTime dtNow = DateTime.Now;
            string strSql = string.Format(@"insert into UserInfo (UserName,Mobile,Pwd,EmployeeNumber,Email,SysUserID,CreateUserID,LastUpdateUserID,Type,Category,Source,IsAuthAE,Status,CreateTime,LastUpdateTime,IsAuthMTZ) values (
              @UserName,@Mobile,@Pwd,@EmployeeNumber,@Email,@SysUserID,@CreateUserID,@CreateUserID,{0},{1},{2},{3},{4},'{5}','{5}',{6});select @@identity", UserConstant.Type, UserConstant.Category, UserConstant.Source, UserConstant.IsAuthAE, UserConstant.Status, dtNow, UserConstant.IsAuthMTZ);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// masj 2017-12-15 添加内部用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Mobile"></param>
        /// <param name="Pwd"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public int InsertUserInfo(string UserName, string Mobile, string Pwd, int categoryID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.VarChar),
                    new SqlParameter("@Mobile", SqlDbType.VarChar),
                    new SqlParameter("@Pwd", SqlDbType.VarChar),
                    new SqlParameter("@CategoryID",SqlDbType.Int)
                    };
            parameters[0].Value = UserName;
            parameters[1].Value = Mobile;
            parameters[2].Value = Pwd;
            parameters[3].Value = categoryID;
            //DateTime dtNow = DateTime.Now;
            string strSql = @"INSERT dbo.UserInfo
        (UserName,
          Mobile,
          Pwd,
          Type,
          Source,
          IsAuthMTZ,
          AuthAEUserID,
          IsAuthAE,
          SysUserID,
          EmployeeNumber,
          Status,
          CreateTime,
          CreateUserID,
          LastUpdateTime,
          LastUpdateUserID,
          Category,
          Email,
          LockState,
          SleepState,
          LockType,
          SleepStatus
        )
VALUES(@UserName, --UserName - varchar(50)
          @Mobile, --Mobile - varchar(20)
          @Pwd, --Pwd - varchar(50)
          1002, --Type - int
          3002, --Source - int
          0, --IsAuthMTZ - bit
          0, --AuthAEUserID - int
          0, --IsAuthAE - bit
          0, --SysUserID - int
          NULL, --EmployeeNumber - varchar(20)
          0, --Status - int
          GETDATE(), --CreateTime - datetime
          0, --CreateUserID - int
          GETDATE(), --LastUpdateTime - datetime
          0, --LastUpdateUserID - int
          @CategoryID, --Category - int
          NULL, --Email - varchar(50)
          0, --LockState - int
          0, --SleepState - int
          0, --LockType - int
          0-- SleepStatus - int
        )
SELECT @@IDENTITY;";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }


        /// <summary>
        /// zlb 2017-07-20 增加内部用户角色和详情
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="TrueName">真实姓名</param>
        /// <param name="RoleID">角色ID</param>
        /// <param name="SysID">系统ID</param>
        /// <param name="CreateUserID">创建人ID</param>
        /// <returns></returns>
        public int InsertUserDetailAndRoleInfo(int UserID, string TrueName, string RoleID, string SysID, int CreateUserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@TrueName", SqlDbType.VarChar,200),
                    new SqlParameter("@RoleID",SqlDbType.VarChar,50),
                    new SqlParameter("@SysID",SqlDbType.VarChar,50),
                    new SqlParameter("@CreateUserID",SqlDbType.Int)
                    };
            parameters[0].Value = UserID;
            parameters[1].Value = TrueName;
            parameters[2].Value = RoleID;
            parameters[3].Value = SysID;
            parameters[4].Value = CreateUserID;
            DateTime dtNow = DateTime.Now;
           
            var strSql = $@"
IF(NOT EXISTS(SELECT 1 FROM DBO.UserRole WITH(NOLOCK) WHERE UserID = {UserID} AND RoleID = '{RoleID}'))
BEGIN
    INSERT INTO DBO.UserRole
            ( UserID ,
              RoleID ,
              SysID ,
              Status ,
              CreateTime ,
              CreateUserID
            )
    VALUES  ( @UserID , -- UserID - int
              @RoleID , -- RoleID - varchar(50)
              @SysID , -- SysID - varchar(50)
              {UserConstant.Status} , -- Status - int
              '{dtNow}' , -- CreateTime - datetime
              @CreateUserID  -- CreateUserID - int
            )
END
";
            
            //strSql += string.Format(";insert into UserDetailInfo (UserID,TrueName,CreateUserID,LastUpdateUserID,Status,CreateTime,LastUpdateTime) values (@UserID,@TrueName,@CreateUserID,@CreateUserID,{0},'{1}','{1}')", UserConstant.Status, dtNow);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }
        /// <summary>
        /// zlb 2017-07-24
        /// 根据用户ID查询用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectUserInfoByUserID(int UserID)
        {
            string strSql = "SELECT TOP 1 U.UserID,U.EmployeeNumber,D.TrueName,U.Mobile,U.Email,U.UserName,R.RoleID FROM UserInfo U LEFT JOIN dbo.UserRole R ON U.UserID=R.UserID LEFT JOIN dbo.UserDetailInfo D ON U.UserID=D.UserID WHERE U.UserID=" + UserID;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-07-24
        /// 根据用户ID修改角色
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RoleID">角色</param>
        /// <returns></returns>
        public int UpdateRolIDByUserID(int UserID, string RoleID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.VarChar,50)

                    };
            parameters[0].Value = UserID;
            parameters[1].Value = RoleID;
            string strSql = "UPDATE dbo.UserRole SET RoleID=@RoleID WHERE UserID=@UserID";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }

        /// <summary>
        /// 根据用户分类、手机号，重置账户密码
        /// </summary>
        /// <param name="Category">用户分类（广告主：29001，媒体主：29002）</param>
        /// <param name="Mobile">手机号</param>
        /// <param name="NewPwd">新密码</param>
        /// <returns>成功大于0，否则失败</returns>
        public int UpdatePwdByCategoryAndMobile(int Category, string Mobile,string NewPwd)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Category", SqlDbType.Int),
                    new SqlParameter("@Pwd", SqlDbType.VarChar),
                    new SqlParameter("@Mobile", SqlDbType.VarChar)
                    };
            parameters[0].Value = Category;
            parameters[1].Value = NewPwd;
            parameters[2].Value = Mobile;
            string strSql = @"UPDATE dbo.UserInfo
SET Pwd = @Pwd
WHERE Status = 0 AND Category = @Category AND Mobile = @Mobile";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }
    }
}
