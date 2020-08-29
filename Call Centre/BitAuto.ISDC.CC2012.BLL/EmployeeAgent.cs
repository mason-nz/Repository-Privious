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
    /// ҵ���߼���EmployeeAgent ��ժҪ˵����
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
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
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgent(int RecID)
        {

            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(RecID);
        }

        /// <summary>
        /// ͨ��UserID�õ�һ������ʵ��
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserID(int UserID)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentByUserID(UserID);
        }

        /// <summary>
        /// 2014-06-19 �Ϸ�
        /// ͨ��UserID��BGID�õ�һ������ʵ��
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgentByUserIdAndBGID(int UserID, int BGID)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentByUserIdAndBGID(UserID, BGID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.EmployeeAgent.Instance.Delete(RecID);
        }

        #endregion

        #region Other
        /// �׼���-��ȡ�ͷ���Ϣ
        /// <summary>
        /// �׼���-��ȡ�ͷ���Ϣ
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

        /// ��ѯ�û���������Ϣ
        /// <summary>
        /// ��ѯ�û���������Ϣ
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

        /// ��ȡ��ǰ��¼���¹�Ͻ�������Ա
        /// <summary>
        /// ��ȡ��ǰ��¼���¹�Ͻ�������Ա
        /// </summary>
        /// <returns></returns>
        public DataTable GetEmployeeAgentByLoginUser()
        {
            int login_userid = Util.GetLoginUserID();
            //int region_id = GetEmployeeAgentRegionID(login_userid);
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentByLoginUser(login_userid);
        }

        /// ��¼���Ƿ��ǳ�������Ա
        /// <summary>
        /// ��¼���Ƿ��ǳ�������Ա
        /// </summary>
        /// <returns></returns>
        private bool IsLoginSupper()
        {
            string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            int userid = BLL.Util.GetLoginUserID();
            DataTable rolesDt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
            DataRow[] rows = rolesDt.Select("RoleName='��������Ա'");
            if (rows.Length == 0)
            {
                return false;
            }
            else return true;
        }
        /// ������ɫ��
        /// <summary>
        /// ������ɫ��
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
        /// ��ȡCC��IMϵͳ�Ĺ��н�ɫ
        /// <summary>
        /// ��ȡCC��IMϵͳ�Ĺ��н�ɫ
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
                //У��
                if (cc_role_name == "��������Ա" && IsLoginSupper() == false)
                {
                    continue;
                }

                //��ѯIm
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
        /// ������������ҵ�����������
        /// <summary>
        /// ������������ҵ�����������
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="btype"></param>
        /// <param name="bgid"></param>
        public void UpdateMutilEmployeeAgent(string uids, int btype, int bgid)
        {
            Dal.EmployeeAgent.Instance.UpdateMutilEmployeeAgent(uids, btype, bgid);
        }
        /// ��ѯ�û���Ϣ��ΪIMϵͳ��
        /// <summary>
        /// ��ѯ�û���Ϣ��ΪIMϵͳ��
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

        /// ��ȡ�����й��ŵ���ϯ��ȫ������
        /// <summary>
        /// ��ȡ�����й��ŵ���ϯ��ȫ������
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllEmployeeAgentAndBusinessGroup()
        {
            return Dal.EmployeeAgent.Instance.GetAllEmployeeAgentAndBusinessGroup();
        }

        /// <summary>
        /// ������ϯ�¹��ţ���㣺1000���������մ˵�����©����������1
        /// </summary>
        /// <param name="genNewAgentID_StartPoint">���</param>
        /// <returns>�������ɵĹ�������</returns>
        public string GenNewAgentID(int genNewAgentID_StartPoint)
        {
            return Dal.EmployeeAgent.Instance.GenNewAgentID(genNewAgentID_StartPoint);
        }

        /// <summary>
        /// ���ָ�����ţ���Ա��ID�������������������
        /// </summary>
        /// <param name="agentNum">����</param>
        /// <param name="userID">Ա��ID</param>
        /// <returns></returns>
        public int EmptyAgentNum(string agentNum, int userID)
        {
            return Dal.EmployeeAgent.Instance.EmptyAgentNum(agentNum, userID);
        }

        #region ר����ϯ
        /// <summary>
        /// ��ѯר����ϯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetEmployeeAgentExclusive(QueryEmployeeAgentExclusive query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentExclusive(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ����ר���ͷ�
        /// </summary>
        /// <param name="userids">�û�id</param>
        /// <param name="isexclusive">�Ƿ�ר����1�ǣ�0���ǣ�</param>
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
        /// ��ȡר������ͨ��timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetEmployeeAgentExclusiveByTimestamp(long timestamp, int maxrow = -1)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentExclusiveByTimestamp(timestamp, maxrow);
        }

        /// ��ȡ��Ŀ�����ʱ���
        /// <summary>
        /// ��ȡ��Ŀ�����ʱ���
        /// </summary>
        /// <returns></returns>
        public long GetEmployeeAgentExclusiveMaxTimeStamp_XA()
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgentExclusiveMaxTimeStamp_XA();
        }

        /// ��Ŀ�������
        /// <summary>
        /// ��Ŀ�������
        /// </summary>
        /// <param name="dt"></param>
        public bool BulkCopyToDB_EmployeeAgentExclusive_XA(DataTable dt, out string msg)
        {
            //�����ʱ��
            Dal.EmployeeAgent.Instance.ClearEmployeeAgentExclusiveTemp_XA();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("UserID", "UserID"));
            list.Add(new SqlBulkCopyColumnMapping("AgentNum", "AgentNum"));
            list.Add(new SqlBulkCopyColumnMapping("UserName", "UserName"));
            list.Add(new SqlBulkCopyColumnMapping("BGID", "BGID"));
            list.Add(new SqlBulkCopyColumnMapping("IsExclusive", "IsExclusive"));
            list.Add(new SqlBulkCopyColumnMapping("TIMESTAMP", "TIMESTAMP"));

            //���
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

        /// ����ʱ�����-��Ŀ
        /// <summary>
        /// ����ʱ�����-��Ŀ
        /// </summary>
        /// <returns></returns>
        public int[] UpdateEmployeeAgentExclusiveFromTemp_XA()
        {
            return Dal.EmployeeAgent.Instance.UpdateEmployeeAgentExclusiveFromTemp_XA();
        }

        #endregion

    }
}

