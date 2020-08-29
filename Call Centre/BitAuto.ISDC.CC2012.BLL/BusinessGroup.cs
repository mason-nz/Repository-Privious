using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class BusinessGroup
    {
        #region Instance
        public static readonly BusinessGroup Instance = new BusinessGroup();
        #endregion

        public DataTable GetAllBusinessGroup()
        {
            return Dal.BusinessGroup.Instance.GetAllBusinessGroup();
        }

        public DataTable GetCallDisplay(bool hastel = false)
        {
            return Dal.BusinessGroup.Instance.GetCallDisplay(hastel);
        }
        public DataTable GetBusinessLine()
        {
            return Dal.BusinessGroup.Instance.GetBusinessLine();
        }
        public DataTable GetBusinessLine(int bgid)
        {
            return Dal.BusinessGroup.Instance.GetBusinessLine(bgid);
        }
        public string GetBusinessLineIDs(int bgid)
        {
            string info = "";
            DataTable dt = GetBusinessLine(bgid);
            foreach (DataRow dr in dt.Rows)
            {
                info += dr["RecID"].ToString() + ",";
            }
            return info.TrimEnd(',');
        }
        public DataTable GetInUseBusinessGroup(int userid)
        {
            return Dal.BusinessGroup.Instance.GetInUseBusinessGroup(userid);
        }

        public DataTable GetBusinessGroupByBGID(int bgId)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupByBGID(bgId);
        }

        public Entities.BusinessGroup GetBusinessGroupInfoByBGID(int bgId)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupInfoByBGID(bgId);
        }

        public int Insert(Entities.BusinessGroup model)
        {
            return Dal.BusinessGroup.Instance.Insert(model);
        }

        public bool CheckGroupNameIsNotUsed(int bgid, string name)
        {
            return Dal.BusinessGroup.Instance.CheckGroupNameIsNotUsed(bgid, name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.BusinessGroup model)
        {
            return Dal.BusinessGroup.Instance.Update(model);
        }
        /// <summary>
        /// 根据userid获取业务组标签
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OnlyByGroup">是否只是按所在分组查询数据</param>
        /// <param name="OnlyByGroup">是否显示停用的标签</param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByUserID(int UserID, bool OnlyByGroup = false, bool IsShowStop = true)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupTagsByUserID(UserID, OnlyByGroup, IsShowStop);
        }

        /// <summary>
        /// 取用户所在分组对应的标签
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetInBusinessGroupTagsByUserID(int UserID)
        {
            return Dal.BusinessGroup.Instance.GetInBusinessGroupTagsByUserID(UserID);
        }
        public DataTable GetTagsByBG_Pid(string BGID, string Pid, string strStatus)
        {
            return Dal.BusinessGroup.Instance.GetTagsByBG_Pid(BGID, Pid, strStatus);
        }

        public DataTable GetTagsCatalogsByBG_Pid(string BGID, string Pid)
        {
            return Dal.BusinessGroup.Instance.GetTagsCatalogsByBG_Pid(BGID, Pid);
        }

        /// <summary>
        /// 根据BGID获取业务组标签
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByBGID(int UserID, int BGID)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupTagsByBGID(UserID, BGID);
        }

        /// <summary>
        /// 根据BGID获取某个业务组标签
        /// </summary>
        /// <param name="BGID"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByBGIDOnly(int BGID)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupTagsByBGIDOnly(BGID);
        }
        /// <summary>
        /// 按名称查询业务组信息（精确查询）
        /// </summary>
        /// <param name="name">业务组名称</param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByName(string name)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupByName(name);
        }

        public DataTable GetBusinessGroupByAreaID(int regionId)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupByAreaID(regionId);
        }

        /// <summary>
        /// 根据组串获取业务组数据
        /// </summary>
        /// <param name="bgids"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByBGIDS(string bgids)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupByBGIDS(bgids);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByUserIDs(string userids)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroupByUserIDs(userids);
        }

        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroup(QueryBusinessGroup query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.BusinessGroup.Instance.GetBusinessGroup(query, order, currentPage, pageSize, out totalCount);
        }
    }
}
