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
	/// ҵ���߼���ConsultNewCar ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:09 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultNewCar
	{
		#region Instance
		public static readonly ConsultNewCar Instance = new ConsultNewCar();
		#endregion

		#region Contructor
		protected ConsultNewCar()
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
		public DataTable GetConsultNewCar(QueryConsultNewCar query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ConsultNewCar.Instance.GetConsultNewCar(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ConsultNewCar.Instance.GetConsultNewCar(new QueryConsultNewCar(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ConsultNewCar GetConsultNewCar(int RecID)
		{
			
			return Dal.ConsultNewCar.Instance.GetConsultNewCar(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryConsultNewCar query = new QueryConsultNewCar();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultNewCar(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ConsultNewCar model)
		{
			return Dal.ConsultNewCar.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ConsultNewCar model)
		{
			return Dal.ConsultNewCar.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.ConsultNewCar.Instance.Delete(RecID);
		}

		#endregion

        #region GetMaxID

        // <summary>
        /// ȡ��ǰ���ֵ
        /// </summary>
        /// <returns></returns>
        public int GetCurrMaxID()
        {
            return Dal.ConsultNewCar.Instance.GetCurrMaxID();
        }

        #endregion
    }
}

