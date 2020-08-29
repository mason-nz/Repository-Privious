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
	/// ҵ���߼���ConsultOrderRelpaceCar ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultOrderRelpaceCar
	{
		#region Instance
		public static readonly ConsultOrderRelpaceCar Instance = new ConsultOrderRelpaceCar();
		#endregion

		#region Contructor
		protected ConsultOrderRelpaceCar()
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
		public DataTable GetConsultOrderRelpaceCar(QueryConsultOrderRelpaceCar query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ConsultOrderRelpaceCar.Instance.GetConsultOrderRelpaceCar(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ConsultOrderRelpaceCar.Instance.GetConsultOrderRelpaceCar(new QueryConsultOrderRelpaceCar(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ConsultOrderRelpaceCar GetConsultOrderRelpaceCar(int RecID)
		{
			
			return Dal.ConsultOrderRelpaceCar.Instance.GetConsultOrderRelpaceCar(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryConsultOrderRelpaceCar query = new QueryConsultOrderRelpaceCar();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultOrderRelpaceCar(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ConsultOrderRelpaceCar model)
		{
			return Dal.ConsultOrderRelpaceCar.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ConsultOrderRelpaceCar model)
		{
			return Dal.ConsultOrderRelpaceCar.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ConsultOrderRelpaceCar model)
		{
			return Dal.ConsultOrderRelpaceCar.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ConsultOrderRelpaceCar model)
		{
			return Dal.ConsultOrderRelpaceCar.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.ConsultOrderRelpaceCar.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.ConsultOrderRelpaceCar.Instance.Delete(sqltran, RecID);
		}

		#endregion

	}
}

