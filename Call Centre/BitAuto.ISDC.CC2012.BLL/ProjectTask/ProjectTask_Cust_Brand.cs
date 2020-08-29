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
	/// ҵ���߼���ProjectTask_Cust_Brand ��ժҪ˵����
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
	public class ProjectTask_Cust_Brand
	{
		#region Instance
		public static readonly ProjectTask_Cust_Brand Instance = new ProjectTask_Cust_Brand();
		#endregion

		#region Contructor
		protected ProjectTask_Cust_Brand()
		{}
		#endregion

		#region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ  ��ҳ
        /// </summary>
        /// <param name="queryProjectTask_Cust_Brand">��ѯֵ����������Ų�ѯ����</param>     
        /// <param name="currentPage">ҳ��,-1����ҳ</param>   
        /// <param name="order"> </param>
        /// <param name="totalCount">������</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <returns>�������缯��</returns>
        public DataTable GetProjectTask_Cust_Brand(QueryProjectTask_Cust_Brand queryProjectTask_Cust_Brand,string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_Cust_Brand.Instance.GetProjectTask_Cust_Brand(queryProjectTask_Cust_Brand, order, currentPage, pageSize, out totalCount);
        }
        #endregion

        /// <summary>
        /// ��������ID��ɾ����Ϣ
        /// </summary>
        /// <param name="tid">����ID</param>
        public void DeleteByTID(string tid)
        {
            Dal.ProjectTask_Cust_Brand.Instance.DeleteByTID(tid);
        }

	}
}

