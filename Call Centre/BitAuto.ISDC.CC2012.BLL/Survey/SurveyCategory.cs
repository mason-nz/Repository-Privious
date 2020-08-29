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
    /// ҵ���߼���SurveyCategory ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyCategory
    {
        #region Instance
        public static readonly SurveyCategory Instance = new SurveyCategory();
        #endregion

        #region Contructor
        protected SurveyCategory()
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
        public DataTable GetSurveyCategory(QuerySurveyCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SurveyCategory.Instance.GetSurveyCategory(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SurveyCategory.Instance.GetSurveyCategory(new QuerySurveyCategory(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyCategory GetSurveyCategory(int SCID)
        {
            return Dal.SurveyCategory.Instance.GetSurveyCategory(SCID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsBySCID(int SCID)
        {
            QuerySurveyCategory query = new QuerySurveyCategory();
            query.SCID = SCID;

            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyCategory(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyCategory model)
        {
            return Dal.SurveyCategory.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int SCID)
        {

            return Dal.SurveyCategory.Instance.Delete(SCID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SCID)
        {

            return Dal.SurveyCategory.Instance.Delete(sqltran, SCID);
        }

        #endregion

        public int GetSCIDByName(string categoryName)
        {
            return Dal.SurveyCategory.Instance.GetSCIDByName(categoryName);
        }

        /// <summary>
        /// ͨ��UserID��ȡ����ҵ����ID
        /// </summary>
        /// <returns>�ɹ��ҵ�����BGID�����򷵻�0</returns>
        public int GetSelfBGIDByUserID(int userID)
        {
            int bgid = 0;

            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userID);
            if (model != null && model.BGID != null)
            {
                bgid = (int)model.BGID;
            }

            return bgid;
        }

        /// <summary>
        /// ��ȡuserID����ҵ��������ġ��������ࡱID
        /// </summary>
        /// <returns>�ɹ��ҵ�����SCID�����򷵻�0</returns>
        public int GetSelfSCIDByUserID(int userID)
        {
            int scid = 0;

            int bgid = GetSelfBGIDByUserID(userID);
            if (bgid != 0)
            {
                Entities.QuerySurveyCategory query = new QuerySurveyCategory();
                query.BGID = bgid;
                query.Name = "��������";
                int count = 0;
                DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "", 1, 1, out count);
                if (dt != null && dt.Rows.Count == 1 && int.TryParse(dt.Rows[0]["SCID"].ToString(), out scid))
                {

                }
            }

            return scid;
        }

        /// <summary>
        ///  ���������Ƿ��ظ�
        /// </summary>
        public bool IsExistsCategoryName(string categoryName)
        {
            return Dal.SurveyCategory.Instance.IsExistsCategoryName(categoryName);
        }
    }
}

