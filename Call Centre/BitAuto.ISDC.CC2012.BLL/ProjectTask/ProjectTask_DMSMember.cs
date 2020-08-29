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
	/// ҵ���߼���ProjectTask_DMSMember ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_DMSMember
	{
		#region Instance
		public static readonly ProjectTask_DMSMember Instance = new ProjectTask_DMSMember();
		#endregion

		#region Contructor
		protected ProjectTask_DMSMember()
		{}
		#endregion

        /// <summary>
        /// �õ���Ҫ�˶ԵĻ�Ա��Ϣ
        /// </summary>
        public List<Entities.ProjectTask_DMSMember> GetProjectTask_DMSMemberByTID(string taskId)
        {
            return Dal.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(taskId);
        }

        /// <summary>
        /// �õ���Ҫ�˶ԵĻ�Ա��Ϣ
        /// </summary>
        public Entities.ProjectTask_DMSMember GetProjectTask_DMSMember(int memberId)
        {
            return Dal.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMember(memberId);
        }



        /// <summary>
        /// ����������ѯ�Ӻ�������ϵͳ�����Ļ�Ա�б�
        /// </summary>
        /// <param name="query">��ѯʵ��</param>
        /// <param name="order">�����ֶ�</param>
        /// <param name="index">��ǰҳ</param>
        /// <param name="pageSize">ҳ��С</param>
        /// <param name="count">������</param>
        /// <returns>����DataTable</returns>
        public DataTable GetProjectTask_DMSMemberBySourceCC(Entities.QueryProjectTask_DMSMember query, string order, int index, int pageSize, out int count)
        {
            return Dal.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberBySourceCC(query, order, index, pageSize, out count);
        }
        /// <summary>
        /// ����������ѯ�Ӻ�������ϵͳ�����Ļ�Ա����״̬����ͳ����
        /// </summary>
        /// <param name="query"></param>
        /// <param name="applyForCount"></param>
        /// <param name="createSuccessfulCount"></param>
        /// <param name="createUnsuccessful"></param>
        /// <param name="rejected"></param>
        public void StatProjectTask_DMSMembersBySourceCC(Entities.QueryProjectTask_DMSMember query, out int applyForCount, out  int createSuccessfulCount, out  int createUnsuccessful, out int rejected)
        {
            DataTable dt = Dal.ProjectTask_DMSMember.Instance.StatProjectTask_DMSMemberBySourceCC(query);
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

        private int GetStatCountBySyncStatus(DataTable dt, int DMSSyncStatus)
        {
            int count = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SyncStatus"].ToString().Trim() == DMSSyncStatus.ToString())
                    {
                        count = int.Parse(dr["SyncStatusCount"].ToString().Trim());
                        break;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// ɾ����Ա
        /// </summary>
        public void Delete(int memberId)
        {
            Entities.ProjectTask_DMSMember c = this.GetProjectTask_DMSMember(memberId);
            if (c == null) { throw new Exception("�޷��õ��˻�Ա"); }

            if (c.OriginalDMSMemberID.Length > 0) { throw new Exception("����ɾ��CRM�����еĻ�Ա"); }

            Dal.ProjectTask_DMSMember.Instance.Delete(memberId);
        }

        public void ProjectTask_DMSMemberDelete(int memberId)
        {
            Entities.ProjectTask_DMSMember c = this.GetProjectTask_DMSMember(memberId);
            if (c == null) { throw new Exception("�޷��õ��˻�Ա"); }

            Dal.ProjectTask_DMSMember.Instance.Delete(memberId);
        }
        public void InsertOrUpdate(Entities.ProjectTask_DMSMember member)
        {
            if (string.IsNullOrEmpty(member.PTID)) { throw new Exception("TID����������"); }

            member.Status = 0;
            if (member.MemberID>0)
            {
                Dal.ProjectTask_DMSMember.Instance.Update(member);
            }
            else
            {
                member.MemberID=Dal.ProjectTask_DMSMember.Instance.Insert(member);
            }

            //����Ʒ������Ʒ��
            Dal.ProjectTask_DMSMember_Brand.Instance.UpdateMemberSerial(member.MemberID, member.BrandIDs, "", 0);
            Dal.ProjectTask_DMSMember_Brand.Instance.UpdateMemberSerial(member.MemberID, "", member.SerialIds, 1);
        }

        /// <summary>
        /// ���ݺ�ʵ��ԱID������CRM����DMSMember������ID
        /// </summary>
        /// <param name="id">��ʵ��ԱID</param>
        /// <param name="originalDMSMemberID">DMSMember������ID</param>
        /// <returns></returns>
        public int UpdateOriginalDMSMemberIDByID(int id, string originalDMSMemberID)
        {
            return Dal.ProjectTask_DMSMember.Instance.UpdateOriginalDMSMemberIDByID(id, originalDMSMemberID);
        }

        /// <summary>
        /// ��������ID��ɾ����Ա��Ϣ
        /// </summary>
        /// <param name="tid">����ID</param>
        public void DeleteByTID(string tid)
        {
            List<Entities.ProjectTask_DMSMember> list = GetProjectTask_DMSMemberByTID(tid);
            foreach (Entities.ProjectTask_DMSMember dmsMember in list)
            {
                BLL.ProjectTask_DMSMember_Brand.Instance.DeleteByMemberID(dmsMember.MemberID);
            }
            Dal.ProjectTask_DMSMember.Instance.DeleteByTID(tid);
        }
        /// <summary>
        /// ����crm��Աidȡccid
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public int GetIDByOriginalDmsMemberID(string orgid)
        {
            return Dal.ProjectTask_DMSMember.Instance.GetIDByOriginalDmsMemberID(orgid);
        }
        public DataTable GetCC_DMSMembersBySourceCC(QueryProjectTask_DMSMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_DMSMember.Instance.GetCC_DMSMembersBySourceCC(query, order, currentPage, pageSize, out totalCount);
        }
        public void StatCC_DMSMembersBySourceCC(QueryProjectTask_DMSMember query, out int applyForCount, out  int createSuccessfulCount, out  int createUnsuccessful, out int rejected)
        {
            DataTable dt = Dal.ProjectTask_DMSMember.Instance.StatCC_DMSMembersBySourceCC(query);
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
    }
}

