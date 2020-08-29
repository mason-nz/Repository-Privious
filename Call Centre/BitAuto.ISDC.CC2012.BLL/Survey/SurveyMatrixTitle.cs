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
	/// ҵ���߼���SurveyMatrixTitle ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:18 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class SurveyMatrixTitle
	{
		#region Instance
		public static readonly SurveyMatrixTitle Instance = new SurveyMatrixTitle();
		#endregion

		#region Contructor
		protected SurveyMatrixTitle()
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
		public DataTable GetSurveyMatrixTitle(QuerySurveyMatrixTitle query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// ��ȡĳ������������������ĸ���
        /// </summary>
        /// <param name="sqid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetSurveyMatrixTitleCount(int sqid, int type)
        {
            return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(sqid, type);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(new QuerySurveyMatrixTitle(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

        /// <summary>
        /// ͳ�Ƶ�����
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatOptionForMatrixRadio(int SMTID,int SPIID)
        {
            return Dal.SurveyMatrixTitle.Instance.StatOptionForMatrixRadio(SMTID, SPIID);
        }

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.SurveyMatrixTitle GetSurveyMatrixTitle(int SMTID)
		{
			
			return Dal.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(SMTID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsBySMTID(int SMTID)
		{
			QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
			query.SMTID = SMTID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyMatrixTitle(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyMatrixTitle model)
		{
			return Dal.SurveyMatrixTitle.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int SMTID)
		{
			
			return Dal.SurveyMatrixTitle.Instance.Delete(SMTID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int SMTID)
		{
			
			return Dal.SurveyMatrixTitle.Instance.Delete(sqltran, SMTID);
		}

		#endregion




        public List<Entities.SurveyMatrixTitle> GetMatrixTitleList(int siid)
        {
            return Dal.SurveyMatrixTitle.Instance.GetMatrixTitleList(siid);
        }

        /// <summary>
        /// �����ʾ�IDȡ�ʾ������о�������
        /// </summary>
        /// <param name="SIID"></param>
        /// <returns></returns>
        public DataTable GetAllMatrixDataTableBySIID(int SIID)
        {
            DataTable dt = null;
            int RowCount = 0;
            QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            query.SIID = SIID;
            query.Status = 0;
            dt = GetSurveyMatrixTitle(query, "", 1, 100000, out RowCount);
            return dt;
        }

        /// <summary>
        /// ȡ��������
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable GetMatrixDataTable(DataTable MatrixDataTable, int SQID, int Type)
        {
            DataTable dt = null;
            //int RowCount = 0;
            //QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            //query.SQID = SQID;
            //query.Type = Type;
            //query.Status = 0;
            //dt = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query, "", 1, 100000, out RowCount);
            if (MatrixDataTable != null)
            {
                dt = MatrixDataTable.Clone();
                DataView dv = MatrixDataTable.DefaultView;
                dv.RowFilter = "SQID=" + SQID + " and Type=" + Type;
                if (dv != null && dv.Count > 0)
                {
                    for (int i = 0; i < dv.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["SMTID"] = dv[i]["SMTID"];
                        row["SIID"] = dv[i]["SIID"];
                        row["SQID"] = dv[i]["SQID"];
                        row["TitleName"] = dv[i]["TitleName"];
                        row["Status"] = dv[i]["Status"];
                        row["Type"] = dv[i]["Type"];
                        row["CreateTime"] = dv[i]["CreateTime"];
                        row["CreateUserID"] = dv[i]["CreateUserID"];
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }
    }
}

