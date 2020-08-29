using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ҵ���߼���ProjectTask_CSTMember ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_CSTMember
	{
		#region Instance
		public static readonly ProjectTask_CSTMember Instance = new ProjectTask_CSTMember();
		#endregion

		#region Contructor
		protected ProjectTask_CSTMember()
		{}
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
        public DataTable GetProjectTask_CSTMember(QueryProjectTask_CSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(query, order, currentPage, pageSize, out totalCount);
        }
        public int GetIDByCSTRecID(string CSTRecID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetIDByCSTRecID(CSTRecID);
        }
        public string GetIDByCSTTID(int ID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetIDByCSTTID(ID);
        }
        /// <summary>
        /// ��ȡ����ͨȫ�Ʊ����¼
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public DataTable GetCstMemberFullNameHistory(string originalCSTRecID)
        {
            DataTable dt = null;
            try
            {
                dt = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(originalCSTRecID, 7, "FullName");
                DataColumn dcFullName = new DataColumn("FullName");
                dt.Columns.Add(dcFullName);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ContrastInfoInside = dr["ContrastInfoInside"].ToString();
                        string[] array = ContrastInfoInside.Split(',');
                        for (int i = 0; i < array.Length; i += 2)
                        {
                            if (array[i].Trim() == "")
                            {
                                continue;
                            }
                            string colName = array[i].Split(':')[0];
                            string colValue = array[i].Split('(')[1].Trim('\'');
                            if (colName.Equals("FullName"))
                            {
                                dr["FullName"] = BLL.Util.UnEscapeString(colValue);
                            }
                        }

                    }

                }
            }
            catch (Exception e)
            {

            }
            return dt;
        }
        /// <summary>
        /// ���������Ų�ѯ����ͨ��Ա��Ϣ
        /// </summary>
        /// <param name="TID"></param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember> GetProjectTask_CSTMemberByTID(string TID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(TID);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(new QueryProjectTask_CSTMember(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_CSTMember GetProjectTask_CSTMember(int ID)
        {

            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Entities.ProjectTask_CSTMember GetProjectTask_CSTMemberModel(int ID)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberModel(ID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByID(int ID)
        {
            QueryProjectTask_CSTMember query = new QueryProjectTask_CSTMember();
            query.ID = ID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTMember(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �Ƿ����ͬһ���ƵĻ�Ա
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool IsExistSameName(string where)
        {
            return Dal.ProjectTask_CSTMember.Instance.IsExistSameName(where);
        }
        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.ProjectTask_CSTMember model)
        {
            return Dal.ProjectTask_CSTMember.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.ProjectTask_CSTMember model)
        {
            return Dal.ProjectTask_CSTMember.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCstMember(Entities.ProjectTask_CSTMember model)
        {
            Dal.ProjectTask_CSTMember_Brand.Instance.Delete(model.ID);
            Dal.ProjectTask_CSTLinkMan.Instance.DeleteByCstMemberID(model.ID);
            return Dal.ProjectTask_CSTMember.Instance.Update(model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int ID)
        {
            Entities.ProjectTask_CSTMember c = this.GetProjectTask_CSTMember(ID);
            if (c == null) { throw new Exception("�޷��õ��˻�Ա"); }

            if (c.OriginalCSTRecID != null) { throw new Exception("����ɾ��CRM�����еĻ�Ա"); }


            Dal.ProjectTask_CSTMember_Brand.Instance.Delete(ID);
            Dal.ProjectTask_CSTLinkMan.Instance.DeleteByCstMemberID(ID);
            return Dal.ProjectTask_CSTMember.Instance.Delete(ID);
        }

        /// <summary>
        /// ��������IDɾ�������ĳ���ͨ��Ա
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteNewProjectTask_CSTMemberByTID(string tid)
        {
            return Dal.ProjectTask_CSTMember.Instance.DeleteNewProjectTask_CSTMemberByTID(tid);
        }
        /// <summary>
        /// ��������ID�����±�ProjectTask_CSTMember���ֶ�StatusΪ-1
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            BLL.ProjectTask_CSTLinkMan.Instance.DeleteByTID(tid);//ɾ������ͨ��Ա��ϵ��
            BLL.ProjectTask_CSTMember_Brand.Instance.DeleteByTID(tid);//ɾ������ͨ��Ա��ӪƷ����Ϣ
            return Dal.ProjectTask_CSTMember.Instance.DeleteByTID(tid);
        }
        #endregion

        public DataTable GetProjectTask_CSTMembersBySourceCC(QueryProjectTask_CSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberBySourceCC(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ����������ѯ�Ӻ�������ϵͳ�����ĳ���ͨ��Ա����״̬����ͳ����
        /// </summary>
        /// <param name="query"></param>
        /// <param name="applyForCount"></param>
        /// <param name="createSuccessfulCount"></param>
        /// <param name="createUnsuccessful"></param>
        /// <param name="rejected"></param>
        public void StatProjectTask_CSTMembersBySourceCC(Entities.QueryProjectTask_CSTMember query, out int applyForCount, out  int createSuccessfulCount, out  int createUnsuccessful, out int rejected)
        {
            DataTable dt = Dal.ProjectTask_CSTMember.Instance.StatProjectTask_CSTMemberBySourceCC(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                applyForCount = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.ApplyFor);
                createSuccessfulCount = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateSuccessful);
                createUnsuccessful = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateUnsuccessful);
                rejected = GetStatCountBySyncStatus(dt, (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected);
            }
            else
            {
                applyForCount = createSuccessfulCount = createUnsuccessful = rejected = 0;
            }
        }

        private int GetStatCountBySyncStatus(DataTable dt, int CSTSyncStatus)
        {
            int count = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SyncStatus"].ToString().Trim() == CSTSyncStatus.ToString())
                    {
                        count = int.Parse(dr["SyncStatusCount"].ToString().Trim());
                        break;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// ��ѯ�Ƿ����ѿ�ͨ�Ļ�Ա
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool IsOpenSuccessMember(int memberId)
        {
            bool isSuccessOpen = false;
            //��ѯ�Ƿ��ǿ�ͨ�Ļ�Ա
            Entities.ProjectTask_CSTMember cstMember = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(memberId);
            if (!string.IsNullOrEmpty(cstMember.OriginalCSTRecID))
            {
                BitAuto.YanFa.Crm2009.Entities.CstMember cstMemberInfo = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(cstMember.OriginalCSTRecID);

                if (cstMemberInfo != null)
                {
                    if (cstMemberInfo.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.CreateSuccessful)
                    {
                        isSuccessOpen = true;
                    }
                }
            }

            return isSuccessOpen;
        }



        public void ProjectTask_CSTMemberDelete(int memberId)
        {
            Entities.ProjectTask_CSTMember c = this.GetProjectTask_CSTMember(memberId);
            if (c == null) { throw new Exception("�޷��õ��˻�Ա"); }

            Dal.ProjectTask_CSTMember.Instance.Delete(memberId);
        }

        /// <summary>
        /// ��ȡҪ������������Ϣ
        /// </summary>
        /// <param name="MemberStr"></param>
        /// <returns></returns>
        public DataTable GetOrderInfo(string MemberStr)
        {
            return Dal.ProjectTask_CSTMember.Instance.GetOrderInfo(MemberStr);
        }

	}
}

