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
    /// ҵ���߼���WorkOrderCategory ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:20 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderCategory
    {
        #region Instance
        public static readonly WorkOrderCategory Instance = new WorkOrderCategory();
        #endregion

        #region Contructor
        protected WorkOrderCategory()
        { }
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
        public DataTable GetWorkOrderCategory(QueryWorkOrderCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderCategory.Instance.GetWorkOrderCategory(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderCategory.Instance.GetWorkOrderCategory(new QueryWorkOrderCategory(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ���ݷ���ID�����ط�������
        /// </summary>
        /// <param name="CategoryID">����ID</param>
        /// <returns>��������</returns>
        public string GetCategoryNameByCategoryID(int CategoryID)
        {
            Entities.WorkOrderCategory model = GetWorkOrderCategory(CategoryID);
            if (model != null)
            {
                return model.Name;
            }
            return string.Empty;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.WorkOrderCategory GetWorkOrderCategory(int RecID)
        {

            return Dal.WorkOrderCategory.Instance.GetWorkOrderCategory(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryWorkOrderCategory query = new QueryWorkOrderCategory();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderCategory(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderCategory model)
        {
            return Dal.WorkOrderCategory.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.WorkOrderCategory.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.WorkOrderCategory.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public DataTable GetCategoryFullByCategoryID(string CategoryID)
        {
            return Dal.WorkOrderCategory.Instance.GetCategoryFullName(CategoryID);
        }

        public string GetCategoryFullName(string CategoryID)
        {

            if (!string.IsNullOrEmpty(CategoryID))
            {
                string fullName = string.Empty;

                DataTable dt = Dal.WorkOrderCategory.Instance.GetCategoryFullName(CategoryID);
                for (int k = dt.Rows.Count - 1; k >= 0; k--)
                {
                    fullName += dt.Rows[k]["Name"].ToString() + "-";
                }

                return fullName.TrimEnd('-');
            }
            else
            {
                return "";
            }


        }

        public string GetWorkCategoryJsonBySql(string where)
        {
            string jsonData = string.Empty;

            DataTable dt = BLL.Util.GetTableInfoBySql("select * from WorkOrderCategory where status=0 " + where + " order by OrderNum asc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                jsonData += i == 0 ? "[{ID:'" + dr["RecID"] + "',Name:'" + dr["Name"] + "'}" : ",{ID:'" + dr["RecID"] + "',Name:'" + dr["Name"] + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    jsonData += "]";
                }

            }

            return jsonData;

        }

    }
}

