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
	/// ҵ���߼���LabelTable ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:04 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class LabelTable
	{
		#region Instance
		public static readonly LabelTable Instance = new LabelTable();
		#endregion

		#region Contructor
		protected LabelTable()
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
		public DataTable GetLabelTable(QueryLabelTable query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.LabelTable.Instance.GetLabelTable(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
            //����Ȩ�ޣ���������
			int totalCount=0;
            string areastr = BLL.BaseData.Instance.GetAgentRegionByUserID(BLL.Util.GetLoginUserID().ToString());
            int areaint;
            if (int.TryParse(areastr, out areaint))
            {
                ;
            }
            else
            {
                areaint = -1;
            }
            return Dal.LabelTable.Instance.GetLabelTable(new QueryLabelTable() { AreaType = areaint }, string.Empty, 1, 1000000, out totalCount);
		}
        /// <summary>
        /// ��ȡȫ����ǩ �� ��ʶ�Ƿ�������ǰ��
        /// </summary>
        /// <param name="bgid"></param>
        /// <returns></returns>
        public DataTable GetLabelTableByBGID(int bgid,int region)
        {
            return Dal.LabelTable.Instance.GetLabelTableByBGID(bgid, region);
        }
		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.LabelTable GetLabelTable(int LTID)
		{
			
			return Dal.LabelTable.Instance.GetLabelTable(LTID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByLTID(int LTID)
		{
			QueryLabelTable query = new QueryLabelTable();
			query.LTID = LTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetLabelTable(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.LabelTable model)
		{
			return Dal.LabelTable.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int LTID)
		{
			
			return Dal.LabelTable.Instance.Delete(LTID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int LTID)
		{
			
			return Dal.LabelTable.Instance.Delete(sqltran, LTID);
		}

		#endregion

        /// �����ƶ�����
        /// <summary>
        /// �����ƶ�����
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1��-1��</param>
        /// <returns></returns>
        public bool MoveUpOrDown(int recid, int sortnum, int type)
        {
            return Dal.LabelTable.Instance.MoveUpOrDown(recid, sortnum, type);
        }
	}
}

