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
	/// ҵ���߼���ConsultPUseCar ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultPUseCar
	{
		#region Instance
		public static readonly ConsultPUseCar Instance = new ConsultPUseCar();
		#endregion

		#region Contructor
		protected ConsultPUseCar()
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
		public DataTable GetConsultPUseCar(QueryConsultPUseCar query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ConsultPUseCar.Instance.GetConsultPUseCar(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ConsultPUseCar.Instance.GetConsultPUseCar(new QueryConsultPUseCar(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ConsultPUseCar GetConsultPUseCar(int RecID)
		{
			
			return Dal.ConsultPUseCar.Instance.GetConsultPUseCar(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryConsultPUseCar query = new QueryConsultPUseCar();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultPUseCar(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ConsultPUseCar model)
		{
			return Dal.ConsultPUseCar.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ConsultPUseCar model)
		{
			return Dal.ConsultPUseCar.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.ConsultPUseCar.Instance.Delete(RecID);
		}

		#endregion

	}
}

