using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.Utils;
using BitAuto.DSC.IM_DMS2014.Entities;
using System.Data.SqlClient;

namespace BitAuto.DSC.IM_DMS2014.Dal
{
    /// 查询基础数据
    /// <summary>
    /// 查询基础数据
    /// </summary>
    public class BaseData : DataBase
    {
        private BaseData() { }
        private static BaseData instance = null;
        public static BaseData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BaseData();
                }
                return instance;
            }
        }

        /// 获取所有大区
        /// <summary>
        /// 获取所有大区
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllDistrict()
        {
            string sql = "SELECT DISTINCT District,DistrictName FROM dbo.v_CityGroup ORDER BY District";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 获取所有城市群
        /// <summary>
        /// 获取所有城市群
        /// </summary>
        /// <returns></returns>
        //public DataTable GetAllCityGroup()
        //{
        //    string sql = "SELECT DISTINCT CityGroup,CityGroupName FROM dbo.v_CityGroup ORDER BY CityGroup";
        //    return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        //}
        /// 获取所有的省市区数据
        /// <summary>
        /// 获取所有的省市区数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAreaInfo()
        {
            string sql = "SELECT * FROM dbo.v_AreaInfo";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        /// 通过大区获取城市群
        /// <summary>
        /// 通过大区获取城市群
        /// </summary>
        /// <returns></returns>
//        public DataTable GetCityGroupByDistrict(string districtID)
//        {
//            string sql = @"SELECT DISTINCT CityGroup,CityGroupName FROM dbo.v_CityGroup 
//                                    WHERE District='" + districtID + @"'
//                                    ORDER BY CityGroup";
//            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
//        }
        /// 通过会员code获取会员信息
        /// <summary>
        /// 通过会员code获取会员信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDMSMemberByCode(string memberCode)
        {
            string sql = @"SELECT a.*,b.LastUserID,b.LastBeginTime,b.LastMessageTime,b.Distribution
                                    FROM dbo.v_DMSMember a
                                    LEFT JOIN dbo.CustomerInfo b ON a.MemberCode=b.MemberCode
                                    WHERE a.MemberCode='" + StringHelper.SqlFilter(memberCode) + "'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        #region 获取坐席相关信息
        /// <summary>
        /// 根据坐席UserID，获取CC系统中，AgentID
        /// </summary>
        /// <param name="userid">坐席UserID</param>
        /// <returns>返回CC系统中，AgentID，若找不到，则返回字符串空</returns>
        public string GetAgentNumByUserID(string userid)
        {
            string sql = string.Format(@"SELECT Top 1 AgentNum FROM v_AgentInfo WHERE userid={0}", StringHelper.SqlFilter(userid));
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["AgentNum"].ToString();
            }
            return string.Empty;
        }
        /// 查询有权限的坐席
        /// <summary>
        /// 查询有权限的坐席
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetAgentInfoData(QueryAgentInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string sql = @"SELECT RecID,UserID,TrueName,AgentNum,BGID,BGName YanFaFROM dbo.v_AgentInfo WHERE 1=1 ";
            //数据权限,为了测试，先注释掉，上线再放开
            
            sql += " AND isLegal=1";
            //查询条件
            if (!string.IsNullOrEmpty(query.BGIDs) && query.BGIDs != "-1")
            {
                sql += " AND BGID IN ('" + SqlFilter(query.BGIDs) + "')";
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                sql += " AND TrueName LIKE '%" + SqlFilter(query.Name) + "%'";
            }
            if (!string.IsNullOrEmpty(query.CityGroups))
            {
                string[] array = query.CityGroups.Split(',');

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = StringHelper.SqlFilter(array[i]);
                }


                string str = "'" + string.Join("','", array) + "'";
                sql += " AND UserID IN (SELECT UserID FROM dbo.CityGroupAgent WHERE Status=0 AND CityGroupID IN (" + str + "))";
            }

            return CommonDal.Instance.GetCommonPageData(sql, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 获取坐席所在区域
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetAgentRegionByUserID(string userid)
        {
            string sql = string.Format(@"SELECT Top 1 RegionID FROM v_AgentInfo WHERE userid={0}", StringHelper.SqlFilter(userid));
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["RegionID"].ToString();
            }
            return string.Empty;
        }
        #endregion

        /// 获取当前用户下所有的管辖分组
        /// <summary>
        /// 获取当前用户下所有的管辖分组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigth(int userid)
        {
            string sql = @"SELECT DISTINCT
                                            a.UserID ,
                                            a.BGID ,
                                            b.Name
                                    FROM    UserGroupDataRigth a
                                            INNER JOIN dbo.BusinessGroup b ON a.BGID = b.BGID
                                    WHERE   b.Status = 0
                                    AND a.UserID='" + userid + "' ORDER BY a.BGID";
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sql).Tables[0];
        }
        /// 获取坐席所属区域的全部业务组
        /// <summary>
        /// 获取坐席所属区域的全部业务组
        /// </summary>
        /// <param name="userid">坐席userid></param>
        /// <returns></returns>
        public DataTable GetUserGroupByUserID(int userid)
        {
            string sql = @"SELECT  bg.BGID,bg.Name
                                    FROM    dbo.BusinessGroup bg
                                            INNER JOIN dbo.EmployeeAgent ea ON bg.RegionID = ea.RegionID
                                    WHERE   bg.Status = 0
                                    AND ea.UserID =" + userid + " ORDER BY bg.Name";
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sql).Tables[0];
        }


        /// <summary>
        /// 根据UserID，返回坐席的所在分组ID
        /// </summary>
        /// <param name="userid">坐席UserID</param>
        /// <returns>返回坐席所在分组ID，座机</returns>
        public int GetAgentBGIDByUserID(int userid)
        {
            int bgid = -2;
            string sql = string.Format(@"SELECT Top 1 BGID FROM v_AgentInfo WHERE userid={0}", userid);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int.TryParse(ds.Tables[0].Rows[0]["BGID"].ToString(), out bgid);
            }
            return bgid;
        }
    }
}
