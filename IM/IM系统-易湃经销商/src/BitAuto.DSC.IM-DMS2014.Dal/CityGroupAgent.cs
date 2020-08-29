using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类CityGroupAgent。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:00 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CityGroupAgent : DataBase
    {
        public static readonly CityGroupAgent Instance = new CityGroupAgent();
        private const string P_CITYGROUPAGENT_SELECT = "p_CityGroupAgent_Select";

        protected CityGroupAgent()
        { }

        /// 按照查询条件查询
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCityGroupAgent(QueryCityGroupAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string sql = @"SELECT  a.* ,
                                        STUFF(( SELECT  ',' + ISNULL(c.TrueName, b.UserID)
                                                FROM    CityGroupAgent b
                                                        INNER JOIN dbo.v_AgentInfo c ON b.UserID = c.UserID
                                                WHERE   CityGroup = b.CityGroupID
                                                ORDER BY b.UserID
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '') AS kefu_nm_list ,
                                        STUFF(( SELECT  ',' + RTRIM(b.UserID)
                                                FROM    CityGroupAgent b
                                                WHERE   CityGroup = b.CityGroupID
                                                ORDER BY b.UserID
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '') AS kefu_id_list
                                YanFaFROM    ( SELECT DISTINCT
                                                    CityGroup ,
                                                    CityGroupName ,
                                                    District ,
                                                    DistrictName
                                          FROM      dbo.v_CityGroup
                                        ) AS a
                                WHERE   1 = 1";
            //查询条件
            if (!string.IsNullOrEmpty(query.DistrictID) && query.DistrictID != "-1")
            {
                //大区
                sql += " AND a.District IN ('" + StringHelper.SqlFilter(query.DistrictID) + "')";
            }
            if (!string.IsNullOrEmpty(query.CityGroupID) && query.CityGroupID != "-1")
            {
                //城市群
                sql += " AND a.CityGroup IN ( '" + StringHelper.SqlFilter(query.CityGroupID) + "' )";
            }
            if (query.IsHave == "true")
            {
                //无坐席
                sql += " AND a.CityGroup NOT IN(SELECT CityGroupID FROM CityGroupAgent WHERE Status=0)";
            }
            else if (!string.IsNullOrEmpty(query.UserID))
            {
                //存在坐席是某某人的城市群
                sql += " AND a.CityGroup IN (SELECT CityGroupID FROM CityGroupAgent WHERE Status=0 AND UserID='" + StringHelper.SqlFilter(query.UserID) + "')";
            }
            return CommonDal.Instance.GetCommonPageData(sql, order, currentPage, pageSize, out totalCount);
        }
        /// 获取城市群或非城市群的坐席
        /// <summary>
        /// 获取城市群或非城市群的坐席
        /// </summary>
        /// <param name="cityGroup"></param>
        /// <param name="memberCode"></param>
        /// <param name="isConvert"></param>
        /// <returns></returns>
        public List<string> GetAgentsByCityGroup(string cityGroup, string memberCode, bool isConvert)
        {
            List<string> lstUser = new List<string>();

            SqlParameter[] parameters =
            {
                new SqlParameter("@CityGroup", SqlDbType.VarChar, 50),
                new SqlParameter("@MemberCode", SqlDbType.VarChar, 50),
                new SqlParameter("@IsConvert", SqlDbType.Bit)
            };
            parameters[0].Value = cityGroup;
            parameters[1].Value = memberCode;
            parameters[2].Value = isConvert;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure,
                "p_GetAgentByCityGroup", parameters);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lstUser.Add(row[0].ToString());
                }
            }

            return lstUser;
        }

        /// 批量删除
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="agents"></param>
        /// <returns></returns>
        public bool DeleteCityGroupAgent(List<string> groups, List<string> agents)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i] = StringHelper.SqlFilter(groups[i]);
            }
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i] = StringHelper.SqlFilter(agents[i]);
            }
            string sql = @"DELETE FROM dbo.CityGroupAgent
                                    WHERE CityGroupID IN ('" + string.Join("','", groups.ToArray()) + "') AND UserID IN ('" + string.Join("','", agents.ToArray()) + "')";
            int num = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            return num != 0;
        }
        /// 获取空表
        /// <summary>
        /// 获取空表
        /// </summary>
        /// <returns></returns>
        public DataTable GetNullCityGroupAgent()
        {
            string sql = "SELECT * FROM dbo.CityGroupAgent WHERE 1=2";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}

