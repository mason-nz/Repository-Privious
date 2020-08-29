using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ҵ���߼���ComSentence ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ComSentence
	{
		#region Instance
		public static readonly ComSentence Instance = new ComSentence();
		#endregion

		#region Contructor
		protected ComSentence()
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
		public DataTable GetComSentence(QueryComSentence query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ComSentence.Instance.GetComSentence(query,order,currentPage,pageSize,out totalCount);
		}

        public DataTable GetComSentenceList(QueryComSentence query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ComSentence.Instance.GetComSentenceList(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetAllLableWithCM(int bgid)
	    {
            return Dal.ComSentence.Instance.GetAllLableWithCM(bgid);
	    }

	    /// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ComSentence.Instance.GetComSentence(new QueryComSentence(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ComSentence GetComSentence(int CSID)
		{
			
			return Dal.ComSentence.Instance.GetComSentence(CSID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByCSID(int CSID)
		{
			QueryComSentence query = new QueryComSentence();
			query.CSID = CSID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetComSentence(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ComSentence model)
		{
			return Dal.ComSentence.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int CSID)
		{
			
			return Dal.ComSentence.Instance.Delete(CSID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int CSID)
		{
			
			return Dal.ComSentence.Instance.Delete(sqltran, CSID);
		}

		#endregion

        #region ��ѯ��ǩ�Ƿ��ڳ������б�ʹ�ù�
        public bool LabelIsUsedInCS(int LTID)
        {
            Entities.QueryComSentence query = new QueryComSentence();
            query.LTID = LTID;

            int totoal;
            DataTable dt = GetComSentence(query, "", 1, 9999, out totoal);

            if (dt != null && dt.Rows.Count>0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ��ѯͬһ��ǩ�³������Ƿ����ظ�
        public bool IsRepeatLableCS(int LTID,string CSName)
        {
            Entities.QueryComSentence query = new QueryComSentence();
            query.LTID = LTID;
            query.Name = CSName;

            int totoal;
            DataTable dt = GetComSentence(query, "", 1, 9999, out totoal);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region ��ѯ�ñ�ǩ�Ƿ��ѱ�������ʹ��
        public bool IsLabelUsedInCS(int LTID)
        {
            Entities.QueryComSentence query = new QueryComSentence();
            query.LTID = LTID;

            int totoal;
            DataTable dt = GetComSentence(query, "", 1, 9999, out totoal);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        public bool MoveUpOrDown(Entities.QueryComSentence query, int sortnum, int type)
        {
            return Dal.ComSentence.Instance.MoveUpOrDown(query, sortnum, type);
        }
    }
}

