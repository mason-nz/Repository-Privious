using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Dal;
using BitAuto.ISDC.CC2012.Entities;
namespace BitAuto.ISDC.CC2012.BLL
{
    public class EmployeeSuper
    {
        public static readonly EmployeeSuper Instance = new EmployeeSuper();

        #region select
        /// <summary>
        /// 分页获取员工（多条件搜索）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetEmployeeSuper(QueryEmployeeSuper query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeSuper.Instance.GetEmployeeSuper(query, order, currentPage, pageSize, out totalCount);
        }
        /// 获取坐席数据
        /// <summary>
        /// 获取坐席数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgent(int userid)
        {
            return Dal.EmployeeSuper.Instance.GetEmployeeAgent(userid);
        }
        #endregion

        #region 更新数据权限
        /// <summary>
        /// 更新数据权限
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dataType">数据权限码，1-个人，2-全部</param>
        /// <param name="creatUserID">修改者UserID</param>
        /// <returns></returns>
        public int UserDataRight_Update(string userID, string creatUserID)
        {
            return Dal.EmployeeSuper.Instance.UserDataRight_Update(userID, creatUserID);
        }
        #endregion

        #region 更新数据权限
        /// <summary>
        /// 更新用户坐席号
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dataType">坐席号</param>
        /// <param name="creatUserID">修改者UserID</param>
        /// <returns></returns>
        public int EmployeeAgent_Update(string userID, string agentNum, string creatUserID)
        {
            return Dal.EmployeeSuper.Instance.EmployeeAgent_Update(userID, agentNum, creatUserID);
        }
        #endregion

        #region 获取当前用户所属分组合并管辖分组
        /// <summary>
        /// 获取当前用户所属分组合并管辖分组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetCurrentUserGroups(int userid)
        {
            //所属分组
            DataTable bgdt1 = BLL.EmployeeSuper.Instance.GetEmployeeAgent(userid);
            //管辖分组
            DataTable bgdt2 = BLL.BusinessGroup.Instance.GetInUseBusinessGroup(userid);
            //合并数据
            DataTable bgdt = new DataTable();
            bgdt.Columns.Add("BGID");
            bgdt.Columns.Add("Name");
            AddDatatable(bgdt1, bgdt);
            AddDatatable(bgdt2, bgdt);

            //排序
            if (bgdt != null && bgdt.Rows.Count > 0)
            {
                DataView dv = bgdt.DefaultView;
                dv.Sort = "Name ASC";
                DataTable dtNew = dv.ToTable();

                return dtNew;
            }
            return bgdt;// new DataTable();
        }

        /// <summary>
        /// 获取当前用户管辖分组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetCurrentUseBusinessGroup(int userid)
        {
            //管辖分组
            DataTable bgdt2 = BLL.BusinessGroup.Instance.GetInUseBusinessGroup(userid);
            //排序
            if (bgdt2 != null && bgdt2.Rows.Count > 0)
            {
                DataView dv = bgdt2.DefaultView;
                dv.Sort = "Name ASC";
                DataTable dtNew = dv.ToTable();
                return dtNew;
            }
            else
            {
                return bgdt2;
            }
        }

        /// <summary>
        /// 合并表格
        /// </summary>
        /// <param name="rdt"></param>
        /// <param name="dedt"></param>
        private static void AddDatatable(DataTable rdt, DataTable dedt)
        {
            if (rdt != null)
            {
                foreach (DataRow dr in rdt.Rows)
                {
                    if (dedt.Select("BGID='" + CommonFunction.ObjectToInteger(dr["BGID"]) + "'").Length == 0)
                    {
                        dedt.Rows.Add(new object[] { dr["BGID"], dr["Name"] });
                    }
                }
            }
        }
        /// <summary>
        /// 获取当前用户所属分组合并管辖分组的id集客
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetCurrentUserGroupIDs(int userid)
        {
            string bgids = "";
            DataTable dtbgid = GetCurrentUserGroups(userid);
            foreach (DataRow row in dtbgid.Rows)
            {
                bgids += row["BGID"].ToString() + ",";
            }
            return bgids.TrimEnd(',');
        }
        #endregion

        #region 查询集中权限
        /// 查询集中权限人员信息
        /// <summary>
        /// 查询集中权限人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public SysRightUserInfo GetSysRightUserInfo(int userid)
        {
            return Dal.EmployeeSuper.Instance.GetSysRightUserInfo(userid);
        }
        /// 查询集中权限人员信息
        /// <summary>
        /// 查询集中权限人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<SysRightUserInfo> GetSysRightUserInfo(string userids)
        {
            return Dal.EmployeeSuper.Instance.GetSysRightUserInfo(userids);
        }
        /// 根据TrueName查询集中权限人员信息
        /// <summary>
        /// 根据TrueName查询集中权限人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetSysRightUserByName(string userName, string order, int pageindex, int pagesize, out int total)
        {
            return Dal.EmployeeSuper.Instance.GetSysRightUserByName(userName, order, pageindex, pagesize, out total);
        }
        #endregion
    }
}
