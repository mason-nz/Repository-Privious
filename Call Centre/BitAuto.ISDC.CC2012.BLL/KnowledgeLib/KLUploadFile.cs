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
	/// ҵ���߼���KLUploadFile ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:09 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class KLUploadFile
	{
		#region Instance
		public static readonly KLUploadFile Instance = new KLUploadFile();
		#endregion

		#region Contructor
		protected KLUploadFile()
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
		public DataTable GetKLUploadFile(QueryKLUploadFile query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.KLUploadFile.Instance.GetKLUploadFile(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.KLUploadFile.Instance.GetKLUploadFile(new QueryKLUploadFile(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.KLUploadFile GetKLUploadFile(long RecID)
		{
			
			return Dal.KLUploadFile.Instance.GetKLUploadFile(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QueryKLUploadFile query = new QueryKLUploadFile();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLUploadFile(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.KLUploadFile model)
		{
			return Dal.KLUploadFile.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.KLUploadFile model)
		{
			return Dal.KLUploadFile.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.KLUploadFile model)
		{
			return Dal.KLUploadFile.Instance.Update(model);
		}
        public void AppendClickCountByFilePath(string path)
        {
            Entities.QueryKLUploadFile query = new QueryKLUploadFile();
            query.FilePath = path.Replace(@"\KnowledgeLib", "").Replace("/", @"\");
            int totalCount=0;
            DataTable dt = BLL.KLUploadFile.Instance.GetKLUploadFile(query, "", 1, 1, out totalCount);
            if (totalCount > 0)
            {
                Entities.KLUploadFile info = BLL.KLUploadFile.Instance.GetKLUploadFile(int.Parse(dt.Rows[0]["RecID"].ToString()));
                if (info != null)
                {
                    info.ClickCount = info.ClickCount + 1;
                    BLL.KLUploadFile.Instance.Update(info);
                }
            }
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLUploadFile model)
		{
			return Dal.KLUploadFile.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.KLUploadFile.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long RecID)
		{ 
			return Dal.KLUploadFile.Instance.Delete(sqltran, RecID);
		}

        public int DeleteByKLID(SqlTransaction sqltran, long KLID)
        {
            return Dal.KLUploadFile.Instance.DeleteByKLID(sqltran, KLID);
        }

		#endregion

	}
}

