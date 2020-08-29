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
	/// ҵ���߼���OtherTaskWorkOrderMapping ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-09-05 03:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class OtherTaskWorkOrderMapping
	{
		#region Instance
		public static readonly OtherTaskWorkOrderMapping Instance = new OtherTaskWorkOrderMapping();
		#endregion

		#region Contructor
		protected OtherTaskWorkOrderMapping()
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
		public DataTable GetOtherTaskWorkOrderMapping(QueryOtherTaskWorkOrderMapping query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.OtherTaskWorkOrderMapping.Instance.GetOtherTaskWorkOrderMapping(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.OtherTaskWorkOrderMapping.Instance.GetOtherTaskWorkOrderMapping(new QueryOtherTaskWorkOrderMapping(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.OtherTaskWorkOrderMapping GetOtherTaskWorkOrderMapping(int RecID)
		{
			
			return Dal.OtherTaskWorkOrderMapping.Instance.GetOtherTaskWorkOrderMapping(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryOtherTaskWorkOrderMapping query = new QueryOtherTaskWorkOrderMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetOtherTaskWorkOrderMapping(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.OtherTaskWorkOrderMapping model)
		{
			return Dal.OtherTaskWorkOrderMapping.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.OtherTaskWorkOrderMapping model)
		{
			return Dal.OtherTaskWorkOrderMapping.Instance.Insert(sqltran, model);
		}

		#endregion

        //#region Update
        ///// <summary>
        ///// ����һ������
        ///// </summary>
        //public int Update(Entities.OtherTaskWorkOrderMapping model)
        //{
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Update(model);
        //}

        ///// <summary>
        ///// ����һ������
        ///// </summary>
        //public int Update(SqlTransaction sqltran, Entities.OtherTaskWorkOrderMapping model)
        //{
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Update(sqltran, model);
        //}

        //#endregion

        //#region Delete
        ///// <summary>
        ///// ɾ��һ������
        ///// </summary>
        //public int Delete(int RecID)
        //{
			
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Delete(RecID);
        //}

        ///// <summary>
        ///// ɾ��һ������
        ///// </summary>
        //public int Delete(SqlTransaction sqltran, int RecID)
        //{
			
        //    return Dal.OtherTaskWorkOrderMapping.Instance.Delete(sqltran, RecID);
        //}

        //#endregion

	}
}

