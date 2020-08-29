using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_DMS2014.Entities;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
    /// 查询基础数据
    /// <summary>
    /// 查询基础数据
    /// </summary>
    public class BaseData
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
            return Dal.BaseData.Instance.GetAllDistrict();
        }
        /// 获取所有城市群
        /// <summary>
        /// 获取所有城市群
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCityGroup()
        {
            return Dal.BaseData.Instance.GetAllCityGroup();
        }
        /// 获取所有的省市区数据
        /// <summary>
        /// 获取所有的省市区数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAreaInfo()
        {
            return Dal.BaseData.Instance.GetAllAreaInfo();
        }


        /// 通过大区获取城市群
        /// <summary>
        /// 通过大区获取城市群
        /// </summary>
        /// <returns></returns>
        public DataTable GetCityGroupByDistrict(string districtID)
        {
            return Dal.BaseData.Instance.GetCityGroupByDistrict(districtID);
        }
        /// 通过会员code获取会员信息
        /// <summary>
        /// 通过会员code获取会员信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDMSMemberByCode(string memberCode)
        {
            return Dal.BaseData.Instance.GetDMSMemberByCode(memberCode);
        }

        #region 获取坐席相关信息
        /// 根据坐席UserID，获取CC系统中，AgentID
        /// <summary>
        /// 根据坐席UserID，获取CC系统中，AgentID
        /// </summary>
        /// <param name="userid">坐席UserID</param>
        /// <returns>返回CC系统中，AgentID，若找不到，则返回字符串空</returns>
        public string GetAgentNumByUserID(string userid)
        {
            return Dal.BaseData.Instance.GetAgentNumByUserID(userid);
        }

        /// <summary>
        /// 根据UserID，返回坐席的所在分组ID
        /// </summary>
        /// <param name="userid">坐席UserID</param>
        /// <returns>返回坐席所在分组ID，座机</returns>
        public int GetAgentBGIDByUserID(int userid)
        {
            return Dal.BaseData.Instance.GetAgentBGIDByUserID(userid);
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
            return Dal.BaseData.Instance.GetAgentInfoData(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 获取坐席所在区域
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetAgentRegionByUserID(string userid)
        {
            return Dal.BaseData.Instance.GetAgentRegionByUserID(userid);
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
            return Dal.BaseData.Instance.GetUserGroupDataRigth(userid);
        }
        /// 获取坐席所属区域的全部业务组
        /// <summary>
        /// 获取坐席所属区域的全部业务组
        /// </summary>
        /// <param name="userid">坐席userid></param>
        /// <returns></returns>
        public DataTable GetUserGroupByUserID(int userid)
        {
            return Dal.BaseData.Instance.GetUserGroupByUserID(userid);
        }

        private const string StartTime_Label = "StartTime";
        private const string EndTime_Label = "EndTime";
        private const string ServerTime_Label = "ServerTime";
        private const string File_Label = "ServerTimeConfig";

        /// 保存时间
        /// <summary>
        /// 保存时间
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        public void SaveTime(ServeTime st, ServeTime et)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(StartTime_Label, st.ToString());
                dic.Add(EndTime_Label, et.ToString());
                Dictionary<string, Dictionary<string, string>> Listdic = new Dictionary<string, Dictionary<string, string>>();
                Listdic.Add(ServerTime_Label, dic);

                string path = AppDomain.CurrentDomain.BaseDirectory + File_Label + ".xml";
                CommonFunc.SaveDictionaryToFile(Listdic, path);
            }
            catch
            {

            }
        }
        /// 读取时间
        /// <summary>
        /// 读取时间
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        public void ReadTime(out ServeTime st, out ServeTime et)
        {
            st = new ServeTime(9, 0);
            et = new ServeTime(18, 0);
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + File_Label + ".xml";
                Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/" + ServerTime_Label);
                st = new ServeTime(dic[StartTime_Label]);
                et = new ServeTime(dic[EndTime_Label]);
            }
            catch
            {

            }
        }
    }
}
