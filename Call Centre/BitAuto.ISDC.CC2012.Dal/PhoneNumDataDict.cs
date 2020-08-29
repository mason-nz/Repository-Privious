using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    /// <summary>
    /// 电话号码所在地区
    /// </summary>
    public class PhoneNumDataDict : DataBase
    {
        /// <summary>
        /// 根据电话号码获取地区ID
        /// </summary>
        /// <param name="AreaNo"></param>
        /// <param name="ProvinceID"></param>
        /// <param name="CityID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void GetAreaID(string AreaNo, out int ProvinceID, out int CityID, out string msg)
        {
            msg = "";
            ProvinceID = 0;
            CityID = 0;

            #region 查询对应的归属地

            string SqlStr = "select top 1 ProviceID,AreaID from dbo.PhoneNumDataDict where PhonePrefix =@PhonePrefix or DistrictNum=@DistrictNum";
            SqlParameter[] parameters = {
                new SqlParameter("@PhonePrefix", SqlDbType.VarChar,10),
                new SqlParameter("@DistrictNum", SqlDbType.VarChar,10)
            };
            parameters[0].Value = AreaNo;
            parameters[1].Value = AreaNo;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SqlStr, parameters);

            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
            {
                int.TryParse(ds.Tables[0].Rows[0]["ProviceID"].ToString(), out ProvinceID);
                int.TryParse(ds.Tables[0].Rows[0]["AreaID"].ToString(), out CityID);
            }

            #endregion
        }

        public static int Add(Entities.PhoneNumDataDict model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PhonePrefix", SqlDbType.VarChar,10),
					new SqlParameter("@DistrictNum", SqlDbType.VarChar,10),
					new SqlParameter("@AreaName", SqlDbType.VarChar,100),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@AreaPid", SqlDbType.Int,4),
					new SqlParameter("@AreaLevel", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ProviceID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PhonePrefix;
            parameters[2].Value = model.DistrictNum;
            parameters[3].Value = model.AreaName;
            parameters[4].Value = model.AreaID;
            parameters[5].Value = model.AreaPid;
            parameters[6].Value = model.AreaLevel;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.ProviceID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PhoneNumDataDict_Insert", parameters);
            return (int)parameters[0].Value;

        }

        /// <summary>
        /// 根据城市名模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="AreaID"></param>
        /// <param name="AreaLevel"></param>
        /// <param name="AreaPid"></param>
        /// <param name="ProviceID"></param>
        /// <returns></returns>
        public static string GetAreaInfoByName(string name, out int AreaID, out int AreaLevel, out int AreaPid, out int ProviceID)
        {
            string retname = "";
            AreaID = 0;
            AreaLevel = 0;
            AreaPid = 0;
            ProviceID = 0;

            string SqlStr = "SELECT top 1 AreaID,Level,PID FROM CRM2009.dbo.AreaInfo WHERE AreaName LIKE '" + StringHelper.SqlFilter(name) + "%' AND Level=2";
            
           

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, SqlStr);

            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
            {
                //在二级城市中找到了

                int.TryParse(ds.Tables[0].Rows[0]["AreaID"].ToString(), out AreaID);
                int.TryParse(ds.Tables[0].Rows[0]["Level"].ToString(), out AreaLevel);
                int.TryParse(ds.Tables[0].Rows[0]["PID"].ToString(), out AreaPid);
                int.TryParse(ds.Tables[0].Rows[0]["PID"].ToString(), out ProviceID);
            }
            else
            {
                //没有在二级城市中找到，去三级城市去找
                SqlStr = "SELECT top 1 AreaID,Level,PID FROM CRM2009.dbo.AreaInfo WHERE AreaName  LIKE '" + StringHelper.SqlFilter(name) + "%' AND Level=3";
                ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, SqlStr);
                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int.TryParse(ds.Tables[0].Rows[0]["AreaID"].ToString(), out AreaID);
                    int.TryParse(ds.Tables[0].Rows[0]["Level"].ToString(), out AreaLevel);
                    int.TryParse(ds.Tables[0].Rows[0]["PID"].ToString(), out AreaPid);


                    //查询省份ID
                    SqlStr = "SELECT top 1 AreaID FROM CRM2009.dbo.AreaInfo WHERE AreaID=" + AreaPid.ToString();
                    ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, SqlStr);
                    if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int.TryParse(ds.Tables[0].Rows[0]["AreaID"].ToString(), out ProviceID);
                    }
                }
            }

            return retname;
        }
    }
}
