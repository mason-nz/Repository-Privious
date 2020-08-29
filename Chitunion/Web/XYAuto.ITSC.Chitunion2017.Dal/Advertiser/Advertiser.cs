using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Advertiser
{
    public class Advertiser : DataBase
    {
        public static readonly Advertiser Instance = new Advertiser();
        /// <summary>
        /// zlb 2017-07-21
        /// 查询广告主列表
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Mobile"></param>
        /// <param name="TrueName"></param>
        /// <param name="UserSource"></param>
        /// <param name="Status"></param>
        /// <param name="UserType"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="CityID"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectAdveristerList(string UserName, string Mobile, string TrueName, int UserSource, int Status, int UserType, int ProvinceID, int CityID, string StarDate, string EndDate, int PageIndex, int PageSize)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@Mobile", SqlDbType.VarChar,20),
                    new SqlParameter("@TrueName", SqlDbType.VarChar,50),
                    new SqlParameter("@UserSource", SqlDbType.Int),
                    new SqlParameter("@Status", SqlDbType.Int),
                    new SqlParameter("@UserType", SqlDbType.Int),
                    new SqlParameter("@ProvinceID", SqlDbType.Int),
                    new SqlParameter("@CityID", SqlDbType.Int),
                    new SqlParameter("@StarDate", SqlDbType.VarChar,20),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,20),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = UserName;
            parameters[1].Value = Mobile;
            parameters[2].Value = TrueName;
            parameters[3].Value = UserSource;
            parameters[4].Value = Status;
            parameters[5].Value = UserType;
            parameters[6].Value = ProvinceID;
            parameters[7].Value = CityID;
            parameters[8].Value = StarDate;
            parameters[9].Value = EndDate;
            parameters[10].Value = PageIndex;
            parameters[11].Value = PageSize;
            parameters[12].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectAdveristerList", parameters);
            int totalCount = (int)(parameters[12].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-07-21
        /// 批量启用禁用用户
        /// </summary>
        /// <param name="UserIDList">用户ID信息</param>
        /// <param name="Status">启用：0，禁用：1</param>
        /// <returns></returns>
        public int UpdateUserStatusInfo(string UserIDList, int Status)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE  dbo.UserInfo set Status={0} WHERE UserID IN ({1})", Status, UserIDList);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
        /// <summary>
        /// zlb 2017-07-21
        /// 批量重置用户密码
        /// </summary>
        /// <param name="UserIDList">用户ID信息</param>
        /// <param name="Pwd">用户密码</param>
        /// <returns></returns>
        public int UpdateUserPwdInfo(string UserIDList, string  Pwd)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE  dbo.UserInfo set Pwd='{0}' WHERE UserID IN ({1})", Pwd, UserIDList);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
    }
}
