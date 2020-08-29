using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class SkillGroupDataRight : DataBase
    {
        #region Instance
        public static readonly SkillGroupDataRight Instance = new SkillGroupDataRight();
        #endregion

        public int UpdateUserSkillDataRight(int UserID, string SGIDAndPriority, int CreateUserID, int RegionID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@SGIDAndPriority", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@RegionID", SqlDbType.Int,4),
                    new SqlParameter("@backvalue", SqlDbType.Int,4)
                                        };
            parameters[0].Value = UserID;
            parameters[1].Value = SGIDAndPriority;
            parameters[2].Value = CreateUserID;
            parameters[3].Value = RegionID;
            parameters[4].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_UserSkillGroupDataRight_Update", parameters);
            return (int)parameters[4].Value;
        }

        //SELECT * FROM dbo.SkillGroup WHERE CDID=99 AND SGID<>1099

        public DataTable GetAutoCallSkillGroup()
        {
            string sqlStr = @" SELECT * FROM dbo.SkillGroup WHERE CDID=99 AND SGID <> 1099 ";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetAllHotlineSkillGroup()
        {
            string sqlStr = @"SELECT  sg.* ,
                                    CallNum
                            FROM   SkillGroup AS sg
                                    LEFT JOIN CallDisplay AS cd ON sg.CDID = cd.CDID
                            WHERE   sg.Status = 0 AND sg.Factory=1 
                            ORDER BY cd.OrderNum ASC,
                                    sg.SGID ASC";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public string GetUserSkillGroup(int UserID)
        {
            string strSql = @"SELECT ISNULL(STUFF(( SELECT    ',' + RTRIM(usgdr.SGID) + ';' + RTRIM(usgdr.SkillPriority)
                                  FROM   dbo.UserSkillDataRight usgdr INNER JOIN dbo.SkillGroup AS sg ON usgdr.SGID=sg.SGID
                                  WHERE  usgdr.UserID = @UserID
                                FOR
                                  XML PATH('')
                                ), 1, 1, ''),'')";
            SqlParameter[] parameters = {
                                           new SqlParameter("UserID", SqlDbType.Int)
                                       };
            parameters[0].Value = UserID;

            return SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters).ToString();
        }

        public DataTable GetUserSkillGroupIdAndPriorty(int UserId)
        {
            string sqlStr = @"SELECT b.SGID,b.ManufacturerSGID,a.SkillPriority FROM 
                            dbo.UserSkillDataRight AS a INNER JOIN dbo.SkillGroup AS b ON a.SGID = b.SGID
                            WHERE b.Status = 0 AND a.UserID=@UserId
                            ORDER BY b.CDID";
            SqlParameter[] parameters = {
                                           new SqlParameter("UserId", SqlDbType.Int) 
                                       };
            parameters[0].Value = UserId;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public string GetManufactureSkillGroupID(int SGID)
        {
            string strSql = @"SELECT ManufacturerSGID FROM dbo.SkillGroup WHERE SGID=@SGID";

            SqlParameter[] parameters = {
                                           new SqlParameter("SGID", SqlDbType.Int) 
                                       };
            parameters[0].Value = SGID;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
        /// 根据字母ID获取技能组名称
        /// <summary>
        /// 根据字母ID获取技能组名称
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public string GetSkillNameByMID(string MID)
        {
            string strSql = @"SELECT Name FROM dbo.SkillGroup WHERE ManufacturerSGID=@MID";

            SqlParameter[] parameters = {
                                           new SqlParameter("MID", SqlDbType.VarChar,20) 
                                       };
            parameters[0].Value = MID;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
        /// 根据数字ID获取技能组名称
        /// <summary>
        /// 根据数字ID获取技能组名称
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public string GetSkillNameBySGID(int SGID)
        {
            string strSql = @"SELECT Name FROM dbo.SkillGroup WHERE SGID=@SGID";

            SqlParameter[] parameters = {
                                           new SqlParameter("SGID", SqlDbType.Int) 
                                       };
            parameters[0].Value = SGID;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
        /// 根据热线获取技能组
        /// <summary>
        /// 根据热线获取技能组
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetHotlineSkillGroupByTelMainNum(string tel)
        {
            string where = "";
            if (!string.IsNullOrEmpty(tel))
            {
                where = " AND cd.TelMainNum='" + StringHelper.SqlFilter(tel) + "' ";
            }
            string sql = @"SELECT  sg.* ,cd.CallNum,cd.Remark,cd.TelMainNum,cd.AreaCode
                                    FROM    SkillGroup AS sg
                                    LEFT JOIN CallDisplay AS cd ON sg.CDID = cd.CDID
                                    WHERE   sg.Status = 0 AND sg.Factory = 1 AND ISNULL(TelMainNum, '')<>''" + where + @"                                            
                                    ORDER BY cd.OrderNum ASC ,sg.SGID ASC";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 技能组与热线的对照表
        /// <summary>
        /// 技能组与热线的对照表
        /// </summary>
        /// <returns></returns>
        public DataTable GetHotlineRelationSkillInfo(string linenums)
        {
            string sql = @" SELECT b.TelMainNum,a.SGID FROM SkillGroup a JOIN CallDisplay b ON a.CDID=b.CDID
                WHERE a.Factory=1 and b.TelMainNum  IN(" + linenums + ")";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}
