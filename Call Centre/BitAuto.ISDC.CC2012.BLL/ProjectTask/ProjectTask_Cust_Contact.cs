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
	/// 业务逻辑类ProjectTask_Cust_Contact 的摘要说明。
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
        /// 得到联系人信息
        /// </summary>
        public DataTable GetContactInfo(Entities.QueryProjectTask_Cust_Contact query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_Cust_Contact.Instance.GetProjectTask_Cust_Contact(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 得到联系人信息
        /// </summary>
        public Entities.ProjectTask_Cust_Contact GetContactInfo(int recID)
        {
            return Dal.ProjectTask_Cust_Contact.Instance.GetProjectTask_Cust_Contact(recID);
        }

        /// <summary>
        /// 得到属于tid的，除contactId外的联系人
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
        /// 新增
        /// </summary>
        public int InsertContact(Entities.ProjectTask_Cust_Contact c)
        {
            c.Status = 0;
            return Dal.ProjectTask_Cust_Contact.Instance.Insert(c);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void UpdateContact(Entities.ProjectTask_Cust_Contact c)
        {
            Dal.ProjectTask_Cust_Contact.Instance.Update(c);
        }


        public void DeleteContact(int id)
        {
            Entities.ProjectTask_Cust_Contact c = this.GetContactInfo(id);
            if (c == null) { throw new Exception("无法得到此联系人"); }

            if (c.OriginalContactID > 0) { throw new Exception("不可删除CRM中已有的联系人"); }

            Dal.ProjectTask_Cust_Contact.Instance.Delete(id);
        }

        /// <summary>
        /// 根据TID，更新ProjectTask_Cust_Contact中，Status值为-1
        /// </summary>
        /// <param name="tid">任务ID</param>
        public void DeleteContactByTID(string tid)
        {
            Dal.ProjectTask_Cust_Contact.Instance.DeleteContactByTID(tid);
        }

	}
}

