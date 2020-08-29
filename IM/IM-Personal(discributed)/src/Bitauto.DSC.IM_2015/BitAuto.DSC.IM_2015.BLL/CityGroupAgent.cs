using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CityGroupAgent 的摘要说明。
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
    public class CityGroupAgent : CommonBll
    {
        public static readonly new CityGroupAgent Instance = new CityGroupAgent();
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
            return Dal.CityGroupAgent.Instance.GetCityGroupAgent(query, order, currentPage, pageSize, out totalCount);
        }
        /// 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CityGroupAgent.Instance.GetCityGroupAgent(new QueryCityGroupAgent(), string.Empty, 1, 1000000, out totalCount);
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
            return Dal.CityGroupAgent.Instance.GetAgentsByCityGroup(cityGroup, memberCode, isConvert);
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
            return Dal.CityGroupAgent.Instance.DeleteCityGroupAgent(groups, agents);
        }
        /// 批量增加
        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="agents"></param>
        /// <returns></returns>
        public bool AddCityGroupAgent(List<string> groups, List<string> agents, int userid)
        {
            DeleteCityGroupAgent(groups, agents);
            DataTable dt = Dal.CityGroupAgent.Instance.GetNullCityGroupAgent();
            foreach (string group in groups)
            {
                foreach (string agent in agents)
                {
                    DataRow dr = dt.NewRow();
                    dr["CityGroupID"] = group;
                    dr["UserID"] = CommonFunc.ObjectToInteger(agent);
                    dr["Status"] = 0;
                    dr["CreateUserID"] = userid;
                    dr["CreateTime"] = DateTime.Now;
                    dt.Rows.Add(dr);
                }
            }
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("CityGroupID", "CityGroupID"));
            list.Add(new SqlBulkCopyColumnMapping("UserID", "UserID"));
            list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
            list.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));

            Util.BulkCopyToDB(dt, CommonBll.IM_conn, "CityGroupAgent", 3000, list);

            return true;
        }
    }
}

