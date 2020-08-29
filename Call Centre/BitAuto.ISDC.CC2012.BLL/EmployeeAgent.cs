using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类EmployeeAgent 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-02 10:01:54 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class EmployeeAgent
    {
        #region Instance
        public static readonly EmployeeAgent Instance = new EmployeeAgent();
        #endregion

        #region Contructor
        protected EmployeeAgent()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetEmployeeAgent(QueryEmployeeAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetEmployeeAgentsByAgentNum(string AgentNum)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentsByAgentNum(AgentNum);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgent(int RecID)
        {

            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(RecID);
        }

        /// <summary>
        /// 通过UserID得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserID(int UserID)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentByUserID(UserID);
        }

        /// <summary>
        /// 2014-06-19 毕帆
        /// 通过UserID和BGID得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserIdAndBGID(int UserID, int BGID)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentByUserIdAndBGID(UserID, BGID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.EmployeeAgent.Instance.Delete(RecID);
        }

        #endregion

        #region Other
        /// 易集客-获取客服信息
        /// <summary>
        /// 易集客-获取客服信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bgid"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKeFuByYiJiKe(string name, string bgid, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeAgent.Instance.GetKeFuByYiJiKe(name, bgid, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int GetUserCountByGroup(string where)
        {
            return Dal.EmployeeAgent.Instance.GetUserCountByGroup(where);
        }

        public void AddOrUpdateWBEmployee(string WBUserIDStr, int RegionID, int genNewAgentID_StartPoint)
        {
            Dal.EmployeeAgent.Instance.AddOrUpdateWBEmployee(WBUserIDStr, RegionID, genNewAgentID_StartPoint);
        }

        /// 查询用户的区域信息
        /// <summary>
        /// 查询用户的区域信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int GetEmployeeAgentRegionID(int UserID)
        {
            Entities.EmployeeAgent user = GetEmployeeAgentByUserID(UserID);
            int? r = user.RegionID;
            if (r.HasValue)
            {
                return r.Value;
            }
            else return -1;
        }

        /// 获取当前登录人下管辖分组的人员
        /// <summary>
        /// 获取当前登录人下管辖分组的人员
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmployeeAgentByLoginUser()
        {
            int login_userid = Util.GetLoginUserID();
            //int region_id = GetEmployeeAgentRegionID(login_userid);
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentByLoginUser(login_userid);
        }

        /// 登录人是否是超级管理员
        /// <summary>
        /// 登录人是否是超级管理员
        /// </summary>
        /// <returns></returns>
        private bool IsLoginSupper()
        {
            string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            int userid = BLL.Util.GetLoginUserID();
            DataTable rolesDt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
            DataRow[] rows = rolesDt.Select("RoleName='超级管理员'");
            if (rows.Length == 0)
            {
                return false;
            }
            else return true;
        }
        /// 创建角色表
        /// <summary>
        /// 创建角色表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateRoleTable()
        {
            DataTable role_dt = new DataTable();
            role_dt.Columns.Add("RoleID");
            role_dt.Columns.Add("RoleName");
            role_dt.Columns.Add("CC_RoleID");
            role_dt.Columns.Add("IM_RoleID");
            return role_dt;
        }
        /// 获取CC和IM系统的共有角色
        /// <summary>
        /// 获取CC和IM系统的共有角色
        /// </summary>
        /// <returns></returns>
        public DataTable GetCCAndIMRoles()
        {
            string cc_sysid = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            string im_sysid = ConfigurationUtil.GetAppSettingValue("IMSysID");

            DataTable cc_dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID(cc_sysid);
            DataTable im_dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID(im_sysid);
            DataTable role_dt = CreateRoleTable();

            foreach (DataRow ccdr in cc_dt.Rows)
            {
                string cc_role_id = ccdr["RoleID"].ToString();
                string cc_role_name = ccdr["RoleName"].ToString();
                //校验
                if (cc_role_name == "超级管理员" && IsLoginSupper() == false)
                {
                    continue;
                }

                //查询Im
                DataRow[] drs = im_dt.Select("RoleName='" + cc_role_name + "'");
                if (drs.Length != 0)
                {
                    string im_role_id = drs[0]["RoleID"].ToString();
                    string role_id = cc_role_id + "_" + im_role_id;
                    role_dt.Rows.Add(new object[] { role_id, cc_role_name, cc_role_id, im_role_id });
                }
            }
            return role_dt;
        }
        /// 批量更新所属业务和所属分组
        /// <summary>
        /// 批量更新所属业务和所属分组
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="btype"></param>
        /// <param name="bgid"></param>
        public void UpdateMutilEmployeeAgent(string uids, int btype, int bgid)
        {
            Dal.EmployeeAgent.Instance.UpdateMutilEmployeeAgent(uids, btype, bgid);
        }
        /// 查询用户信息（为IM系统）
        /// <summary>
        /// 查询用户信息（为IM系统）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgentForIM(int userid)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentForIM(userid);
        }

        public string GetAgentNumberAndUserNameByUserId(int UserId)
        {
            return Dal.EmployeeAgent.Instance.GetAgentNumberAndUserNameByUserId(UserId);
        }
        #endregion

        /// 获取所有有工号的坐席和全部分组
        /// <summary>
        /// 获取所有有工号的坐席和全部分组
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllEmployeeAgentAndBusinessGroup()
        {
            return Dal.EmployeeAgent.Instance.GetAllEmployeeAgentAndBusinessGroup();
        }

        /// <summary>
        /// 生成坐席新工号，起点：1000，后续按照此点往后补漏，且自增加1
        /// </summary>
        /// <param name="genNewAgentID_StartPoint">起点</param>
        /// <returns>返回生成的工号内容</returns>
        public string GenNewAgentID(int genNewAgentID_StartPoint)
        {
            return Dal.EmployeeAgent.Instance.GenNewAgentID(genNewAgentID_StartPoint);
        }

        /// <summary>
        /// 清空指定工号，且员工ID不等于这个参数的数据
        /// </summary>
        /// <param name="agentNum">工号</param>
        /// <param name="userID">员工ID</param>
        /// <returns></returns>
        public int EmptyAgentNum(string agentNum, int userID)
        {
            return Dal.EmployeeAgent.Instance.EmptyAgentNum(agentNum, userID);
        }

        #region 专属坐席
        /// <summary>
        /// 查询专属坐席
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetEmployeeAgentExclusive(QueryEmployeeAgentExclusive query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentExclusive(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 设置专属客服
        /// </summary>
        /// <param name="userids">用户id</param>
        /// <param name="isexclusive">是否专属（1是，0不是）</param>
        /// <returns></returns>
        public bool SetEmployeeAgentExclusive(string userids, string isexclusive)
        {
            if (string.IsNullOrEmpty(userids))
            {
                return false;
            }
            if (string.IsNullOrEmpty(isexclusive))
            {
                isexclusive = "0";
            }

            return Dal.EmployeeAgent.Instance.SetEmployeeAgentExclusive(userids, isexclusive);
        }


        /// <summary>
        /// 获取专属数据通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgentExclusiveByTimestamp(long timestamp, int maxrow = -1)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentExclusiveByTimestamp(timestamp, maxrow);
        }

        /// 获取项目表最大时间戳
        /// <summary>
        /// 获取项目表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetEmployeeAgentExclusiveMaxTimeStamp_XA()
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentExclusiveMaxTimeStamp_XA();
        }

        /// 项目数据入库
        /// <summary>
        /// 项目数据入库
        /// </summary>
        /// <param name="dt"></param>
        public bool BulkCopyToDB_EmployeeAgentExclusive_XA(DataTable dt, out string msg)
        {
            //清空临时表
            Dal.EmployeeAgent.Instance.ClearEmployeeAgentExclusiveTemp_XA();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("UserID", "UserID"));
            list.Add(new SqlBulkCopyColumnMapping("AgentNum", "AgentNum"));
            list.Add(new SqlBulkCopyColumnMapping("UserName", "UserName"));
            list.Add(new SqlBulkCopyColumnMapping("BGID", "BGID"));
            list.Add(new SqlBulkCopyColumnMapping("IsExclusive", "IsExclusive"));
            list.Add(new SqlBulkCopyColumnMapping("TIMESTAMP", "TIMESTAMP"));

            //入库
            Util.BulkCopyToDB(dt, Dal.AutoCallSyncData.Instance.Holly_Business, "EmployeeAgent_Temp", 10000, list, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// 从临时表更新-项目
        /// <summary>
        /// 从临时表更新-项目
        /// </summary>
        /// <returns></returns>
        public int[] UpdateEmployeeAgentExclusiveFromTemp_XA()
        {
            return Dal.EmployeeAgent.Instance.UpdateEmployeeAgentExclusiveFromTemp_XA();
        }

        #endregion

    }
}

