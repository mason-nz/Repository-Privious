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
	/// ҵ���߼���ProjectTask_Cust_Contact ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_Cust_Contact
	{
		#region Instance
		public static readonly ProjectTask_Cust_Contact Instance = new ProjectTask_Cust_Contact();
		#endregion

		#region Contructor
		protected ProjectTask_Cust_Contact()
		{}
		#endregion

        /// <summary>
        /// �õ���ϵ����Ϣ
        /// </summary>
        public DataTable GetContactInfo(Entities.QueryProjectTask_Cust_Contact query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_Cust_Contact.Instance.GetProjectTask_Cust_Contact(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// �õ���ϵ����Ϣ
        /// </summary>
        public Entities.ProjectTask_Cust_Contact GetContactInfo(int recID)
        {
            return Dal.ProjectTask_Cust_Contact.Instance.GetProjectTask_Cust_Contact(recID);
        }

        /// <summary>
        /// �õ�����tid�ģ���contactId�����ϵ��
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public DataTable GetContactInfoExcept(string tid, int contactId)
        {
            int tc = -1;
            Entities.QueryProjectTask_Cust_Contact q = new Entities.QueryProjectTask_Cust_Contact();
            q.PTID = tid;
            DataTable dt = this.GetContactInfo(q, "", 1, 10000, out tc);
            if (dt == null || dt.Rows.Count == 0) { return null; }
            string s = contactId.ToString();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ID"].ToString() == s)
                {
                    dt.Rows.RemoveAt(i);
                    break;
                }
            }
            return dt;
        }

        /// <summary>
        /// ����
        /// </summary>
        public int InsertContact(Entities.ProjectTask_Cust_Contact c)
        {
            c.Status = 0;
            return Dal.ProjectTask_Cust_Contact.Instance.Insert(c);
        }

        /// <summary>
        /// ����
        /// </summary>
        public void UpdateContact(Entities.ProjectTask_Cust_Contact c)
        {
            Dal.ProjectTask_Cust_Contact.Instance.Update(c);
        }


        public void DeleteContact(int id)
        {
            Entities.ProjectTask_Cust_Contact c = this.GetContactInfo(id);
            if (c == null) { throw new Exception("�޷��õ�����ϵ��"); }

            if (c.OriginalContactID > 0) { throw new Exception("����ɾ��CRM�����е���ϵ��"); }

            Dal.ProjectTask_Cust_Contact.Instance.Delete(id);
        }

        /// <summary>
        /// ����TID������ProjectTask_Cust_Contact�У�StatusֵΪ-1
        /// </summary>
        /// <param name="tid">����ID</param>
        public void DeleteContactByTID(string tid)
        {
            Dal.ProjectTask_Cust_Contact.Instance.DeleteContactByTID(tid);
        }

	}
}

