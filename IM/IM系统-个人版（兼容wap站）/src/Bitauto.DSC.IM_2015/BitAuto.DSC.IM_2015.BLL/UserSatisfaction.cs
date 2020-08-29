using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ҵ���߼���UserSatisfaction ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:05 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserSatisfaction
	{
		#region Instance
		public static readonly UserSatisfaction Instance = new UserSatisfaction();
		#endregion

		#region Contructor
		protected UserSatisfaction()
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
		public DataTable GetUserSatisfaction(QueryUserSatisfaction query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserSatisfaction.Instance.GetUserSatisfaction(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserSatisfaction.Instance.GetUserSatisfaction(new QueryUserSatisfaction(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.UserSatisfaction GetUserSatisfaction(int RecID)
		{
			
			return Dal.UserSatisfaction.Instance.GetUserSatisfaction(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryUserSatisfaction query = new QueryUserSatisfaction();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserSatisfaction(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Insert
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.UserSatisfaction.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.UserSatisfaction.Instance.Delete(sqltran, RecID);
		}

		#endregion


        //<summary>
        //�ж϶�ָ���Ự������������Ƿ����
        //</summary>
        //<param name="CSID">�ỰID</param>
        //<returns></returns>
        public bool SatisfactionExists(int CSID)
        {
            return Dal.UserSatisfaction.Instance.SatisfactionExists(CSID);
        }

        /// <summary>
        /// ȡ�����ͳ������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">������</param>
        /// <param name="logUserID">��ǰ��¼��</param>
        /// <returns>��ͳ�Ʒ��أ���daytime�����ڣ�����ͳ�Ʒ��أ�weekb:�ܿ�ʼ���ڣ�weeke���ܽ������ڣ�����ͳ�Ʒ��أ���monthb:�¿�ʼ���ڣ�monthe:�½������ڣ�����������userid��truename��������agentnum�����ţ�sumduihua���ܶԻ�����chanping����������profcmy����Ʒ�ǳ���������promy����Ʒ��������proyb����Ʒһ�㣬probmy����Ʒ����������profcbmy����Ʒ�ǳ�����������perfcmy������ǳ����⣬permy��������������peryb������һ�㣬perbmy���������⣬perfcbmy������ǳ������⣩</returns>
        public DataTable UserSatisfaction_Total_Select(QueryUserSatisfactionTotal query, string order, int currentPage, int pageSize, out int totalCount, int logUserID)
        {
            return Dal.UserSatisfaction.Instance.UserSatisfaction_Total_Select(query, order, currentPage, pageSize, out totalCount, logUserID);
        }

        /// <summary>
        /// ȡ�����ͳ�����ݻ�������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns>����ֵ��sumduihua���ܶԻ�����chanping����������profcmy����Ʒ�ǳ���������promy����Ʒ��������proyb����Ʒһ�㣬probmy����Ʒ����������profcbmy����Ʒ�ǳ�����������perfcmy������ǳ����⣬permy��������������peryb������һ�㣬perbmy���������⣬perfcbmy������ǳ�������</returns>
        public DataTable UserSatisfaction_Total_Select(QueryUserSatisfactionTotal query, int logUserID)
        {
            return Dal.UserSatisfaction.Instance.UserSatisfaction_Total_Select(query,logUserID);
        }


        

	}
}

