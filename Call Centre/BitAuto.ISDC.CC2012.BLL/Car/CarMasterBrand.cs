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
	/// ҵ���߼���CarMasterBrand ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-12-11 03:57:10 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CarMasterBrand
	{
		#region Instance
		public static readonly CarMasterBrand Instance = new CarMasterBrand();
		#endregion

		#region Contructor
		protected CarMasterBrand()
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
        public DataTable GetCarMasterBrand(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CarMasterBrand.Instance.GetCarMasterBrand(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CarMasterBrand.Instance.GetCarMasterBrand(string.Empty, string.Empty, 1, 1000000, out totalCount);
        }

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CarMasterBrand GetCarMasterBrand(int MasterBrandID)
		{
			
			return Dal.CarMasterBrand.Instance.GetCarMasterBrand(MasterBrandID);
		}

		#endregion

        //#region IsExists
        ///// <summary>
        ///// �Ƿ���ڸü�¼
        ///// </summary>
        //public bool IsExistsByMasterBrandID(int MasterBrandID)
        //{
        //    QueryCarMasterBrand query = new QueryCarMasterBrand();
        //    query.MasterBrandID = MasterBrandID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetCarMasterBrand(query, string.Empty, 1, 1, out count);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //#endregion

		#region Insert
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(Entities.CarMasterBrand model)
		{
			Dal.CarMasterBrand.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.CarMasterBrand model)
		{
			Dal.CarMasterBrand.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.CarMasterBrand model)
		{
			return Dal.CarMasterBrand.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.CarMasterBrand model)
		{
			return Dal.CarMasterBrand.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int MasterBrandID)
		{
			
			return Dal.CarMasterBrand.Instance.Delete(MasterBrandID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int MasterBrandID)
		{
			
			return Dal.CarMasterBrand.Instance.Delete(sqltran, MasterBrandID);
		}

		#endregion


        internal void DeleteTable()
        {
            Dal.CarMasterBrand.Instance.DeleteTable();
        }

        internal DataTable GetAllListFromCrm2009()
        {
            return Dal.CarMasterBrand.Instance.GetAllListFromCrm2009();
        }
    }
}

